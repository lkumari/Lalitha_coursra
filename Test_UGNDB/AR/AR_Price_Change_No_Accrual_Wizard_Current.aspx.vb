
' ************************************************************************************************
'
' Name:		AR_Price_Change_No_Accrual_Wizard_Current.aspx
' Purpose:	This Code Behind is for the AR Part Accrual Wizard
'
'	The Price Change – “No Accrual” event 
'   a.	Multiple Parts / Price Code Only - Current or Future, NOT BOTH TYPES
'   b.	No selection of SOLDTO, CABBV, DABBV, or Facility. All will be affected per part selection
'   c.	Only allowed for Mass Production And Service
'   f.	The System will check shipping history daily. If the price at the ship date matches the AR Event price, then a notification will be sent to Accounting to close the event.
'   g.	Approval only needed by Accounting Mgr
'
' Date		Author	    
' 04/06/2010   Roderick Carlson
' 08/10/2011   Roderick Carlson - Mike Echevarria some parts may become service but are not shipping that way. So service should always be allowed for selection
Partial Class AR_Price_Change_No_Accrual_Wizard_Current
    Inherits System.Web.UI.Page

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
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

    Protected Sub BindData()

        Try

            Dim ds As DataSet
            Dim dt As DataTable

            Dim objAREventDetailBLL As AREventDetailBLL = New AREventDetailBLL

            ds = ARGroupModule.GetAREvent(ViewState("AREID"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("CustApprvEffDate") = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
                lblCustApprvEffdate.Text = ds.Tables(0).Rows(0).Item("CustApprvEffDate").ToString
            End If

            dt = objAREventDetailBLL.GetAREventDetail(ViewState("AREID"))

            If commonFunctions.CheckDataTable(dt) = True Then
                ddUGNFacility.SelectedValue = dt.Rows(0).Item("COMPNY").ToString
                Session("COMPNYWhereClause") = " AND COMPNY IN ('" & ddUGNFacility.SelectedValue & "') "

                txtFGPartNo.Text = Replace(dt.Rows(0).Item("PARTNO").ToString.Trim, "( f )", "").Trim
                txtCustomerPartNo.Text = dt.Rows(0).Item("CPART").ToString.Trim
                'txtBarCodePartNo.Text = dt.Rows(0).Item("BARPT#").ToString.Trim
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

    Protected Function HandleCustomerPartNoPopUps(ByVal CustomerPartNoClientControlID As String, ByVal BPCSPartNoClientControlID As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../DataMaintenance/CustomerPartNoLookUp.aspx?CustomerPartNoValueControlID=" & CustomerPartNoClientControlID

            'If BarCodePartNoClientControlID IsNot Nothing And BarCodePartNoClientControlID <> "" Then
            '    strPagePath &= "&BarCodePartNoValueControlID=" & BarCodePartNoClientControlID
            'End If

            If BPCSPartNoClientControlID IsNot Nothing And BPCSPartNoClientControlID <> "" Then
                strPagePath &= "&BPCSPartNoValueControlID=" & BPCSPartNoClientControlID
            End If

            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','CustomerPartNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleCustomerPartNoPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleCustomerPartNoPopUps = ""
        End Try

    End Function

    'Private Sub UNCheckAllParts()

    '    Try
    '        Session("CHECKED_PARTNO_ITEMS") = Nothing

    '        For Each row As GridViewRow In gvPartNo.Rows

    '            Dim index As String = gvPartNo.DataKeys(row.RowIndex).Value.ToString.Trim

    '            If index <> txtSelectedPartNo.Text Then
    '                Dim myCheckBox As CheckBox = DirectCast(row.FindControl("cbSelect"), CheckBox)
    '                myCheckBox.Checked = False
    '            End If

    '        Next

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Private Sub RememberOldPARTNOValues()

        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()

            'Dim index As Integer = -1
            Dim index As String = ""

            For Each row As GridViewRow In gvPartNo.Rows

                'index = CInt(gvPARTNO.DataKeys(row.RowIndex).Value)
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

                'btnUpdate.Visible = ViewState("isAdmin")
                'rbUpdateType.Visible = ViewState("isAdmin")

                gvPriceCode.Visible = False
                btnFilterPriceCode.Visible = True
                btnClearFilterPriceCode.Visible = True

            Else
                Session("CHECKED_PARTNO_ITEMS") = Nothing

                btnUpdate.Visible = False
                rbUpdateType.Visible = False

                btnFilterPriceCode.Visible = False
                btnClearFilterPriceCode.Visible = False
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

                    'Dim index As Integer = CInt(gvPARTNO.DataKeys(row.RowIndex).Value)
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

                btnUpdate.Visible = ViewState("isAdmin")
                rbUpdateType.Visible = ViewState("isAdmin")
            Else
                Session("CHECKED_PRCCDE_ITEMS") = Nothing

                btnUpdate.Visible = False
                rbUpdateType.Visible = False

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

    Private Sub RePopulatePriceCodeValues()

        Try
            Dim SelectedPriceCodeList As Collections.ArrayList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

            If SelectedPriceCodeList IsNot Nothing AndAlso SelectedPriceCodeList.Count > 0 Then
                For Each row As GridViewRow In gvPriceCode.Rows

                    'Dim index As Integer = CInt(gvPriceCode.DataKeys(row.RowIndex).Value)
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Price Change - No Accrual Wizard"

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
            ViewState("CurrentTeamMemberID") = 0
            ViewState("isAdmin") = False
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 246 'Mike Echevarria
                End If

                ViewState("TeamMemberID") = iTeamMemberID

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

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page            
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try

            DisableControls()

            'if sales/vp sales or admin, then allow use
            btnFilterPartNo.Visible = ViewState("isAdmin")
            btnClearFilterPartNo.Visible = ViewState("isAdmin")

            'rbUpdateType.Visible = ViewState("isAdmin")
            'btnUpdate.Visible = ViewState("isAdmin")

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page            
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub DisableControls()

        Try
            btnFilterPriceCode.Visible = False
            btnClearFilterPriceCode.Visible = False

            'btnFilterSOLDTO.Visible = False
            'btnClearFilterSOLDTO.Visible = False

            btnUpdate.Visible = False
            rbUpdateType.Visible = False

            gvPartNo.Visible = False
            gvPriceCode.Visible = False

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

                CheckRights()

                BindCriteria()

                Session("CHECKED_PARTNO_ITEMS") = Nothing
                Session("CHECKED_PRCCDE_ITEMS") = Nothing

                Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "

                ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                lblAREID.Text = ViewState("AREID")

                'search Customer PartNo
                Dim strCustomerPartNoClientScript As String = HandleCustomerPartNoPopUps(txtCustomerPartNo.ClientID, "")
                iBtnCustomerPartNo.Attributes.Add("onClick", strCustomerPartNoClientScript)

                ''search BarCode PartNo
                'Dim strBarCodePartNoClientScript As String = HandleCustomerPartNoPopUps(txtTemp.ClientID, txtBarCodePartNo.ClientID, "")
                'iBtnBarCodePartNo.Attributes.Add("onClick", strBarCodePartNoClientScript)

                'these changes could be in the future
                'Session("PARTNOWhereClause") = " AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"
                Session("PARTNOWhereClause") = " AND SHPDTE >= '20000101' "

                'If ViewState("CustApprvEndDate") <> "" Then
                '    Session("PARTNOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
                'End If

                'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
                Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S','T','P') "
                'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
                Session("SOLDTOWhereClause") = " AND SOLDTO > 0 "
                Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

                BindData()

                EnableControls()

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > <a href='AR_Event_List.aspx'>AR Event Search </a> > <a href='AR_Event_Detail.aspx?AREID=" & ViewState("AREID") & "'> Event Detail </a> > Price Change - No Accrual Wizard for current parts "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
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

    Protected Sub btnBackToAREvent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToAREvent.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

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

            DisableControls()
            Session("CHECKED_PARTNO_ITEMS") = Nothing
            Session("CHECKED_PRCCDE_ITEMS") = Nothing

            'Session("PARTNOWhereClause") = " AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'"
            Session("PARTNOWhereClause") = " AND SHPDTE >= '20000101' "

            'If ViewState("CustApprvEndDate") <> "" Then
            '    Session("PARTNOWhereClause") &= " AND SHPDTE <= '" & ViewState("CustApprvEndDate") & "'"
            'End If

            'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S','T','P') "
            'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
            Session("SOLDTOWhereClause") = " AND SOLDTO > 0 "
            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            txtCustomerPartNo.Text = ""
            'txtBarCodePartNo.Text = ""
            txtFGPartNo.Text = ""

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
            'filter part number list based on text fields (FG part no or cust part no)

            ClearMessages()

            'Session("PARTNOWhereClause") = Session("COMPNYWhereClause") & " AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "'  "
            Session("PARTNOWhereClause") = Session("COMPNYWhereClause") & " SHPDTE >= '20000101' "

            DisableControls()

            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S','T','P') "
            'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
            Session("SOLDTOWhereClause") = " AND SOLDTO > 0 "
            Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            Session("CHECKED_PARTNO_ITEMS") = Nothing
            Session("CHECKED_PRCCDE_ITEMS") = Nothing

            Dim ds As DataSet

            Dim iRowCounter As Integer = 0

            Dim strTempWhereClause As String = "AND PARTNO IN  ("
            Dim strPartNoList As String = ""

            If txtCustomerPartNo.Text.Trim <> "" Or txtFGPartNo.Text.Trim <> "" Then
                '(LREY) 01/08/2014
                'ds = commonFunctions.GetCustomerPartBPCSPartRelate(txtFGPartNo.Text.Trim, txtCustomerPartNo.Text.Trim, "", "", "")

                'If commonFunctions.CheckDataSet(ds) = True Then
                '    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                '        If ds.Tables(0).Rows(iRowCounter).Item("BPCSPartNo").ToString <> "" Then
                '            If strPartNoList <> "" Then
                '                strPartNoList &= ","
                '            End If

                '            strPartNoList &= "'" & ds.Tables(0).Rows(iRowCounter).Item("BPCSPartNo") & "'"
                '        End If

                '        'do not get too many parts at a time
                '        If iRowCounter > 250 Then
                '            iRowCounter = ds.Tables(0).Rows.Count - 1
                '            lblMessage.Text &= "WARNING: TOO MANY INTERNAL FINISHED PART NUMBERS (" & ds.Tables(0).Rows.Count.ToString & ") HAVE BEEN FOUND TO MATCH THE CUSTOMER PART NUMBER. PLEASE BE MORE SPECIFIC IN YOUR SEARCH CRITERIA OR USE THE SELECT BY CUSTOMER WIZARD."
                '        End If
                '    Next
                'End If

                If strPartNoList <> "" Then
                    strTempWhereClause &= strPartNoList

                    'Session("PARTNOWhereClause") = Session("PRCCDEWhereClause") & Session("COMPNYWhereClause") & " AND SHPDTE >= '" & ViewState("CustApprvEffDate") & "' " & strTempWhereClause & ")"
                    Session("PARTNOWhereClause") = Session("PRCCDEWhereClause") & Session("COMPNYWhereClause") & " AND SHPDTE >= '20000101' " & strTempWhereClause & ")"

                    gvPartNo.Visible = True

                    gvPartNo.DataBind()

                    RememberOldPARTNOValues()

                    gvPriceCode.DataBind()

                End If
            Else
                lblMessage.Text = "Error: Please filter the part number list first."
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

    Private Sub FilterPriceCode()

        Try

            RememberOldPARTNOValues()

            'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','F','O','P','S','T') "
            'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S','T','P') "

            'Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
            'Session("SOLDTOWhereClause") = " AND SOLDTO > 0 "
            'Session("CABBVWhereClause") = " AND CABBV IS NOT NULL "

            Dim SelectedPARTNOList As New Collections.ArrayList()

            Dim iRowCounter As Integer = 0
            Dim strTempWhereClause As String = "AND PARTNO IN  ("
            Dim strPartNoList As String = ""

            If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
                SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                For iRowCounter = 0 To SelectedPARTNOList.Count - 1
                    If strPartNoList <> "" Then
                        strPartNoList &= ","
                    End If

                    strPartNoList &= "'" & SelectedPARTNOList.Item(iRowCounter).ToString.Trim & "'"
                Next
            End If

            If strPartNoList <> "" Then
                strTempWhereClause &= strPartNoList
            End If

            If strPartNoList <> "" Then
                Session("PARTNOWhereClause") = " AND SHPDTE >= '20000101' " & strTempWhereClause & ")"
                'Session("PRCCDEWhereClause") = Session("PARTNOWhereClause") & " AND PRCCDE IN ('A','F','O','P','S','T') "
                'Session("PRCCDEWhereClause") = Session("PARTNOWhereClause") & " AND PRCCDE IN ('A','S') "
                Session("PRCCDEWhereClause") = Session("PARTNOWhereClause") & " AND PRCCDE IN ('A','S','T','P') "
                'Session("COMPNYWhereClause") = Session("PRCCDEWhereClause") & " AND COMPNY IN ('UN','UP','UR','US') "
                'Session("SOLDTOWhereClause") = Session("PRCCDEWhereClause") & " AND SOLDTO > 0 "
                'Session("CABBVWhereClause") = Session("PRCCDEWhereClause") & " AND CABBV IS NOT NULL "

                gvPartNo.DataBind()
                gvPriceCode.DataBind()
            Else
                lblMessage.Text = "Error: at least one part number must be selected"
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
    Protected Sub btnFilterPriceCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilterPriceCode.Click


        Try
            Dim SelectedPARTNOList As New Collections.ArrayList()
            Dim iPartTotalCount As Integer = 0

            Dim strTempWhereClause As String = " AND PARTNO IN ("
            Dim strPartNoList As String = ""
            Dim strPartNo As String = ""

            'filter price code list based on selected part number(s)
            'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "

            DisableControls()

            ClearMessages()

            If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
                Session("CHECKED_PRCCDE_ITEMS") = Nothing

                iPartTotalCount = 0

                SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                    iPartTotalCount = SelectedPARTNOList.Count
                End If

                If iPartTotalCount > 0 Then
                    For iRowPartCounter = 0 To iPartTotalCount - 1
                        'only collect the first item selected
                        strPartNo = SelectedPARTNOList.Item(iRowPartCounter).ToString.Trim

                        If strPartNo <> "" Then
                            If strPartNoList <> "" Then
                                strPartNoList &= ","
                            End If

                            strPartNoList &= "'" & strPartNo & "'"

                        End If
                    Next

                    strTempWhereClause &= strPartNoList & ") "

                    Session("PARTNOWhereClause") = strTempWhereClause
                    
                    'Mike Echevarria 2011-Aug-10
                    'Session("PRCCDEWhereClause") = Session("PARTNOWhereClause") & Session("PRCCDEWhereClause")
                    'allow mass production and service because some parts are currently mass produciton but might become service
                    Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "

                    btnUpdate.Visible = ViewState("isAdmin")
                    rbUpdateType.Visible = ViewState("isAdmin")

                    btnFilterPriceCode.Visible = True
                    btnClearFilterPriceCode.Visible = True

                    gvPriceCode.Visible = True

                    gvPriceCode.DataBind()
                End If

            Else
                lblMessage.Text = "Please check at least one part number in the list"
            End If

            'If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
            '    btnUpdate.Visible = ViewState("isAdmin")
            '    rbUpdateType.Visible = ViewState("isAdmin")

            '    btnFilterPriceCode.Visible = True
            '    btnClearFilterPriceCode.Visible = True
            'End If

            'btnFilterPriceCode.Visible = True
            'btnClearFilterPriceCode.Visible = True

            gvPartNo.Visible = True

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

    Protected Sub btnClearFilterPriceCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearFilterPriceCode.Click

        Try
            'filter price code list based on selected facility and part number(s)
            'Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S') "
            Session("PRCCDEWhereClause") = " AND PRCCDE IN ('A','S','T','P') "

            DisableControls()

            ClearMessages()

            Session("CHECKED_PRCCDE_ITEMS") = Nothing

            btnFilterPriceCode.Visible = True
            btnClearFilterPriceCode.Visible = True

            gvPartNo.Visible = True

            gvPriceCode.DataBind()

            If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then
                btnUpdate.Visible = ViewState("isAdmin")
                rbUpdateType.Visible = ViewState("isAdmin")

                btnFilterPriceCode.Visible = True
                btnClearFilterPriceCode.Visible = True
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

    Protected Sub gvPriceCode_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceCode.RowDataBound

        Try

            RePopulatePriceCodeValues()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim dsCurrentPrice As DataSet

            Dim dt As DataTable
            Dim objAREventApproval As New ARApprovalBLL

            Dim SelectedPARTNOList As New Collections.ArrayList()
            Dim SelectedPriceCodeList As New Collections.ArrayList()

            Dim dCurrentPrice As Double = 0

            Dim iApprovalStatus As Integer = 0
            Dim iAccountingManagerID As Integer = 0
            Dim iApprovalRowID As Integer = 0

            Dim iPartRowCounter As Integer = 0
            Dim iPartTotalCount As Integer = 0

            Dim iPriceCodeRowCounter As Integer = 0
            Dim iPriceCodeTotalCount As Integer = 0

            Dim strPartNo As String = ""
            Dim strPriceCodeNo As String = ""

            Dim strFacility As String = ""
            Dim strWhereClause As String = ""
            Dim strWhereClauseChanged As String = ""

            If ddUGNFacility.SelectedIndex > 0 Then
                strFacility = ddUGNFacility.SelectedValue

                strWhereClause = " AND COMPNY = '" & strFacility & "'"
                'insert new AR Event Details
                If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then

                    'delete any future parts
                    ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "F")

                    'check to see if current parts should be deleted or appended
                    If rbUpdateType.SelectedValue = "R" Then
                        'delete all AR Event Details
                        ARGroupModule.DeleteAREventDetail(ViewState("AREID"), "")
                    End If

                    iPartTotalCount = 0
                    SelectedPARTNOList = DirectCast(Session("CHECKED_PARTNO_ITEMS"), Collections.ArrayList)

                    If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing And SelectedPARTNOList IsNot Nothing Then
                        iPartTotalCount = SelectedPARTNOList.Count
                    End If

                    For iRowPartCounter = 0 To iPartTotalCount - 1
                        strPartNo = SelectedPARTNOList.Item(iRowPartCounter).ToString.Trim

                        If strPartNo <> "" Then
                            iPriceCodeTotalCount = 0
                            SelectedPriceCodeList = DirectCast(Session("CHECKED_PRCCDE_ITEMS"), Collections.ArrayList)

                            If Session("CHECKED_PRCCDE_ITEMS") IsNot Nothing And SelectedPriceCodeList IsNot Nothing Then
                                iPriceCodeTotalCount = SelectedPriceCodeList.Count
                            End If

                            If iPriceCodeTotalCount > 0 Then
                                'iterate price codes
                                For iPriceCodeRowCounter = 0 To SelectedPriceCodeList.Count - 1
                                    strPriceCodeNo = SelectedPriceCodeList.Item(iPriceCodeRowCounter).ToString.Trim

                                    If strPriceCodeNo <> "" Then
                                        'cabbv and soldto do not affect the price master. so it can be collected one and used a lot below
                                        strWhereClauseChanged = strWhereClause & " AND PARTNO = '" & strPartNo & "'" & " AND PRCCDE = '" & strPriceCodeNo & "'"

                                        dCurrentPrice = 0
                                        dsCurrentPrice = ARGroupModule.GetARShippingPriceDynamically(ViewState("AREID"), strWhereClauseChanged)
                                        If commonFunctions.CheckDataSet(dsCurrentPrice) = True Then
                                            If dsCurrentPrice.Tables(0).Rows(0).Item("USE_RELPRC") IsNot System.DBNull.Value Then
                                                'first row is most recent value
                                                dCurrentPrice = dsCurrentPrice.Tables(0).Rows(0).Item("USE_RELPRC")
                                            End If

                                        End If


                                        'Mike Echevarria 2011-Aug-10
                                        'ds = ARGroupModule.GetARShippingHistory(strFacility, "", "", strPartNo, strPriceCodeNo, "", "", "", "", "")
                                        'allow mass production and service because some parts are currently mass produciton but might become service
                                        ds = ARGroupModule.GetARShippingHistory(strFacility, "", "", strPartNo, "", "", "", "", "", "", "", "")

                                        'check to make sure selected combination exists in RELPRC_Accruals table
                                        If commonFunctions.CheckDataSet(ds) = True Then
                                            'only facility, partno, and price code(s) were selected
                                            '(LREY) 01/08/2014
                                            'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", "", strPartNo, txtCustomerPartNo.Text.Trim, "", strPriceCodeNo, "", 0, 0, dCurrentPrice, False, 0)
                                            ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", strPartNo, txtCustomerPartNo.Text.Trim, "", strPriceCodeNo, 0, 0, dCurrentPrice, False, 0)
                                        End If

                                    End If 'If strPriceCodeNo <> "" Then
                                Next 'iPriceCodeRowCounter 
                            Else

                                'ds = ARGroupModule.GetARShippingHistory("", "", "", strPartNo, "", ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), "")
                                ds = ARGroupModule.GetARShippingHistory(strFacility, "", "", strPartNo, "", "", "", "", "", "", "", "")

                                'check to make sure selected combination exists in RELPRC_Accruals table
                                If commonFunctions.CheckDataSet(ds) = True Then

                                    'get current price
                                    'strWhereClause = " AND COMPNY = '" & strFacility & "'"                                    
                                    'strWhereClause &= " AND EDATE <= '" & ViewState("CustApprvEffDate") & "'"
                                    strWhereClauseChanged = strWhereClause & " AND PARTNO = '" & strPartNo & "'"

                                    dCurrentPrice = 0
                                    dsCurrentPrice = ARGroupModule.GetARShippingPriceDynamically(ViewState("AREID"), strWhereClauseChanged)
                                    If commonFunctions.CheckDataSet(dsCurrentPrice) = True Then
                                        If dsCurrentPrice.Tables(0).Rows(0).Item("USE_RELPRC") IsNot System.DBNull.Value Then
                                            'first row is most recent value
                                            dCurrentPrice = dsCurrentPrice.Tables(0).Rows(0).Item("USE_RELPRC")
                                        End If

                                    End If

                                    'only facility and partno were selected
                                    '(LREY) 01/08/2014
                                    'ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", "", strPartNo, txtCustomerPartNo.Text.Trim, "", "", "", 0, 0, dCurrentPrice, False, 0)
                                    ARGroupModule.InsertAREventDetail(ViewState("AREID"), strFacility, "", strPartNo, txtCustomerPartNo.Text.Trim, "", "", 0, 0, dCurrentPrice, False, 0)

                                End If

                            End If 'If SelectedPriceCodeList.Count > 0 Then

                        End If ' If strPartNo <> "" Then
                    Next 'iRowPartCounter = 0 

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

                    lblMessage.Text &= "Information Saved."

                    Response.Redirect("AR_Event_Detail.aspx?AREID=" & ViewState("AREID"), False)

                End If 'If Session("CHECKED_PARTNO_ITEMS") IsNot Nothing Then

            Else
                lblMessage.Text = "Error: Please select a UGN Facility"
            End If 'if facility is selected

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
            ClearMessages()

            If ddUGNFacility.SelectedIndex > 0 Then
                Session("COMPNYWhereClause") = " AND COMPNY IN ('" & ddUGNFacility.SelectedValue & "') "
            Else
                Session("COMPNYWhereClause") = " AND COMPNY IN ('UN','UP','UR','US') "
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

    Protected Sub btnFuturePriceChangeNoAccrualWizard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFuturePriceChangeNoAccrualWizard.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Price_Change_No_Accrual_Wizard_Future.aspx?AREID=" & ViewState("AREID"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnCustomerWizard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomerWizard.Click

        Try
            ClearMessages()

            Response.Redirect("AR_Customer_Price_Change_No_Accrual_Wizard.aspx?AREID=" & ViewState("AREID"), False)

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
