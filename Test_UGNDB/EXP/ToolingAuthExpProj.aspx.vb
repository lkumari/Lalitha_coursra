' ************************************************************************************************
' Name:		ToolingAuthExpProj.aspx
' Purpose:	This Code Behind is for the Tooling Authorization Data entry
'
' Date		Author	    
' 12/12/2011    Roderick Carlson
' 01/10/2013    Roderick Carlson - Due to some RFDs (Quote Only Source Quotes) not needing programs, there must be some allowance for missing programs
' 03/07/2013    Roderick Carlson - Reggie Parenza - allow prototype parts to be more flexible - no rfd, no DMS drawing, no existance
' ************************************************************************************************
Partial Class ToolingAuthExpProj
    Inherits System.Web.UI.Page

    Private _totalMaterial As Double = 0

    Private _totalNumberHours As Double = 0
    Private _totalLabor As Double = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Tooling Authorization Details"

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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False
            ViewState("isEnabled") = False

            ViewState("StatusID") = 0
            ViewState("TeamMemberID") = 0

            ViewState("TANo") = 0

            ViewState("OriginalRFDNo") = 0
            ViewState("OriginalCostSheetID") = 0

            ViewState("isProgramManagement") = False
            ViewState("isQualityEngineer") = False

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()

            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    iTeamMemberID = 140 ' Bryan Hall
                    'iTeamMemberID = 22 'Terry Turnquist 
                    'iTeamMemberID = 111 'Nancy Hulbert
                    'iTeamMemberID = 428 'Tracy Theos
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Quality Engineer
                ViewState("isQualityEngineer") = False
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 22)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 22
                    ViewState("isQualityEngineer") = True
                End If

                'Program Management
                ViewState("isProgramManagement") = False
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 31)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 31
                    ViewState("isProgramManagement") = True
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 42)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isEdit") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'Iniator         
            'ds = commonFunctions.GetTeamMember("")
            ds = TAModule.GetTAInitiator()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddInitiatorTeamMember.DataSource = ds
                ddInitiatorTeamMember.DataTextField = ds.Tables(0).Columns("ddFullTeamMemberName").ColumnName.ToString()
                ddInitiatorTeamMember.DataValueField = ds.Tables(0).Columns("TeamMemberID").ColumnName.ToString()
                ddInitiatorTeamMember.DataBind()               
                'ddInitiatorTeamMember.Items.Insert(0, "")
            End If

            ' Quality Engineer
            ds = commonFunctions.GetTeamMemberBySubscription(22)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddQualityEngineer.DataSource = ds
                ddQualityEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddQualityEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddQualityEngineer.DataBind()              
                'ddQualityEngineer.Items.Insert(0, "")
            End If

            ' Account Manager
            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddAccountManager.DataBind()             
                'ddAccountManager.Items.Insert(0, "")
            End If

            ' Program Manager
            ds = commonFunctions.GetTeamMemberBySubscription(31)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProgramManager.DataSource = ds
                ddProgramManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddProgramManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddProgramManager.DataBind()             
                'ddProgramManager.Items.Insert(0, "")
            End If

            ds = TAModule.GetTAStatusMaint()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddStatus.DataSource = ds
                ddStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName
                ddStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddStatus.DataBind()
                'ddStatus.Items.Insert(0, "")
            End If

            'Type of Change
            ds = TAModule.GetTAChangeTypeMaint(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddChangeType.DataSource = ds
                ddChangeType.DataTextField = ds.Tables(0).Columns("ddChangeTypeName").ColumnName
                ddChangeType.DataValueField = ds.Tables(0).Columns("ChangeTypeId").ColumnName
                ddChangeType.DataBind()
                ddChangeType.SelectedIndex = 0
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            'bind UGN Facility
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()             
                'ddUGNFacility.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindData()

        Try
            Dim ds As DataSet

            ds = TAModule.GetTA(ViewState("TANo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                lblTANo.Text = ViewState("TANo")
                lblTAProjectNo.Text = ds.Tables(0).Rows(0).Item("TAProjectNo").ToString

                If ds.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                        ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountManagerID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID") > 0 Then
                        If ddInitiatorTeamMember.Items.FindByValue(ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID")) IsNot Nothing Then
                            ddInitiatorTeamMember.SelectedValue = ds.Tables(0).Rows(0).Item("InitiatorTeamMemberID")
                        End If
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("ProgramManagerID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ProgramManagerID") > 0 Then
                        ddProgramManager.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramManagerID")
                    End If
                End If

                If ds.Tables(0).Rows(0).Item("QualityEngineerID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("QualityEngineerID") > 0 Then
                        ddQualityEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("QualityEngineerID")
                    End If
                End If

                If ddQualityEngineer.SelectedValue = "" And ViewState("isQualityEngineer") = True Then
                    ddQualityEngineer.SelectedValue = ViewState("TeamMemberID")
                End If

                ViewState("StatusID") = 1
                If ds.Tables(0).Rows(0).Item("StatusID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("StatusID") > 0 Then
                        ViewState("StatusID") = ds.Tables(0).Rows(0).Item("StatusID")
                        ddStatus.SelectedValue = ds.Tables(0).Rows(0).Item("StatusID")
                    End If
                End If

                txtTADesc.Text = ds.Tables(0).Rows(0).Item("TADesc").ToString.Trim
                txtDueDate.Text = ds.Tables(0).Rows(0).Item("DueDate").ToString
                lblIssueDate.Text = ds.Tables(0).Rows(0).Item("IssueDate").ToString
                txtImplementationDate.Text = ds.Tables(0).Rows(0).Item("ImplementationDate").ToString

                If ds.Tables(0).Rows(0).Item("VoidComment").ToString.Trim <> "" Then
                    txtVoidComment.Text = ds.Tables(0).Rows(0).Item("VoidComment").ToString.Trim
                    txtVoidComment.Visible = True
                    lblVoidComment.Visible = True
                    lblVoidCommentMarker.Visible = True
                Else
                    txtVoidComment.Visible = False
                    lblVoidComment.Visible = False
                    lblVoidCommentMarker.Visible = False
                End If

                ViewState("OriginalRFDNo") = 0
                If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                        txtRFDNo.Text = ds.Tables(0).Rows(0).Item("RFDNo")
                        ViewState("OriginalRFDNo") = ds.Tables(0).Rows(0).Item("RFDNo")
                    End If
                End If

                ViewState("OriginalCostSheetID") = 0
                If ds.Tables(0).Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("CostSheetID") > 0 Then
                        txtCostSheetID.Text = ds.Tables(0).Rows(0).Item("CostSheetID")
                        ViewState("OriginalCostSheetID") = ds.Tables(0).Rows(0).Item("CostSheetID")
                    End If
                End If

                ddUGNFacility.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString.Trim

                txtChargeOther.Text = ds.Tables(0).Rows(0).Item("ChargeOther").ToString.Trim
                If txtChargeOther.Text.Trim <> "" Then
                    cbCharge.Checked = False
                End If

                ddChangeType.SelectedValue = ds.Tables(0).Rows(0).Item("ChangeTypeID").ToString.Trim

                txtInstructions.Text = ds.Tables(0).Rows(0).Item("Instructions").ToString
                txtRules.Text = ds.Tables(0).Rows(0).Item("Rules").ToString

                If txtRules.Text.Trim = "" Then
                    txtRules.Text = "4 Pt 6 53/1 6 lb. CF-1.125"
                End If

                txtSerialNo.Text = ds.Tables(0).Rows(0).Item("SerialNo").ToString

                If ds.Tables(0).Rows(0).Item("isDieshopComplete") IsNot System.DBNull.Value Then
                    cbDieshopComplete.Checked = ds.Tables(0).Rows(0).Item("isDieshopComplete")
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Function DisplayImage(ByVal EncodeType As String) As String
        Dim strReturn As String = ""

        If EncodeType = Nothing Then
            strReturn = ""
        ElseIf EncodeType = "application/vnd.ms-excel" Or EncodeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            strReturn = "~/images/xls.jpg"
        ElseIf EncodeType = "application/pdf" Then
            strReturn = "~/images/pdf.jpg"
        ElseIf EncodeType = "application/msword" Or EncodeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            strReturn = "~/images/doc.jpg"
        Else
            strReturn = "~/images/PreviewUp.jpg"
        End If

        Return strReturn
    End Function 'EOF DisplayImage
    Private Sub ImportRFD()

        Try
            ''bind existing RFD info to Tooling Authorization

            Dim ds As DataSet
            Dim dsTAFinishedPart As DataSet
            Dim iTAFinishedPartRowCounter As Integer = 0

            Dim dsTAChildPart As DataSet
            Dim iTAChildPartRowCounter As Integer = 0

            Dim dsTACustomerProgram As DataSet
            Dim iTACustomerProgramRowCounter As Integer = 0

            Dim dtChildPart As DataTable
            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL

            Dim dtInternalFinishedPart As DataTable
            Dim objRFDInternalFinishedPartBLL As RFDFinishedGoodBLL = New RFDFinishedGoodBLL

            Dim dtCustomerProgram As DataTable
            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL

            Dim dtFacilityDept As DataTable
            Dim objRFDFacilityDeptBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL

            Dim iRFDNo As Integer = 0
            Dim iChildRowID As Integer = 0

            Dim iRowCounter As Integer = 0
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0
            Dim strNewDrawingNo As String = ""
            Dim strCurrentCustomerPartName As String = ""

            Dim dsRFDQE As DataSet

            If txtRFDNo.Text.Trim <> "" Then
                iRFDNo = CType(txtRFDNo.Text.Trim, Integer)

                If iRFDNo > 0 Then
                    ds = RFDModule.GetRFD(iRFDNo)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        'need to wipe out old programs, finished parts, and child parts

                        'wipe out finished parts
                        dsTAFinishedPart = TAModule.GetTAFinishedPart(ViewState("TANo"))
                        If commonFunctions.CheckDataSet(dsTAFinishedPart) = True Then
                            For iTAFinishedPartRowCounter = 0 To dsTAFinishedPart.Tables(0).Rows.Count - 1
                                TAModule.DeleteTAFinishedPart(dsTAFinishedPart.Tables(0).Rows(iTAFinishedPartRowCounter).Item("RowID"), dsTAFinishedPart.Tables(0).Rows(iTAFinishedPartRowCounter).Item("RowID"))
                            Next
                        End If

                        'wipe out child parts
                        dsTAChildPart = TAModule.GetTAChildPart(ViewState("TANo"))
                        If commonFunctions.CheckDataSet(dsTAChildPart) = True Then
                            For iTAChildPartRowCounter = 0 To dsTAChildPart.Tables(0).Rows.Count - 1
                                TAModule.DeleteTAChildPart(dsTAChildPart.Tables(0).Rows(iTAChildPartRowCounter).Item("RowID"), dsTAChildPart.Tables(0).Rows(iTAChildPartRowCounter).Item("RowID"))
                            Next
                        End If

                        'wipe out customer programs
                        dsTACustomerProgram = TAModule.GetTACustomerProgram(ViewState("TANo"))
                        If commonFunctions.CheckDataSet(dsTACustomerProgram) = True Then
                            For iTACustomerProgramRowCounter = 0 To dsTACustomerProgram.Tables(0).Rows.Count - 1
                                TAModule.DeleteTACustomerProgram(dsTACustomerProgram.Tables(0).Rows(iTACustomerProgramRowCounter).Item("RowID"), dsTACustomerProgram.Tables(0).Rows(iTACustomerProgramRowCounter).Item("RowID"))
                            Next
                        End If

                        'ddAccountManager.SelectedValue = -1
                        If ds.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                                ddAccountManager.SelectedValue = ds.Tables(0).Rows(0).Item("AccountManagerID")
                            End If
                        End If

                        'ddProgramManager.SelectedValue = -1
                        If ds.Tables(0).Rows(0).Item("ProgramManagerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ProgramManagerID") > 0 Then
                                ddProgramManager.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramManagerID")
                            End If
                        End If

                        txtCostSheetID.Text = ""
                        If ds.Tables(0).Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CostSheetID") > 0 Then
                                txtCostSheetID.Text = ds.Tables(0).Rows(0).Item("CostSheetID").ToString
                            End If
                        End If

                        strNewDrawingNo = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString
                        strCurrentCustomerPartName = ds.Tables(0).Rows(0).Item("CurrentCustomerPartName").ToString

                        'ddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("NewTopLevelDesignationType").ToString
                        If ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString <> "" Then

                            dtInternalFinishedPart = objRFDInternalFinishedPartBLL.GetRFDFinishedGood(iRFDNo)
                            'if internal finished good list exists in RFD else insert based on Customer Part Number
                            If dtInternalFinishedPart.Rows.Count > 0 Then
                                For iRowCounter = 0 To dtInternalFinishedPart.Rows.Count - 1

                                    'see if the RFD assigned a Drawing number to the new finished good, else use the parent DMS DrawingNo
                                    If dtInternalFinishedPart.Rows(0).Item("DrawingNo").ToString <> "" Then
                                        strNewDrawingNo = dtInternalFinishedPart.Rows(iRowCounter).Item("DrawingNo").ToString
                                    Else
                                        strNewDrawingNo = ds.Tables(0).Rows(0).Item("NewDrawingNo").ToString
                                    End If

                                    'see if the RFD assigned a Part Name to the new finished good, else use the Customer Part Name
                                    If dtInternalFinishedPart.Rows(0).Item("PartName").ToString <> "" Then
                                        strCurrentCustomerPartName = dtInternalFinishedPart.Rows(iRowCounter).Item("PartName").ToString
                                    Else
                                        strCurrentCustomerPartName = ds.Tables(0).Rows(0).Item("CurrentCustomerPartName").ToString
                                    End If

                                    If commonFunctions.CheckDataTable(dtInternalFinishedPart) = True Then
                                        TAModule.InsertTAFinishedPart(ViewState("TANo"), ds.Tables(0).Rows(0).Item("CurrentCustomerPartNo").ToString, strCurrentCustomerPartName, "", ds.Tables(0).Rows(0).Item("CurrentDesignLevel").ToString, ds.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString, ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString, ds.Tables(0).Rows(0).Item("NewCustomerPartName").ToString, dtInternalFinishedPart.Rows(iRowCounter).Item("PartNo").ToString, ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString, strNewDrawingNo)
                                    End If
                                Next
                            Else
                                TAModule.InsertTAFinishedPart(ViewState("TANo"), ds.Tables(0).Rows(0).Item("CurrentCustomerPartNo").ToString, ds.Tables(0).Rows(0).Item("CurrentCustomerPartName").ToString, "", ds.Tables(0).Rows(0).Item("CurrentDesignLevel").ToString, ds.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString, ds.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString, ds.Tables(0).Rows(0).Item("NewCustomerPartName").ToString, "", ds.Tables(0).Rows(0).Item("NewDesignLevel").ToString, strNewDrawingNo)
                            End If

                        End If

                        dtChildPart = objRFDChildPartBLL.GetRFDChildPart(0, iRFDNo)
                        For iRowCounter = 0 To dtChildPart.Rows.Count - 1
                            If commonFunctions.CheckDataTable(dtChildPart) = True Then
                                'only allow first Semi-Finished Good to be selected.
                                If dtChildPart.Rows(iRowCounter).Item("NewDesignationType").ToString = "B" Then

                                    txtCostSheetID.Text = ""
                                    If dtChildPart.Rows(0).Item("CostSheetId") IsNot System.DBNull.Value Then
                                        txtCostSheetID.Text = dtChildPart.Rows(0).Item("CostSheetId").ToString
                                    End If

                                    ' ddDesignationType.SelectedValue = dtChildPart.Rows(0).Item("NewDesignationType").ToString
                                    TAModule.InsertTAChildPart(ViewState("TANo"), dtChildPart.Rows(iRowCounter).Item("CurrentPartNo").ToString, dtChildPart.Rows(iRowCounter).Item("CurrentPartName").ToString, dtChildPart.Rows(iRowCounter).Item("CurrentDrawingNo").ToString, dtChildPart.Rows(iRowCounter).Item("NewPartNo").ToString, dtChildPart.Rows(iRowCounter).Item("NewPartName").ToString, dtChildPart.Rows(iRowCounter).Item("NewDrawingNo").ToString)

                                    iRowCounter = dtChildPart.Rows.Count - 1
                                End If
                            End If
                        Next

                        'append to customer program list
                        dtCustomerProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(iRFDNo)

                        If commonFunctions.CheckDataTable(dtCustomerProgram) = True Then

                            For iRowCounter = 0 To dtCustomerProgram.Rows.Count - 1
                                iProgramID = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramID") > 0 Then
                                        iProgramID = dtCustomerProgram.Rows(iRowCounter).Item("ProgramID")
                                    End If
                                End If

                                iProgramYear = 0
                                If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                    If dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear") > 0 Then
                                        iProgramYear = dtCustomerProgram.Rows(iRowCounter).Item("ProgramYear")
                                    End If
                                End If

                                If iProgramID > 0 Then
                                    TAModule.InsertTACustomerProgram(ViewState("TANo"), iProgramID, iProgramYear)
                                End If

                            Next
                        End If

                        'get the first facility in the list
                        dtFacilityDept = objRFDFacilityDeptBLL.GetRFDFacilityDept(iRFDNo)

                        If commonFunctions.CheckDataTable(dtFacilityDept) = True Then
                            ddUGNFacility.SelectedValue = dtFacilityDept.Rows(0).Item("UGNFacility").ToString
                        End If

                        'get RFD QE approver
                        dsRFDQE = RFDModule.GetRFDApproval(iRFDNo, 22, 0, False, False, False, True, True) 'quality engineering

                        If commonFunctions.CheckDataSet(dsRFDQE) = True Then
                            If dsRFDQE.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                                ddQualityEngineer.SelectedValue = dsRFDQE.Tables(0).Rows(0).Item("TeamMemberID")
                            End If
                        End If

                        SaveToolingAuthorization()

                        gvCustomerProgram.DataBind()
                        gvFinishedPart.DataBind()
                        gvChildPart.DataBind()

                        lblMessage.Text &= "<br />Information successfully copied from RFD."
                    End If
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
    Protected Sub iBtnGetRFDinfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetRFDinfo.Click

        Try
            ClearMessages()

            'bind existing RFD info to Tooling Authorization

            'if this is a brand new Cost Sheet, then save it first
            If ViewState("TANo") = 0 Then
                'btnSave_Click(sender, e)
                SaveToolingAuthorization()
            End If

            ImportRFD()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub DisableControls()

        Try


            'btnDieshop.Visible = False
            'btnDieshopBottom.Visible = False
            btnNotify.Visible = False
            btnPreviewTA.Visible = False
            btnPreviewTABottom.Visible = False
            btnSave.Visible = False
            btnSaveBottom.Visible = False

            'btnVoid.Visible = False

            cbCharge.Enabled = False
            cbNotifyAll.Visible = False

            ddAccountManager.Enabled = False
            ddChangeType.Enabled = False
            ddInitiatorTeamMember.Enabled = False
            ddQualityEngineer.Enabled = False
            ddProgramManager.Enabled = False
            ddStatus.Enabled = False
            ddUGNFacility.Enabled = False

            gvChildPart.Columns(0).Visible = False
            gvCustomerProgram.Columns(0).Visible = False
            gvFinishedPart.Columns(0).Visible = False

            'gvToolingAuthTask.Columns(gvToolingAuthTask.Columns.Count - 1).Visible = False
            'gvToolingAuthTask.Columns(gvToolingAuthTask.Columns.Count - 2).Visible = False
            gvToolingAuthTask.Columns(0).Visible = False
            gvToolingAuthTask.Columns(1).Visible = False
            If gvToolingAuthTask.FooterRow IsNot Nothing Then
                gvToolingAuthTask.ShowFooter = False
            End If

            gvSupportingDoc.Columns(0).Visible = False

            hlnkRFD.Visible = False
            hlnkCostSheet.Visible = False
            hlnkDieLayout.Visible = False

            iBtnGetRFDinfo.Visible = False

            imgDueDate.Visible = False
            imgImplementationDate.Visible = False

            menuTabs.Items(1).Enabled = False
            menuTabs.Items(2).Enabled = False
            menuTabs.Items(3).Enabled = False
            menuTabs.Items(4).Enabled = False
            menuTabs.Items(5).Enabled = False

            txtRFDNo.Enabled = False
            txtCostSheetID.Enabled = False
            txtDueDate.Enabled = False
            txtImplementationDate.Enabled = False
            txtTADesc.Enabled = False
            txtChargeOther.Enabled = False

            tblCustomerProgramEdit.Visible = False
            tblFinishedPart.Visible = False
            tblUpload.Visible = False

            '''''''''''''''Dieshop '''''''''''''''
            txtInstructions.Enabled = False
            txtRules.Enabled = False
            txtSerialNo.Enabled = False

            gvMaterial.Columns(0).Visible = False
            If gvMaterial.FooterRow IsNot Nothing Then
                gvMaterial.ShowFooter = False
            End If

            gvLabor.Columns(0).Visible = False
            If gvLabor.FooterRow IsNot Nothing Then
                gvLabor.ShowFooter = False
            End If
            '''''''''''''''''''''''''''''''''''''

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub EnableControls()

        Try
            Dim dsRFD As DataSet
            Dim iRFDNo As Integer = 0

            Dim dsCostSheet As DataSet

            DisableControls()

            ViewState("isEnabled") = False

            If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 And ViewState("isEdit") = True Then
                ViewState("isEnabled") = True

                btnSave.Visible = ViewState("isAdmin")
                'btnVoid.Visible = ViewState("isAdmin")

                'only allow Quality Engineer, Program Manager, or Initiator to import from RFD
                If ViewState("isQualityEngineer") = True _
                    Or ViewState("isProgramManagement") = True _
                    Or ViewState("TeamMemberID") = ddInitiatorTeamMember.SelectedValue Then
                    'ddStatus.Enabled = ViewState("isAdmin")
                    iBtnGetRFDinfo.Visible = ViewState("isAdmin")
                End If

                imgDueDate.Visible = ViewState("isAdmin")
                imgImplementationDate.Visible = ViewState("isAdmin")

                txtChargeOther.Enabled = Not cbCharge.Checked
            Else
                ViewState("isEdit") = False
                ViewState("isAdmin") = False
            End If

            btnSaveBottom.Visible = btnSave.Visible

            cbCharge.Enabled = ViewState("isAdmin")

            ddAccountManager.Enabled = ViewState("isAdmin")
            ddChangeType.Enabled = ViewState("isAdmin")
            ddInitiatorTeamMember.Enabled = ViewState("isAdmin")
            ddProgramManager.Enabled = ViewState("isAdmin")
            ddQualityEngineer.Enabled = ViewState("isAdmin")
            ddUGNFacility.Enabled = ViewState("isAdmin")

            txtCostSheetID.Enabled = ViewState("isAdmin")
            txtDueDate.Enabled = ViewState("isAdmin")
            txtImplementationDate.Enabled = ViewState("isAdmin")
            txtRFDNo.Enabled = ViewState("isAdmin")
            txtTADesc.Enabled = ViewState("isAdmin")
            txtVoidComment.Enabled = ViewState("isAdmin")

            ' do not show grids until TA has been created
            If ViewState("TANo") <> 0 Then
                menuTabs.Items(1).Enabled = True
                menuTabs.Items(2).Enabled = True
                menuTabs.Items(3).Enabled = True
                menuTabs.Items(4).Enabled = True
                menuTabs.Items(5).Enabled = True

                gvChildPart.Visible = True
                gvFinishedPart.Visible = True

                Dim strPreviewTAClientScript As String = "javascript:void(window.open('crViewExpProjToolingAuth.aspx?FormType=TA&ArchiveData=0&TAProjectNo=" & lblTAProjectNo.Text.Trim & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                btnPreviewTA.Attributes.Add("onclick", strPreviewTAClientScript)
                btnPreviewTABottom.Attributes.Add("onclick", strPreviewTAClientScript)

                Dim strPreviewDieshopClientScript As String = "javascript:void(window.open('crViewExpProjToolingAuth.aspx?FormType=DS&ArchiveData=0&TAProjectNo=" & lblTAProjectNo.Text.Trim & "'," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
                btnPreviewDieshop.Attributes.Add("onclick", strPreviewDieshopClientScript)
                btnPreviewDieshopBottom.Attributes.Add("onclick", strPreviewDieshopClientScript)

                'btnCopyBottom.Visible = ViewState("isEnabled")

                'voided
                If ViewState("StatusID") = 4 Then
                    btnPreviewTA.Visible = False
                Else
                    btnPreviewTA.Visible = True
                End If

                'btnDieshop.Visible = btnPreviewTA.Visible
                'btnDieshopBottom.Visible = btnPreviewTA.Visible

                btnPreviewDieshop.Visible = btnPreviewTA.Visible
                btnPreviewDieshopBottom.Visible = btnPreviewTA.Visible

                btnPreviewTABottom.Visible = btnPreviewTA.Visible

                'if the RFD is referenced then it should be validated and show hyper links plus other controls need to stay disabled
                If txtRFDNo.Text.Trim <> "" Then

                    iRFDNo = CType(txtRFDNo.Text.Trim, Integer)

                    If iRFDNo > 0 Then

                        dsRFD = RFDModule.GetRFD(iRFDNo)

                        If commonFunctions.CheckDataSet(dsRFD) = True Then
                            hlnkRFD.Visible = True
                            hlnkRFD.NavigateUrl = "~/RFD/crRFD_Preview.aspx?RFDNo=" & txtRFDNo.Text.Trim

                            txtCostSheetID.Enabled = False
                            ddUGNFacility.Enabled = False
                        Else
                            'wipe out RFD number if invalid
                            txtRFDNo.Text = ""
                        End If

                    End If
                Else
                    If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 Then
                        tblCustomerProgramEdit.Visible = ViewState("isAdmin")

                         gvCustomerProgram.Columns(0).Visible = ViewState("isAdmin")
                        gvChildPart.Columns(0).Visible = ViewState("isAdmin")
                        gvFinishedPart.Columns(0).Visible = ViewState("isAdmin")

                    End If

                End If

                'if the Cost Sheet is referenced then it should be validated 
                If txtCostSheetID.Text.Trim <> "" Then
                    dsCostSheet = CostingModule.GetCostSheet(CType(txtCostSheetID.Text.Trim, Integer))

                    If commonFunctions.CheckDataSet(dsCostSheet) = True Then
                        hlnkCostSheet.NavigateUrl = "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
                        hlnkCostSheet.Visible = True

                        If dsCostSheet.Tables(0).Rows(0).Item("isDieCut") IsNot System.DBNull.Value Then
                            If dsCostSheet.Tables(0).Rows(0).Item("isDieCut") = True Then

                                hlnkDieLayout.NavigateUrl = "~/Costing/Die_Layout_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
                                hlnkDieLayout.Visible = True
                            End If
                        End If
                    End If
                Else 'remove cost sheet if invalid
                    txtCostSheetID.Text = ""
                End If

                If ViewState("StatusID") <> 3 And ViewState("StatusID") <> 4 Then

                    'only allow Quality Engineer, Program Manager, or Initiator to change status or notify
                    If ViewState("isQualityEngineer") = True _
                        Or ViewState("isProgramManagement") = True _
                        Or ViewState("TeamMemberID") = ddInitiatorTeamMember.SelectedValue Then
                        ddStatus.Enabled = ViewState("isAdmin")

                        btnCopyBottom.Visible = ViewState("isAdmin")
                        btnNotify.Visible = ViewState("isAdmin")

                        cbNotifyAll.Visible = ViewState("isAdmin")
                    End If

                    If txtRFDNo.Text.Trim = "" Then
                        txtCostSheetID.Enabled = ViewState("isAdmin")
                    End If

                    tblUpload.Visible = ViewState("isAdmin")

                    'tasked team members can edit grid but champions can add or delete
                    'gvToolingAuthTask.Columns(gvToolingAuthTask.Columns.Count - 1).Visible = ViewState("isAdmin")
                    'gvToolingAuthTask.Columns(gvToolingAuthTask.Columns.Count - 2).Visible = ViewState("isEnabled")
                    gvToolingAuthTask.Columns(0).Visible = ViewState("isEnabled")
                    gvToolingAuthTask.Columns(1).Visible = ViewState("isAdmin")
                    If gvToolingAuthTask.FooterRow IsNot Nothing Then
                        gvToolingAuthTask.ShowFooter = ViewState("isAdmin")
                    End If

                    'gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isAdmin")
                    gvSupportingDoc.Columns(0).Visible = ViewState("isEnabled")

                    tblCommunicationBoardExistingQuestion.Visible = ViewState("isEnabled")
                    tblCommunicationBoardNewQuestion.Visible = ViewState("isEnabled")


                    ''''''''''''''' Dieshop '''''''''''''''''''''''''''''
                    If cbDieshopComplete.Checked = False Then
                        btnSaveDieshop.Visible = ViewState("isAdmin")

                        txtInstructions.Enabled = ViewState("isAdmin")
                        txtRules.Enabled = ViewState("isAdmin")
                        txtSerialNo.Enabled = ViewState("isAdmin")

                        gvMaterial.Columns(0).Visible = ViewState("isAdmin")
                        If gvMaterial.FooterRow IsNot Nothing Then
                            gvMaterial.ShowFooter = ViewState("isAdmin")
                        End If

                        gvLabor.Columns(0).Visible = ViewState("isAdmin")
                        If gvLabor.FooterRow IsNot Nothing Then
                            gvLabor.ShowFooter = ViewState("isAdmin")
                        End If

                        btnDieshopComplete.Visible = ViewState("isAdmin")
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''
                End If
            Else
                'based on current team member and subscriptions, prefill initiator, QE, Account Mangers, and Program Manager if possible
                'check RFD if it helps
                If ViewState("isEnabled") = True Then
                    ddInitiatorTeamMember.SelectedValue = ViewState("TeamMemberID")
                End If

                If ViewState("isProgramManagement") = True Then
                    ddProgramManager.SelectedValue = ViewState("TeamMemberID")
                End If

                If ViewState("isQualityEngineer") = True Then
                    ddQualityEngineer.SelectedValue = ViewState("TeamMemberID")
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub HandleCommentFields()

        Try

            txtTADesc.Attributes.Add("onkeypress", "return tbLimit();")
            txtTADesc.Attributes.Add("onkeyup", "return tbCount(" + lblTADescCharCount.ClientID + ");")
            txtTADesc.Attributes.Add("maxLength", "500")

            txtVoidComment.Attributes.Add("onkeypress", "return tbLimit();")
            txtVoidComment.Attributes.Add("onkeyup", "return tbCount(" + lblVoidCommentCharCount.ClientID + ");")
            txtVoidComment.Attributes.Add("maxLength", "150")

            txtInstructions.Attributes.Add("onkeypress", "return tbLimit();")
            txtInstructions.Attributes.Add("onkeyup", "return tbCount(" + lblInstructionsCharCount.ClientID + ");")
            txtInstructions.Attributes.Add("maxLength", "2000")

            txtRules.Attributes.Add("onkeypress", "return tbLimit();")
            txtRules.Attributes.Add("onkeyup", "return tbCount(" + lblRulesCharCount.ClientID + ");")
            txtRules.Attributes.Add("maxLength", "400")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                'clear crystal reports
                TAModule.CleanTACrystalReports()

                InitializeViewState()

                CheckRights()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("TANo") <> "" Then
                    ''Used to allow TM(s) to Communicated with Approvers for Q&A
                    If HttpContext.Current.Request.QueryString("pRC") <> "" Then
                        ViewState("pRC") = HttpContext.Current.Request.QueryString("pRC")
                    Else
                        ViewState("pRC") = 0
                    End If

                    ViewState("TANo") = Replace(HttpContext.Current.Request.QueryString("TANo"), "U", "")

                    BindData()
                End If

                ''***********************************************
                ''Code Below overrides the breadcrumb navigation 
                ''***********************************************
                Dim mpTextBox As Label
                mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                If Not mpTextBox Is Nothing Then
                    mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Spending Requests </b> > <a href='ToolingAuthExpProjList.aspx'><b> Tooling Authorization Search </b></a> > Details > <a href='ToolingAuthExpProjHistory.aspx?TANo=" & ViewState("TANo") & "' > <b >History </b> </a>"

                    mpTextBox.Visible = True
                    Master.FindControl("SiteMapPath1").Visible = False
                End If

                InitializeAllPopUps()

                HandleCommentFields()

                'btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void this? If so, click ok to see and update the void comment field. THEN CLICK VOID AGAIN. ')){}else{return false}")                
                ddStatus.Attributes.Add("onchange", "alert('WARNING: When manually changing the status, please read all messages on the screen before saving AND PLEASE WAIT UNTIL THIS SCREEN REFRESHES.')")

                btnDieshopComplete.Attributes.Add("onclick", "if(confirm('Are you sure that this dieshop cost form is complete? If so, click ok to continue. ')){}else{return false}")

            End If

            'if the team member clicked on a link to the communication board then go to that tab
            If ViewState("pRC") > 0 Then
                mvTabs.ActiveViewIndex = Int32.Parse(5)
                mvTabs.GetActiveView()
                ViewState("pRC") = 0
            End If

            If HttpContext.Current.Session("CopyTA") IsNot Nothing Then
                If HttpContext.Current.Session("CopyTA") <> "" Then
                    lblMessage.Text &= "The tooling authorization was successfully copied and saved."
                    HttpContext.Current.Session("CopyTA") = Nothing
                End If
            End If

            EnableControls()

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub ClearMessages()

        Try

            lblMessage.Text = ""
            lblMessageChildPart.Text = ""
            lblMessageCustomerProgram.Text = ""
            lblMessageCommunicationBoard.Text = ""
            lblMessageDieshop.Text = ""
            lblMessageDieshopBottom.Text = ""
            lblMessageFinishedPart.Text = ""
            lblMessageLabor.Text = ""
            lblMessageMaterial.Text = ""
            lblMessageSaveBottom.Text = ""
            lblMessageSupportingDocs.Text = ""
            lblMessageTasks.Text = ""
            lblMessageTasksBottom.Text = ""

            lblFileUploadLabel.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub RemovePendingTasks()

        Try
            Dim iRowCounter As Integer = 0
            Dim iRowID As Integer = 0

            Dim objTask As New ExpProjToolingAuthBLL
            Dim dtTask As DataTable
            dtTask = objTask.GetTATask(ViewState("TANo"))

            If commonFunctions.CheckDataTable(dtTask) = True Then
                For iRowCounter = 0 To dtTask.Rows.Count - 1
                    If dtTask.Rows(iRowCounter).Item("RowID") IsNot System.DBNull.Value Then
                        If dtTask.Rows(iRowCounter).Item("RowID") > 0 Then
                            iRowID = dtTask.Rows(iRowCounter).Item("RowID")
                        End If
                    End If

                    If dtTask.Rows(iRowCounter).Item("CompletionDate").ToString = "" Then
                        objTask.DeleteTATask(iRowID, iRowID)
                    End If
                Next
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub SaveToolingAuthorization()

        Try

            Dim ds As DataSet

            If cbCharge.Checked = True Then
                txtChargeOther.Text = ""
            End If

            Dim iRFDNo As Integer = 0
            If txtRFDNo.Text.Trim <> "" Then
                iRFDNo = CType(txtRFDNo.Text, Integer)
            End If

            Dim iCostSheetID As Integer = 0
            If txtCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtCostSheetID.Text, Integer)
            End If

            Dim iAccountManagerID As Integer = 0
            If ddAccountManager.SelectedIndex > 0 Then
                iAccountManagerID = ddAccountManager.SelectedValue
            Else
                iAccountManagerID = ddAccountManager.Items(0).Value
            End If

            Dim iInitiatorTeamMemberID As Integer = 0
            If ddInitiatorTeamMember.SelectedIndex > 0 Then
                iInitiatorTeamMemberID = ddInitiatorTeamMember.SelectedValue
            Else
                iInitiatorTeamMemberID = ViewState("TeamMemberID")
            End If

            Dim iProgramManagerID As Integer = 0
            If ddProgramManager.SelectedIndex > 0 Then
                iProgramManagerID = ddProgramManager.SelectedValue
            Else
                iProgramManagerID = ddProgramManager.Items(0).Value
            End If

            'if the curret user is the initiator and is a program manager and the program manager has not been assigned then assign it
            If iProgramManagerID = 0 And ViewState("isProgramManagement") = True Then
                iProgramManagerID = ViewState("TeamMemberID")
            End If

            Dim iQualityEngineerID As Integer = 0
            If ddQualityEngineer.SelectedIndex > 0 Then
                iQualityEngineerID = ddQualityEngineer.SelectedValue
            Else
                iQualityEngineerID = ddQualityEngineer.Items(0).Value
            End If

            'if the curret user is the initiator and is a quality engineer and the quality engineer has not been assigned then assign it
            If iQualityEngineerID = 0 And ViewState("isQualityEngineer") = True Then
                iQualityEngineerID = ViewState("TeamMemberID")
            End If

            'if TA Exists then update, else insert
            If ViewState("TANo") <> 0 Then

                'if the team member is trying to set the stats as complete manually
                If ddStatus.SelectedValue = 3 Then

                    'if dieshop complete
                    'If isDieshopComplete() = True Then
                    If cbDieshopComplete.Checked = True Then
                        ViewState("StatusID") = 3

                        'remove pending tasked team members
                        RemovePendingTasks()

                        'update history
                        TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Tooling Authorization Completed")
                        'Else
                        '    'cannot close until dieshop is closed
                        '    ViewState("StatusID") = 2
                        '    ddStatus.SelectedValue = 2
                        '    lblMessage.Text &= "<br />The Tooling Authorization cannot be set to complete until the dieshop cost form is complete."
                    End If

                End If

                TAModule.UpdateTA(ViewState("TANo"), ViewState("StatusID"), ddChangeType.SelectedValue, txtDueDate.Text.Trim, _
                txtImplementationDate.Text.Trim, iAccountManagerID, iInitiatorTeamMemberID, iProgramManagerID, iQualityEngineerID, _
                iRFDNo, iCostSheetID, txtTADesc.Text.Trim, txtChargeOther.Text.Trim, ddUGNFacility.SelectedValue, _
                txtInstructions.Text.Trim, txtRules.Text.Trim, txtSerialNo.Text.Trim, cbDieshopComplete.Checked)

            Else

                ds = TAModule.InsertTA(1, ddChangeType.SelectedValue, _
                txtDueDate.Text.Trim, Format(Today.Date, "M/d/yyyy"), txtImplementationDate.Text.Trim, iAccountManagerID, _
                iInitiatorTeamMemberID, iProgramManagerID, iQualityEngineerID, iRFDNo, iCostSheetID, _
                txtTADesc.Text.Trim, txtChargeOther.Text.Trim, ddUGNFacility.SelectedValue, _
                txtInstructions.Text.Trim, txtRules.Text.Trim, txtSerialNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("NewTANo") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("NewTANo") > 0 Then
                            ViewState("TANo") = ds.Tables(0).Rows(0).Item("NewTANo")
                            lblTAProjectNo.Text = "U" & ViewState("TANo") 'ViewState("TAProjectNo")
                            lblTANo.Text = ViewState("TANo")
                            ddStatus.SelectedValue = 1
                            ViewState("StatusID") = 1

                            'update history
                            TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Tooling Authorization Created")
                        End If
                    End If

                End If
            End If

            If HttpContext.Current.Session("BLLerror") Is Nothing And InStr(lblMessage.Text, "Record Saved Successfully", CompareMethod.Text) <= 0 Then
                lblMessage.Text &= "<br />Record Saved Successfully.<br />"
            Else
                lblMessage.Text &= "<br />" & HttpContext.Current.Session("BLLerror") & "<br />"
            End If

            'BindData()

            'CheckDieshop()

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub CheckDieshopFormTask()

        Try

            Dim objTask As New ExpProjToolingAuthBLL
            Dim dtTask As DataTable
            Dim iRowCounter As Integer
            Dim bFound As Boolean = False

            Dim iTeamMemberID As Integer = 0

            dtTask = objTask.GetTATask(ViewState("TANo"))

            'search the list of tasks
            If commonFunctions.CheckDataTable(dtTask) = True Then
                For iRowCounter = 0 To dtTask.Rows.Count - 1
                    If dtTask.Rows(iRowCounter).Item("TaskID") IsNot System.DBNull.Value Then
                        If dtTask.Rows(iRowCounter).Item("TaskID") = 10 Then
                            'Task Complete Die Shop Cost Form is found
                            bFound = True
                        End If
                    End If
                Next
            End If

            'If Task Complete Die Shop Cost Form is not found in the list
            If bFound = False Then
                iTeamMemberID = GetDieLayoutWorkflowTeamMemberID()

                If iTeamMemberID > 0 Then
                    objTask.InsertTATask(ViewState("TANo"), 10, iTeamMemberID, Format(Today.Date.AddDays(1), "M/d/yyyy"))
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Function GetDieLayoutWorkflowTeamMemberID() As Integer

        Dim iReturn As Integer = 0

        Try
            Dim dsWorkflow As DataSet
            Dim iWorkflowTeamMemberID As Integer = 0
            Dim iWorkflowBackUpTeamMemberID As Integer = 0
            Dim iWorkflowDeptInChargeTeamMemberID As Integer = 0

            Dim dsTeamMember As DataSet
            Dim iTeamMemberID As Integer = 0

            'get Die Layout View subscription or backup/DepInCharge from Workflow
            dsWorkflow = commonFunctions.GetWorkFlow(0, 2)
            If commonFunctions.CheckDataSet(dsWorkflow) = True Then

                'Workflow Teammember
                If dsWorkflow.Tables(0).Rows(0).Item("TeamMemberID") IsNot System.DBNull.Value Then
                    iWorkflowTeamMemberID = dsWorkflow.Tables(0).Rows(0).Item("TeamMemberID")

                    dsTeamMember = SecurityModule.GetTeamMember(iWorkflowTeamMemberID, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                            'If Workflow team member is not working then get backup
                            If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                                iTeamMemberID = iWorkflowTeamMemberID
                            Else
                                'Workflow Backup Teammember
                                If dsWorkflow.Tables(0).Rows(0).Item("BackupTeamMemberID") IsNot System.DBNull.Value Then
                                    iWorkflowBackUpTeamMemberID = dsWorkflow.Tables(0).Rows(0).Item("BackupTeamMemberID")
                                End If

                                dsTeamMember = SecurityModule.GetTeamMember(iWorkflowBackUpTeamMemberID, "", "", "", "", "", True, Nothing)
                                If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                    If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                                        'If Backup Workflow team member is not working then get deptincharge
                                        If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                                            iTeamMemberID = iWorkflowBackUpTeamMemberID
                                        Else
                                            'Workflow DeptInCharge Teammember
                                            If dsWorkflow.Tables(0).Rows(0).Item("DeptInChargeTMID") IsNot System.DBNull.Value Then
                                                iWorkflowDeptInChargeTeamMemberID = dsWorkflow.Tables(0).Rows(0).Item("DeptInChargeTMID")
                                            End If

                                            dsTeamMember = SecurityModule.GetTeamMember(iWorkflowDeptInChargeTeamMemberID, "", "", "", "", "", True, Nothing)
                                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                                                    'If Workflow team member is not working then get backup
                                                    If dsTeamMember.Tables(0).Rows(0).Item("Working") = True Then
                                                        iTeamMemberID = iWorkflowDeptInChargeTeamMemberID
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            iReturn = iTeamMemberID

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return iReturn

    End Function

    Private Function GetCostSheetRFDNo(ByVal CostSheetID As Integer) As Integer

        'return RFD of CostShhet
        Dim iReturn As Integer = 0

        Try
            Dim ds As DataSet
            ds = CostingModule.GetCostSheet(CostSheetID)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("RFDNo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("RFDNo") > 0 Then
                        iReturn = ds.Tables(0).Rows(0).Item("RFDNo")
                    End If
                End If
            Else
                txtCostSheetID.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return iReturn

    End Function

    Private Function CheckRFD() As Boolean

        Dim bReturn As Boolean = False

        Try
            'validate RFD, if it does not exist, remove it

            Dim iRFDNo As Integer = 0
            Dim ds As DataSet

            If txtRFDNo.Text.Trim <> "" Then
                iRFDNo = CType(txtRFDNo.Text.Trim, Integer)

                If iRFDNo > 0 Then
                    ds = RFDModule.GetRFD(iRFDNo)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        bReturn = True
                    End If
                End If

            End If

            If bReturn = False Then
                txtRFDNo.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return bReturn

    End Function

    Private Function CheckCostSheet() As Boolean

        Dim bReturn As Boolean = False

        Try
            Dim iCostSheetID As Integer = 0
            Dim ds As DataSet

            If txtCostSheetID.Text.Trim <> "" Then
                iCostSheetID = CType(txtCostSheetID.Text.Trim, Integer)

                If iCostSheetID > 0 Then
                    ds = CostingModule.GetCostSheet(iCostSheetID)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        bReturn = True
                    End If
                End If

            End If

            If bReturn = False Then
                txtCostSheetID.Text = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return bReturn

    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveDieshop.Click, btnSaveBottom.Click

        Try
            ClearMessages()

            'do not let obsolete items be saved
            Dim bContinue As Boolean = True

            If InStr(ddChangeType.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete Change Type to another."
                bContinue = False
            End If

            If InStr(ddUGNFacility.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete UGN Facility to another."
                bContinue = False
            End If

            If InStr(ddInitiatorTeamMember.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete Initiator to someone else."
                bContinue = False
            End If

            If InStr(ddAccountManager.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete Account Manager to someone else."
                bContinue = False
            End If

            If InStr(ddProgramManager.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete Program Manager to someone else."
                bContinue = False
            End If

            If InStr(ddQualityEngineer.SelectedItem.Text, "**") > 0 Then
                lblMessage.Text &= "<br />Error: Please change the obsolete Quality Engineer to someone else."
                bContinue = False
            End If

            If bContinue = True Then 'if no obsolete items
                'if voiding then
                If ViewState("StatusID") = 4 Then
                    TAModule.DeleteTA(ViewState("TANo"), txtVoidComment.Text.Trim)

                    lblMessage.Text &= "<br />The Tooling Authorization has been voided.<br />"

                    'update history
                    TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Voided")
                Else

                    CheckRFD()
                    CheckCostSheet()

                    SaveToolingAuthorization()

                    'need to check if RFD Changes then do a fresh import
                    If ViewState("OriginalRFDNo") > 0 And txtRFDNo.Text.Trim <> "" Then
                        If ViewState("OriginalRFDNo") <> CType(txtRFDNo.Text.Trim, Integer) Then
                            ImportRFD()
                        End If
                    End If

                    'if no RFD but a cost sheet was assiged, then get RFD of Cost Sheet and then import data
                    'make sure an RFD is assigned to cost sheet
                    If txtCostSheetID.Text.Trim <> "" And txtRFDNo.Text.Trim = "" Then 'ViewState("OriginalCostSheetID") > 0 And
                        Dim iCostSheetID As Integer = CType(txtCostSheetID.Text.Trim, Integer)
                        Dim iRFDNo As Integer = 0

                        If ViewState("OriginalCostSheetID") <> iCostSheetID And iCostSheetID > 0 Then
                            iRFDNo = GetCostSheetRFDNo(iCostSheetID)
                            If iRFDNo > 0 Then
                                txtRFDNo.Text = iRFDNo.ToString
                                ImportRFD()
                            Else
                                lblMessage.Text &= "<br />Error: This cost sheet needs an RFD assigned in order to reference in this module."
                            End If
                        End If
                    End If

                    CheckDieshopFormTask()

                End If
            End If


        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageSaveBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnAddToCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCustomerProgram.Click

        Try

            ClearMessages()

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            If InStr(ddProgram.SelectedItem.Text, "**") > 0 Then 'And ViewState("CurrentCustomerProgramRow") = 0
                lblMessage.Text &= "Error: An obsolete program cannot be selected. The information was NOT saved."
                cddMakes.SelectedValue = ""
                ddYear.SelectedIndex = -1
            Else
                iProgramID = ddProgram.SelectedValue

                'make sure Year Selected is in range of SOP and EOP
                If ddYear.SelectedIndex > 0 Then
                    iProgramYear = ddYear.SelectedValue
                End If

                If iProgramYear > 0 Then
                    TAModule.InsertTACustomerProgram(ViewState("TANo"), iProgramID, iProgramYear)

                    gvCustomerProgram.DataBind()

                    If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
                        lblMessage.Text &= HttpContext.Current.Session("BLLerror")
                    Else
                        HttpContext.Current.Session("BLLerror") = Nothing
                        lblMessage.Text &= "Program and Customer were added or updated."
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveUploadSupportingDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadSupportingDocument.Click

        Try
            ClearMessages()

            If ViewState("TANo") > 0 Then
                If fileUploadSupportingDoc.HasFile Then
                    If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(fileUploadSupportingDoc.PostedFile.FileName)

                        Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)
                        Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType
                        Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                        fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                        If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".msg") Or (FileExt = ".ppt") Or (FileExt = ".pptx") Then

                            ''***************
                            '' Insert Record
                            ''***************
                            TAModule.InsertTASupportingDoc(ViewState("TANo"), fileUploadSupportingDoc.FileName, txtSupportingDocDesc.Text.Trim, SupportingDocBinaryFile, SupportingDocFileSize, SupportingDocEncodeType)

                            revUploadFile.Enabled = False

                            lblMessage.Text &= "<br />File Uploaded Successfully<br />"

                            gvSupportingDoc.DataBind()
                            gvSupportingDoc.Visible = True

                            revUploadFile.Enabled = True

                            txtSupportingDocDesc.Text = ""
                        End If
                    Else
                        lblMessage.Text &= "<br />File exceeds size limit.  Please select a file less than 3MB (3000KB)."

                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text

    End Sub

    Protected Sub menuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub 'EOF menuTabs_MenuItemClick

    Protected Sub gvQuestionAppendReply_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Try

            ClearMessages()

            Dim iBtnAppendReply As ImageButton
            Dim iRSSID As Integer = 0

            iBtnAppendReply = CType(sender, ImageButton)

            If iBtnAppendReply.CommandName.ToString <> "" Then

                iRSSID = CType(iBtnAppendReply.CommandName, Integer)

                If iRSSID > 0 Then

                    ViewState("CurrentRSSID") = iRSSID

                    txtQuestionComment.Text = iBtnAppendReply.AlternateText

                    btnSaveReplyComment.Visible = ViewState("isEnabled")
                    btnResetReplyComment.Visible = ViewState("isEnabled")

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

    Protected Sub gvQuestion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQuestion.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim RSSID As Integer

                Dim drRSSID As ExpProjToolingAuth.TARSSRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjToolingAuth.TARSSRow)

                If DataBinder.Eval(e.Row.DataItem, "RSSID") IsNot DBNull.Value Then
                    RSSID = drRSSID.RSSID
                    ' Reference the rpCBRC ObjectDataSource
                    Dim rpCBRC As ObjectDataSource = CType(e.Row.FindControl("odsReply"), ObjectDataSource)

                    ' Set the Parameter value
                    rpCBRC.SelectParameters("TANo").DefaultValue = drRSSID.TANo.ToString
                    rpCBRC.SelectParameters("RSSID").DefaultValue = drRSSID.RSSID.ToString()
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

    End Sub

    'Protected Sub gvToolingAuthTask_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvToolingAuthTask.DataBound

    '    'hide header of first column
    '    If gvToolingAuthTask.Rows.Count > 0 Then
    '        gvToolingAuthTask.HeaderRow.Cells(0).Visible = False
    '        gvToolingAuthTask.HeaderRow.Cells(1).Visible = False
    '    End If

    'End Sub

    Protected Sub gvToolingAuthTask_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvToolingAuthTask.RowCommand

        Try
            ClearMessages()

            Dim ddInsertTaskTemp As DropDownList
            Dim ddInsertTaskTeamMemberTemp As DropDownList
            Dim txtInsertTaskTargetDateTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                ddInsertTaskTemp = CType(gvToolingAuthTask.FooterRow.FindControl("ddInsertTask"), DropDownList)
                ddInsertTaskTeamMemberTemp = CType(gvToolingAuthTask.FooterRow.FindControl("ddInsertTaskTeamMember"), DropDownList)

                If ddInsertTaskTemp.SelectedIndex > 0 And ddInsertTaskTeamMemberTemp.SelectedIndex > 0 Then
                    If InStr(ddInsertTaskTemp.SelectedItem.Text, "**") <= 0 And InStr(ddInsertTaskTeamMemberTemp.SelectedItem.Text, "**") <= 0 Then
                        txtInsertTaskTargetDateTemp = CType(gvToolingAuthTask.FooterRow.FindControl("txtInsertTaskTargetDate"), TextBox)

                        odsToolingAuthTask.InsertParameters("TANo").DefaultValue = ViewState("TANo")
                        odsToolingAuthTask.InsertParameters("TaskID").DefaultValue = ddInsertTaskTemp.SelectedValue
                        odsToolingAuthTask.InsertParameters("TeamMemberID").DefaultValue = ddInsertTaskTeamMemberTemp.SelectedValue
                        odsToolingAuthTask.InsertParameters("TargetDate").DefaultValue = txtInsertTaskTargetDateTemp.Text

                        intRowsAffected = odsToolingAuthTask.Insert()
                    Else
                        lblMessage.Text &= "Error: Neither task nor the team member can be obsolete. Please select another."
                    End If
                Else
                    lblMessage.Text &= "Error: A task and team member are required to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvToolingAuthTask.ShowFooter = False
                'gvToolingAuthTask.Columns(3).Visible = False
            Else
                gvToolingAuthTask.ShowFooter = True
                'gvToolingAuthTask.Columns(3).Visible = ViewState("isAdmin")
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddInsertTaskTemp = CType(gvToolingAuthTask.FooterRow.FindControl("ddInsertTask"), DropDownList)
                ddInsertTaskTemp.SelectedIndex = -1

                ddInsertTaskTeamMemberTemp = CType(gvToolingAuthTask.FooterRow.FindControl("ddInsertTaskTeamMember"), DropDownList)
                ddInsertTaskTeamMemberTemp.SelectedIndex = -1

                txtInsertTaskTargetDateTemp = CType(gvToolingAuthTask.FooterRow.FindControl("txtInsertTaskTargetDate"), TextBox)
                txtInsertTaskTargetDateTemp.Text = ""

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageTasks.Text = lblMessage.Text
        lblMessageTasksBottom.Text = lblMessage.Text

    End Sub
    Private Property LoadDataEmpty_ToolingAuthTask() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_ToolingAuthTask") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_ToolingAuthTask"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_ToolingAuthTask") = value
        End Set

    End Property
    Protected Sub odsToolingAuthTask_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsToolingAuthTask.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            'Dim dt As Costing.CostSheetMaterial_MaintDataTable = CType(e.ReturnValue, Costing.CostSheetMaterial_MaintDataTable)
            Dim dt As ExpProjToolingAuth.TATaskDataTable = CType(e.ReturnValue, ExpProjToolingAuth.TATaskDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_ToolingAuthTask = True
            Else
                LoadDataEmpty_ToolingAuthTask = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        'lblMessageLowerPage.Text = lblMessage.Text
        'lblMessageHeader.Text = lblMessage.Text

    End Sub
    Protected Sub gvToolingAuthTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvToolingAuthTask.RowCreated

        Try

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_ToolingAuthTask
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

    Protected Sub btnRSSSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSSubmit.Click

        Try
            ClearMessages()

            'save comment
            TAModule.InsertTARSS(ViewState("TANo"), ViewState("TeamMemberID"), ViewState("SubscriptionID"), txtRSSComment.Text.Trim)

            'update history
            TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Message Sent")

            gvQuestion.DataBind()

            'get list of tasked team members to send email TO
            Dim strEmailToAddress As String = GetTaskTeamMemberList(True)
            Dim strEmailCCAddress As String = ""

            'append email addresses of key team members
            If ddAccountManager.SelectedIndex >= 0 Then
                strEmailCCAddress = AppendEmail(CType(ddAccountManager.SelectedValue, Integer), strEmailCCAddress)
            End If

            If ddInitiatorTeamMember.SelectedIndex >= 0 Then
                strEmailCCAddress = AppendEmail(CType(ddInitiatorTeamMember.SelectedValue, Integer), strEmailCCAddress)
            End If

            If ddProgramManager.SelectedIndex >= 0 Then
                strEmailCCAddress = AppendEmail(CType(ddProgramManager.SelectedValue, Integer), strEmailCCAddress)
            End If

            If ddQualityEngineer.SelectedIndex >= 0 Then
                strEmailCCAddress = AppendEmail(CType(ddQualityEngineer.SelectedValue, Integer), strEmailCCAddress)
            End If

            '''''''''''''''''''''''''''''''''''
            ' ''Build Email
            '''''''''''''''''''''''''''''''''''
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
            Dim strTADetail As String = strProdOrTestEnvironment & "EXP/ToolingAuthExpProj.aspx?TANo=" & ViewState("TANo")
            Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

            Dim strEmailSubject As String = "Tooling Authorization Question - TA Project No: " & lblTAProjectNo.Text.Trim & " - MESSAGE RECEIVED"
            Dim strEmailBody As String = ""

            strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"

            strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> sent you message regarding TA Project No.: <font color='red'>" & lblTAProjectNo.Text.Trim & "</font><br />"

            strEmailBody &= "<font size='3' face='Verdana'><p><b>Desc:</b> <font>" & txtTADesc.Text.Trim & "</font>.</p><br />"

            strEmailBody &= "<br />PLEASE DO NOT REPLY TO THIS EMAIL. INSTEAD KEEP THE CONVERSATION IN THE SYSTEM BY USING THE LINK BELOW.<br />"

            If txtRSSComment.Text.Trim <> "" Then
                strEmailBody &= "<p><b>Question: </b><font>" & txtRSSComment.Text.Trim & "</font></p><br /><br />"
            End If

            strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br /><br />"
            strEmailBody &= "<p><a href='" & strTADetail & "&pRC=1" & "'>Click here</a> to answer the message.</font>"
            strEmailBody &= "</td></tr></table>"

            SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnRSSReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRSSReset.Click

        Try
            ClearMessages()

            txtRSSComment.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnResetReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetReplyComment.Click

        Try

            ClearMessages()

            txtReply.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveReplyComment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReplyComment.Click

        Try

            ClearMessages()

            If ViewState("CurrentRSSID") > 0 Then

                'save comment
                TAModule.InsertTARSSReply(ViewState("TANo"), ViewState("CurrentRSSID"), ViewState("TeamMemberID"), txtReply.Text.Trim)

                'update history
                TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Message Sent")

                gvQuestion.DataBind()

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

                'Dim strEmailPreviewURL As String = strProdOrTestEnvironment & "EXP/crViewExpProjToolingAuth.aspx?TANo=" & ViewState("TANo")
                Dim strEmailDetailURL As String = strProdOrTestEnvironment & "EXP/ToolingAuthExpProj.aspx?TANo=" & ViewState("TANo")

                Dim strCurrentUserFullName As String = HttpContext.Current.Request.Cookies("UGNDB_UserFullName").Value

                'get list of tasked team members to send email TO
                Dim strEmailToAddress As String = GetTaskTeamMemberList(True)
                Dim strEmailCCAddress As String = ""

                'append email addresses of key team members
                If ddAccountManager.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddAccountManager.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddInitiatorTeamMember.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddInitiatorTeamMember.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddProgramManager.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddProgramManager.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddQualityEngineer.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddQualityEngineer.SelectedValue, Integer), strEmailCCAddress)
                End If


                '''''''''''''''''''''''''''''''''''
                ' ''Build Email
                '''''''''''''''''''''''''''''''''''

                Dim strEmailSubject As String = "Tooling Authorization Reply - TA Project No: " & lblTAProjectNo.Text.Trim & " - MESSAGE RECEIVED"
                Dim strEmailBody As String = ""

                strEmailBody = "<table><tr><td valign='top' width='20%'><img src='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/images/messanger.jpg'  height='20%' width='20%'/></td><td valign='top'>"
                strEmailBody &= "<font size='3' face='Verdana'><p><b>" & strCurrentUserFullName & "</b> replied to the message regarding regarding Tooling Authorization Project No: <font color='red'>" & lblTAProjectNo.Text.Trim & "</font><br />"

                strEmailBody &= "<font size='3' face='Verdana'><p><b>Desc:</b> <font>" & txtTADesc.Text.Trim & "</font>.</p><br />"

                If txtQuestionComment.Text.Trim <> "" Then
                    strEmailBody &= "<p><b>Question: </b><font>" & txtQuestionComment.Text.Trim & "</font></p><br /><br />"
                End If

                If txtReply.Text.Trim <> "" Then
                    strEmailBody &= "<p><b>Reply: </b><font>" & txtReply.Text.Trim & "</font></p><br /><br />"
                End If

                strEmailBody &= "<font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br /><br />"
                strEmailBody &= "<p><a href='" & strEmailDetailURL & "&pRC=1" & "'>Click here</a> if you need to respond.</font>"
                strEmailBody &= "</td></tr></table>"

                SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody)

                txtQuestionComment.Text = ""
                txtReply.Text = ""

                ViewState("CurrentRSSID") = 0

                btnResetReplyComment.Visible = False
                btnSaveReplyComment.Visible = False

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCommunicationBoard.Text = lblMessage.Text

    End Sub

    Protected Sub btnAddFinishedPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddFinishedPart.Click

        Try

            ClearMessages()

            'nned to pull Customer Part Number based on Internal Part Number
            Dim ds As DataSet
            Dim dsDrawing As DataSet

            Dim strNewCustomerPartNo As String = ""
            Dim strNewCustomerPartName As String = ""

            If ddChangeType.SelectedValue = 2 Then
                TAModule.InsertTAFinishedPart(ViewState("TANo"), "", "", "", "", "", strNewCustomerPartNo, strNewCustomerPartName, txtNewInternalPartNo.Text.Trim, txtNewDesignLevel.Text.Trim, txtNewInternalDrawingNo.Text.Trim)

                gvFinishedPart.DataBind()
            Else
                '(LREY) 01/08/2014
                ' ''ds = commonFunctions.GetCustomerPartPartRelate(txtNewInternalPartNo.Text.Trim, "", "", "", "")
                ' ''If commonFunctions.CheckDataSet(ds) = True Then
                ' ''    strNewCustomerPartNo = ds.Tables(0).Rows(0).Item("CustomerPartNo").ToString
                ' ''    strNewCustomerPartName = ds.Tables(0).Rows(0).Item("CustomerPartName").ToString

                ' ''    If strNewCustomerPartNo <> "" Then
                ' ''        'need to validate Design Levels once there is a standard place of storing design levels

                ' ''        'need to validate drawing number                    
                ' ''        dsDrawing = PEModule.GetDrawing(txtNewInternalDrawingNo.Text.Trim)
                ' ''        If commonFunctions.CheckDataSet(dsDrawing) = True Then
                ' ''            'TAModule.InsertToolingAuthorizationFinishedPart(ViewState("TANo"), ddCurrentCustomerPartNo.SelectedValue, ddCurrentInternalPartNo.SelectedValue, txtCurrentDesignLevel.Text.Trim, ddNewCustomerPartNo.SelectedValue, ddNewInternalPartNo.SelectedValue, txtNewDesignLevel.Text.Trim)
                ' ''            TAModule.InsertTAFinishedPart(ViewState("TANo"), "", "", "", "", "", strNewCustomerPartNo, strNewCustomerPartName, txtNewInternalPartNo.Text.Trim, txtNewDesignLevel.Text.Trim, txtNewInternalDrawingNo.Text.Trim)

                ' ''            gvFinishedPart.DataBind()
                ' ''        Else
                ' ''            lblMessage.Text &= "<br />Error: The DMS Drawing number does not exist. This part cannot be added to the list."
                ' ''        End If

                ' ''    End If
                ' ''Else
                ' ''    lblMessage.Text &= "<br />Error: The part number does not exist and cannot be added to the list."
                ' ''End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageFinishedPart.Text = lblMessage.Text

    End Sub

    Protected Sub btnAddChildPart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddChildPart.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim dsDrawing As DataSet

            ds = commonFunctions.GetPartNo(txtNewPartNo.Text.Trim, "", "", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                dsDrawing = PEModule.GetDrawing(txtNewChildDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(dsDrawing) = True Then

                    TAModule.InsertTAChildPart(ViewState("TANo"), "", "", "", txtNewPartNo.Text.Trim, ds.Tables(0).Rows(0).Item("PartName").ToString, txtNewChildDrawingNo.Text.Trim)

                    gvChildPart.DataBind()
                Else
                    lblMessage.Text &= "Error: The DMS Drawing number does not exist. This part cannot be added to the list."
                End If
            Else
                lblMessage.Text &= "Error: The part number does not exist. This part cannot be added to the list."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageChildPart.Text = lblMessage.Text

    End Sub

    'Protected Sub gvCustomerProgram_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCustomerProgram.DataBound

    '    Try

    '        ''hide header of columns
    '        If gvCustomerProgram.Rows.Count > 0 Then
    '            gvCustomerProgram.HeaderRow.Cells(0).Visible = False
    '            '    gvCustomerProgram.HeaderRow.Cells(1).Visible = False
    '            '    'gvCustomerProgram.HeaderRow.Cells(2).Visible = False
    '            '    'gvCustomerProgram.HeaderRow.Cells(6).Visible = False
    '            '    gvCustomerProgram.HeaderRow.Cells(3).Visible = False
    '            '    gvCustomerProgram.HeaderRow.Cells(7).Visible = False
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub gvCustomerProgram_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomerProgram.RowCreated

        Try
            'hide columns            
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
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

    Protected Sub gvFinishedPart_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFinishedPart.RowCreated

        Try
            'hide columns            
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
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

    Protected Sub gvChildPart_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChildPart.RowCreated

        Try
            'hide columns
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
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
    Protected Function GetTaskTeamMemberList(ByVal isCommunicationBoardMessage As Boolean) As String

        'use the same logic for task notification or communication board

        Dim strEmailToAddress As String = ""

        Try
            Dim dsTeamMember As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim bAlreadyNotified As Boolean = False
            Dim bTeamMemberWorking As Boolean = False

            Dim iRowCounter As Integer = 0
            Dim objTask As New ExpProjToolingAuthBLL
            Dim dtTask As DataTable
            dtTask = objTask.GetTATask(ViewState("TANo"))

            If commonFunctions.CheckDataTable(dtTask) = True Then
                For iRowCounter = 0 To dtTask.Rows.Count - 1
                    iTeamMemberID = 0
                    bAlreadyNotified = False
                    bTeamMemberWorking = False

                    If dtTask.Rows(iRowCounter).Item("TeamMemberID") IsNot System.DBNull.Value Then
                        If dtTask.Rows(iRowCounter).Item("TeamMemberID") > 0 Then
                            iTeamMemberID = dtTask.Rows(iRowCounter).Item("TeamMemberID")
                            If dtTask.Rows(iRowCounter).Item("NotificationDate").ToString <> "" Then
                                bAlreadyNotified = True
                            End If

                            dsTeamMember = SecurityModule.GetTeamMember(iTeamMemberID, "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                                    bTeamMemberWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                                End If

                                'insert only working team members who have not been already added to the list
                                If bTeamMemberWorking = True And dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" And InStr(strEmailToAddress, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString, CompareMethod.Binary) <= 0 Then
                                    'determine if the team member was already notified, 
                                    'then check to see if TA-Initiator clicked the check box to notifiy all again
                                    If cbNotifyAll.Checked = True Then
                                        bAlreadyNotified = False
                                    End If

                                    If bAlreadyNotified = False Or isCommunicationBoardMessage = True Then
                                        If strEmailToAddress <> "" Then
                                            strEmailToAddress &= ";"
                                        End If

                                        strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                                        Try
                                            If isCommunicationBoardMessage = False Then
                                                'update row with notification date                                               
                                                objTask.UpdateTATask(dtTask.Rows(iRowCounter).Item("RowID"), ViewState("TANo"), dtTask.Rows(iRowCounter).Item("TaskID"), iTeamMemberID, Format(Today.Date, "M/d/yyyy"), dtTask.Rows(iRowCounter).Item("TargetDate").ToString, dtTask.Rows(iRowCounter).Item("CompletionDate").ToString)
                                            End If
                                        Catch ex As Exception
                                            lblMessage.Text &= "<br />Error updating notification date."
                                        End Try
                                    End If

                                End If
                            End If
                        End If
                    End If
                Next
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        GetTaskTeamMemberList = strEmailToAddress

    End Function

    Protected Sub SendEmail(ByVal EmailToAddress As String, ByVal EmailCCAddress As String, ByVal EmailSubject As String, ByVal EmailBody As String)

        Try

            Dim strSubject As String = ""
            Dim strBody As String = ""

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()
            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"

            Dim strEmailToAddress As String = commonFunctions.CleanEmailList(EmailToAddress)
            Dim strEmailCCAddress As String = commonFunctions.CleanEmailList(EmailCCAddress)

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!<br /><br />"
                strBody &= "<h1>This information is purely for testing and is NOT valid!!!</h1><br /><br />"
            End If

            strSubject &= EmailSubject

            strBody &= EmailBody

            Dim mail As New MailMessage()

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & EmailToAddress & "<br />"
                strBody &= "<br />Email CC Address List: " & strEmailCCAddress & "<br />"

                EmailToAddress = "Roderick.Carlson@ugnauto.com"
                strEmailCCAddress = ""
            End If

            'set the content
            mail.Subject = strSubject
            mail.Body = strBody

            'set the addresses
            mail.From = New MailAddress(strEmailFromAddress)
            Dim i As Integer

            'build email To list
            Dim emailList As String() = EmailToAddress.Split(";")

            For i = 0 To UBound(emailList)
                If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                    mail.To.Add(emailList(i))
                End If
            Next i

            If InStr(strProdOrTestEnvironment, "Prod_", CompareMethod.Text) > 0 Then
                'build email CC List
                If strEmailCCAddress IsNot Nothing Then
                    emailList = strEmailCCAddress.Split(";")

                    For i = 0 To UBound(emailList)
                        If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                            mail.CC.Add(emailList(i))
                        End If
                    Next i
                End If

                'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")
            End If

            mail.IsBodyHtml = True

            'send the message 
            Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

            Try
                smtp.Send(mail)
                lblMessage.Text &= "Email Notification sent."
            Catch ex As Exception
                lblMessage.Text &= "Email Notification queued."
                UGNErrorTrapping.InsertEmailQueue("Spending Request TA Notification", strEmailFromAddress, EmailToAddress, strEmailCCAddress, strSubject, strBody, "")
            End Try

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Private Function AppendEmail(ByVal TeamMemberID As Integer, ByVal EmailList As String) As String

        Dim strReturn As String = EmailList

        Try

            Dim dsTeamMember As DataSet
            Dim bTeamMemberWorking As Boolean = False

            dsTeamMember = SecurityModule.GetTeamMember(TeamMemberID, "", "", "", "", "", True, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                If dsTeamMember.Tables(0).Rows(0).Item("Working") IsNot System.DBNull.Value Then
                    bTeamMemberWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                End If

                'only include working team members that are not already in the list
                If bTeamMemberWorking = True And dsTeamMember.Tables(0).Rows(0).Item("Email").ToString <> "" And InStr(strReturn, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString, CompareMethod.Binary) <= 0 Then

                    If strReturn <> "" Then
                        strReturn &= ";"
                    End If

                    strReturn &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString

                End If
            End If

        Catch ex As Exception
            strReturn = ""

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return strReturn

    End Function

    'Private Function isTAComplete() As Boolean

    '    Dim bReturn As Boolean = True

    '    Try
    '        'it is complete if all tasks have a completion date

    '        Dim iRowCounter As Integer = 0
    '        Dim objTask As New ExpProjToolingAuthBLL
    '        Dim dtTask As DataTable

    '        'Dim iTaskID As Integer = 0

    '        dtTask = objTask.GetTATask(ViewState("TANo"))

    '        'only check if in-process if there are more tasks than just 1
    '        If commonFunctions.CheckDataTable(dtTask) = True And ViewState("StatusID") = 2 Then

    '            For iRowCounter = 0 To dtTask.Rows.Count - 1
    '                'If dtTask.Rows(iRowCounter).Item("TaskID") IsNot System.DBNull.Value Then
    '                '    iTaskID = dtTask.Rows(iRowCounter).Item("TaskID")
    '                'End If

    '                If dtTask.Rows(iRowCounter).Item("CompletionDate").ToString = "" Then 'And iTaskID <> 10 'Complete Die Shop Cost Form
    '                    bReturn = False
    '                End If
    '            Next
    '        Else
    '            bReturn = False
    '        End If

    '    Catch ex As Exception
    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    Return bReturn

    'End Function

    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

        Try
            ' Build the client script to open a popup window
            ' Pass the ClientID of the 
            ' TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & DrawingControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text &= ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    Protected Function HandleBPCSPopUps(ByVal ccPartNo As String, ByVal ccPartRevision As String, ByVal ccPartName As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the 
            ' TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
               "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & ccPartNo & "&vcPartRevision=" & ccPartRevision & "&vcPartName=" & ccPartName
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleBPCSPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text &= ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleBPCSPopUps = ""
        End Try

    End Function

    Protected Sub InitializeAllPopUps()

        Try

            ''search new Internal F.G. PartNo
            Dim strNewInternalPartNoClientScript As String = HandleBPCSPopUps(txtNewInternalPartNo.ClientID, "", "")
            iBtnNewInternalPartNo.Attributes.Add("onClick", strNewInternalPartNoClientScript)

            ''search new BPCS PartNo
            Dim strNewPartNoClientScript As String = HandleBPCSPopUps(txtNewPartNo.ClientID, "", "")
            iBtnNewPartNo.Attributes.Add("onClick", strNewPartNoClientScript)

            'search new Internal F.G drawingno popup
            Dim strNewInternalDrawingNoSearchClientScript As String = HandleDrawingPopUps(txtNewInternalDrawingNo.ClientID)
            iBtnNewInternalDrawingNoSearch.Attributes.Add("onClick", strNewInternalDrawingNoSearchClientScript)

            'search new child drawingno popup
            Dim strNewChildDrawingNoSearchClientScript As String = HandleDrawingPopUps(txtNewChildDrawingNo.ClientID)
            iBtnNewChildDrawingNoSearch.Attributes.Add("onClick", strNewChildDrawingNoSearchClientScript)

        Catch ex As Exception

            'update error on web page
            lblMessage.Text &= ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    'Private Sub CheckDieshop()

    '    Try
    '        Dim dsDieShop As DataSet

    '        dsDieShop = TAModule.GetTADieShop(ViewState("TANo"))

    '        'create die shop if not already exists
    '        If commonFunctions.CheckDataSet(dsDieShop) = False Then
    '            TAModule.InsertTADieShop(ViewState("TANo"))
    '        End If

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text &= ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    'Private Function CheckPartsAssigned() As Boolean

    '    Try

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text &= ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Function
    Protected Sub btnNotify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNotify.Click

        Try
            ClearMessages()

            'Dim bContinue As Boolean = False
            Dim bContinue As Boolean = True

            'make sure a customer/program has been asigned
            Dim dsCust As DataSet
            dsCust = TAModule.GetTACustomerProgram(ViewState("TANo"))
            'bContinue = commonFunctions.CheckDataSet(dsCust)

            Dim dsFinishedPart As DataSet
            dsFinishedPart = TAModule.GetTAFinishedPart(ViewState("TANo"))

            Dim dsChildPart As DataSet
            dsChildPart = TAModule.GetTAChildPart(ViewState("TANo"))

            'If commonFunctions.CheckDataSet(dsFinishedPart) = False And commonFunctions.CheckDataSet(dsFinishedPart) = False Then
            '    bContinue = False
            'End If

            If commonFunctions.CheckDataSet(dsFinishedPart) = False And commonFunctions.CheckDataSet(dsChildPart) = False Then
                bContinue = False
            End If

            If bContinue = True Then
                'update status
                ddStatus.SelectedValue = 2
                ViewState("StatusID") = 2

                SaveToolingAuthorization()

                'CheckPartsAssigned()

                CheckDieshopFormTask()

                'get list of tasked team members to send email TO
                Dim strEmailToAddress As String = GetTaskTeamMemberList(False)

                gvToolingAuthTask.DataBind()

                Dim strEmailCCAddress As String = ""

                'append email addresses of key team members
                If ddAccountManager.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddAccountManager.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddInitiatorTeamMember.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddInitiatorTeamMember.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddProgramManager.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddProgramManager.SelectedValue, Integer), strEmailCCAddress)
                End If

                If ddQualityEngineer.SelectedIndex >= 0 Then
                    strEmailCCAddress = AppendEmail(CType(ddQualityEngineer.SelectedValue, Integer), strEmailCCAddress)
                End If

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                Dim strTADetail As String = strProdOrTestEnvironment & "EXP/ToolingAuthExpProj.aspx?TANo=" & ViewState("TANo")
                'Dim strDieshopDetail As String = strProdOrTestEnvironment & "EXP/ToolingAuthDieshopExpProj.aspx?TANo=" & ViewState("TANo")
                'Dim strPreviewTA As String = strProdOrTestEnvironment & "EXP/crViewExpProjToolingAuth.aspx?FormType=TA&ArchiveData=0&TAProjectNo=" & lblTAProjectNo.Text.Trim
                'Dim strPreviewDieshop As String = strProdOrTestEnvironment & "EXP/crViewExpProjToolingAuth.aspx?FormType=DS&ArchiveData=0&TAProjectNo=" & lblTAProjectNo.Text.Trim
                Dim strPreviewRFD As String = strProdOrTestEnvironment & "RFD/crRFD_Preview.aspx?RFDNo=" & txtRFDNo.Text.Trim
                Dim strPreviewCostSheet As String = strProdOrTestEnvironment & "Costing/Cost_Sheet_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim
                Dim strPreviewDieLayout As String = strProdOrTestEnvironment & "Costing/Die_Layout_Preview.aspx?CostSheetID=" & txtCostSheetID.Text.Trim

                Dim strEmailSubject As String = "Spending Request Tooling Authorization: " & lblTAProjectNo.Text.Trim
                Dim strEmailBody As String = ""

                strEmailBody &= "<p><font size='2' face='Tahoma'>Spending Request - Tooling Authorization for Dieshop is available for Review/Task Completion. <a href='" & strTADetail & "'>Click here</a> to access the record.</font></p>"

                'strEmailBody &= "<p><font size='2' face='Tahoma'>NOTE: <a href='" & strDieshopDetail & "'>Click here</a> to update the Dieshop Cost Form.</font></p>"

                strEmailBody &= "<table style='border: 1px solid #D0D0BF; width: 900px; font-size: 13; font-family: Tahoma;'>"
                strEmailBody &= "<tr bgcolor='#EBEBEB'><td colspan='4'><strong>PROJECT OVERVIEW</strong></td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right' style='width: 50px;'>Project No:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td style='width: 100px;'>" & lblTAProjectNo.Text.Trim & "</td>"
                strEmailBody &= "<td style='width: 100px;'>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>Project Description:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & txtTADesc.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>Type of Change:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & ddChangeType.SelectedItem.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>Issued By:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & ddInitiatorTeamMember.SelectedItem.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>UGN Location:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & ddUGNFacility.SelectedItem.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>Due Date:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & txtDueDate.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                strEmailBody &= "<tr>"
                strEmailBody &= "<td class='p_text' align='right'>Implementation Date:&nbsp;&nbsp; </td>"
                strEmailBody &= "<td>" & txtImplementationDate.Text & "</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "<td>&nbsp;</td>"
                strEmailBody &= "</tr>"

                If txtRFDNo.Text.Trim <> "" And hlnkRFD.Visible = True Then
                    strEmailBody &= "<tr>"
                    strEmailBody &= "<td class='p_text' align='right'>RFDNo:&nbsp;&nbsp; </td>"
                    strEmailBody &= "<td><a href='" & strPreviewRFD & "'><b><u>" & txtRFDNo.Text.Trim & " </u></b></a></td>"
                    strEmailBody &= "</tr>"
                End If

                If txtCostSheetID.Text.Trim <> "" And hlnkCostSheet.Visible = True Then
                    strEmailBody &= "<tr>"
                    strEmailBody &= "<td class='p_text' align='right'>CostSheet ID:&nbsp;&nbsp; </td>"
                    strEmailBody &= "<td><a href='" & strPreviewCostSheet & "'><b><u>" & txtCostSheetID.Text.Trim & " </u></b></a></td>"

                    If hlnkDieLayout.Visible = True Then
                        strEmailBody &= "<td class='p_text' align='right'>Die Layout:&nbsp;&nbsp; </td>"
                        strEmailBody &= "<td><a href='" & strPreviewDieLayout & "'><b><u> Die Layout </u></b></a></td>"
                    Else
                        strEmailBody &= "<td>&nbsp;</td>"
                        strEmailBody &= "<td>&nbsp;</td>"
                    End If

                    strEmailBody &= "</tr>"
                End If

                dsCust = TAModule.GetTACustomerProgram(ViewState("TANo"))
                If commonFunctions.CheckDataSet(dsCust) = True Then
                    ''***************************************************
                    ''Get list of Customer/Program information for display
                    ''***************************************************
                    strEmailBody &= "<tr><td colspan='4'>"
                    strEmailBody &= "<table style='font-size: 11; font-family: Tahoma;'  width='100%'>"
                    strEmailBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                    strEmailBody &= "<td><strong>Customer</strong></td>"
                    strEmailBody &= "<td><strong>Make</strong></td>"
                    strEmailBody &= "<td><strong>Program / Platform / Assembly Plant</strong></td>"
                    strEmailBody &= "<td><strong>Year </strong></td>"
                    strEmailBody &= "</tr>"

                    For i = 0 To dsCust.Tables.Item(0).Rows.Count - 1
                        strEmailBody &= "<tr style='border-color:white'>"
                        strEmailBody &= "<td height='25'>" & dsCust.Tables(0).Rows(i).Item("ddCustomerDesc") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsCust.Tables(0).Rows(i).Item("Make") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsCust.Tables(0).Rows(i).Item("ddProgramName") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsCust.Tables(0).Rows(i).Item("ProgramYear") & "&nbsp;</td>"
                        strEmailBody &= "</tr>"
                    Next

                    strEmailBody &= "</table>"
                    strEmailBody &= "</td></tr>"
                End If

                ''***************************************************
                ''Get list of Finished Part information for display
                ''***************************************************           
                dsFinishedPart = TAModule.GetTAFinishedPart(ViewState("TANo"))
                If commonFunctions.CheckDataSet(dsFinishedPart) = True Then
                    strEmailBody &= "<tr><td colspan='4'>"
                    strEmailBody &= "<table style='font-size: 11; font-family: Tahoma;'  width='100%'>"
                    strEmailBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                    strEmailBody &= "<td><strong>Current Customer Part Number</strong></td>"
                    strEmailBody &= "<td><strong>New Customer Part Number</strong></td>"
                    strEmailBody &= "<td><strong>Internal Part Number</strong></td>"
                    strEmailBody &= "<td><strong>Design Level</strong></td>"
                    strEmailBody &= "<td><strong>Drawing No</strong></td>"
                    strEmailBody &= "<td><strong>Name</strong></td>"
                    strEmailBody &= "</tr>"

                    For i = 0 To dsFinishedPart.Tables.Item(0).Rows.Count - 1
                        strEmailBody &= "<tr style='border-color:white'>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("CurrentCustomerPartNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("NewCustomerPartNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("NewInternalPartNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("NewDesignLevel") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("NewDrawingNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsFinishedPart.Tables(0).Rows(i).Item("NewCustomerPartName") & "&nbsp;</td>"
                        strEmailBody &= "</tr>"
                    Next

                    strEmailBody &= "</table>"
                    strEmailBody &= "</td></tr>"
                End If

                ''***************************************************
                ''Get list of Finished Part information for display
                ''***************************************************            
                dsChildPart = TAModule.GetTAChildPart(ViewState("TANo"))
                If commonFunctions.CheckDataSet(dsChildPart) = True Then
                    strEmailBody &= "<tr><td colspan='4'>"
                    strEmailBody &= "<table style='font-size: 11; font-family: Tahoma;'  width='100%'>"
                    strEmailBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                    strEmailBody &= "<td><strong>Current BPCS Part Number</strong></td>"
                    strEmailBody &= "<td><strong>Revision</strong></td>"
                    strEmailBody &= "<td><strong>Drawing No</strong></td>"
                    strEmailBody &= "<td><strong>Name</strong></td>"
                    strEmailBody &= "<td><strong>New BPCS Part Number</strong></td>"
                    strEmailBody &= "<td><strong>Revision</strong></td>"
                    strEmailBody &= "<td><strong>Drawing No</strong></td>"
                    strEmailBody &= "<td><strong>Name</strong></td>"
                    strEmailBody &= "</tr>"

                    For i = 0 To dsChildPart.Tables.Item(0).Rows.Count - 1
                        strEmailBody &= "<tr style='border-color:white'>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("CurrentPartNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("CurrentPartRevision") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("CurrentDrawingNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("CurrentPartName") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("NewPartNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("NewPartRevision") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("NewDrawingNo") & "&nbsp;</td>"
                        strEmailBody &= "<td height='25'>" & dsChildPart.Tables(0).Rows(i).Item("NewPartName") & "&nbsp;</td>"
                        strEmailBody &= "</tr>"
                    Next

                    strEmailBody &= "</table>"
                    strEmailBody &= "</td></tr>"
                End If

                ''***************************************************
                ''Get list of Supporting Documentation
                ''***************************************************
                Dim dsDoc As DataSet
                dsDoc = TAModule.GetTASupportingDocList(ViewState("TANo"))
                If commonFunctions.CheckDataSet(dsDoc) = True Then
                    strEmailBody &= "<tr><td colspan='4'>"
                    strEmailBody &= "<table style='font-size: 11; font-family: Tahoma;' width='100%'>"
                    strEmailBody &= "<tr bgcolor='#EBEBEB' style='border-color:#EBEBEB'>"
                    strEmailBody &= "<td colspan='2'><strong>SUPPORTING DOCUMENT(S):</strong></td>"
                    strEmailBody &= "</tr>"
                    strEmailBody &= "<tr>"
                    strEmailBody &= "<td colspan='2'>"
                    strEmailBody &= "<table border='0' style='font-size: 13; font-family: Tahoma;'>"
                    strEmailBody &= "  <tr>"
                    strEmailBody &= "   <td width='250px'><b>File Description</b></td>"
                    strEmailBody &= "   <td width='250px'>&nbsp;</td>"
                    strEmailBody &= "</tr>"

                    For i = 0 To dsDoc.Tables.Item(0).Rows.Count - 1
                        strEmailBody &= "<tr>"
                        strEmailBody &= "<td height='25'>" & dsDoc.Tables(0).Rows(i).Item("SupportingDocDesc") & "</td>"
                        strEmailBody &= "<td height='25'><a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString() & "/EXP/ToolingAuthExpProjDocument.aspx?RowID=" & dsDoc.Tables(0).Rows(i).Item("RowID") & "' target='_blank'>" & dsDoc.Tables(0).Rows(i).Item("SupportingDocName") & "</a></td>"
                        strEmailBody &= "</tr>"
                    Next
                    strEmailBody &= "</table>"
                    strEmailBody &= "</td></tr>"
                End If

                strEmailBody &= "</table>"

                SendEmail(strEmailToAddress, strEmailCCAddress, strEmailSubject, strEmailBody)
            Else
                lblMessage.Text &= "Error: The tooling authorization could not be sent. Please check to make sure all required fields have been assigned (Part or Program for example)."
            End If 'all required fields exist

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        'lblMessageTasks.Text = lblMessage.Text
        lblMessageTasksBottom.Text = lblMessage.Text
    End Sub

    'Protected Sub btnDieShop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDieshop.Click, btnDieshopBottom.Click

    '    Try
    '        Response.Redirect("ToolingAuthDieshopExpProj.aspx?TANo=" & ViewState("TANo"), False)
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub cbCharge_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCharge.CheckedChanged

        Try
            ClearMessages()

            txtChargeOther.Enabled = Not cbCharge.Checked
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Private Function isDieshopComplete() As Boolean

    '    Dim bReturn As Boolean = False

    '    Try
    '        Dim ds As DataSet

    '        'Get Dieshop Cost Form to see if complete
    '        ds = TAModule.GetTADieShop(ViewState("TANo"))
    '        If commonFunctions.CheckDataSet(ds) = True Then
    '            If ds.Tables(0).Rows(0).Item("isComplete") IsNot System.DBNull.Value Then
    '                bReturn = ds.Tables(0).Rows(0).Item("isComplete")
    '            End If
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    '    Return bReturn

    'End Function

    Private Sub CheckComplete()

        Try
            If ViewState("TANo") > 0 And ViewState("StatusID") = 2 Then
                'if all tasks are complete, then set to Complete
                If TAModule.isTAComplete(ViewState("TANo")) = True And cbDieshopComplete.Checked = True Then
                    ViewState("StatusID") = 3
                    ddStatus.SelectedValue = 3
                    SaveToolingAuthorization()

                    'only notify team members in a daily email of all closed TAs

                Else 'reset to in-process if needed
                    If ViewState("StatusID") <> 3 Then
                        ViewState("StatusID") = 2
                        ddStatus.SelectedValue = 2
                        SaveToolingAuthorization()
                    End If
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

    Protected Sub gvToolingAuthTask_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvToolingAuthTask.RowDeleted

        Try
            ClearMessages()

            CheckComplete()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub gvToolingAuthTask_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles gvToolingAuthTask.RowUpdated

        Try
            ClearMessages()

            CheckComplete()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

    '    Try

    '        ClearMessages()

    '        ViewState("StatusID") = 4

    '        DisableControls()

    '        lblVoidComment.Visible = True
    '        lblVoidCommentMarker.Visible = True
    '        txtVoidComment.Visible = True
    '        txtVoidComment.Enabled = True

    '        btnVoid.Attributes.Add("onclick", "")

    '        btnCopy.Visible = False

    '        btnPreviewTA.Visible = False
    '        btnPreviewTABottom.Visible = btnPreviewTA.Visible = False

    '        btnPreviewDieshop.Visible = btnPreviewTA.Visible
    '        btnPreviewDieshopBottom.Visible = btnPreviewTA.Visible
    '        btnDieshop.Visible = btnPreviewTA.Visible

    '        btnVoid.Visible = True

    '        If txtVoidComment.Text.Trim <> "" Then

    '            ddStatus.SelectedValue = 4

    '            TAModule.DeleteTA(ViewState("TANo"), txtVoidComment.Text.Trim)

    '            lblMessage.Text = "The Tooling Authorization has been voided.<br />"

    '            btnVoid.Visible = False
    '            btnVoidCancel.Visible = False

    '            'update history
    '            TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Voided")

    '        Else
    '            lblMessage.Text &= "To void, please fill in the reason in the Void Comment field and then CLICK THE VOID BUTTON AGAIN."
    '            txtVoidComment.Focus()
    '            btnVoidCancel.Visible = ViewState("isEdit")
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub btnVoidCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoidCancel.Click

    '    Try
    '        ClearMessages()

    '        Response.Redirect("ToolingAuthExpProj.aspx?TANo=" & ViewState("TANo"), False)

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try

    'End Sub

    Protected Sub ddStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddStatus.SelectedIndexChanged

        Try
            ClearMessages()

            Dim iStatus As Integer = 1

            If ddStatus.SelectedIndex >= 0 Then
                iStatus = CType(ddStatus.SelectedValue, Integer)
            End If

            'if already in-process then do NOT go back to OPEN
            If ViewState("StatusID") = 2 And iStatus = 1 Then
                ddStatus.SelectedValue = 2
                iStatus = 2
                lblMessage.Text &= "<br />You can NOT roll the status back to OPEN once submitted."
            End If

            'if closing then warn the user that pending activities will be removed from the task list
            If ViewState("StatusID") <> 3 And iStatus = 3 Then             
               
                ''if dieshop complete
                'If isDieshopComplete() = True Then
                '    lblMessage.Text &= "<br />If you manually close this then all incomplete tasks by team members will be removed and the TA will be closed."
                'Else
                '    'cannot close until dieshop is closed
                '    ViewState("StatusID") = 2
                '    ddStatus.SelectedValue = 2
                '    lblMessage.Text &= "<br />The Tooling Authorization cannot be set to complete until the dieshop cost form is complete."
                'End If

            End If

            'if voiding
            If iStatus = 4 Then
                ViewState("StatusID") = 4

                DisableControls()

                lblVoidComment.Visible = True
                lblVoidCommentMarker.Visible = True
                txtVoidComment.Visible = True
                txtVoidComment.Enabled = True

                btnCopyBottom.Visible = False

                btnPreviewTA.Visible = False
                btnPreviewTABottom.Visible = btnPreviewTA.Visible

                btnPreviewDieshop.Visible = btnPreviewTA.Visible
                btnPreviewDieshopBottom.Visible = btnPreviewTA.Visible
                'btnDieshop.Visible = btnPreviewTA.Visible

                btnSave.Visible = ViewState("isAdmin")

                lblMessage.Text &= "<br />To void, please fill in the reason in the Void Comment field and then CLICK THE SAVE BUTTON."
                txtVoidComment.Focus()

            Else
                EnableControls()
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text &= ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Private Property LoadDataEmpty_TADSLabor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_TADSLabor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_TADSLabor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_TADSLabor") = value
        End Set

    End Property

    Protected Sub odsTADSLabor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTADSLabor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As ExpProjToolingAuth.TADieShopLaborDataTable = CType(e.ReturnValue, ExpProjToolingAuth.TADieShopLaborDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_TADSLabor = True
            Else
                LoadDataEmpty_TADSLabor = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        'lblMessageLowerPage.Text = lblMessage.Text
        'lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLabor.RowCreated

        Try
            ''hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_TADSLabor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        'lblMessageLowerPage.Text = lblMessage.Text
        'lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLabor.RowCommand

        Try
            ClearMessages()

            Dim ddInsertLaborTemp As DropDownList
            Dim txtInsertLaborNumHrsTemp As TextBox
            Dim txtInsertLaborNotesTemp As TextBox
            Dim txtInsertLaborCostTemp As TextBox
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                ddInsertLaborTemp = CType(gvLabor.FooterRow.FindControl("ddInsertLabor"), DropDownList)

                If ddInsertLaborTemp.SelectedIndex > 0 Then

                    txtInsertLaborNumHrsTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborNumHrs"), TextBox)
                    txtInsertLaborNotesTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborNotes"), TextBox)
                    txtInsertLaborCostTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborCost"), TextBox)

                    odsTADSMaterial.InsertParameters("TANo").DefaultValue = ViewState("TANo")
                    odsTADSLabor.InsertParameters("DSLaborID").DefaultValue = ddInsertLaborTemp.SelectedValue
                    odsTADSLabor.InsertParameters("NumberHours").DefaultValue = txtInsertLaborNumHrsTemp.Text
                    odsTADSLabor.InsertParameters("Notes").DefaultValue = txtInsertLaborNotesTemp.Text
                    odsTADSLabor.InsertParameters("Cost").DefaultValue = txtInsertLaborCostTemp.Text

                    intRowsAffected = odsTADSLabor.Insert()

                Else
                    lblMessage.Text &= "Error: A Labor is required to insert."
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvLabor.ShowFooter = False
            Else
                gvLabor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddInsertLaborTemp = CType(gvLabor.FooterRow.FindControl("ddInsertLabor"), DropDownList)
                ddInsertLaborTemp.SelectedIndex = -1

                txtInsertLaborNumHrsTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborNumHrs"), TextBox)
                txtInsertLaborNumHrsTemp.Text = ""

                txtInsertLaborNotesTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborNotes"), TextBox)
                txtInsertLaborNotesTemp.Text = ""

                txtInsertLaborCostTemp = CType(gvLabor.FooterRow.FindControl("txtInsertLaborCost"), TextBox)
                txtInsertLaborCostTemp.Text = ""

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageLabor.Text = lblMessage.Text

    End Sub

    Protected Sub gvLabor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLabor.RowDataBound

        Try

            Dim dNumberHours As Double = 0
            Dim dLaborCost As Double = 0

            If e.Row.RowType = DataControlRowType.DataRow Then
                ''Calculate Footer Totals
                Dim drNumberHours As ExpProjToolingAuth.TADieShopLaborRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjToolingAuth.TADieShopLaborRow)

                ''Tables.Item(0).Rows.Count > 0) Then
                If DataBinder.Eval(e.Row.DataItem, "NumberHours") IsNot DBNull.Value Then
                    dNumberHours = drNumberHours.NumberHours
                    _totalNumberHours += dNumberHours
                End If

                If DataBinder.Eval(e.Row.DataItem, "Cost") IsNot DBNull.Value Then
                    dLaborCost = DataBinder.Eval(e.Row.DataItem, "Cost") * dNumberHours
                    _totalLabor += dLaborCost
                    'e.Row.Cells(gvLabor.Columns.Count - 1).Text = String.Format("{0:#,##0.00}", (dLaborCost.ToString))
                    e.Row.Cells(gvLabor.Columns.Count - 1).Text = Format(dLaborCost, "#,##0.00")
                End If

            ElseIf e.Row.RowType = DataControlRowType.Footer Then
                '' ''Display Totals at footer             
                e.Row.Cells(gvLabor.Columns.Count - 4).ForeColor = Color.Red
                e.Row.Cells(gvLabor.Columns.Count - 3).ForeColor = Color.Black

                e.Row.Cells(gvLabor.Columns.Count - 2).ForeColor = Color.Red
                e.Row.Cells(gvLabor.Columns.Count - 1).ForeColor = Color.Black
                'e.Row.Cells(3).Font.Size = 10

                e.Row.Cells(gvLabor.Columns.Count - 4).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(gvLabor.Columns.Count - 3).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(gvLabor.Columns.Count - 2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(gvLabor.Columns.Count - 1).HorizontalAlign = HorizontalAlign.Right

                e.Row.Cells(gvLabor.Columns.Count - 4).Text = "Total Number Hours: "
                'e.Row.Cells(gvLabor.Columns.Count - 3).Text = String.Format("{0:#,##0.00}", (_totalNumberHours.ToString))
                e.Row.Cells(gvLabor.Columns.Count - 3).Text = Format(_totalNumberHours, "#,##0.00")

                e.Row.Cells(gvLabor.Columns.Count - 2).Text = "Total Labor: "
                'e.Row.Cells(gvLabor.Columns.Count - 1).Text = String.Format("{0:#,##0.00}", (_totalLabor.ToString))
                e.Row.Cells(gvLabor.Columns.Count - 1).Text = Format(_totalLabor, "#,##0.00")

                ViewState("TotalLabor") = _totalLabor
                RecalculateTotal()
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Property LoadDataEmpty_TADSMaterial() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_TADSMaterial") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_TADSMaterial"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_TADSMaterial") = value
        End Set

    End Property

    Protected Sub odsTADSMaterial_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsTADSMaterial.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)


            Dim dt As ExpProjToolingAuth.TADieShopMaterialDataTable = CType(e.ReturnValue, ExpProjToolingAuth.TADieShopMaterialDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_TADSMaterial = True
            Else
                LoadDataEmpty_TADSMaterial = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        'lblMessageLowerPage.Text = lblMessage.Text
        'lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvMaterial_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMaterial.RowCreated

        Try
            ''hide first column
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            'End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            '    e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            'End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_TADSMaterial
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        'lblMessageLowerPage.Text = lblMessage.Text
        'lblMessageHeader.Text = lblMessage.Text

    End Sub

    Protected Sub gvMaterial_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMaterial.RowCommand

        Try

            ClearMessages()

            Dim ddInsertMatTemp As DropDownList
            Dim txtInsertMatNotesTemp As TextBox
            Dim txtInsertMatQtyTemp As TextBox
            Dim txtInsertMatCostTemp As TextBox
            Dim ddInsertMatUnitTemp As DropDownList
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                ddInsertMatTemp = CType(gvMaterial.FooterRow.FindControl("ddInsertMat"), DropDownList)

                If ddInsertMatTemp.SelectedIndex > 0 Then

                    txtInsertMatNotesTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatNotes"), TextBox)
                    txtInsertMatQtyTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatQty"), TextBox)
                    txtInsertMatCostTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatCost"), TextBox)

                    ddInsertMatUnitTemp = CType(gvMaterial.FooterRow.FindControl("ddInsertMatUnit"), DropDownList)

                    odsTADSMaterial.InsertParameters("TANo").DefaultValue = ViewState("TANo")
                    odsTADSMaterial.InsertParameters("DSMaterialID").DefaultValue = ddInsertMatTemp.SelectedValue
                    odsTADSMaterial.InsertParameters("Notes").DefaultValue = txtInsertMatNotesTemp.Text
                    odsTADSMaterial.InsertParameters("Quantity").DefaultValue = txtInsertMatQtyTemp.Text
                    odsTADSMaterial.InsertParameters("Cost").DefaultValue = txtInsertMatCostTemp.Text
                    odsTADSMaterial.InsertParameters("UnitID").DefaultValue = IIf(ddInsertMatUnitTemp.SelectedValue = "", 0, ddInsertMatUnitTemp.SelectedValue)

                    intRowsAffected = odsTADSMaterial.Insert()

                Else
                    lblMessage.Text &= "Error: A Material is required to insert."
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvMaterial.ShowFooter = False
            Else
                gvMaterial.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then

                ddInsertMatTemp = CType(gvMaterial.FooterRow.FindControl("ddInsertMat"), DropDownList)
                ddInsertMatTemp.SelectedIndex = -1

                txtInsertMatNotesTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatNotes"), TextBox)
                txtInsertMatNotesTemp.Text = ""

                txtInsertMatQtyTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatQty"), TextBox)
                txtInsertMatQtyTemp.Text = ""

                txtInsertMatCostTemp = CType(gvMaterial.FooterRow.FindControl("txtInsertMatCost"), TextBox)
                txtInsertMatCostTemp.Text = ""

                ddInsertMatUnitTemp = CType(gvMaterial.FooterRow.FindControl("ddInsertMatUnit"), DropDownList)
                ddInsertMatUnitTemp.SelectedIndex = -1
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageMaterial.Text = lblMessage.Text

    End Sub

    Protected Sub gvMaterial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMaterial.RowDataBound

        Try

            Dim dQuantity As Double = 0
            Dim dMaterialsCost As Double = 0

            If e.Row.RowType = DataControlRowType.DataRow Then
                ''Calculate Footer Totals
                Dim drQuantity As ExpProjToolingAuth.TADieShopMaterialRow = CType(CType(e.Row.DataItem, DataRowView).Row, ExpProjToolingAuth.TADieShopMaterialRow)

                ''Tables.Item(0).Rows.Count > 0) Then
                If DataBinder.Eval(e.Row.DataItem, "Quantity") IsNot DBNull.Value Then
                    dQuantity = drQuantity.Quantity
                End If

                If DataBinder.Eval(e.Row.DataItem, "Cost") IsNot DBNull.Value Then
                    dMaterialsCost = DataBinder.Eval(e.Row.DataItem, "Cost") * dQuantity
                    _totalMaterial += dMaterialsCost
                    'e.Row.Cells(gvMaterial.Columns.Count - 1).Text = String.Format("{0:#,###.##}", (dMaterialsCost.ToString))
                    e.Row.Cells(gvMaterial.Columns.Count - 1).Text = Format(dMaterialsCost, "#,##0.00")
                End If
            ElseIf e.Row.RowType = DataControlRowType.Footer Then

                '' ''Display Totals at footer                           
                e.Row.Cells(gvMaterial.Columns.Count - 2).ForeColor = Color.Red
                e.Row.Cells(gvMaterial.Columns.Count - 1).ForeColor = Color.Black

                e.Row.Cells(gvMaterial.Columns.Count - 2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(gvMaterial.Columns.Count - 1).HorizontalAlign = HorizontalAlign.Right
                'e.Row.Cells(3).Font.Size = 10

                e.Row.Cells(gvMaterial.Columns.Count - 2).Text = "Total Material: "
                'e.Row.Cells(gvMaterial.Columns.Count - 1).Text = String.Format("{0:#,###.##}", (_totalMaterial.ToString))
                e.Row.Cells(gvMaterial.Columns.Count - 1).Text = Format(_totalMaterial, "#,##0.00")

                ViewState("TotalMaterial") = _totalMaterial
                RecalculateTotal()
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text = ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub RecalculateTotal()

        Try
            lblTotalDieShop.Text = Format(ViewState("TotalMaterial") + ViewState("TotalLabor"), "$#,##0.00")
            'lblTotalDieShop.Text = String.Format(ViewState("TotalMaterial") + ViewState("TotalLabor"), "{$#,##0.00;($#,##0.00}")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub UpdateDieshopFormTaskCompletion()

        Try

            Dim objTask As New ExpProjToolingAuthBLL
            Dim dtTask As DataTable

            Dim iRowCounter As Integer
            Dim strNotificationDate As String = ""

            dtTask = objTask.GetTATask(ViewState("TANo"))

            'search the list of tasks for dieshop cost form
            If commonFunctions.CheckDataTable(dtTask) = True Then
                For iRowCounter = 0 To dtTask.Rows.Count - 1
                    If dtTask.Rows(iRowCounter).Item("TaskID") IsNot System.DBNull.Value Then
                        If dtTask.Rows(iRowCounter).Item("TaskID") = 10 Then
                            'if found then set task as complete
                            If dtTask.Rows(iRowCounter).Item("NotificationDate").ToString = "" Then
                                strNotificationDate = Format(Today.Date, "M/d/yyyy")
                            Else
                                strNotificationDate = dtTask.Rows(iRowCounter).Item("NotificationDate").ToString()
                            End If

                            objTask.UpdateTATask(dtTask.Rows(iRowCounter).Item("RowID"), ViewState("TANo"), 10, ViewState("TeamMemberID"), strNotificationDate, dtTask.Rows(iRowCounter).Item("TargetDate").ToString, Format(Today.Date, "M/d/yyyy"))
                        End If
                    End If
                Next
            End If

            ''if TA has all completed tasks then close it
            'If TAModule.isTAComplete(ViewState("TANo")) = True And ViewState("StatusID") = 2 Then
            '    ViewState("StatusID") = 3               
            '    TAModule.UpdateTA(ViewState("TANo"), ViewState("StatusID"), ViewState("ChangeTypeID"), ViewState("DueDate"), ViewState("ImplementationDate"), ViewState("AccountManagerID"), ViewState("InitiatorTeamMemberID"), ViewState("ProgramManagerID"), ViewState("QualityEngineerID"), ViewState("RFDNo"), ViewState("CostSheetID"), ViewState("TADesc"), ViewState("ChargeOther"), ViewState("UGNFacility"))

            '    'update history
            '    TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Tooling Authorization Completed")
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnDieshopComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDieshopComplete.Click

        Try
            cbDieshopComplete.Checked = True

            SaveToolingAuthorization()

            'set completion date on other screen for this task
            UpdateDieshopFormTaskCompletion()

            'update history
            TAModule.InsertTAHistory(ViewState("TANo"), ViewState("TeamMemberID"), "Dieshop Cost Form Complete")
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnCopyBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyBottom.Click

        Try
            ClearMessages()

            Session("RecordCopied") = Nothing

            Dim ds As DataSet
            Dim iPreviousTANo As Integer = ViewState("TANo")
            Dim iNewTANo As Integer = 0

            Dim iAccountManagerID As Integer = 0
            If ddAccountManager.SelectedIndex > 0 Then
                If InStr(ddAccountManager.SelectedItem.Text, "**") <= 0 Then
                    iAccountManagerID = ddAccountManager.SelectedValue
                End If
            End If

            Dim iProgramManagerID As Integer = 0
            If ddProgramManager.SelectedIndex > 0 Then
                If InStr(ddProgramManager.SelectedItem.Text, "**") <= 0 Then
                    iProgramManagerID = ddProgramManager.SelectedValue
                End If
            End If

            'if the curret user is the initiator and is a program manager and the program manager has not been assigned then assign it
            If iProgramManagerID = 0 And ViewState("isProgramManagement") = True Then
                iProgramManagerID = ViewState("TeamMemberID")
            End If

            Dim iQualityEngineerID As Integer = 0
            If ddQualityEngineer.SelectedIndex > 0 Then
                If InStr(ddQualityEngineer.SelectedItem.Text, "**") <= 0 Then
                    iQualityEngineerID = ddQualityEngineer.SelectedValue
                End If
            End If

            'if the curret user is the initiator and is a quality engineer and the quality engineer has not been assigned then assign it
            If iQualityEngineerID = 0 And ViewState("isQualityEngineer") = True Then
                iQualityEngineerID = ViewState("TeamMemberID")
            End If

            ds = TAModule.InsertTA(1, ddChangeType.SelectedValue, _
                txtDueDate.Text.Trim, Format(Today.Date, "M/d/yyyy"), txtImplementationDate.Text.Trim, iAccountManagerID, _
                ViewState("TeamMemberID"), iProgramManagerID, iQualityEngineerID, 0, 0, _
                txtTADesc.Text.Trim, txtChargeOther.Text.Trim, ddUGNFacility.SelectedValue, _
                txtInstructions.Text.Trim, txtRules.Text.Trim, txtSerialNo.Text.Trim)

            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("NewTANo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("NewTANo") > 0 Then
                        iNewTANo = ds.Tables(0).Rows(0).Item("NewTANo")

                        TAModule.CopyTAChildPart(iNewTANo, iPreviousTANo)
                        TAModule.CopyTACustomerProgram(iNewTANo, iPreviousTANo)
                        TAModule.CopyTAFinishedPart(iNewTANo, iPreviousTANo)
                        TAModule.CopyTATask(iNewTANo, iPreviousTANo)

                        'update history
                        TAModule.InsertTAHistory(iNewTANo, ViewState("TeamMemberID"), "Tooling Authorization was created by being copied from U-" & iPreviousTANo.ToString)

                        'redirect to new TANo
                        Response.Redirect("ToolingAuthExpProj.aspx?TANo=" & iNewTANo.ToString, False)

                        Session("CopyTA") = "Copied"

                    End If
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
End Class
