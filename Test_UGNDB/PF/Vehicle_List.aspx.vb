' ************************************************************************************************
' Name:	Vehicle_List.aspx.vb
' Purpose:	This program is used to display data stored in the database.
'
' Date		    Author	    
' 03/19/2008    LRey			Created .Net application
' 04/22/2008    LRey            commented out all references to DABBV per Mike E.
' 08/11/2008    LRey            Added SoldTo to get functions
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' ************************************************************************************************
Partial Class PMT_Vehicle_List
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Vehicle Volume"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > Vehicle Volume Search"
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
            ddYear.Focus()


            If HttpContext.Current.Session("sessionVehicleCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionVehicleCurrentPage")
            End If

            If Not Page.IsPostBack Then
                ViewState("sPGMID") = ""
                ViewState("sYear") = ""
                ViewState("sCABBV") = ""
                ViewState("sSoldTo") = 0
                ' ''ViewState("sDABBV") = ""
                ViewState("sAMGRID") = ""
                ViewState("sMake") = ""

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("PFV_PlanningYear") Is Nothing Then
                    ddYear.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_PlanningYear").Value)
                    ViewState("sYear") = Server.HtmlEncode(Request.Cookies("PFV_PlanningYear").Value)
                End If

                If Not Request.Cookies("PFV_Program") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_Program").Value)
                    ViewState("sPGMID") = Server.HtmlEncode(Request.Cookies("PFV_Program").Value)
                End If

                If (Not Request.Cookies("PFV_CABBV") Is Nothing) And (Not Request.Cookies("PFV_SoldTo") Is Nothing) Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_SoldTo").Value) & "|" & Server.HtmlEncode(Request.Cookies("PFV_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("PFV_CABBV").Value)
                    ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("PFV_SoldTo").Value)
                End If

                ' ''If Not Request.Cookies("PFV_DABBV") Is Nothing Then
                ' ''    ddCustomerPlant.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_DABBV").Value)
                ' ''    ViewState("sDABBV") = Server.HtmlEncode(Request.Cookies("PFV_DABBV").Value)
                ' ''End If

                If Not Request.Cookies("PFV_AMGRID") Is Nothing Then
                    ddAccountManager.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_AMGRID").Value)
                    ViewState("sAMGRID") = Server.HtmlEncode(Request.Cookies("PFV_AMGRID").Value)
                End If

                If Not Request.Cookies("PFV_Make") Is Nothing Then
                    ddMakes.SelectedValue = Server.HtmlEncode(Request.Cookies("PFV_Make").Value)
                    ViewState("sMake") = Server.HtmlEncode(Request.Cookies("PFV_Make").Value)
                End If

                ''******
                '' Bind data to table listing
                ''*******
                BindData()
            Else
                ViewState("sPGMID") = ddProgram.SelectedValue
                ViewState("sYear") = ddYear.SelectedValue
                ViewState("sCABBV") = ""
                ViewState("sSoldTo") = 0
                Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                If Not (Pos = 0) Then
                    ViewState("sCABBV") = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                    ViewState("sSoldTo") = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                End If
                ' ''ViewState("sDABBV") = ddCustomerPlant.SelectedValue
                ViewState("sAMGRID") = ddAccountManager.SelectedValue
                ViewState("sMake") = ddMakes.SelectedValue
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Protected Sub BindCriteria()
        Try

            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetCustomer(False)
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddCustomerDesc").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("ddCustomerValue").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ' '' ''bind existing data to drop down Customer Plant control for selection criteria for search
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

            ''bind existing data to drop down Planning Year control for selection criteria for search
            ds = commonFunctions.GetYear("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
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


            ''bind existing data to drop down Make control for selection criteria for search
            ds = commonFunctions.GetProgramMake()
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddMakes.DataSource = ds
                ddMakes.DataTextField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddMakes.DataValueField = ds.Tables(0).Columns("Make").ColumnName.ToString()
                ddMakes.DataBind()
                ddMakes.Items.Insert(0, "")
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub 'EOF BindCriteria

    Private Sub BindData()
        Dim ds As DataSet = New DataSet
        Try
            'bind existing TMTrans data to repeater control at bottom of the list screen
            If ViewState("sPGMID") = Nothing Or CType(ViewState("sPGMID"), String) = "" Then
                ViewState("sPGMID") = 0
            End If
            If ViewState("sYear") = Nothing Or CType(ViewState("sYear"), String) = "" Then
                ViewState("sYear") = 0
            End If
            If ViewState("sAMGRID") = Nothing Or CType(ViewState("sAMGRID"), String) = "" Then
                ViewState("sAMGRID") = 0
            End If
            
            ' ''ds = PFModule.GetVehicle(ViewState("sPGMID"), ViewState("sYear"), ViewState("sCABBV"), ViewState("sDABBV"), ViewState("sAMGRID"))
            ds = PFModule.GetVehicle(ViewState("sPGMID"), ViewState("sYear"), ViewState("sCABBV"), ViewState("sSoldTo"), ViewState("sAMGRID"), ViewState("sMake"))

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
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            HttpContext.Current.Session("sessionVehicleCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionVehicleCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionVehicleCurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionVehicleCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionVehicleCurrentPage") = CurrentPage

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

    ''Protected Sub ddCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCustomer.SelectedIndexChanged
    ''    Dim ds As DataSet = New DataSet

    ''    ''bind existing data to drop down Customer Plant control for selection criteria for search
    ''    ds = commonFunctions.GetCustomerDestination(ddCustomer.SelectedValue)
    ''    If (ds.Tables.Item(0).Rows.Count > 0) Then
    ''        ddCustomerPlant.DataSource = ds
    ''        ddCustomerPlant.DataTextField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
    ''        ddCustomerPlant.DataValueField = ds.Tables(0).Columns("DABBV").ColumnName.ToString()
    ''        ddCustomerPlant.DataBind()
    ''        ddCustomerPlant.Items.Insert(0, "")
    ''        ddCustomerPlant.SelectedIndex = 0
    ''    End If
    ''End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ''******
            '' Delete cookies in search parameters.
            ''******
            PFModule.DeletePFCookies_VehicleVolume()
            HttpContext.Current.Session("sessionVehicleCurrentPage") = Nothing

            ''******
            '' Redirect to the Vehicle Volume List page
            ''******
            Response.Redirect("Vehicle_List.aspx", False)

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            ''******
            '' Store search parameters
            ''******
            HttpContext.Current.Session("sessionVehicleCurrentPage") = Nothing

            Response.Cookies("PFV_PlanningYear").Value = ddYear.SelectedValue
            Response.Cookies("PFV_Program").Value = ddProgram.SelectedValue
            Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            If Not (Pos = 0) Then
                Response.Cookies("PFV_CABBV").Value = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                Response.Cookies("PFV_SoldTo").Value = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            End If
            ' ''Response.Cookies("PFV_DABBV").Value = ddCustomerPlant.SelectedValue
            Response.Cookies("PFV_AMGRID").Value = ddAccountManager.SelectedValue
            Response.Cookies("PFV_Make").Value = ddMakes.SelectedValue

            ' Set viewstate variable to the first page
            CurrentPage = 0

            ' Reload control
            BindData()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Response.Redirect("Vehicle_Volume.aspx", False)
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Function SetClickable(ByVal Obsolete As Boolean) As String

        Dim strReturnValue As String = "False"

        Try
            If Obsolete = False Then
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

        SetClickable = strReturnValue

    End Function 'EOF SetClickable
End Class
