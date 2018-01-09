' ************************************************************************************************
'
' Name:		MaterialSpecList.aspx
' Purpose:	This Code Behind is for the Product Engineering Material Specificiations List
'
' Date		 Author	    
' 03/02/2011 Roderick Carlson
' 08/26/2011 Roderick Carlson - Added DrawingNo paramter
' ************************************************************************************************
Partial Class MaterialSpecList
    Inherits System.Web.UI.Page
    Protected WithEvents lnkMaterialSpecNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkMaterialSpecDesc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRevisionDate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAreaWeight As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkSubfamily As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkDrawingNo As System.Web.UI.WebControls.LinkButton

    Protected Function SetSelectMaterialSpecNoHyperlink(ByVal MaterialSpecNo As String) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = "MaterialSpecDetail.aspx?MaterialSpecNo=" & MaterialSpecNo

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetSelectMaterialSpecNoHyperlink = strReturnValue

    End Function

    Protected Function SetSelectDrawingNoHyperlink(ByVal DrawingNo As String) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = "DrawingDetail.aspx?DrawingNo=" & DrawingNo

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetSelectDrawingNoHyperlink = strReturnValue

    End Function

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = PEModule.GetDrawingMaterialSpecSearch(ViewState("MaterialSpecNo"), ViewState("MaterialSpecDesc"), ViewState("StartRevisionDate"), _
              ViewState("EndRevisionDate"), ViewState("SubfamilyID"), ViewState("MaterialAreaWeight"), ViewState("DrawingNo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpInfo.DataSource = dv
                rpInfo.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            Else
                cmdFirst.Enabled = False
                cmdFirstBottom.Enabled = False

                cmdGo.Enabled = False
                cmdGoBottom.Enabled = False

                cmdPrev.Enabled = False
                cmdPrevBottom.Enabled = False

                cmdNext.Enabled = False
                cmdNextBottom.Enabled = False

                cmdLast.Enabled = False
                cmdLastBottom.Enabled = False

                rpInfo.Visible = False

                txtGoToPage.Visible = False
                txtGoToPageBottom.Visible = False
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
      Handles lnkMaterialSpecNo.Click, lnkMaterialSpecDesc.Click, lnkRevisionDate.Click, lnkAreaWeight.Click, lnkSubfamily.Click, lnkDrawingNo.Click

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

    Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing  data to repeater control at bottom of screen                       
            ds = PEModule.GetDrawingMaterialSpecSearch(ViewState("MaterialSpecNo"), ViewState("MaterialSpecDesc"), ViewState("StartRevisionDate"), _
             ViewState("EndRevisionDate"), ViewState("SubfamilyID"), ViewState("MaterialAreaWeight"), ViewState("DrawingNo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                rpInfo.DataSource = ds
                rpInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 15

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpInfo.DataSource = objPds
                rpInfo.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                lblCurrentPageBottom.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()

                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1
                txtGoToPageBottom.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdFirstBottom.Enabled = Not objPds.IsFirstPage

                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdPrevBottom.Enabled = Not objPds.IsFirstPage

                cmdNext.Enabled = Not objPds.IsLastPage
                cmdNextBottom.Enabled = Not objPds.IsLastPage

                cmdLast.Enabled = Not objPds.IsLastPage
                cmdLastBottom.Enabled = Not objPds.IsLastPage

            Else
                cmdFirst.Enabled = False
                cmdFirstBottom.Enabled = False

                cmdGo.Enabled = False
                cmdGoBottom.Enabled = False

                cmdPrev.Enabled = False
                cmdPrevBottom.Enabled = False

                cmdNext.Enabled = False
                cmdNextBottom.Enabled = False

                cmdLast.Enabled = False
                cmdLastBottom.Enabled = False

                rpInfo.Visible = False

                txtGoToPage.Visible = False
                txtGoToPageBottom.Visible = False
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
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'different form id (36) but same form security as DMS Drawings
                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 35)

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
            m.ContentLabel = "Material Specification - List and Search"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> <b>Drawing Management</b> > <b>Material Specification </b> > List and Search "
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

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            CheckRights()

            'clear crystal reports
            PEModule.CleanPEDMScrystalReports()

            If HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage")
            End If

            If Not Page.IsPostBack Then
                ViewState("lnkMaterialSpecNo") = "DESC"
                ViewState("lnkMaterialSpecDesc") = "ASC"
                ViewState("lnkRevisionDate") = "ASC"
                ViewState("lnkSubfamilyID") = "ASC"
                ViewState("lnkDrawingNo") = "ASC"

                ViewState("MaterialSpecNo") = ""
                ViewState("MaterialSpecDesc") = ""
                ViewState("RevisionDate") = ""               
                ViewState("SubFamilyID") = ""
                ViewState("MaterialAreaWeight") = ""
                ViewState("DrawingNo") = ""

                '' ''******
                '' '' Bind drop down lists
                '' ''******
                BindCriteria()

                '' ''******
                ' ''get saved value of past search criteria or query string, query string takes precedence
                '' ''******

                If HttpContext.Current.Request.QueryString("MaterialSpecNo") <> "" Then
                    txtSearchMaterialSpecNo.Text = HttpContext.Current.Request.QueryString("MaterialSpecNo")
                    ViewState("MaterialSpecNo") = HttpContext.Current.Request.QueryString("MaterialSpecNo")
                Else
                    If Not Request.Cookies("PEModule_SaveMaterialSpecNo") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMaterialSpecNo").Value) <> "" Then
                            txtSearchMaterialSpecNo.Text = Request.Cookies("PEModule_SaveMaterialSpecNo").Value
                            ViewState("MaterialSpecNo") = Request.Cookies("PEModule_SaveMaterialSpecNo").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("MaterialSpecDesc") <> "" Then
                    txtSearchMaterialSpecDesc.Text = HttpContext.Current.Request.QueryString("MaterialSpecDesc")
                    ViewState("MaterialSpecDesc") = HttpContext.Current.Request.QueryString("MaterialSpecDesc")
                Else
                    If Not Request.Cookies("PEModule_SaveMaterialSpecDesc") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMaterialSpecDesc").Value) <> "" Then
                            txtSearchMaterialSpecDesc.Text = Request.Cookies("PEModule_SaveMaterialSpecDesc").Value
                            ViewState("MaterialSpecDesc") = Request.Cookies("PEModule_SaveMaterialSpecDesc").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("SubFamilyID") <> "" Then
                    ddSubFamily.SelectedValue = HttpContext.Current.Request.QueryString("SubFamilyID")
                    ViewState("SubfamilyID") = HttpContext.Current.Request.QueryString("SubFamilyID")
                Else
                    If Not Request.Cookies("PEModule_SaveMaterialSpecSubfamilyID") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value) <> "" Then
                            ddSubFamily.SelectedValue = Request.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value
                            ViewState("SubFamilyID") = Request.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("MaterialAreaWeight") <> "" Then
                    txtSearchMaterialAreaWeight.Text = HttpContext.Current.Request.QueryString("MaterialAreaWeight")
                    ViewState("MaterialAreaWeight") = HttpContext.Current.Request.QueryString("MaterialAreaWeight")
                Else
                    If Not Request.Cookies("PEModule_SaveMaterialAreaWeight") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMaterialAreaWeight").Value) <> "" Then
                            txtSearchMaterialAreaWeight.Text = Request.Cookies("PEModule_SaveMaterialAreaWeight").Value
                            ViewState("MaterialAreaWeight") = Request.Cookies("PEModule_SaveMaterialAreaWeight").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    txtSearchDrawingNo.Text = HttpContext.Current.Request.QueryString("DrawingNo")
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                Else
                    If Not Request.Cookies("PEModule_SaveMaterialDrawingNo") Is Nothing Then
                        If Trim(Request.Cookies("PEModule_SaveMaterialDrawingNo").Value) <> "" Then
                            txtSearchDrawingNo.Text = Request.Cookies("PEModule_SaveMaterialDrawingNo").Value
                            ViewState("DrawingNo") = Request.Cookies("PEModule_SaveMaterialDrawingNo").Value
                        End If
                    End If
                End If

                '' ''******
                'load repeater control
                '' ''******
                BindData()

            Else
                If txtSearchMaterialSpecNo.Text.Trim <> "" Then
                    ViewState("MaterialSpecNo") = txtSearchMaterialSpecNo.Text.Trim
                End If

                If txtSearchMaterialSpecDesc.Text.Trim <> "" Then
                    ViewState("MaterialSpecDesc") = txtSearchMaterialSpecDesc.Text.Trim
                End If

                If ddSubFamily.SelectedIndex > 0 Then
                    ViewState("SubFamilyID") = ddSubFamily.SelectedValue()
                End If

                If txtSearchMaterialAreaWeight.Text.Trim <> "" Then
                    ViewState("MaterialAreaWeight") = txtSearchMaterialAreaWeight.Text.Trim
                End If

                If txtSearchDrawingNo.Text.Trim <> "" Then
                    ViewState("DrawingNo") = txtSearchDrawingNo.Text.Trim
                End If

                'focus on RFDNo field
                txtSearchMaterialSpecNo.Focus()
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = Nothing

            PEModule.DeletePEMaterialSpecCookies()

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'set saved value of what criteria was used to search   
            ''''''''''''''''''''''''''''''''''''''''''''''''''''

            Response.Cookies("PEModule_SaveMaterialSpecNo").Value = txtSearchMaterialSpecNo.Text.Trim

            Response.Cookies("PEModule_SaveMaterialSpecDesc").Value = txtSearchMaterialSpecDesc.Text.Trim

            If ddSubFamily.SelectedIndex > 0 Then
                Response.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value = ddSubFamily.SelectedValue
            Else
                Response.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value = 0
                Response.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("PEModule_SaveMaterialAreaWeight").Value = txtSearchMaterialAreaWeight.Text.Trim

            Response.Cookies("PEModule_SaveMaterialDrawingNo").Value = txtSearchDrawingNo.Text.Trim

            Response.Redirect("MaterialSpecList.aspx?MaterialSpecNo=" & Server.UrlEncode(txtSearchMaterialSpecNo.Text.Trim) _
            & "&MaterialSpecDesc=" & Server.UrlEncode(txtSearchMaterialSpecDesc.Text.Trim) _
            & "&SubfamilyID=" & ddSubFamily.SelectedValue _
            & "&MaterialAreaWeight=" & Server.UrlEncode(txtSearchMaterialAreaWeight.Text.Trim) _
            & "&DrawingNo=" & Server.UrlEncode(txtSearchDrawingNo.Text.Trim) _
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

            PEModule.DeletePEMaterialSpecCookies()

            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = Nothing

            Response.Redirect("MaterialSpecList.aspx", False)

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

            Response.Redirect("MaterialSpecDetail.aspx", False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click, cmdPrevBottom.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click, cmdNextBottom.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click, cmdFirstBottom.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Trim <> "" Then
                txtGoToPageBottom.Text = txtGoToPage.Text

                ' Set viewstate variable to the specific page
                If CType(txtGoToPage.Text.Trim, Integer) > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If

                HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub cmdGoBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGoBottom.Click

        Try
            If txtGoToPageBottom.Text.Trim <> "" Then
                txtGoToPage.Text = txtGoToPageBottom.Text

                ' Set viewstate variable to the specific page
                If CType(txtGoToPageBottom.Text.Trim, Integer) > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPageBottom.Text - 1
                End If

                HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click, cmdLastBottom.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionDrawingMaterialSpecCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

End Class
