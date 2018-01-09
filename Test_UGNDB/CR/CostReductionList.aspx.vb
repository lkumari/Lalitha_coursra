' ************************************************************************************************
' Name:	CostReductionList.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 01/14/2010    LRey			Created .Net application
' 02/26/2010    Roderick Carlson    Modified - added preview button for phase2
' 03/08/2010    Roderick Carlson    Modified - make project leader list bring back just project leaders, cleaned up some error trapping
' 05/17/2010    Roderick Carlson    Modified - CR-2895 - allow users to sort list
' 05/21/2010    Roderick Carlson    Modified - Cleaned up repeater control if no results were returned
' 11/02/2010    Roderick Carlson    Modified - The panel had a width of 1000px fixed. It is changed to have no fixed width. This way, users with small screens can still see all of the screen. Added Email Link for Admin
' 09/16/2011    Roderick Carlson    Modified - Added Export to Excel button
' 12/10/2012    Roderick Carlson    Modified - Fix spelling error recieve to receive
' ************************************************************************************************
Partial Class CR_Cost_Reduction_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkDescription As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRank As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkUGNFacility As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkProjectCategory As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCommodity As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkEstImpDate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCompletion As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkProjectNo As System.Web.UI.WebControls.LinkButton

    Private htControls As New System.Collections.Hashtable

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
        'DO NOT DELETE THIS

    End Sub
    Private Sub PrepareGridViewForExport(ByRef gv As Control)

        Dim l As Literal = New Literal()
        Dim i As Integer


        For i = 0 To gv.Controls.Count

            If ((Nothing <> htControls(gv.Controls(i).GetType().Name)) Or (Nothing <> htControls(gv.Controls(i).GetType().BaseType.Name))) Then
                l.Text = GetControlPropertyValue(gv.Controls(i))

                gv.Controls.Remove(gv.Controls(i))

                gv.Controls.AddAt(i, l)

            End If

            If (gv.Controls(i).HasControls()) Then

                PrepareGridViewForExport(gv.Controls(i))

            End If

        Next

    End Sub
    Private Function GetControlPropertyValue(ByVal control As Control) As String
        Dim controlType As Type = control.[GetType]()
        Dim strControlType As String = controlType.Name
        Dim strReturn As String = "Error"
        Dim bReturn As Boolean

        Dim ctrlProps As System.Reflection.PropertyInfo() = controlType.GetProperties()
        Dim ExcelPropertyName As String = DirectCast(htControls(strControlType), String)

        If ExcelPropertyName Is Nothing Then
            ExcelPropertyName = DirectCast(htControls(control.[GetType]().BaseType.Name), String)
            If ExcelPropertyName Is Nothing Then
                Return strReturn
            End If
        End If

        For Each ctrlProp As System.Reflection.PropertyInfo In ctrlProps

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(String) Then
                Try
                    strReturn = DirectCast(ctrlProp.GetValue(control, Nothing), String)
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(Boolean) Then
                Try
                    bReturn = CBool(ctrlProp.GetValue(control, Nothing))
                    strReturn = IIf(bReturn, "True", "False")
                    Exit Try
                Catch
                    strReturn = "Error"
                End Try
            End If

            If ctrlProp.Name = ExcelPropertyName AndAlso ctrlProp.PropertyType Is GetType(ListItem) Then
                Try
                    strReturn = DirectCast((ctrlProp.GetValue(control, Nothing)), ListItem).Text
                    Exit Try
                Catch
                    strReturn = ""
                End Try
            End If
        Next
        Return strReturn
    End Function

    Protected Function SetPreviewFormHyperLink(ByVal ProjectNo As String) As String

        Dim strReturnValue As String = ""

        Try
            If ProjectNo <> "" Then
                'strReturnValue = "javascript:void(window.open('crViewCostReductionDetail.aspx?pProjNo=" & ProjectNo & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=600,width=950,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                strReturnValue = "~/CR/crViewCostReductionDetail.aspx?pProjNo=" & ProjectNo
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewFormHyperLink = strReturnValue

    End Function

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try

            Dim attachment As String = "attachment; filename=CostReductionProjectList.xls"

            Response.ClearContent()

            Response.AddHeader("content-disposition", attachment)

            Response.ContentType = "application/ms-excel"

            Dim sw As StringWriter = New StringWriter()

            Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

            'EnablePartialRendering = False

            Dim ds As DataSet
            ds = CRModule.GetCostReductionSearch(IIf(ViewState("sProjNo") = Nothing, 0, ViewState("sProjNo")), IIf(ViewState("sLeader") = Nothing, 0, ViewState("sLeader")), ViewState("sUGNFacility"), IIf(ViewState("sCommodity") = Nothing, 0, ViewState("sCommodity")), IIf(ViewState("sProjCat") = Nothing, 0, ViewState("sProjCat")), ViewState("sDesc"), IIf(ViewState("sRFDNo") = Nothing, 0, ViewState("sRFDNo")), ViewState("filterPlantControllerReviewed"), ViewState("isPlantControllerReviewed"), ViewState("filterOffsetsCostDowns"), ViewState("isOffsetsCostDowns"), ViewState("includeCompleted"))

            If commonFunctions.CheckDataSet(ds) = True Then
                Dim tempDataGridView As New GridView


                tempDataGridView.HeaderStyle.BackColor = Color.White 'System.Drawing.Color.LightGray
                tempDataGridView.HeaderStyle.ForeColor = Color.Black 'System.Drawing.Color.Black
                tempDataGridView.HeaderStyle.Font.Bold = True

                tempDataGridView.AutoGenerateColumns = False

                Dim ProjectNoColumn As New BoundField
                ProjectNoColumn.HeaderText = "ProjectNo"
                ProjectNoColumn.DataField = "ProjectNo"
                tempDataGridView.Columns.Add(ProjectNoColumn)

                Dim UGNFacilityNameColumn As New BoundField
                UGNFacilityNameColumn.HeaderText = "UGN Facility"
                UGNFacilityNameColumn.DataField = "UGNFacilityName"
                tempDataGridView.Columns.Add(UGNFacilityNameColumn)

                Dim ProjectCategoryNameColumn As New BoundField
                ProjectCategoryNameColumn.HeaderText = "Project Category"
                ProjectCategoryNameColumn.DataField = "ProjectCategoryName"
                tempDataGridView.Columns.Add(ProjectCategoryNameColumn)

                Dim SuccessRateColumn As New BoundField
                SuccessRateColumn.HeaderText = "Success Rate"
                SuccessRateColumn.DataField = "SuccessRate"
                tempDataGridView.Columns.Add(SuccessRateColumn)

                Dim RankColumn As New BoundField
                RankColumn.HeaderText = "Rank"
                RankColumn.DataField = "Rank"
                tempDataGridView.Columns.Add(RankColumn)

                Dim CommodityNameColumn As New BoundField
                CommodityNameColumn.HeaderText = "Commodity"
                CommodityNameColumn.DataField = "CommodityName"
                tempDataGridView.Columns.Add(CommodityNameColumn)

                Dim DescriptionColumn As New BoundField
                DescriptionColumn.HeaderText = "Description"
                DescriptionColumn.DataField = "Description"
                tempDataGridView.Columns.Add(DescriptionColumn)

                Dim ProjectLeaderNameColumn As New BoundField
                ProjectLeaderNameColumn.HeaderText = "Project Leader"
                ProjectLeaderNameColumn.DataField = "ProjectLeaderName"
                tempDataGridView.Columns.Add(ProjectLeaderNameColumn)

                Dim DateSubmittedColumn As New BoundField
                DateSubmittedColumn.HeaderText = "Date Submitted"
                DateSubmittedColumn.DataField = "DateSubmitted"
                tempDataGridView.Columns.Add(DateSubmittedColumn)

                Dim EstImpDateColumn As New BoundField
                EstImpDateColumn.HeaderText = "Est. Imp. Date"
                EstImpDateColumn.DataField = "EstImpDate"
                tempDataGridView.Columns.Add(EstImpDateColumn)

                Dim CompletionColumn As New BoundField
                CompletionColumn.HeaderText = "Completion Percent"
                CompletionColumn.DataField = "Completion"
                tempDataGridView.Columns.Add(CompletionColumn)

                Dim EstAnnualCostSaveColumn As New BoundField
                EstAnnualCostSaveColumn.HeaderText = "Actual Gross Annual Cost Save"
                EstAnnualCostSaveColumn.DataField = "EstAnnualCostSave"
                tempDataGridView.Columns.Add(EstAnnualCostSaveColumn)

                Dim CapExColumn As New BoundField
                CapExColumn.HeaderText = "CapEx Saving"
                CapExColumn.DataField = "CapEx"
                tempDataGridView.Columns.Add(CapExColumn)

                Dim RFDNoColumn As New BoundField
                RFDNoColumn.HeaderText = "RFDNo"
                RFDNoColumn.DataField = "RFDNo"
                tempDataGridView.Columns.Add(RFDNoColumn)

                Dim CapExProjNoColumn As New BoundField
                CapExProjNoColumn.HeaderText = "CapEx ProjNo"
                CapExProjNoColumn.DataField = "CapExProjNo"
                tempDataGridView.Columns.Add(CapExProjNoColumn)

                Dim isPlantControllerReviewedColumn As New BoundField
                isPlantControllerReviewedColumn.HeaderText = "Is Plant Controller Reviewed"
                isPlantControllerReviewedColumn.DataField = "isPlantControllerReviewed"
                tempDataGridView.Columns.Add(isPlantControllerReviewedColumn)

                tempDataGridView.DataSource = ds
                tempDataGridView.DataBind()

                tempDataGridView.RenderControl(htw)

                Response.Write(sw.ToString())

                Response.End()

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Cost Reduction Project Search"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > Cost Reduction Project Search"
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
            ctl = m.FindControl("CRExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            'focus on Vehicle List screen Program field
            txtProjectNo.Focus()

            If HttpContext.Current.Session("sessionCRCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionCRCurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ViewState("lnkDescription") = "ASC"
                ViewState("lnkRank") = "ASC"
                ViewState("lnkUGNFacility") = "ASC"
                ViewState("lnkProjectCategory") = "ASC"
                ViewState("lnkCommodity") = "ASC"
                ViewState("lnkEstImpDate") = "ASC"
                ViewState("lnkCompletion") = "ASC"
                ViewState("lnkProjectNo") = "ASC"

                ViewState("sProjNo") = 0
                ViewState("sLeader") = 0
                ViewState("sUGNFacility") = ""
                ViewState("sCommodity") = 0
                ViewState("sProjCat") = 0
                ViewState("sDesc") = ""
                ViewState("sRFDNo") = 0
                ViewState("filterPlantControllerReviewed") = 0
                ViewState("isPlantControllerReviewed") = 0
                ViewState("filterOffsetsCostDowns") = 0
                ViewState("isOffsetsCostDowns") = 0
                ViewState("includeCompleted") = 0

                BindCriteria()

                ''******
                '' Store searched values in cookies to return back to previous searches.  
                '' User must use the reset button to clear out values.
                ''******
                If Not Request.Cookies("CR_ProjNo") Is Nothing Then
                    txtProjectNo.Text = Server.HtmlEncode(Request.Cookies("CR_ProjNo").Value)
                    ViewState("sProjNo") = Server.HtmlEncode(Request.Cookies("CR_ProjNo").Value)
                End If

                If Not Request.Cookies("CR_Leader") Is Nothing Then
                    ddLeader.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Leader").Value)
                    ViewState("sLeader") = Server.HtmlEncode(Request.Cookies("CR_Leader").Value)
                End If

                If Not Request.Cookies("CR_UGNFacility") Is Nothing Then
                    ddUGNFacility.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_UGNFacility").Value)
                    ViewState("sUGNFacility") = Server.HtmlEncode(Request.Cookies("CR_UGNFacility").Value)
                End If

                If Not Request.Cookies("CR_Commodity") Is Nothing Then
                    ddCommodity.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_Commodity").Value)
                    ViewState("sCommodity") = Server.HtmlEncode(Request.Cookies("CR_Commodity").Value)
                End If

                If Not Request.Cookies("CR_ProjCat") Is Nothing Then
                    ddProjectCategory.SelectedValue = Server.HtmlEncode(Request.Cookies("CR_ProjCat").Value)
                    ViewState("sProjCat") = Server.HtmlEncode(Request.Cookies("CR_ProjCat").Value)
                End If

                If Not Request.Cookies("CR_Desc") Is Nothing Then
                    txtDescription.Text = Server.HtmlEncode(Request.Cookies("CR_Desc").Value)
                    ViewState("sDesc") = Server.HtmlEncode(Request.Cookies("CR_Desc").Value)
                End If

                If Not Request.Cookies("CR_RFDNo") Is Nothing Then
                    txtRFDNo.Text = Server.HtmlEncode(Request.Cookies("CR_RFDNo").Value)
                    ViewState("sRFDNo") = Server.HtmlEncode(Request.Cookies("CR_RFDNo").Value)
                End If


                If Not Request.Cookies("CR_FilterPlantControllerReviewed") Is Nothing Then
                    If Not Request.Cookies("CR_IsPlantControllerReviewed") Is Nothing Then
                        If CType(Request.Cookies("CR_FilterPlantControllerReviewed").Value, Integer) = 1 Then
                            ViewState("filterPlantControllerReviewed") = 1
                            ViewState("isPlantControllerReviewed") = CType(Request.Cookies("CR_IsPlantControllerReviewed").Value, Integer)
                            ddPlantControllerReviewed.SelectedValue = CType(Request.Cookies("CR_IsPlantControllerReviewed").Value, Integer)
                        End If
                    End If
                End If

                If Not Request.Cookies("CR_FilterOffsetsCostDowns") Is Nothing Then
                    If Not Request.Cookies("CR_IsOffsetsCostDowns") Is Nothing Then
                        If CType(Request.Cookies("CR_FilterOffsetsCostDowns").Value, Integer) = 1 Then
                            ViewState("filterOffsetsCostDowns") = 1
                            ViewState("isOffsetsCostDowns") = CType(Request.Cookies("CR_IsOffsetsCostDowns").Value, Integer)
                            ddOffsetsCostDowns.SelectedValue = CType(Request.Cookies("CR_IsOffsetsCostDowns").Value, Integer)
                        End If
                    End If
                End If

                If Not Request.Cookies("CR_IncludeCompleted") Is Nothing Then                    
                    ViewState("includeCompleted") = CType(Server.HtmlEncode(Request.Cookies("CR_IncludeCompleted").Value), Integer)
                    cbIncludeCompleted.Checked = ViewState("includeCompleted")
                End If

                ''******
                '' Bind drop down lists
                ''******
                BindData()

            Else
                ViewState("sProjNo") = txtProjectNo.Text
                ViewState("sLeader") = ddLeader.SelectedValue
                ViewState("sUGNFacility") = ddUGNFacility.SelectedValue
                ViewState("sCommodity") = ddCommodity.SelectedValue
                ViewState("sProjCat") = ddProjectCategory.SelectedValue
                ViewState("sDesc") = txtDescription.Text
                ViewState("sRFDNo") = txtRFDNo.Text

                ViewState("filterPlantControllerReviewed") = 0
                ViewState("isPlantControllerReviewed") = 0
                If ddPlantControllerReviewed.SelectedIndex > 0 Then
                    ViewState("filterPlantControllerReviewed") = 1
                    ViewState("isPlantControllerReviewed") = ddPlantControllerReviewed.SelectedValue
                End If

                ViewState("filterOffsetsCostDowns") = 0
                ViewState("isOffsetsCostDowns") = 0
                If ddOffsetsCostDowns.SelectedIndex > 0 Then
                    ViewState("filterOffsetsCostDowns") = 1
                    ViewState("isOffsetsCostDowns") = ddOffsetsCostDowns.SelectedValue
                End If

                If cbIncludeCompleted.Checked = True Then
                    ViewState("includeCompleted") = 1
                Else
                    ViewState("includeCompleted") = 0
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            ViewState("ObjectRole") = False
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

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
            ViewState("isProposedDetailsViewable") = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 97 'Cost Reduction Project Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")

                    '' developer testing as another team member
                    'If iTeamMemberID = 530 Then
                    '    'iTeamMemberID = 553 'bill muha
                    '    'iTeamMemberID = 612 'dan marcon                        
                    '    'iTeamMemberID = 571 'adrian way   
                    '    'iTeamMemberID = 171 'greg hall
                    '    iTeamMemberID = 657 'nicolas leclercq
                    'End If

                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")
                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        ViewState("ObjectRole") = True
                                        ViewState("Admin") = True
                                        ViewState("isProposedDetailsViewable") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        ViewState("isProposedDetailsViewable") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = False
                                        ViewState("isProposedDetailsViewable") = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                        ViewState("isProposedDetailsViewable") = True
                                        'btnAdd.Enabled = False
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        'btnAdd.Enabled = False
                                        ViewState("isProposedDetailsViewable") = True
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ''** No Entry allowed **''
                                        ViewState("ObjectRole") = False
                                        'btnAdd.Enabled = False
                                        ViewState("isProposedDetailsViewable") = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"

                        btnAdd.Enabled = ViewState("ObjectRole")
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
#End Region 'EOF Form Level Security

    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Project Leader control for selection criteria for search
        ds = CRModule.GetCostReductionProjectLeaders()
        If commonFunctions.CheckDataset(ds) = True Then
            ddLeader.DataSource = ds
            ddLeader.DataTextField = ds.Tables(0).Columns("ddTeamMemberName").ColumnName.ToString()
            ddLeader.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName
            ddLeader.DataBind()
            ddLeader.Items.Insert(0, "")
        End If

        ''bind existing data to drop down UGN Location control for selection criteria for search
        ds = commonFunctions.GetUGNFacility("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddUGNFacility.DataSource = ds
            ddUGNFacility.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
            ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName.ToString()
            ddUGNFacility.DataBind()
            ddUGNFacility.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Commodity control for selection criteria 
        ds = commonFunctions.GetCommodity(0, "", "", 0)
        If commonFunctions.CheckDataset(ds) = True Then
            ddCommodity.DataSource = ds
            ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName.ToString()
            ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
            ddCommodity.DataBind()
            ddCommodity.Items.Insert(0, "")
        End If

        ''bind existing data to drop down Project Category control for selection criteria for search
        ds = CRModule.GetProjectCategory("")
        If commonFunctions.CheckDataset(ds) = True Then
            ddProjectCategory.DataSource = ds
            ddProjectCategory.DataTextField = ds.Tables(0).Columns("ddProjectCategoryName").ColumnName.ToString()
            ddProjectCategory.DataValueField = ds.Tables(0).Columns("PCID").ColumnName.ToString()
            ddProjectCategory.DataBind()
            ddProjectCategory.Items.Insert(0, "")
        End If
    End Sub 'EOF BindCriteria

    Private Sub BindData()

        Try

            Dim ds As DataSet

            ds = CRModule.GetCostReductionSearch(IIf(ViewState("sProjNo") = Nothing, 0, ViewState("sProjNo")), IIf(ViewState("sLeader") = Nothing, 0, ViewState("sLeader")), ViewState("sUGNFacility"), IIf(ViewState("sCommodity") = Nothing, 0, ViewState("sCommodity")), IIf(ViewState("sProjCat") = Nothing, 0, ViewState("sProjCat")), ViewState("sDesc"), IIf(ViewState("sRFDNo") = Nothing, 0, ViewState("sRFDNo")), ViewState("filterPlantControllerReviewed"), ViewState("isPlantControllerReviewed"), ViewState("filterOffsetsCostDowns"), ViewState("isOffsetsCostDowns"), ViewState("includeCompleted"))

            tblRepeater.Visible = False

            btnExportToExcel.Visible = False
            cmdGo.Visible = False

            cmdFirst.Visible = False
            cmdPrev.Visible = False
            cmdNext.Visible = False
            cmdLast.Visible = False

            lblCurrentPage.Visible = False

            txtGoToPage.Visible = False

            If commonFunctions.CheckDataSet(ds) = True Then
                btnExportToExcel.Visible = True
                cmdGo.Visible = True
                tblRepeater.Visible = True

                rpCostReduction.DataSource = ds
                rpCostReduction.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 25

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpCostReduction.DataSource = objPds
                rpCostReduction.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdFirst.Visible = True

                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdPrev.Visible = True

                cmdNext.Enabled = Not objPds.IsLastPage
                cmdNext.Visible = True

                cmdLast.Enabled = Not objPds.IsLastPage
                cmdLast.Visible = True

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

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = CRModule.GetCostReductionSearch(IIf(ViewState("sProjNo") = Nothing, 0, ViewState("sProjNo")), IIf(ViewState("sLeader") = Nothing, 0, ViewState("sLeader")), ViewState("sUGNFacility"), IIf(ViewState("sCommodity") = Nothing, 0, ViewState("sCommodity")), IIf(ViewState("sProjCat") = Nothing, 0, ViewState("sProjCat")), ViewState("sDesc"), IIf(ViewState("sRFDNo") = Nothing, 0, ViewState("sRFDNo")), ViewState("filterPlantControllerReviewed"), ViewState("isPlantControllerReviewed"), ViewState("filterOffsetsCostDowns"), ViewState("isOffsetsCostDowns"), ViewState("includeCompleted"))

            If commonFunctions.CheckDataset(ds) = True Then
                tblRepeater.Visible = True

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpCostReduction.DataSource = dv
                rpCostReduction.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            Else
                tblRepeater.Visible = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles lnkDescription.Click, lnkRank.Click, lnkUGNFacility.Click, lnkProjectCategory.Click, lnkCommodity.Click, lnkEstImpDate.Click, lnkCompletion.Click, lnkProjectNo.Click

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
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

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
            HttpContext.Current.Session("sessionCRCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionCRCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionCRCurrentPage") = CurrentPage

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


                HttpContext.Current.Session("sessionCRCurrentPage") = CurrentPage

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
            HttpContext.Current.Session("sessionCRCurrentPage") = CurrentPage

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
        Response.Redirect("CostReduction.aspx?pProjNo=0", False)
    End Sub 'EOF btnAdd_Click

    Public Sub SendEmailToCRAdmin(ByVal CRProjNo As Integer, ByVal CRDescription As String)
        Try
            ''**************************************************************************
            ''This section is used to send project link to the Cost Reduction administrator
            ''**************************************************************************
            Dim i As Integer = 0
            Dim ds As DataSet = New DataSet

            'Dim dsCC As DataSet = New DataSet
            Dim EmailTO As String = Nothing
            Dim CurrentEmpEmail As String = Nothing
            CurrentEmpEmail = HttpContext.Current.Request.Cookies("UGNDB_User_Email").Value
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'Dim DefaultTMID As Integer = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim SubjectText As String = Nothing
            Dim EmpName As String = Nothing
            'Dim EmailCC As String = Nothing

            ''********************************************************
            ''Send Notification only if there is a valid Email Address
            ''********************************************************
            If CurrentEmpEmail <> Nothing And ViewState("Admin") = True Then
                Dim SendFrom As MailAddress = New MailAddress(CurrentEmpEmail)
                Dim SendTo As MailAddress = Nothing
                Dim MyMessage As MailMessage

                SendTo = New MailAddress(CurrentEmpEmail) 'use for testing only
                MyMessage = New MailMessage(SendFrom, SendTo)

                ''Test or Production Message display
                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Subject = "TEST: "
                    MyMessage.Body = "THIS IS AN EMAIL IN THE UGN DATABASE TEST ENVIRONMENT. DATA IS NOT VALID FOR USE<br><br>"
                Else
                    MyMessage.Subject = ""
                    MyMessage.Body = ""
                End If

                MyMessage.Subject &= "Cost Reduction Project " & SubjectText & CRProjNo

                MyMessage.Body &= "<p><font size='2' face='Verdana'>Please update the status on the Cost Reduction ProjectNo: " & CRProjNo
                MyMessage.Body &= "<br><br>Description: " & CRDescription
                MyMessage.Body &= "<br><br><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/CostReduction.aspx?pProjNo=" & CRProjNo & "'>Click Here to View Project " & CRProjNo & " Details " & "</a> "
                'MyMessage.Body &= "<br><br><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/CR/crViewCostReductionDetail.aspx?pProjNo=" & CRProjNo & "'>Click Here to Preview Project " & CRProjNo & " in PDF format. " & "</a> "
                MyMessage.Body &= "</p><br>Thank you."
                MyMessage.Body &= "</font></p><br><br>+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++<br><br><h4>This email is intended to broadcast information only.  Please <u>do not</u> reply back to this email because you will not receive a response.<br>Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.</h4>+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++<br>"

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    MyMessage.Body &= "<p>EmailTO: " & EmailTO & "</p>"
                    'MyMessage.Body &= "<p>EmailCC: " & EmailCC & "</p>"
                End If

                ''**********************************
                ''Connect & Send email notification
                ''**********************************
                MyMessage.IsBodyHtml = True
                Dim emailClient As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)
                emailClient.Send(MyMessage)

            End If 'If CurrentEmpEmail <> Nothing And ViewState("Admin") = True

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub aHlinkEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim TempaHlinkEmail As HtmlAnchor

        Dim strProjectNo As String = ""
        Dim strDescription As String = ""
        Dim iColonPos As Integer = 0

        TempaHlinkEmail = CType(sender, HtmlAnchor)

        iColonPos = InStr(TempaHlinkEmail.Title, ":")

        If iColonPos > 0 Then

            strProjectNo = Mid(TempaHlinkEmail.Title, 1, iColonPos - 1)
            strDescription = Mid(TempaHlinkEmail.Title, iColonPos + 1)

            SendEmailToCRAdmin(strProjectNo, strDescription)

            lblErrors.Text = "ProjectNo:" & strProjectNo & " has been sent to the Cost Reduction Administrator."
            lblErrors.Visible = True
        End If

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            lblErrors.Text = ""

            HttpContext.Current.Session("sessionCRCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("CR_ProjNo").Value = txtProjectNo.Text
            Response.Cookies("CR_Leader").Value = ddLeader.SelectedValue
            Response.Cookies("CR_UGNFacility").Value = ddUGNFacility.SelectedValue
            Response.Cookies("CR_Commodity").Value = ddCommodity.SelectedValue
            Response.Cookies("CR_ProjCat").Value = ddProjectCategory.SelectedValue
            Response.Cookies("CR_Desc").Value = txtDescription.Text
            Response.Cookies("CR_RFDNo").Value = txtRFDNo.Text

            Response.Cookies("CR_FilterPlantControllerReviewed").Value = 0
            Response.Cookies("CR_IsPlantControllerReviewed").Value = 0
            If ddPlantControllerReviewed.SelectedIndex > 0 Then
                Response.Cookies("CR_FilterPlantControllerReviewed").Value = 1
                Response.Cookies("CR_IsPlantControllerReviewed").Value = ddPlantControllerReviewed.SelectedValue
            End If

            Response.Cookies("CR_FilterOffsetsCostDowns").Value = 0
            Response.Cookies("CR_IsOffsetsCostDowns").Value = 0
            If ddOffsetsCostDowns.SelectedIndex > 0 Then
                Response.Cookies("CR_FilterOffsetsCostDowns").Value = 1
                Response.Cookies("CR_IsOffsetsCostDowns").Value = ddOffsetsCostDowns.SelectedValue
            End If

            If cbIncludeCompleted.Checked = True Then
                ViewState("includeCompleted") = 1
                Response.Cookies("CR_IncludeCompleted").Value = 1
            Else
                ViewState("includeCompleted") = 0
                Response.Cookies("CR_IncludeCompleted").Value = 0
                Response.Cookies("CR_IncludeCompleted").Expires = DateTime.Now.AddDays(-1)
            End If

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
            lblErrors.Text = ""

            CRModule.DeleteCostReductionCookies()

            HttpContext.Current.Session("sessionCRCurrentPage") = Nothing

            Response.Redirect("CostReductionList.aspx", False)

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnReset_click

    Protected Function SetTextColor(ByVal strDate As String, ByVal ProjectCategoryName As String) As String

        Dim strReturnValue As String = "Black"
        If Date.Today > strDate And ProjectCategoryName <> "Completed" Then
            strReturnValue = "Red"
        End If
        SetTextColor = strReturnValue

    End Function 'EOF SetTextColor
End Class
