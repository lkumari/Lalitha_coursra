' ************************************************************************************************
'
' Name:		Material_List.aspx
' Purpose:	This Code Behind is to maintain the catching ability factor used by the Costing Module
'
' Date		Author	    
' 10/14/2008 RCarlson
' 08/26/2010 RCarlson   added isActiveBPCSOnly Parameter to GetUGNDBVendor
' 01/03/2014 LREY       Replaced "BPCSPartNo" to PartNo, SoldTo/CABBV to Customer, Vendor to Supplier wherever used.
' 19/12/2014 LMeka      Add UNGFacilityCode  
' ************************************************************************************************
Partial Class Costing_Material_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkMaterialID As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkMaterialName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDrawingNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartRevision As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUGNDBVendorName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkStandardCost As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkQuoteCost As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkFreightCost As System.Web.UI.WebControls.LinkButton

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblMessage.Text = ""

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Material/Packaging List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > Material/Packaging List "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            CheckRights()

            If HttpContext.Current.Session("sessionMaterialCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionMaterialCurrentPage")
            End If

            If Not Page.IsPostBack Then

                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                ViewState("MaterialID") = ""
                ViewState("PartName") = ""
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""

                ViewState("UGNDBVendorID") = 0
                ViewState("PurchasedGoodID") = 0

                ViewState("OldMaterialGroup") = ""

                ViewState("isPackaging") = 0
                ViewState("filterPackaging") = 0
                ViewState("isCoating") = 0
                ViewState("filterCoating") = 0
                ViewState("Obsolete") = 0
                ViewState("filterObsolete") = 0

                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******
                If HttpContext.Current.Request.QueryString("MaterialID") <> "" Then
                    txtSearchMaterialIDValue.Text = HttpContext.Current.Request.QueryString("MaterialID")
                    ViewState("MaterialID") = HttpContext.Current.Request.QueryString("MaterialID")
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialIDSearch").Value) <> "" Then
                            txtSearchMaterialIDValue.Text = Request.Cookies("CostingModule_SaveMaterialIDSearch").Value
                            ViewState("MaterialID") = Request.Cookies("CostingModule_SaveMaterialIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartNameValue.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialPartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialPartNameSearch").Value) <> "" Then
                            txtSearchPartNameValue.Text = Request.Cookies("CostingModule_SaveMaterialPartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("CostingModule_SaveMaterialPartNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNoValue.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Value) <> "" Then
                            txtSearchDrawingNoValue.Text = Request.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNoValue.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialPartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialPartNoSearch").Value) <> "" Then
                            txtSearchPartNoValue.Text = Request.Cookies("CostingModule_SaveMaterialPartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("CostingModule_SaveMaterialPartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNDBVendorID") <> "" Then
                    If HttpContext.Current.Request.QueryString("UGNDBVendorID") > 0 Then
                        ddSearchVendorValue.SelectedValue = HttpContext.Current.Request.QueryString("UGNDBVendorID")
                        ViewState("UGNDBVendorID") = HttpContext.Current.Request.QueryString("UGNDBVendorID")
                    End If
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value) <> "" Then
                            If CType(Request.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value, Integer) > 0 Then
                                ddSearchVendorValue.SelectedValue = Request.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value
                                ViewState("UGNDBVendorID") = CType(Request.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value, Integer)
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchasedGoodID") <> "" Then
                    If HttpContext.Current.Request.QueryString("PurchasedGoodID") > 0 Then
                        ddSearchPurchasedGoodValue.SelectedValue = HttpContext.Current.Request.QueryString("PurchasedGoodID")
                        ViewState("PurchasedGoodID") = HttpContext.Current.Request.QueryString("PurchasedGoodID")
                    End If
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value) <> "" Then
                            If CType(Request.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value, Integer) > 0 Then
                                ddSearchPurchasedGoodValue.SelectedValue = Request.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value
                                ViewState("PurchasedGoodID") = CType(Request.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value, Integer)
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("OldMaterialGroup") <> "" Then
                    txtSearchOldMaterialGroupValue.Text = HttpContext.Current.Request.QueryString("OldMaterialGroup")
                    ViewState("OldMaterialGroup") = HttpContext.Current.Request.QueryString("OldMaterialGroup")
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch") Is Nothing Then
                        If Trim(Request.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Value) <> "" Then
                            txtSearchOldMaterialGroupValue.Text = Request.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Value
                            ViewState("OldMaterialGroup") = Request.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("isPackaging") <> "" Then
                    ViewState("isPackaging") = CType(HttpContext.Current.Request.QueryString("isPackaging"), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialIsPackagingSearch") Is Nothing Then
                        ViewState("isPackaging") = CType(Request.Cookies("CostingModule_SaveMaterialIsPackagingSearch").Value, Integer)
                    End If
                End If

                If HttpContext.Current.Request.QueryString("filterPackaging") <> "" Then
                    ViewState("filterPackaging") = CType(HttpContext.Current.Request.QueryString("filterPackaging"), Integer)

                    'If ViewState("filterPackaging") > 0 And ViewState("isPackaging") > 0 Then
                    '    ddSearchPackaging.SelectedValue = "Only"
                    'End If

                    'If ViewState("filterPackaging") > 0 And ViewState("isPackaging") = 0 Then
                    '    ddSearchPackaging.SelectedValue = "None"
                    'End If
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialFilterPackagingSearch") Is Nothing Then
                        ViewState("filterPackaging") = CType(Request.Cookies("CostingModule_SaveMaterialFilterPackagingSearch").Value, Integer)
                    End If
                End If

                If ViewState("filterPackaging") > 0 And ViewState("isPackaging") > 0 Then
                    ddSearchPackaging.SelectedValue = "Only"
                End If

                If ViewState("filterPackaging") > 0 And ViewState("isPackaging") = 0 Then
                    ddSearchPackaging.SelectedValue = "None"
                End If

                If HttpContext.Current.Request.QueryString("isCoating") <> "" Then
                    ViewState("isCoating") = CType(HttpContext.Current.Request.QueryString("isCoating"), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialIsCoatingSearch") Is Nothing Then
                        ViewState("isCoating") = CType(Request.Cookies("CostingModule_SaveMaterialIsCoatingSearch").Value, Integer)
                    End If
                End If

                If HttpContext.Current.Request.QueryString("filterCoating") <> "" Then
                    ViewState("filterCoating") = CType(HttpContext.Current.Request.QueryString("filterCoating"), Integer)

                    'If ViewState("filterCoating") > 0 And ViewState("isCoating") > 0 Then
                    '    ddSearchCoating.SelectedValue = "Only"
                    'End If

                    'If ViewState("filterCoating") > 0 And ViewState("isCoating") = 0 Then
                    '    ddSearchCoating.SelectedValue = "None"
                    'End If
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialFilterCoatingSearch") Is Nothing Then
                        ViewState("filterCoating") = CType(Request.Cookies("CostingModule_SaveMaterialFilterCoatingSearch").Value, Integer)
                    End If
                End If

                If ViewState("filterCoating") > 0 And ViewState("isCoating") > 0 Then
                    ddSearchCoating.SelectedValue = "Only"
                End If

                If ViewState("filterCoating") > 0 And ViewState("isCoating") = 0 Then
                    ddSearchCoating.SelectedValue = "None"
                End If

                If HttpContext.Current.Request.QueryString("Obsolete") <> "" Then
                    ViewState("Obsolete") = CType(HttpContext.Current.Request.QueryString("Obsolete"), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialObsoleteSearch") Is Nothing Then
                        ViewState("Obsolete") = CType(Request.Cookies("CostingModule_SaveMaterialObsoleteSearch").Value, Integer)
                    End If
                End If

                If HttpContext.Current.Request.QueryString("filterObsolete") <> "" Then
                    ViewState("filterObsolete") = CType(HttpContext.Current.Request.QueryString("filterObsolete"), Integer)
                Else
                    If Not Request.Cookies("CostingModule_SaveMaterialFilterObsoleteSearch") Is Nothing Then
                        ViewState("filterObsolete") = CType(Request.Cookies("CostingModule_SaveMaterialFilterObsoleteSearch").Value, Integer)
                    End If
                End If

                If ViewState("filterObsolete") > 0 And ViewState("Obsolete") > 0 Then
                    ddSearchObsolete.SelectedValue = "Only"
                End If

                If ViewState("filterObsolete") > 0 And ViewState("Obsolete") = 0 Then
                    ddSearchObsolete.SelectedValue = "None"
                End If

                BindData()
            Else
                ViewState("MaterialID") = txtSearchMaterialIDValue.Text.Trim
                ViewState("PartName") = txtSearchPartNameValue.Text.Trim
                ViewState("DrawingNo") = txtSearchDrawingNoValue.Text.Trim
                ViewState("PartNo") = txtSearchPartNoValue.Text.Trim

                If ddSearchVendorValue.SelectedIndex > 0 Then
                    ViewState("UGNDBVendorID") = ddSearchVendorValue.SelectedValue
                Else
                    ViewState("UGNDBVendorID") = 0
                End If

                If ddSearchPurchasedGoodValue.SelectedIndex > 0 Then
                    ViewState("PurchasedGoodID") = ddSearchPurchasedGoodValue.SelectedValue
                Else
                    ViewState("PurchasedGoodID") = 0
                End If

                If ddSearchUGNFacilityCode.SelectedIndex > 0 Then
                    ViewState("UGNFacilityCode") = ddSearchUGNFacilityCode.SelectedValue
                Else
                    ViewState("UGNFacilityCode") = 0
                End If

                ViewState("OldMaterialGroup") = txtSearchOldMaterialGroupValue.Text.Trim

                ViewState("isCoating") = 0
                ViewState("filterCoating") = 0

                If ddSearchCoating.SelectedIndex > 0 Then
                    If ddSearchCoating.SelectedValue = "Only" Then
                        ViewState("isCoating") = 1
                        ViewState("filterCoating") = 1
                    End If

                    If ddSearchCoating.SelectedValue = "None" Then
                        ViewState("isCoating") = 0
                        ViewState("filterCoating") = 1
                    End If
                End If

                ViewState("isPackaging") = 0
                ViewState("filterPackaging") = 0

                If ddSearchPackaging.SelectedIndex > 0 Then
                    If ddSearchPackaging.SelectedValue = "Only" Then
                        ViewState("isPackaging") = 1
                        ViewState("filterPackaging") = 1
                    End If

                    If ddSearchPackaging.SelectedValue = "None" Then
                        ViewState("isPackaging") = 0
                        ViewState("filterPackaging") = 1
                    End If
                End If

                ViewState("Obsolete") = 0
                ViewState("filterObsolete") = 0

                If ddSearchObsolete.SelectedIndex > 0 Then
                    If ddSearchObsolete.SelectedValue = "Only" Then
                        ViewState("Obsolete") = 1
                        ViewState("filterObsolete") = 1
                    End If

                    If ddSearchObsolete.SelectedValue = "None" Then
                        ViewState("Obsolete") = 0
                        ViewState("filterObsolete") = 1
                    End If
                End If
            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing data to repeater control at bottom of screen                       
            ds = CostingModule.GetMaterial(ViewState("MaterialID"), ViewState("PartName"), ViewState("PartNo"), ViewState("DrawingNo"), _
            ViewState("UGNDBVendorID"), ViewState("PurchasedGoodID"), ViewState("UGNFacilityCode"), ("OldMaterialGroup"), _
            ViewState("isPackaging"), ViewState("filterPackaging"), _
            ViewState("isCoating"), ViewState("filterCoating"), _
            ViewState("Obsolete"), ViewState("filterObsolete"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then

                    ' Create a DataView from the DataTable.
                    Dim dv As DataView = New DataView(ds.Tables(0))

                    'Enforce the sort on the dataview
                    dv.Sort = SortOrder

                    'Set the DataGrid's Source and bind it.
                    rpMaterial.DataSource = dv
                    rpMaterial.DataBind()

                    'Dispose Items
                    ds.Dispose()
                    dv.Dispose()
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkMaterialID.Click, lnkMaterialName.Click, lnkDrawingNo.Click, lnkPartNo.Click, lnkPartRevision.Click, lnkUGNDBVendorName.Click, lnkStandardCost.Click, lnkQuoteCost.Click, lnkFreightCost.Click

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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionMaterialCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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


                HttpContext.Current.Session("sessionMaterialCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionMaterialCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionMaterialCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        lblMessage.Text = ""

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionMaterialCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

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

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 75)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = True
                            End Select
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub EnableControls()

        Try

            rpMaterial.Visible = Not ViewState("isRestricted")
            lblSearchTip.Visible = Not ViewState("isRestricted")

            lblSearchMaterialIDLabel.Visible = Not ViewState("isRestricted")
            txtSearchMaterialIDValue.Visible = Not ViewState("isRestricted")

            lblSearchPartNameLabel.Visible = Not ViewState("isRestricted")
            txtSearchPartNameValue.Visible = Not ViewState("isRestricted")

            lblSearchPartNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchPartNoValue.Visible = Not ViewState("isRestricted")

            lblSearchDrawingNoLabel.Visible = Not ViewState("isRestricted")
            txtSearchDrawingNoValue.Visible = Not ViewState("isRestricted")

            lblSearchVendorLabel.Visible = Not ViewState("isRestricted")
            ddSearchVendorValue.Visible = Not ViewState("isRestricted")

            lblSearchPurchasedGoodLabel.Visible = Not ViewState("isRestricted")
            ddSearchPurchasedGoodValue.Visible = Not ViewState("isRestricted")

            lblSearchOldMaterialGroupLabel.Visible = Not ViewState("isRestricted")
            txtSearchOldMaterialGroupValue.Visible = Not ViewState("isRestricted")

            btnReset.Visible = Not ViewState("isRestricted")
            btnSearch.Visible = Not ViewState("isRestricted")
            lblReview1.Visible = Not ViewState("isRestricted")
            lblReview2.Visible = Not ViewState("isRestricted")
            btnAdd.Visible = Not ViewState("isRestricted")

            cmdFirst.Visible = Not ViewState("isRestricted")
            cmdNext.Visible = Not ViewState("isRestricted")
            txtGoToPage.Visible = Not ViewState("isRestricted")
            cmdGo.Visible = Not ViewState("isRestricted")
            cmdPrev.Visible = Not ViewState("isRestricted")
            cmdLast.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                btnAdd.Enabled = ViewState("isAdmin")

                'gvMaterial.Columns(gvMaterial.Columns.Count - 1).Visible = ViewState("isAdmin")
                'If gvMaterial.FooterRow IsNot Nothing Then
                '    gvMaterial.FooterRow.Visible = ViewState("isAdmin")
                'End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing data to drop down PurchasedGood 
            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchPurchasedGoodValue.DataSource = ds
                ddSearchPurchasedGoodValue.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddSearchPurchasedGoodValue.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddSearchPurchasedGoodValue.DataBind()
                ddSearchPurchasedGoodValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Vendor 
            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchVendorValue.DataSource = ds
                ddSearchVendorValue.DataTextField = ds.Tables(0).Columns("ddSupplierName").ColumnName.ToString()
                ddSearchVendorValue.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddSearchVendorValue.DataBind()
                ddSearchVendorValue.Items.Insert(0, "")
            End If

            ''bind existing data to drop down UGN Facility Code
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacilityCode.DataSource = ds
                ddSearchUGNFacilityCode.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacilityCode.DataValueField = ds.Tables(0).Columns("UGNFacilityCode").ColumnName
                ddSearchUGNFacilityCode.DataBind()
                ddSearchUGNFacilityCode.Items.Insert(0, "")

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindData()

        Try

            Dim ds As DataSet = New DataSet

            'bind existing data to repeater control at bottom of screen                       
            ds = CostingModule.GetMaterial(ViewState("MaterialID"), ViewState("PartName"), ViewState("PartNo"), ViewState("DrawingNo"), _
             ViewState("UGNDBVendorID"), ViewState("PurchasedGoodID"), ViewState("UGNFacilityCode"), ViewState("OldMaterialGroup"), _
             ViewState("isPackaging"), ViewState("filterPackaging"), _
             ViewState("isCoating"), ViewState("filterCoating"), _
             ViewState("Obsolete"), ViewState("filterObsolete"))

            If ViewState("isRestricted") = False Then
                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                        rpMaterial.DataSource = ds
                        rpMaterial.DataBind()

                        ' Populate the repeater control with the Items DataSet
                        Dim objPds As PagedDataSource = New PagedDataSource
                        objPds.DataSource = ds.Tables(0).DefaultView

                        ' Indicate that the data should be paged
                        objPds.AllowPaging = True

                        ' Set the number of items you wish to display per page
                        objPds.PageSize = 30

                        ' Set the PagedDataSource's current page
                        objPds.CurrentPageIndex = CurrentPage

                        rpMaterial.DataSource = objPds
                        rpMaterial.DataBind()

                        lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                        ViewState("LastPageCount") = objPds.PageCount - 1
                        txtGoToPage.Text = CurrentPage + 1

                        ' Disable Prev or Next buttons if necessary
                        cmdFirst.Enabled = Not objPds.IsFirstPage
                        cmdPrev.Enabled = Not objPds.IsFirstPage
                        cmdNext.Enabled = Not objPds.IsLastPage
                        cmdLast.Enabled = Not objPds.IsLastPage
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod


            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            HttpContext.Current.Session("sessionMaterialCurrentPage") = Nothing

            ViewState("MaterialID") = txtSearchMaterialIDValue.Text.Trim
            Response.Cookies("CostingModule_SaveMaterialIDSearch").Value = ViewState("MaterialID")

            ViewState("PartName") = txtSearchPartNameValue.Text.Trim
            Response.Cookies("CostingModule_SaveMaterialPartNameSearch").Value = ViewState("PartName")

            ViewState("DrawingNo") = txtSearchDrawingNoValue.Text.Trim
            Response.Cookies("CostingModule_SaveMaterialDrawingNoSearch").Value = ViewState("DrawingNo")

            ViewState("PartNo") = txtSearchPartNoValue.Text.Trim
            Response.Cookies("CostingModule_SaveMaterialPartNoSearch").Value = ViewState("PartNo")

            If ddSearchVendorValue.SelectedIndex > 0 Then
                ViewState("UGNDBVendorID") = ddSearchVendorValue.SelectedValue
            Else
                ViewState("UGNDBVendorID") = 0
            End If
            Response.Cookies("CostingModule_SaveMaterialUGNDBVendorIDSearch").Value = ViewState("UGNDBVendorID")

            If ddSearchPurchasedGoodValue.SelectedIndex > 0 Then
                ViewState("PurchasedGoodID") = ddSearchPurchasedGoodValue.SelectedValue
            Else
                ViewState("PurchasedGoodID") = 0
            End If
            Response.Cookies("CostingModule_SaveMaterialPurchasedGoodIDSearch").Value = ViewState("PurchasedGoodID")

            ViewState("OldMaterialGroup") = txtSearchOldMaterialGroupValue.Text.Trim
            Response.Cookies("CostingModule_SaveMaterialOldMaterialGroupSearch").Value = ViewState("OldMaterialGroup")

            ViewState("isCoating") = 0
            ViewState("filterCoating") = 0

            If ddSearchCoating.SelectedIndex > 0 Then
                If ddSearchCoating.SelectedValue = "Only" Then
                    ViewState("isCoating") = 1
                    ViewState("filterCoating") = 1
                End If

                If ddSearchCoating.SelectedValue = "None" Then
                    ViewState("isCoating") = 0
                    ViewState("filterCoating") = 1
                End If
            End If
            Response.Cookies("CostingModule_SaveMaterialIsCoatingSearch").Value = ViewState("isCoating")
            Response.Cookies("CostingModule_SaveMaterialFilterCoatingSearch").Value = ViewState("filterCoating")

            ViewState("isPackaging") = 0
            ViewState("filterPackaging") = 0

            If ddSearchPackaging.SelectedIndex > 0 Then
                If ddSearchPackaging.SelectedValue = "Only" Then
                    ViewState("isPackaging") = 1
                    ViewState("filterPackaging") = 1
                End If

                If ddSearchPackaging.SelectedValue = "None" Then
                    ViewState("isPackaging") = 0
                    ViewState("filterPackaging") = 1
                End If
            End If
            Response.Cookies("CostingModule_SaveMaterialIsPackagingSearch").Value = ViewState("isPackaging")
            Response.Cookies("CostingModule_SaveMaterialFilterPackagingSearch").Value = ViewState("filterPackaging")

            ViewState("Obsolete") = 0
            ViewState("filterObsolete") = 0

            If ddSearchObsolete.SelectedIndex > 0 Then
                If ddSearchObsolete.SelectedValue = "Only" Then
                    ViewState("Obsolete") = 1
                    ViewState("filterObsolete") = 1
                End If

                If ddSearchObsolete.SelectedValue = "None" Then
                    ViewState("Obsolete") = 0
                    ViewState("filterObsolete") = 1
                End If
            End If
            Response.Cookies("CostingModule_SaveMaterialObsoleteSearch").Value = ViewState("Obsolete")
            Response.Cookies("CostingModule_SaveMaterialFilterObsoleteSearch").Value = ViewState("filterObsolete")

            Response.Redirect("Material_List.aspx?MaterialID=" & Server.UrlEncode(ViewState("MaterialID")) & _
            "&PartName=" & Server.UrlEncode(ViewState("PartName")) & _
            "&DrawingNo=" & Server.UrlEncode(ViewState("DrawingNo")) & _
            "&PartNo=" & Server.UrlEncode(ViewState("PartNo")) & _
            "&UGNDBVendorID=" & ViewState("UGNDBVendorID") & _
            "&PurchasedGoodID=" & ViewState("PurchasedGoodID") & _
            "&UGNFacilityCode=" & ViewState("UGNFacilityCode") & _
            "&OldMaterialGroup=" & ViewState("OldMaterialGroup") & _
            "&isCoating= " & ViewState("isCoating") & _
            "&filterCoating= " & ViewState("filterCoating") & _
            "&isPackaging= " & ViewState("isPackaging") & _
            "&filterPackaging= " & ViewState("filterPackaging") & _
            "&Obsolete= " & ViewState("Obsolete") & _
            "&filterObsolete= " & ViewState("filterObsolete"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            CostingModule.DeleteMaterialCookies()

            HttpContext.Current.Session("sessionMaterialCurrentPage") = Nothing

            Response.Redirect("Material_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            Response.Redirect("Material_Maint.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    
 
End Class
