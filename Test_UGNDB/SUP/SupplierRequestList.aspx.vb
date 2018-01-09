' ************************************************************************************************
' Name:	SupplierRequestList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 09/07/2010    LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 02/24/2014    LRey            Modified to adhere to the new ERP Supplier codes.
' ************************************************************************************************
Partial Class SUP_SupplierRequestList
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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

            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Supplier Request Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                If ViewState("pAprv") = 0 Then
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Supplier Request Search"
                Else
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > Supplier Request Search > <a href='crSupplierRequestApproval.aspx?pSUPNo=" & ViewState("pSUPNo") & "&pAprv=1'><b>Approval</b></a>"
                End If
                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("PURExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            'focus on Vehicle List screen Program field
            txtSUPNo.Focus()

            If HttpContext.Current.Session("sessionSupCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionSupCurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("sSUPNo") = ""
                ViewState("sRBID") = 0
                ViewState("sVendor") = ""
                ViewState("sSName") = ""
                ViewState("sPDesc") = ""
                ViewState("sVTYPE") = ""
                ViewState("sVTDesc") = ""
                ViewState("sLoc") = ""
                ViewState("sRStat") = ""
                ViewState("sDSF") = ""
                ViewState("sDST") = ""

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("SUP_SUPNo") Is Nothing Then
                    txtSUPNo.Text = Server.HtmlEncode(Request.Cookies("SUP_SUPNo").Value)
                    ViewState("sSUPNo") = Server.HtmlEncode(Request.Cookies("SUP_SUPNo").Value)
                End If

                If Not Request.Cookies("SUP_RBID") Is Nothing Then
                    ddRequestedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("SUP_RBID").Value)
                    ViewState("sRBID") = Server.HtmlEncode(Request.Cookies("SUP_RBID").Value)
                End If

                If Not Request.Cookies("SUP_Vendor") Is Nothing Then
                    txtVendor.Text = Server.HtmlEncode(Request.Cookies("SUP_Vendor").Value)
                    ViewState("sVendor") = Server.HtmlEncode(Request.Cookies("SUP_Vendor").Value)
                End If

                If Not Request.Cookies("SUP_SName") Is Nothing Then
                    txtVendorName.Text = Server.HtmlEncode(Request.Cookies("SUP_SName").Value)
                    ViewState("sSName") = Server.HtmlEncode(Request.Cookies("SUP_SName").Value)
                End If

                If Not Request.Cookies("SUP_PDesc") Is Nothing Then
                    txtProdDesc.Text = Server.HtmlEncode(Request.Cookies("SUP_PDesc").Value)
                    ViewState("sPDesc") = Server.HtmlEncode(Request.Cookies("SUP_PDesc").Value)
                End If

                If Not Request.Cookies("SUP_VTYPE") Is Nothing Then
                    ddVendorType.SelectedValue = Server.HtmlEncode(Request.Cookies("SUP_VTYPE").Value)
                    ViewState("sVTYPE") = Server.HtmlEncode(Request.Cookies("SUP_VTYPE").Value)
                End If

                If Not Request.Cookies("SUP_VTDesc") Is Nothing Then
                    ddVTypeDesc.SelectedValue = Server.HtmlEncode(Request.Cookies("SUP_VTDesc").Value)
                    ViewState("sDeptID") = Server.HtmlEncode(Request.Cookies("SUP_VTDesc").Value)
                End If

                If Not Request.Cookies("SUP_Loc") Is Nothing Then
                    ddUGNLocation.SelectedValue = Server.HtmlEncode(Request.Cookies("SUP_Loc").Value)
                    ViewState("sLoc") = Server.HtmlEncode(Request.Cookies("SUP_Loc").Value)
                End If

                If Not Request.Cookies("SUP_RSTAT") Is Nothing Then
                    ddStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("SUP_RSTAT").Value)
                    ViewState("sRStat") = Server.HtmlEncode(Request.Cookies("SUP_RSTAT").Value)
                End If

                If Not Request.Cookies("SUP_DSF") Is Nothing Then
                    txtDateSubFrom.Text = Server.HtmlEncode(Request.Cookies("SUP_DSF").Value)
                    ViewState("sDSF") = Server.HtmlEncode(Request.Cookies("SUP_DSF").Value)
                End If

                If Not Request.Cookies("SUP_DST") Is Nothing Then
                    txtDateSubTo.Text = Server.HtmlEncode(Request.Cookies("SUP_DST").Value)
                    ViewState("sDST") = Server.HtmlEncode(Request.Cookies("SUP_DST").Value)
                End If

                
                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sSUPNo") = txtSUPNo.Text
                ViewState("sRBID") = ddRequestedBy.SelectedValue
                ViewState("sVendor") = txtVendor.Text
                ViewState("sSName") = txtVendorName.Text
                ViewState("sPDesc") = txtProdDesc.Text
                ViewState("sVTYPE") = ddVendorType.SelectedValue
                ViewState("sVTDesc") = ddVTypeDesc.SelectedValue
                ViewState("sLoc") = ddUGNLocation.SelectedValue
                ViewState("sRStat") = ddStatus.SelectedValue
                ViewState("sDSF") = txtDateSubFrom.Text
                ViewState("sDST") = txtDateSubTo.Text
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
            ''bind existing data to drop down Requested By control for selection criteria for search
            ds = commonFunctions.GetTeamMember("") '**SubscriptionID # is used for Project Leader
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddRequestedBy.DataSource = ds
                ddRequestedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddRequestedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddRequestedBy.DataBind()
                ddRequestedBy.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Ship To control for selection criteria for search
            ds = commonFunctions.GetUGNFacility("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddUGNLocation.DataSource = ds
                ddUGNLocation.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNLocation.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
                ddUGNLocation.DataBind()
                ddUGNLocation.Items.Insert(0, "")
            End If


            ''bind existing data to drop down Vendor Type control for selection criteria for search
            ds = commonFunctions.GetVendorType(True)
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

    Private Sub BindData()
        Try
            lblErrors.Text = ""
            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = SUPModule.GetSupplierRequestSearch(ViewState("sSUPNo"), IIf(ViewState("sRBID") = Nothing, 0, ViewState("sRBID")), ViewState("sVendor"), ViewState("sSName"), ViewState("sPDesc"), ViewState("sVTYPE"), ViewState("sVTDesc"), ViewState("sLoc"), ViewState("sRStat"), ViewState("sDSF"), ViewState("sDST"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    rpSUP.DataSource = ds
                    rpSUP.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpSUP.DataSource = objPds
                    rpSUP.DataBind()

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                    ViewState("LastPageCount") = objPds.PageCount - 1
                    txtGoToPage.Text = CurrentPage + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirst.Enabled = Not objPds.IsFirstPage
                    cmdPrev.Enabled = Not objPds.IsFirstPage
                    cmdNext.Enabled = Not objPds.IsLastPage
                    cmdLast.Enabled = Not objPds.IsLastPage

                    ' Display # of records
                    If (CurrentPage + 1) > 1 Then
                        lblFromRec.Text = (((CurrentPage + 1) * 30) - 30) + 1
                        lblToRec.Text = (CurrentPage + 1) * 30
                        If lblToRec.Text > objPds.DataSourceCount Then
                            lblToRec.Text = objPds.DataSourceCount
                        End If
                    Else
                        lblFromRec.Text = ds.Tables.Count
                        lblToRec.Text = rpSUP.Items.Count
                    End If
                    lblTotalRecords.Text = objPds.DataSourceCount
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

    End Sub 'EOF of BindData

#Region "Paging Routine"
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

    End Property 'EOF CurrentPage

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionSupCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdPrev_Click

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionSupCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdNext_Click

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionSupCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub 'EOF cmdFirst_Click

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionSupCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdGo_Click

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionSupCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdLast_Click

#End Region 'EOF Paging Routine

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        '* Redirect users to search for a vendor prior to creating/submitting a new supplier request entry
        Response.Redirect("SupplierLookUp.aspx?sBtnSrch=False&pForm=SUPPLIER", False)

    End Sub 'EOF btnAdd_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("SUP_SUPNo").Value = txtSUPNo.Text
            Response.Cookies("SUP_RBID").Value = ddRequestedBy.SelectedValue
            Response.Cookies("SUP_Vendor").Value = txtVendor.Text
            Response.Cookies("SUP_SName").Value = txtVendorName.Text
            Response.Cookies("SUP_PDesc").Value = txtProdDesc.Text
            Response.Cookies("SUP_VTYPE").Value = ddVendorType.SelectedValue
            Response.Cookies("SUP_VTDesc").Value = ddVTypeDesc.SelectedValue
            Response.Cookies("SUP_Loc").Value = ddUGNLocation.SelectedValue
            Response.Cookies("SUP_RSTAT").Value = ddStatus.SelectedValue
            Response.Cookies("SUP_DSF").Value = txtDateSubFrom.Text
            Response.Cookies("SUP_DST").Value = txtDateSubTo.Text

            ' Set viewstate variable to the first page
            CurrentPage = 0

            ' Reload control
            BindData()

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            SUPModule.DeleteSupplierRequestCookies()
            HttpContext.Current.Session("sessionSupCurrentPage") = Nothing

            Response.Redirect("SupplierRequestList.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Public Function ShowHideHistory(ByVal ProjectStatus As String) As Boolean
        Return True
    End Function 'EOF ShowHideHistory


    Protected Function SetTextColor(ByVal RoutingStatus As String) As Color

        Dim strReturnValue As Color = Color.Black

        Select Case RoutingStatus
            Case "A"
                strReturnValue = Color.Black
            Case "C"
                strReturnValue = Color.Black
            Case "N"
                strReturnValue = Color.Black
            Case "T"
                strReturnValue = Color.Black
            Case "R"
                strReturnValue = Color.White
            Case "V"
                strReturnValue = Color.White
        End Select

        SetTextColor = strReturnValue

    End Function 'EOF SetTextColor

    Protected Function SetBackGroundColor(ByVal RoutingStatus As String) As String

        Dim strReturnValue As String = "White"

        Select Case RoutingStatus
            Case "A"
                strReturnValue = "Lime"
            Case "C"
                strReturnValue = "White'"
            Case "N"
                strReturnValue = "Fuchsia"
            Case "T"
                strReturnValue = "Yellow"
            Case "R"
                strReturnValue = "Red"
            Case "V"
                strReturnValue = "Gray"
        End Select

        SetBackGroundColor = strReturnValue

    End Function 'EOF SetBackGroundColor

End Class
