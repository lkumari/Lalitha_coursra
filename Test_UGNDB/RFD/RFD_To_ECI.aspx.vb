' ************************************************************************************************
'
' Name:		RFD_To_ECI.aspx.vb

' Purpose:	This Code Behind to search for ECIs. The user can either push RFD information to an existing ECI or create a new ECI
'
' Date		Author	  Roderick Carlson 
' 11/22/2010 	Created
Partial Class RFD_To_ECI
    Inherits System.Web.UI.Page

    'Private Sub BindCriteria()

    '    Try
    '        'Dim ds As DataSet

    '        ''bind existing data to drop down Customer control for selection criteria for search
    '        'ds = commonFunctions.GetCABBV()
    '        'If commonFunctions.CheckDataset(ds) = True Then
    '        '    ddCABBV.DataSource = ds
    '        '    ddCABBV.DataTextField = ds.Tables(0).Columns("CustomerNameCombo").ColumnName.ToString()
    '        '    ddCABBV.DataValueField = ds.Tables(0).Columns("CABBV").ColumnName.ToString()
    '        '    ddCABBV.DataBind()
    '        '    ddCABBV.Items.Insert(0, "")
    '        'End If

    '    Catch ex As Exception

    '        'update error on web page
    '        lblMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub CheckRights()

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

            ViewState("TeamMemberID") = 0
            ViewState("SubscriptionID") = 0

            'need to know team members specific subscription/role            
            ViewState("isDefaultQualityEngineer") = False
            ViewState("isQualityEngineer") = False


            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''''' TESTING AS Different user
                If iTeamMemberID = 530 Then
                    'iTeamMemberID = 32 'Dan Cade
                    'iTeamMemberID = 433 'Derek Ames
                    'iTeamMemberID = 698 'Emmanuel Reymond
                    'iTeamMemberID = 246 'Mike Echevarria
                    'iTeamMemberID = 575 'Rick Matheny
                    'iTeamMemberID = 105 'Ron Davis
                    'iTeamMemberID = 46 'Joseph Koch
                    'iTeamMemberID = 188 'Duane Rushing
                    'iTeamMemberID = 613 ' Stephanie Serdar
                    'iTeamMemberID = 672 ' John Mercado
                    'iTeamMemberID = 476 ' Pranav
                    iTeamMemberID = 140 ' Bryan Hall
                    'iTeamMemberID = 611 'Vincente.Chavez
                    'iTeamMemberID = 428 'Tracy Theos
                    'iTeamMemberID = 222 'Jim Meade
                    'iTeamMemberID = 666 ' Chris Sleath
                    'iTeamMemberID = 2 'Brett Barta
                    'iTeamMemberID = 4 'Kenta Shinohara 
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                

                'Quality Engineer
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 22)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 22
                    ViewState("isQualityEngineer") = True
                End If


                'Default QualityEngineer
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 51)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("isDefaultQualityEngineer") = True
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 37)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    'there should be no read only viewers of this page, unless admin users see an approved quote
                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isEdit") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete

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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try

            If ViewState("isQualityEngineer") = True Or ViewState("isAdmin") = True Then
                btnAdd.Enabled = ViewState("isEdit")
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
    Private Sub SendDataBackToParentForm(ByVal ECINo As String)

        Try
            ' If the control id's of the parent form are available for receiving data, continue.
            ' Otherwise, exit this procedure.
            If ViewState("ECINoControlID") Is Nothing Then
                Exit Sub
            End If

            ' Build client JavaScript code:
            '  1. populate textboxes on parent form
            '  2. re-validate the parent form
         
            Dim strScript As String = "<script>window.opener.document.forms[0]." & ViewState("ECINoControlID").ToString() & ".value = '" & ECINo & "';"

            'If ViewState("BarCodevcPartNo") IsNot Nothing Then
            '    strScript += "window.opener.document.forms[0]." & ViewState("BarCodevcPartNo").ToString() & ".value = '" & BarCodePartNo & "';"
            'End If

            strScript += "self.close();</script>"

            '"window.opener.Page_ClientValidate();" & _   DISABLE
            Dim cstype As Type = Me.GetType()
            ClientScript.RegisterClientScriptBlock(cstype, "test", strScript)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub ' SendDataBackToParentForm
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack Then

                CheckRights()

                Dim FullName As String = commonFunctions.getUserName()
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If

                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                End If

                ' Save the QueryString controls in ViewState
                If Request.QueryString("ECINoControlID") IsNot Nothing Then
                    ViewState("ECINoControlID") = Request.QueryString("ECINoControlID").ToString()
                End If

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                End If

                If HttpContext.Current.Request.QueryString("RFDSelectionType") <> "" Then
                    ViewState("RFDSelectionType") = HttpContext.Current.Request.QueryString("RFDSelectionType")
                End If

                If HttpContext.Current.Request.QueryString("ChildRowID") <> "" Then
                    ViewState("ChildRowID") = HttpContext.Current.Request.QueryString("ChildRowID")
                End If

                ViewState("ECINo") = ""
                ViewState("ECIDesc") = ""

                ''******
                '' Bind drop down lists
                ''******
                'BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("ECINo") <> "" Then
                    txtECINo.Text = HttpContext.Current.Request.QueryString("ECINo")
                    ViewState("ECINo") = HttpContext.Current.Request.QueryString("ECINo")
                End If

                If HttpContext.Current.Request.QueryString("ECIDesc") <> "" Then
                    txtECIDesc.Text = HttpContext.Current.Request.QueryString("ECIDesc")
                    ViewState("ECIDesc") = HttpContext.Current.Request.QueryString("ECIDesc")
                End If

                EnableControls()

            Else

                ViewState("ECINo") = txtECINo.Text
                ViewState("ECIDesc") = txtECIDesc.Text

            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub gvECIList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvECIList.SelectedIndexChanged

        Try
            ' The user has selected an item from the GridView.
            ' Send the data back to the parent form.
            Dim row As GridViewRow = gvECIList.SelectedRow
            Dim strECINo As String = row.Cells(1).Text

            SendDataBackToParentForm(strECINo)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            ViewState("ECINo") = ""
            ViewState("ECIDesc") = ""

            txtECINo.Text = ""
            txtECIDesc.Text = ""

            odsECIList.SelectParameters("ECINo").DefaultValue = ""
            odsECIList.SelectParameters("ECIDesc").DefaultValue = ""
            
            gvECIList.DataBind()

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ViewState("ECINo") = txtECINo.Text            
            ViewState("ECIDesc") = txtECIDesc.Text
            
            odsECIList.SelectParameters("ECINo").DefaultValue = ""
            odsECIList.SelectParameters("ECIDesc").DefaultValue = ""

            gvECIList.DataBind()
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            lblMessage.Text = ""

            ViewState("ECINo") = 0

            Dim dsRFD As DataSet
            Dim dsECI As DataSet
            'Dim dsFGPartNos As DataSet 'from PXREF

            Dim dtRFDFinishedGood As DataTable
            Dim objRFDFinishedGoodBLL As RFDFinishedGoodBLL = New RFDFinishedGoodBLL

            Dim dtChildPart As DataTable
            Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL

            Dim iChildRowID As Integer = 0
            Dim iRowCounter As Integer = 0

            Dim dtCustomerProgram As DataTable
            Dim objRFDCustomerProgramBLL As RFDCustomerProgramBLL = New RFDCustomerProgramBLL

            Dim strCABBV As String = ""
            Dim iSoldTo As Integer = 0
            Dim isCustomerApprovalRequired As Boolean = False
            Dim strCustomerApprovalDate As String = ""
            Dim strCustomerApprovalNo As String = ""
            Dim strSOPDate As String = ""
            Dim strEOPDate As String = ""

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim dtFacilityDept As DataTable
            Dim objRFDFacilityDeptBLL As RFDFacilityDeptBLL = New RFDFacilityDeptBLL

            Dim strUGNFacility As String = ""
            Dim iDepartmentID As Integer = 0

            Dim dtVendor As DataTable
            Dim objRFDVendorBLL As RFDVendorBLL = New RFDVendorBLL
            Dim objECIVendorBLL As ECIVendorBLL = New ECIVendorBLL
            Dim iUGNDBVendorID As Integer = 0
            Dim iBPCSVendorID As Integer = 0

            Dim strECIType As String = rbECIType.SelectedValue
            Dim strDesc As String = ""
            Dim iCostSheetID As Integer = 0
            Dim iQualityEngineerID As Integer = 0
            Dim strCurrentDrawingNo As String = ""
            Dim strNewDrawingNo As String = ""
            Dim strCurrentPartNo As String = ""
            Dim strNewPartNo As String = ""
            Dim strCurrentPartRevision As String = ""
            Dim strNewPartRevision As String = ""
            Dim strNewPartName As String = ""
            Dim strCurrentCustomerPartNo As String = ""
            Dim strNewCustomerPartNo As String = ""
            Dim strCurrentDesignLevel As String = ""
            Dim strNewDesignLevel As String = ""
            Dim strCurrentCustomerDrawingNo As String = ""
            Dim strNewCustomerDrawingNo As String = ""
            Dim strDesignationType As String = ""
            Dim iBusinessProcessTypeID As Integer = 0
            Dim iCommodityID As Integer = 0
            Dim iPurchasedGoodID As Integer = 0
            Dim iProductTechnologyID As Integer = 0
            Dim iSubFamilyID As Integer = 0
            Dim iAccountManagerID As Integer = 0
            Dim isPPAP As Boolean = False
            Dim iPPAPLevel As Integer = 0
            Dim strProductionStatus As String = ""
            Dim strVendorRequirement As String = ""

            dsRFD = RFDModule.GetRFD(ViewState("RFDNo"))

            If ViewState("isQualityEngineer") = True Then
                iQualityEngineerID = ViewState("TeamMemberID")
            End If

            If commonFunctions.CheckDataSet(dsRFD) = True Then

                strDesc = dsRFD.Tables(0).Rows(0).Item("RFDDesc").ToString

                If dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID") IsNot System.DBNull.Value Then
                    If dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID") > 0 Then
                        iBusinessProcessTypeID = dsRFD.Tables(0).Rows(0).Item("BusinessProcessTypeID")
                    End If
                End If

                If dsRFD.Tables(0).Rows(0).Item("AccountManagerID") IsNot System.DBNull.Value Then
                    If dsRFD.Tables(0).Rows(0).Item("AccountManagerID") > 0 Then
                        iAccountManagerID = dsRFD.Tables(0).Rows(0).Item("AccountManagerID")
                    End If
                End If

                If dsRFD.Tables(0).Rows(0).Item("isPPAP") IsNot System.DBNull.Value Then
                    isPPAP = dsRFD.Tables(0).Rows(0).Item("isPPAP")
                End If

                strProductionStatus = dsRFD.Tables(0).Rows(0).Item("PriceCode").ToString

                strVendorRequirement = dsRFD.Tables(0).Rows(0).Item("VendorRequirement").ToString

                If dsRFD.Tables(0).Rows(0).Item("NewCommodityID") IsNot System.DBNull.Value Then
                    If dsRFD.Tables(0).Rows(0).Item("NewCommodityID") > 0 Then
                        iCommodityID = dsRFD.Tables(0).Rows(0).Item("NewCommodityID")
                    End If
                End If

                If dsRFD.Tables(0).Rows(0).Item("NewProductTechnologyID") IsNot System.DBNull.Value Then
                    If dsRFD.Tables(0).Rows(0).Item("NewProductTechnologyID") > 0 Then
                        iProductTechnologyID = dsRFD.Tables(0).Rows(0).Item("NewProductTechnologyID")
                    End If
                End If

                'need to check if this is a child part or customer part
                'if this is a top level customer part/finished good then...
                If ViewState("RFDSelectionType") = "TL" Then 'top level

                    If dsRFD.Tables(0).Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                        If dsRFD.Tables(0).Rows(0).Item("CostSheetID") > 0 Then
                            iCostSheetID = dsRFD.Tables(0).Rows(0).Item("CostSheetID")
                        End If
                    End If

                    strCurrentCustomerPartNo = dsRFD.Tables(0).Rows(0).Item("CurrentCustomerPartNo").ToString
                    strNewCustomerPartNo = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartNo").ToString

                    strCurrentDesignLevel = dsRFD.Tables(0).Rows(0).Item("CurrentDesignLevel").ToString
                    strNewDesignLevel = dsRFD.Tables(0).Rows(0).Item("NewDesignLevel").ToString

                    strCurrentDrawingNo = dsRFD.Tables(0).Rows(0).Item("CurrentDrawingNo").ToString
                    strNewDrawingNo = dsRFD.Tables(0).Rows(0).Item("NewDrawingNo").ToString

                    strNewPartName = dsRFD.Tables(0).Rows(0).Item("NewCustomerPartName").ToString

                    strCurrentCustomerDrawingNo = dsRFD.Tables(0).Rows(0).Item("CurrentCustomerDrawingNo").ToString
                    strNewCustomerDrawingNo = dsRFD.Tables(0).Rows(0).Item("NewCustomerDrawingNo").ToString

                    strDesignationType = dsRFD.Tables(0).Rows(0).Item("DesignationType").ToString

                    If dsRFD.Tables(0).Rows(0).Item("NewSubFamilyID") IsNot System.DBNull.Value Then
                        If dsRFD.Tables(0).Rows(0).Item("NewSubFamilyID") > 0 Then
                            iSubFamilyID = dsRFD.Tables(0).Rows(0).Item("NewSubFamilyID")
                        End If
                    End If

                    'get first finished good BPCS part number, if exists
                    dtRFDFinishedGood = objRFDFinishedGoodBLL.GetRFDFinishedGood(ViewState("RFDNo"))
                    If commonFunctions.CheckDataTable(dtRFDFinishedGood) = True Then
                        strNewPartNo = dtRFDFinishedGood.Rows(0).Item("PartNo").ToString
                        strNewPartRevision = dtRFDFinishedGood.Rows(0).Item("PartRevision").ToString
                        strNewPartName = dtRFDFinishedGood.Rows(0).Item("PartName").ToString
                    End If

                    'get current BPCS PartNos based on Current Customer Part No
                    '(LREY) 01/07/2014
                    ''If strCurrentCustomerPartNo <> "" Then
                    ''    dsFGPartNos = commonFunctions.GetCustomerPartPartRelate("", strCurrentCustomerPartNo, "", "", "")

                    ''    If commonFunctions.CheckDataSet(dsFGPartNos) = True Then
                    ''        strCurrentPartNo = dsFGPartNos.Tables(0).Rows(0).Item("PartNo").ToString.Trim
                    ''    End If

                    ''End If

                End If

                'if this is a child part
                If ViewState("RFDSelectionType") = "CP" And ViewState("ChildRowID") <> "" Then 'childpart
                    iChildRowID = CType(ViewState("ChildRowID"), Integer)

                    If iChildRowID > 0 Then
                        dtChildPart = objRFDChildPartBLL.GetRFDChildPart(iChildRowID, ViewState("RFDNo"))

                        If commonFunctions.CheckDataTable(dtChildPart) = True Then
                            strDesignationType = dtChildPart.Rows(0).Item("NewDesignationType").ToString

                            strCurrentDrawingNo = dtChildPart.Rows(0).Item("CurrentDrawingNo").ToString
                            strNewDrawingNo = dtChildPart.Rows(0).Item("NewDrawingNo").ToString

                            If dtChildPart.Rows(0).Item("NewPurchasedGoodID") IsNot System.DBNull.Value Then
                                If dtChildPart.Rows(0).Item("NewPurchasedGoodID") > 0 Then
                                    iPurchasedGoodID = dtChildPart.Rows(0).Item("NewPurchasedGoodID")
                                End If
                            End If

                            strCurrentPartNo = dtChildPart.Rows(0).Item("CurrentPartNo").ToString
                            strNewPartNo = dtChildPart.Rows(0).Item("NewPartNo").ToString

                            strCurrentPartRevision = dtChildPart.Rows(0).Item("CurrentPartRevision").ToString
                            strNewPartRevision = dtChildPart.Rows(0).Item("NewPartRevision").ToString

                            strNewPartName = dtChildPart.Rows(0).Item("NewPartName").ToString

                            If dtChildPart.Rows(0).Item("CostSheetID") IsNot System.DBNull.Value Then
                                If dtChildPart.Rows(0).Item("CostSheetID").ToString <> "" Then
                                    iCostSheetID = CType(dtChildPart.Rows(0).Item("CostSheetID").ToString, Integer)
                                End If
                            End If

                            If dtChildPart.Rows(0).Item("NewSubFamilyID") IsNot System.DBNull.Value Then
                                If dtChildPart.Rows(0).Item("NewSubFamilyID") > 0 Then
                                    iSubFamilyID = dtChildPart.Rows(0).Item("NewSubFamilyID")
                                End If
                            End If

                        End If
                    End If
                End If

                dsECI = ECIModule.InsertECI(0, strECIType, 1, "", "", _
                    ViewState("RFDNo"), iCostSheetID, iQualityEngineerID, _
                    strCurrentDrawingNo, strNewDrawingNo, strCurrentPartNo, strNewPartNo, strCurrentPartRevision, _
                    strNewPartRevision, strNewPartName, strCurrentCustomerPartNo, strNewCustomerPartNo, strCurrentDesignLevel, _
                    strNewDesignLevel, strCurrentCustomerDrawingNo, strNewCustomerDrawingNo, strDesignationType, iBusinessProcessTypeID, _
                    iCommodityID, iPurchasedGoodID, iProductTechnologyID, iSubFamilyID, iAccountManagerID, iQualityEngineerID, _
                    isPPAP, iPPAPLevel, strProductionStatus, False, False, "", "", strDesc, "", "", strVendorRequirement, 0, 0)

                If commonFunctions.CheckDataSet(dsECI) = True Then
                    If dsECI.Tables(0).Rows(0).Item("NewECINo") IsNot System.DBNull.Value Then
                        If dsECI.Tables(0).Rows(0).Item("NewECINo") > 0 Then

                            ViewState("ECINo") = dsECI.Tables(0).Rows(0).Item("NewECINo")

                            lblMessage.Text = "A New ECI was created: " & ViewState("ECINo")

                            gvECIList.DataBind()

                            'append to customer program list
                            dtCustomerProgram = objRFDCustomerProgramBLL.GetRFDCustomerProgram(ViewState("RFDNo"))

                            If commonFunctions.CheckDataTable(dtCustomerProgram) = True Then

                                For iRowCounter = 0 To dtCustomerProgram.Rows.Count - 1
                                    strCABBV = ""
                                    If dtCustomerProgram.Rows(iRowCounter).Item("CABBV").ToString <> "" Then
                                        strCABBV = dtCustomerProgram.Rows(iRowCounter).Item("CABBV").ToString
                                    End If

                                    iSoldTo = 0
                                    If dtCustomerProgram.Rows(iRowCounter).Item("Soldto") IsNot System.DBNull.Value Then
                                        If dtCustomerProgram.Rows(iRowCounter).Item("SoldTo") > 0 Then
                                            iSoldTo = dtCustomerProgram.Rows(iRowCounter).Item("SoldTo")
                                        End If
                                    End If

                                    isCustomerApprovalRequired = False
                                    If dtCustomerProgram.Rows(iRowCounter).Item("isCustomerApprovalRequired") IsNot System.DBNull.Value Then
                                        isCustomerApprovalRequired = dtCustomerProgram.Rows(iRowCounter).Item("isCustomerApprovalRequired")
                                    End If

                                    strCustomerApprovalDate = ""
                                    If dtCustomerProgram.Rows(iRowCounter).Item("CustomerApprovalDate") IsNot System.DBNull.Value Then
                                        strCustomerApprovalDate = dtCustomerProgram.Rows(iRowCounter).Item("CustomerApprovalDate").ToString
                                    End If

                                    strCustomerApprovalNo = ""
                                    If dtCustomerProgram.Rows(iRowCounter).Item("CustomerApprovalNo") IsNot System.DBNull.Value Then
                                        strCustomerApprovalNo = dtCustomerProgram.Rows(iRowCounter).Item("CustomerApprovalNo").ToString
                                    End If

                                    strSOPDate = ""
                                    If dtCustomerProgram.Rows(iRowCounter).Item("SOPDate") IsNot System.DBNull.Value Then
                                        strSOPDate = dtCustomerProgram.Rows(iRowCounter).Item("SOPDate").ToString
                                    End If

                                    strEOPDate = ""
                                    If dtCustomerProgram.Rows(iRowCounter).Item("EOPDate") IsNot System.DBNull.Value Then
                                        strEOPDate = dtCustomerProgram.Rows(iRowCounter).Item("EOPDate").ToString
                                    End If

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
                                        ECIModule.InsertECICustomerProgram(ViewState("ECINo"), isCustomerApprovalRequired, strCustomerApprovalDate, strCustomerApprovalNo, iProgramID, iProgramYear, strSOPDate, strEOPDate)
                                    End If

                                    '  *** i have to add these two strCABBV, iSoldTo,***

                                Next
                            End If

                            'append to the facility list
                            dtFacilityDept = objRFDFacilityDeptBLL.GetRFDFacilityDept(ViewState("RFDNo"))

                            If commonFunctions.CheckDataTable(dtFacilityDept) = True Then
                                For iRowCounter = 0 To dtFacilityDept.Rows.Count - 1
                                    strUGNFacility = dtFacilityDept.Rows(iRowCounter).Item("UGNFacility").ToString

                                    iDepartmentID = 0
                                    If dtFacilityDept.Rows(iRowCounter).Item("DepartmentID") IsNot System.DBNull.Value Then
                                        If dtFacilityDept.Rows(iRowCounter).Item("DepartmentID") > 0 Then
                                            iDepartmentID = dtFacilityDept.Rows(iRowCounter).Item("DepartmentID")
                                        End If
                                    End If

                                    ECIModule.InsertECIFacilityDept(ViewState("ECINo"), strUGNFacility, iDepartmentID)
                                Next
                            End If

                            'append to the Vendor list - NO FUTURE VENDORS - ONLY EXISTING BPCS VENDORS
                            dtVendor = objRFDVendorBLL.GetRFDVendor(ViewState("RFDNo"))
                            If commonFunctions.CheckDataTable(dtVendor) = True Then
                                For iRowCounter = 0 To dtVendor.Rows.Count - 1
                                    iUGNDBVendorID = 0
                                    iBPCSVendorID = 0
                                    If dtVendor.Rows(iRowCounter).Item("BPCSVendorID") IsNot System.DBNull.Value Then
                                        If dtVendor.Rows(iRowCounter).Item("BPCSVendorID") > 0 Then
                                            iBPCSVendorID = dtVendor.Rows(iRowCounter).Item("BPCSVendorID")
                                        End If
                                    End If

                                    If dtVendor.Rows(iRowCounter).Item("UGNDBVendorID") IsNot System.DBNull.Value Then
                                        If dtVendor.Rows(iRowCounter).Item("UGNDBVendorID") > 0 Then
                                            iUGNDBVendorID = dtVendor.Rows(iRowCounter).Item("UGNDBVendorID")
                                        End If
                                    End If

                                    If iBPCSVendorID > 0 Then
                                        objECIVendorBLL.InsertECIVendor(ViewState("ECINo"), iUGNDBVendorID, "", "", "")
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If

            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
