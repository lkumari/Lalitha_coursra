' ************************************************************************************************
'
' Name:		MaterialMaint.aspx
' Purpose:	This Code Behind is to maintain the catching ability factor used by the Costing Module
'
' Date		Author	    
' 10/14/2008    RCarlson   Created
' 11/04/2009    RCarlson   Modified: added BPCS Part Name, added function to update DMS Drawing with BPCS PartNo
' 08/26/2010    RCarlson   Modified: added isActiveBPCSOnly Parameter to GetUGNDBVendor
' 08/26/2011    RCarlson   Modified: allow negative values to numeric fields 
' 01/03/2014    LREY       Replaced "PartNo" with "PartNo" wherever used. Hide the PartRevision fields as the new RMs will have revision in its part numbering scheme.
'12/19/2014     LMeka      Modified: Added UGNFacility Code, 
' ************************************************************************************************
Partial Class Material_Maint
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Material/Packaging Maintenance"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > <a href='Material_List.aspx'><b> Material/Packaging Search </b></a> > Material/Packaging Maintenance "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            CheckRights()

            If Not Page.IsPostBack Then
                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                BindCriteria()

                ViewState("MaterialID") = 0
                If HttpContext.Current.Request.QueryString("MaterialID") <> "" Then
                    ViewState("MaterialID") = CType(HttpContext.Current.Request.QueryString("MaterialID"), Integer)
                    BindData()
                End If

                'Dim strPartNoClientScript As String = HandleBPCSPopUps(txtPartNoValue.ClientID, txtPartRevisionValue.ClientID, txtMaterialNameValue.ClientID)
                '' Dim strUGNFaciity As String = HandleBPCSPopUps(ddUGNFacilityCodeValue.ClientID, "", ddUGNFacilityCodeValue.ClientID)
                Dim strPartNoClientScript As String = HandleBPCSPopUps(txtPartNoValue.ClientID, "", txtMaterialNameValue.ClientID)
                iBtnPartNo.Attributes.Add("onClick", strPartNoClientScript)

                Dim strDrawingNoClientScript As String = HandleDrawingPopUps(txtDrawingNoValue.ClientID)
                iBtnGetDrawingInfo.Attributes.Add("onClick", strDrawingNoClientScript)
            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Function HandleBPCSPopUps(ByVal cciPartNo As String, ByVal cciPartRevision As String, ByVal cciPartDesc As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & cciPartNo & "&vcPartRevision=" & cciPartRevision & "&vcPartDescr=" & cciPartDesc
            
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleBPCSPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleBPCSPopUps = ""
        End Try

    End Function
    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

        Try
            ' Build the client script to open a popup window 
            ' Pass the ClientID of the TextBox (which will receive data from the popup)
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
                "window.open('" & strPagePath & "','DrawingNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function
    Protected Sub ValidateIdentificationNumbers()

        Try
            Dim ds As DataSet

            If txtDrawingNoValue.Text.Trim <> "" Then
                'ds = PEModule.GetDrawing(txtDrawingNoValue.Text.Trim, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "")
                ds = PEModule.GetDrawing(txtDrawingNoValue.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />WARNING: The DMS drawing number is not in the DMS system. Please contact Product Engineering."
                End If
            End If

            If txtPartNoValue.Text.Trim <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtPartNoValue.Text.Trim, "")
                If commonFunctions.CheckDataSet(ds) = False Then
                    lblMessage.Text &= "<br />WARNING: The Internal Part Number is not in the Oracle system. Please contact Product Engineering."
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

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 75)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = True
                            End Select
                        End If
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
    Protected Sub EnableControls()

        Try
            Dim ds As DataSet

            If lblMaterialIDValue.Text <> "" Then
                lblMaterialIDLabel.Visible = Not ViewState("isRestricted")
                lblMaterialIDValue.Visible = Not ViewState("isRestricted")
            End If

            lblMaterialNameLabel.Visible = Not ViewState("isRestricted")
            lblMaterialNameMarker.Visible = Not ViewState("isRestricted")
            txtMaterialNameValue.Visible = Not ViewState("isRestricted")

            lblMaterialDescLabel.Visible = Not ViewState("isRestricted")
            txtMaterialDescValue.Visible = Not ViewState("isRestricted")

            lblPartNoLabel.Visible = Not ViewState("isRestricted")
            txtPartNoValue.Visible = Not ViewState("isRestricted")

            'lblPartRevisionLabel.Visible = Not ViewState("isRestricted")
            'txtPartRevisionValue.Visible = Not ViewState("isRestricted")

            lblDrawingNoLabel.Visible = Not ViewState("isRestricted")
            txtDrawingNoValue.Visible = Not ViewState("isRestricted")

            lblUGNDBVendorLabel.Visible = Not ViewState("isRestricted")
            ddUGNDBVendorValue.Visible = Not ViewState("isRestricted")

            lblPurchasedGoodLabel.Visible = Not ViewState("isRestricted")
            ddPurchasedGoodValue.Visible = Not ViewState("isRestricted")

            lblOldMaterialGroupLabel.Visible = Not ViewState("isRestricted")
            lblOldMaterialGroupValue.Visible = Not ViewState("isRestricted")

            If lblOldMaterialGroupValue.Text = "" Then
                lblOldMaterialGroupLabel.Visible = False
            End If

            If lblStandardCostDateValue.Text <> "" Then
                lblStandardCostDateLabel.Visible = Not ViewState("isRestricted")
                lblStandardCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblPurchasedCostDateValue.Text <> "" Then
                lblPurchasedCostDateLabel.Visible = Not ViewState("isRestricted")
                lblPurchasedCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblQuoteCostDateValue.Text <> "" Then
                lblQuoteCostDateLabel.Visible = Not ViewState("isRestricted")
                lblQuoteCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblFreightCostDateValue.Text <> "" Then
                lblFreightCostDateLabel.Visible = Not ViewState("isRestricted")
                lblFreightCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            lblStandardCostLabel.Visible = Not ViewState("isRestricted")
            lblStandardCostValue.Visible = Not ViewState("isRestricted")

            lblPurchasedCostLabel.Visible = Not ViewState("isRestricted")
            lblPurchasedCostValue.Visible = Not ViewState("isRestricted")

            lblQuoteCostLabel.Visible = Not ViewState("isRestricted")
            txtQuoteCostValue.Visible = Not ViewState("isRestricted")

            lblFreightCostLabel.Visible = Not ViewState("isRestricted")
            txtFreightCostValue.Visible = Not ViewState("isRestricted")

            lblUnitofMeasureLabel.Visible = Not ViewState("isRestricted")
            ddUnitofMeasureValue.Visible = Not ViewState("isRestricted")

            lblQuoteCostLabel.Visible = Not ViewState("isRestricted")
            txtQuoteCostValue.Visible = Not ViewState("isRestricted")

            lblFreightCostLabel.Visible = Not ViewState("isRestricted")
            txtFreightCostValue.Visible = Not ViewState("isRestricted")

            lblUnitofMeasureLabel.Visible = Not ViewState("isRestricted")
            ddUnitofMeasureValue.Visible = Not ViewState("isRestricted")

            lblIsCoatingLabel.Visible = Not ViewState("isRestricted")
            cbIsCoatingValue.Visible = Not ViewState("isRestricted")

            lblObsoleteLabel.Visible = Not ViewState("isRestricted")
            cbObsoleteValue.Visible = Not ViewState("isRestricted")

            lblIsPackagingLabel.Visible = Not ViewState("isRestricted")
            cbIsPackagingValue.Visible = Not ViewState("isRestricted")

            If ViewState("isRestricted") = False Then

                btnSave.Visible = ViewState("isAdmin")

                If ViewState("MaterialID") > 0 Then
                    btnCopy.Visible = ViewState("isAdmin")
                End If

                txtMaterialNameValue.Enabled = ViewState("isAdmin")
                txtMaterialDescValue.Enabled = ViewState("isAdmin")
                txtPartNoValue.Enabled = ViewState("isAdmin")
                'txtPartRevisionValue.Enabled = ViewState("isAdmin")
                txtDrawingNoValue.Enabled = ViewState("isAdmin")
                ddUGNDBVendorValue.Enabled = ViewState("isAdmin")
                ddPurchasedGoodValue.Enabled = ViewState("isAdmin")
                txtQuoteCostValue.Enabled = ViewState("isAdmin")
                txtFreightCostValue.Enabled = ViewState("isAdmin")
                ddUnitofMeasureValue.Enabled = ViewState("isAdmin")
                txtQuoteCostValue.Enabled = ViewState("isAdmin")
                txtFreightCostValue.Enabled = ViewState("isAdmin")
                ddUnitofMeasureValue.Enabled = ViewState("isAdmin")
                cbIsCoatingValue.Enabled = ViewState("isAdmin")
                cbObsoleteValue.Enabled = ViewState("isAdmin")
                cbIsPackagingValue.Enabled = ViewState("isAdmin")
                iBtnPartNo.Visible = ViewState("isAdmin")
                iBtnGetDrawingInfo.Visible = ViewState("isAdmin")

                hlnkDrawingNo.Visible = False
                If txtDrawingNoValue.Text.Trim <> "" Then
                    ds = PEModule.GetDrawing(txtDrawingNoValue.Text.Trim)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        hlnkDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtDrawingNoValue.Text.Trim
                        hlnkDrawingNo.Visible = True
                        btnUpdateDrawing.Visible = ViewState("isAdmin")
                    End If
                End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            'bind existing data to drop down PurchasedGood 
            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPurchasedGoodValue.DataSource = ds
                ddPurchasedGoodValue.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName.ToString()
                ddPurchasedGoodValue.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGoodValue.DataBind()
                ddPurchasedGoodValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Vendor 
            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNDBVendorValue.DataSource = ds
                ddUGNDBVendorValue.DataTextField = ds.Tables(0).Columns("ddSupplierName").ColumnName.ToString()
                ddUGNDBVendorValue.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddUGNDBVendorValue.DataBind()
                ddUGNDBVendorValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Unit of Measure
            ds = commonFunctions.GetUnit(0, "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUnitofMeasureValue.DataSource = ds
                ddUnitofMeasureValue.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName.ToString()
                ddUnitofMeasureValue.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddUnitofMeasureValue.DataBind()
                ddUnitofMeasureValue.Items.Insert(0, "")
            End If
            ''bind existing data to drop down UGN Facility Code
            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacilityCodeValue.DataSource = ds
                ddUGNFacilityCodeValue.DataTextField = ds.Tables(0).Columns("UGNFacilityName").ColumnName.ToString()
                ddUGNFacilityCodeValue.DataValueField = ds.Tables(0).Columns("UGNFacilityCode").ColumnName
                ddUGNFacilityCodeValue.DataBind()
                ddUGNFacilityCodeValue.Items.Insert(0, "")

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
    Private Sub BindData()

        Try

            Dim ds As DataSet = New DataSet
            Dim dStandardCost As Double = 0
            Dim dPurchasedCost As Double = 0
            Dim dQuoteCost As Double = 0
            Dim dFreightCost As Double = 0

            If ViewState("MaterialID") > 0 Then
                'bind existing CostSheet data to for top level cost sheet info                     
                ds = CostingModule.GetMaterial(ViewState("MaterialID"), "", "", "", 0, 0, "", "", False, False, False, False, False, False)

                If ViewState("isRestricted") = False Then
                    If commonFunctions.CheckDataSet(ds) = True Then


                        lblMaterialIDValue.Text = ds.Tables(0).Rows(0).Item("MaterialID")
                        txtMaterialNameValue.Text = ds.Tables(0).Rows(0).Item("MaterialName").ToString
                        txtMaterialDescValue.Text = ds.Tables(0).Rows(0).Item("MaterialDesc").ToString
                        txtDrawingNoValue.Text = ds.Tables(0).Rows(0).Item("DrawingNo").ToString

                        If ds.Tables(0).Rows(0).Item("UGNDBVendorID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("UGNDBVendorID") > 0 Then
                                ddUGNDBVendorValue.SelectedValue = ds.Tables(0).Rows(0).Item("UGNDBVendorID")
                            End If
                        End If

                        txtPartNoValue.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString
                        lblPartDescValue.Text = ds.Tables(0).Rows(0).Item("PartDescription").ToString

                        If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                ddPurchasedGoodValue.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("UGNFacilityCode").ToString <> "" Then
                            ddUGNFacilityCodeValue.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacilityCode")
                        End If

                        If ds.Tables(0).Rows(0).Item("OldMaterialGroup").ToString <> "" Then
                            lblOldMaterialGroupValue.Text = ds.Tables(0).Rows(0).Item("OldMaterialGroup").ToString
                        End If

                        If ds.Tables(0).Rows(0).Item("StandardCost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("StandardCost") <> 0 Then
                                lblStandardCostValue.Text = ds.Tables(0).Rows(0).Item("StandardCost")
                                dStandardCost = ds.Tables(0).Rows(0).Item("StandardCost")
                                ViewState("StandardCost") = dStandardCost
                            End If
                        End If

                        lblStandardCostDateValue.Text = ds.Tables(0).Rows(0).Item("StandardCostDate").ToString

                        If ds.Tables(0).Rows(0).Item("PurchasedCost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedCost") <> 0 Then
                                lblPurchasedCostValue.Text = ds.Tables(0).Rows(0).Item("PurchasedCost")
                                dPurchasedCost = ds.Tables(0).Rows(0).Item("PurchasedCost")
                                ViewState("PurchasedCost") = dPurchasedCost
                            End If
                        End If

                        lblPurchasedCostDateValue.Text = ds.Tables(0).Rows(0).Item("PurchasedCostDate").ToString

                        If ds.Tables(0).Rows(0).Item("QuoteCost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("QuoteCost") <> 0 Then
                                txtQuoteCostValue.Text = ds.Tables(0).Rows(0).Item("QuoteCost")
                                dQuoteCost = ds.Tables(0).Rows(0).Item("QuoteCost")
                                ViewState("QuoteCost") = ds.Tables(0).Rows(0).Item("QuoteCost")
                            End If
                        End If

                        lblQuoteCostDateValue.Text = ds.Tables(0).Rows(0).Item("QuoteCostDate").ToString

                        If ds.Tables(0).Rows(0).Item("FreightCost") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("FreightCost") <> 0 Then
                                txtFreightCostValue.Text = ds.Tables(0).Rows(0).Item("FreightCost")
                                dFreightCost = ds.Tables(0).Rows(0).Item("FreightCost")
                                ViewState("FreightCost") = ds.Tables(0).Rows(0).Item("FreightCost")
                            End If
                        End If

                        lblFreightCostDateValue.Text = ds.Tables(0).Rows(0).Item("FreightCostDate").ToString

                        If ds.Tables(0).Rows(0).Item("UnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("UnitID") > 0 Then
                                ddUnitofMeasureValue.SelectedValue = ds.Tables(0).Rows(0).Item("UnitID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("IsCoating") IsNot System.DBNull.Value Then
                            cbIsCoatingValue.Checked = ds.Tables(0).Rows(0).Item("IsCoating")
                        End If

                        If ds.Tables(0).Rows(0).Item("IsPackaging") IsNot System.DBNull.Value Then
                            cbIsPackagingValue.Checked = ds.Tables(0).Rows(0).Item("IsPackaging")
                        End If

                        If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                            cbObsoleteValue.Checked = ds.Tables(0).Rows(0).Item("Obsolete")
                        End If

                        If dPurchasedCost + dFreightCost <> dStandardCost Then
                            lblMessage.Text &= "<br />WARNING: The purchased cost PLUS freight cost does NOT EQUAL the standard cost."
                            txtFreightCostValue.BackColor = Color.Yellow
                        Else
                            txtFreightCostValue.BackColor = Color.White
                        End If

                        If dPurchasedCost <> dQuoteCost Then
                            lblMessage.Text &= "<br />WARNING: The purchased cost does NOT EQUAL the quote cost."
                            txtQuoteCostValue.BackColor = Color.Yellow
                        Else
                            txtQuoteCostValue.BackColor = Color.White
                        End If

                    End If  'end material load ds is not empty

                End If ' end restricted read only
            End If ' if formula id > 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error

            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            lblMessage.Text = ""

            Dim ds As DataSet
            Dim bPartNoFound As Boolean = False
            Dim iRowCounter As Integer = 0

            ValidateIdentificationNumbers()

            Dim dTempFreightCost As Double = 0
            If txtFreightCostValue.Text.Trim <> "" Then
                dTempFreightCost = CType(txtFreightCostValue.Text, Double)
            End If

            Dim dTempQuoteCost As Double = 0
            If txtQuoteCostValue.Text.Trim <> "" Then
                dTempQuoteCost = CType(txtQuoteCostValue.Text, Double)
            End If

            Dim iTempPurchasedGoodID As Integer = 0
            If ddPurchasedGoodValue.SelectedIndex > 0 Then
                iTempPurchasedGoodID = ddPurchasedGoodValue.SelectedValue
            End If

            Dim strUGNFacilityCode As String = ""
            If ddUGNFacilityCodeValue.SelectedValue <> "" Then
                strUGNFacilityCode = ddUGNFacilityCodeValue.SelectedValue
            End If


            Dim iTempUGNDBVendorID As Integer = 0
            If ddUGNDBVendorValue.SelectedIndex > 0 Then
                iTempUGNDBVendorID = ddUGNDBVendorValue.SelectedValue
            End If

            Dim iTempUnitID As Integer = 0
            If ddUnitofMeasureValue.SelectedIndex > 0 Then
                iTempUnitID = ddUnitofMeasureValue.SelectedValue
            End If

            Dim strQuoteCostDate As String = lblQuoteCostDateValue.Text
            If ViewState("QuoteCost") <> dTempQuoteCost Then
                strQuoteCostDate = Today.Date
                lblQuoteCostDateValue.Text = strQuoteCostDate
            End If

            Dim strFreightCostDate As String = lblFreightCostDateValue.Text
            If ViewState("FreightCost") <> dTempFreightCost Then
                strFreightCostDate = Today.Date
                lblFreightCostDateValue.Text = strFreightCostDate
            End If

            If ViewState("MaterialID") = 0 Then
                'check if BPCS Part No already exists
                If txtPartNoValue.Text.Trim <> "" And cbObsoleteValue.Checked = False Then
                    ds = CostingModule.GetMaterial(0, "", txtPartNoValue.Text.Trim, "", 0, 0, "", "", False, False, False, False, False, True)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        lblMessage.Text &= "<br />Error: The new material could not be saved because there already is a material with this Internal PartNo."
                        bPartNoFound = True
                    Else
                    End If
                End If

                If bPartNoFound = False Then
                    'insert new record           

                    ds = CostingModule.InsertMaterial(txtMaterialNameValue.Text.Trim, txtMaterialDescValue.Text.Trim, txtPartNoValue.Text.Trim, _
                     "", txtDrawingNoValue.Text.Trim, iTempUGNDBVendorID, iTempPurchasedGoodID, strUGNFacilityCode, _
                    dTempQuoteCost, strQuoteCostDate, dTempFreightCost, strFreightCostDate, iTempUnitID, cbIsCoatingValue.Checked, _
                    cbIsPackagingValue.Checked, cbObsoleteValue.Checked)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        ViewState("MaterialID") = ds.Tables(0).Rows(0).Item("NewMaterialID")
                        txtMaterialNameValue.Text = ds.Tables(0).Rows(0).Item("NewMaterialName").ToString
                        lblMaterialIDValue.Text = ds.Tables(0).Rows(0).Item("NewMaterialID")
                        lblMaterialIDLabel.Visible = True
                        lblMaterialIDValue.Visible = True
                        lblMessage.Text &= "<br />Saved Successfully."
                    End If
                End If

            Else

                'check if BPCS Part number exists for a different material
                If txtPartNoValue.Text.Trim <> "" And cbObsoleteValue.Checked = False Then
                    ds = CostingModule.GetMaterial(0, "", txtPartNoValue.Text.Trim, "", 0, 0, "", "", False, False, False, False, False, True)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                            If ds.Tables(0).Rows(iRowCounter).Item("MaterialID") <> ViewState("MaterialID") Then
                                lblMessage.Text &= "<br />Error: The material could not be saved because there is already a material with this Internal Part No."
                                bPartNoFound = True
                            End If
                        Next
                    Else
                    End If
                End If

                If bPartNoFound = False Then

                    ds = CostingModule.InsertMaterial(txtMaterialNameValue.Text.Trim, txtMaterialDescValue.Text.Trim, txtPartNoValue.Text.Trim, _
                     "", txtDrawingNoValue.Text.Trim, iTempUGNDBVendorID, iTempPurchasedGoodID, strUGNFacilityCode, _
                    dTempQuoteCost, strQuoteCostDate, dTempFreightCost, strFreightCostDate, iTempUnitID, cbIsCoatingValue.Checked, _
                    cbIsPackagingValue.Checked, cbObsoleteValue.Checked)
                    lblMessage.Text &= "<br />Saved Successfully."
                End If

            End If

            If ViewState("PurchasedCost") + dTempFreightCost <> ViewState("StandardCost") Then
                lblMessage.Text &= "<br />WARNING: The purchased cost PLUS freight cost does NOT EQUAL the standard cost."
                txtFreightCostValue.BackColor = Color.Yellow
            Else
                txtFreightCostValue.BackColor = Color.White
            End If

            If ViewState("PurchasedCost") <> dTempQuoteCost Then
                lblMessage.Text &= "<br />WARNING: The purchased cost does NOT EQUAL the quote cost."
                txtQuoteCostValue.BackColor = Color.Yellow
            Else
                txtQuoteCostValue.BackColor = Color.White
            End If

            If lblStandardCostDateValue.Text <> "" Then
                lblStandardCostDateLabel.Visible = Not ViewState("isRestricted")
                lblStandardCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblPurchasedCostDateValue.Text <> "" Then
                lblPurchasedCostDateLabel.Visible = Not ViewState("isRestricted")
                lblPurchasedCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblQuoteCostDateValue.Text <> "" Then
                lblQuoteCostDateLabel.Visible = Not ViewState("isRestricted")
                lblQuoteCostDateValue.Visible = Not ViewState("isRestricted")
            End If

            If lblFreightCostDateValue.Text <> "" Then
                lblFreightCostDateLabel.Visible = Not ViewState("isRestricted")
                lblFreightCostDateValue.Visible = Not ViewState("isRestricted")
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

    Protected Sub iBtnGetDrawingInfo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnGetDrawingInfo.Click

        'Try
        '    lblMessage.Text = ""

        '    Dim ds As DataSet = New DataSet

        '    If txtDrawingNoValue.Text.Trim <> "" Then

        '        'bind existing Drawing Info info to Material                  
        '        ds = PEModule.GetDrawing(txtDrawingNoValue.Text.Trim, "", "", "", "", "", 0, 0, 0, 0, 0, "", "", "", 0, False, False, "", "")

        '        If commonFunctions.CheckDataset(ds) = True Then
        '            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
        '                If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
        '                    ddPurchasedGoodValue.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
        '                End If
        '            End If
        '        End If
        '    End If

        'Catch ex As Exception

        '    'get current event name
        '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

        '    'update error on web page
        '    lblMessage.Text &= ex.Message & "<br />" & mb.Name

        '    'log and email error
        '    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        'End Try

    End Sub

    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        lblMessage.Text = ""

        Try
            ViewState("MaterialID") = 0
            lblMaterialIDLabel.Visible = False
            lblMaterialIDValue.Text = ""
            'txtPartNoValue.Text = ""
            'txtPartRevisionValue.Text = ""
            cbObsoleteValue.Checked = False
            lblOldMaterialGroupLabel.Visible = False
            lblOldMaterialGroupValue.Text = ""
            lblStandardCostLabel.Visible = False
            lblStandardCostValue.Visible = False
            lblStandardCostDateLabel.Visible = False
            lblStandardCostDateValue.Visible = False
            lblPurchasedCostLabel.Visible = False
            lblPurchasedCostValue.Visible = False
            lblPurchasedCostDateLabel.Visible = False
            lblPurchasedCostDateValue.Visible = False
            'lblPartDescValue.Text = ""

            btnCopy.Visible = False
            btnUpdateDrawing.Visible = False

            txtMaterialNameValue.Text = "Copy Of " & txtMaterialNameValue.Text

            lblMessage.Text &= "<br />The material has been copied. Please click save."

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnUpdateDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDrawing.Click

        Try
            lblMessage.Text = ""

            If txtDrawingNoValue.Text.Trim <> "" And txtPartNoValue.Text.Trim <> "" Then
                'PEModule.InsertDrawingBPCS(txtDrawingNoValue.Text.Trim, txtPartNoValue.Text.Trim, txtPartRevisionValue.Text.Trim)
                PEModule.InsertDrawingBPCS(txtDrawingNoValue.Text.Trim, txtPartNoValue.Text.Trim, "")
                lblMessage.Text = "<br />Drawing updated successfully."
            Else
                lblMessage.Text = "<br />Error: The DrawingNo and BPCS PartNo are required to update the DMS Drawing."
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
