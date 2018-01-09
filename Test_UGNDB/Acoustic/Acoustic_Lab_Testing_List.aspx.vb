' ************************************************************************************************
' Name:	Acoustic_Lab_Testing_List.aspx.vb
' Purpose:	This program is used to display data that is stored in the database.
'
' Date		    Author	    
' 02/05/2009    LRey			Created .Net application
' 06/21/2012    LRey            Added "Record Listed" to the search page.
' 01/08/2014    LREy            Replaced GetCustomer with GetOEMManufacturer. SOLDTO/CABBV is not used in the new ERP.
' ************************************************************************************************
Partial Class Acoustic_Acoustic_Lab_Testing_List
    Inherits System.Web.UI.Page
    Protected WithEvents lnkProjNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkTestDesc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCustomer As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPgm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRRNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPrjStat As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDtReq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkTestIssuance As System.Web.UI.WebControls.LinkButton


    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles lnkProjNo.Click, lnkTestDesc.Click, lnkCustomer.Click, lnkPgm.Click, lnkRRNo.Click, lnkPrjStat.Click, lnkDtReq.Click, lnkTestIssuance.Click

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
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            'bind existing CostSheet data to repeater control at bottom of screen                       
            ds = AcousticModule.GetProjectData(ViewState("sProjNo"), ViewState("sProjStatus"), ViewState("sCABBV"), IIf(ViewState("sPGMID") = Nothing, 0, ViewState("sPGMID")), IIf(ViewState("sRequester") = Nothing, 0, ViewState("sRequester")), ViewState("sReiterRefNo"), ViewState("sTestDesc"), ViewState("sReqNo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpProjectInfo.DataSource = dv
                rpProjectInfo.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for Acoustic Lab Testing"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > Acoustic Lab Testing Search"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("RnDExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

            'focus on Vehicle List screen Program field
            txtProjectNo.Focus()

            If HttpContext.Current.Session("sessionALCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionALCurrentPage")
            End If


            If Not Page.IsPostBack Then
                ViewState("sProjNo") = ""
                ViewState("sProjStatus") = ""
                ViewState("sCABBV") = ""
                'ViewState("sSoldTo") = 0
                ViewState("sPGMID") = 0
                ViewState("sRequester") = 0
                ViewState("sReiterRefNo") = ""
                ViewState("sTestDesc") = ""
                ViewState("sReqNo") = ""

                ''******
                '' Bind data to table listing
                ''*******
                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("AL_ProjNo") Is Nothing Then
                    txtProjectNo.Text = Server.HtmlEncode(Request.Cookies("AL_ProjNo").Value)
                    ViewState("sProjNo") = Server.HtmlEncode(Request.Cookies("AL_ProjNo").Value)
                End If

                If Not Request.Cookies("AL_ProjStatus") Is Nothing Then
                    ddProjectStatus.SelectedValue = Server.HtmlEncode(Request.Cookies("AL_ProjStatus").Value)
                    ViewState("sProjStatus") = Server.HtmlEncode(Request.Cookies("AL_ProjStatus").Value)
                End If

                'If (Not Request.Cookies("AL_CABBV") Is Nothing) And (Not Request.Cookies("AL_SoldTo") Is Nothing) Then
                '    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("AL_SoldTo").Value) & "|" & Server.HtmlEncode(Request.Cookies("AL_CABBV").Value)
                '    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("AL_CABBV").Value)
                '    ViewState("sSoldTo") = Server.HtmlEncode(Request.Cookies("AL_SoldTo").Value)
                'End If


                If (Not Request.Cookies("AL_CABBV") Is Nothing) Then
                    ddCustomer.SelectedValue = Server.HtmlEncode(Request.Cookies("AL_CABBV").Value)
                    ViewState("sCABBV") = Server.HtmlEncode(Request.Cookies("AL_CABBV").Value)
                End If

                If Not Request.Cookies("AL_Program") Is Nothing Then
                    ddProgram.SelectedValue = Server.HtmlEncode(Request.Cookies("AL_Program").Value)
                    ViewState("sPGMID") = Server.HtmlEncode(Request.Cookies("AL_Program").Value)
                End If

                If Not Request.Cookies("AL_Requester") Is Nothing Then
                    ddSubmittedBy.SelectedValue = Server.HtmlEncode(Request.Cookies("AL_Requester").Value)
                    ViewState("sRequester") = Server.HtmlEncode(Request.Cookies("AL_Requester").Value)
                End If

                If Not Request.Cookies("AL_ReiterRefNo") Is Nothing Then
                    txtReiterRefNo.Text = Server.HtmlEncode(Request.Cookies("AL_ReiterRefNo").Value)
                    ViewState("sReiterRefNo") = Server.HtmlEncode(Request.Cookies("AL_ReiterRefNo").Value)
                End If

                If Not Request.Cookies("AL_TestDesc") Is Nothing Then
                    txtTestDescription.Text = Server.HtmlEncode(Request.Cookies("AL_TestDesc").Value)
                    ViewState("sTestDesc") = Server.HtmlEncode(Request.Cookies("AL_TestDesc").Value)
                End If

                If Not Request.Cookies("AL_ReqNo") Is Nothing Then
                    txtRequestNo.Text = Server.HtmlEncode(Request.Cookies("AL_ReqNo").Value)
                    ViewState("sReqNo") = Server.HtmlEncode(Request.Cookies("AL_ReqNo").Value)
                End If


                ''******
                '' Bind drop down lists
                ''******
                BindData()
            Else
                ViewState("sProjNo") = txtProjectNo.Text
                ViewState("sProjStatus") = ddProjectStatus.SelectedValue
                'Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
                'If Not (Pos = 0) Then
                '    ViewState("sCABBV") = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
                '    ViewState("sSoldTo") = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
                'End If
                ViewState("sCABBV") = ddCustomer.SelectedValue
                ViewState("sPGMID") = ddProgram.SelectedValue
                ViewState("sRequester") = ddSubmittedBy.SelectedValue
                ViewState("sReiterRefNo") = txtReiterRefNo.Text
                ViewState("sTestDesc") = txtTestDescription.Text
                ViewState("sReqNo") = txtRequestNo.Text
            End If
            'Session("ddCustomer") = ViewState("sCABBV")


        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Private Sub BindData()

        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = AcousticModule.GetProjectData(ViewState("sProjNo"), ViewState("sProjStatus"), ViewState("sCABBV"), IIf(ViewState("sPGMID") = Nothing, 0, ViewState("sPGMID")), IIf(ViewState("sRequester") = Nothing, 0, ViewState("sRequester")), ViewState("sReiterRefNo"), ViewState("sTestDesc"), ViewState("sReqNo"))


            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                    rpProjectInfo.DataSource = ds
                    rpProjectInfo.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 30

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpProjectInfo.DataSource = objPds
                    rpProjectInfo.DataBind()

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
                        lblToRec.Text = rpProjectInfo.Items.Count
                    End If
                    lblTotalRecords.Text = objPds.DataSourceCount
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

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

    Protected Sub BindCriteria()
        Try


            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Customer Plant control for selection criteria for search
            ds = commonFunctions.GetProgram("", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProgram.DataSource = ds
                ddProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddProgram.DataBind()
                ddProgram.Items.Insert(0, "")
            End If

            ' '' ''bind existing data to drop down Commodity control for selection criteria 
            ' ''ds = commonFunctions.GetCommodity("")
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddCommodity.DataSource = ds
            ' ''    ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityName").ColumnName.ToString()
            ' ''    ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ' ''    ddCommodity.DataBind()
            ' ''    ddCommodity.Items.Insert(0, "")
            ' ''    ddCommodity.SelectedIndex = 0
            ' ''End If

            '' ''bind existing data to drop down Customer control for selection criteria for search
            ' ''ds = commonFunctions.GetCABBV()
            ' ''If (ds.Tables.Item(0).Rows.Count > 0) Then
            ' ''    ddCustomer.DataSource = ds
            ' ''    ddCustomer.DataTextField = ds.Tables(0).Columns("CustomerNameCombo").ColumnName.ToString()
            ' ''    ddCustomer.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
            ' ''    ddCustomer.DataBind()
            ' ''    ddCustomer.Items.Insert(0, "")
            ' ''End If

            ''bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            'bind existing data to drop down level control for selection criteria for search
            ds = AcousticModule.GetAcousticStatus("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddProjectStatus.DataSource = ds
                ddProjectStatus.DataTextField = ds.Tables(0).Columns("status").ColumnName.ToString()
                ddProjectStatus.DataValueField = ds.Tables(0).Columns("statusCode").ColumnName.ToString()
                ddProjectStatus.DataBind()
                ddProjectStatus.Items.Insert(0, "")
            End If

            ''bind existing data to drop down Project Requester control for selection criteria for search
            ds = commonFunctions.GetTeamMember("")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddSubmittedBy.DataSource = ds
                ddSubmittedBy.DataTextField = ds.Tables(0).Columns("TeamMemberName").ColumnName.ToString()
                ddSubmittedBy.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddSubmittedBy.DataBind()
                ddSubmittedBy.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionALCurrentPage") = CurrentPage

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

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionALCurrentPage") = CurrentPage

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

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionALCurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionALCurrentPage") = CurrentPage

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

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionALCurrentPage") = CurrentPage

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

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            HttpContext.Current.Session("sessionALCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("AL_ProjNo").Value = txtProjectNo.Text
            Response.Cookies("AL_ProjStatus").Value = ddProjectStatus.SelectedValue
            'Dim Pos As Integer = InStr(ddCustomer.SelectedValue, "|")
            'If Not (Pos = 0) Then
            '    Response.Cookies("AL_CABBV").Value = Microsoft.VisualBasic.Right(ddCustomer.SelectedValue, Len(ddCustomer.SelectedValue) - Pos)
            '    Response.Cookies("AL_SoldTo").Value = Microsoft.VisualBasic.Left(ddCustomer.SelectedValue, Pos - 1)
            'End If
            Response.Cookies("AL_CABBV").Value = ddCustomer.SelectedValue
            Response.Cookies("AL_Program").Value = ddProgram.SelectedValue
            Response.Cookies("AL_Requester").Value = ddSubmittedBy.SelectedValue
            Response.Cookies("AL_ReiterRefNo").Value = txtReiterRefNo.Text
            Response.Cookies("AL_TestDesc").Value = txtTestDescription.Text
            Response.Cookies("AL_ReqNo").Value = txtRequestNo.Text

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

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            AcousticModule.DeleteAcousticCookies()
            HttpContext.Current.Session("sessionALCurrentPage") = Nothing

            Response.Redirect("Acoustic_Lab_Testing_List.aspx", False)
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Acoustic_Project_Detail.aspx", False)
    End Sub
    Public Function GoToTestRequest(ByVal RequestID As Integer, ByVal ReqCategory As Integer) As String
        Dim strReturnValue As String = "#"

        If Not IsDBNull(RequestID) Then
            'strReturnValue = "~/RnD/TestIssuanceDetail.aspx?pReqID=" & RequestID & "&pReqCategory=" & ReqCategory
            strReturnValue = "~/RnD/crViewTestIssuanceRequestForm.aspx?pReqID=" & RequestID
        End If

        GoToTestRequest = strReturnValue

    End Function 'EOF GoToAcoustic
End Class
