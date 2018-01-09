' ************************************************************************************************
'
' Name:		ECI_List.aspx
' Purpose:	This Code Behind is for the Engineering Change Instruction
'
' Date		    Author	    
' 06/11/2009    Roderick Carlson
' 08/26/2010    Roderick Carlson     Modified : added isActiveBPCSOnly Parameter to GetUGNDBVendor
' 11/22/2010    Roderick Carlson     Modified : use new ECI Search function, added ECI Description to search
' 02/20/2012    Roderick Carlson     Modified : added debug logic to page load to help touble-shoot a search error
' 02/18/2013    Roderick Carlson     Modified : Aded ECI Initiator List
' ************************************************************************************************
Partial Class ECI_List
    Inherits System.Web.UI.Page
    Protected WithEvents lnkECINo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkECIType As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkECIStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDrawingNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewCustomerPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDesignLevel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkBusinessProcessType As System.Web.UI.WebControls.LinkButton

    Protected Function SetBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White"

        Try
            If StatusID = "1" Or StatusID = "2" Then
                strReturnValue = "Yellow"
            End If

            If StatusID = "4" Then
                strReturnValue = "Gray"
            End If

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

    Protected Function SetPreviewECIHyperLink(ByVal ECINo As String, ByVal ECIType As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = ""

        Try
            If ECINo <> "" And StatusID <> "4" Then
                strReturnValue = "javascript:void(window.open('ECI_Preview.aspx?ECINo=" & ECINo & "&ECIType=" & ECIType & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewECIHyperLink = strReturnValue

    End Function

    Protected Function SetPreviewUgnIPPHyperLink(ByVal ECINo As String, ByVal StatusID As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try
            If ArchiveData = 0 Then
                If ECINo <> "" And StatusID <> "4" Then
                    strReturnValue = "javascript:void(window.open('UGN_IPP_Preview.aspx?ECINo=" & ECINo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                End If
            Else
                strReturnValue = "javascript:void(window.open('UGN_IPP_Preview.aspx?ECINo=" & ECINo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewUgnIPPHyperLink = strReturnValue

    End Function
    Protected Function SetPreviewCustomerIPPHyperLink(ByVal ECINo As String, ByVal StatusID As String) As String

        Dim strReturnValue As String = ""

        Try
            If ECINo <> "" And StatusID <> "4" Then
                strReturnValue = "javascript:void(window.open('Customer_IPP_Preview.aspx?ECINo=" & ECINo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewCustomerIPPHyperLink = strReturnValue

    End Function
    Protected Function SetECIHyperlink(ByVal ECINo As String, ByVal ECIType As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try
            If ArchiveData = 0 Then
                strReturnValue = "ECI_Detail.aspx?ECINo=" & ECINo
            Else
                strReturnValue = "javascript:void(window.open('ECI_Preview.aspx?ECINo=" & ECINo & "&ECIType=" & ECIType & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetECIHyperlink = strReturnValue

    End Function

    Protected Function SetVisibleECIHyperLink(ByVal ECINo As String, ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            If ECINo <> "" And StatusID <> "4" Then
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

        SetVisibleECIHyperLink = bReturnValue

    End Function

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

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 86)

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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckIncludeArchives()

        Try
            ddSearchBusinessProcessType.Visible = Not cbIncludeArchive.Checked
            ddSearchCustomer.Visible = Not cbIncludeArchive.Checked
            ddSearchCommodity.Visible = Not cbIncludeArchive.Checked
            ddSearchCustomerIPP.Visible = Not cbIncludeArchive.Checked
            ddSearchDesignationType.Visible = Not cbIncludeArchive.Checked
            ddSearchInitiatorTeamMember.Visible = Not cbIncludeArchive.Checked
            ddSearchPPAP.Visible = Not cbIncludeArchive.Checked
            ddSearchProductTechnology.Visible = Not cbIncludeArchive.Checked
            ddSearchProgram.Visible = Not cbIncludeArchive.Checked
            ddSearchPurchasedGood.Visible = Not cbIncludeArchive.Checked

            ddSearchStatus.Visible = Not cbIncludeArchive.Checked
            ddSearchSubFamily.Visible = Not cbIncludeArchive.Checked
            ddSearchUGNFacility.Visible = Not cbIncludeArchive.Checked
            ddSearchUgnIPP.Visible = Not cbIncludeArchive.Checked
            ddSearchVendor.Visible = Not cbIncludeArchive.Checked

            txtSearchCustomerPartNo.Visible = Not cbIncludeArchive.Checked
            txtSearchDrawingNo.Visible = Not cbIncludeArchive.Checked
            txtSearchLastUpdatedOnEndDate.Visible = Not cbIncludeArchive.Checked
            txtSearchLastUpdatedOnStartDate.Visible = Not cbIncludeArchive.Checked

            imgSearchLastUpdatedOnEndDate.Visible = Not cbIncludeArchive.Checked
            imgSearchLastUpdatedOnStartDate.Visible = Not cbIncludeArchive.Checked
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

            CheckIncludeArchives()

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

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgram.DataSource = ds
                ddSearchProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddSearchProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddSearchProgram.DataBind()
                ddSearchProgram.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchSubFamily.DataSource = ds
                ddSearchSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSearchSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSearchSubFamily.DataBind()
                ddSearchSubFamily.Items.Insert(0, "")
            End If

            ds = ECIModule.GetECIStatus(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchStatus.DataSource = ds
                ddSearchStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddSearchStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddSearchStatus.DataBind()
                ddSearchStatus.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchDesignationType.DataSource = ds
                ddSearchDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddSearchDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddSearchDesignationType.DataBind()
                ddSearchDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacility.DataSource = ds
                ddSearchUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddSearchUGNFacility.DataBind()
                ddSearchUGNFacility.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetBusinessProcessType(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchBusinessProcessType.DataSource = ds
                ddSearchBusinessProcessType.DataTextField = ds.Tables(0).Columns("ddBusinessProcessTypeName").ColumnName.ToString()
                ddSearchBusinessProcessType.DataValueField = ds.Tables(0).Columns("BusinessProcessTypeID").ColumnName
                ddSearchBusinessProcessType.DataBind()
                ddSearchBusinessProcessType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCustomer.DataSource = ds
                ddSearchCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataBind()
                ddSearchCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProductTechnology.DataSource = ds
                ddSearchProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName.ToString()
                ddSearchProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddSearchProductTechnology.DataBind()
                ddSearchProductTechnology.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCommodity.DataSource = ds
                ddSearchCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddSearchCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddSearchCommodity.DataBind()
                ddSearchCommodity.Items.Insert(0, "")
                ddSearchCommodity.SelectedIndex = 0
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchPurchasedGood.DataSource = ds
                ddSearchPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddSearchPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddSearchPurchasedGood.DataBind()
                ddSearchPurchasedGood.Items.Insert(0, "")
            End If

            'Iniator (Quality Engineer)
            'ds = commonFunctions.GetTeamMemberBySubscription(22)
            ds = ECIModule.GetECIInitiator()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchInitiatorTeamMember.DataSource = ds
                ddSearchInitiatorTeamMember.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchInitiatorTeamMember.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchInitiatorTeamMember.DataBind()
                ddSearchInitiatorTeamMember.Items.Insert(0, "")
            End If

            ' Quality Engineer
            ds = commonFunctions.GetTeamMemberBySubscription(22)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchQualityEngineer.DataSource = ds
                ddSearchQualityEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchQualityEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchQualityEngineer.DataBind()
                ddSearchQualityEngineer.Items.Insert(0, "")
            End If

            ' Account Manager
            ds = commonFunctions.GetTeamMemberBySubscription(18)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchAccountManager.DataSource = ds
                ddSearchAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchAccountManager.DataBind()
                ddSearchAccountManager.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchVendor.DataSource = ds
                ddSearchVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName.ToString()
                ddSearchVendor.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddSearchVendor.DataBind()
                ddSearchVendor.Items.Insert(0, "")
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
            ds = ECIModule.GetECISearch(ViewState("ECINo"), ViewState("ECIDesc"), ViewState("ECIType"), _
            ViewState("StatusID"), ViewState("IssueDate"), ViewState("ImplementationDate"), _
            ViewState("RFDNo"), ViewState("CostSheetID"), ViewState("InitiatorTeamMemberID"), ViewState("DrawingNo"), _
            ViewState("PartNo"), ViewState("PartName"), ViewState("CustomerValue"), ViewState("CustomerPartNo"), _
            ViewState("DesignLevel"), ViewState("DesignationType"), ViewState("BusinessProcessTypeID"), ViewState("ProgramID"), _
            ViewState("CommodityID"), ViewState("PurchasedGoodID"), ViewState("ProductTechnologyID"), ViewState("SubFamilyID"), _
            ViewState("UGNFacility"), ViewState("UGNDBVendorID"), ViewState("AccountManagerID"), ViewState("QualityEngineerID"), _
            ViewState("filterPPAP"), ViewState("isPPAP"), ViewState("filterUgnIPP"), ViewState("isUgnIPP"), _
            ViewState("filterCustomerIPP"), ViewState("isCustomerIPP"), _
            ViewState("LastUpdatedOnStartDate"), ViewState("LastUpdatedOnEndDate"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                rpECIInfo.DataSource = ds
                rpECIInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpECIInfo.DataSource = objPds
                rpECIInfo.DataBind()

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
    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = ECIModule.GetECISearch(ViewState("ECINo"), ViewState("ECIDesc"), ViewState("ECIType"), _
            ViewState("StatusID"), ViewState("IssueDate"), ViewState("ImplementationDate"), _
            ViewState("RFDNo"), ViewState("CostSheetID"), ViewState("InitiatorTeamMemberID"), ViewState("DrawingNo"), _
            ViewState("PartNo"), ViewState("PartName"), ViewState("CustomerValue"), ViewState("CustomerPartNo"), _
            ViewState("DesignLevel"), ViewState("DesignationType"), ViewState("BusinessProcessTypeID"), ViewState("ProgramID"), _
            ViewState("CommodityID"), ViewState("PurchasedGoodID"), ViewState("ProductTechnologyID"), ViewState("SubFamilyID"), _
            ViewState("UGNFacility"), ViewState("UGNDBVendorID"), ViewState("AccountManagerID"), ViewState("QualityEngineerID"), _
            ViewState("filterPPAP"), ViewState("isPPAP"), ViewState("filterUGNIPP"), ViewState("isUGNIPP"), _
            ViewState("filterCustomerIPP"), ViewState("isCustomerIPP"), _
            ViewState("LastUpdatedOnStartDate"), ViewState("LastUpdatedOnEndDate"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpECIInfo.DataSource = dv
                rpECIInfo.DataBind()

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
       Handles lnkBusinessProcessType.Click, lnkNewDesignLevel.Click, lnkECINo.Click, lnkECIStatus.Click, lnkECIType.Click, lnkNewPartNo.Click, lnkNewCustomerPartNo.Click, lnkNewDrawingNo.Click, lnkNewPartName.Click

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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Engineering Change Instruction"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Quality</b> > ECI Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ECIExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

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

        'Dim strDebug As String = ""

        Try

            If HttpContext.Current.Session("sessionECICurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionECICurrentPage")
            End If

            'clear crystal reports
            ECIModule.CleanECICrystalReports()

            If Not Page.IsPostBack Then
                'strDebug &= ";not postpack"

                CheckRights()

                'strDebug &= ";checked rights"

                ViewState("lnkECINo") = "DESC"
                ViewState("lnkECIType") = "ASC"
                ViewState("lnkECIStatus") = "ASC"
                ViewState("lnkNewDrawingNo") = "ASC"
                ViewState("lnkNewPartNo") = "ASC"
                ViewState("lnkNewCustomerPartNo") = "ASC"
                ViewState("lnkNewPartName") = "ASC"
                ViewState("lnkDesc") = "ASC"
                ViewState("lnkBusinessProcessType") = "ASC"

                ViewState("ECINo") = 0
                ViewState("ECIDesc") = ""
                ViewState("ECIType") = ""
                ViewState("StatusID") = 0
                ViewState("IssueDate") = ""
                ViewState("ImplementationDate") = ""
                ViewState("RFDNo") = 0
                ViewState("CostSheetID") = 0
                ViewState("InitiatorTeamMemberID") = 0
                ViewState("DrawingNo") = ""
                ViewState("PartNo") = ""
                ViewState("PartName") = ""
                ViewState("CustomerValue") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("DesignLevel") = ""
                ViewState("DesignationType") = ""
                ViewState("BusinessProcessTypeID") = 0
                ViewState("ProgramID") = 0
                ViewState("CommodityID") = 0
                ViewState("PurchasedGoodID") = 0
                ViewState("ProductTechnologyID") = 0
                ViewState("SubFamilyID") = 0
                ViewState("UGNFacility") = ""
                ViewState("UGNDBVendorID") = 0
                ViewState("AccountManagerID") = 0
                ViewState("QualityEngineerID") = 0
                ViewState("filterPPAP") = 0
                ViewState("isPPAP") = 0
                ViewState("filterUgnIPP") = 0
                ViewState("isUgnIPP") = 0
                ViewState("filterCustomerIPP") = 0
                ViewState("isCustomerIPP") = 0
                ViewState("LastUpdatedOnStartDate") = ""
                ViewState("LastUpdatedOnEndDate") = ""
                ViewState("IncludeArchive") = 0

                'strDebug &= ";set viewstate"

                BindCriteria()

                'strDebug &= ";BindCriteria"

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    txtSearchECINo.Text = HttpContext.Current.Request.QueryString("ECINo")
                    ViewState("ECINo") = HttpContext.Current.Request.QueryString("ECINo")
                Else
                    If Not Request.Cookies("ECIModule_SaveECINoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveECINoSearch").Value) <> "" Then
                            txtSearchECINo.Text = Request.Cookies("ECIModule_SaveECINoSearch").Value
                            ViewState("ECINo") = Request.Cookies("ECIModule_SaveECINoSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";ECINo Search"

                If HttpContext.Current.Request.QueryString("ECIDesc") <> "" Then
                    txtSearchECIDesc.Text = HttpContext.Current.Request.QueryString("ECIDesc")
                    ViewState("ECIDesc") = HttpContext.Current.Request.QueryString("ECIDesc")
                Else
                    If Not Request.Cookies("ECIModule_SaveECIDescSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveECIDescSearch").Value) <> "" Then
                            txtSearchECIDesc.Text = Request.Cookies("ECIModule_SaveECIDescSearch").Value
                            ViewState("ECIDesc") = Request.Cookies("ECIModule_SaveECIDescSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";ECIDesc Search"

                If HttpContext.Current.Request.QueryString("ECIType") <> "" Then
                    ddSearchECIType.SelectedValue = HttpContext.Current.Request.QueryString("ECIType")
                    ViewState("ECIType") = HttpContext.Current.Request.QueryString("ECIType")
                Else
                    If Not Request.Cookies("ECIModule_SaveECITypeSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveECITypeSearch").Value) <> "" Then
                            ddSearchECIType.SelectedValue = Request.Cookies("ECIModule_SaveECITypeSearch").Value
                            ViewState("ECIType") = Request.Cookies("ECIModule_SaveECITypeSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";ECI Type Search"

                If HttpContext.Current.Request.QueryString("StatusID") <> "" Then
                    ddSearchStatus.SelectedValue = HttpContext.Current.Request.QueryString("StatusID")
                    ViewState("StatusID") = HttpContext.Current.Request.QueryString("StatusID")
                Else
                    If Not Request.Cookies("ECIModule_SaveStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveStatusIDSearch").Value) <> "" Then
                            ddSearchStatus.SelectedValue = Request.Cookies("ECIModule_SaveStatusIDSearch").Value
                            ViewState("StatusID") = Request.Cookies("ECIModule_SaveStatusIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";ECI Status Search"

                If HttpContext.Current.Request.QueryString("IssueDate") <> "" Then
                    txtSearchIssueDate.Text = HttpContext.Current.Request.QueryString("IssueDate")
                    ViewState("IssueDate") = HttpContext.Current.Request.QueryString("IssueDate")
                Else
                    If Not Request.Cookies("ECIModule_SaveIssueDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveIssueDateSearch").Value) <> "" Then
                            txtSearchIssueDate.Text = Request.Cookies("ECIModule_SaveIssueDateSearch").Value
                            ViewState("IssueDate") = Request.Cookies("ECIModule_SaveIssueDateSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Issue Date Search"

                If HttpContext.Current.Request.QueryString("ImplementationDate") <> "" Then
                    txtSearchImplementationDate.Text = HttpContext.Current.Request.QueryString("ImplementationDate")
                    ViewState("ImplementationDate") = HttpContext.Current.Request.QueryString("ImplementationDate")
                Else
                    If Not Request.Cookies("ECIModule_SaveImplementationDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveImplementationDateSearch").Value) <> "" Then
                            txtSearchImplementationDate.Text = Request.Cookies("ECIModule_SaveImplementationDateSearch").Value
                            ViewState("ImplementationDate") = Request.Cookies("ECIModule_SaveImplementationDateSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Implementation Date Search"

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    txtSearchRFDNo.Text = HttpContext.Current.Request.QueryString("RFDNo")
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                Else
                    If Not Request.Cookies("ECIModule_SaveRFDNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveRFDNoSearch").Value) <> "" Then
                            txtSearchRFDNo.Text = Request.Cookies("ECIModule_SaveRFDNoSearch").Value
                            ViewState("RFDNo") = Request.Cookies("ECIModule_SaveRFDNoSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";RFDNo Search"

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    txtSearchCostSheetID.Text = HttpContext.Current.Request.QueryString("CostSheetID")
                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")
                Else
                    If Not Request.Cookies("ECIModule_SaveCostSheetIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveCostSheetIDSearch").Value) <> "" Then
                            txtSearchCostSheetID.Text = Request.Cookies("ECIModule_SaveCostSheetIDSearch").Value
                            ViewState("CostSheetID") = Request.Cookies("ECIModule_SaveCostSheetIDSearch").Value
                        End If
                    End If
                End If

                ' strDebug &= ";Cost Sheet ID Search"

                If HttpContext.Current.Request.QueryString("InitiatorTeamMemberID") <> "" Then
                    ddSearchInitiatorTeamMember.SelectedValue = HttpContext.Current.Request.QueryString("InitiatorTeamMemberID")
                    ViewState("InitiatorTeamMemberID") = HttpContext.Current.Request.QueryString("InitiatorTeamMemberID")
                Else
                    If Not Request.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value) <> "" Then
                            ddSearchInitiatorTeamMember.SelectedValue = Request.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value
                            ViewState("InitiatorTeamMemberID") = Request.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Initiator Team MemberID Search"

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("ECIModule_SaveDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveDrawingNoSearch").Value) <> "" Then
                            txtSearchDrawingNo.Text = Request.Cookies("ECIModule_SaveDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("ECIModule_SaveDrawingNoSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";DrawingNo Search"

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtSearchPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("ECIModule_SavePartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SavePartNoSearch").Value) <> "" Then
                            txtSearchPartNo.Text = Request.Cookies("ECIModule_SavePartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("ECIModule_SavePartNoSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";PartNo Search"

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtSearchPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("ECIModule_SavePartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SavePartNameSearch").Value) <> "" Then
                            txtSearchPartName.Text = Request.Cookies("ECIModule_SavePartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("ECIModule_SavePartNameSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Part Name Search"

                If HttpContext.Current.Request.QueryString("CustomerValue") <> "" Then
                    ddSearchCustomer.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))
                    ViewState("CustomerValue") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CustomerValue"))

                Else
                    If Not Request.Cookies("ECIModule_SaveCustomerSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveCustomerSearch").Value) <> "" Then
                            ddSearchCustomer.SelectedValue = Request.Cookies("ECIModule_SaveCustomerSearch").Value
                            ViewState("CustomerValue") = Request.Cookies("ECIModule_SaveCustomerSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Customer Value Search"
                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtSearchCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                Else
                    If Not Request.Cookies("ECIModule_SaveCustomerPartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveCustomerPartNoSearch").Value) <> "" Then
                            txtSearchCustomerPartNo.Text = Request.Cookies("ECIModule_SaveCustomerPartNoSearch").Value
                            ViewState("CustomerPartNo") = Request.Cookies("ECIModule_SaveCustomerPartNoSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";CustomerPartNo Search"

                If HttpContext.Current.Request.QueryString("DesignLevel") <> "" Then
                    txtSearchDesignLevel.Text = HttpContext.Current.Request.QueryString("DesignLevel")
                    ViewState("DesignLevel") = HttpContext.Current.Request.QueryString("DesignLevel")
                Else
                    If Not Request.Cookies("ECIModule_SaveDesignLevelSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveDesignLevelSearch").Value) <> "" Then
                            txtSearchDesignLevel.Text = Request.Cookies("ECIModule_SaveDesignLevelSearch").Value
                            ViewState("DesignLevel") = Request.Cookies("ECIModule_SaveDesignLevelSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Design Level Search"

                If HttpContext.Current.Request.QueryString("DesignationType") <> "" Then
                    ddSearchDesignationType.SelectedValue = HttpContext.Current.Request.QueryString("DesignationType")
                    ViewState("DesignationType") = HttpContext.Current.Request.QueryString("DesignationType")
                Else
                    If Not Request.Cookies("ECIModule_SaveDesignationTypeSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveDesignationTypeSearch").Value) <> "" Then
                            ddSearchDesignationType.SelectedValue = Request.Cookies("ECIModule_SaveDesignationTypeSearch").Value
                            ViewState("DesignationType") = Request.Cookies("ECIModule_SaveDesignationTypeSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";DesignationType Search"

                If HttpContext.Current.Request.QueryString("BusinessProcessTypeID") <> "" Then
                    ddSearchBusinessProcessType.SelectedValue = HttpContext.Current.Request.QueryString("BusinessProcessTypeID")
                    ViewState("BusinessProcessTypeID") = HttpContext.Current.Request.QueryString("BusinessProcessTypeID")
                Else
                    If Not Request.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value) <> "" Then
                            ddSearchBusinessProcessType.SelectedValue = Request.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value
                            ViewState("BusinessProcessTypeID") = Request.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Business Process Type Search"

                If HttpContext.Current.Request.QueryString("ProgramID") <> "" Then
                    ddSearchProgram.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                    ViewState("ProgramID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProgramID"))
                Else
                    If Not Request.Cookies("ECIModule_SaveProgramIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveProgramIDSearch").Value) <> "" Then
                            ddSearchProgram.SelectedValue = Request.Cookies("ECIModule_SaveProgramIDSearch").Value
                            ViewState("ProgramID") = Request.Cookies("ECIModule_SaveProgramIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";ProgramID Search"

                If HttpContext.Current.Request.QueryString("CommodityID") <> "" Then
                    ddSearchCommodity.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                    ViewState("CommodityID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("CommodityID"))
                Else
                    If Not Request.Cookies("ECIModule_SaveCommodityIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveCommodityIDSearch").Value) <> "" Then
                            ddSearchCommodity.SelectedValue = Request.Cookies("ECIModule_SaveCommodityIDSearch").Value
                            ViewState("CommodityID") = Request.Cookies("ECIModule_SaveCommodityIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Commodity Search"

                If HttpContext.Current.Request.QueryString("PurchasedGoodID") <> "" Then
                    ddSearchPurchasedGood.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("PurchasedGoodID"))
                    ViewState("PurchasedGoodID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("PurchasedGoodID"))
                Else
                    If Not Request.Cookies("ECIModule_SavePurchasedGoodIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value) <> "" Then
                            ddSearchPurchasedGood.SelectedValue = Request.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value
                            ViewState("PurchasedGoodID") = Request.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Purchased Good Search"

                If HttpContext.Current.Request.QueryString("ProductTechnologyID") <> "" Then
                    ddSearchProductTechnology.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProductTechnologyID"))
                    ViewState("ProductTechnologyID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("ProductTechnologyID"))
                Else
                    If Not Request.Cookies("ECIModule_SaveProductTechnologyIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value) <> "" Then
                            ddSearchProductTechnology.SelectedValue = Request.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value
                            ViewState("ProductTechnologyID") = Request.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Product Technology Search"

                If HttpContext.Current.Request.QueryString("SubFamilyID") <> "" Then
                    ddSearchSubFamily.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("SubFamilyID"))
                    ViewState("SubFamilyID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("SubFamilyID"))
                Else
                    If Not Request.Cookies("ECIModule_SaveSubFamilyIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveSubFamilyIDSearch").Value) <> "" Then
                            ddSearchSubFamily.SelectedValue = Request.Cookies("ECIModule_SaveSubFamilyIDSearch").Value
                            ViewState("SubFamilyID") = Request.Cookies("ECIModule_SaveSubFamilyIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Subfamily Search"

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddSearchUGNFacility.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                    ViewState("UGNFacility") = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNFacility"))
                Else
                    If Not Request.Cookies("ECIModule_SaveUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveUGNFacilitySearch").Value) <> "" Then
                            ddSearchUGNFacility.SelectedValue = Request.Cookies("ECIModule_SaveUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("ECIModule_SaveUGNFacilitySearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Facility Search"

                If HttpContext.Current.Request.QueryString("UGNDBVendorID") <> "" Then
                    ddSearchVendor.SelectedValue = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNDBVendorID"))
                    ViewState("UGNDBVendorID") = Server.UrlDecode(HttpContext.Current.Request.QueryString("UGNDBVendorID"))
                Else
                    If Not Request.Cookies("ECIModule_SaveUGNDBVendorIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value) <> "" Then
                            ddSearchVendor.SelectedValue = Request.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value
                            ViewState("UGNDBVendorID") = Request.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";UGNDB VendorID Search"

                If HttpContext.Current.Request.QueryString("AccountManagerID") <> "" Then
                    ddSearchAccountManager.SelectedValue = HttpContext.Current.Request.QueryString("AccountManagerID")
                    ViewState("AccountManagerID") = HttpContext.Current.Request.QueryString("AccountManagerID")
                Else
                    If Not Request.Cookies("ECIModule_SaveAccountManagerIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveAccountManagerIDSearch").Value) <> "" Then
                            ddSearchAccountManager.SelectedValue = Request.Cookies("ECIModule_SaveAccountManagerIDSearch").Value
                            ViewState("AccountManagerID") = Request.Cookies("ECIModule_SaveAccountManagerIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Account Manager Search"

                If HttpContext.Current.Request.QueryString("QualityEngineerID") <> "" Then
                    ddSearchQualityEngineer.SelectedValue = HttpContext.Current.Request.QueryString("QualityEngineerID")
                    ViewState("QualityEngineerID") = HttpContext.Current.Request.QueryString("QualityEngineerID")
                Else
                    If Not Request.Cookies("ECIModule_SaveQualityEngineerIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value) <> "" Then
                            ddSearchQualityEngineer.SelectedValue = Request.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value
                            ViewState("QualityEngineerID") = Request.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";Quality Engineer Search"

                If HttpContext.Current.Request.QueryString("isPPAP") <> "" Then
                    ViewState("isPPAP") = CType(HttpContext.Current.Request.QueryString("isPPAP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveIsPPAPSearch") Is Nothing Then
                        ViewState("isPPAP") = CType(Request.Cookies("ECIModule_SaveIsPPAPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";isPPAP Search"

                If HttpContext.Current.Request.QueryString("filterPPAP") <> "" Then
                    ViewState("filterPPAP") = CType(HttpContext.Current.Request.QueryString("filterPPAP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveFilterPPAPSearch") Is Nothing Then
                        ViewState("filterPPAP") = CType(Request.Cookies("ECIModule_SaveFilterPPAPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";filter PPAP Search"

                If ViewState("filterPPAP") > 0 And ViewState("isPPAP") > 0 Then
                    ddSearchPPAP.SelectedValue = "Only"
                End If

                'strDebug &= ";PPAP only Search"

                If ViewState("filterPPAP") > 0 And ViewState("isPPAP") = 0 Then
                    ddSearchPPAP.SelectedValue = "None"
                End If

                'strDebug &= ";PPAP none Search"

                If HttpContext.Current.Request.QueryString("isUgnIPP") <> "" Then
                    ViewState("isUgnIPP") = CType(HttpContext.Current.Request.QueryString("isUgnIPP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveIsUgnIPPSearch") Is Nothing Then
                        ViewState("isUgnIPP") = CType(Request.Cookies("ECIModule_SaveIsUgnIPPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";isUgnIPP Search"

                If HttpContext.Current.Request.QueryString("filterUgnIPP") <> "" Then
                    ViewState("filterUgnIPP") = CType(HttpContext.Current.Request.QueryString("filterUgnIPP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveFilterUgnIPPSearch") Is Nothing Then
                        ViewState("filterUgnIPP") = CType(Request.Cookies("ECIModule_SaveFilterUgnIPPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";filter isUgnIPP Search"

                If ViewState("filterUgnIPP") > 0 And ViewState("isUgnIPP") > 0 Then
                    ddSearchUgnIPP.SelectedValue = "Only"
                End If

                'strDebug &= ";isUgnIPP only Search"

                If ViewState("filterUgnIPP") > 0 And ViewState("isUgnIPP") = 0 Then
                    ddSearchUgnIPP.SelectedValue = "None"
                End If

                'strDebug &= ";isUgnIPP none Search"

                If HttpContext.Current.Request.QueryString("isCustomerIPP") <> "" Then
                    ViewState("isCustomerIPP") = CType(HttpContext.Current.Request.QueryString("isCustomerIPP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveIsCustomerIPPSearch") Is Nothing Then
                        ViewState("isCustomerIPP") = CType(Request.Cookies("ECIModule_SaveIsCustomerIPPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";isCustomerIPP Search"

                If HttpContext.Current.Request.QueryString("filterCustomerIPP") <> "" Then
                    ViewState("filterCustomerIPP") = CType(HttpContext.Current.Request.QueryString("filterCustomerIPP"), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveFilterCustomerIPPSearch") Is Nothing Then
                        ViewState("filterCustomerIPP") = CType(Request.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Value, Integer)
                    End If
                End If

                'strDebug &= ";filter isCustomerIPP Search"

                If ViewState("filterCustomerIPP") > 0 And ViewState("isCustomerIPP") > 0 Then
                    ddSearchCustomerIPP.SelectedValue = "Only"
                End If

                'strDebug &= ";isCustomerIPP only Search"

                If ViewState("filterCustomerIPP") > 0 And ViewState("isCustomerIPP") = 0 Then
                    ddSearchCustomerIPP.SelectedValue = "None"
                End If

                'strDebug &= ";isCustomerIPP none Search"

                If HttpContext.Current.Request.QueryString("LastUpdatedOnStartDate") <> "" Then
                    txtSearchLastUpdatedOnStartDate.Text = HttpContext.Current.Request.QueryString("LastUpdatedOnStartDate")
                    ViewState("LastUpdatedOnStartDate") = HttpContext.Current.Request.QueryString("LastUpdatedOnStartDate")
                Else
                    If Not Request.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Value) <> "" Then
                            txtSearchLastUpdatedOnStartDate.Text = Request.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Value
                            ViewState("LastUpdatedOnStartDate") = Request.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Value
                        End If
                    End If
                End If

                ' strDebug &= ";LastUpdatedOnStartDate Search"

                If HttpContext.Current.Request.QueryString("LastUpdatedOnEndDate") <> "" Then
                    txtSearchLastUpdatedOnEndDate.Text = HttpContext.Current.Request.QueryString("LastUpdatedOnEndDate")
                    ViewState("LastUpdatedOnEndDate") = HttpContext.Current.Request.QueryString("LastUpdatedOnEndDate")
                Else
                    If Not Request.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Value) <> "" Then
                            txtSearchLastUpdatedOnEndDate.Text = Request.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Value
                            ViewState("LastUpdatedOnEndDate") = Request.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Value
                        End If
                    End If
                End If

                'strDebug &= ";LastUpdatedOnEndDate Search"

                ViewState("IncludeArchive") = 0
                If HttpContext.Current.Request.QueryString("IncludeArchive") <> "" Then
                    ViewState("IncludeArchive") = CType(Server.UrlDecode(HttpContext.Current.Request.QueryString("IncludeArchive")), Integer)
                Else
                    If Not Request.Cookies("ECIModule_SaveIncludeArchiveSearch") Is Nothing Then
                        If Trim(Request.Cookies("ECIModule_SaveIncludeArchiveSearch").Value) <> "" Then
                            ViewState("IncludeArchive") = CType(Request.Cookies("ECIModule_SaveIncludeArchiveSearch").Value, Integer)
                        End If
                    End If
                End If

                cbIncludeArchive.Checked = ViewState("IncludeArchive")

                'strDebug &= ";IncludeArchive Search"

                'load repeater control
                BindData()

                'strDebug &= ";BindData Search"

                'handle if accordion should be opened or closed - default to closed
                If Request.Cookies("UGNDB_ShowECIAdvancedSearch") IsNot Nothing Then
                    If Request.Cookies("UGNDB_ShowECIAdvancedSearch").Value.Trim <> "" Then
                        If CType(Request.Cookies("UGNDB_ShowECIAdvancedSearch").Value, Integer) = 1 Then
                            accAdvancedSearch.SelectedIndex = 0
                            cbShowAdvancedSearch.Checked = True
                        Else
                            accAdvancedSearch.SelectedIndex = -1
                            cbShowAdvancedSearch.Checked = False
                        End If
                    End If

                Else
                    accAdvancedSearch.SelectedIndex = -1
                    cbShowAdvancedSearch.Checked = False
                End If

                EnableControls()

                'strDebug &= ";EnableControls Search"
            Else
                ' strDebug &= ";postback"

                If txtSearchECINo.Text.Trim <> "" Then
                    ViewState("ECINo") = txtSearchECINo.Text.Trim
                End If

                If txtSearchECIDesc.Text.Trim <> "" Then
                    ViewState("ECIDesc") = txtSearchECIDesc.Text.Trim
                End If

                If ddSearchECIType.SelectedIndex > 0 Then
                    ViewState("ECIType") = ddSearchECIType.SelectedValue
                End If

                If ddSearchStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddSearchStatus.SelectedValue
                End If

                If txtSearchIssueDate.Text.Length > 0 Then
                    ViewState("IssueDate") = txtSearchIssueDate.Text.Trim
                End If

                If txtSearchImplementationDate.Text.Length > 0 Then
                    ViewState("ImplementationDate") = txtSearchImplementationDate.Text.Trim
                End If

                If txtSearchRFDNo.Text.Length > 0 Then
                    ViewState("RFDNo") = txtSearchRFDNo.Text.Trim
                End If

                If txtSearchCostSheetID.Text.Length > 0 Then
                    ViewState("CostSheetID") = txtSearchCostSheetID.Text.Trim
                End If

                If ddSearchInitiatorTeamMember.SelectedIndex > 0 Then
                    ViewState("InitiatorTeamMemberID") = ddSearchInitiatorTeamMember.SelectedValue
                End If

                If txtSearchDrawingNo.Text.Length > 0 Then
                    ViewState("DrawingNo") = txtSearchDrawingNo.Text.Trim
                End If

                If txtSearchPartNo.Text.Length > 0 Then
                    ViewState("PartNo") = txtSearchPartNo.Text.Trim
                End If

                If txtSearchPartName.Text.Length > 0 Then
                    ViewState("PartName") = txtSearchPartName.Text.Trim
                End If

                If ddSearchCustomer.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddSearchCustomer.SelectedValue
                End If

                If txtSearchCustomerPartNo.Text.Length > 0 Then
                    ViewState("CustomerPartNo") = txtSearchCustomerPartNo.Text.Trim
                End If

                If txtSearchDesignLevel.Text.Length > 0 Then
                    ViewState("DesignLevel") = txtSearchDesignLevel.Text.Trim
                End If

                If ddSearchDesignationType.SelectedIndex > 0 Then
                    ViewState("DesignationType") = ddSearchDesignationType.SelectedValue
                End If

                If ddSearchBusinessProcessType.SelectedIndex > 0 Then
                    ViewState("BusinessProcessTypeID") = ddSearchBusinessProcessType.SelectedValue
                End If

                If ddSearchProgram.SelectedIndex > 0 Then
                    ViewState("ProgramID") = ddSearchProgram.SelectedValue
                End If

                If ddSearchCommodity.SelectedIndex > 0 Then
                    ViewState("CommodityID") = ddSearchCommodity.SelectedValue
                End If

                If ddSearchPurchasedGood.SelectedIndex > 0 Then
                    ViewState("PurchasedGoodID") = ddSearchPurchasedGood.SelectedValue
                End If

                If ddSearchProductTechnology.SelectedIndex > 0 Then
                    ViewState("ProductTechnologyID") = ddSearchProductTechnology.SelectedValue
                End If

                If ddSearchSubFamily.SelectedIndex > 0 Then
                    ViewState("SubFamilyID") = ddSearchSubFamily.SelectedValue
                End If

                If ddSearchUGNFacility.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddSearchUGNFacility.SelectedValue
                End If

                If ddSearchVendor.SelectedIndex > 0 Then
                    ViewState("UGNDBVendorID") = ddSearchVendor.SelectedValue
                End If

                If ddSearchAccountManager.SelectedIndex > 0 Then
                    ViewState("AccountManagerID") = ddSearchAccountManager.SelectedValue
                End If

                If ddSearchQualityEngineer.SelectedIndex > 0 Then
                    ViewState("QualityEngineerID") = ddSearchQualityEngineer.SelectedValue
                End If

                ViewState("isPPAP") = 0
                ViewState("filterPPAP") = 0

                If ddSearchPPAP.SelectedIndex > 0 Then
                    If ddSearchPPAP.SelectedValue = "Only" Then
                        ViewState("isPPAP") = 1
                        ViewState("filterPPAP") = 1
                    End If

                    If ddSearchPPAP.SelectedValue = "None" Then
                        ViewState("isPPAP") = 0
                        ViewState("filterPPAP") = 1
                    End If
                End If

                ViewState("isUgnIPP") = 0
                ViewState("filterUgnIPP") = 0

                If ddSearchUgnIPP.SelectedIndex > 0 Then
                    If ddSearchUgnIPP.SelectedValue = "Only" Then
                        ViewState("isUgnIPP") = 1
                        ViewState("filterUgnIPP") = 1
                    End If

                    If ddSearchUgnIPP.SelectedValue = "None" Then
                        ViewState("isUgnIpp") = 0
                        ViewState("filterUgnIPP") = 1
                    End If
                End If

                ViewState("isCustomerIpp") = 0
                ViewState("filterCustomerIpp") = 0

                If ddSearchCustomerIPP.SelectedIndex > 0 Then
                    If ddSearchCustomerIPP.SelectedValue = "Only" Then
                        ViewState("isCustomerIPP") = 1
                        ViewState("filterCustomerIPP") = 1
                    End If

                    If ddSearchCustomerIPP.SelectedValue = "None" Then
                        ViewState("isCustomerIPP") = 0
                        ViewState("filterCustomerIPP") = 1
                    End If
                End If

                If txtSearchLastUpdatedOnStartDate.Text.Length > 0 Then
                    ViewState("LastUpdatedOnStartDate") = txtSearchLastUpdatedOnStartDate.Text.Trim
                End If

                If txtSearchLastUpdatedOnEndDate.Text.Length > 0 Then
                    ViewState("LastUpdatedOnEndDate") = txtSearchLastUpdatedOnEndDate.Text.Trim
                End If

                If cbIncludeArchive.Checked = True Then
                    ViewState("IncludeArchive") = 1
                Else
                    ViewState("IncludeArchive") = 0
                End If

                'focus on ECINo field
                txtSearchECINo.Focus()
            End If

            If HttpContext.Current.Session("DeletedECI") IsNot Nothing Then
                If HttpContext.Current.Session("DeletedECI") <> "" Then
                    lblMessage.Text = "The ECI " & HttpContext.Current.Session("DeletedECI") & " was deleted."
                    HttpContext.Current.Session("DeletedECI") = Nothing
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message & strDebug, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        lblMessage.Text = ""

        Try

            Response.Redirect("ECI_Detail.aspx", False)

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

            ECIModule.DeleteECICookies()

            HttpContext.Current.Session("sessionECICurrentPage") = Nothing

            Response.Redirect("ECI_List.aspx", False)

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
            HttpContext.Current.Session("sessionECICurrentPage") = Nothing

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'set saved value of what criteria was used to search   
            ''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim iECINo As Integer = 0

            If IsNumeric(txtSearchECINo.Text.Trim) Then
                iECINo = CType(txtSearchECINo.Text.Trim, Integer)
            End If

            'if ECI Number to search for is less than 200000, then it is archive data. so the checkbox will be turned on
            If (iECINo < 200000 Or txtSearchECINo.Text.Trim.Length < 6) And txtSearchECINo.Text.Trim <> "" Then
                cbIncludeArchive.Checked = True
                accAdvancedSearch.SelectedIndex = 0
                cbShowAdvancedSearch.Checked = True
                Response.Cookies("UGNDB_ShowECIAdvancedSearch").Value = 1
            End If

            Response.Cookies("ECIModule_SaveECINoSearch").Value = txtSearchECINo.Text.Trim

            Response.Cookies("ECIModule_SaveECIDescSearch").Value = txtSearchECIDesc.Text.Trim

            If ddSearchECIType.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveECITypeSearch").Value = ddSearchECIType.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveECITypeSearch").Value = ""
                Response.Cookies("ECIModule_SaveECITypeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchStatus.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveStatusIDSearch").Value = ddSearchStatus.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveStatusIDSearch").Value = ""
                Response.Cookies("ECIModule_SaveStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ECIModule_SaveIssueDateSearch").Value = txtSearchIssueDate.Text.Trim

            Response.Cookies("ECIModule_SaveImplementationDateSearch").Value = txtSearchImplementationDate.Text.Trim

            Response.Cookies("ECIModule_SaveRFDNoSearch").Value = txtSearchRFDNo.Text.Trim

            Response.Cookies("ECIModule_SaveCostSheetIDSearch").Value = txtSearchCostSheetID.Text.Trim

            If ddSearchInitiatorTeamMember.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value = ddSearchInitiatorTeamMember.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveInitiatorTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ECIModule_SaveDrawingNoSearch").Value = txtSearchDrawingNo.Text.Trim

            Response.Cookies("ECIModule_SavePartNoSearch").Value = txtSearchPartNo.Text.Trim

            Response.Cookies("ECIModule_SavePartNameSearch").Value = txtSearchPartName.Text.Trim

            If ddSearchCustomer.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveCustomerSearch").Value = ddSearchCustomer.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveCustomerSearch").Value = 0
                Response.Cookies("ECIModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ECIModule_SaveCustomerPartNoSearch").Value = txtSearchCustomerPartNo.Text.Trim

            Response.Cookies("ECIModule_SaveDesignLevelSearch").Value = txtSearchDesignLevel.Text.Trim

            If ddSearchDesignationType.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveDesignationTypeSearch").Value = ddSearchDesignationType.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveDesignationTypeSearch").Value = ""
                Response.Cookies("ECIModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchBusinessProcessType.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value = ddSearchBusinessProcessType.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveBusinessProcessTypeIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProgram.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveProgramIDSearch").Value = ddSearchProgram.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveProgramIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchCommodity.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveCommodityIDSearch").Value = ddSearchCommodity.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveCommodityIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveCommodityIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchPurchasedGood.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value = ddSearchPurchasedGood.SelectedValue
            Else
                Response.Cookies("ECIModule_SavePurchasedGoodIDSearch").Value = 0
                Response.Cookies("ECIModule_SavePurchasedGoodIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProductTechnology.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value = ddSearchProductTechnology.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveProductTechnologyIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveProductTechnologyIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchSubFamily.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveSubFamilyIDSearch").Value = ddSearchSubFamily.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveSubFamilyIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveSubFamilyIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveUGNFacilitySearch").Value = ddSearchUGNFacility.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveUGNFacilitySearch").Value = ""
                Response.Cookies("ECIModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchVendor.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value = ddSearchVendor.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveUGNDBVendorIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchAccountManager.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveAccountManagerIDSearch").Value = ddSearchAccountManager.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveAccountManagerIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchQualityEngineer.SelectedIndex > 0 Then
                Response.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value = ddSearchQualityEngineer.SelectedValue
            Else
                Response.Cookies("ECIModule_SaveQualityEngineerIDSearch").Value = 0
                Response.Cookies("ECIModule_SaveQualityEngineerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            ViewState("IsPPAP") = 0
            Response.Cookies("ECIModule_SaveIsPPAPSearch").Value = 0

            ViewState("filterPAPP") = 0
            Response.Cookies("ECIModule_SaveFilterPPAPSearch").Value = 0

            If ddSearchPPAP.SelectedIndex > 0 Then
                If ddSearchPPAP.SelectedValue = "Only" Then
                    ViewState("IsPPAP") = 1
                    Response.Cookies("ECIModule_SaveIsPPAPSearch").Value = 1
                    ViewState("filterPAPP") = 1
                    Response.Cookies("ECIModule_SaveFilterPPAPSearch").Value = 1
                End If

                If ddSearchPPAP.SelectedValue = "None" Then
                    ViewState("IsPPAP") = 0
                    Response.Cookies("ECIModule_SaveIsPPAPSearch").Value = 0
                    ViewState("filterPPAP") = 1
                    Response.Cookies("ECIModule_SaveFilterPPAPSearch").Value = 1
                End If
            End If

            ViewState("isUgnIPP") = 0
            Response.Cookies("ECIModule_SaveIsUgnIPPSearch").Value = 0

            ViewState("filterUgnIPP") = 0
            Response.Cookies("ECIModule_SaveFilterUgnIPPSearch").Value = 0

            If ddSearchUgnIPP.SelectedIndex > 0 Then
                If ddSearchUgnIPP.SelectedValue = "Only" Then
                    ViewState("isUgnIPP") = 1
                    Response.Cookies("ECIModule_SaveIsUgnIPPSearch").Value = 1
                    ViewState("filterUgnIPP") = 1
                    Response.Cookies("ECIModule_SaveFilterUgnIPPSearch").Value = 1
                End If

                If ddSearchUgnIPP.SelectedValue = "None" Then
                    ViewState("isUgnIPP") = 0
                    Response.Cookies("ECIModule_SaveIsUgnIPPSearch").Value = 0
                    ViewState("filterUgnIPP") = 1
                    Response.Cookies("ECIModule_SaveFilterUgnIPPSearch").Value = 1
                End If
            End If

            ViewState("isCustomerIPP") = 0
            Response.Cookies("ECIModule_SaveIsCustomerIPPSearch").Value = 0

            ViewState("filterCustomerIPP") = 0
            Response.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Value = 0

            If ddSearchCustomerIPP.SelectedIndex > 0 Then
                If ddSearchCustomerIPP.SelectedValue = "Only" Then
                    ViewState("isCustomerIPP") = 1
                    Response.Cookies("ECIModule_SaveIsCustomerIPPSearch").Value = 1
                    ViewState("filterCustomerIPP") = 1
                    Response.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Value = 1
                End If

                If ddSearchCustomerIPP.SelectedValue = "None" Then
                    ViewState("isCustomerIPP") = 0
                    Response.Cookies("ECIModule_SaveIsCustomerIPPSearch").Value = 0
                    ViewState("filterCustomerIPP") = 1
                    Response.Cookies("ECIModule_SaveFilterCustomerIPPSearch").Value = 1
                End If
            End If

            Response.Cookies("ECIModule_SaveLastUpdatedOnStartDateSearch").Value = txtSearchLastUpdatedOnStartDate.Text.Trim

            Response.Cookies("ECIModule_SaveLastUpdatedOnEndDateSearch").Value = txtSearchLastUpdatedOnEndDate.Text.Trim

            If cbIncludeArchive.Checked = True Then
                Response.Cookies("ECIModule_SaveIncludeArchiveSearch").Value = 1
                ViewState("IncludeArchive") = 1
            Else
                Response.Cookies("ECIModule_SaveIncludeArchiveSearch").Value = 0
                ViewState("IncludeArchive") = 0
                Response.Cookies("ECIModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Redirect("ECI_List.aspx?ECINo=" & Server.UrlEncode(txtSearchECINo.Text.Trim) _
            & "&ECIDesc=" & Server.UrlEncode(txtSearchECIDesc.Text.Trim) _
            & "&ECIType=" & Server.UrlEncode(ddSearchECIType.SelectedValue) _
            & "&StatusID=" & Server.UrlEncode(ddSearchStatus.SelectedValue) _
            & "&IssueDate=" & Server.UrlEncode(txtSearchIssueDate.Text.Trim) _
            & "&ImplementationDate=" & Server.UrlEncode(txtSearchImplementationDate.Text.Trim) _
            & "&RFDNo=" & Server.UrlEncode(txtSearchRFDNo.Text.Trim) _
            & "&CostSheetID=" & Server.UrlEncode(txtSearchCostSheetID.Text.Trim) _
            & "&InitiatorTeamMemberID=" & Server.UrlEncode(ddSearchInitiatorTeamMember.SelectedValue) _
            & "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNo.Text.Trim) _
            & "&PartNo=" & Server.UrlEncode(txtSearchPartNo.Text.Trim) _
            & "&PartName=" & Server.UrlEncode(txtSearchPartName.Text.Trim) _
            & "&CustomerValue=" & Server.UrlEncode(ddSearchCustomer.SelectedValue) _
            & "&CustomerPartNo=" & Server.UrlEncode(txtSearchCustomerPartNo.Text.Trim) _
            & "&DesignLevel=" & Server.UrlEncode(txtSearchDesignLevel.Text.Trim) _
            & "&DesignationType=" & Server.UrlEncode(ddSearchDesignationType.SelectedValue) _
            & "&BusinessProcessTypeID=" & Server.UrlEncode(ddSearchBusinessProcessType.SelectedValue) _
            & "&ProgramID=" & Server.UrlEncode(ddSearchProgram.SelectedValue) _
            & "&CommodityID=" & Server.UrlEncode(ddSearchCommodity.SelectedValue) _
            & "&PurchasedGoodID=" & Server.UrlEncode(ddSearchPurchasedGood.SelectedValue) _
            & "&ProductTechnologyID=" & Server.UrlEncode(ddSearchProductTechnology.SelectedValue) _
            & "&SubFamilyID=" & Server.UrlEncode(ddSearchSubFamily.SelectedValue) _
            & "&UGNFacility=" & Server.UrlEncode(ddSearchUGNFacility.SelectedValue) _
            & "&UGNDBVendorID=" & Server.UrlEncode(ddSearchVendor.SelectedValue) _
            & "&AccountManagerID=" & Server.UrlEncode(ddSearchAccountManager.SelectedValue) _
            & "&QualityEngineerID=" & Server.UrlEncode(ddSearchQualityEngineer.SelectedValue) _
            & "&filterPPAP=" & ViewState("filterPPAP") _
            & "&isPPAP=" & ViewState("isPPAP") _
            & "&filterUgnIPP=" & ViewState("filterUgnIPP") _
            & "&isUgnIPP=" & ViewState("isUgnIPP") _
            & "&filterCustomerIPP=" & ViewState("filterCustomerIPP") _
            & "&isCustomerIPP=" & ViewState("isCustomerIPP") _
            & "&LastUpdatedOnStartDate=" & Server.UrlEncode(txtSearchLastUpdatedOnStartDate.Text.Trim) _
            & "&LastUpdatedOnEndDate=" & Server.UrlEncode(txtSearchLastUpdatedOnEndDate.Text.Trim) _
            & "&IncludeArchive=" & ViewState("IncludeArchive") _
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

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionECICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionECICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionECICurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionECICurrentPage") = CurrentPage

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

                HttpContext.Current.Session("sessionECICurrentPage") = CurrentPage

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

    Protected Sub cbShowAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowAdvancedSearch.CheckedChanged

        Try
            lblMessage.Text = ""

            If cbShowAdvancedSearch.Checked = False Then
                Response.Cookies("UGNDB_ShowECIAdvancedSearch").Value = 0
                accAdvancedSearch.SelectedIndex = -1
            Else
                Response.Cookies("UGNDB_ShowECIAdvancedSearch").Value = 1
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

    Protected Sub cbIncludeArchive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbIncludeArchive.CheckedChanged

        Try
            lblMessage.Text = ""

            CheckIncludeArchives()

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
