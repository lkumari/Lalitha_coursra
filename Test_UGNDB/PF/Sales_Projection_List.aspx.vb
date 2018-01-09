' ************************************************************************************************
' Name:	Sales_Projection_List.aspx.vb
' Purpose:	This program is used to display data that is stored in the database.
'
' Date		    Author	    
' 03/19/2008    LRey			Created .Net application
' 04/22/2008    LRey            commented out all references to DABBV per Mike E.
' 08/12/2008    LRey            Added SoldTo to get functions
' 08/06/2010    LRey            Added Royalty to the get functions
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' ************************************************************************************************
Partial Class PMT_Sales_Projection_List
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Sales Projection"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > Sales Projection Search"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            'focus on Vehicle List screen Program field
            txtPartNo.Focus()

            If HttpContext.Current.Session("sessionPFCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionPFCurrentPage")
            End If

            If Not Page.IsPostBack Then
                ViewState("sPNO") = ""
                ViewState("sCMDTYID") = 0
                ViewState("sSoldTo") = 0
                ViewState("sCABBV") = ""
                ViewState("sPGMID") = 0
                ViewState("sPTID") = 0
                ViewState("sPGMSTS") = ""
                ViewState("sAMGRID") = 0
                ViewState("sUGNFAC") = ""
                ViewState("sRID") = 0

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PF_PartNo") Is Nothing Then
                    txtPartNo.Text = Server.HtmlEncode(Request.Cookies("PF_PartNo").Value)
                    ViewState("sPNO") = Server.HtmlEncode(Request.Cookies("PF_PartNo").Value)
                End If

                If Not Request.Cookies("PF_Commodity") Is Nothing Then
                    ddCommodity.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_Commodity").Value)
                    ViewState("sCMDTYID") = Server.HtmlEncode(Request.Cookies("PF_Commodity").Value)
                End If

                If (Not Request.Cookies("PF_CABBV") Is Nothing) And (Not Request.Cookies("PF_SoldTo") Is Nothing) Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_SoldTo").Value) & "|" & Server.HtmlEncode(Request.Cookies("PF_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("PF_CABBV").Value)
                    ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("PF_SoldTo").Value)
                End If

                If Not Request.Cookies("PF_Program") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_Program").Value)
                    ViewState("sPGMID") = Server.HtmlEncode(Request.Cookies("PF_Program").Value)
                End If

                If Not Request.Cookies("PF_ProductTechnology") Is Nothing Then
                    ddProductTechnology.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_ProductTechnology").Value)
                    ViewState("sPTID") = Server.HtmlEncode(Request.Cookies("PF_ProductTechnology").Value)
                End If

                If Not Request.Cookies("PF_ProgramStatus") Is Nothing Then
                    ddProgramStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_ProgramStatus").Value)
                    ViewState("sPGMSTS") = Server.HtmlEncode(Request.Cookies("PF_ProgramStatus").Value)
                End If

                If Not Request.Cookies("PF_AMGRID") Is Nothing Then
                    ddAccountManager.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_AMGRID").Value)
                    ViewState("sAMGRID") = Server.HtmlEncode(Request.Cookies("PF_AMGRID").Value)
                End If

                If Not Request.Cookies("PF_UGNFacility") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_UGNFacility").Value)
                    ViewState("sUGNFAC") = Server.HtmlEncode(Request.Cookies("PF_UGNFacility").Value)
                End If

                If Not Request.Cookies("PF_RID") Is Nothing Then
                    ddRoyalty.SelectedValue = Server.HtmlEncode(Request.Cookies("PF_RID").Value)
                    ViewState("sRID") = Server.HtmlEncode(Request.Cookies("PF_RID").Value)
                End If

                ''******
                '' Bind data to table listing
                ''*******
                BindData()
            Else
                ViewState("sPNO") = txtPartNo.Text
                ViewState("sCMDTYID") = ddCommodity.SelectedValue
                Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                If Not (Pos = 0) Then
                    ViewState("sCABBV") = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                    ViewState("sSoldTo") = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                End If
                If ddProgram.SelectedIndex > 0 Then
                    ViewState("sPGMID") = ddProgram.SelectedValue()
                End If
                ViewState("sPTID") = ddProductTechnology.SelectedValue
                ViewState("sPGMSTS") = ddProgramStatus.SelectedValue
                ViewState("sAMGRID") = ddAccountManager.SelectedValue
                ViewState("sUGNFAC") = ddUGNFacility.SelectedValue
                ViewState("sRID") = ddRoyalty.SelectedValue
            End If
            Session("ddCustomer") = ViewState("sCABBV")
            Session("ddProgram") = ViewState("sPGMID")
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_load

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Customer Plant control for selection criteria for search
        ' ''ds = commonFunctions.GetCustomerDestination(ddCustomer.SelectedValue)
        ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
        ' ''    ddCustomerPlant.DataSource = ds
        ' ''    ddCustomerPlant.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
        ' ''    ddCustomerPlant.DataValueField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
        ' ''    ddCustomerPlant.DataBind()
        ' ''    ddCustomerPlant.Items.Insert(0, "")
        ' ''    ddCustomerPlant.SelectedIndex = 0
        ' ''End If

        ''bind existing data to drop down Customer Plant control for selection criteria for search
        ds = commonFunctions.GetPlatformProgram(0, 0, "", "", "")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgram.DataSource = ds
            ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramModelPlatformAssembly").ColumnName.ToString()
            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ddProgram.DataBind()
            ddProgram.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Product Technology control for selection criteria
        ds = commonFunctions.GetProductTechnology("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProductTechnology.DataSource = ds
            ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName.ToString()
            ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
            ddProductTechnology.DataBind()
            ddProductTechnology.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Commodity control for selection criteria 
        ds = commonFunctions.GetCommodity(0, "", "", 0)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCommodity.DataSource = ds
            ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
            ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ddCommodity.DataBind()
            ddCommodity.Items.Insert(0, "")
            ddCommodity.SelectedIndex = 0
        End If

        ''bind existing data to drop down Customer control for selection criteria for search
        ds = commonFunctions.GetCustomer(False)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddCustomer.DataSource = ds
            ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
            ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()

            ddCustomer.DataBind()
            ddCustomer.Items.Insert(0, "")
        End If


        ''bind existing data to drop down Account Manager control for selection criteria for search
        ds = commonFunctions.GetTeamMemberBySubscription(18) '**SubscriptionID 18 is used for Account Manager
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddAccountManager.DataSource = ds
            ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
            ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
            ddAccountManager.DataBind()
            ddAccountManager.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Planning Year control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Royalty control for selection criteria
        ds = commonFunctions.GetRoyalty("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddRoyalty.DataSource = ds
            ddRoyalty.DataTextField = ds.Tables(0).Columns("ddRoyaltyName").ColumnName.ToString()
            ddRoyalty.DataValueField = ds.Tables(0).Columns("RoyaltyID").ColumnName
            ddRoyalty.DataBind()
            ddRoyalty.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Private Sub BindData()
        Dim ds As DataSet = New DataSet
        Try
            'bind existing TMTrans data to repeater control at bottom of the list screen

            If ViewState("sPGMID") = Nothing Or CType(ViewState("sPGMID"), String) = "" Then
                ViewState("sPGMID") = 0
            End If

            If ViewState("sCMDTYID") = Nothing Or CType(ViewState("sCMDTYID"), String) = "" Then
                ViewState("sCMDTYID") = 0
            End If

            If ViewState("sPTID") = Nothing Or CType(ViewState("sPTID"), String) = "" Then
                ViewState("sPTID") = 0
            End If

            If ViewState("sAMGRID") = Nothing Or CType(ViewState("sAMGRID"), String) = "" Then
                ViewState("sAMGRID") = 0
            End If

            If ViewState("sRID") = Nothing Or CType(ViewState("sRID"), String) = "" Then
                ViewState("sRID") = 0
            End If

            ds = PFModule.GetProjectedSalesListing(ViewState("sPNO"), ViewState("sPGMID"), ViewState("sPGMSTS"), ViewState("sCMDTYID"), ViewState("sCABBV"), ViewState("sSoldTo"), ViewState("sPTID"), ViewState("sAMGRID"), ViewState("sUGNFAC"), ViewState("sRID"), 0)

            If ds.Tables.Count > 0 Then
                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 30

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpVehicleVolume.DataSource = objPds
                rpVehicleVolume.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdNext.Enabled = Not objPds.IsLastPage

                ' Display # of records
                If (CurrentPage + 1) > 1 Then
                    lblFromRec.Text = (((CurrentPage + 1) * 30) - 30) + 1
                    lblToRec.Text = (CurrentPage + 1) * 30
                    If lblToRec.Text > objPds.DataSourceCount Then
                        lblToRec.Text = objPds.DataSourceCount
                    End If
                Else
                    lblFromRec.Text = ds.Tables.Count
                    lblToRec.Text = rpVehicleVolume.Items.Count
                End If
                lblTotalRecords.Text = objPds.DataSourceCount

            End If
        Catch ex As Exception
            lblErrors.Text = ex.Message
            lblErrors.Visible = True
        End Try
    End Sub 'EOF BindData

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
            HttpContext.Current.Session("sessionPFCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPFCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPFCurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionPFCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionPFCurrentPage") = CurrentPage

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

    Protected Sub ddCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCustomer.SelectedIndexChanged
        Dim ds As DataSet = New DataSet
        Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
        Dim tempCABBV As String = Nothing
        Dim tempSoldTo As Integer = Nothing

        If Not (Pos = 0) Then
            tempCABBV = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
            tempSoldTo = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
        End If

        ''bind existing data to drop down Program control for selection criteria for search
        ddProgram.ClearSelection()
        ' ''ds = commonFunctions.GetProgramByCABBVDABBV(0, ddCustomer.SelectedValue, "")
        ds = commonFunctions.GetProgramByCABBVDABBV(0, tempCABBV, tempSoldTo)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddProgram.DataSource = ds

            ddProgram.DataTextField = ds.Tables(0).Columns("ProgramName").ColumnName.ToString()
            ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
            ddProgram.DataBind()
            ddProgram.Items.Insert(0, "")
        Else
            ds = commonFunctions.GetProgram("", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName.ToString()
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If
        End If

    End Sub 'EOF ddCustomer_SelectedIndexChanged

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ''******
        '' Delete cookies in search parameters.
        ''******
        PFModule.DeletePFCookies_SalesProjection()
        HttpContext.Current.Session("sessionPFCurrentPage") = Nothing

        ''******
        '' Redirect to the Sales Projection List page
        ''******
        Response.Redirect("Sales_Projection_List.aspx", False)

    End Sub 'EOF btnReset_Click

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionPFCurrentPage") = Nothing

            'set saved value of what criteria was used to search   
            Response.Cookies("PF_PartNo").Value = txtPartNo.Text
            Response.Cookies("PF_Commodity").Value = ddCommodity.SelectedValue

            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            If Not (Pos = 0) Then
                Response.Cookies("PF_CABBV").Value = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                Response.Cookies("PF_SoldTo").Value = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If

            Response.Cookies("PF_Program").Value = ddProgram.SelectedValue
            Response.Cookies("PF_ProductTechnology").Value = ddProductTechnology.SelectedValue
            Response.Cookies("PF_ProgramStatus").Value = ddProgramStatus.SelectedValue
            Response.Cookies("PF_AMGRID").Value = ddAccountManager.SelectedValue
            Response.Cookies("PF_UGNFacility").Value = ddUGNFacility.SelectedValue
            Response.Cookies("PF_RID").Value = ddRoyalty.SelectedValue

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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Sales_Projection.aspx", False)
    End Sub 'EOF btnAdd_click
End Class
