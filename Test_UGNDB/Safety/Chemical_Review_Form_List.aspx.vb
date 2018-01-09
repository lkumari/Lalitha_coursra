' ************************************************************************************************
'
' Name:		Safety_Chemical_Review_Form_List.aspx
' Purpose:	This Code Behind is for the Chemical Review Form List / Search page
'
' Date		    Author	    
' 01/11/2010    Roderick Carlson    Created
' 02/28/2011    Roderick Carlson    Modified - Added isActive Dropdown 
' ************************************************************************************************

Partial Class Safety_Chemical_Review_Form_List
    Inherits System.Web.UI.Page
    Protected WithEvents lnkStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkChemicalReviewFormID As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRequestDate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkProductName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRnDStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkHRSafetyStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCorpEnvStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPlantEnvStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPurchasingStatusName As System.Web.UI.WebControls.LinkButton

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = SafetyModule.GetChemicalReviewForm(ViewState("ChemRevFormID"), ViewState("StatusID"), ViewState("UGNFacility"), _
            ViewState("RequestedByTeamMemberID"), ViewState("RequestDateStart"), ViewState("RequestDateEnd"), _
            ViewState("ApprovingTeamMemberID"), ViewState("ProductName"), ViewState("ProductManufacturer"), _
            ViewState("PurchaseFrom"), ViewState("DeptArea"), ViewState("ChemicalDesc"), ViewState("filterActive"), ViewState("isActive"))

            If commonFunctions.CheckDataset(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpChemicalReviewFormInfo.DataSource = dv
                rpChemicalReviewFormInfo.DataBind()

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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkStatusName.Click, lnkChemicalReviewFormID.Click, lnkRequestDate.Click, lnkProductName.Click, lnkRnDStatusName.Click, lnkHRSafetyStatusName.Click, lnkCorpEnvStatusName.Click, lnkPlantEnvStatusName.Click, lnkPurchasingStatusName.Click

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

    Protected Function SetPreviewVisible(ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            If StatusID <> "4" Then
                bReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewVisible = bReturnValue

    End Function
    Protected Function SetPreviewFormHyperLink(ByVal ChemRevFormID As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = ""

        Try
            If ChemRevFormID <> "" And StatusID <> "4" Then
                strReturnValue = "javascript:void(window.open('Chemical_Review_Form_Preview.aspx?ChemRevFormID=" & ChemRevFormID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=600,width=950,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewFormHyperLink = strReturnValue

    End Function

    Protected Function SetFormHyperlink(ByVal ChemRevFormID As String) As String

        Dim strReturnValue As String = ""

        Try
            strReturnValue = "Chemical_Review_Form_Detail.aspx?ChemRevFormID=" & ChemRevFormID

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetFormHyperlink = strReturnValue

    End Function

    Protected Function SetBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White" 'N/A or 3-complete 

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2" 'in-process
                    strReturnValue = "Yellow"
                Case "4" 'void
                    strReturnValue = "Gray"
                Case "5" 'rejected
                    strReturnValue = "Red"
                Case "6" 'tasked
                    strReturnValue = "Aqua"
                Case "7" 'on-hold
                    strReturnValue = "Blue"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black" 'default

        Try
            Select Case StatusID
                'Case "1" 'open
                '    strReturnValue = "Black"
                'Case "2" 'in-process
                '    strReturnValue = "Black"
                Case "4" 'void
                    strReturnValue = "White"
                Case "5" 'rejected
                    strReturnValue = "White"
                    'Case "6" 'tasked
                    '    strReturnValue = "Black"
                Case "7" 'on-hold
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetForeGroundColor = strReturnValue

    End Function

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

            ViewState("isAdmin") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 96)

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
                            ViewState("isAdmin") = True

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
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

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ds = SafetyModule.GetChemicalReviewFormApprovers()
            If commonFunctions.CheckDataset(ds) = True Then
                ddApprover.DataSource = ds
                ddApprover.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddApprover.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddApprover.DataBind()
                ddApprover.Items.Insert(0, "")
            End If

            ds = SafetyModule.GetChemicalReviewFormRequestedByTeamMembers()
            If commonFunctions.CheckDataset(ds) = True Then
                ddRequestedByTeamMember.DataSource = ds
                ddRequestedByTeamMember.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
                ddRequestedByTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
                ddRequestedByTeamMember.DataBind()
                ddRequestedByTeamMember.Items.Insert(0, "")
            End If

            ds = SafetyModule.GetChemicalReviewFormStatus(0, False)
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

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = SafetyModule.GetChemicalReviewForm(ViewState("ChemRevFormID"), ViewState("StatusID"), ViewState("UGNFacility"), _
            ViewState("RequestedByTeamMemberID"), ViewState("RequestDateStart"), ViewState("RequestDateEnd"), _
            ViewState("ApprovingTeamMemberID"), ViewState("ProductName"), ViewState("ProductManufacturer"), _
            ViewState("PurchaseFrom"), ViewState("DeptArea"), ViewState("ChemicalDesc"), _
            ViewState("filterActive"), ViewState("isActive"))

            If commonFunctions.CheckDataset(ds) = True Then
                rpChemicalReviewFormInfo.DataSource = ds
                rpChemicalReviewFormInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpChemicalReviewFormInfo.DataSource = objPds
                rpChemicalReviewFormInfo.DataBind()

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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub EnableControls()

        Try

            btnAdd.Enabled = ViewState("isAdmin")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Chemical Review Form - List"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab - Safety</b> > Chemical Review Form List and Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("RnDExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage")
            End If

            'clear crystal reports
            SafetyModule.CleanChemicalReviewFormCrystalReports()

            If Not Page.IsPostBack Then

                ViewState("lnkStatusName") = "DESC"
                ViewState("lnkChemicalReviewFormID") = "ASC"
                ViewState("lnkRequestDate") = "ASC"
                ViewState("lnkProductName") = "ASC"
                ViewState("lnkRnDStatusName") = "ASC"
                ViewState("lnkHRSafetyStatusName") = "ASC"
                ViewState("lnkCorpEnvStatusName") = "ASC"
                ViewState("lnkPlantEnvStatusName") = "ASC"
                ViewState("lnkPurchasingStatusName") = "ASC"

                ViewState("ChemRevFormID") = 0                
                ViewState("StatusID") = 0
                ViewState("UGNFacility") = ""
                ViewState("RequestedByTeamMemberID") = 0
                ViewState("RequestDateStart") = ""
                ViewState("RequestDateEnd") = ""
                ViewState("ApprovingTeamMemberID") = 0
                ViewState("ProductName") = ""
                ViewState("ProductManufacturer") = ""
                ViewState("PurchaseFrom") = ""
                ViewState("DeptArea") = ""
                ViewState("ChemicalDesc") = ""
                ViewState("isActive") = 0
                ViewState("filterActive") = 0

                '' ''******
                '' '' Bind drop down lists
                '' ''******
                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("ChemRevFormID") <> "" Then
                    txtChemRevFormID.Text = HttpContext.Current.Request.QueryString("ChemRevFormID")
                    ViewState("ChemRevFormID") = HttpContext.Current.Request.QueryString("ChemRevFormID")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormIDSearch").Value) <> "" Then
                            txtChemRevFormID.Text = Request.Cookies("SafetyModule_SaveChemRevFormIDSearch").Value
                            ViewState("ChemRevFormID") = Request.Cookies("SafetyModule_SaveChemRevFormIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("StatusID") <> "" Then
                    ddStatus.SelectedValue = HttpContext.Current.Request.QueryString("StatusID")
                    ViewState("StatusID") = HttpContext.Current.Request.QueryString("StatusID")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value) <> "" Then
                            ddStatus.SelectedValue = Request.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value
                            ViewState("StatusID") = Request.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacility.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
                    ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value) <> "" Then
                            ddUGNFacility.SelectedValue = Request.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RequestedByTeamMemberID") <> "" Then
                    ddRequestedByTeamMember.SelectedValue = HttpContext.Current.Request.QueryString("RequestedByTeamMemberID")
                    ViewState("RequestedByTeamMemberID") = HttpContext.Current.Request.QueryString("RequestedByTeamMemberID")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value) <> "" Then
                            ddRequestedByTeamMember.SelectedValue = Request.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value
                            ViewState("RequestedByTeamMemberID") = Request.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RequestDateStart") <> "" Then
                    txtRequestDateStart.Text = HttpContext.Current.Request.QueryString("RequestDateStart")
                    ViewState("RequestDateStart") = HttpContext.Current.Request.QueryString("RequestDateStart")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Value) <> "" Then
                            txtRequestDateStart.Text = Request.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Value
                            ViewState("RequestDateStart") = Request.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("RequestDateEnd") <> "" Then
                    txtRequestDateEnd.Text = HttpContext.Current.Request.QueryString("RequestDateEnd")
                    ViewState("RequestDateEnd") = HttpContext.Current.Request.QueryString("RequestDateEnd")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Value) <> "" Then
                            txtRequestDateEnd.Text = Request.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Value
                            ViewState("RequestDateEnd") = Request.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ApprovingTeamMemberID") <> "" Then
                    ddApprover.SelectedValue = HttpContext.Current.Request.QueryString("ApprovingTeamMemberID")
                    ViewState("ApprovingTeamMemberID") = HttpContext.Current.Request.QueryString("ApprovingTeamMemberID")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value) <> "" Then
                            ddApprover.SelectedValue = Request.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value
                            ViewState("ApprovingTeamMemberID") = Request.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProductName") <> "" Then
                    txtProductName.Text = HttpContext.Current.Request.QueryString("ProductName")
                    ViewState("ProductName") = HttpContext.Current.Request.QueryString("ProductName")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormProductNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Value) <> "" Then
                            txtProductName.Text = Request.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Value
                            ViewState("ProductName") = Request.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProductManufacturer") <> "" Then
                    txtProductManufacturer.Text = HttpContext.Current.Request.QueryString("ProductManufacturer")
                    ViewState("ProductManufacturer") = HttpContext.Current.Request.QueryString("ProductManufacturer")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Value) <> "" Then
                            txtProductManufacturer.Text = Request.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Value
                            ViewState("ProductManufacturer") = Request.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchaseFrom") <> "" Then
                    txtPurchaseFrom.Text = HttpContext.Current.Request.QueryString("PurchaseFrom")
                    ViewState("PurchaseFrom") = HttpContext.Current.Request.QueryString("PurchaseFrom")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Value) <> "" Then
                            txtPurchaseFrom.Text = Request.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Value
                            ViewState("PurchaseFrom") = Request.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DeptArea") <> "" Then
                    txtDeptArea.Text = HttpContext.Current.Request.QueryString("DeptArea")
                    ViewState("DeptArea") = HttpContext.Current.Request.QueryString("DeptArea")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Value) <> "" Then
                            txtDeptArea.Text = Request.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Value
                            ViewState("DeptArea") = Request.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ChemicalDesc") <> "" Then
                    txtChemicalDesc.Text = HttpContext.Current.Request.QueryString("ChemicalDesc")
                    ViewState("ChemicalDesc") = HttpContext.Current.Request.QueryString("ChemicalDesc")
                Else
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch") Is Nothing Then
                        If Trim(Request.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Value) <> "" Then
                            txtChemicalDesc.Text = Request.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Value
                            ViewState("ChemicalDesc") = Request.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Value
                        End If
                    End If
                End If

                If Not Request.Cookies("SafetyModule_SaveChemRevFormFilterActiveSearch") Is Nothing Then
                    If Not Request.Cookies("SafetyModule_SaveChemRevFormIsActiveSearch") Is Nothing Then
                        If CType(Request.Cookies("SafetyModule_SaveChemRevFormFilterActiveSearch").Value, Integer) = 1 Then
                            ViewState("filterActive") = 1
                            ViewState("isActive") = CType(Request.Cookies("SafetyModule_SaveChemRevFormIsActiveSearch").Value, Integer)
                            ddActive.SelectedValue = CType(Request.Cookies("SafetyModule_SaveChemRevFormIsActiveSearch").Value, Integer)
                        End If
                    End If
                End If

                'load repeater control
                BindData()

            Else
                If txtChemRevFormID.Text <> "" Then
                    ViewState("ChemRevFormID") = txtChemRevFormID.Text.Trim
                End If

                If ddStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddStatus.SelectedValue
                End If

                If ddUGNFacility.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddUGNFacility.SelectedValue
                End If

                If ddRequestedByTeamMember.SelectedIndex > 0 Then
                    ViewState("RequestedByTeamMemberID") = ddRequestedByTeamMember.SelectedValue
                End If

                If txtRequestDateStart.Text.Length > 0 Then
                    ViewState("RequestDateStart") = txtRequestDateStart.Text.Trim
                End If

                If txtRequestDateEnd.Text.Length > 0 Then
                    ViewState("RequestDateEnd") = txtRequestDateEnd.Text.Trim
                End If

                If ddApprover.SelectedIndex > 0 Then
                    ViewState("ApprovingTeamMemberID") = ddApprover.SelectedValue
                End If

                If txtProductName.Text <> "" Then
                    ViewState("ProductName") = txtProductName.Text.Trim
                End If

                If txtProductManufacturer.Text <> "" Then
                    ViewState("ProductManufacturer") = txtProductManufacturer.Text.Trim
                End If

                If txtPurchaseFrom.Text <> "" Then
                    ViewState("PurchaseFrom") = txtPurchaseFrom.Text.Trim
                End If

                If txtDeptArea.Text <> "" Then
                    ViewState("DeptArea") = txtDeptArea.Text.Trim
                End If

                If txtChemicalDesc.Text <> "" Then
                    ViewState("ChemicalDesc") = txtChemicalDesc.Text.Trim
                End If

                ViewState("filterActive") = 0
                ViewState("isActive") = 0
                If ddActive.SelectedIndex > 0 Then
                    ViewState("filterActive") = 1
                    ViewState("isActive") = ddActive.SelectedValue
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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            lblMessage.Text = ""

            SafetyModule.DeleteChemicalReviewFormCookies()

            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = Nothing

            Response.Redirect("Chemical_Review_Form_Detail.aspx", False)

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

        lblMessage.Text = ""

        Try
            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = Nothing

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'set saved value of what criteria was used to search   
            ''''''''''''''''''''''''''''''''''''''''''''''''''''

            Response.Cookies("SafetyModule_SaveChemRevFormIDSearch").Value = txtChemRevFormID.Text.Trim

            If ddStatus.SelectedIndex > 0 Then
                Response.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value = ddStatus.SelectedValue
            Else
                Response.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Value = 0
                Response.Cookies("SafetyModule_SaveChemRevFormStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value = ddUGNFacility.SelectedValue
            Else
                Response.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Value = ""
                Response.Cookies("SafetyModule_SaveChemRevFormUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddRequestedByTeamMember.SelectedIndex > 0 Then
                Response.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value = ddRequestedByTeamMember.SelectedValue
            Else
                Response.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Value = 0
                Response.Cookies("SafetyModule_SaveChemRevFormRequestedByTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("SafetyModule_SaveChemRevFormRequestDateStartSearch").Value = txtRequestDateStart.Text.Trim
            Response.Cookies("SafetyModule_SaveChemRevFormRequestDateEndSearch").Value = txtRequestDateEnd.Text.Trim

            If ddApprover.SelectedIndex > 0 Then
                Response.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value = ddApprover.SelectedValue
            Else
                Response.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Value = 0
                Response.Cookies("SafetyModule_SaveChemRevFormApprovingTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("SafetyModule_SaveChemRevFormProductNameSearch").Value = txtProductName.Text.Trim
            Response.Cookies("SafetyModule_SaveChemRevFormProductManufacturerSearch").Value = txtProductManufacturer.Text.Trim
            Response.Cookies("SafetyModule_SaveChemRevFormPurchaseFromSearch").Value = txtPurchaseFrom.Text.Trim
            Response.Cookies("SafetyModule_SaveChemRevFormDeptAreaSearch").Value = txtDeptArea.Text.Trim
            Response.Cookies("SafetyModule_SaveChemRevFormChemicalDescSearch").Value = txtChemicalDesc.Text.Trim

            Response.Cookies("SafetyModule_SaveChemRevFormFilterActiveSearch").Value = 0
            Response.Cookies("SafetyModule_SaveChemRevFormIsActiveSearch").Value = 0
            ViewState("isActive") = 0
            ViewState("filterActive") = 0
            If ddActive.SelectedIndex > 0 Then
                Response.Cookies("SafetyModule_SaveChemRevFormFilterActiveSearch").Value = 1
                ViewState("filterActive") = 1
                Response.Cookies("SafetyModule_SaveChemRevFormIsActiveSearch").Value = ddActive.SelectedValue
                ViewState("filterActive") = ddActive.SelectedValue
            End If

            Response.Redirect("Chemical_Review_Form_List.aspx?ChemRevFormID=" & Server.UrlEncode(txtChemRevFormID.Text.Trim) _
            & "&StatusID=" & ddStatus.SelectedValue _
            & "&UGNFacility=" & ddUGNFacility.SelectedValue _
            & "&RequestedByTeamMemberID=" & ddRequestedByTeamMember.SelectedValue _
            & "&RequestDateStart=" & Server.UrlEncode(txtRequestDateStart.Text.Trim) _
            & "&RequestDateEnd=" & Server.UrlEncode(txtRequestDateEnd.Text.Trim) _
            & "&ApprovingTeamMemberID=" & ddApprover.SelectedValue _
            & "&ProductName=" & Server.UrlEncode(txtProductName.Text.Trim) _
            & "&ProductManufacturer=" & Server.UrlEncode(txtProductManufacturer.Text.Trim) _
            & "&PurchaseFrom=" & Server.UrlEncode(txtPurchaseFrom.Text.Trim) _
            & "&DeptArea=" & Server.UrlEncode(txtDeptArea.Text.Trim) _
            & "&ChemicalDesc=" & Server.UrlEncode(txtChemicalDesc.Text.Trim) _
            & "&isApproved=" & ViewState("isActive") _
            & "&filterApproved=" & ViewState("filterActive") _
            , False)

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
            lblMessage.Text = ""

            SafetyModule.DeleteChemicalReviewFormCookies()

            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = Nothing

            Response.Redirect("Chemical_Review_Form_List.aspx", False)
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

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = CurrentPage

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

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = CurrentPage

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

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = CurrentPage

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

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = CurrentPage

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

                HttpContext.Current.Session("sessionChemicalReviewFormCurrentPage") = CurrentPage

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

End Class
