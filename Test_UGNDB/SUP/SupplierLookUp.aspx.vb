' ************************************************************************************************
' Name:		SupplierLookUp.aspx
' Purpose:	This Code Behind is for the Supplier Request Look Up page. This page will be called from
'           various modules to allow team members to search or request new suppliers and include unapproved
'           suppliers as (f) future vendors in the drop down lists.
'
' Date		    Author	    
' 05/06/02011   LRey			Created .Net application
' 02/24/2014    LRey            Modified to adhere to the new ERP Supplier codes.
' ************************************************************************************************

Partial Class SUP_SupplierLookUp
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim m As ASP.lookupmasterpage_master = Master
            ' ''check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "Supplier Look Up"
                mpTextBox.Font.Size = 18
                mpTextBox.Visible = True
                mpTextBox.Font.Bold = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If


            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pSUPNo") <> "" Then
                ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")
            Else
                ViewState("pSUPNo") = ""
            End If

            ''Used to take user back to Approval screen after reset/save
            If HttpContext.Current.Request.QueryString("pAprv") <> "" Then
                ViewState("pAprv") = HttpContext.Current.Request.QueryString("pAprv")
            Else
                ViewState("pAprv") = 0
            End If

            'Used to display data in gridview if search buttonwas pressed
            If HttpContext.Current.Request.QueryString("sBtnSrch") <> "" Then
                ViewState("sBtnSrch") = HttpContext.Current.Request.QueryString("sBtnSrch")
            Else
                ViewState("sBtnSrch") = False
            End If

            'Used to capture the form name where the user entered from
            If HttpContext.Current.Request.QueryString("pForm") <> "" Then
                ViewState("pForm") = HttpContext.Current.Request.QueryString("pForm")
            Else
                ViewState("pForm") = ""
            End If

            ''Used to take user back to CapEx screen after reset/save
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If


            ''Used to take user back to IOR screen after reset/save
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
            Else
                ViewState("pIORNo") = ""
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtSUPNo.Focus()

            'If HttpContext.Current.Session("sessionSupCurrentPage") IsNot Nothing Then

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sSUPNo") = ""
                ViewState("sSName") = ""
                ViewState("sVTYPE") = ""
                ViewState("sRStat") = ""
                ViewState("sVendor") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("SUPLU_SUPNo") Is Nothing Then
                    txtSUPNo.Text = Server.HtmlEncode(Request.Cookies("SUPLU_SUPNo").Value)
                    ViewState("sSUPNo") = Server.HtmlEncode(Request.Cookies("SUPLU_SUPNo").Value)
                End If

                If Not Request.Cookies("SUPLU_SName") Is Nothing Then
                    txtVendorName.Text = Server.HtmlEncode(Request.Cookies("SUPLU_SName").Value)
                    ViewState("sSName") = Server.HtmlEncode(Request.Cookies("SUPLU_SName").Value)
                End If

                If Not Request.Cookies("SUPLU_VTYPE") Is Nothing Then
                    ddVendorType.SelectedValue = Server.HtmlEncode(Request.Cookies("SUPLU_VTYPE").Value)
                    ViewState("sVTYPE") = Server.HtmlEncode(Request.Cookies("SUPLU_VTYPE").Value)
                End If

                If Not Request.Cookies("SUPLU_RSTAT") Is Nothing Then
                    ddRecStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("SUPLU_RSTAT").Value)
                    ViewState("sRStat") = Server.HtmlEncode(Request.Cookies("SUPLU_RSTAT").Value)
                End If

                If Not Request.Cookies("SUPLU_Vendor") Is Nothing Then
                    txtVendor.Text = Server.HtmlEncode(Request.Cookies("SUPLU_Vendor").Value)
                    ViewState("sVendor") = Server.HtmlEncode(Request.Cookies("SUPLU_Vendor").Value)
                End If


            Else
                ViewState("sSUPNo") = txtSUPNo.Text
                ViewState("sSName") = txtVendorName.Text
                ViewState("sVTYPE") = ddVendorType.SelectedValue
                ViewState("sRStat") = ddRecStatus.SelectedValue
                ViewState("sVendor") = txtVendor.Text
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

#Region "Form Level Security using Roles &/or Subscriptions"
    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            ViewState("Admin") = False
            ViewState("ObjectRole") = False

            If ViewState("pForm") = "SUPPLIER" Then
                gvSupplierLookUp.Columns(0).Visible = False
            End If

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 110 'Supplier Request Form ID
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
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                           
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Supchasing Leads
                                            btnAdd.Enabled = True
                                            ViewState("ObjectRole") = True
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            'N/A
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            'N/A
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            'N/A
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

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
#End Region 'EOF Security

    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddVendorType.DataSource = ds
                ddVendorType.DataTextField = ds.Tables(0).Columns("ddVType").ColumnName.ToString()
                ddVendorType.DataValueField = ds.Tables(0).Columns("VType").ColumnName.ToString()
                ddVendorType.DataBind()
                ddVendorType.Items.Insert(0, "")
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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            SUPModule.DeleteSupplierLookUpCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            Select Case ViewState("pForm")
                Case "EXPKG"
                    Response.Redirect("SupplierRequest.aspx?pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo"), False)
                Case "PURIOR"
                    Response.Redirect("SupplierRequest.aspx?pForm=" & ViewState("pForm") & "&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo"), False)
                Case "SUPPLIER"
                    Response.Redirect("SupplierRequest.aspx", False)
            End Select

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("SUPLU_SUPNo").Value = txtSUPNo.Text
            Response.Cookies("SUPLU_SName").Value = txtVendorName.Text
            Response.Cookies("SUPLU_VTYPE").Value = ddVendorType.SelectedValue
            Response.Cookies("SUPLU_RSTAT").Value = ddRecStatus.SelectedValue
            Response.Cookies("SUPLU_Vendor").Value = txtVendor.Text

            Dim sParm As String = Nothing
            Dim pForm As String = ViewState("pForm")
            Select Case pForm
                Case "EXPKG"
                    sParm = "&pForm=EXPKG&pProjNo=" & ViewState("pProjNo")
                Case "PURIOR"
                    sParm = "&pForm=PURIOR&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo")
                Case "SUPPLIER"
                    sParm = "&pForm=SUPPLIER"
            End Select

            If ViewState("sSUPNo") = Nothing And ViewState("sSName") = Nothing And ViewState("sVTYPE") = Nothing And ViewState("sRStat") = Nothing And ViewState("sVendor") = Nothing Then
                Response.Redirect("SupplierLookUp.aspx?sBtnSrch=false" & sParm, False)
            Else
                Response.Redirect("SupplierLookUp.aspx?sBtnSrch=True&sSUPNo=" & ViewState("sSUPNo") & "&sSName=" & ViewState("sSName") & "&sVTYPE=" & ViewState("sVTYPE") & "&sRStat=" & ViewState("sRStat") & "&sVendor=" & ViewState("sVendor") & sParm, False)

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

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            SUPModule.DeleteSupplierLookUpCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            Dim sParm As String = Nothing
            Dim pForm As String = ViewState("pForm")
            Select Case pForm
                Case "EXPKG"
                    sParm = "&pForm=EXPKG&pProjNo=" & ViewState("pProjNo")
                Case "PURIOR"
                    sParm = "&pForm=PURIOR&pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo")
                Case "SUPPLIER"
                    sParm = "&pForm=SUPPLIER"
            End Select

            Response.Redirect("SupplierLookUp.aspx?BtnSrch=False" & sParm, False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Protected Function SetHyperlink(ByVal VendorType As String, ByVal VendorNo As String, ByVal Status As String, ByVal SUPNo As Integer) As String
        Dim strReturnValue As String = ""
        Try
            SUPModule.DeleteSupplierLookUpCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            Dim pform As String = ViewState("pForm")
            If (Status <> "Void") And (VendorNo <> 0 Or VendorNo <> Nothing) Then
                If Status <> "Inactive" Then
                    Select Case pform
                        Case "EXPKG"
                            strReturnValue = "~/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1&pVTp=" & VendorType & "&pVNo=" & VendorNo & "&pNF=" & IIf(SUPNo = 0, 2, 1)
                        Case "PURIOR"
                            strReturnValue = "~/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pVTp=" & VendorType & "&pVNo=" & VendorNo & "&pNF=" & IIf(SUPNo = 0, 2, 1)
                    End Select
                End If
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetHyperlink = strReturnValue

    End Function 'EOF SetHyperlink

    Protected Function SetClickable(ByVal Status As String) As String

        Dim strReturnValue As String = "False"

        Try
            If Status <> "VOID" Then
                If Status <> "INACTIVE" Then
                    strReturnValue = "True"
                End If
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetClickable = strReturnValue

    End Function 'EOF SetClickable


    Protected Function SetHyperlink2(ByVal VendorType As String, ByVal VendorNo As String, ByVal Status As String, ByVal VendorName As String, ByVal SupNo As Integer) As String
        Dim strReturnValue As String = ""
        Try
            Dim ds As DataSet = New DataSet
            Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value

            SUPModule.DeleteSupplierLookUpCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            Dim pForm As String = ViewState("pForm")
            If Status = "INACTIVE" Then
                Select Case pForm
                    Case "EXPKG"
                        strReturnValue = "~/EXP/PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1&pVTp=" & VendorType & "&pVNo=" & VendorNo & "&pNF=" & IIf(SupNo = 0, 2, 1)
                    Case "PURIOR"
                        strReturnValue = "~/PUR/InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo") & "&pVTp=" & VendorType & "&pVNo=" & VendorNo & "&pNF=" & IIf(SupNo = 0, 2, 1)
                End Select
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetHyperlink2 = strReturnValue

    End Function 'EOF SetHyperlink2

    Protected Function SetClickable2(ByVal Status As String) As String

        Dim strReturnValue As String = "False"

        Try
            If Status = "INACTIVE" Then
                strReturnValue = "True"
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetClickable2 = strReturnValue

    End Function 'EOF SetClickable2


    Protected Sub gvSupplierLookUp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSupplierLookUp.RowDataBound
        Try

            '***
            'This section provides the user with the popup for confirming the delete of a record.
            'Called by the onClientClick event.
            '***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the PrevAPL ImageButton
                Dim imgBtn2 As ImageButton = CType(e.Row.FindControl("ibtnActivate"), ImageButton)
                If imgBtn2 IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)
                    If db.CommandName = "REQSUPACT" Then
                        Dim rec As Supplier.SupplierRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Supplier.SupplierRow)
                        Dim strPreviewClientScript As String = "javascript:void(window.open('RequestSupplierActivation.aspx?pVNO=" & DataBinder.Eval(e.Row.DataItem, "VendorNo") & "&pVName=" & DataBinder.Eval(e.Row.DataItem, "VendorName") & "&pVType=" & DataBinder.Eval(e.Row.DataItem, "VendorType") & "&pForm=" & ViewState("pForm") & "&pProjNo=" & ViewState("pProjNo") & "'," & Now.Ticks.ToString & ",'width=800px,height=550px,top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=no,location=yes'));"
                        db.Attributes.Add("onclick", strPreviewClientScript)
                    End If
                End If
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvPlatformProgramList_RowDataBound

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            SUPModule.DeleteSupplierLookUpCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing
            Dim pForm As String = ViewState("pForm")
            Select Case pForm
                Case "EXPKG"
                    Response.Redirect("~\EXP\PackagingExpProj.aspx?pProjNo=" & ViewState("pProjNo") & "&pEV=1", False)
                Case "PURIOR"
                    Response.Redirect("~\PUR\InternalOrderRequest.aspx?pIORNo=" & ViewState("pIORNo") & "&pProjNo=" & ViewState("pProjNo"), False)
                Case "SUPPLIER"
                    Response.Redirect("SupplierRequestList.aspx", False)
            End Select

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOf btnCancel_Click
End Class
