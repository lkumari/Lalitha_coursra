' ************************************************************************************************
'
' Name:		ToolingAuthExpProjList.aspx
' Purpose:	This Code Behind is for the Tooling Authorization search page
'
' Date		    Author	    
' 03/07/2012    Roderick Carlson
' ************************************************************************************************
Partial Class ToolingAuthExpProjList
    Inherits System.Web.UI.Page

    Protected WithEvents lnkStatus As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkTAProjectNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartNo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkNewDesignLevel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkPartName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkIssueDate As System.Web.UI.WebControls.LinkButton

    Protected Function SetBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White"

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2" 'in-process
                    strReturnValue = "Yellow"
                Case "4" 'void
                    strReturnValue = "Gray"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetBackGroundColor = strReturnValue

    End Function

    Protected Function SetForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black"

        Try
            Select Case StatusID
                Case "4" 'void               
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetForeGroundColor = strReturnValue

    End Function

    Protected Function SetHistoryVisible(ByVal ArchiveData As String) As Boolean

        Dim bReturnValue As Boolean = True

        Try
            If ArchiveData <> "" Then
                If CType(ArchiveData, Integer) > 0 Then
                    bReturnValue = False
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetHistoryVisible = bReturnValue

    End Function

    Protected Function SetPreviewToolingAuthHyperLink(ByVal TAProjectNo As String, ByVal StatusID As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = "javascript:void(window.open('crViewExpProjToolingAuth.aspx?FormType=TA&ArchiveData= " & ArchiveData & " &TAProjectNo=" & TAProjectNo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewToolingAuthHyperLink = strReturnValue

    End Function

    Protected Function SetPreviewDieshopHyperLink(ByVal TAProjectNo As String, ByVal StatusID As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try

            strReturnValue = "javascript:void(window.open('crViewExpProjToolingAuth.aspx?FormType=DS&ArchiveData=" & ArchiveData & "&TAProjectNo=" & TAProjectNo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewDieshopHyperLink = strReturnValue

    End Function

    Protected Function SetToolingAuthHyperlink(ByVal TANo As String, ByVal TAProjectNo As String, ByVal ArchiveData As Integer) As String

        Dim strReturnValue As String = ""

        Try
            If ArchiveData = 0 Then
                strReturnValue = "ToolingAuthExpProj.aspx?TANo=" & TANo
            Else
                strReturnValue = "javascript:void(window.open('crViewExpProjToolingAuth.aspx?FormType=TA&ArchiveData=1&TAProjectNo=" & TAProjectNo & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetToolingAuthHyperlink = strReturnValue

    End Function

    Protected Function SetVisibleToolingAuthHyperLink(ByVal TANo As String, ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try

            If TANo <> "" And StatusID <> "4" Then
                bReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetVisibleToolingAuthHyperLink = bReturnValue

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

                'If iTeamMemberID = 530 Then
                '    'iTeamMemberID = 140 ' Bryan Hall
                '    'iTeamMemberID = 22 'Terry Turnquist 
                '    'iTeamMemberID = 111 'Nancy Hulbert
                '    iTeamMemberID = 428 'Tracy Theos
                'End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 42)

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckIncludeArchives()

        Try

            ddSearchCustomer.Visible = Not cbIncludeArchive.Checked
            ddSearchProgram.Visible = Not cbIncludeArchive.Checked
            ddSearchProgramManager.Visible = Not cbIncludeArchive.Checked
            txtSearchCostSheetID.Visible = Not cbIncludeArchive.Checked

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''bind existing data to drop downs for selection criteria of search
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim ds As DataSet

            'Iniator         
            'ds = commonFunctions.GetTeamMember("")
            ds = TAModule.GetTAInitiator()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchInitiatorTeamMember.DataSource = ds
                ddSearchInitiatorTeamMember.DataTextField = ds.Tables(0).Columns("ddFullTeamMemberName").ColumnName.ToString()
                ddSearchInitiatorTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
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
            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchAccountManager.DataSource = ds
                ddSearchAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchAccountManager.DataBind()
                ddSearchAccountManager.Items.Insert(0, "")
            End If

            ' Program Manager
            ds = commonFunctions.GetTeamMemberBySubscription(31)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgramManager.DataSource = ds
                ddSearchProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddSearchProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddSearchProgramManager.DataBind()
                ddSearchProgramManager.Items.Insert(0, "")
            End If

            ds = TAModule.GetTAStatusMaint()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchStatus.DataSource = ds
                ddSearchStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddSearchStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddSearchStatus.DataBind()
                ddSearchStatus.Items.Insert(0, "")
            End If

            ''Type of Change
            'ds = TAModule.GetTAChangeTypeMaint(0, "")
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddChangeType.DataSource = ds
            '    ddChangeType.DataTextField = ds.Tables(0).Columns("ddChangeTypeName").ColumnName
            '    ddChangeType.DataValueField = ds.Tables(0).Columns("ChangeTypeId").ColumnName
            '    ddChangeType.DataBind()
            'End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchCustomer.DataSource = ds
                ddSearchCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddSearchCustomer.DataBind()
                ddSearchCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProgram("", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchProgram.DataSource = ds
                ddSearchProgram.DataTextField = ds.Tables(0).Columns("ddProgramName").ColumnName.ToString()
                ddSearchProgram.DataValueField = ds.Tables(0).Columns("ProgramID").ColumnName
                ddSearchProgram.DataBind()
                ddSearchProgram.Items.Insert(0, "")
            End If

            'bind UGN Facility
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSearchUGNFacility.DataSource = ds
                ddSearchUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddSearchUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddSearchUGNFacility.DataBind()
                ddSearchUGNFacility.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub BindData()

        Try
            tblResult.Visible = False

            Dim ds As DataSet
            Dim strTAProjectNo As String = ViewState("TAProjectNo").ToString.ToUpper

            If strTAProjectNo <> "" And InStr(strTAProjectNo, "U", CompareMethod.Binary) <= 0 And InStr(Mid(strTAProjectNo, 1, 1), "%", CompareMethod.Binary) <= 0 Then
                strTAProjectNo = "U" & strTAProjectNo
            End If

            'bind existing data to repeater control                    
            ds = TAModule.GetTASearch(strTAProjectNo, ViewState("StatusID"), ViewState("TADesc"), ViewState("PartName"), ViewState("RFDNo"), ViewState("CostSheetID"), ViewState("PartNo"), ViewState("DesignLevel"), ViewState("InitiatorTeamMemberID"), ViewState("QualityEngineerID"), ViewState("AccountManagerID"), ViewState("ProgramManagerID"), ViewState("UGNFacility"), ViewState("CustomerValue"), ViewState("ProgramID"), ViewState("IncludeArchive"))

            If commonFunctions.CheckDataSet(ds) = True Then
                tblResult.Visible = True

                rpInfo.DataSource = ds
                rpInfo.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 30

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpInfo.DataSource = objPds
                rpInfo.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                lblCurrentPageBottom.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()

                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Visible = True
                txtGoToPageBottom.Visible = txtGoToPage.Visible
                txtGoToPage.Text = CurrentPage + 1
                txtGoToPageBottom.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdFirstBottom.Enabled = Not objPds.IsFirstPage

                cmdGo.Enabled = True
                cmdGoBottom.Enabled = cmdGo.Enabled

                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdPrevBottom.Enabled = Not objPds.IsFirstPage

                cmdNext.Enabled = Not objPds.IsLastPage
                cmdNextBottom.Enabled = Not objPds.IsLastPage

                cmdLast.Enabled = Not objPds.IsLastPage
                cmdLastBottom.Enabled = Not objPds.IsLastPage

                ' Display # of records
                If (CurrentPage + 1) > 1 Then
                    lblFromRec.Text = (((CurrentPage + 1) * objPds.PageSize) - objPds.PageSize) + 1
                    lblToRec.Text = (CurrentPage + 1) * objPds.PageSize
                    If lblToRec.Text > objPds.DataSourceCount Then
                        lblToRec.Text = objPds.DataSourceCount
                    End If
                Else
                    lblFromRec.Text = ds.Tables.Count
                    lblToRec.Text = rpInfo.Items.Count
                End If
                lblTotalRecords.Text = objPds.DataSourceCount
            Else
                cmdFirst.Enabled = False
                cmdGo.Enabled = False
                cmdPrev.Enabled = False
                cmdNext.Enabled = False
                cmdLast.Enabled = False

                cmdFirstBottom.Enabled = cmdFirst.Enabled
                cmdGoBottom.Enabled = cmdGo.Enabled
                cmdPrevBottom.Enabled = cmdPrev.Enabled
                cmdNextBottom.Enabled = cmdNext.Enabled
                cmdLastBottom.Enabled = cmdLast.Enabled

                txtGoToPage.Visible = False
                txtGoToPageBottom.Visible = txtGoToPage.Visible
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            Dim strTAProjectNo As String = ViewState("TAProjectNo")

            If strTAProjectNo <> "" And InStr(strTAProjectNo, "U", CompareMethod.Binary) <= 0 And InStr(Mid(strTAProjectNo, 1, 1), "%", CompareMethod.Binary) <= 0 Then
                strTAProjectNo = "U" & strTAProjectNo
            End If

            ds = TAModule.GetTASearch(strTAProjectNo, ViewState("StatusID"), ViewState("TADesc"), _
                ViewState("PartName"), ViewState("RFDNo"), ViewState("CostSheetID"), _
                ViewState("PartNo"), ViewState("DesignLevel"), _
                ViewState("InitiatorTeamMemberID"), ViewState("QualityEngineerID"), _
                ViewState("AccountManagerID"), ViewState("ProgramManagerID"), _
                ViewState("UGNFacility"), ViewState("CustomerValue"), ViewState("ProgramID"), _
                ViewState("IncludeArchive"))

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
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
       Handles lnkTAProjectNo.Click

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        lblMessage.Text = ""

        Try

            Response.Redirect("ToolingAuthExpProj.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            lblMessage.Text = ""

            TAModule.DeleteTACookies()

            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = Nothing

            Response.Redirect("ToolingAuthExpProjList.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        lblMessage.Text = ""

        Try
            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = Nothing

            Response.Cookies("ToolingAuthModule_SaveTAProjectNoSearch").Value = txtSearchTAProjectNo.Text.Trim.ToUpper

            Response.Cookies("ToolingAuthModule_SaveTADescSearch").Value = txtSearchTADesc.Text.Trim

            If ddSearchStatus.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value = ddSearchStatus.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ToolingAuthModule_SavePartNameSearch").Value = txtSearchPartName.Text.Trim

            Response.Cookies("ToolingAuthModule_SaveRFDNoSearch").Value = txtSearchRFDNo.Text.Trim

            Response.Cookies("ToolingAuthModule_SaveCostSheetIDSearch").Value = txtSearchCostSheetID.Text.Trim

            Response.Cookies("ToolingAuthModule_SavePartNoSearch").Value = txtSearchPartNo.Text.Trim

            Response.Cookies("ToolingAuthModule_SaveDesignLevelSearch").Value = txtSearchDesignLevel.Text.Trim

            If ddSearchInitiatorTeamMember.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value = ddSearchInitiatorTeamMember.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchQualityEngineer.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value = ddSearchQualityEngineer.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchAccountManager.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value = ddSearchAccountManager.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProgramManager.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value = ddSearchProgramManager.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value = ddSearchUGNFacility.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value = ""
                Response.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchCustomer.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveCustomerSearch").Value = ddSearchCustomer.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveCustomerSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddSearchProgram.SelectedIndex > 0 Then
                Response.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value = ddSearchProgram.SelectedValue
            Else
                Response.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value = 0
                Response.Cookies("ToolingAuthModule_SaveProgramIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If cbIncludeArchive.Checked = True Then
                Response.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Value = 1
                ViewState("IncludeArchive") = 1
            Else
                Response.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Value = 0
                ViewState("IncludeArchive") = 0
                Response.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            CurrentPage = 0

            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click, cmdNextBottom.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

            ' Reload control
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click, cmdPrevBottom.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

            ' Reload control
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click, cmdLastBottom.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

            ' Reload control
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click, cmdFirstBottom.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

            ' Reload control
            BindData()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try

            If txtGoToPage.Text.Trim <> "" Then
                txtGoToPageBottom.Text = txtGoToPage.Text

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If

                HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

                ' Reload control
                BindData()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbShowAdvancedSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowAdvancedSearch.CheckedChanged

        Try
            lblMessage.Text = ""

            If cbShowAdvancedSearch.Checked = False Then
                Response.Cookies("UGNDB_ShowToolingAuthAdvancedSearch").Value = 0
                accAdvancedSearch.SelectedIndex = -1
            Else
                Response.Cookies("UGNDB_ShowToolingAuthAdvancedSearch").Value = 1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search Tooling Authorization"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Spending Requests </b> > Tooling Authorization Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("SPRExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If HttpContext.Current.Session("sessionToolingAuthCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionToolingAuthCurrentPage")
            End If

            'clear crystal reports
            TAModule.CleanTACrystalReports()

            If Not Page.IsPostBack Then

                CheckRights()

                ViewState("lnkTAProjectNo") = "DESC"
                ViewState("lnkStatus") = "ASC"
                ViewState("lnkPartNo") = "ASC"
                ViewState("lnkNewDesignLevel") = "ASC"
                ViewState("lnkPartName") = "ASC"
                ViewState("lnkIssueDate") = "ASC"

                ViewState("TAProjectNo") = ""
                ViewState("StatusID") = 0
                ViewState("TADesc") = ""
                ViewState("PartName") = ""
                ViewState("RFDNo") = ""
                ViewState("CostSheetID") = ""
                ViewState("PartNo") = ""
                ViewState("DesignLevel") = ""
                ViewState("InitiatorTeamMemberID") = 0
                ViewState("QualityEngineerID") = 0
                ViewState("AccountManagerID") = 0
                ViewState("ProgramManagerID") = 0
                ViewState("UGNFacility") = ""
                ViewState("CustomerValue") = ""
                ViewState("ProgramID") = 0
                ViewState("IncludeArchive") = 1

                '' ''******
                '' '' Bind drop down lists
                '' ''******
                BindCriteria()

                ' ''******
                ''get saved value of past search criteria or query string, query string takes precedence
                ' ''******
                If Not Request.Cookies("ToolingAuthModule_SaveTAProjectNoSearch") Is Nothing Then
                    txtSearchTAProjectNo.Text = Request.Cookies("ToolingAuthModule_SaveTAProjectNoSearch").Value
                    ViewState("TAProjectNo") = Request.Cookies("ToolingAuthModule_SaveTAProjectNoSearch").Value
                End If
                If Not Request.Cookies("ToolingAuthModule_SaveTAStatusIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value) <> "" Then
                        ddSearchStatus.SelectedValue = Request.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value
                        ViewState("StatusID") = Request.Cookies("ToolingAuthModule_SaveTAStatusIDSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveTADescSearch") Is Nothing Then
                    txtSearchTADesc.Text = Request.Cookies("ToolingAuthModule_SaveTADescSearch").Value
                    ViewState("TADesc") = Request.Cookies("ToolingAuthModule_SaveTADescSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SavePartNameSearch") Is Nothing Then
                    txtSearchPartName.Text = Request.Cookies("ToolingAuthModule_SavePartNameSearch").Value
                    ViewState("PartName") = Request.Cookies("ToolingAuthModule_SavePartNameSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveRFDNoSearch") Is Nothing Then
                    txtSearchRFDNo.Text = Request.Cookies("ToolingAuthModule_SaveRFDNoSearch").Value
                    ViewState("RFDNo") = Request.Cookies("ToolingAuthModule_SaveRFDNoSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveCostSheetIDSearch") Is Nothing Then
                    txtSearchCostSheetID.Text = Request.Cookies("ToolingAuthModule_SaveCostSheetIDSearch").Value
                    ViewState("CostSheetID") = Request.Cookies("ToolingAuthModule_SaveCostSheetIDSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SavePartNoSearch") Is Nothing Then
                    txtSearchPartNo.Text = Request.Cookies("ToolingAuthModule_SavePartNoSearch").Value
                    ViewState("PartNo") = Request.Cookies("ToolingAuthModule_SavePartNoSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value) <> "" Then
                        ddSearchInitiatorTeamMember.SelectedValue = Request.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value
                        ViewState("InitiatorTeamMemberID") = Request.Cookies("ToolingAuthModule_SaveInitiatorTeamMemberIDSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveDesignLevelSearch") Is Nothing Then
                    txtSearchDesignLevel.Text = Request.Cookies("ToolingAuthModule_SaveDesignLevelSearch").Value
                    ViewState("DesignLevel") = Request.Cookies("ToolingAuthModule_SaveDesignLevelSearch").Value
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value) <> "" Then
                        ddSearchQualityEngineer.SelectedValue = Request.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value
                        ViewState("QualityEngineerID") = Request.Cookies("ToolingAuthModule_SaveQualityEngineerIDSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value) <> "" Then
                        ddSearchAccountManager.SelectedValue = Request.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value
                        ViewState("AccountManagerID") = Request.Cookies("ToolingAuthModule_SaveAccountManagerIDSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value) <> "" Then
                        ddSearchAccountManager.SelectedValue = Request.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value
                        ViewState("ProgramManagerID") = Request.Cookies("ToolingAuthModule_SaveProgramManagerIDSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveUGNFacilitySearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value) <> "" Then
                        ddSearchUGNFacility.SelectedValue = Request.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value
                        ViewState("UGNFacility") = Request.Cookies("ToolingAuthModule_SaveUGNFacilitySearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveCustomerSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveCustomerSearch").Value) <> "" Then
                        ddSearchCustomer.SelectedValue = Request.Cookies("ToolingAuthModule_SaveCustomerSearch").Value
                        ViewState("CustomerValue") = Request.Cookies("ToolingAuthModule_SaveCustomerSearch").Value
                    End If
                End If

                If Not Request.Cookies("ToolingAuthModule_SaveProgramIDSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value) <> "" Then
                        ddSearchProgram.SelectedValue = Request.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value
                        ViewState("ProgramID") = Request.Cookies("ToolingAuthModule_SaveProgramIDSearch").Value
                    End If
                End If

                ViewState("IncludeArchive") = 0
                If Not Request.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch") Is Nothing Then
                    If Trim(Request.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Value) <> "" Then
                        ViewState("IncludeArchive") = CType(Request.Cookies("ToolingAuthModule_SaveIncludeArchiveSearch").Value, Integer)
                    End If
                End If

                If ViewState("IncludeArchive") = 1 Then
                    cbIncludeArchive.Checked = True
                Else
                    cbIncludeArchive.Checked = False
                End If

                'load repeater control
                BindData()

                'handle if accordion should be opened or closed - default to closed
                If Request.Cookies("UGNDB_ShowToolingAuthAdvancedSearch") IsNot Nothing Then
                    If Request.Cookies("UGNDB_ShowToolingAuthAdvancedSearch").Value.Trim <> "" Then
                        If CType(Request.Cookies("UGNDB_ShowToolingAuthAdvancedSearch").Value, Integer) = 1 Then
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

                If ViewState("ProgramID") > 0 _
                    Or ViewState("UGNFacility") <> "" _
                    Or ViewState("CustomerValue") <> "" _
                    Or ViewState("AccountManagerID") > 0 _
                    Or ViewState("ProgramManagerID") > 0 _
                    Or ViewState("IncludeArchive") > 0 Then

                    accAdvancedSearch.SelectedIndex = 0
                    cbShowAdvancedSearch.Checked = True
                End If

                EnableControls()

            Else
                ViewState("TAProjectNo") = txtSearchTAProjectNo.Text.Trim

                If ddSearchStatus.SelectedIndex > 0 Then
                    ViewState("StatusID") = ddSearchStatus.SelectedValue
                Else
                    ViewState("StatusID") = 0
                End If

                ViewState("TADesc") = txtSearchTADesc.Text.Trim

                ViewState("PartName") = txtSearchPartName.Text.Trim

                ViewState("RFDNo") = txtSearchRFDNo.Text.Trim

                ViewState("CostSheetID") = txtSearchCostSheetID.Text.Trim

                ViewState("PartNo") = txtSearchPartNo.Text.Trim

                If ddSearchInitiatorTeamMember.SelectedIndex > 0 Then
                    ViewState("InitiatorTeamMemberID") = ddSearchInitiatorTeamMember.SelectedValue
                Else
                    ViewState("InitiatorTeamMemberID") = 0
                End If

                ViewState("DesignLevel") = txtSearchDesignLevel.Text.Trim

                If ddSearchAccountManager.SelectedIndex > 0 Then
                    ViewState("AccountManagerID") = ddSearchAccountManager.SelectedValue
                Else
                    ViewState("AccountManagerID") = 0
                End If

                If ddSearchProgramManager.SelectedIndex > 0 Then
                    ViewState("ProgramManagerID") = ddSearchProgramManager.SelectedValue
                Else
                    ViewState("ProgramManagerID") = 0
                End If

                If ddSearchQualityEngineer.SelectedIndex > 0 Then
                    ViewState("QualityEngineerID") = ddSearchQualityEngineer.SelectedValue
                End If

                ViewState("UGNFacility") = ddSearchUGNFacility.SelectedValue

                If ddSearchCustomer.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddSearchCustomer.SelectedValue
                Else
                    ViewState("CustomerValue") = ""
                End If

                If ddSearchProgram.SelectedIndex > 0 Then
                    ViewState("ProgramID") = ddSearchProgram.SelectedValue
                Else
                    ViewState("ProgramID") = 0
                End If

                If cbIncludeArchive.Checked = True Then
                    ViewState("IncludeArchive") = 1
                Else
                    ViewState("IncludeArchive") = 0
                End If

                'focus on TAProjectNo field
                txtSearchTAProjectNo.Focus()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

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

                HttpContext.Current.Session("sessionToolingAuthCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
