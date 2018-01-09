' ************************************************************************************************
'
' Name:		drawinglist.aspx
' Purpose:	This Code Behind is for the main page of the PE Drawings Management App
'
' Date		Author	    
' 05/22/2008 Roderick Carlson - copied from pre-integration version
' 10/20/2008 Roderick Carlson - added CABBV to page
'                             - Server.UrlDecode seems to mess up partial searches if % is in front of text
' 12/03/2008 Roderick Carlson - In BindCriteria, checked if ds was null
' 02/04/2009 Roderick Carlson - added enhanced navigation page bar
' 05/21/2009 Roderick Carlson - PDE #2714 - Server.UrlDecode messes up a lot. But Server.URLEncode is still needed
' 05/28/2009 Roderick Carlson - PDE #2715 - added vehicle year
' 06/04/2009 Roderick Carlson - Added SoldTo and DesignationType, removed IncludeBOM check box
' 07/08/2009 Roderick Carlson - Put release type on result/repeator list column
' 07/28/2009 Roderick Carlson - PDE # 2731 - put packaging info in normal preview - put all crystal reports in popups, saved last tab used
' 08/21/2009 Roderick Carlson - Put BPCS Part Info in SubTable
' 09/03/2009 Roderick Carlson - Put accordion control on search page for advanced searching
' 09/22/2009 Roderick Carlson - Changed Search function to getDrawingSearch and removed PartName from Result Set
' 02/16/2010 Roderick Carlson - PDE-2836 - Add Search By Make
' 06/28/2010 Roderick Carlson - PDE-2909 - Release Type Work
' 12/17/2010 Roderick Carlson - Made Production the Default Search Release Type
' 12/20/2013    LRey        Replaced "SoldTo|CABBV" to "PartNo" wherever used. Customer DDL to OEMManufacturer.
' 01/06/2014    LRey        Replaced "BPCSPart " to "Part" wherever used.
' ************************************************************************************************

Partial Class DrawingList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    Protected WithEvents lnkDrawing As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkOldPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDensityValue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAMDValue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkWMDValue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkReleaseType As System.Web.UI.WebControls.LinkButton

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Drawing"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > Drawing Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("DMGExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

#End Region
    Protected Sub EnabledControls()

        btnAdd.Enabled = ViewState("isAdmin")

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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 35)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    If iRoleID = 11 Then ' ADMIN RIGHTS                                                               
                        ViewState("isAdmin") = True
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Session("DMSTabSelected") = Nothing
            Session("DMS-Parent-Complete") = Nothing

            If HttpContext.Current.Session("sessionDrawingCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionDrawingCurrentPage")
            End If

            If Not Page.IsPostBack Then
                ViewState("lnkStatus") = "ASC"
                ViewState("lnkDrawing") = "ASC"
                ViewState("lnkPartNo1") = "ASC"
                ViewState("lnkPartName") = "ASC"

                ViewState("DrawingNo") = ""
                ViewState("CustomerPartNo") = ""
                ViewState("Customer") = ""
                ViewState("CustomerValue") = ""
                ViewState("DesignationType") = ""
                ViewState("PartNo") = ""
                ViewState("PartName") = ""
                ViewState("Commodity") = 0
                ViewState("PurchasedGood") = 0
                ViewState("VehicleYear") = 0
                ViewState("Program") = 0
                ViewState("SubFamily") = 0
                ViewState("DensityValue") = 0
                ViewState("DrawingByEngineer") = 0
                ViewState("Construction") = ""
                ViewState("Notes") = ""
                ViewState("ReleaseType") = 1 'default search to the production release type
                ViewState("Status") = ""
                ViewState("IncludeBOM") = False
                ViewState("DrawingDateStart") = ""
                ViewState("DrawingDateEnd") = ""
                ViewState("Make") = ""
                ViewState("ProductTechnology") = 0

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("CustomerPartNo") <> "" Then
                    txtCustomerPartNo.Text = HttpContext.Current.Request.QueryString("CustomerPartNo")
                    ViewState("CustomerPartNo") = HttpContext.Current.Request.QueryString("CustomerPartNo")
                Else
                    If Not Request.Cookies("PEModule_SaveCustomerPartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveCustomerPartNoSearch").Value) <> "" Then
                            txtCustomerPartNo.Text = Request.Cookies("PEModule_SaveCustomerPartNoSearch").Value
                            ViewState("CustomerPartNo") = Request.Cookies("PEModule_SaveCustomerPartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerValue") <> "" Then
                    ddCustomer.SelectedValue = HttpContext.Current.Request.QueryString("CustomerValue")
                    ViewState("CustomerValue") = HttpContext.Current.Request.QueryString("CustomerValue")
                Else
                    If Not Request.Cookies("PEModule_SaveCustomerSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveCustomerSearch").Value) <> "" Then
                            ddCustomer.SelectedValue = Request.Cookies("PEModule_SaveCustomerSearch").Value
                            ViewState("CustomerValue") = Request.Cookies("PEModule_SaveCustomerSearch").Value
                        End If
                    End If
                End If

                ViewState("Customer") = ViewState("CustomerValue")

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("PEModule_SaveDrawingNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDrawingNoSearch").Value) <> "" Then
                            txtDrawingNo.Text = Request.Cookies("PEModule_SaveDrawingNoSearch").Value
                            ViewState("DrawingNo") = Request.Cookies("PEModule_SaveDrawingNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("PEModule_SavePartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SavePartNoSearch").Value) <> "" Then
                            txtPartNo.Text = Request.Cookies("PEModule_SavePartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("PEModule_SavePartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("PEModule_SavePartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SavePartNameSearch").Value) <> "" Then
                            txtPartName.Text = Request.Cookies("PEModule_SavePartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("PEModule_SavePartNameSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Commodity") <> "" Then
                    ddCommodity.SelectedValue = HttpContext.Current.Request.QueryString("Commodity")
                    ViewState("Commodity") = HttpContext.Current.Request.QueryString("Commodity")
                Else
                    If Not Request.Cookies("PEModule_SaveCommoditySearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveCommoditySearch").Value) <> "" Then
                            ddCommodity.SelectedValue = Request.Cookies("PEModule_SaveCommoditySearch").Value
                            ViewState("Commodity") = Request.Cookies("PEModule_SaveCommoditySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PurchasedGood") <> "" Then
                    ddPurchasedGood.SelectedValue = HttpContext.Current.Request.QueryString("PurchasedGood")
                    ViewState("PurchasedGood") = HttpContext.Current.Request.QueryString("PurchasedGood")
                Else
                    If Not Request.Cookies("PEModule_SavePurchasedGoodSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SavePurchasedGoodSearch").Value) <> "" Then
                            ddPurchasedGood.SelectedValue = Request.Cookies("PEModule_SavePurchasedGoodSearch").Value
                            ViewState("PurchasedGood") = Request.Cookies("PEModule_SavePurchasedGoodSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("VehicleYear") <> "" Then
                    ddYear.SelectedValue = HttpContext.Current.Request.QueryString("VehicleYear")
                    ViewState("VehicleYear") = HttpContext.Current.Request.QueryString("VehicleYear")
                Else
                    If Not Request.Cookies("PEModule_SaveVehicleYearSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveVehicleYearSearch").Value) <> "" Then
                            ddYear.SelectedValue = Request.Cookies("PEModule_SaveVehicleYearSearch").Value
                            ViewState("VehicleYear") = Request.Cookies("PEModule_SaveVehicleYearSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Program") <> "" Then
                    ddProgram.SelectedValue = HttpContext.Current.Request.QueryString("Program")
                    ViewState("Program") = HttpContext.Current.Request.QueryString("Program")
                Else
                    If Not Request.Cookies("PEModule_SaveProgramSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveProgramSearch").Value) <> "" Then
                            ddProgram.SelectedValue = Request.Cookies("PEModule_SaveProgramSearch").Value
                            ViewState("Program") = Request.Cookies("PEModule_SaveProgramSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("SubFamily") <> "" Then
                    ddSubFamily.SelectedValue = HttpContext.Current.Request.QueryString("SubFamily")
                    ViewState("SubFamily") = HttpContext.Current.Request.QueryString("SubFamily")
                Else
                    If Not Request.Cookies("PEModule_SaveSubFamilySearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveSubFamilySearch").Value) <> "" Then
                            ddSubFamily.SelectedValue = Request.Cookies("PEModule_SaveSubFamilySearch").Value
                            ViewState("SubFamily") = Request.Cookies("PEModule_SaveSubFamilySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DensityValue") <> "" Then
                    ddDensityValue.SelectedValue = HttpContext.Current.Request.QueryString("DensityValue")
                    ViewState("DensityValue") = HttpContext.Current.Request.QueryString("DensityValue")
                Else
                    If Not Request.Cookies("PEModule_SaveDensityValueSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDensityValueSearch").Value) <> "" Then
                            ddDensityValue.SelectedValue = Request.Cookies("PEModule_SaveDensityValueSearch").Value
                            ViewState("DensityValue") = Request.Cookies("PEModule_SaveDensityValueSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingByEngineer") <> "" Then
                    ddDrawingByEngineer.SelectedValue = HttpContext.Current.Request.QueryString("DrawingByEngineer")
                    ViewState("DrawingByEngineer") = HttpContext.Current.Request.QueryString("DrawingByEngineer")
                Else
                    If Not Request.Cookies("PEModule_SaveDrawingByEngineerSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDrawingByEngineerSearch").Value) <> "" Then
                            ddDrawingByEngineer.SelectedValue = Request.Cookies("PEModule_SaveDrawingByEngineerSearch").Value
                            ViewState("DrawingByEngineer") = Request.Cookies("PEModule_SaveDrawingByEngineerSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Construction") <> "" Then
                    txtConstruction.Text = HttpContext.Current.Request.QueryString("Construction")
                    ViewState("Construction") = HttpContext.Current.Request.QueryString("Construction")
                Else
                    If Not Request.Cookies("PEModule_SaveConstructionSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveConstructionSearch").Value) <> "" Then
                            txtConstruction.Text = Request.Cookies("PEModule_SaveConstructionSearch").Value
                            ViewState("Construction") = Request.Cookies("PEModule_SaveConstructionSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Notes") <> "" Then
                    txtNotes.Text = HttpContext.Current.Request.QueryString("Notes")
                    ViewState("Notes") = HttpContext.Current.Request.QueryString("Notes")
                Else
                    If Not Request.Cookies("PEModule_SaveNotesSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveNotesSearch").Value) <> "" Then
                            txtNotes.Text = Request.Cookies("PEModule_SaveNotesSearch").Value
                            ViewState("Notes") = Request.Cookies("PEModule_SaveNotesSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ReleaseType") <> "" Then
                    'ddReleaseType.SelectedValue = HttpContext.Current.Request.QueryString("ReleaseType")
                    ViewState("ReleaseType") = HttpContext.Current.Request.QueryString("ReleaseType")
                Else
                    If Not Request.Cookies("PEModule_SaveReleaseTypeSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveReleaseTypeSearch").Value) <> "" Then
                            'ddReleaseType.SelectedValue = Request.Cookies("PEModule_SaveReleaseTypeSearch").Value
                            ViewState("ReleaseType") = Request.Cookies("PEModule_SaveReleaseTypeSearch").Value
                        End If
                    End If
                End If

                If ViewState("ReleaseType") > 0 Then
                    ddReleaseType.SelectedValue = ViewState("ReleaseType")
                Else
                    ddReleaseType.SelectedIndex = -1
                End If

                If HttpContext.Current.Request.QueryString("Status") <> "" Then
                    ddStatus.SelectedValue = HttpContext.Current.Request.QueryString("Status")
                    ViewState("Status") = HttpContext.Current.Request.QueryString("Status")
                Else
                    If Not Request.Cookies("PEModule_SaveStatusSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveStatusSearch").Value) <> "" Then
                            ddStatus.SelectedValue = Request.Cookies("PEModule_SaveStatusSearch").Value
                            ViewState("Status") = Request.Cookies("PEModule_SaveStatusSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DesignationType") <> "" Then
                    ddDesignationType.SelectedValue = HttpContext.Current.Request.QueryString("DesignationType")
                    ViewState("DesignationType") = HttpContext.Current.Request.QueryString("DesignationType")
                Else
                    If Not Request.Cookies("PEModule_SaveDesignationTypeSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDesignationTypeSearch").Value) <> "" Then
                            ddDesignationType.SelectedValue = Request.Cookies("PEModule_SaveDesignationTypeSearch").Value
                            ViewState("DesignationType") = Request.Cookies("PEModule_SaveDesignationTypeSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingDateStart") <> "" Then
                    txtLastUpdatedOnStart.Text = HttpContext.Current.Request.QueryString("DrawingDateStart")
                    ViewState("DrawingDateStart") = HttpContext.Current.Request.QueryString("DrawingDateStart")
                Else
                    If Not Request.Cookies("PEModule_SaveDrawingDateStartSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDrawingDateStartSearch").Value) <> "" Then
                            txtLastUpdatedOnStart.Text = Request.Cookies("PEModule_SaveDrawingDateStartSearch").Value
                            ViewState("DrawingDateStart") = Request.Cookies("PEModule_SaveDrawingDateStartSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingDateEnd") <> "" Then
                    txtLastUpdatedOnEnd.Text = HttpContext.Current.Request.QueryString("DrawingDateEnd")
                    ViewState("DrawingDateEnd") = HttpContext.Current.Request.QueryString("DrawingDateEnd")
                Else
                    If Not Request.Cookies("PEModule_SaveDrawingDateEndSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveDrawingDateEndSearch").Value) <> "" Then
                            txtLastUpdatedOnEnd.Text = Request.Cookies("PEModule_SaveDrawingDateEndSearch").Value
                            ViewState("DrawingDateEnd") = Request.Cookies("PEModule_SaveDrawingDateEndSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("Make") <> "" Then
                    ddMake.SelectedValue = HttpContext.Current.Request.QueryString("Make")
                    ViewState("Make") = HttpContext.Current.Request.QueryString("Make")
                Else
                    If Not Request.Cookies("PEModule_SaveMakeSearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMakeSearch").Value) <> "" Then
                            ddMake.SelectedValue = Request.Cookies("PEModule_SaveMakeSearch").Value
                            ViewState("Make") = Request.Cookies("PEModule_SaveMakeSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ProductTechnology") <> "" Then
                    ddProductTechnology.SelectedValue = HttpContext.Current.Request.QueryString("ProductTechnology")
                    ViewState("ProductTechnology") = HttpContext.Current.Request.QueryString("ProductTechnology")
                Else
                    If Not Request.Cookies("PEModule_SaveProductTechnologySearch") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveProductTechnologySearch").Value) <> "" Then
                            ddProductTechnology.SelectedValue = Request.Cookies("PEModule_SaveProductTechnologySearch").Value
                            ViewState("ProductTechnology") = Request.Cookies("PEModule_SaveProductTechnologySearch").Value
                        End If
                    End If
                End If

                'load repeater control
                BindData()

                'handle if accordion should be opened or closed - default to closed
                If Request.Cookies("UGNDB_ShowDrawingAdvancedSearch") IsNot Nothing Then
                    If Request.Cookies("UGNDB_ShowDrawingAdvancedSearch").Value.Trim <> "" Then
                        If CType(Request.Cookies("UGNDB_ShowDrawingAdvancedSearch").Value, Integer) = 1 Then
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
            Else
                ViewState("DrawingNo") = txtDrawingNo.Text
                ViewState("CustomerPartNo") = txtCustomerPartNo.Text

                If ddCustomer.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddCustomer.SelectedValue
                    ViewState("Customer") = ddCustomer.SelectedValue
                End If

                ViewState("PartNo") = txtPartNo.Text
                ViewState("PartName") = txtPartName.Text

                If ddCommodity.SelectedIndex > 0 Then
                    ViewState("Commodity") = ddCommodity.SelectedValue()
                End If

                If ddPurchasedGood.SelectedIndex > 0 Then
                    ViewState("PurchasedGood") = ddPurchasedGood.SelectedValue()
                End If

                If ddYear.SelectedIndex > 0 Then
                    ViewState("VehicleYear") = ddYear.SelectedValue()
                End If

                If ddProgram.SelectedIndex > 0 Then
                    ViewState("Program") = ddProgram.SelectedValue()
                End If

                If ddSubFamily.SelectedIndex > 0 Then
                    ViewState("SubFamily") = ddSubFamily.SelectedValue()
                End If

                If ddDensityValue.SelectedIndex > 0 Then
                    ViewState("DensityValue") = ddDensityValue.SelectedValue()
                End If

                If ddDrawingByEngineer.SelectedIndex > 0 Then
                    ViewState("DrawingByEngineer") = ddDrawingByEngineer.SelectedValue()
                End If

                ViewState("Construction") = txtConstruction.Text
                ViewState("Notes") = txtNotes.Text

                If ddReleaseType.SelectedIndex > 0 Then
                    ViewState("ReleaseType") = ddReleaseType.SelectedValue()
                Else
                    ViewState("ReleaseType") = 0
                End If

                ViewState("Status") = ddStatus.SelectedValue()

                ViewState("DrawingDateStart") = txtLastUpdatedOnStart.Text
                ViewState("DrawingDateEnd") = txtLastUpdatedOnEnd.Text

                If ddMake.SelectedIndex > 0 Then
                    ViewState("Make") = ddMake.SelectedValue()
                End If

                If ddProductTechnology.SelectedIndex > 0 Then
                    ViewState("ProductTechnology") = ddProductTechnology.SelectedValue()
                End If
            End If

            'clear crystal reports
            PEModule.CleanPEDMScrystalReports()

            CheckRights()

            EnabledControls()

            'focus on DrawingNo field
            txtDrawingNo.Focus()
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

            Dim ds As DataSet

            'bind existing Part/Drawing data to repeater control at bottom of screen   
            ds = PEModule.GetDrawingSearch(ViewState("DrawingNo"), ViewState("ReleaseType"), ViewState("PartNo"), ViewState("PartName"), ViewState("CustomerPartNo"), ViewState("Customer"), ViewState("DesignationType"), ViewState("VehicleYear"), ViewState("Program"), ViewState("SubFamily"), ViewState("Commodity"), ViewState("PurchasedGood"), ViewState("DensityValue"), ViewState("Construction"), ViewState("Status"), ViewState("Notes"), ViewState("DrawingByEngineer"), False, ViewState("DrawingDateStart"), ViewState("DrawingDateEnd"), ViewState("Make"), ViewState("ProductTechnology"))

            If commonFunctions.CheckDataset(ds) = True Then

                rpDrawingInfo.DataSource = ds
                rpDrawingInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpDrawingInfo.DataSource = objPds
                rpDrawingInfo.DataBind()

                'lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()

                '' Disable Prev or Next buttons if necessary
                'cmdPrev.Enabled = Not objPds.IsFirstPage
                'cmdNext.Enabled = Not objPds.IsLastPage
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
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataset(ds) = True Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
                ddDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgramMake()
            If commonFunctions.CheckDataset(ds) = True Then
                ddMake.DataSource = ds
                ddMake.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddMake.DataValueField = ds.Tables(0).Columns("Make").ColumnName
                ddMake.DataBind()
                ddMake.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology(0)
            If (commonFunctions.CheckDataset(ds) = True) Then
                ddProductTechnology.DataSource = ds
                ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName
                ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddProductTechnology.DataBind()
                ddProductTechnology.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataset(ds) = True Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataset(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingDensity()
            If commonFunctions.CheckDataset(ds) = True Then
                ddDensityValue.DataSource = ds
                ddDensityValue.DataTextField = ds.Tables(0).Columns("densityValue").ColumnName
                ddDensityValue.DataValueField = ds.Tables(0).Columns("densityValue").ColumnName
                ddDensityValue.DataBind()
                ddDensityValue.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataset(ds) = True Then
                ddPurchasedGood.DataSource = ds
                ddPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGood.DataBind()
                ddPurchasedGood.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingReleaseTypeList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddReleaseType.DataSource = ds
                ddReleaseType.DataTextField = ds.Tables(0).Columns("ddReleaseTypeName").ColumnName
                ddReleaseType.DataValueField = ds.Tables(0).Columns("ReleaseTypeID").ColumnName
                ddReleaseType.DataBind()
                ddReleaseType.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingByEngineers
            If commonFunctions.CheckDataset(ds) = True Then
                ddDrawingByEngineer.DataSource = ds
                ddDrawingByEngineer.DataTextField = ds.Tables(0).Columns("DrawingByEngineerFullName").ColumnName.ToString()
                ddDrawingByEngineer.DataValueField = ds.Tables(0).Columns("DrawingByEngineerID").ColumnName
                ddDrawingByEngineer.DataBind()
                ddDrawingByEngineer.Items.Insert(0, "")
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

            ds = PEModule.GetDrawingSearch(ViewState("DrawingNo"), ViewState("ReleaseType"), ViewState("PartNo"), _
            ViewState("PartName"), ViewState("CustomerPartNo"), ViewState("Customer"), _
            ViewState("DesignationType"), ViewState("VehicleYear"), ViewState("Program"), _
            ViewState("SubFamily"), ViewState("Commodity"), ViewState("PurchasedGood"), ViewState("DensityValue"), _
            ViewState("Construction"), ViewState("Status"), ViewState("Notes"), ViewState("DrawingByEngineer"), False, _
            ViewState("DrawingDateStart"), ViewState("DrawingDateEnd"), ViewState("Make"), ViewState("ProductTechnology"))

            If commonFunctions.CheckDataset(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpDrawingInfo.DataSource = dv
                rpDrawingInfo.DataBind()

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
        Handles lnkDrawing.Click, lnkPartNo.Click, lnkPartName.Click, lnkOldPartName.Click, lnkAMDValue.Click, lnkDensityValue.Click, lnkWMDValue.Click, lnkStatus.Click, lnkReleaseType.Click

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
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            PEModule.DeletePECookies()
            HttpContext.Current.Session("sessionDrawingCurrentPage") = Nothing

            Response.Redirect("DrawingList.aspx", False)
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
    Private Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            lblMessage.Text = ""

            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionDrawingCurrentPage") = CurrentPage

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
    Private Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            lblMessage.Text = ""

            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionDrawingCurrentPage") = CurrentPage

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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            HttpContext.Current.Session("sessionDrawingCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("PEModule_SaveDrawingNoSearch").Value = txtDrawingNo.Text
            Response.Cookies("PEModule_SaveCustomerPartNoSearch").Value = txtCustomerPartNo.Text

            If ddDesignationType.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveDesignationTypeSearch").Value = ddDesignationType.SelectedValue
            Else
                Response.Cookies("PEModule_SaveDesignationTypeSearch").Value = 0
                Response.Cookies("PEModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddCustomer.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveCustomerSearch").Value = ddCustomer.SelectedValue
            Else
                Response.Cookies("PEModule_SaveCustomerSearch").Value = ""
                Response.Cookies("PEModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("PEModule_SavePartNoSearch").Value = txtPartNo.Text
            Response.Cookies("PEModule_SavePartNameSearch").Value = txtPartName.Text

            If ddCommodity.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveCommoditySearch").Value = ddCommodity.SelectedValue
            Else
                Response.Cookies("PEModule_SaveCommoditySearch").Value = 0
                Response.Cookies("PEModule_SaveCommoditySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddPurchasedGood.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SavePurchasedGoodSearch").Value = ddPurchasedGood.SelectedValue
            Else
                Response.Cookies("PEModule_SavePurchasedGoodSearch").Value = 0
                Response.Cookies("PEModule_SavePurchasedGoodSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddYear.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveVehicleYearSearch").Value = ddYear.SelectedValue
            Else
                Response.Cookies("PEModule_SaveVehicleYearSearch").Value = 0
                Response.Cookies("PEModule_SaveVehicleYearSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddProgram.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveProgramSearch").Value = ddProgram.SelectedValue
            Else
                Response.Cookies("PEModule_SaveProgramSearch").Value = 0
                Response.Cookies("PEModule_SaveProgramSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSubFamily.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveSubFamilySearch").Value = ddSubFamily.SelectedValue
            Else
                Response.Cookies("PEModule_SaveSubFamilySearch").Value = 0
                Response.Cookies("PEModule_SaveSubFamilySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddDensityValue.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveDensityValueSearch").Value = ddDensityValue.SelectedValue
            Else
                Response.Cookies("PEModule_SaveDensityValueSearch").Value = 0
                Response.Cookies("PEModule_SaveDensityValueSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddDrawingByEngineer.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Value = ddDrawingByEngineer.SelectedValue
            Else
                Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Value = 0
                Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("PEModule_SaveConstructionSearch").Value = txtConstruction.Text
            Response.Cookies("PEModule_SaveNotesSearch").Value = txtNotes.Text

            If ddReleaseType.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveReleaseTypeSearch").Value = ddReleaseType.SelectedValue
            Else
                Response.Cookies("PEModule_SaveReleaseTypeSearch").Value = 0
            End If

            Response.Cookies("PEModule_SaveStatusSearch").Value = ddStatus.SelectedValue()

            Response.Cookies("PEModule_SaveDrawingDateStartSearch").Value = txtLastUpdatedOnStart.Text
            Response.Cookies("PEModule_SaveDrawingDateEndSearch").Value = txtLastUpdatedOnEnd.Text

            If ddMake.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveMakeSearch").Value = ddMake.SelectedValue
            Else
                Response.Cookies("PEModule_SaveMakeSearch").Value = 0
                Response.Cookies("PEModule_SaveMakeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddProductTechnology.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveProductTechnologySearch").Value = ddProductTechnology.SelectedValue
            Else
                Response.Cookies("PEModule_SaveProductTechnologySearch").Value = 0
                Response.Cookies("PEModule_SaveProductTechnologySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Redirect("DrawingList.aspx?DrawingNo=" & Server.UrlEncode(txtDrawingNo.Text.Trim) _
            & "&CustomerPartNo=" & Server.UrlEncode(txtCustomerPartNo.Text.Trim) _
            & "&DesignationType=" & ddDesignationType.SelectedValue _
            & "&CustomerValue=" & ddCustomer.SelectedValue _
            & "&PartNo=" & Server.UrlEncode(txtPartNo.Text.Trim) _
            & "&PartName=" & Server.UrlEncode(txtPartName.Text.Trim) _
            & "&Commodity=" & Server.UrlEncode(ddCommodity.SelectedValue) _
            & "&PurchasedGood=" & Server.UrlEncode(ddPurchasedGood.SelectedValue) _
            & "&VehicleYear=" & Server.UrlEncode(ddYear.SelectedValue) _
            & "&Program=" & Server.UrlEncode(ddProgram.SelectedValue) _
            & "&SubFamily=" & Server.UrlEncode(ddSubFamily.SelectedValue) _
            & "&DensityValue=" & Server.UrlEncode(ddDensityValue.SelectedValue) _
            & "&DrawingByEngineer=" & Server.UrlEncode(ddDrawingByEngineer.SelectedValue) _
            & "&Construction=" & Server.UrlEncode(txtConstruction.Text.Trim) _
            & "&Notes=" & Server.UrlEncode(txtNotes.Text.Trim) _
            & "&ReleaseType=" & Server.UrlEncode(ddReleaseType.SelectedValue) _
            & "&Status=" & Server.UrlEncode(ddStatus.SelectedValue) _
            & "&LastUpdatedOnStart=" & Server.UrlEncode(txtLastUpdatedOnStart.Text.Trim) _
            & "&LastUpdatedOnEnd=" & Server.UrlEncode(txtLastUpdatedOnEnd.Text.Trim) _
            & "&Make=" & ddMake.SelectedValue _
            & "&ProductTechnology=" & ddProductTechnology.SelectedValue, False)

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

            Response.Redirect("DrawingDetail.aspx?DrawingNo=NewDrawing", False)
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
            lblMessage.Text = ""

            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionDrawingCurrentPage") = CurrentPage

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
            lblMessage.Text = ""

            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionDrawingCurrentPage") = CurrentPage

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
            lblMessage.Text = ""

            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionDrawingCurrentPage") = CurrentPage

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

    Protected Sub cbShowAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowAdvancedSearch.CheckedChanged

        Try
            lblMessage.Text = ""

            If cbShowAdvancedSearch.Checked = False Then
                Response.Cookies("UGNDB_ShowDrawingAdvancedSearch").Value = 0
                accAdvancedSearch.SelectedIndex = -1
            Else
                Response.Cookies("UGNDB_ShowDrawingAdvancedSearch").Value = 1
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

    Protected Sub ddMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMake.SelectedIndexChanged

        lblMessage.Text = ""

        Try
            Dim dsProgram As DataSet

            If ddMake.SelectedIndex > 0 Then
                dsProgram = commonFunctions.GetProgram("", "", ddMake.SelectedValue)
                If commonFunctions.CheckDataset(dsProgram) = True Then
                    ddProgram.Items.Clear()
                    ddProgram.DataSource = dsProgram
                    ddProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                    ddProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
                    ddProgram.DataBind()
                    ddProgram.Items.Insert(0, "")
                End If
            Else
                dsProgram = commonFunctions.GetProgram("", "", "")
                If commonFunctions.CheckDataset(dsProgram) = True Then
                    ddProgram.Items.Clear()
                    ddProgram.DataSource = dsProgram
                    ddProgram.DataTextField = dsProgram.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                    ddProgram.DataValueField = dsProgram.Tables(0).Columns("ProgramID").ColumnName
                    ddProgram.DataBind()
                    ddProgram.Items.Insert(0, "")
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
End Class

