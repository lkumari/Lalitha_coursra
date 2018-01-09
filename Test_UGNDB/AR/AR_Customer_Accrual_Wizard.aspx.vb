' ************************************************************************************************
'
' Name:		AR_Customer_Accrual_Wizard.aspx
' Purpose:	This Code Behind is for the AR Customer Accrual Wizard
'
' Customer Accrual 
'   i.	    Created by Sales or Accounting Manager
'   ii.	    Require at least one SOLDTO and ONLY ONE UGN FACILITY
'   iii.    Multiple CABBV AND COMPNY allowed.
'   iv.     NO PARTNO OR PRICE CODE SELECTION ALLOWED
'   v.	    Selection criteria based on Shipping History
'   vi.	    Effective Date must be prior to current date
'   vii.    Only allowed for Mass Production And Service
'   viii.	Extra field showing what is actually credited to the customer, “Final Deduction Amount”
'   ix.	    Date field when the customer was credited
'   x.	    Approval needed by Billing(Accounting Manager), Sales, Possibly VP of Sales, Possibly CFO, and Possibly CEO (depending upon deduction amount)
'
' Date		Author	    
' 04/05/2011   Roderick Carlson
' 05/14/2012   Roderick Carlson - get all parts, show checked and let user UNcheck certain ones
' 09/21/2012   Roderick Carlson - all button to select all parts

Partial Class AR_Customer_Accrual_Wizard
    Inherits System.Web.UI.Page

    Private Sub RememberOldPARTNOValues()

        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()

            Dim index As String = ""

            For Each row As GridViewRow In gvPartNo.Rows

                index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
                    SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedPARTNOList.Contains(index) Then
                        SelectedPARTNOList.Add(index)
                    End If
                Else
                    SelectedPARTNOList.Remove(index)
                End If
            Next

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                Session("CHECKED_PARTNO_ITEMS") = SelectedPARTNOList

                'gvPriceCode.Visible = False
                'btnFilterPriceCode.Visible = True
                'btnClearFilterPriceCode.Visible = True

                'gvSOLDTO.Visible = False
                'btnFilterSOLDTO.Visible = False
                'btnClearFilterSOLDTO.Visible = False

                'gvCABBV.Visible = False
                'btnFilterCABBV.Visible = False
                'btnClearFilterCABBV.Visible = False

            Else
                Session("CHECKED_PARTNO_ITEMS") = Nothing

                'btnUpdate.Visible = False
                'rbUpdateType.Visible = False

                'btnFilterPriceCode.Visible = False
                'btnClearFilterPriceCode.Visible = False
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

    Private Sub RePopulatePARTNOValues()

        Try
            Dim SelectedPARTNOList As Collections.ArrayList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

            If SelectedPARTNOList IsNot Nothing AndAlso SelectedPARTNOList.Count > 0 Then
                For Each row As GridViewRow In gvPartNo.Rows

                    Dim index As String = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedPARTNOList.Contains(index) Then
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

    Private Sub RememberOldSOLDTOValues()

        Dim SelectedSOLDTOList As New Collections.ArrayList()

        Dim index As String = ""
        Dim previous_index As String = ""

        For Each row As GridViewRow In gvSOLDTO.Rows

            index = gvSOLDTO.DataKeys(row.RowIndex).Value.ToString.Trim

            Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

            ' Check in the Session
            If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing Then
                SelectedSOLDTOList = DirectCast(Session("CHECKED_SOLDTO_ITEMS"), Collections.ArrayList)
            End If

            If result Then
                If Not SelectedSOLDTOList.Contains(index) Then
                    SelectedSOLDTOList.Add(index)
                End If
            Else
                'some SOLDTOs are NOT UNIQUE TO CUSTOMER NAMES
                If previous_index <> index Then
                    SelectedSOLDTOList.Remove(index)
                End If
            End If

            previous_index = index
        Next

        Session("CHECKED_PRCCDE_ITEMS") = Nothing
        Session("CHECKED_CABBV_ITEMS") = Nothing
        Session("CHECKED_PARTNO_ITEMS") = Nothing

        gvPriceCode.Visible = False
        gvCABBV.Visible = False
        gvPartNo.Visible = False
        btnSelectParts.Visible = gvPartNo.Visible
        btnUnselectParts.Visible = gvPartNo.Visible

        If SelectedSOLDTOList IsNot Nothing AndAlso SelectedSOLDTOList.Count > 0 Then
            Session("CHECKED_SOLDTO_ITEMS") = SelectedSOLDTOList

            btnFilterPriceCode.Visible = True
            btnClearPriceCode.Visible = True
            'btnFilterCABBV.Visible = True
            'btnClearCABBV.Visible = True

            btnUpdate.Visible = ViewState("isAdmin")
            rbUpdateType.Visible = ViewState("isAdmin")
        Else
            Session("CHECKED_SOLDTO_ITEMS") = Nothing

            btnFilterPriceCode.Visible = False
            btnClearPriceCode.Visible = False
            'btnFilterCABBV.Visible = False
            'btnClearCABBV.Visible = False

            btnUpdate.Visible = False
            rbUpdateType.Visible = False
        End If

    End Sub

    Private Sub RePopulateCABBVValues()

        Try
            Dim SelectedCABBVList As Collections.ArrayList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)

            If SelectedCABBVList IsNot Nothing AndAlso SelectedCABBVList.Count > 0 Then
                For Each row As GridViewRow In gvCABBV.Rows

                    Dim index As Integer = CInt(gvCABBV.DataKeys(row.RowIndex).Value)

                    If SelectedCABBVList.Contains(index) Then
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

    Private Sub RePopulateSOLDTOValues()

        Try
            Dim SelectedSOLDTOList As Collections.ArrayList = DirectCast(Session("CHECKED_SOLDTO_ITEMS"), Collections.ArrayList)

            If SelectedSOLDTOList IsNot Nothing AndAlso SelectedSOLDTOList.Count > 0 Then
                For Each row As GridViewRow In gvSOLDTO.Rows

                    'Dim index As Integer = CInt(gvSOLDTO.DataKeys(row.RowIndex).Value)
                    Dim index As String = gvSOLDTO.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedSOLDTOList.Contains(index) Then
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

    Protected Sub cbSelectSOLDTO_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ClearMessages()

        Try

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            If cbSelectedCheckbox.Checked = True Then
                lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."
            Else
                lblMessage.Text = cbSelectedCheckbox.ToolTip & " was UNchecked."
            End If

            RememberOldSOLDTOValues()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RememberOldCABBVValues()

        Try
            Dim SelectedCABBVList As New Collections.ArrayList()

            'Dim index As Integer = -1
            Dim index As String = ""

            For Each row As GridViewRow In gvCABBV.Rows

                'index = CInt(gvCABBV.DataKeys(row.RowIndex).Value)
                index = gvCABBV.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_CABBV_ITEMS") IsNot Nothing Then
                    SelectedCABBVList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedCABBVList.Contains(index) Then
                        SelectedCABBVList.Add(index)
                    End If
                Else
                    SelectedCABBVList.Remove(index)
                End If
            Next

            Session("CHECKED_CABBV_ITEMS") = Nothing

            If SelectedCABBVList IsNot Nothing AndAlso SelectedCABBVList.Count > 0 Then
                Session("CHECKED_CABBV_ITEMS") = SelectedCABBVList
            Else
                Session("CHECKED_CABBV_ITEMS") = Nothing
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

    Private Sub RememberOldPriceCodeValues()

        Try
            Dim SelectedPriceCodeList As New Collections.ArrayList()

            'Dim index As Integer = -1
            Dim index As String = ""

            For Each row As GridViewRow In gvPriceCode.Rows

                'index = CInt(gvPriceCode.DataKeys(row.RowIndex).Value)
                index = gvPriceCode.DataKeys(row.RowIndex).Value.ToString.Trim

                Dim result As Boolean = DirectCast(row.FindControl("cbSelect"), CheckBox).Checked

                ' Check in the Session
                If Session("CHECKED_PRCCDE_ITEMS") IsNot Nothing Then
                    SelectedPriceCodeList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)
                End If

                If result Then
                    If Not SelectedPriceCodeList.Contains(index) Then
                        SelectedPriceCodeList.Add(index)
                    End If
                Else
                    SelectedPriceCodeList.Remove(index)
                End If
            Next

            If SelectedPriceCodeList IsNot Nothing AndAlso SelectedPriceCodeList.Count > 0 Then
                Session("CHECKED_PRCCDE_ITEMS") = SelectedPriceCodeList
            Else
                Session("CHECKED_PRCCDE_ITEMS") = Nothing
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

    Protected Sub cbSelectPriceCode_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."

            RememberOldPriceCodeValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RePopulatePriceCodeValues()

        Try
            Dim SelectedPriceCodeList As Collections.ArrayList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

            If SelectedPriceCodeList IsNot Nothing AndAlso SelectedPriceCodeList.Count > 0 Then
                For Each row As GridViewRow In gvPriceCode.Rows

                    Dim index As String = gvPriceCode.DataKeys(row.RowIndex).Value.ToString.Trim

                    If SelectedPriceCodeList.Contains(index) Then
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

    Protected Sub cbSelectCABBV_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ClearMessages()

        Try

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            'lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."
            If cbSelectedCheckbox.Checked = True Then
                lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."
            Else
                lblMessage.Text = cbSelectedCheckbox.ToolTip & " was UNchecked."
            End If

            RememberOldCABBVValues()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindSoldTo(ByVal UGNFacility As String)

        Try
            Dim ds As DataSet

            Dim iRowCounter As Integer = 0
            Dim liSoldTo As ListItem

            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            If UGNFacility <> "" Then
                Session("COMPNYWhereClause") = " AND COMPNY IN ('" & UGNFacility & "') "
            Else
                Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
            End If

            Session("SOLDTOWhereClause") = Session("COMPNYWhereClause") & Session("CABBVWhereClause") & " AND PRCCDE IN ('A','S') " 'AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"

            If ViewState("CustApprvEndDate") <> "" Then
                Session("SOLDTOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
            End If

            ddSoldTo.Items.Clear()

            ds = ARGroupModule.GetARShippingHistoryDynamically(0, "SOLDTO", "SOLDTO,CUSNM", Session("SOLDTOWhereClause"))
            If commonFunctions.CheckDataSet(ds) = True Then             
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1

                    If ds.Tables(0).Rows(iRowCounter).Item("CUSNM").ToString.Trim <> "" And ds.Tables(0).Rows(iRowCounter).Item("SOLDTO").ToString.Trim <> "" Then
                        liSoldTo = New ListItem
                        liSoldTo.Text = ds.Tables(0).Rows(iRowCounter).Item("SOLDTO").ToString.Trim & " | " & ds.Tables(0).Rows(iRowCounter).Item("CUSNM").ToString.Trim
                        liSoldTo.Value = ds.Tables(0).Rows(iRowCounter).Item("SOLDTO").ToString.Trim
                        ddSoldTo.Items.Add(liSoldTo)
                    End If

                Next
                ddSoldTo.Items.Insert(0, "")
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
            Dim ds As DataSet

            'ds = commonFunctions.GetSoldTo()
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddSoldTo.DataSource = ds
            '    ddSoldTo.DataTextField = ds.Tables(0).Columns("SoldTo").ColumnName
            '    ddSoldTo.DataValueField = ds.Tables(0).Columns("SoldTo").ColumnName
            '    ddSoldTo.DataBind()
            '    ddSoldTo.Items.Insert(0, "")
            'End If

            'BindSoldTo("")

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.Trim
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.Trim
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
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

    Protected Sub ClearMessages()

        Try

            lblMessage.Text = ""
            lblMessageBottom.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "AR Customer Accrual Wizard"

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
            ViewState("CurrentTeamMemberID") = 0
            ViewState("isAdmin") = False
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    ' ''test developer as another team member
                    'If iTeamMemberID = 530 Then
                    '    'mike echevarria
                    '    'iTeamMemberID = 246

                    '    'Jeffrey.Kist 
                    '    iTeamMemberID = 718
                    'End If

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

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49) '52

                    If commonFunctions.CheckDataSet(dsRoleForm) = True Then

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

    Protected Sub BindData()

        Try

            Dim ds As DataSet

            ds = ARGroupModule.GetAREvent(ViewState("AREID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("CustApprvEffDate") = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
                lblShipDateFrom.Text = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
                ViewState("CustApprvEndDate") = ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString

                If ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString.Trim <> "" Then
                    lblShipDateTo.Text = ds.Tables(0).Rows(0).Item("CustApprvEndDate").ToString
                Else
                    lblShipDateTo.Text = "None"
                End If
            End If

            ds = ARGroupModule.GetAREventDetail(ViewState("AREID"))
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("COMPNY").ToString.Trim
                BindSoldTo(ddUGNFacility.SelectedValue)
                ddSoldTo.SelectedValue = ds.Tables(0).Rows(0).Item("SOLDTO").ToString.Trim
            Else
                BindSoldTo("")
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

    Private Sub DisableControls()

        Try
            btnFilterPriceCode.Visible = False
            btnClearPriceCode.Visible = False

            btnFilterCABBV.Visible = False
            btnClearCABBV.Visible = False

            btnFilterPartNo.Visible = False
            btnClearFilterPartNo.Visible = False

            btnUpdate.Visible = False
            rbUpdateType.Visible = False

            gvSOLDTO.Visible = False
            gvCABBV.Visible = False
            gvPriceCode.Visible = False
            gvPartNo.Visible = False
            btnSelectParts.Visible = False
            btnUnselectParts.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try
            DisableControls()

            If ViewState("SubscriptionID") = 9 Or ViewState("SubscriptionID") = 23 Or ViewState("isAdmin") = True Then

                btnFilterSoldTo.Visible = ViewState("isAdmin")

                'btnUpdate.Visible = ViewState("isAdmin")
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
      
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then
                Session("CHECKED_SOLDTO_ITEMS") = Nothing

                Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "

                CheckRights()

                BindCriteria()

                ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                lblAREID.Text = ViewState("AREID")

                Session("SOLDTOWhereClause") = Nothing

                BindData()

                Session("SOLDTOWhereClause") &= " AND PRCCDE IN ('A','S') AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"

                If ViewState("CustApprvEndDate") <> "" Then
                    Session("SOLDTOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
                End If

                Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "

                Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

                Session("PARTNOWhereClause") = " AND PARTNO ='NA PART' "

                EnableControls()

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'> Event Detail </a> > Customer Accrual Wizard "

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

    Protected Sub gvSOLDTO_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSOLDTO.PageIndexChanging

        Try

            RememberOldSOLDTOValues()
            gvSOLDTO.PageIndex = e.NewPageIndex
            gvSOLDTO.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvSOLDTO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSOLDTO.RowDataBound

        Try
            RePopulateSOLDTOValues()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub btnBackToAREvent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToAREvent.Click

        Try

            Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnFilterUGNFacility_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterUGNFacility.Click

    '    Try
    '        ClearMessages()

    '        'RememberOldPriceCodeValues()

    '        Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
    '        Session("SOLDTOWhereClause") = " AND SOLDTO > 0 "
    '        Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

    '        Dim SelectedPriceCodeList As New Collections.ArrayList()

    '        Dim iRowCounter As Integer = 0
    '        Dim strTempWhereClause As String = "AND PRCCDE IN  ("
    '        Dim strPriceCodeList As String = ""

    '        If Session("CHECKED_PRCCDE_ITEMS") IsNot Nothing Then
    '            SelectedPriceCodeList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

    '            For iRowCounter = 0 To SelectedPriceCodeList.Count - 1
    '                If strPriceCodeList <> "" Then
    '                    strPriceCodeList &= ","
    '                End If

    '                strPriceCodeList &= "'" & SelectedPriceCodeList.Item(iRowCounter).ToString.Trim & "'"
    '            Next
    '        End If

    '        If strPriceCodeList <> "" Then
    '            strTempWhereClause &= strPriceCodeList
    '        End If

    '        If strPriceCodeList <> "" Then
    '            Session("PRCCDEWhereClause") = Session("PARTNOWhereClause") & strTempWhereClause & ")"
    '            Session("COMPNYWhereClause") = Session("PRCCDEWhereClause") & " AND COMPNY IN ('UN','UP','UR','US') "
    '            Session("SOLDTOWhereClause") = Session("PRCCDEWhereClause") & " AND SOLDTO > 0 "
    '            Session("CABBVWhereClause") = Session("PRCCDEWhereClause") & " AND CABBV IS NOT NULL "

    '            'gvPriceCode.DataBind()
    '            gvUGNFacility.DataBind()

    '        Else
    '            lblMessage.Text = "Error: at least one part number must be selected"
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub gvUGNFacility_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUGNFacility.PageIndexChanging

    '    Try

    '        RememberOldUGNFacilityValues()
    '        gvUGNFacility.PageIndex = e.NewPageIndex
    '        gvUGNFacility.DataBind()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub gvUGNFacility_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUGNFacility.RowDataBound

    '    Try

    '        RePopulateUGNFacilityValues()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnClearSoldTO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearSoldTo.Click


        Try
            ClearMessages()

            DisableControls()

            Dim strUGNFacility As String = ""

            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            If ddUGNFacility.SelectedIndex >= 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue

                'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
                Session("COMPNYWhereClause") = " AND COMPNY = '" & strUGNFacility & "'"

                Session("SOLDTOWhereClause") = " AND PRCCDE IN ('A','S') AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"

                Session("SOLDTOWhereClause") = Session("COMPNYWhereClause") & Session("CABBVWhereClause") & " AND PRCCDE IN ('A','S') AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"

                If ViewState("CustApprvEndDate") <> "" Then
                    Session("SOLDTOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
                End If

                Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "

                Session("CHECKED_SOLDTO_ITEMS") = Nothing
                Session("CHECKED_PRCCDE_ITEMS") = Nothing
                Session("CHECKED_CABBV_ITEMS") = Nothing

                'Session("CHECKED_COMPNY_ITEMS") = Nothing

                RePopulateSOLDTOValues()

                ddSoldTo.SelectedIndex = -1
                gvSOLDTO.Visible = True
                gvSOLDTO.DataBind()
            Else
                lblMessage.Text = "Please select a UGN Facility."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnFilterCABBV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterCABBV.Click

        Try
            'filter cabbv list based on selected sold to(s) 
            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            ClearMessages()

            'Dim SelectedSOLDTOList As New Collections.ArrayList()
            'Dim iSOLDTORowCounter As Integer = 0
            'Dim iSOLDTOTotalCount As Integer = 0

            'Dim strTempSOLDTOWhereClause As String = " AND SOLDTO IN  ("
            'Dim strSOLDTOList As String = ""

            If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing Then
                ''get all selected SOLDTOs
                'SelectedSOLDTOList = DirectCast(Session("CHECKED_SOLDTO_ITEMS"), Collections.ArrayList)

                'If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing And SelectedSOLDTOList IsNot Nothing Then
                '    iSOLDTOTotalCount = SelectedSOLDTOList.Count
                'End If

                'If iSOLDTOTotalCount > 0 Then
                '    For iSOLDTORowCounter = 0 To iSOLDTOTotalCount - 1
                '        If strSOLDTOList <> "" Then
                '            strSOLDTOList &= ","
                '        End If

                '        strSOLDTOList &= SelectedSOLDTOList(iSOLDTORowCounter).ToString.Trim
                '    Next
                'End If

                'If strSOLDTOList <> "" Then
                '    strTempSOLDTOWhereClause &= strSOLDTOList & ")"
                'End If

                Session("CHECKED_CABBV_ITEMS") = Nothing
                Session("CHECKED_PARTNO_ITEMS") = Nothing

                If Session("SOLDTOWhereClause") IsNot Nothing Then
                    If Session("SOLDTOWhereClause").ToString <> "" Then                      
                        'Session("CABBVWhereClause") = Session("SOLDTOWhereClause") & strTempSOLDTOWhereClause & Session("CABBVWhereClause")
                        Session("CABBVWhereClause") = Session("PRCCDEWhereClause") & Session("CABBVWhereClause")
                        Session("PARTNOWhereClause") = Session("SOLDTOWhereClause") & Session("PRCCDEWhereClause")
                    End If
                End If

                btnClearCABBV.Visible = True
                btnFilterCABBV.Visible = True

                gvCABBV.Visible = True
                gvCABBV.DataBind()

                RememberOldCABBVValues()

                btnFilterPartNo.Visible = True
                btnClearFilterPartNo.Visible = True

                
                gvPartNo.Visible = True
                btnSelectParts.Visible = gvPartNo.Visible
                btnUnselectParts.Visible = gvPartNo.Visible

                gvPartNo.DataBind()

            Else
                lblMessage.Text = "Please check at least one SOLDTO item in the list"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvCABBV_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCABBV.PageIndexChanging

        Try

            RememberOldCABBVValues()
            gvCABBV.PageIndex = e.NewPageIndex
            gvCABBV.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub gvCABBV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCABBV.RowDataBound

        Try
            RePopulateCABBVValues()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnFilterUGNFacility_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterUGNFacility.Click

    '    Try
    '        'filter facility based on sold to(s) and cabbv(s)
    '        Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "

    '        DisableControls()

    '        ClearMessages()

    '        Dim SelectedCABBVList As New Collections.ArrayList()
    '        Dim iCABBVRowCounter As Integer = 0
    '        Dim iCABBVTotalCount As Integer = 0

    '        Dim strTempCABBVWhereClause As String = " AND CABBV IN  ("
    '        Dim strCABBVList As String = ""

    '        If Session("CHECKED_CABBV_ITEMS") IsNot Nothing Then
    '            'get all selected CABBVs
    '            SelectedCABBVList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)

    '            If Session("CHECKED_CABBV_ITEMS") IsNot Nothing And SelectedCABBVList IsNot Nothing Then
    '                iCABBVTotalCount = SelectedCABBVList.Count
    '            End If

    '            If iCABBVTotalCount > 0 Then
    '                For iCABBVRowCounter = 0 To iCABBVTotalCount - 1
    '                    If strCABBVList <> "" Then
    '                        strCABBVList &= ","
    '                    End If

    '                    strCABBVList &= "'" & SelectedCABBVList(iCABBVRowCounter).ToString.Trim & "'"
    '                Next
    '            End If

    '            If strCABBVList <> "" Then
    '                strTempCABBVWhereClause &= strCABBVList & ")"

    '                'Session("CABBVWhereClause") = Session("CABBVWhereClause") & strTempCABBVWhereClause
    '            End If

    '            Session("CHECKED_COMPNY_ITEMS") = Nothing

    '            If Session("CABBVWhereClause") IsNot Nothing Then
    '                If Session("CABBVWhereClause").ToString <> "" Then
    '                    'Session("COMPNYWhereClause") = Session("CABBVWhereClause") & Session("COMPNYWhereClause")
    '                    Session("COMPNYWhereClause") = Session("CABBVWhereClause") & strTempCABBVWhereClause & Session("COMPNYWhereClause")
    '                End If
    '            End If

    '            btnClearCABBV.Visible = True
    '            btnFilterCABBV.Visible = True

    '            gvCABBV.Visible = True

    '            btnClearUGNFacility.Visible = True
    '            btnFilterUGNFacility.Visible = True

    '            gvUGNFacility.Visible = True
    '            gvUGNFacility.DataBind()

    '            btnUpdate.Visible = ViewState("isAdmin")
    '            rbUpdateType.Visible = ViewState("isAdmin")
    '        Else
    '            lblMessage.Text = "Please check at least one CABBV item in the list"
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageBottom.Text = lblMessage.Text
    'End Sub

    Protected Sub btnClearCABBV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearCABBV.Click

        Try

            'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "
            Session("PARTNOWhereClause") = " AND PARTNO ='NA PART' "

            ClearMessages()

            Session("CHECKED_CABBV_ITEMS") = Nothing
            Session("CHECKED_PARTNO_ITEMS") = Nothing

            gvCABBV.Visible = False
            gvPartNo.Visible = False
            btnSelectParts.Visible = False
            btnUnselectParts.Visible = False

            btnFilterPartNo.Visible = False
            btnClearCABBV.Visible = False

            'Session("CHECKED_COMPNY_ITEMS") = Nothing

            'btnUpdate.Visible = ViewState("isAdmin")
            'rbUpdateType.Visible = ViewState("isAdmin")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    'Protected Sub btnClearUGNFacility_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearUGNFacility.Click

    '    Try

    '        Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "

    '        ClearMessages()

    '        Session("CHECKED_COMPNY_ITEMS") = Nothing

    '        btnUpdate.Visible = ViewState("isAdmin")
    '        rbUpdateType.Visible = ViewState("isAdmin")

    '        gvUGNFacility.Visible = False

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageBottom.Text = lblMessage.Text

    'End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim SelectedSOLDTOList As New Collections.ArrayList()
            Dim SelectedPriceCodeList As New Collections.ArrayList()
            Dim SelectedCABBVList As New Collections.ArrayList()
            Dim SelectedPARTNOList As New Collections.ArrayList()
            'Dim SelectedCOMPNYList As New Collections.ArrayList()

            Dim iSOLDTORowCounter As Integer = 0
            Dim iSOLDTOTotalCount As Integer = 0

            Dim iPriceCodeRowCounter As Integer = 0
            Dim iPriceCodeTotalCount As Integer = 0

            Dim iCABBVRowCounter As Integer = 0
            Dim iCABBVTotalCount As Integer = 0

            Dim iPARTNORowCounter As Integer = 0
            Dim iPARTNOTotalCount As Integer = 0

            'Dim iCOMPNYRowCounter As Integer = 0
            'Dim iCOMPNYTotalCount As Integer = 0

            Dim strSOLDTO As String = ""
            Dim strPriceCode As String = ""
            Dim strCABBV As String = ""
            Dim strPARTNO As String = ""
            Dim strUGNFacility As String = ""

            Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            Dim iApprovalStatus As Integer = 0
            Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0

            If ddUGNFacility.SelectedIndex >= 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue
            End If

            If strUGNFacility <> "" Then

                iSOLDTOTotalCount = 0
                SelectedSOLDTOList = DirectCast(Session("CHECKED_SOLDTO_ITEMS"), Collections.ArrayList)

                If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing And SelectedSOLDTOList IsNot Nothing Then
                    iSOLDTOTotalCount = SelectedSOLDTOList.Count
                End If

                If iSOLDTOTotalCount > 0 Then

                    ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "F")
                    If rbUpdateType.SelectedValue = "R" Then
                        'delete all AR Event Details
                        ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")
                    End If

                    'iterate through SOLDTOs
                    For iSOLDTORowCounter = 0 To SelectedSOLDTOList.Count - 1
                        strSOLDTO = SelectedSOLDTOList.Item(iSOLDTORowCounter).ToString.Trim

                        iPriceCodeTotalCount = 0
                        SelectedPriceCodeList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

                        If Session("CHECKED_PRCCDE_ITEMS") IsNot Nothing And SelectedPriceCodeList IsNot Nothing Then
                            iPriceCodeTotalCount = SelectedPriceCodeList.Count
                        End If

                        If iPriceCodeTotalCount > 0 Then
                            'iterate price codes
                            For iPriceCodeRowCounter = 0 To SelectedPriceCodeList.Count - 1
                                strPriceCode = SelectedPriceCodeList.Item(iPriceCodeRowCounter).ToString.Trim

                                If strPriceCode <> "" Then
                                    iCABBVTotalCount = 0
                                    SelectedCABBVList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)

                                    If Session("CHECKED_CABBV_ITEMS") IsNot Nothing And SelectedCABBVList IsNot Nothing Then
                                        iCABBVTotalCount = SelectedCABBVList.Count
                                    End If

                                    If iCABBVTotalCount > 0 Then

                                        'iterate through CABBVs
                                        For iCABBVRowCounter = 0 To SelectedCABBVList.Count - 1
                                            strCABBV = SelectedCABBVList(iCABBVRowCounter).ToString.Trim

                                            'ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strCABBV, strSOLDTO, "", strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "")

                                            'check to make sure selected combination exists in RELPRC_Accruals table
                                            'If commonFunctions.CheckDataSet(ds) = True Then
                                            'SOLDTO(s), and CABBV(s) were selected
                                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, "", "", "", strPriceCode, "", 0, 0, 0, False)
                                            iPARTNOTotalCount = 0
                                            SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                                            If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                                                iPARTNOTotalCount = SelectedPARTNOList.Count
                                            End If

                                            If iPARTNOTotalCount > 0 Then

                                                'iterate through PARTNOs
                                                For iPARTNORowCounter = 0 To SelectedPARTNOList.Count - 1
                                                    strPARTNO = SelectedPARTNOList(iPARTNORowCounter).ToString.Trim

                                                    ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strCABBV, strSOLDTO, strPARTNO, strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                                    'check to make sure selected combination exists in RELPRC_Accruals table
                                                    If commonFunctions.CheckDataSet(ds) = True Then
                                                        'SOLDTO(s), CABBV(s) and PARTNO(s) were selected
                                                        '(LREY) 01/08/2014
                                                        'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, strPARTNO, "", "", strPriceCode, "", 0, 0, 0, False, 0)
                                                        ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strPARTNO, "", "", strPriceCode, 0, 0, 0, False, 0)
                                                    End If
                                                Next 'next selected PARTNO
                                            Else
                                                'no PARTNO was selected but at least one cabbv was selected
                                                ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strCABBV, strSOLDTO, "", strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                                'check to make sure selected combination exists in RELPRC_Accruals table
                                                If commonFunctions.CheckDataSet(ds) = True Then
                                                    'SOLDTO(s), and CABBV(s) were selected
                                                    '(LREY) 01/08/2014
                                                    'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, "", "", "", strPriceCode, "", 0, 0, 0, False, 0)
                                                    ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, "", "", "", strPriceCode, 0, 0, 0, False, 0)
                                                End If
                                            End If 'If iPARTNOTotalCount > 0 Then

                                            'End If
                                        Next 'next selected CABBV
                                    Else
                                        'no cabbv was selected but at least one price code was selected
                                        ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, "", strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                        'check to make sure selected combination exists in RELPRC_Accruals table
                                        If commonFunctions.CheckDataSet(ds) = True Then
                                            'SOLDTO(s), CABBV(s), and Price Code(s) were selected
                                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, "", "", "", strPriceCode, "", 0, 0, 0, False)

                                            iPARTNOTotalCount = 0
                                            SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                                            If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                                                iPARTNOTotalCount = SelectedPARTNOList.Count
                                            End If

                                            If iPARTNOTotalCount > 0 Then

                                                'iterate through PARTNOs
                                                For iPARTNORowCounter = 0 To SelectedPARTNOList.Count - 1
                                                    strPARTNO = SelectedPARTNOList(iPARTNORowCounter).ToString.Trim

                                                    ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, strPARTNO, strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                                    'check to make sure selected combination exists in RELPRC_Accruals table
                                                    If commonFunctions.CheckDataSet(ds) = True Then
                                                        'SOLDTO(s), PARTNO(s), Price Code(s) were selected
                                                        '(LREY) 01/08/2014
                                                        'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, strPARTNO, "", "", strPriceCode, "", 0, 0, 0, False, 0)
                                                        ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strPARTNO, "", "", strPriceCode, 0, 0, 0, False, 0)
                                                    End If
                                                Next 'next selected PARTNO
                                            Else
                                                'no PARTNO was selected but at least one price code was selected
                                                ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, "", strPriceCode, ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                                'check to make sure selected combination exists in RELPRC_Accruals table
                                                If commonFunctions.CheckDataSet(ds) = True Then
                                                    'SOLDTO(s), and PARTNO(s) were selected
                                                    '(LREY) 01/08/2014
                                                    'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, "", "", "", strPriceCode, "", 0, 0, 0, False, 0)
                                                    ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", "", "", "", strPriceCode, 0, 0, 0, False, 0)
                                                End If
                                            End If 'If iPARTNOTotalCount > 0 Then
                                        End If
                                    End If 'If iCABBVTotalCount > 0 Then

                                End If 'If strPriceCodeNo <> "" Then
                            Next 'iPriceCodeRowCounter 
                        Else 'no price code selected
                            'check if cabbvs are selected without price codes

                            iCABBVTotalCount = 0
                            SelectedCABBVList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)

                            If Session("CHECKED_CABBV_ITEMS") IsNot Nothing And SelectedCABBVList IsNot Nothing Then
                                iCABBVTotalCount = SelectedCABBVList.Count
                            End If

                            If iCABBVTotalCount > 0 Then

                                'iterate through CABBVs
                                For iCABBVRowCounter = 0 To SelectedCABBVList.Count - 1
                                    strCABBV = SelectedCABBVList(iCABBVRowCounter).ToString.Trim

                                    ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strCABBV, strSOLDTO, "", "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                    'check to make sure selected combination exists in RELPRC_Accruals table
                                    If commonFunctions.CheckDataSet(ds) = True Then
                                        'SOLDTO(s), and CABBV(s) were selected
                                        'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, "", "", "", "", "", 0, 0, 0, False)

                                        iPARTNOTotalCount = 0
                                        SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                                        If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                                            iPARTNOTotalCount = SelectedPARTNOList.Count
                                        End If

                                        If iPARTNOTotalCount > 0 Then

                                            'iterate through PARTNOs
                                            For iPARTNORowCounter = 0 To SelectedPARTNOList.Count - 1
                                                strPARTNO = SelectedPARTNOList(iPARTNORowCounter).ToString.Trim

                                                ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strPARTNO, strSOLDTO, "", "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                                'check to make sure selected combination exists in RELPRC_Accruals table
                                                If commonFunctions.CheckDataSet(ds) = True Then
                                                    'SOLDTO(s) and PARTNO(s) were selected
                                                    '(LREY) 01/08/2014
                                                    'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, strPARTNO, "", "", "", "", 0, 0, 0, False, 0)
                                                    ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strPARTNO, "", "", "", 0, 0, 0, False, 0)
                                                End If
                                            Next 'next selected PARTNO
                                        Else
                                            'no PARTNO was selected but at least one soldto and cabbv was selected
                                            ds = ARGroupModule.GetARShippingHistory(strUGNFacility, strCABBV, strSOLDTO, "", "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                            'check to make sure selected combination exists in RELPRC_Accruals table
                                            If commonFunctions.CheckDataSet(ds) = True Then
                                                'SOLDTO(s) and CABBV(s) were selected
                                                '(LREY) 01/08/2014
                                                'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, strSOLDTO, "", "", "", "", "", 0, 0, 0, False, 0)
                                                ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, strCABBV, "", "", "", "", 0, 0, 0, False, 0)
                                            End If
                                        End If 'If iPARTNOTotalCount > 0 Then
                                    End If
                                Next 'next selected CABBV

                            Else 'if no cabbv selected, nor was a price code selected, only a SoldTo and maybe part was selected
                                ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, "", "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                'check to make sure selected combination exists in RELPRC_Accruals table
                                If commonFunctions.CheckDataSet(ds) = True Then
                                    'only SOLDTO(s) were selected
                                    'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, "", "", "", "", "", 0, 0, 0, False)


                                    iPARTNOTotalCount = 0
                                    SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                                    If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                                        iPARTNOTotalCount = SelectedPARTNOList.Count
                                    End If

                                    If iPARTNOTotalCount > 0 Then

                                        'iterate through PARTNOs
                                        For iPARTNORowCounter = 0 To SelectedPARTNOList.Count - 1
                                            strPARTNO = SelectedPARTNOList(iPARTNORowCounter).ToString.Trim

                                            ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, strPARTNO, "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                            'check to make sure selected combination exists in RELPRC_Accruals table
                                            If commonFunctions.CheckDataSet(ds) = True Then
                                                'SOLDTO(s) and PARTNO(s) were selected
                                                '(LREY) 01/08/2014
                                                'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, strPARTNO, "", "", "", "", 0, 0, 0, False, 0)
                                                ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strPARTNO, "", "", "", 0, 0, 0, False, 0)
                                            End If
                                        Next 'next selected PARTNO
                                    Else
                                        'no PARTNO was selected but at least one soldto was selected
                                        ds = ARGroupModule.GetARShippingHistory(strUGNFacility, "", strSOLDTO, "", "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "", "", "", "", "")

                                        'check to make sure selected combination exists in RELPRC_Accruals table
                                        If commonFunctions.CheckDataSet(ds) = True Then
                                            'SOLDTO(s) and CABBV(s) were selected
                                            '(LREY) 01/08/2014
                                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", strSOLDTO, "", "", "", "", "", 0, 0, 0, False, 0)
                                            ARGroupModule.InsertAREventDetail(ViewState("AREID"), strUGNFacility, "", "", "", "", "", 0, 0, 0, False, 0)
                                        End If
                                    End If 'If iPARTNOTotalCount > 0 Then

                                End If

                            End If 'If iCABBVTotalCount > 0 Then

                        End If 'If SelectedPriceCodeList.Count > 0 Then

                    Next 'for each sold to selected

                    'if event was already approved, reset approval
                    dt = objAREventApproval.GetAREventApprovalStatus(ViewState("AREID"), 21)
                    If commonFunctions.CheckDataTable(dt) = True Then
                        If dt.Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                            iApprovalStatus = dt.Rows(0).Item("StatusID")

                            If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                iAccountingManagerID = dt.Rows(0).Item("TeamMemberID")
                            End If

                            If dt.Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                iApprovalRowID = dt.Rows(0).Item("RowID")
                            End If

                            Select Case iApprovalStatus
                                Case 0, 1, 3
                                    'do nothing
                                Case 4 'already approved - set to open
                                    'objAREventApproval.UpdateAREventApprovalStatus(ViewState("AREID"), 1, iAccountingManagerID, 21, "", 1, iApprovalRowID, iApprovalRowID)
                                    'reset Accounting Manager approval
                                    ARGroupModule.UpdateAREventApprovalReset(ViewState("AREID"), iAccountingManagerID, 1)
                                    ARGroupModule.UpdateAREventStatus(ViewState("AREID"), 1) 'Open (Pending Sales Submission)
                                    ViewState("EventStatusID") = 1
                                    'update history
                                    ARGroupModule.InsertAREventHistory(ViewState("AREID"), ViewState("TeamMemberID"), "Updated Event")
                            End Select
                        End If
                    End If 'end if dt has values

                    'update accrual details
                    ARGroupModule.UpdateAREventAccrual(ViewState("AREID"))

                    lblMessage.Text &= "Information Saved."

                    Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

                Else
                    lblMessage.Text &= "Error: At least one SOLDTO must be selected."
                End If 'SelectedSOLDTOList.Count > 0
            Else
                lblMessage.Text &= "Error: At least one UGN Facility must be selected."
            End If 'if UGN Facility Selected

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    'Protected Sub btnUnSelectAllSoldTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSelectAllSoldTo.Click



    'End Sub

    'Protected Sub btnSelectAllSoldTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAllSoldTo.Click

    '    Try
    '        For Each row As GridViewRow In gvSOLDTO.Rows

    '            Dim index As String = gvSOLDTO.DataKeys(row.RowIndex).Value.ToString.Trim

    '            Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
    '            myCheckBox.Checked = True

    '        Next

    '        RememberOldSOLDTOValues()

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    lblMessageBottom.Text = lblMessage.Text

    'End Sub

    Protected Sub btnFilterSoldTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterSoldTo.Click

        Try
            ClearMessages()

            DisableControls()

            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            Dim strUGNFacility As String = ""

            If ddUGNFacility.SelectedIndex >= 0 Then
                strUGNFacility = ddUGNFacility.SelectedValue

                Session("COMPNYWhereClause") = " AND COMPNY = '" & strUGNFacility & "'"

                Dim strTempSOLDTOWhereClause As String = " AND SOLDTO IN  ("

                If ddSoldTo.SelectedValue <> "" Then
                    strTempSOLDTOWhereClause &= ddSoldTo.SelectedValue & ")"
                Else
                    strTempSOLDTOWhereClause = ""
                End If

                Session("SOLDTOWhereClause") = Session("COMPNYWhereClause") & Session("CABBVWhereClause") & strTempSOLDTOWhereClause & " AND PRCCDE IN ('A','S') AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"

                If ViewState("CustApprvEndDate") <> "" Then
                    Session("SOLDTOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
                End If

                Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
                Session("PARTNOWhereClause") = " AND PARTNO IS NOT NULL "

                Session("CHECKED_SOLDTO_ITEMS") = Nothing
                Session("CHECKED_PRCCDE_ITEMS") = Nothing
                Session("CHECKED_CABBV_ITEMS") = Nothing
                Session("CHECKED_PARTNO_ITEMS") = Nothing

                RePopulateSOLDTOValues()

                gvSOLDTO.Visible = True
                gvSOLDTO.DataBind()
            Else
                lblMessage.Text = "Please select a UGN Facility."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub ddUGNFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUGNFacility.SelectedIndexChanged

        Try
            If ddUGNFacility.SelectedIndex > 0 Then
                BindSoldTo(ddUGNFacility.SelectedValue)
            Else
                BindSoldTo("")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub cbSelectPartNo_OnCheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            ClearMessages()

            Dim cbSelectedCheckbox As CheckBox

            cbSelectedCheckbox = CType(sender, CheckBox)

            lblMessage.Text = cbSelectedCheckbox.ToolTip & " was checked."

            RememberOldPARTNOValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnFilterPriceCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterPriceCode.Click

        Try
   
            ClearMessages()

            Dim SelectedSOLDTOList As New Collections.ArrayList()
            Dim iSOLDTORowCounter As Integer = 0
            Dim iSOLDTOTotalCount As Integer = 0

            Dim strTempSOLDTOWhereClause As String = " AND SOLDTO IN  ("
            Dim strSOLDTOList As String = ""

            If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing Then
                'get all selected SOLDTOs
                SelectedSOLDTOList = DirectCast(Session("CHECKED_SOLDTO_ITEMS"), Collections.ArrayList)

                If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing And SelectedSOLDTOList IsNot Nothing Then
                    iSOLDTOTotalCount = SelectedSOLDTOList.Count
                End If

                If iSOLDTOTotalCount > 0 Then
                    For iSOLDTORowCounter = 0 To iSOLDTOTotalCount - 1
                        If strSOLDTOList <> "" Then
                            strSOLDTOList &= ","
                        End If

                        strSOLDTOList &= SelectedSOLDTOList(iSOLDTORowCounter).ToString.Trim
                    Next
                End If

                If strSOLDTOList <> "" Then
                    strTempSOLDTOWhereClause &= strSOLDTOList & ")"
                End If

                Session("CHECKED_CABBV_ITEMS") = Nothing
                Session("CHECKED_PRCCDE_ITEMS") = Nothing

                If Session("SOLDTOWhereClause") IsNot Nothing Then
                    If Session("SOLDTOWhereClause").ToString <> "" Then

                        Session("PRCCDEWhereClause") = Session("SOLDTOWhereClause") & strTempSOLDTOWhereClause '& Session("CABBVWhereClause")
                    End If
                End If

                btnClearCABBV.Visible = True
                btnFilterCABBV.Visible = True

                gvPriceCode.Visible = True
                gvPriceCode.DataBind()

                RememberOldPriceCodeValues()
            Else
                lblMessage.Text = "Please check at least one SOLDTO item in the list"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnClearPriceCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearPriceCode.Click

        Try
            'filter price code list based on selected facility and part number(s)
            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "

            DisableControls()

            ClearMessages()

            Session("CHECKED_PRCCDE_ITEMS") = Nothing
            Session("CHECKED_CABBV_ITEMS") = Nothing

            'btnFilterPriceCode.Visible = True
            'btnClearPriceCode.Visible = True

            gvSOLDTO.Visible = True

            gvPriceCode.DataBind()

            If Session("CHECKED_SOLDTO_ITEMS") IsNot Nothing Then
                btnUpdate.Visible = ViewState("isAdmin")
                rbUpdateType.Visible = ViewState("isAdmin")

                btnFilterPriceCode.Visible = True
                btnClearPriceCode.Visible = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnFilterPartNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterPartNo.Click

        Try
            ClearMessages()

            Dim SelectedCABBVList As New Collections.ArrayList()
            Dim iCABBVRowCounter As Integer = 0
            Dim iCABBVTotalCount As Integer = 0

            Dim strTempCABBVWhereClause As String = " AND CABBV IN  ("
            Dim strCABBVList As String = ""

            Session("CHECKED_PARTNO_ITEMS") = Nothing

            If Session("CHECKED_CABBV_ITEMS") IsNot Nothing Then
                'get all selected SOLDTOs
                SelectedCABBVList = DirectCast(Session("CHECKED_CABBV_ITEMS"), Collections.ArrayList)

                If Session("CHECKED_CABBV_ITEMS") IsNot Nothing And SelectedCABBVList IsNot Nothing Then
                    iCABBVTotalCount = SelectedCABBVList.Count
                End If

                If iCABBVTotalCount > 0 Then
                    For iCABBVRowCounter = 0 To iCABBVTotalCount - 1
                        If strCABBVList <> "" Then
                            strCABBVList &= ","
                        End If

                        strCABBVList &= "'" & SelectedCABBVList(iCABBVRowCounter).ToString.Trim & "'"
                    Next
                End If

                If strCABBVList <> "" Then
                    strTempCABBVWhereClause &= strCABBVList & ")"
                End If

                'Session("CHECKED_CABBV_ITEMS") = Nothing
                'Session("CHECKED_PRCCDE_ITEMS") = Nothing

                If Session("CABBVWhereClause") IsNot Nothing Then
                    If Session("CABBVWhereClause").ToString <> "" Then

                        Session("PARTNOWhereClause") = Session("SOLDTOWhereClause") & Session("PRCCDEWhereClause") & strTempCABBVWhereClause
                    End If
                End If

                'RememberOldPriceCodeValues()
            Else 'no cabbvs selected
                Session("PARTNOWhereClause") = Session("SOLDTOWhereClause") & Session("PRCCDEWhereClause")
            End If

            RePopulatePARTNOValues()

            gvPartNo.Visible = True
            btnSelectParts.Visible = gvPartNo.Visible
            btnUnselectParts.Visible = gvPartNo.Visible

            gvPartNo.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvPartNo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPartNo.PageIndexChanging

        Try

            RememberOldPARTNOValues()
            gvPartNo.PageIndex = e.NewPageIndex
            gvPartNo.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPriceCode_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPriceCode.PageIndexChanging

        Try

            RememberOldPriceCodeValues()
            gvPriceCode.PageIndex = e.NewPageIndex
            gvPriceCode.DataBind()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvPartNo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPartNo.RowDataBound

        Try

            RePopulatePARTNOValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnClearFilterPartNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearFilterPartNo.Click

        Try

            ClearMessages()

            Session("PARTNOWhereClause") = " AND PARTNO ='NA PART' "

            Session("CHECKED_PARTNO_ITEMS") = Nothing

            gvPartNo.Visible = False
            btnSelectParts.Visible = gvPartNo.Visible
            btnUnselectParts.Visible = gvPartNo.Visible

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnSelectParts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectParts.Click

        Try

            Dim index As String = ""
            Dim myCheckBox As CheckBox

            For Each row As GridViewRow In gvPartNo.Rows

                index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                myCheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                myCheckBox.Checked = True

            Next

            RememberOldPARTNOValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUnselectParts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectParts.Click

        Try

            Dim index As String = ""
            Dim myCheckBox As CheckBox

            For Each row As GridViewRow In gvPartNo.Rows

                index = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

                myCheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
                myCheckBox.Checked = False

            Next

            RememberOldPARTNOValues()

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
