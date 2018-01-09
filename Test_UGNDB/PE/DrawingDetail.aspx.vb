' ***********************************************************************************************
'
' Name:		DrawingDetail.aspx
' Purpose:	This Code Behind is for the Drawing Detail of the PE Drawings Management System App
'
' Date		Author	    
' 07/29/2008 Roderick Carlson - ported from pre-integrated version
' 10/20/2008 Roderick Carlson - added CABBV to page
' 10/23/2008 Roderick Carlson - when copying drawings, make sure to clear out customer part number
' 10/23/2008 Roderick Carlson - added Packaging Tab
' 11/10/2008 Roderick Carlson - reversed Roll Length and Width Labels
' 11/18/2008 Roderick Carlson - btnRevision and btnStep needed to copy images, only include working team members in email
'                             - when uploading images, save them to the db and refresh
' 12/03/2008 Roderick Carlson - in Databind, filter subfamily dropdown of family value exists 
' 12/08/2008 Roderick Carlson - validate text change on AMD and WMD Textboxes
' 01/14/2009 Roderick Carlson - allow page to show link to previous revision if it exists
' 03/06/2009 Roderick Carlson - Adjusted so that only certain release types can be used
' 03/09/2009 Roderick Carlson - PDE-2680 - Adjested Comments field on Packaging tab to be 300 chars. Put new char couting label on all mult-line fields
' 03/26/2009 Roderick Carlson - PDE-2692 - Adjested Comments field on Packaging tab to be 500 chars.
' 03/31/2009 Roderick Carlson - Adjusted GetVendor Parameters
' 05/28/2009 Roderick Carlson - PDE # 2715 - added vehicle year
' 07/08/2009 Roderick Carlson - PDE # 2728 - added nonrectagular and noshape to DrawingLayoutType
' 07/28/2009 Roderick Carlson - PDE # 2731 - put packaging info in normal preview - put all crystal reports in popups, saved last tab used
' 08/21/2009 Roderick Carlson - Put BPCS Part Info into GridView
' 08/28/2009 Roderick Carlson - Adjusted Vendor for UGNDB Future Vendor
' 09/03/2009 Roderick Carlson - Added Customer Program Sub Table and makde txtCustomerPartNoValue usable
' 09/17/2009 Roderick Carlson - Check to see if drawing exists
' 09/22/2009 Roderick Carlson - Added link to push customer program to subdrawings, remove duplicates and sort of sort where used list
' 10/12/2009 Roderick Carlson - PDE-2761 - Added link to upload Customer Drawing Image
' 10/14/2009 Roderick Carlson - Added Customer DrawingNo field to Insert Customer Drawing Image
' 01/05/2010 Roderick Carlson - PDE-2807 - allow subdrawings to be updated to next or previous revisions
' 01/18/2010 Roderick Carlson - PDE-2816 - added approved vendor list and unapproved vendor list
' 02/16/2010 Roderick Carlson - cleaned up data validation of AMD and WMD fields
' 02/22/2010 Roderick Carlson - PDE-2834 - Added SubDrawing Fields - Process, Equipment, and ProcessParameters, Product Technology
' 05/11/2010 Roderick Carlson - PDE-2892 - Adjusted ddFamily dropdown selected index
' 06/28/2010 Roderick Carlson - PDE-2909 - Release Type Work
' 08/25/2010 Roderick Carlson - adjusted extra isActiveBPCSOnly parameter
' 09/15/2010 Roderick Carlson - PDE-2979 - prevented child of itself being in the BOM
' 11/11/2010 Roderick Carlson - PDE-2026 - make notes fields bigger on vendor lists
' 02/03/2011 Roderick Carlson - Add Logic for Managing Parent Drawings
' 02/09/2011 Roderick Carlson - Update the copy as new step functionality:
'                                               As Step N+1 is created of a Drawing, 
'                                               then Drawing of step N would become a child drawing of step N+1.
'                                               For example, Drawing 0501-00022-1(00) is of step 1.
'                                               If step 2 is created to become 0501-00022-2(00),
'                                               then 0501-00022-1(00) would become a child of 0501-00022-2(00).
' 03/08/2011 Roderick Carlson - do not allow obsolete drawings to be shown
' 03/15/2011 Roderick Carlson - allow Admin to Edit an Issued Drawing
' 05/23/2011 Roderick Carlson - Initialize ViewState variables
' 06/06/2011 Roderick Carlson - Added Warning Popup to Edit button
' 06/29/2011 Roderick Carlson - Make sure BOM tab requires edit notes when issued drawing is edited
' 07/15/2011 Roderick Carlson - Combine Grid and Treeview of BOM into a single TreeView/HTML table component
'                             - allow multiline text boxes to show all text even after disabled
'                             - move Date Issued/Release field to upper right corner
' 09/30/2011 Roderick Carlson - Add Material Spec Links to Email Notification
' 10/18/2011 Roderick Carlson - Give engineer the option to copy the referenced material spec list when copying the drawing
'                             - Appended Release Type and Status to the Where Used Tree
' 11/28/2011 Roderick Carlson - Allow check-all functionality for deleting on BOM tab
' 12/13/2011 Roderick Carlson - Add Program Make Cascading Dropdowns
' 05/07/2012 Roderick Carlson - On the server side of code, added extra layer to not add program if no year selected
' 05/22/2012 Roderick Carlson - Simplified Where-Used tree to only show parent drawings - excluded obsolete drawings
' 12/18/2012 Roderick Carlson - PDE-3237 - AddPurchasingToNotification automatically
' 12/18/2013 LRey   	      - Replaced "BPCS Part No" to "Part No" wherever used. Incresed fields MaxLength to 40.
' 12/20/2013 LRey             - Replaced "SoldTo|CABBV" to "PartNo" wherever used. Customer DDL to OEMManufacturer.
' 01/03/2014 LRey             - Replaced "BPCSPart " to "Part" wherever used. Disabled the CustomerPartNoLookup button on the form. We are not pulling the Customer XREf from the new ERP.
' ************************************************************************************************

Partial Class DrawingDetail
    Inherits System.Web.UI.Page

    Private ParentList(-1) As String
    Private ParentCounter As Integer = 0

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False
            ViewState("isEdit") = False
            ViewState("isEnabled") = False
            ViewState("isOverride") = False

            ViewState("DisableAll") = False
            ViewState("ImageExists") = False

            ViewState("AlternativeDrawingNo") = ""
            ViewState("CopyType") = ""
            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0
            ViewState("CurrentSubDrawingRow") = 0
            ViewState("DrawingNo") = ""
            ViewState("NextDrawingNoRevision") = ""
            ViewState("PackagingImage") = ""
            ViewState("PreviousDrawingNoRevision") = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function HandleCustomerPartNoPopUps(ByVal CustomerccPartNo As String) As String

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
                "../DataMaintenance/CustomerPartNoLookUp.aspx?CustomervcPartNo=" & CustomerccPartNo
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                strWindowAttribs & "');return false;"

            HandleCustomerPartNoPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleCustomerPartNoPopUps = ""
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    '(LREY) 01/07/2014 - CheckCustomerPart relates to F3 PXREF which will not be used in the new ERP
    'Protected Sub CheckCustomerPart()

    '    Try
    '        Dim dsCustomerPartNo As DataSet

    '        dsCustomerPartNo = commonFunctions.GetCustomerPartBPCSPartRelate("", txtCustomerPartNoValue.Text.Trim, "", "", "")

    '        If commonFunctions.CheckDataSet(dsCustomerPartNo) = False Then
    '            lblMessage.Text &= "<br />WARNING: The assigned Customer Part Number " & txtCustomerPartNoValue.Text.Trim & " is NOT in the Legacy System."
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

    Protected Sub CheckParts()

        Try

            Dim dsDrawingBPCS As DataSet
            Dim dsBPCS As DataSet
            Dim iRowCounter As Integer = 0
            Dim strDrawingPartno As String = ""

            dsDrawingBPCS = PEModule.GetDrawingBPCS(ViewState("DrawingNo"))

            'get all Internal Part numbers assigned to drawing
            If commonFunctions.CheckDataSet(dsDrawingBPCS) = True Then
                For iRowCounter = 0 To dsDrawingBPCS.Tables(0).Rows.Count - 1
                    strDrawingPartno = dsDrawingBPCS.Tables(0).Rows(iRowCounter).Item("PartNo").ToString

                    If strDrawingPartno <> "" Then
                        'check if valid Internal PartNo
                        dsBPCS = commonFunctions.GetBPCSPartNo(strDrawingPartno, "")
                        If commonFunctions.CheckDataSet(dsBPCS) = False Then
                            lblMessage.Text &= "<br />WARNING: The Internal PartNo " & strDrawingPartno & " is not in the Legacy System yet."
                            'bResult = False
                        End If
                    End If

                Next
            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

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

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    'iTeamMemberID = 694 ' Adam.Miller 
                '    iTeamMemberID = 698 'Emmanuel Reymond
                'End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 35)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isEdit") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

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
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub HandleDrawingLayoutType()

        Try
            lblMessageDrawingImage.Text = ""
            btnDeleteDrawingImage.Visible = False

            If ddDrawingLayoutType.SelectedValue = "Other" Or ddDrawingLayoutType.SelectedValue = "Other-MD-Critical" Then
                lblUploadDMSImage.Visible = ViewState("isEnabled")
                btnSaveUploadImage.Visible = ViewState("isEnabled")
                uploadImage.Visible = ViewState("isEnabled")

                If ViewState("ImageExists") = True Then
                    btnDeleteDrawingImage.Visible = ViewState("isEnabled")
                Else
                    btnDeleteDrawingImage.Visible = False
                    imgDrawing.Src = ""
                End If
            Else
                lblUploadDMSImage.Visible = False
                btnSaveUploadImage.Visible = False
                uploadImage.Visible = False
                btnDeleteDrawingImage.Visible = False

                If ViewState("ImageExists") = True Then
                    lblMessageDrawingImage.Text = "WARNING: According to the drawing layout type, the uploaded image should not be used."
                End If
            End If

            If ddDrawingLayoutType.SelectedValue = "" Or ddDrawingLayoutType.SelectedValue = "Blank-Standard" Or ddDrawingLayoutType.SelectedValue = "Other" Or ddDrawingLayoutType.SelectedValue = "Non-Rectangular" Or ddDrawingLayoutType.SelectedValue = "No-Shape" Then
                lblWMDVal.Text = "Dim 1: "
                lblAMDVal.Text = "Dim 2: "
            End If

            If ddDrawingLayoutType.SelectedValue = "Blank-MD-Critical" Or ddDrawingLayoutType.SelectedValue = "Other-MD-Critical" Then
                lblWMDVal.Text = "WMD (direction of the arrow): "
                lblAMDVal.Text = "AMD                         : "
            End If

            If ddDrawingLayoutType.SelectedValue = "Rolled-Goods" Then

                lblWMDVal.Text = "See Packaging Tab"
                lblAMDVal.Text = "Roll Width: "

                txtWMDVal.Visible = False
                txtWMDVal.Text = 0
                txtWMDRef.Visible = False
                txtWMDRef.Text = ""
                txtWMDTol.Visible = False
                txtWMDTol.Text = 0
                ddWMDUnits.Visible = False
                ddWMDUnits.SelectedValue = ""
                lblWMDToleranceLabel.Visible = False
                lblPackagingRollLengthLabel.Visible = True
                txtPackagingRollLength.Visible = True
                lblPackagingRollLengthTolerance.Visible = True
                txtPackagingRollLengthTolerance.Visible = True
                txtPackagingRollLengthRef.Visible = True
                ddPackagingRollLengthUnits.Visible = True
            Else
                lblWMDVal.Visible = True
                txtWMDVal.Visible = True
                txtWMDRef.Visible = True
                txtWMDTol.Visible = True
                ddWMDUnits.Visible = True
                lblWMDToleranceLabel.Visible = True
                lblPackagingRollLengthLabel.Visible = False
                txtPackagingRollLength.Visible = False
                lblPackagingRollLengthTolerance.Visible = False
                txtPackagingRollLengthTolerance.Visible = False
                txtPackagingRollLengthRef.Visible = False
                ddPackagingRollLengthUnits.Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function HandleDrawingBPCSPopUps(ByVal ccPartNo As String, ByVal ccPartRevision As String) As String

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            Dim strPagePath As String = _
                "DrawingPartNoLookUp.aspx?vcPartNo=" & ccPartNo & "&vcPartRevision=" & ccPartRevision
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            'iBtnDrawingBPCS1.Attributes.Add("onClick", strClientScript)
            HandleDrawingBPCSPopUps = strClientScript

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingBPCSPopUps = ""
        End Try

    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Drawing Detail"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > Drawing Detail "
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
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub HandleCommentFields()

        Try
            txtComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtComments.Attributes.Add("onkeyup", "return tbCount(" + lblCommentsCharCount.ClientID + ");")
            txtComments.Attributes.Add("maxLength", "400")

            txtConstruction.Attributes.Add("onkeypress", "return tbLimit();")
            txtConstruction.Attributes.Add("onkeyup", "return tbCount(" + lblConstructionCharCount.ClientID + ");")
            txtConstruction.Attributes.Add("maxLength", "400")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotesCharCount.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "400")

            txtRevisionNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtRevisionNotes.Attributes.Add("onkeyup", "return tbCount(" + lblRevisionNotesCharCount.ClientID + ");")
            txtRevisionNotes.Attributes.Add("maxLength", "400")

            txtPackagingInstructions.Attributes.Add("onkeypress", "return tbLimit();")
            txtPackagingInstructions.Attributes.Add("onkeyup", "return tbCount(" + lblPackagingInstructionsCharCount.ClientID + ");")
            txtPackagingInstructions.Attributes.Add("maxLength", "200")

            txtPackagingIncomingInspectionComments.Attributes.Add("onkeypress", "return tbLimit();")
            txtPackagingIncomingInspectionComments.Attributes.Add("onkeyup", "return tbCount(" + lblPackagingIncomingInspectionCommentsCharCount.ClientID + ");")
            txtPackagingIncomingInspectionComments.Attributes.Add("maxLength", "500")

            txtSubDrawingNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtSubDrawingNotes.Attributes.Add("onkeyup", "return tbCount(" + lblSubDrawingNotesCharCount.ClientID + ");")
            txtSubDrawingNotes.Attributes.Add("maxLength", "100")

            txtSubDrawingProcess.Attributes.Add("onkeypress", "return tbLimit();")
            txtSubDrawingProcess.Attributes.Add("onkeyup", "return tbCount(" + lblSubDrawingProcessCharCount.ClientID + ");")
            txtSubDrawingProcess.Attributes.Add("maxLength", "100")

            txtSubDrawingProcessParameters.Attributes.Add("onkeypress", "return tbLimit();")
            txtSubDrawingProcessParameters.Attributes.Add("onkeyup", "return tbCount(" + lblSubDrawingProcessParametersCharCount.ClientID + ");")
            txtSubDrawingProcessParameters.Attributes.Add("maxLength", "100")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Session("DMS-Parent-Complete") = Nothing

            Dim iSelectedTab As Integer = 0

            If Not Page.IsPostBack Then

                InitializeViewState()

                CheckRights()

                mvDMSTabs.ActiveViewIndex = Int32.Parse(0)
                mvDMSTabs.GetActiveView()

                BindCriteria()

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If

                If HttpContext.Current.Request.QueryString("CopyType") <> "" Then
                    ViewState("CopyType") = HttpContext.Current.Request.QueryString("CopyType")
                End If

                If HttpContext.Current.Request.QueryString("CopyMaterialSpec") <> "" Then
                    cbCopyMaterialSpecList.Checked = CType(HttpContext.Current.Request.QueryString("CopyMaterialSpec"), Boolean)
                End If

                BindData()

                HandleCommentFields()

                EnableControls()

                Call lnkViewBOMTree_Click(sender, e)

                btnVoid.Attributes.Add("onclick", "if(confirm('Are you sure that you want to void this drawing?.  ')){}else{return false}")
                btnEdit.Attributes.Add("onclick", "if(confirm('Are you sure that you want edit this drawing? Using the Edit functionality might impact parts already in production. Are you sure you should not create a new drawing revision instead? ')){}else{return false}")

            Else
                If Session("DMSTabSelected") IsNot Nothing Then
                    iSelectedTab = CType(Session("DMSTabSelected"), Integer)
                End If

                EnableControls()
            End If

            If menuDMSTabs.Items(iSelectedTab).Enabled = True Then
                menuDMSTabs.Items(iSelectedTab).Selected = True
            Else
                menuDMSTabs.Items(0).Selected = True
            End If

            CreatePopUps()

            PEModule.CleanPEDMScrystalReports()

            If ViewState("DisableAll") = True Then
                DisableAll()
            End If

            Page.ClientScript.RegisterStartupScript(Me.[GetType](), "jsCheckboxes", "function CheckBOM(DrawingNo,ActionTypeChecked){  /* alert(DrawingNo);alert(ActionTypeChecked); */ if (ActionTypeChecked == false) { /* alert('uncheck'); */ /* alert((eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value).indexOf(DrawingNo)); alert((eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value).replace(DrawingNo,'')); */ eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value=eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value.replace(DrawingNo,''); } else { if ((eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value).indexOf(DrawingNo) <= 0 ) { eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value=eval(document.getElementById('" & txtSaveCheckBoxBOMDrawingNo.ClientID & "')).value + ';' + DrawingNo; } } } ", True)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub CreatePopUps()

        Try
            lnkPackagingPreview.NavigateUrl = "DrawingPackagingPreview.aspx?DrawingNo=" & ViewState("DrawingNo")

            Dim str, redirstr As String

            'generate detail drawing report popup
            str = "DrawingRevisionCompare.aspx?DrawingNo=" & ViewState("DrawingNo")
            redirstr = "javascript:void(window.open('" + str + "',0,'top=10,resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=no'));"
            btnCompareRevisions.Attributes.Add("onclick", redirstr)

            'show printer friendly view of BOM in a popup
            str = "DrawingBOMPrinterFriendlyView.aspx?DrawingNo=" & ViewState("DrawingNo") & "&DrawingName="
            If lblOldDrawingPartNameValue.Text.Trim <> "" Then
                str &= lblOldDrawingPartNameValue.Text & " | "
            End If
            str &= lblPartName.Text
            redirstr = "javascript:void(window.open('" + str + "',0,'top=0,width=1000,height=600,resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=no'));"
            btnPrinterFriendlyBOMView.Attributes.Add("onclick", redirstr)

            'search current Customer PartNo
            Dim strCustomerPartNoClientScript As String = HandleCustomerPartNoPopUps(txtCustomerPartNoValue.ClientID)
            iBtnCustomerPartNoSearch.Attributes.Add("onClick", strCustomerPartNoClientScript)

            'search current drawingno popup
            Dim strCurrentDrawingNoClientScript As String = HandleDrawingPopUps(txtSubDrawingNo.ClientID)
            ibtnSearchSubDrawing.Attributes.Add("onClick", strCurrentDrawingNoClientScript)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub DisableAll()

        txtInStep.Enabled = False
        txtInitialDimensionAndDensity.Enabled = False

        menuDMSTabs.Items(1).Enabled = False
        menuDMSTabs.Items(2).Enabled = False
        menuDMSTabs.Items(3).Enabled = False
        menuDMSTabs.Items(4).Enabled = False
        menuDMSTabs.Items(5).Enabled = False

        cbCopyMaterialSpecList.Visible = False

        btnCompareRevisions.Visible = False
        btnCopy.Visible = False
        btnDeleteDrawingImage.Visible = False
        btnFindSimilar.Visible = False
        btnPreview.Visible = False
        lnkPackagingPreview.Visible = False
        btnReset.Visible = False
        btnRevision.Visible = False
        btnSaveSubDrawing.Visible = False

        btnSaveUploadImage.Visible = False
        btnSendNotification.Visible = False
        btnStep.Visible = False
        btnVoid.Visible = False
        lnkChangeSubDrawingReleaseTypes.Visible = False
        lnkPushCustomerProgramToSubDrawing.Visible = False

        'lblCustomer.Visible = False
        'ddCustomer.Visible = False

        'lblMake.Visible = False
        'ddMake.Visible = False
        tblMakes.Visible = False

        'lblProgram.Visible = False
        'ddProgram.Visible = False

        lblYear.Visible = False
        ddYear.Visible = False

        lblEOPDate.Visible = False
        lblSOPDate.Visible = False
        lblYear.Visible = False
        lblYearMarker.Visible = False

        txtEOPDate.Visible = False
        txtSOPDate.Visible = False

        btnAddToCustomerProgram.Visible = False

        gvCustomerProgram.Visible = False

        '************* controls for Identification
        txtOldPartName.Enabled = False
        ddReleaseType.Enabled = False

        ddDesignationType.Enabled = False
        ddCommodity.Enabled = False
        ddPurchasedGood.Enabled = False
        ddFamily.Enabled = False
        ddSubFamily.Enabled = False

        txtComments.Enabled = False
        txtCustomerPartNoValue.Enabled = False
        iBtnCustomerPartNoSearch.Visible = False

    End Sub
    Private Sub EnableControls()

        Try
            If lblApprovalStatus.Text = "New" Or ViewState("CopyType") = "New" Or ViewState("DrawingNo") = "NewDrawing" Or ViewState("isOverride") = True Then
                ViewState("isEnabled") = ViewState("isAdmin")
            Else
                ViewState("isEnabled") = False
            End If

            'for new drawings, disable all but first tab
            If ViewState("DrawingNo") = "NewDrawing" Or ViewState("CopyType") = "New" Then
                Session("DMSTabSelected") = 0

                txtInStep.Enabled = ViewState("isEnabled")
                txtInitialDimensionAndDensity.Enabled = ViewState("isEnabled")

                menuDMSTabs.Items(1).Enabled = False
                menuDMSTabs.Items(2).Enabled = False
                menuDMSTabs.Items(3).Enabled = False
                menuDMSTabs.Items(4).Enabled = False
                menuDMSTabs.Items(5).Enabled = False

                cbCopyMaterialSpecList.Visible = False

                btnCompareRevisions.Visible = False
                btnCopy.Visible = False
                btnFindSimilar.Visible = False
                btnPreview.Visible = False
                lnkPackagingPreview.Visible = False
                btnReset.Visible = False
                btnRevision.Visible = False
                btnSaveSubDrawing.Visible = False
                btnSaveUploadImage.Visible = False
                btnSendNotification.Visible = False
                btnStep.Visible = False
                btnVoid.Visible = False

                lnkChangeSubDrawingReleaseTypes.Visible = False
                lnkPushCustomerProgramToSubDrawing.Visible = False

                tblMakes.Visible = False

                btnAddToCustomerProgram.Visible = False

                gvCustomerProgram.Visible = False
            Else
                txtInStep.Enabled = False
                txtInitialDimensionAndDensity.Enabled = False

                menuDMSTabs.Items(1).Enabled = True
                menuDMSTabs.Items(2).Enabled = True
                menuDMSTabs.Items(3).Enabled = True
                menuDMSTabs.Items(4).Enabled = True
                menuDMSTabs.Items(5).Enabled = True

                ddYear.Visible = ViewState("isAdmin")
                tblMakes.Visible = ViewState("isAdmin")

                lblEOPDate.Visible = ViewState("isAdmin")
                lblSOPDate.Visible = ViewState("isAdmin")
                lblYear.Visible = ViewState("isAdmin")
                lblYearMarker.Visible = ViewState("isAdmin")

                txtEOPDate.Visible = ViewState("isAdmin")
                txtSOPDate.Visible = ViewState("isAdmin")

                btnAddToCustomerProgram.Visible = ViewState("isAdmin")

                gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = ViewState("isAdmin")
            End If

            '************* controls for Identification
            txtOldPartName.Enabled = ViewState("isAdmin")

            If hlnkECINo.Text.Trim = "" Or hlnkECINo.Text.Trim = "0" Then
                ddReleaseType.Enabled = ViewState("isAdmin") 'can be updated after issued but not if a released ECI is associated to it 
            Else
                ddReleaseType.Enabled = False
            End If

            ddDesignationType.Enabled = ViewState("isAdmin")
            ddCommodity.Enabled = ViewState("isAdmin")
            ddPurchasedGood.Enabled = ViewState("isAdmin")
            ddFamily.Enabled = ViewState("isAdmin")
            ddSubFamily.Enabled = ViewState("isAdmin")
            txtComments.Enabled = ViewState("isAdmin")
            txtCustomerPartNoValue.Enabled = ViewState("isAdmin")
            '(LREY) 01/03/2013
            'iBtnCustomerPartNoSearch.Visible = ViewState("isAdmin")

            '************* controls for Specifications
            txtConstruction.Enabled = ViewState("isEnabled")
            txtThicknessUnits.Enabled = ViewState("isEnabled")
            txtDensityUnits.Enabled = ViewState("isEnabled")
            ddTolerance.Enabled = ViewState("isEnabled")
            ddDrawingLayoutType.Enabled = ViewState("isEnabled")
            cbCADavailable.Enabled = ViewState("isEnabled")
            txtThickVal.Enabled = ViewState("isEnabled")
            txtThickTol.Enabled = ViewState("isEnabled")
            txtDensityVal.Enabled = ViewState("isEnabled")
            txtDensityTol.Enabled = ViewState("isEnabled")
            txtWMDVal.Enabled = ViewState("isEnabled")
            ddWMDUnits.Enabled = ViewState("isEnabled")
            txtWMDTol.Enabled = ViewState("isEnabled")
            txtAMDVal.Enabled = ViewState("isEnabled")
            ddAMDUnits.Enabled = ViewState("isEnabled")
            txtAMDTol.Enabled = ViewState("isEnabled")
            txtNotes.Enabled = ViewState("isEnabled")
            txtRevisionNotes.Enabled = ViewState("isEnabled")

            If ViewState("DrawingNo") <> "" Then
                gvDrawingMaterialSpecRelate.Visible = True
                lblDrawingMaterialRelateTitle.Visible = True

                gvDrawingMaterialSpecRelate.Columns(gvDrawingMaterialSpecRelate.Columns.Count - 1).Visible = ViewState("isEdit")
                gvDrawingMaterialSpecRelate.ShowFooter = ViewState("isEdit")
            Else
                gvDrawingMaterialSpecRelate.Visible = False
                lblDrawingMaterialRelateTitle.Visible = False

                gvDrawingMaterialSpecRelate.Columns(gvDrawingMaterialSpecRelate.Columns.Count - 1).Visible = False
                gvDrawingMaterialSpecRelate.ShowFooter = False
            End If


            HandleDrawingLayoutType()

            HandleCADavailableCheckbox()

            '************* controls for Bill Of Materials         
            btnSaveSubDrawing.Visible = ViewState("isEnabled")
            btnManageParentDrawings.Visible = ViewState("isAdmin")

            If ViewState("CurrentSubDrawingRow") > 0 Then
                btnSaveSubDrawing.Text = "Update SubDrawing"
            Else
                btnSaveSubDrawing.Text = "Add SubDrawing"
                btnCancelEditSubDrawing.Visible = False
            End If


            '************* controls for Principals/Notifications
            ddEngineer.Enabled = ViewState("isEnabled")
            ddDrawingByEngineer.Enabled = ViewState("isEnabled")
            ddCheckedByEngineer.Enabled = ViewState("isEnabled")
            ddProcessEngineer.Enabled = ViewState("isEnabled")
            ddQualityEngineer.Enabled = ViewState("isEnabled")

            gvDrawingNotifications.Columns(1).Visible = ViewState("isEnabled")

            If gvDrawingNotifications.FooterRow IsNot Nothing Then
                gvDrawingNotifications.FooterRow.Visible = ViewState("isEnabled")
            End If

            '************* controls for Drawing Internal Part Assignment
            gvBPCSInfo.Columns(gvBPCSInfo.Columns.Count - 1).Visible = True
            'If gvBPCSInfo.FooterRow IsNot Nothing Then
            gvBPCSInfo.FooterRow.Visible = True
            'End If

            'ddVendor.Enabled = ViewState("isEdit")
            txtPackagingInstructions.Enabled = ViewState("isEdit")
            txtPackagingRollLength.Enabled = ViewState("isEdit")
            txtPackagingRollLengthTolerance.Enabled = ViewState("isEdit")
            ddPackagingRollLengthUnits.Enabled = ViewState("isEdit")
            txtPackagingIncomingInspectionComments.Enabled = ViewState("isEdit")

            btnEdit.Visible = False
            btnDeleteAllCheckedBOM.Visible = False
            If lblApprovalStatus.Text = "Issued" And ViewState("isOverride") = False Then
                btnEdit.Visible = ViewState("isAdmin")
            Else
                btnDeleteAllCheckedBOM.Visible = ViewState("isAdmin")
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
    Private Sub CancelEdit()

        Try

            lblAppendRevisionNotes.Visible = False
            txtAppendRevisionNotes.Visible = False
            rfvAppendRevisionNotes.Enabled = False
            txtAppendRevisionNotes.Text = ""

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

            Dim bFoundVendor As Boolean = False

            Dim dblAMDVal As Double
            Dim dblDensity As Double
            Dim dblPackagingRollLength As Double
            Dim dblThickness As Double
            Dim dblWMDVal As Double

            Dim ds As DataSet
            Dim dsVendor As DataSet

            Dim iCtr As Integer = 0
            Dim iFacilityCount As Integer = 0
            Dim iFacilityComma As Integer = 0
            Dim iFirstDashLocation As Integer = 0
            Dim iTempFacilityCode As Integer = 0

            Dim iHeigthBySpecificCharCount As Integer = 0
            Dim iHeightByTextFieldLength As Integer = 0
            Dim iHeightToUse As Integer = 0

            Dim strAMDUnits As String = ""
            Dim strCurrentString As String = ""
            Dim strDrawingLayoutType As String = ""
            Dim strFacility As String = ""
            Dim strReleaseType As String = ""
            Dim strRemainingString As String = ""
            Dim tmpStatus As String = ""
            Dim strTempString As String = ""
            Dim strWMDUnits As String = ""

            If ViewState("DrawingNo") <> "" And ViewState("DrawingNo") <> "NewDrawing" Then

                ds = PEModule.GetDrawing(ViewState("DrawingNo"))

                If commonFunctions.CheckDataSet(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                        If ViewState("CopyType") = "New" Then
                            lblMessageIdentification.Text = "This information is copied from Drawing " & ViewState("DrawingNo") & ". <br /> The drawing number will be generated and the information will be stored only after the save button is pressed. <br /> The image will be copied, and the copy will be renamed according to the new drawing number. "
                            lblDrawingNo.Text = ""

                            cbPreviewBOM.Visible = False
                            btnPreview.Visible = False
                            btnVoid.Visible = False
                            btnFindSimilar.Visible = False
                            btnReset.Visible = False
                            btnCompareRevisions.Visible = False

                            cbCopyMaterialSpecList.Visible = False

                            btnCopy.Visible = False
                            btnRevision.Visible = False
                            btnStep.Visible = False
                            btnSendNotification.Visible = False
                            txtInStep.Text = "1"
                        Else
                            lblDrawingNo.Text = ViewState("DrawingNo")

                            ViewState("PreviousDrawingNoRevision") = PEModule.GetPreviousDrawingRevision(ViewState("DrawingNo"))
                            ViewState("NextDrawingNoRevision") = PEModule.GetNextDrawingRevision(ViewState("DrawingNo"))

                            If ViewState("PreviousDrawingNoRevision") <> "" Then
                                lnkOpenPreviousRevision.Visible = True
                            Else
                                lnkOpenPreviousRevision.Visible = False
                            End If

                            If ViewState("NextDrawingNoRevision") <> "" Then
                                lnkOpenNextRevision.Visible = True
                            Else
                                lnkOpenNextRevision.Visible = False
                            End If

                            hlnkECINo.Text = ""
                            hlnkECINo.Visible = False
                            If ds.Tables(0).Rows(0).Item("ECINo") IsNot System.DBNull.Value Then
                                If ds.Tables(0).Rows(0).Item("ECINo") > 0 Then
                                    hlnkECINo.Text = ds.Tables(0).Rows(0).Item("ECINo").ToString
                                    hlnkECINo.NavigateUrl = "~/ECI/ECI_Preview.aspx?ECINo=" & ds.Tables(0).Rows(0).Item("ECINo").ToString
                                    hlnkECINo.Visible = True
                                End If
                            End If

                            If ds.Tables(0).Rows(0).Item("OldCustomerPartName").ToString <> "" Then
                                lblOldCustomerPartNameValue.Text = ds.Tables(0).Rows(0).Item("OldCustomerPartName").ToString.Trim
                                lblOldCustomerPartNameValue.Visible = True
                                lblOldCustomerPartNameLabel.Visible = True
                            End If

                            If ds.Tables(0).Rows(0).Item("customerpartno").ToString <> "" Then
                                txtCustomerPartNoValue.Text = ds.Tables(0).Rows(0).Item("customerpartno").ToString.Trim
                            End If

                            btnPreview.Visible = True
                            cbPreviewBOM.Visible = True
                            btnCompareRevisions.Visible = True

                            If ds.Tables(0).Rows(0).Item("UpdatedBy").ToString <> "" Then
                                lblLastUpdatedByValue.Text = ds.Tables(0).Rows(0).Item("UpdatedBy").ToString.Trim
                                lblLastUpdatedByValue.Visible = True
                                lblLastUpdatedByLabel.Visible = True
                                lblLastUpdatedOnValue.Text = ds.Tables(0).Rows(0).Item("UpdatedOn").ToString.Trim
                                lblLastUpdatedOnLabel.Visible = True
                                lblLastUpdatedOnValue.Visible = True
                            End If

                            tmpStatus = ds.Tables(0).Rows(0).Item("approvalstatus").ToString
                            lblApprovalStatusID.Text = ds.Tables(0).Rows(0).Item("approvalstatus").ToString
                            'lblStatusValue.Text = ds.Tables(0).Rows(0).Item("approvalstatusdecoded").ToString                          

                            'btnEdit.Visible = False
                            Select Case tmpStatus
                                Case "A", "I"
                                    'if item has been final approved, disable all buttons                                                    
                                    lblMessageIdentification.Text &= "** This drawing has been issued.  No changes can be made, except for Customer Information, Legacy Information, Packaging Information, and Release Type."
                                    ' btnEdit.Visible = ViewState("isAdmin")
                                    'Case "R", "W", "N", "P"
                            End Select

                            lblApprovalStatus.Text = ds.Tables(0).Rows(0).Item("approvalstatusdecoded").ToString
                            lblSubmitApproval.Text = ds.Tables(0).Rows(0).Item("submittedOn").ToString
                            txtInStep.Text = ds.Tables(0).Rows(0).Item("insteptracking").ToString
                        End If ' copying new drawing or working with existing drawing

                        iFirstDashLocation = InStr(ViewState("DrawingNo"), "-")
                        txtInitialDimensionAndDensity.Text = Mid$(ViewState("DrawingNo"), iFirstDashLocation + 1, 2)

                        txtDensityUnits.Text = ds.Tables(0).Rows(0).Item("DensityUnits").ToString.Trim
                        txtThicknessUnits.Text = ds.Tables(0).Rows(0).Item("ThicknessUnits").ToString.Trim

                        txtConstruction.Text = ds.Tables(0).Rows(0).Item("construction").ToString.Trim
                        iHeightToUse = 150

                        If txtConstruction.Text.Trim <> "" And txtConstruction.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0

                            'count all characters
                            iHeigthBySpecificCharCount = (txtConstruction.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtConstruction.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If
                            End If
                        End If
                        txtConstruction.Height = iHeightToUse

                        strDrawingLayoutType = ds.Tables(0).Rows(0).Item("DrawingLayoutType").ToString

                        If ds.Tables(0).Rows(0).Item("CADavailable") IsNot System.DBNull.Value Then
                            cbCADavailable.Checked = ds.Tables(0).Rows(0).Item("CADavailable")
                        End If

                        ViewState("AlternativeDrawingNo") = ""
                        Select Case strDrawingLayoutType
                            Case "Blank-Standard"
                                ViewState("AlternativeDrawingNo") = "blankstandard"
                            Case "Rolled-Goods"
                                ViewState("AlternativeDrawingNo") = "rolledgoods"
                            Case "Blank-MD-Critical"
                                ViewState("AlternativeDrawingNo") = "blankmdcritical"
                            Case "Non-Rectangular"
                                ViewState("AlternativeDrawingNo") = "nonrectangularshape"
                            Case "No-Shape"
                                ViewState("AlternativeDrawingNo") = "noshape"
                        End Select

                        ddDrawingLayoutType.SelectedValue = strDrawingLayoutType

                        If ds.Tables(0).Rows(0).Item("wmdvalue") IsNot System.DBNull.Value Then
                            dblWMDVal = ds.Tables(0).Rows(0).Item("wmdvalue")
                            If dblWMDVal > 0 Then

                                txtWMDVal.Text = dblWMDVal.ToString("n4").Replace(",", "")

                                If ds.Tables(0).Rows(0).Item("wmdunits") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("wmdunits").ToString <> "" Then
                                        ddWMDUnits.SelectedValue = ds.Tables(0).Rows(0).Item("wmdunits").ToString

                                        If ds.Tables(0).Rows(0).Item("wmdunits").ToString = "m" Then
                                            txtWMDRef.Text = CStr(Math.Round(dblWMDVal * 3.2808399, 2)) & " feet"
                                        End If
                                        If ds.Tables(0).Rows(0).Item("wmdunits").ToString = "mm" Then
                                            txtWMDRef.Text = CStr(Math.Round(dblWMDVal * 0.0393700787, 2)) & " inches"
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        txtWMDTol.Text = ds.Tables(0).Rows(0).Item("wmdtolerance").ToString

                        If ds.Tables(0).Rows(0).Item("amdvalue") IsNot System.DBNull.Value Then
                            dblAMDVal = ds.Tables(0).Rows(0).Item("amdvalue")
                            If dblAMDVal > 0 Then
                                txtAMDVal.Text = dblAMDVal.ToString("n4").Replace(",", "")

                                If ds.Tables(0).Rows(0).Item("amdunits") IsNot System.DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("amdunits").ToString <> "" Then
                                        ddAMDUnits.SelectedValue = ds.Tables(0).Rows(0).Item("amdunits").ToString

                                        If ds.Tables(0).Rows(0).Item("amdunits").ToString = "m" Then
                                            txtAMDRef.Text = CStr(Math.Round(dblAMDVal * 3.2808399, 2)) & " feet"
                                        End If
                                        If ds.Tables(0).Rows(0).Item("amdunits").ToString = "mm" Then
                                            txtAMDRef.Text = CStr(Math.Round(dblAMDVal * 0.0393700787, 2)) & " inches"
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        txtAMDTol.Text = ds.Tables(0).Rows(0).Item("amdtolerance").ToString

                        If ds.Tables(0).Rows(0).Item("thicknessvalue") IsNot System.DBNull.Value Then
                            dblThickness = ds.Tables(0).Rows(0).Item("thicknessvalue")
                            If dblThickness > 0 Then
                                txtThickVal.Text = dblThickness.ToString("n4").Replace(",", "")
                            End If
                        End If

                        txtThickTol.Text = ds.Tables(0).Rows(0).Item("thicknesstolerance").ToString

                        If ds.Tables(0).Rows(0).Item("densityvalue") IsNot System.DBNull.Value Then
                            dblDensity = ds.Tables(0).Rows(0).Item("densityvalue")
                            If dblDensity > 0 Then
                                txtDensityVal.Text = dblDensity.ToString("n4").Replace(",", "")
                            End If
                        End If

                        txtDensityTol.Text = ds.Tables(0).Rows(0).Item("densitytolerance").ToString.Trim
                        txtNotes.Text = ds.Tables(0).Rows(0).Item("notes").ToString.Trim

                        iHeightToUse = 150
                        If txtNotes.Text.Trim <> "" And txtNotes.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0

                            'count all characters
                            iHeigthBySpecificCharCount = (txtNotes.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtNotes.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If
                            End If
                        End If
                        txtNotes.Height = iHeightToUse

                        txtComments.Text = ds.Tables(0).Rows(0).Item("Comments").ToString.Trim
                        iHeightToUse = 150
                        If txtComments.Text.Trim <> "" And txtComments.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0

                            'count all characters
                            iHeigthBySpecificCharCount = (txtComments.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtComments.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If


                            End If
                        End If
                        txtComments.Height = iHeightToUse

                        txtRevisionNotes.Text = ds.Tables(0).Rows(0).Item("revisionNotes").ToString.Trim
                        iHeightToUse = 150

                        If txtRevisionNotes.Text.Trim <> "" And txtRevisionNotes.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0

                            'count all characters
                            iHeigthBySpecificCharCount = (txtRevisionNotes.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtRevisionNotes.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If
                            End If
                        End If
                        txtRevisionNotes.Height = iHeightToUse

                        If ds.Tables(0).Rows(0).Item("EngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("EngineerID") > 0 Then
                                ddEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("EngineerID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("DrawingByEngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("DrawingByEngineerID") > 0 Then
                                ddDrawingByEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("DrawingByEngineerID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("CheckedByEngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CheckedByEngineerID") > 0 Then
                                ddCheckedByEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("CheckedByEngineerID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("ProcessEngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ProcessEngineerID") > 0 Then
                                ddProcessEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("ProcessEngineerID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("QualityEngineerID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("QualityEngineerID") > 0 Then
                                ddQualityEngineer.SelectedValue = ds.Tables(0).Rows(0).Item("QualityEngineerID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("ToleranceID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ToleranceID") > 0 Then
                                ddTolerance.SelectedValue = ds.Tables(0).Rows(0).Item("ToleranceID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("DesignationType") IsNot System.DBNull.Value Then
                            If Trim(ds.Tables(0).Rows(0).Item("DesignationType")) <> "" Then
                                ddDesignationType.SelectedValue = ds.Tables(0).Rows(0).Item("DesignationType")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("CommodityID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CommodityID") > 0 Then
                                ddCommodity.SelectedValue = ds.Tables(0).Rows(0).Item("CommodityID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("ProductTechnologyID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ProductTechnologyID") > 0 Then
                                ddProductTechnology.SelectedValue = ds.Tables(0).Rows(0).Item("ProductTechnologyID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("PurchasedGoodID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PurchasedGoodID") > 0 Then
                                ddPurchasedGood.SelectedValue = ds.Tables(0).Rows(0).Item("PurchasedGoodID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("FamilyID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("FamilyID") > 0 Then
                                ddFamily.SelectedValue = ds.Tables(0).Rows(0).Item("FamilyID")

                                'filter subfamily dropdown choices if a family exists
                                Dim dsFamily As DataSet
                                Dim iFamilyID As Integer = 0

                                If ddFamily.SelectedIndex > 0 Then
                                    iFamilyID = ddFamily.SelectedValue
                                End If

                                dsFamily = commonFunctions.GetSubFamily(iFamilyID)
                                If commonFunctions.CheckDataSet(dsFamily) = True Then
                                    ddSubFamily.DataSource = dsFamily
                                    'ddSubFamily.DataTextField = dsFamily.Tables(0).Columns("subFamilyName").ColumnName
                                    ddSubFamily.DataTextField = dsFamily.Tables(0).Columns("ddSubFamilyName").ColumnName
                                    ddSubFamily.DataValueField = dsFamily.Tables(0).Columns("subFamilyID").ColumnName
                                    ddSubFamily.DataBind()
                                    ddSubFamily.Items.Insert(0, "")
                                End If

                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("SubFamilyID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("SubFamilyID") > 0 Then
                                ddSubFamily.SelectedValue = ds.Tables(0).Rows(0).Item("SubFamilyID")
                            End If
                        End If

                        If Trim(ds.Tables(0).Rows(0).Item("OldPartName").ToString) <> "" Then
                            lblOldDrawingPartNameValue.Text = ds.Tables(0).Rows(0).Item("OldPartName")
                            lblOldDrawingPartNameValue.Visible = True
                            txtOldPartName.Text = ds.Tables(0).Rows(0).Item("OldPartName").ToString.Trim
                        End If

                        If Trim(ds.Tables(0).Rows(0).Item("OldCategoryName").ToString) <> "" Then
                            lblOldCategoryTypeValue.Text = ds.Tables(0).Rows(0).Item("OldCategoryName").ToString.Trim
                            lblOldCategoryTypeValue.Visible = True
                            lblOldCategoryTypeLabel.Visible = True
                        End If

                        If ds.Tables(0).Rows(0).Item("notificationsent") IsNot System.DBNull.Value Then
                            lblNotification.Text = IIf(ds.Tables(0).Rows(0).Item("notificationsent") = "1/1/1900", "", ds.Tables(0).Rows(0).Item("notificationsent"))
                        End If

                        If ds.Tables(0).Rows(0).Item("ReleaseTypeID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ReleaseTypeID") > 0 Then
                                ddReleaseType.SelectedValue = ds.Tables(0).Rows(0).Item("ReleaseTypeID")
                            End If
                        End If

                        lblPartNo.Text = ds.Tables(0).Rows(0).Item("PartNo").ToString.Trim
                        lblPartRevision.Text = ds.Tables(0).Rows(0).Item("Part_Revision").ToString.Trim
                        lblPartName.Text = ds.Tables(0).Rows(0).Item("PartName").ToString.Trim

                        ddVendor.Visible = False
                        lblVendorLabel.Visible = False
                        If ds.Tables(0).Rows(0).Item("UGNDBVendorID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("UGNDBVendorID") > 0 Then
                                ddVendor.SelectedValue = ds.Tables(0).Rows(0).Item("UGNDBVendorID")
                                ddVendor.Visible = True
                                lblVendorLabel.Visible = True
                            End If
                        End If

                        txtPackagingInstructions.Text = ds.Tables(0).Rows(0).Item("PackagingInstructions").ToString.Trim

                        If txtPackagingInstructions.Text.Trim <> "" And txtPackagingInstructions.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0
                            iHeightToUse = 200

                            'count all characters
                            iHeigthBySpecificCharCount = (txtPackagingInstructions.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtPackagingInstructions.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If

                                txtPackagingInstructions.Height = iHeightToUse
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("PackagingRollLength") IsNot System.DBNull.Value Then
                            dblPackagingRollLength = ds.Tables(0).Rows(0).Item("PackagingRollLength")
                            If dblPackagingRollLength > 0 Then
                                txtPackagingRollLength.Text = dblPackagingRollLength.ToString("n4").Replace(",", "")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("PackagingRollUnits") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PackagingRollUnits").ToString.Trim <> "" Then
                                ddPackagingRollLengthUnits.SelectedValue = ds.Tables(0).Rows(0).Item("PackagingRollUnits").ToString.Trim

                                If ds.Tables(0).Rows(0).Item("PackagingRollUnits").ToString = "m" Then
                                    txtPackagingRollLengthRef.Text = CStr(Math.Round(dblPackagingRollLength * 3.2808399, 2)) & " feet"
                                End If

                                If ds.Tables(0).Rows(0).Item("PackagingRollUnits").ToString = "mm" Then
                                    txtPackagingRollLengthRef.Text = CStr(Math.Round(dblPackagingRollLength * 0.0393700787, 2)) & " inches"
                                End If
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("PackagingRollTolerance") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PackagingRollTolerance").ToString.Trim <> "" Then
                                txtPackagingRollLengthTolerance.Text = ds.Tables(0).Rows(0).Item("PackagingRollTolerance").ToString.Trim
                            End If
                        End If

                        txtPackagingIncomingInspectionComments.Text = ds.Tables(0).Rows(0).Item("PackagingIncomingInspectionComments").ToString.Trim

                        If txtPackagingIncomingInspectionComments.Text.Trim <> "" And txtPackagingIncomingInspectionComments.Text.Trim.Length <> 0 Then

                            iHeigthBySpecificCharCount = 0
                            iHeightByTextFieldLength = 0
                            iHeightToUse = 200

                            'count all characters
                            iHeigthBySpecificCharCount = (txtPackagingIncomingInspectionComments.Text.Trim.Length / 80) * 20
                            'count the number of carriage return line feeds
                            iHeightByTextFieldLength = (UBound(Split(txtPackagingIncomingInspectionComments.Text, vbCrLf)) * 40)

                            'if calculated heights are greater than 200 use the greater of the 2
                            If iHeigthBySpecificCharCount > iHeightToUse Or iHeightByTextFieldLength > iHeightToUse Then
                                If iHeigthBySpecificCharCount > iHeightByTextFieldLength Then
                                    iHeightToUse = iHeigthBySpecificCharCount
                                Else
                                    iHeightToUse = iHeightByTextFieldLength
                                End If

                                txtPackagingIncomingInspectionComments.Height = iHeightToUse
                            End If
                        End If

                        ViewState("PackagingImage") = ""
                        lblVendorTip.Text = ""

                        dsVendor = PEModule.GetDrawingApprovedVendor(ViewState("DrawingNo"))
                        If commonFunctions.CheckDataSet(dsVendor) = True Then
                            bFoundVendor = True
                        End If

                        dsVendor = PEModule.GetDrawingApprovedVendor(ViewState("DrawingNo"))
                        If commonFunctions.CheckDataSet(dsVendor) = True Then
                            bFoundVendor = True
                        End If

                        dsVendor = PEModule.GetDrawingUnapprovedVendor(ViewState("DrawingNo"))
                        If commonFunctions.CheckDataSet(dsVendor) = True Then
                            bFoundVendor = True
                        End If

                        If bFoundVendor = True _
                            Or txtPackagingInstructions.Text.Trim <> "" _
                            Or txtPackagingRollLength.Text.Trim <> "" _
                            Or txtPackagingRollLengthRef.Text.Trim <> "" _
                            Or txtPackagingRollLengthRef.Text.Trim <> "" _
                            Or txtPackagingRollLengthTolerance.Text.Trim <> "" _
                            Or txtPackagingIncomingInspectionComments.Text.Trim <> "" Then

                            menuDMSTabs.Items(5).ImageUrl = "~/images/asterick_blue.gif"
                            lblVendorTip.Text = "Product Engineering has approved vendor name(s)/material(s) which meet this print.  See separate list.  Only approved listed materials may be used."
                        End If

                        'Submit button should be disabled, if the drawing is pending, approved, rejected or waived
                        If ds.Tables(0).Rows(0).Item("approvalstatus").ToString = "N" And ViewState("isAdmin") = True Then
                            btnSendNotification.Visible = True

                            btnVoid.Visible = True
                        Else
                            'once drawing has been submitted, cannot resubmit or add approvers
                            btnSendNotification.Visible = False
                        End If

                        btnSave.Visible = ViewState("isEdit")
                        btnSaveIdentification.Visible = ViewState("isEdit")
                        btnReset.Visible = ViewState("isEdit")

                        lnkChangeSubDrawingReleaseTypes.Visible = ViewState("isAdmin")
                        lnkPushCustomerProgramToSubDrawing.Visible = ViewState("isAdmin")

                        cbCopyMaterialSpecList.Visible = ViewState("isAdmin")
                        btnCopy.Visible = ViewState("isAdmin")

                        'only run these queries if user is admin
                        If ViewState("isAdmin") = True Then
                            Dim dsDrawingMaxRevision As DataSet = PEModule.GetDrawingMaxRevision(ViewState("DrawingNo"))
                            If commonFunctions.CheckDataSet(dsDrawingMaxRevision) = True Then
                                'only show this button is max revision
                                If ViewState("DrawingNo") = dsDrawingMaxRevision.Tables(0).Rows(0).Item("MaxRevisionDrawing").ToString Then
                                    btnRevision.Visible = True
                                End If
                            End If

                            Dim dsDrawingMaxStep As DataSet = PEModule.GetDrawingMaxStep(ViewState("DrawingNo"))
                            If commonFunctions.CheckDataSet(dsDrawingMaxStep) = True Then
                                If ViewState("DrawingNo") = dsDrawingMaxStep.Tables(0).Rows(0).Item("MaxStepDrawing").ToString Then
                                    'only show this button is max step
                                    If txtInStep.Text.Trim <> "" Then
                                        If CType(txtInStep.Text.Trim, Integer) < 9 Then
                                            btnStep.Visible = True
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        'get image detail
                        ViewState("ImageExists") = False
                        ds = PEModule.GetDrawingImages(ViewState("DrawingNo"), ViewState("AlternativeDrawingNo"))
                        If commonFunctions.CheckDataSet(ds) = True Then

                            If ViewState("AlternativeDrawingNo") = "" Then
                                ViewState("ImageExists") = True
                            End If

                            imgDrawing.Src = "DrawingDisplayImage.aspx?DrawingNo=" & ViewState("DrawingNo") & "&AlternativeDrawingNo=" & ViewState("AlternativeDrawingNo")

                        End If 'ds result is nothing    

                        ds = PEModule.GetDrawingCustomerImages(ViewState("DrawingNo"))
                        If commonFunctions.CheckDataSet(ds) = True Then
                            hlnkCustomerImage.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & ViewState("DrawingNo")
                            txtCustomerDrawingNo.Text = ds.Tables(0).Rows(0).Item("CustomerDrawingNo").ToString.Trim
                        End If

                        CheckParts()

                        '(LREY) 01/07/2014
                        'CheckCustomerPart()

                    Else
                        lblMessage.Text &= "<br />Error: The drawing has been set to obsolete."
                        gvCustomerProgram.Visible = False
                        ViewState("DisableAll") = True
                    End If

                Else
                    lblMessage.Text = "Error: The DMS Drawing " & ViewState("DrawingNo") & " does not exist."
                    ViewState("DisableAll") = True
                End If
            Else
                btnSave.Visible = ViewState("isEdit") 'ViewState("isAdmin")
            End If ' no drawing selected, creating new drawing
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageBillOfMaterials.Text = ""
            lblMessageBillOfMaterialsBottom.Text = ""
            lblMessageBPCSassignments.Text = ""
            lblMessageCustomerImageUpload.Text = ""
            lblMessageDMSImageUpload.Text = ""
            lblMessageIdentification.Text = ""
            lblMessagePackaging.Text = ""
            lblMessagePackagingBottom.Text = ""
            lblMessagePrincipals.Text = ""
            lblMessageSpecifications.Text = ""
            lblDrawingWhereUsedMessage.Text = ""
            lblMessageVendor.Text = ""
            lblMessageVendorBottom.Text = ""

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

            'ds = commonFunctions.GetOEMManufacturer("")
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddCustomer.DataSource = ds
            '    ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
            '    ddCustomer.DataBind()
            '    ddCustomer.Items.Insert(0, "")
            'End If

            ds = commonFunctions.GetCommodity(0, "", "", 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCommodity.DataSource = ds
                ddCommodity.DataTextField = ds.Tables(0).Columns("ddCommodityByClassification").ColumnName
                ddCommodity.DataValueField = ds.Tables(0).Columns("CommodityID").ColumnName
                ddCommodity.DataBind()
                ddCommodity.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetDesignationType()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddDesignationType.DataSource = ds
                ddDesignationType.DataTextField = ds.Tables(0).Columns("ddDesignationTypeName").ColumnName.ToString()
                ddDesignationType.DataValueField = ds.Tables(0).Columns("DesignationType").ColumnName
                ddDesignationType.DataBind()
                ddDesignationType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPurchasedGood("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPurchasedGood.DataSource = ds
                ddPurchasedGood.DataTextField = ds.Tables(0).Columns("ddPurchasedGoodName").ColumnName
                ddPurchasedGood.DataValueField = ds.Tables(0).Columns("PurchasedGoodID").ColumnName
                ddPurchasedGood.DataBind()
                ddPurchasedGood.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingTolerance(0, "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddTolerance.DataSource = ds
                ddTolerance.DataTextField = ds.Tables(0).Columns("ddtoleranceName").ColumnName
                ddTolerance.DataValueField = ds.Tables(0).Columns("toleranceID").ColumnName
                ddTolerance.DataBind()
                ddTolerance.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetFamily()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddFamily.DataSource = ds
                ddFamily.DataTextField = ds.Tables(0).Columns("ddFamilyName").ColumnName
                ddFamily.DataValueField = ds.Tables(0).Columns("familyID").ColumnName
                ddFamily.DataBind()
                ddFamily.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName
                ddSubFamily.DataValueField = ds.Tables(0).Columns("subFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetYear("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddYear.DataSource = ds
                ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
                ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName
                ddYear.DataBind()
                ddYear.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetProductTechnology(0)
            If (commonFunctions.CheckDataSet(ds) = True) Then
                ddProductTechnology.DataSource = ds
                ddProductTechnology.DataTextField = ds.Tables(0).Columns("ddProductTechnologyName").ColumnName
                ddProductTechnology.DataValueField = ds.Tables(0).Columns("ProductTechnologyID").ColumnName
                ddProductTechnology.DataBind()
                ddProductTechnology.Items.Insert(0, "")
            End If

            ds = PEModule.GetDrawingReleaseTypeList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddReleaseType.DataSource = ds
                ddReleaseType.DataTextField = ds.Tables(0).Columns("ddReleaseTypeName").ColumnName
                ddReleaseType.DataValueField = ds.Tables(0).Columns("ReleaseTypeID").ColumnName
                ddReleaseType.DataBind()
                ddReleaseType.Items.Insert(0, "")
            End If

            'select * from Subscriptions_Maint
            'bind existing team member list for DMS General Engineer Subscription to Engineer Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(30)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEngineer.DataSource = ds
                ddEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddEngineer.DataBind()
                ddEngineer.Items.Insert(0, "")
            End If

            'bind existing team member list for DMS Drawing By Engineer Subscription to Drawing By Engineer Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(25)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddDrawingByEngineer.DataSource = ds
                ddDrawingByEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddDrawingByEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddDrawingByEngineer.DataBind()
                ddDrawingByEngineer.Items.Insert(0, "")
            End If

            'bind existing team member list for DMS Checked By Engineer Subscription to Drawing By Engineer Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(26)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCheckedByEngineer.DataSource = ds
                ddCheckedByEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddCheckedByEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddCheckedByEngineer.DataBind()
                ddCheckedByEngineer.Items.Insert(0, "")
            End If

            'bind existing team member list for DMS Process Engineer Subscription to Drawing By Engineer Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(28)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddProcessEngineer.DataSource = ds
                ddProcessEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddProcessEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddProcessEngineer.DataBind()
                ddProcessEngineer.Items.Insert(0, "")
            End If

            'bind existing team member list for DMS Quality Engineer Subscription to Drawing By Engineer Dropdown
            ds = commonFunctions.GetTeamMemberBySubscription(29)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddQualityEngineer.DataSource = ds
                ddQualityEngineer.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddQualityEngineer.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddQualityEngineer.DataBind()
                ddQualityEngineer.Items.Insert(0, "")
            End If

            'bind existing data to drop down Customer control for selection criteria for search
            ds = commonFunctions.GetUGNDBVendor(0, "", "", False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddVendor.DataSource = ds
                ddVendor.DataTextField = ds.Tables(0).Columns("ddVendorName").ColumnName
                ddVendor.DataValueField = ds.Tables(0).Columns("UGNDBVendorID").ColumnName
                ddVendor.DataBind()
                ddVendor.Items.Insert(0, "")
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
    Protected Sub ClearFields()

        Try
            lblDrawingNo.Text = ""
            lblSubmitApproval.Text = ""
            txtInStep.Text = ""
            txtDensityUnits.Text = ""
            txtThicknessUnits.Text = ""
            txtConstruction.Text = ""
            txtWMDVal.Text = ""
            txtWMDTol.Text = ""
            txtAMDVal.Text = ""
            txtAMDTol.Text = ""
            txtThickVal.Text = ""
            txtThickTol.Text = ""
            txtDensityVal.Text = ""
            txtDensityTol.Text = ""
            txtNotes.Text = ""
            txtComments.Text = ""
            txtRevisionNotes.Text = ""
            ddEngineer.SelectedIndex = -1
            ddDrawingByEngineer.SelectedIndex = -1
            ddCheckedByEngineer.SelectedIndex = -1
            ddTolerance.SelectedIndex = -1
            ddProgram.SelectedIndex = -1
            ddFamily.SelectedIndex = -1
            ddSubFamily.SelectedIndex = -1
            lblNotification.Text = ""
            ddVendor.SelectedIndex = -1
            txtPackagingInstructions.Text = ""
            txtPackagingRollLength.Text = ""
            txtPackagingIncomingInspectionComments.Text = ""

            ClearMessages()

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
            ClearMessages()

            Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub AddPurchasingToNotification()

        Try

            Dim ds As DataSet
            Dim iFamily As Integer = 0
            Dim iRowCount As Integer = 0

            'add purchasing team member if workflow assignment exists, family is selected, and the drawing is not issued yet
            If ddFamily.SelectedIndex > 0 And btnSendNotification.Visible = True Then
                iFamily = ddFamily.SelectedValue

                ds = commonFunctions.GetWorkFlowFamilyPurchasingAssignments(0, iFamily)
                If commonFunctions.CheckDataSet(ds) = True Then

                    For iRowCount = 0 To ds.Tables(0).Rows.Count - 1

                        If ds.Tables(0).Rows(iRowCount).Item("TeamMemberID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(iRowCount).Item("TeamMemberID") > 0 Then 'default family found
                                PEModule.InsertDrawingNotification(ViewState("DrawingNo"), ds.Tables(0).Rows(iRowCount).Item("TeamMemberID"))
                            End If
                        End If
                    Next

                    gvDrawingNotifications.DataBind()

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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveIdentification.Click

        Try
            ClearMessages()

            Dim bNewPart As Boolean = False
            Dim bPartsExist As Boolean = False
            Dim bCustomerPartsExist As Boolean = False
            Dim strOriginalDrawingNo As String = ""

            CheckParts()

            '(LREY) 01/07/2014
            'CheckCustomerPart()

            'save information to database
            'if new, then generate new drawing number           
            If ViewState("DrawingNo") = "NewDrawing" Or ViewState("CopyType") = "New" Then

                If ViewState("CopyType") = "New" Then
                    Session("DMSTabSelected") = 0

                    strOriginalDrawingNo = ViewState("DrawingNo")
                End If

                ViewState("DrawingNo") = GenerateDrawingNo()

                If strOriginalDrawingNo <> "" Then
                    copyNotificationList(strOriginalDrawingNo)
                    CopySubDrawings(strOriginalDrawingNo)
                    CopyImage(strOriginalDrawingNo)
                    'do not copy customer program list - Mark Rimkus September 16, 2009
                    'PEModule.CopyDrawingCustomerProgram(ViewState("DrawingNo"), strOriginalDrawingNo)
                    gvCustomerProgram.DataBind()

                    PEModule.CopyDrawingApprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)
                    gvDrawingApprovedVendor.DataBind()

                    PEModule.CopyDrawingUnapprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)
                    gvDrawingUnapprovedVendor.DataBind()

                    If cbCopyMaterialSpecList.Checked = True Then
                        PEModule.CopyDrawingDrawingMaterialSpecRelateByDrawing(ViewState("DrawingNo"), strOriginalDrawingNo)
                    End If
                End If

                bNewPart = True
            End If

            AddPurchasingToNotification()

            If bNewPart = True Then

                'insert new drawing
                PEModule.InsertDrawing(ViewState("DrawingNo"), txtOldPartName.Text, IIf(ddReleaseType.SelectedIndex <= 0, 0, ddReleaseType.SelectedValue), _
                txtInStep.Text.Trim, "", 0, txtCustomerPartNoValue.Text, _
                ddDesignationType.SelectedValue, cbCADavailable.Checked, _
                IIf(ddSubFamily.SelectedIndex <= 0, 0, ddSubFamily.SelectedValue), _
                IIf(ddProductTechnology.SelectedIndex <= 0, 0, ddProductTechnology.SelectedValue), _
                IIf(ddCommodity.SelectedIndex <= 0, 0, ddCommodity.SelectedValue), _
                IIf(ddPurchasedGood.SelectedIndex <= 0, 0, ddPurchasedGood.SelectedValue), _
                IIf(ddEngineer.SelectedIndex <= 0, 0, ddEngineer.SelectedValue), _
                IIf(ddDrawingByEngineer.SelectedIndex <= 0, 0, ddDrawingByEngineer.SelectedValue), _
                IIf(ddCheckedByEngineer.SelectedIndex <= 0, 0, ddCheckedByEngineer.SelectedValue), _
                IIf(ddProcessEngineer.SelectedIndex <= 0, 0, ddProcessEngineer.SelectedValue), _
                IIf(ddQualityEngineer.SelectedIndex <= 0, 0, ddQualityEngineer.SelectedValue), _
                IIf(txtDensityVal.Text.Trim = "", 0, txtDensityVal.Text.Trim), txtDensityUnits.Text.Trim, txtDensityTol.Text.Trim, _
                IIf(txtThickVal.Text.Trim = "", 0, txtThickVal.Text.Trim), txtThicknessUnits.Text.Trim, txtThickTol.Text.Trim, _
                ddDrawingLayoutType.SelectedValue, IIf(txtAMDVal.Text.Trim = "", 0, txtAMDVal.Text.Trim), ddAMDUnits.SelectedValue, _
                txtAMDTol.Text.Trim, _
                IIf(txtWMDVal.Text.Trim = "", 0, txtWMDVal.Text.Trim), ddWMDUnits.SelectedValue, txtWMDTol.Text.Trim, _
                IIf(ddTolerance.SelectedIndex <= 0, 0, ddTolerance.SelectedValue), _
                txtConstruction.Text.Trim, txtRevisionNotes.Text.Trim, txtNotes.Text.Trim, _
                txtComments.Text.Trim)

                Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & "&CopyMaterialSpec=" & cbCopyMaterialSpecList.Checked, False)
            Else
                'Update Drawing
                PEModule.UpdateDrawing(ViewState("DrawingNo"), txtOldPartName.Text, IIf(ddReleaseType.SelectedIndex <= 0, 0, ddReleaseType.SelectedValue), _
                txtInStep.Text.Trim, "", 0, txtCustomerPartNoValue.Text, ddDesignationType.SelectedValue, cbCADavailable.Checked, _
                IIf(ddSubFamily.SelectedIndex <= 0, 0, ddSubFamily.SelectedValue), _
                IIf(ddProductTechnology.SelectedIndex <= 0, 0, ddProductTechnology.SelectedValue), _
                IIf(ddCommodity.SelectedIndex <= 0, 0, ddCommodity.SelectedValue), _
                IIf(ddPurchasedGood.SelectedIndex <= 0, 0, ddPurchasedGood.SelectedValue), _
                IIf(ddEngineer.SelectedIndex <= 0, 0, ddEngineer.SelectedValue), _
                IIf(ddDrawingByEngineer.SelectedIndex <= 0, 0, ddDrawingByEngineer.SelectedValue), _
                IIf(ddCheckedByEngineer.SelectedIndex <= 0, 0, ddCheckedByEngineer.SelectedValue), _
                IIf(ddProcessEngineer.SelectedIndex <= 0, 0, ddProcessEngineer.SelectedValue), _
                IIf(ddQualityEngineer.SelectedIndex <= 0, 0, ddQualityEngineer.SelectedValue), _
                IIf(txtDensityVal.Text.Trim = "", 0, txtDensityVal.Text.Trim), txtDensityUnits.Text.Trim, txtDensityTol.Text.Trim, _
                IIf(txtThickVal.Text.Trim = "", 0, txtThickVal.Text.Trim), txtThicknessUnits.Text.Trim, txtThickTol.Text.Trim, _
                ddDrawingLayoutType.SelectedValue, IIf(txtAMDVal.Text.Trim = "", 0, txtAMDVal.Text.Trim), ddAMDUnits.SelectedValue, _
                txtAMDTol.Text.Trim, _
                IIf(txtWMDVal.Text.Trim = "", 0, txtWMDVal.Text.Trim), ddWMDUnits.SelectedValue, txtWMDTol.Text.Trim, _
                IIf(ddTolerance.SelectedIndex <= 0, 0, ddTolerance.SelectedValue), _
                txtConstruction.Text.Trim, txtRevisionNotes.Text.Trim, txtNotes.Text.Trim, _
                txtComments.Text.Trim, IIf(ddVendor.SelectedIndex <= 0, 0, ddVendor.SelectedValue), txtPackagingInstructions.Text, _
                IIf(txtPackagingRollLength.Text.Trim = "", 0, txtPackagingRollLength.Text.Trim), txtPackagingRollLengthTolerance.Text, _
                IIf(ddPackagingRollLengthUnits.SelectedIndex <= 0, "", ddPackagingRollLengthUnits.SelectedValue), txtPackagingIncomingInspectionComments.Text)

                If txtAppendRevisionNotes.Text.Trim <> "" Then
                    PEModule.UpdateDrawingAppendRevisionNotes(ViewState("DrawingNo"), vbNewLine & txtAppendRevisionNotes.Text.Trim)
                End If

                If cbCADavailable.Checked Then
                    If hlnkCustomerImage.NavigateUrl <> "" And txtCustomerDrawingNo.Text.Trim <> "" Then
                        PEModule.UpdateDrawingCustomerImage(ViewState("DrawingNo"), txtCustomerDrawingNo.Text.Trim)
                    End If
                End If
                'for existing parts, rebind objects to updated data
                BindData()
            End If 'if new drawing

            ViewState("isOverride") = False

            EnableControls()

            CancelEdit()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Function GenerateDrawingNo() As String

        Try
            '08/05/2008 - New Rule - Facility will not be referenced in DMS Drawing Number
            ''Drawings will have auto-generated numbers in the format of X1234-56789-A(BB)
            ''1234 = represents 4 digit family/subfamily code
            ''56 = represents some type of measurement of the material
            ''789 = represents next numeric sequence, of the presence of materials with same combination of 1234-56 above
            ''A = represents in-step tracking number
            ''BB = represents change level, starts at 0 for each drawing, auto-incremented for revisions       

            Dim ds As DataSet
            Dim iRowCount As Integer = 1
            Dim iAltRowCount As Integer = 1

            Dim iCtr As Integer = 0
            Dim iFirstDashLocation As Integer = 0

            Dim strNewDrawingNo As String = ""
            Dim strChangeLevel As String = ""

            'check digits 5 and 6 before incrementing numSeq, to avoid gaps
            Dim strNumberSequence As String = "1"
            Dim iNumberSequence As Integer = 0

            If ViewState("DrawingNo") <> "" And ViewState("DrawingNo") <> "NewDrawing" Then
                iFirstDashLocation = InStr(ViewState("DrawingNo"), "-")
                strNumberSequence = Mid$(ViewState("DrawingNo"), iFirstDashLocation + 3, 3)
            End If
            iNumberSequence = CType(strNumberSequence, Integer)

            ''count number of records that have the same 1234-56 value, add 1 for the new record,
            ''initial implementation has all records with value of 56 as '00' ... no
            Dim strSubFamilyID As String = ddSubFamily.SelectedValue().ToString

            Dim strInitialDimensionAndDensity As String = txtInitialDimensionAndDensity.Text.Trim

            strInitialDimensionAndDensity = strInitialDimensionAndDensity.PadLeft(2, "0")

            strChangeLevel = "0"

            strNewDrawingNo = strSubFamilyID.PadLeft(4, "0") & "-" & strInitialDimensionAndDensity                       'PORTION: 1234-56
            strNewDrawingNo = strNewDrawingNo & strNumberSequence.PadLeft(3, "0")                                        'PORTION: 789 
            strNewDrawingNo = strNewDrawingNo & "-" & txtInStep.Text.Trim & "(" & strChangeLevel.PadLeft(2, "0") & ")"   'PORTION: -A(BB) 

            ''check to make sure new part number is not already used
            'ds = PEModule.GetDrawing(strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
            ds = PEModule.GetDrawing(strNewDrawingNo)

            'if the part number exist, increment the numSeq and retry            
            While (iRowCount > 0 Or iAltRowCount > 0)

                iNumberSequence += 1

                strNewDrawingNo = strSubFamilyID.PadLeft(4, "0") + "-" + strInitialDimensionAndDensity                   'PORTION: 1234-56
                strNewDrawingNo = strNewDrawingNo + iNumberSequence.ToString.PadLeft(3, "0")                             'PORTION: 789 

                'first get 1234-56789 section to see if it exists. If it does not exist, use it and append step and revision later.
                'If this section of the drawing number exists, increment the 789 digits
                'ds = PEModule.GetDrawing(strNewDrawingNo & "%", "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                ds = PEModule.GetDrawing(strNewDrawingNo & "%")
                iRowCount = ds.Tables.Item(0).Rows.Count

                'check if X or simular number with just UGN Facility is in front of new drawing number
                'ds = PEModule.GetDrawing("%" & strNewDrawingNo & "%", "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                ds = PEModule.GetDrawing("%" & strNewDrawingNo & "%")
                iAltRowCount = ds.Tables.Item(0).Rows.Count

                If iRowCount = 0 And iAltRowCount = 0 Then
                    '1234-56789 is available to use, check if the step and revision is too
                    strNewDrawingNo = strNewDrawingNo + "-" + txtInStep.Text + "(" + strChangeLevel.PadLeft(2, "0") + ")"    'PORTION: -A(BB) 
                    'ds = PEModule.GetDrawing(strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                    ds = PEModule.GetDrawing(strNewDrawingNo)
                    iRowCount = ds.Tables.Item(0).Rows.Count

                    'ds = PEModule.GetDrawing("%" & strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                    ds = PEModule.GetDrawing("%" & strNewDrawingNo)
                    iAltRowCount = ds.Tables.Item(0).Rows.Count
                End If
                'iRowCount = ds.Tables.Item(0).Rows.Count                
            End While

            GenerateDrawingNo = strNewDrawingNo

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            GenerateDrawingNo = ""
        End Try

    End Function
    Private Sub copyNotificationList(ByVal OriginalDrawingNo As String)

        Try
            Dim ds As DataSet
            Dim iCtr As Integer
            Dim iTeamMemberID As Integer

            ds = PEModule.GetDrawingNotifications(OriginalDrawingNo)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iCtr = 0 To ds.Tables.Item(0).Rows.Count - 1
                    iTeamMemberID = ds.Tables(0).Rows(iCtr).Item("TeamMemberID")
                    PEModule.InsertDrawingNotification(ViewState("DrawingNo"), iTeamMemberID)
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

    End Sub
    Private Sub CopySubDrawings(ByVal OriginalDrawingNo As String)

        Try
            Dim dsSubDrawings As DataSet
            Dim iSubDrawingRowCounter As Integer = 0
            Dim strTempSubDrawingNo As String = ""
            Dim dblTempDrawingQuantity As Double
            Dim strTempNotes As String = ""
            Dim strTempProcess As String = ""
            Dim strTempEquipment As String = ""
            Dim strTempProcessParameters As String = ""

            dsSubDrawings = PEModule.GetSubDrawing(OriginalDrawingNo, "", "", "", "", "", 0, "", False)
            If commonFunctions.CheckDataSet(dsSubDrawings) = True Then

                For iSubDrawingRowCounter = 0 To dsSubDrawings.Tables(0).Rows.Count - 1
                    strTempSubDrawingNo = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("SubDrawingNo").ToString
                    dblTempDrawingQuantity = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("DrawingQuantity")
                    strTempNotes = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("Notes").ToString
                    strTempProcess = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("Process").ToString
                    strTempEquipment = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("Equipment").ToString
                    strTempProcessParameters = dsSubDrawings.Tables(0).Rows(iSubDrawingRowCounter).Item("ProcessParameters").ToString
                    PEModule.InsertSubDrawing(ViewState("DrawingNo"), strTempSubDrawingNo, dblTempDrawingQuantity, strTempNotes, strTempProcess, strTempEquipment, strTempProcessParameters)
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

    End Sub
    Protected Sub CopyImage(ByVal OriginalDrawingNo As String)

        Try
            Dim dsImages As DataSet
            Dim TempImageURL As String = ""
            Dim TempImageBytes As Byte()

            If (ddDrawingLayoutType.SelectedValue = "Other" Or ddDrawingLayoutType.SelectedValue = "Other-MD-Critical") And ViewState("isAdmin") = True Then
                dsImages = PEModule.GetDrawingImages(OriginalDrawingNo, "")

                If commonFunctions.CheckDataSet(dsImages) = True Then
                    If dsImages.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
                        TempImageBytes = dsImages.Tables(0).Rows(0).Item("DrawingImage")
                        TempImageURL = dsImages.Tables(0).Rows(0).Item("ImageURL")
                        PEModule.InsertDrawingImage(ViewState("DrawingNo"), TempImageURL, TempImageBytes)
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
    Public Sub SendEmail(ByVal EmailToAddress As String)

        Try

            Dim ds As DataSet
            Dim iRowCounter As Integer = 0

            Dim strBody As String = ""
            Dim strSubject As String = ""
            Dim strEmailFromAddress As String = Trim(commonFunctions.getUserName())

            If strEmailFromAddress <> "" Then
                strEmailFromAddress &= "@ugnauto.com"

                Dim mail As New MailMessage()

                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strSubject = "TEST PLEASE DISREGARD: "
                    strBody = "<h1>TEST PLEASE DISREGARD: </h1>"
                End If

                strSubject &= " Drawing Management System - Drawing Issued: " & ViewState("DrawingNo")

                strBody &= "<font size='3' face='Verdana'>A new DMS Drawing has been issued and is ready for your review.</font><br /><br />"

                strBody &= "<font size='2' face='Verdana'>Drawing No.: <b>" & ViewState("DrawingNo") & "</b></font><br />"

                If lblOldDrawingPartNameValue.Text <> "" Then
                    strBody = strBody & "<font size='2' face='Verdana'>Drawing Name: <b>" & lblOldDrawingPartNameValue.Text & "</b></font><br />"
                End If

                If lblPartName.Text <> "" Then
                    strBody = strBody & "<font size='2' face='Verdana'>Part Name: <b>" & lblPartName.Text & "</b></font><br />"
                End If

                strBody &= "<br /><b><font size='1' color='red' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font></b><br /><br />"

                strBody &= "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString & "PE/DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & "'><b><u>Click here to view Drawing</u></b></a><br /><br />"

                If txtComments.Text.Trim <> "" Then
                    strBody &= "<font size='2' face='Verdana'>Comments: " & txtComments.Text & "</font><br />"
                End If

                'include references to Material Specificications
                ds = PEModule.GetDrawingMaterialSpecRelateByDrawingNo(ViewState("DrawingNo"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    strBody &= "<br /><br /><font size='2' face='Verdana'>Below is a list of Material Specification(s) associated to this DMS Drawing</font><br /><br />"

                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        strBody &= "<a href='" & System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString & "PE/MaterialSpecDetail.aspx?MaterialSpecNo=" & ds.Tables(0).Rows(iRowCounter).Item("MaterialSpecNo").ToString.Trim & "'><b><u>Click here to view Material Specification: " & ds.Tables(0).Rows(iRowCounter).Item("MaterialSpecNo").ToString.Trim & " </u></b></a><br />"
                    Next
                End If

                strBody = strBody & "<font size='2' face='Verdana'>Thank you.</font><br />"

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    strBody = strBody & "<br /><br />EmailToAddress: " & EmailToAddress
                    strBody = strBody & "<br />EmailCcAddress: " & strEmailFromAddress
                End If

                'set the content
                mail.Subject = strSubject
                mail.Body = strBody

                'set the addresses
                mail.From = New MailAddress(strEmailFromAddress)

                mail.IsBodyHtml = True

                Dim i As Integer
                Dim emailList As String() = EmailToAddress.Split(";")

                If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                    mail.To.Add("roderick.carlson@ugnauto.com")
                Else
                    For i = 0 To UBound(emailList)
                        mail.To.Add(emailList(i))
                    Next i

                    emailList = Nothing
                    emailList = strEmailFromAddress.Split(";")
                    For i = 0 To UBound(emailList)
                        If emailList(i) <> "" Then
                            mail.CC.Add(emailList(i))
                        End If
                    Next i
                    'mail.Bcc.Add("roderick.carlson@ugnauto.com")
                End If

                'send the message 
                Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                Try
                    smtp.Send(mail)
                    lblMessage.Text &= "Email Notification sent."
                Catch ex As Exception
                    lblMessage.Text &= "Email Notification queued."
                    UGNErrorTrapping.InsertEmailQueue("PE-DMS Notification", strEmailFromAddress, EmailToAddress, "", strSubject, strBody, "")
                End Try
                'lblMessagePrincipals.Text = "Notification Email was sent successfully."
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message & ", EmailToAddress:" & EmailToAddress, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        Try
            ClearMessages()

            Session("DMSTabSelected") = 0

            Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & "&CopyType=New" & "&CopyMaterialSpec=" & cbCopyMaterialSpecList.Checked, False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnRevision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRevision.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim bNewPartMade As Boolean = False
            Dim strOriginalDrawingNo As String = ViewState("DrawingNo")

            'create part revision of existing part
            ds = PEModule.CopyDrawing(ViewState("DrawingNo"), "Rev")

            If commonFunctions.CheckDataSet(ds) = True Then
                ViewState("DrawingNo") = ds.Tables(0).Rows(0).Item("newPart")
                CopyImage(strOriginalDrawingNo)
                PEModule.CopyDrawingCustomerProgram(ViewState("DrawingNo"), strOriginalDrawingNo)

                PEModule.CopyDrawingApprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)

                PEModule.CopyDrawingUnapprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)

                PEModule.CopyDrawingBOM(ViewState("DrawingNo"), strOriginalDrawingNo)

                If cbCopyMaterialSpecList.Checked = True Then
                    PEModule.CopyDrawingDrawingMaterialSpecRelateByDrawing(ViewState("DrawingNo"), strOriginalDrawingNo)
                End If

                bNewPartMade = True
                Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & "&CopyMaterialSpec=" & cbCopyMaterialSpecList.Checked, False)
            End If

            If bNewPartMade = False Then
                lblMessage.Text = "Error occurred on Create Revision.  It is possible that the maximum number of revisions has been reached. If not, please contact IS Support."
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
    Protected Sub btnStep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStep.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim bNewPartMade As Boolean = False
            Dim strOriginalDrawingNo As String = ViewState("DrawingNo")

            'create new part part with an updated step number
            If CType(txtInStep.Text.Trim, Integer) < 9 Then
                ds = PEModule.CopyDrawing(ViewState("DrawingNo"), "Step")

                If commonFunctions.CheckDataSet(ds) = True Then

                    ViewState("DrawingNo") = ds.Tables(0).Rows(0).Item("newPart")

                    If ViewState("DrawingNo") <> "" And strOriginalDrawingNo <> "" Then
                        CopyImage(strOriginalDrawingNo)
                        PEModule.CopyDrawingCustomerProgram(ViewState("DrawingNo"), strOriginalDrawingNo)

                        PEModule.CopyDrawingApprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)

                        PEModule.CopyDrawingUnapprovedVendor(ViewState("DrawingNo"), strOriginalDrawingNo)

                        'put the old step as a child of the new step in the BOM of the new step
                        PEModule.InsertSubDrawing(ViewState("DrawingNo"), strOriginalDrawingNo, 1, "Each step is a child of the next step", "", "", "")

                        If cbCopyMaterialSpecList.Checked = True Then
                            PEModule.CopyDrawingDrawingMaterialSpecRelateByDrawing(ViewState("DrawingNo"), strOriginalDrawingNo)
                        End If

                        Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & "&CopyMaterialSpec=" & cbCopyMaterialSpecList.Checked, False)
                    End If

                End If

            End If

            If bNewPartMade = False Then
                lblMessage.Text = "Error occurred on Create Step.  It is possible that the maximum number of steps has been reached. If not, please contact IS Support."
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
    Protected Sub ddTolerance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTolerance.SelectedIndexChanged

        Try
            ClearMessages()

            Dim dsTolerance As DataSet

            If ddTolerance.SelectedIndex > 0 Then
                dsTolerance = PEModule.GetDrawingTolerance(ddTolerance.SelectedValue, "")

                If commonFunctions.CheckDataSet(dsTolerance) = True Then
                    txtDensityVal.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("densityValue") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("densityValue"))
                    txtDensityTol.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("densityTolerance") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("densityTolerance"))
                    txtDensityUnits.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("densityUnits") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("densityUnits"))
                    txtThickVal.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("thicknessValue") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("thicknessValue"))
                    txtThickTol.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("thicknessTolerance") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("thicknessTolerance"))
                    txtThicknessUnits.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("thicknessUnits") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("thicknessUnits"))
                    txtWMDVal.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("WMDValue") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("WMDValue"))
                    txtWMDTol.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("WMDTolerance") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("WMDTolerance"))
                    ddWMDUnits.SelectedValue = IIf(dsTolerance.Tables(0).Rows(0).Item("WMDUnits") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("WMDUnits"))
                    txtAMDVal.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("AMDValue") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("AMDValue"))
                    txtAMDTol.Text = IIf(dsTolerance.Tables(0).Rows(0).Item("AMDTolerance") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("AMDTolerance"))
                    ddAMDUnits.SelectedValue = IIf(dsTolerance.Tables(0).Rows(0).Item("AMDUnits") Is System.DBNull.Value, "", dsTolerance.Tables(0).Rows(0).Item("AMDUnits"))
                End If
            Else
                txtDensityVal.Text = ""
                txtDensityTol.Text = ""
                txtDensityUnits.Text = ""
                txtThickVal.Text = ""
                txtThickTol.Text = ""
                txtThicknessUnits.Text = ""
                txtWMDVal.Text = ""
                txtWMDTol.Text = ""
                ddWMDUnits.SelectedIndex = 0
                txtAMDVal.Text = ""
                txtAMDTol.Text = ""
                ddAMDUnits.SelectedIndex = 0
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
    Protected Sub ddFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFamily.SelectedIndexChanged

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim iFamilyID As Integer = 0

            If ddFamily.SelectedIndex > 0 Then
                iFamilyID = ddFamily.SelectedValue
            End If

            ds = commonFunctions.GetSubFamily(iFamilyID)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                'ddSubFamily.DataTextField = ds.Tables(0).Columns("subFamilyName").ColumnName
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName
                ddSubFamily.DataValueField = ds.Tables(0).Columns("subFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
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
    Protected Sub DeleteImage(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ClearMessages()

            lblMessageDrawingImage.Text = ""

            If (ddDrawingLayoutType.SelectedValue = "Other" Or ddDrawingLayoutType.SelectedValue = "Other-MD-Critical") And ViewState("isEnabled") = True Then
                PEModule.DeleteDrawingImage(ViewState("DrawingNo"))
                ViewState("ImageExists") = False
            End If

            HandleDrawingLayoutType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub DeleteCustomerImage(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ClearMessages()

            If ViewState("DrawingNo") <> "" And ViewState("isAdmin") = True Then
                PEModule.DeleteDrawingCustomerImage(ViewState("DrawingNo"))
                hlnkCustomerImage.NavigateUrl = ""
            End If

            HandleCADavailableCheckbox()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub ddWMDUnits_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddWMDUnits.SelectedIndexChanged

        Try
            ClearMessages()

            Dim dWMDValue As Double = 0

            If txtWMDVal.Text.Trim <> "" Then
                dWMDValue = CType(txtWMDVal.Text.Trim, Double)
            End If

            If ddWMDUnits.SelectedIndex = -1 Then
                txtWMDRef.Text = ""
            Else
                If ddWMDUnits.SelectedValue = "mm" Then
                    txtWMDRef.Text = CStr(Math.Round(dWMDValue * 0.0393700787, 2)) & " inches"
                End If

                If ddWMDUnits.SelectedValue = "m" Then
                    txtWMDRef.Text = CStr(Math.Round(dWMDValue * 3.2808399, 2)) & " feet"
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
    Protected Sub ddAMDUnits_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAMDUnits.SelectedIndexChanged

        Try
            ClearMessages()

            Dim dAMDValue As Double = 0

            If txtAMDVal.Text.Trim <> "" Then
                dAMDValue = CType(txtAMDVal.Text.Trim, Double)
            End If

            If ddAMDUnits.SelectedIndex = -1 Then
                txtAMDRef.Text = ""
            Else
                If ddAMDUnits.SelectedValue = "mm" And dAMDValue > 0 Then
                    txtAMDRef.Text = CStr(Math.Round(dAMDValue * 0.0393700787, 2)) & " inches"
                End If

                If ddAMDUnits.SelectedValue = "m" And dAMDValue > 0 Then
                    txtAMDRef.Text = CStr(Math.Round(dAMDValue * 3.2808399, 2)) & " feet"
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
    Protected Sub txtWMDVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWMDVal.TextChanged

        Try
            Me.Validate()

            If Page.IsValid Then
                Call ddWMDUnits_SelectedIndexChanged(sender, e)
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
    Protected Sub txtAMDVal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAMDVal.TextChanged

        Try
            Me.Validate()

            If Page.IsValid Then
                Call ddAMDUnits_SelectedIndexChanged(sender, e)
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
    Protected Sub ddDrawingLayoutType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddDrawingLayoutType.SelectedIndexChanged

        Try
            Dim bOverride As Boolean = False
            Dim strTempAppendRevisionNotes As String = txtAppendRevisionNotes.Text.Trim

            If ViewState("isOverride") = True Then
                bOverride = True
            End If

            ClearMessages()

            lblMessageDrawingImage.Text = ""

            'Call btnSave_Click(sender, e)

            If bOverride = True Then
                ViewState("isOverride") = True
                txtAppendRevisionNotes.Text = strTempAppendRevisionNotes
                EnableControls()

                lblAppendRevisionNotes.Visible = True
                txtAppendRevisionNotes.Visible = True
                rfvAppendRevisionNotes.Enabled = True
            End If

            HandleDrawingLayoutType()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnSaveUploadImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadImage.Click

        Try
            Dim ds As DataSet
            Dim newFileName As String
            'Dim fileUploadDrawing As FileUpload

            ClearMessages()

            'fileUploadDrawing = CType(Me.FindControl("uploadImage"), FileUpload)
            'If fileUploadDrawing.PostedFile Is Nothing OrElse String.IsNullOrEmpty(fileUploadDrawing.PostedFile.FileName) OrElse fileUploadDrawing.PostedFile.InputStream Is Nothing Then
            '    lblMessageDrawingSpecification.Text = "Please choose a file to upload"
            'Else
            If Not uploadImage.HasFile Then
                ''-- Missing file selection
                lblMessage.Text &= "Please choose a file to upload"
            Else
                If InStr(UCase(uploadImage.FileName), ".JPG") = 0 Then
                    '-- Selection of non-JPG file
                    lblMessage.Text &= "You can upload only JPG files"
                Else
                    If uploadImage.PostedFile.ContentLength > 250000 Then
                        '-- File too large
                        lblMessage.Text &= "Uploaded file size must be less than 250 KB"
                    Else
                        '-- File upload and save to DB, rename to DrawingNo, append count to end of string
                        Dim iDrawingCount As Integer = 0
                        newFileName = ViewState("DrawingNo") & ".jpg"

                        ds = PEModule.GetDrawingImages(ViewState("DrawingNo"), "")
                        If commonFunctions.CheckDataSet(ds) = True Then
                            iDrawingCount = ds.Tables.Item(0).Rows.Count
                            newFileName = ViewState("DrawingNo") & "_" + iDrawingCount.ToString & ".jpg"
                        End If

                        If ViewState("DrawingNo") <> "" And ViewState("DrawingNo") <> "NewDrawing" Then
                            'Dim username As String = System.Security.Principal.WindowsIdentity.GetCurrent().Name
                            ' uploadImage. -- old way was to save to a network folder
                            'uploadImage.SaveAs((System.Configuration.ConfigurationManager.AppSettings("DMSDrawingImageLocation").ToString) & newFileName)

                            'Load FileUpload's InputStream into Byte array
                            Dim imageBytes(uploadImage.PostedFile.InputStream.Length) As Byte

                            uploadImage.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length)

                            PEModule.InsertDrawingImage(ViewState("DrawingNo"), newFileName, imageBytes)
                            ds = PEModule.GetDrawingImages(ViewState("DrawingNo"), "")
                            If commonFunctions.CheckDataSet(ds) = True Then
                                imgDrawing.Src = "DisplayDrawingImage.aspx?DrawingNo=" & ViewState("DrawingNo")
                                ViewState("ImageExists") = True
                                HandleDrawingLayoutType()

                                If ViewState("isOverride") = True And txtAppendRevisionNotes.Text.Trim = "" Then
                                    txtAppendRevisionNotes.Text = "Updated Drawing Image"
                                End If

                                Call btnSave_Click(sender, e)

                                lblMessage.Text &= "<br />File uploaded successfully."

                                CancelEdit()

                            Else
                                lblMessage.Text &= "<br />ERROR: File upload failed."
                            End If

                        End If
                    End If
                End If
            End If
            ' End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageSpecifications.Text = lblMessage.Text
        lblMessageDMSImageUpload.Text = lblMessage.Text

    End Sub

    'Protected Sub gvSubDrawings_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSubDrawings.DataBound

    '    'hide header columns
    '    If gvSubDrawings.Rows.Count > 0 Then
    '        'gvSubDrawings.HeaderRow.Cells(0).Visible = False
    '        gvSubDrawings.HeaderRow.Cells(1).Visible = False
    '        gvSubDrawings.HeaderRow.Cells(2).Visible = False
    '    End If

    'End Sub
    'Protected Sub gvSubDrawings_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSubDrawings.RowCommand

    '    'Try
    '    '    lblMessage.Text = ""

    '    '    Dim ds As DataSet
    '    '    Dim txtSubDrawingNoTemp As TextBox
    '    '    'Dim ddSubDrawingNoTemp As DropDownList
    '    '    Dim txtDrawingQuantityTemp As TextBox
    '    '    Dim txtNotesTemp As TextBox
    '    '    Dim txtProcessTemp As TextBox
    '    '    Dim txtEquipmentTemp As TextBox
    '    '    Dim txtProcessParametersTemp As TextBox
    '    '    Dim intRowsAffected As Integer = 0

    '    '    ''***
    '    '    ''This section allows the inserting of a new row when called by the OnInserting event call.
    '    '    ''***
    '    '    If (e.CommandName = "Insert") Then

    '    '        'ddSubDrawingNoTemp = CType(gvSubDrawings.FooterRow.FindControl("ddInsertSubDrawings"), DropDownList)
    '    '        txtSubDrawingNoTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertSubDrawing"), TextBox)

    '    '        If txtSubDrawingNoTemp.Text.Trim <> "" Then
    '    '            ds = PEModule.GetDrawing(txtSubDrawingNoTemp.Text.Trim, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")

    '    '            If commonFunctions.CheckDataset(ds) = True Then
    '    '                txtDrawingQuantityTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertDrawingQuantity"), TextBox)
    '    '                txtNotesTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertNotes"), TextBox)
    '    '                txtProcessTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertProcess"), TextBox)
    '    '                txtEquipmentTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertEquipment"), TextBox)
    '    '                txtProcessParametersTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertProcessParameters"), TextBox)

    '    '                odsSubDrawings.InsertParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
    '    '                'odsSubDrawings.InsertParameters("SubDrawingNo").DefaultValue = ddSubDrawingNoTemp.SelectedValue
    '    '                odsSubDrawings.InsertParameters("SubDrawingNo").DefaultValue = txtSubDrawingNoTemp.Text.Trim
    '    '                odsSubDrawings.InsertParameters("DrawingQuantity").DefaultValue = txtDrawingQuantityTemp.Text
    '    '                odsSubDrawings.InsertParameters("Notes").DefaultValue = txtNotesTemp.Text
    '    '                odsSubDrawings.InsertParameters("Process").DefaultValue = txtProcessTemp.Text
    '    '                odsSubDrawings.InsertParameters("Equipment").DefaultValue = txtEquipmentTemp.Text
    '    '                odsSubDrawings.InsertParameters("ProcessParameters").DefaultValue = txtProcessParametersTemp.Text
    '    '                intRowsAffected = odsSubDrawings.Insert()

    '    '                lblMessage.Text = "The sub-drawing was successfully added to the bill of materials."
    '    '            Else
    '    '                lblMessage.Text = "Error: The sub-drawing could not be added to the bill of materials because it does not exist."
    '    '            End If

    '    '        End If

    '    '    End If

    '    '    ''***
    '    '    ''This section allows show/hides the footer row when the Edit control is clicked
    '    '    ''***
    '    '    If e.CommandName = "Edit" Then
    '    '        gvSubDrawings.ShowFooter = False
    '    '    Else
    '    '        gvSubDrawings.ShowFooter = True
    '    '    End If

    '    '    ''***
    '    '    ''This section clears out the values in the footer row
    '    '    ''***
    '    '    If e.CommandName = "Undo" Then
    '    '        'ddSubDrawingNoTemp = CType(gvSubDrawings.FooterRow.FindControl("ddInsertSubDrawings"), DropDownList)
    '    '        'ddSubDrawingNoTemp.SelectedIndex = -1

    '    '        txtSubDrawingNoTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertSubDrawing"), TextBox)
    '    '        txtSubDrawingNoTemp.Text = ""

    '    '        txtDrawingQuantityTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertDrawingQuantity"), TextBox)
    '    '        txtDrawingQuantityTemp.Text = ""

    '    '        txtNotesTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertNotes"), TextBox)
    '    '        txtNotesTemp.Text = ""

    '    '        txtProcessTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertProcess"), TextBox)
    '    '        txtProcessTemp.Text = ""

    '    '        txtEquipmentTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertEquipment"), TextBox)
    '    '        txtEquipmentTemp.Text = ""

    '    '        txtProcessParametersTemp = CType(gvSubDrawings.FooterRow.FindControl("txtInsertProcessParameters"), TextBox)
    '    '        txtProcessParametersTemp.Text = ""

    '    '    End If

    '    'Catch ex As Exception

    '    '    'get current event name
    '    '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '    '    'update error on web page
    '    '    lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '    '    'log and email error
    '    '    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    'End Try

    '    'lblMessageBillOfMaterials.Text = lblMessage.Text
    '    'lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    'End Sub
    'Protected Sub SubDrawingIncRev_OnClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    '    Try
    '        ClearMessages()

    '        Dim strNewSubDrawingNoTemp As String = ""
    '        Dim iRowIDTemp As Integer = 0

    '        Dim lblEditRowIDTemp As Label
    '        Dim lblSubDrawingNoTemp As Label

    '        Dim txtDrawingQuantityTemp As TextBox
    '        Dim txtNotesTemp As TextBox

    '        'Dim currentRowInEdit As Integer = gvSubDrawings.EditIndex
    '        Dim currentRowInEdit As Integer = gvSubDrawings.SelectedIndex

    '        If currentRowInEdit >= 0 Then
    '            lblSubDrawingNoTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("lblEditSubDrawing"), Label)
    '            lblEditRowIDTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("lblEditRowID"), Label)

    '            txtDrawingQuantityTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("txtEditDrawingQuantity"), TextBox)
    '            txtNotesTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("txtEditNotes"), TextBox)

    '            If lblSubDrawingNoTemp IsNot Nothing And lblEditRowIDTemp IsNot Nothing Then
    '                If lblSubDrawingNoTemp.Text.Trim <> "" And lblEditRowIDTemp.Text.Trim <> "" Then
    '                    iRowIDTemp = CType(lblEditRowIDTemp.Text.Trim, Integer)

    '                    strNewSubDrawingNoTemp = PEModule.GetNextDrawingRevision(lblSubDrawingNoTemp.Text)

    '                    If strNewSubDrawingNoTemp <> "" And iRowIDTemp > 0 Then
    '                        'PEModule.UpdateSubDrawing(iRowIDTemp, strNewSubDrawingNoTemp, txtDrawingQuantityTemp.Text.Trim, txtNotesTemp.Text.Trim)

    '                        gvSubDrawings.DataBind()
    '                    End If
    '                End If
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

    'End Sub
    'Protected Sub SubDrawingDecRev_OnClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    '    Try
    '        ClearMessages()

    '        Dim strNewSubDrawingNoTemp As String = ""
    '        Dim iRowIDTemp As Integer = 0

    '        Dim lblEditRowIDTemp As Label
    '        Dim lblSubDrawingNoTemp As Label

    '        Dim txtDrawingQuantityTemp As TextBox
    '        Dim txtNotesTemp As TextBox

    '        Dim currentRowInEdit As Integer = gvSubDrawings.EditIndex

    '        If currentRowInEdit >= 0 Then
    '            lblSubDrawingNoTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("lblEditSubDrawing"), Label)
    '            lblEditRowIDTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("lblEditRowID"), Label)

    '            txtDrawingQuantityTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("txtEditDrawingQuantity"), TextBox)
    '            txtNotesTemp = CType(gvSubDrawings.Rows(currentRowInEdit).FindControl("txtEditNotes"), TextBox)

    '            If lblSubDrawingNoTemp IsNot Nothing And lblEditRowIDTemp IsNot Nothing Then
    '                If lblSubDrawingNoTemp.Text.Trim <> "" And lblEditRowIDTemp.Text.Trim <> "" Then
    '                    iRowIDTemp = CType(lblEditRowIDTemp.Text.Trim, Integer)

    '                    strNewSubDrawingNoTemp = PEModule.GetPreviousDrawingRevision(lblSubDrawingNoTemp.Text)

    '                    If strNewSubDrawingNoTemp <> "" And iRowIDTemp > 0 Then
    '                        'PEModule.UpdateSubDrawing(iRowIDTemp, strNewSubDrawingNoTemp, txtDrawingQuantityTemp.Text.Trim, txtNotesTemp.Text.Trim)

    '                        gvSubDrawings.DataBind()
    '                    End If
    '                End If
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

    'End Sub
    Protected Sub gvDrawingNotifications_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDrawingNotifications.RowCommand

        Try

            Dim ddNotificationTeamMemberTemp As DropDownList
            Dim intRowsAffected As Integer = 0

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            If (e.CommandName = "Insert") Then

                ddNotificationTeamMemberTemp = CType(gvDrawingNotifications.FooterRow.FindControl("ddInsertNotificationTeamMember"), DropDownList)

                odsDrawingNotifications.InsertParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
                odsDrawingNotifications.InsertParameters("TeamMemberID").DefaultValue = ddNotificationTeamMemberTemp.SelectedValue
                intRowsAffected = odsDrawingNotifications.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDrawingNotifications.ShowFooter = False
            Else
                gvDrawingNotifications.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddNotificationTeamMemberTemp = CType(gvDrawingNotifications.FooterRow.FindControl("ddInsertNotificationTeamMember"), DropDownList)
                ddNotificationTeamMemberTemp.SelectedIndex = -1
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
    Protected Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click

        Try
            ClearMessages()

            PEModule.DeleteDrawing(ViewState("DrawingNo"))

            lblMessage.Text = "The drawing was deleted successfully."

            ViewState("DrawingNo") = "NewDrawing"

            'clear fields and hide buttons
            ddReleaseType.SelectedIndex = -1
            ddReleaseType.Enabled = False

            ddCommodity.SelectedIndex = -1
            ddCommodity.Enabled = False

            'ddCustomer.SelectedIndex = -1
            'ddCustomer.Enabled = False

            ddDesignationType.SelectedIndex = -1
            ddDesignationType.Enabled = False

            'ddMake.SelectedIndex = -1
            'ddMake.Enabled = False
            tblMakes.Visible = False

            ddPurchasedGood.SelectedIndex = -1
            ddPurchasedGood.Enabled = False

            ddFamily.SelectedIndex = -1
            ddFamily.Enabled = False

            ddSubFamily.SelectedIndex = -1
            ddSubFamily.Enabled = False

            ddProgram.SelectedIndex = -1
            ddProgram.Enabled = False

            ddYear.SelectedIndex = -1
            ddYear.Enabled = False

            lblApprovalStatus.Text = ""

            txtInitialDimensionAndDensity.Text = ""
            txtInitialDimensionAndDensity.Enabled = False

            txtInStep.Text = ""
            txtInStep.Enabled = False

            ddVendor.SelectedIndex = -1
            lblCustomerLabel.Text = ""
            txtCustomerPartNoValue.Text = ""
            lblOldCustomerPartNameValue.Text = ""
            lblOldCategoryTypeValue.Text = ""

            cbCopyMaterialSpecList.Visible = False

            btnAddToCustomerProgram.Visible = False
            btnSave.Visible = False
            btnSaveIdentification.Visible = False
            btnVoid.Visible = False
            btnCopy.Visible = False
            btnRevision.Visible = False
            btnStep.Visible = False
            btnSendNotification.Visible = False
            btnReset.Visible = False
            btnCompareRevisions.Visible = False
            btnPreview.Visible = False

            lnkChangeSubDrawingReleaseTypes.Visible = False
            lnkPushCustomerProgramToSubDrawing.Visible = False

            cbPreviewBOM.Visible = False

            menuDMSTabs.Items(1).Enabled = False
            menuDMSTabs.Items(2).Enabled = False
            menuDMSTabs.Items(3).Enabled = False
            menuDMSTabs.Items(4).Enabled = False
            menuDMSTabs.Items(5).Enabled = False

            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = False

            iBtnCustomerPartNoSearch.Visible = False
            txtCustomerPartNoValue.Enabled = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub menuDMSTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuDMSTabs.MenuItemClick

        Try
            If Int32.Parse(e.Item.Value) = 2 Then 'Bill of Materials tab
                rfvAppendRevisionNotes.ValidationGroup = "vgSubDrawing"
            Else
                rfvAppendRevisionNotes.ValidationGroup = "vgDrawing"
            End If

            mvDMSTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
            Session("DMSTabSelected") = Int32.Parse(e.Item.Value) 'menuDMSTabs.SelectedValue

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnFindSimilar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindSimilar.Click

        Try
            ClearMessages()

            PEModule.DeletePECookies()

            Response.Cookies("PEModule_SaveDrawingNoSearch").Value = ""
            Response.Cookies("PEModule_SaveCustomerPartNoSearch").Value = ""
            Response.Cookies("PEModule_SavePartNoSearch").Value = ""
            Response.Cookies("PEModule_SavePartNameSearch").Value = ""

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


            Response.Cookies("PEModule_SaveProgramSearch").Value = 0
            Response.Cookies("PEModule_SaveProgramSearch").Expires = DateTime.Now.AddDays(-1)

            Response.Cookies("PEModule_SaveSubFamilySearch").Value = 0
            Response.Cookies("PEModule_SaveSubFamilySearch").Expires = DateTime.Now.AddDays(-1)

            Response.Cookies("PEModule_SaveDensityValueSearch").Value = txtDensityVal.Text.Trim

            Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Value = 0
            Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Expires = DateTime.Now.AddDays(-1)


            Response.Cookies("PEModule_SaveConstructionSearch").Value = ""
            Response.Cookies("PEModule_SaveNotesSearch").Value = ""
            Response.Cookies("PEModule_SaveReleaseTypeSearch").Value = ""
            Response.Cookies("PEModule_SaveStatusSearch").Value = ""
            Response.Cookies("PEModule_SaveBOMSearch").Value = False
            Response.Cookies("PEModule_SaveDrawingDateStartSearch").Value = ""
            Response.Cookies("PEModule_SaveDrawingDateEndSearch").Value = ""

            Response.Redirect("DrawingList.aspx?Commodity=" & Server.UrlEncode(ddCommodity.SelectedValue) & "&PurchasedGood=" & Server.UrlEncode(ddPurchasedGood.SelectedValue) & "&DensityValue=" & Server.UrlEncode(txtDensityVal.Text.Trim), False)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnSendNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendNotification.Click

        Try
            ClearMessages()

            Dim strEmailToAddress As String = ""
            Dim strEmailCCAddress As String = ""
            Dim dsTeamMember As DataSet

            Dim dsNotificationList As DataSet
            Dim iNotificationCounter As Integer
            Dim iTeamMemberID As Integer

            If ddProcessEngineer.SelectedIndex = -1 Or ddQualityEngineer.SelectedIndex = -1 Then
                lblMessage.Text &= "<br />Error: The Process Engineer and Quality Engineer are both required."
            Else
                AddPurchasingToNotification()

                'update drawing status
                PEModule.UpdateDrawingStatus(ViewState("DrawingNo"), "I")

                'get Engineer
                If ddEngineer.SelectedIndex > 0 Then
                    dsTeamMember = SecurityModule.GetTeamMember(ddEngineer.SelectedValue, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then


                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If

                'get DrawingBy Engineer
                If ddDrawingByEngineer.SelectedIndex > 0 Then
                    dsTeamMember = SecurityModule.GetTeamMember(ddDrawingByEngineer.SelectedValue, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If

                'get CheckedBy Engineer
                If ddCheckedByEngineer.SelectedIndex > 0 Then
                    dsTeamMember = SecurityModule.GetTeamMember(ddCheckedByEngineer.SelectedValue, "", "", "", "", "", True, Nothing)
                    If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                        If strEmailToAddress.Trim <> "" Then
                            strEmailToAddress &= ";"
                        End If

                        strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                    End If
                End If

                'get Process Engineer
                If ddProcessEngineer.SelectedIndex > 0 Then
                    dsTeamMember = SecurityModule.GetTeamMember(ddProcessEngineer.SelectedValue, "", "", "", "", "", True, Nothing)
                    If dsTeamMember IsNot Nothing Then
                        If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then

                            If strEmailToAddress.Trim <> "" Then
                                strEmailToAddress &= ";"
                            End If

                            strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                        End If
                    End If
                End If

                'get Quality Engineer
                If ddQualityEngineer.SelectedIndex > 0 Then
                    dsTeamMember = SecurityModule.GetTeamMember(ddQualityEngineer.SelectedValue, "", "", "", "", "", True, Nothing)
                    If dsTeamMember IsNot Nothing Then
                        If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then

                            If strEmailToAddress.Trim <> "" Then
                                strEmailToAddress &= ";"
                            End If

                            strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                        End If
                    End If
                End If

                'get NotificationList
                dsNotificationList = PEModule.GetDrawingNotifications(ViewState("DrawingNo"))
                If dsNotificationList IsNot Nothing Then
                    If (dsNotificationList.Tables.Count > 0 And dsNotificationList.Tables.Item(0).Rows.Count > 0) Then
                        For iNotificationCounter = 0 To dsNotificationList.Tables.Item(0).Rows.Count - 1

                            iTeamMemberID = dsNotificationList.Tables(0).Rows(iNotificationCounter).Item("TeamMemberID")

                            'get email address of Team Member in Notification List
                            dsTeamMember = SecurityModule.GetTeamMember(iTeamMemberID, "", "", "", "", "", True, Nothing)
                            If dsTeamMember IsNot Nothing Then
                                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then

                                    If strEmailToAddress.Trim <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If
                            End If
                        Next

                    End If
                End If

                If strEmailToAddress <> "" Or strEmailCCAddress <> "" Then
                    SendEmail(strEmailToAddress)
                End If

                'refresh page
                Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo"), False)
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
    Protected Sub lnkChangeSubDrawingReleaseTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkChangeSubDrawingReleaseTypes.Click

        Try
            ClearMessages()

            Response.Redirect("DrawingReleaseTypeChange.aspx?DrawingNo=" & ViewState("DrawingNo"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub BuildWhereUsedTree(ByVal ChildDrawingNo As String, ByVal n As TreeNode)

        Try
            Dim iRecursionCounter As Integer = Session("sessionDMSChangeParentRecursionCounter")
            Dim iCurrentRecursionLevel As Integer = Session("sessionDMSChangeParentCurrentRecursionLevel")

            If Session("sessionDMSChangeParentRecursionCounter") = Nothing Then
                iRecursionCounter = 0
            End If

            Dim ds As DataSet
            Dim dsParent As DataSet

            Dim iCounter As Integer = 0

            Dim strParentDrawingNo As String = ""
            Dim strParentDrawingName As String = ""
            Dim strParentDrawingReleaseType As String = ""
            Dim strParentDrawingStatus As String = ""

            'preventing an infinite loop
            Session("sessionDMSChangeParentRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 999 Then

                ds = PEModule.GetSubDrawing("", ChildDrawingNo, "", "", "", "", 0, "", False)
                If commonFunctions.CheckDataSet(ds) = True Then

                    For iCounter = 0 To ds.Tables(0).Rows.Count - 1
                        iRecursionCounter += 1
                        Session("sessionDMSChangeParentRecursionCounter") = iRecursionCounter + 1

                        strParentDrawingNo = ds.Tables(0).Rows(iCounter).Item("DrawingNo").ToString.Trim

                        If strParentDrawingNo <> "" Then
                            dsParent = PEModule.GetDrawing(strParentDrawingNo)

                            If commonFunctions.CheckDataSet(dsParent) = True Then
                                strParentDrawingName = dsParent.Tables(0).Rows(0).Item("OldPartName").ToString.Trim
                                strParentDrawingReleaseType = dsParent.Tables(0).Rows(0).Item("ddReleaseTypeName").ToString.Trim
                                strParentDrawingStatus = dsParent.Tables(0).Rows(0).Item("approvalStatusDecoded").ToString.Trim

                                If dsParent.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                                    Dim node As New TreeNode(strParentDrawingNo & "  -  " & strParentDrawingName & " (Release Type = " & strParentDrawingReleaseType & " , Status = " & strParentDrawingStatus & " )")  '& " - Level: " & iCurrentRecursionLevel & " - Recursion Counter: " & iRecursionCounter)
                                    If n Is Nothing Then
                                        'root.Checked = True
                                        'root.SelectAction = TreeNodeSelectAction.None
                                        'root.ChildNodes.Add(node)
                                    Else
                                        n.ChildNodes.Add(node)
                                        'n.Checked = True                                
                                    End If

                                    node.Checked = True

                                    Session("sessionDMSChangeParentCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                                    'BuildWhereUsedTree(strParentDrawingNo, node)
                                    Session("sessionDMSChangeParentCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                                End If  'not obsolete drawing                              
                            End If 'drawing exists
                        End If 'end SubDrawings
                    Next 'end iCounter Loop                                                
                End If
            End If 'end check recursion counter

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub BuildCurrentDrawingAsTopTree(ByVal sDrawingNumber As String, ByVal n As TreeNode)

        Dim iRecursionCounter As Integer = Session("sessionDMSCurrentDrawingAsTopRecursionCounter")
        Dim iCurrentRecursionLevel As Integer = Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel")

        If Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = Nothing Then
            iRecursionCounter = 0
        End If

        Dim ds As DataSet
        Dim iSize As Integer = 0
        Dim iCounter As Integer = 0

        Dim strSubDrawingNo As String = ""
        Dim strSubDrawingName As String = ""
        Dim strPartNo As String = ""
        Dim strPartRevision As String = ""
        Dim strQuantity As String = ""
        Dim strNotes As String = ""
        Dim strProcess As String = ""
        Dim strProcessParameters As String = ""

        Try
            'preventing an infinite loop
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 500 Then

                ds = PEModule.GetSubDrawing(sDrawingNumber, "", "", "", "", "", 0, "", False)
                If commonFunctions.CheckDataSet(ds) = True Then
                    iSize = ds.Tables(0).Rows.Count

                    'if SubDrawings Exist.
                    If iSize > 0 Then
                        strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString
                        strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity")
                        strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString
                        strProcess = ds.Tables(0).Rows(iCounter).Item("Process").ToString
                        strProcessParameters = ds.Tables(0).Rows(iCounter).Item("ProcessParameters").ToString

                        Dim rootString As String = sDrawingNumber

                        If lblOldDrawingPartNameValue.Text.Trim <> "" Then
                            rootString &= " - " & lblOldDrawingPartNameValue.Text.Trim
                        End If

                        If lblOldCustomerPartNameValue.Text.Trim <> "" Then
                            rootString &= " | " & lblOldCustomerPartNameValue.Text.Trim
                        End If

                        Dim root As New TreeNode("<span style='text-decoration:none;font-size:16;font-weight:bold'>" & rootString & "</span>")


                        ' start by creating a ROOT node                    
                        If iRecursionCounter = 0 Then
                            tvCurrentDrawingAsTop.Nodes.Add(root)
                        End If


                        Dim strTreeNodeTitleText As String = "<table border='1' width='1000px' style='cursor:default;text-decoration:none;background:maroon;color:white;font-size:10;font-weight:bold'><tr>"

                        strTreeNodeTitleText &= "<td width='90px' align='center' style='text-decoration:none'>Sub-Drawing</td><td width='130px'>Name</td><td width='80px'>Internal</br>Part No</td><td width='50px'>Quantity</td><td width='150px'>Notes</td><td width='75px'>Process</td><td width='75px'>Process<br />Parameters</td>"

                        If ViewState("isEnabled") = True And iCurrentRecursionLevel <= 1 Then
                            strTreeNodeTitleText &= "<td width='50px' align='center'>Edit</td><td width='50px' align='center'>Delete</td>"
                            strTreeNodeTitleText &= "<td width='50px' align='center'>&nbsp;</td>"
                        Else
                            strTreeNodeTitleText &= "<td width='50px' align='center'>&nbsp;</td><td width='50px' align='center'>&nbsp;</td>"
                        End If

                        strTreeNodeTitleText &= "</tr></table>"
                        Dim nodeTitle As New TreeNode(strTreeNodeTitleText)

                        If n Is Nothing Then
                            root.SelectAction = TreeNodeSelectAction.None
                            root.ChildNodes.Add(nodeTitle)
                        Else
                            n.SelectAction = TreeNodeSelectAction.None
                            n.ChildNodes.Add(nodeTitle)
                        End If

                        For iCounter = 0 To iSize - 1

                            iRecursionCounter += 1
                            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = iRecursionCounter + 1

                            strSubDrawingNo = ds.Tables(0).Rows(iCounter).Item("SubDrawingNo").ToString.Trim
                            strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString.Trim

                            strPartNo = ds.Tables(0).Rows(iCounter).Item("SubPartNo").ToString.Trim
                            strPartRevision = ds.Tables(0).Rows(iCounter).Item("SubPart_Revision").ToString.Trim
                            strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity").ToString.Trim
                            strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString.Trim
                            strProcess = ds.Tables(0).Rows(iCounter).Item("Process").ToString.Trim
                            strProcessParameters = ds.Tables(0).Rows(iCounter).Item("ProcessParameters").ToString.Trim

                            If strSubDrawingNo.Trim.Length > 0 Then
                                Dim strTreeNodeText As String = "<table runat='server' id='tbl" & strSubDrawingNo.Trim & "' name='tbl" & strSubDrawingNo.Trim & "' border='1' width='1000px' style='cursor:default;text-decoration:none'><tr>"

                                strTreeNodeText &= "<td align='center' width='90px'><a target='_blank' href='DrawingDetail.aspx?DrawingNo=" & strSubDrawingNo & "'>" & strSubDrawingNo & "</a></td><td width='130px'>" & strSubDrawingName & "</td><td width='80px'>" & strPartNo & "&nbsp;</td><td width='50px'>" & strQuantity & "&nbsp;</td><td width='150px'>" & strNotes & "&nbsp;</td><td width='75px'>" & strProcess & "&nbsp;</td><td width='75px'>" & strProcessParameters & "&nbsp;</td>"

                                If ViewState("isEnabled") = True And iCurrentRecursionLevel <= 1 Then
                                    strTreeNodeText &= "<td align='center' width='50px'><a href='DMSDrawingEditBOM.aspx?ParentDrawingNo=" & ViewState("DrawingNo") & "&ChildDrawingNo=" & strSubDrawingNo & "' target='_blank'><img src='../images/edit.jpg' alt='' border='0' /></a></td><td align='center' width='50px'><a href='DMSDrawingDeleteBOM.aspx?ParentDrawingNo=" & ViewState("DrawingNo") & "&ChildDrawingNo=" & strSubDrawingNo & "' target='_blank'> <img src='../images/delete.jpg' alt='' border='0' /></a></td>"
                                    strTreeNodeText &= "<td id='tdc" & strSubDrawingNo.Trim & "' name='tdc" & strSubDrawingNo.Trim & "' width='50px' align='center' ><input type='checkbox' runat='server' id='cb" & strSubDrawingNo.Trim & "' name='cb" & strSubDrawingNo.Trim & "' onclick='javascript:CheckBOM("" " & strSubDrawingNo.Trim & " "",this.checked);' /></td>"
                                Else
                                    strTreeNodeText &= "<td align='center' width='50px'>&nbsp;</td><td align='center' width='50px'>&nbsp;</td>"
                                End If

                                strTreeNodeText &= "</tr></table>"

                                Dim node As New TreeNode(strTreeNodeText)

                                If n Is Nothing Then
                                    'node.ShowCheckBox = True
                                    'tvCurrentDrawingAsTop.Attributes.Add("style", "nowrap")

                                    root.SelectAction = TreeNodeSelectAction.None
                                    root.ChildNodes.Add(node)
                                    root.Expand()
                                Else
                                    n.ChildNodes.Add(node)
                                End If

                                Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                                If strSubDrawingNo <> sDrawingNumber Then
                                    BuildCurrentDrawingAsTopTree(strSubDrawingNo, node)
                                End If

                                Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                            End If 'end SubDrawings
                        Next 'end iCounter Loop
                    Else
                        If iRecursionCounter = 0 Then
                            lblMessageBillOfMaterials.Text = "There are no sub-drawings currently defined for this drawing."
                        End If
                    End If 'end iSize                    
                End If
            End If 'end check recursion counter

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub lnkViewBOMTree_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBOMTree.Click

        Try
            ClearMessages()

            'BindData()

            'need code to clear tree
            tvCurrentDrawingAsTop.Nodes.Clear()

            'clear session variable
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = 0
            Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = 1

            BuildCurrentDrawingAsTopTree(ViewState("DrawingNo"), Nothing)

            'Expand the Whole Tree
            'tvCurrentDrawingAsTop.ExpandAll()

            'clean session variables
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = Nothing
            Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = Nothing
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub BuildParentList(ByVal childPartNo As String)

        Try
            Dim dsParentList As DataSet
            Dim iCounter As Integer
            Dim bFoundIt As Boolean = False

            dsParentList = PEModule.GetSubDrawing("", childPartNo, "", "", "", "", 0, "", 0)

            'if has NO parents then
            'If dsParentList Is Nothing Or dsParentList.Tables.Count = 0 Or dsParentList.Tables(0).Rows.Count = 0 Then
            '    'add to Parent List Array
            '    ReDim Preserve ParentList(ParentList.Length)
            '    ParentList(ParentList.Length - 1) = childPartNo
            '    ParentCounter += 1
            'If dsParentList Is Nothing Or dsParentList.Tables.Count = 0 Or dsParentList.Tables(0).Rows.Count = 0 Then
            If commonFunctions.CheckDataSet(dsParentList) = False Then
                'check if already in the list
                For iCounter = 0 To ParentList.Length - 1
                    If childPartNo = ParentList(iCounter) Then
                        bFoundIt = True
                    End If
                Next

                'if not in the list, then add it
                If bFoundIt = False Then
                    'add to Parent List Array
                    ReDim Preserve ParentList(ParentList.Length)
                    ParentList(ParentList.Length - 1) = childPartNo
                    ParentCounter += 1
                End If
            Else 'if has parents then parse
                For iCounter = 0 To dsParentList.Tables.Item(0).Rows.Count - 1
                    'recursive call self
                    BuildParentList(dsParentList.Tables(0).Rows(iCounter).Item("DrawingNo"))
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

    End Sub

    Protected Sub SortParentList()

        Try

            Dim iCounter As Integer = 0
            Dim tempDrawing As String = ""

            For iCounter = 0 To ParentList.Length - 2
                If ParentList(iCounter) > ParentList(iCounter + 1) Then
                    tempDrawing = ParentList(iCounter + 1)
                    ParentList(iCounter + 1) = ParentList(iCounter)
                    ParentList(iCounter) = tempDrawing
                End If
            Next

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub StartParentList(ByVal childPartNo As String)

        Try
            'Dim ds As DataSet
            'Dim iCounter As Integer

            tvDrawingWhereUsed.Nodes.Clear()

            'add root node
            Dim strDrawingName As String = ""
            If lblOldDrawingPartNameValue.Text.Trim <> "" Then
                strDrawingName = lblOldDrawingPartNameValue.Text.Trim
            End If

            If lblOldDrawingPartNameValue.Text.Trim <> "" And lblPartName.Text.Trim <> "" Then
                strDrawingName &= " | "
            End If

            If lblPartName.Text.Trim <> "" Then
                strDrawingName &= lblPartName.Text.Trim
            End If

            Dim rootnode As TreeNode = New TreeNode(ViewState("DrawingNo") & " - " & strDrawingName)
            tvDrawingWhereUsed.Nodes.Add(rootnode)

            BuildWhereUsedTree(childPartNo, rootnode)

            tvDrawingWhereUsed.ExpandAll()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub tvCurrentDrawingAsTop_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvCurrentDrawingAsTop.SelectedNodeChanged

        Try
            'get Drawing Number from Current Node
            Dim iRightParenthesisPlace As Integer = InStr(tvCurrentDrawingAsTop.SelectedNode.Text, ")")
            Dim strDrawingNo As String = Mid$(tvCurrentDrawingAsTop.SelectedNode.Text, 1, iRightParenthesisPlace)

            'open sub-drawing in new window
            Page.ClientScript.RegisterStartupScript(Me.GetType(), strDrawingNo, "window.open('DrawingDetail.aspx?DrawingNo=" & strDrawingNo & "' ," & Now.Ticks & " ,'resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=yes');", True)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub lnkWhereUsed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkWhereUsed.Click

        Try
            StartParentList(ViewState("DrawingNo"))
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub tvDrawingWhereUsed_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvDrawingWhereUsed.SelectedNodeChanged

        Try
            Dim iRightParenthesisPlace As Integer = 0
            Dim strDrawingNo As String = ""

            If tvDrawingWhereUsed.SelectedNode.Text <> "" Then
                iRightParenthesisPlace = InStr(tvDrawingWhereUsed.SelectedNode.Text, ")")
                strDrawingNo = Mid$(tvDrawingWhereUsed.SelectedNode.Text, 1, iRightParenthesisPlace)

                'open sub-drawing in new window
                Page.ClientScript.RegisterStartupScript(Me.GetType(), strDrawingNo, "window.open('DrawingDetail.aspx?DrawingNo=" & strDrawingNo & "' ," & Now.Ticks & " ,'resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=yes');", True)
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
    'Protected Sub gvSubDrawings_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubDrawings.RowDataBound

    '    Try
    '        ' Build the client script to open a popup window containing
    '        ' SubDrawings. Pass the ClientID of 4 the 
    '        ' four TextBoxes (which will receive data from the popup)
    '        ' in a query string.

    '        Dim strWindowAttribs As String = _
    '            "width=950px," & _
    '            "height=550px," & _
    '            "left='+((screen.width-950)/2)+'," & _
    '            "top='+((screen.height-550)/2)+'," & _
    '            "resizable=yes,scrollbars=yes,status=yes"

    '        If (e.Row.RowType = DataControlRowType.Footer) Then
    '            Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnInsertGetSubDrawing"), ImageButton)
    '            'Dim ddInsertSubDrawing As DropDownList = CType(e.Row.FindControl("ddInsertSubDrawings"), DropDownList)
    '            Dim txtInsertSubDrawing As TextBox = CType(e.Row.FindControl("txtInsertSubDrawing"), TextBox)
    '            If ibtn IsNot Nothing And txtInsertSubDrawing IsNot Nothing Then

    '                'Dim strPagePath As String = _
    '                '    "SubDrawingLookup.aspx?ddSubDrawingControlID=" & ddInsertSubDrawing.ClientID
    '                Dim strPagePath As String = _
    '                    "DrawingLookup.aspx?DrawingControlID=" & txtInsertSubDrawing.ClientID

    '                Dim strClientScript As String = _
    '                    "window.open('" & strPagePath & "','SubDrawings','" & _
    '                    strWindowAttribs & "');return false;"
    '                ibtn.Attributes.Add("onClick", strClientScript)
    '            End If

    '            'Make sure there is an empty row in the InsertSubDrawing DropDown Box
    '            'ddInsertSubDrawing.Items.Insert(0, "")

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
    'Protected Sub EmptySubDrawingsDataBound(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Try
    '        Dim localDetailsViewEmptySubDrawings As DetailsView = CType(sender, DetailsView)

    '        ' Build the client script to open a popup window containing
    '        ' SubDrawings. Pass the ClientID of 4 the 
    '        ' four TextBoxes (which will receive data from the popup)
    '        ' in a query string.

    '        Dim strWindowAttribs As String = _
    '            "width=950px," & _
    '            "height=600px," & _
    '            "left='+((screen.width-950)/2)+'," & _
    '            "top='+((screen.height-600)/2)+'," & _
    '            "resizable=yes,scrollbars=yes,status=yes"

    '        Dim ibtn As ImageButton = CType(localDetailsViewEmptySubDrawings.FindControl("ibtnEmptyGetSubDrawing"), ImageButton)
    '        Dim ddEmptySubDrawing As DropDownList = CType(localDetailsViewEmptySubDrawings.FindControl("ddEmptyInsertSubDrawings"), DropDownList)
    '        If ibtn IsNot Nothing Then

    '            'Dim strPagePath As String = _
    '            '    "SubDrawingLookup.aspx?ddSubDrawingControlID=" & ddEmptySubDrawing.ClientID
    '            Dim strPagePath As String = _
    '                "DrawingLookup.aspx?DrawingControlID=" & ddEmptySubDrawing.ClientID
    '            Dim strClientScript As String = _
    '                "window.open('" & strPagePath & "','SubDrawings','" & _
    '                strWindowAttribs & "');return false;"
    '            ibtn.Attributes.Add("onClick", strClientScript)
    '        End If

    '        'Make sure there is an empty row in the InsertSubDrawing DropDown Box
    '        ddEmptySubDrawing.Items.Insert(0, "")
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_BPCSInfo() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_BPCSInfo") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_BPCSInfo"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_BPCSInfo") = value
        End Set

    End Property
    'Private Property LoadDataEmpty_SubDrawings() As Boolean
    '' From Andrew Robinson's Insert Empty GridView solution
    '' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

    '' some controls that are used within a GridView,
    '' such as the calendar control, can cuase post backs.
    '' we need to preserve LoadDataEmpty across post backs.

    'Get
    '    If ViewState("LoadDataEmpty_SubDrawings") IsNot Nothing Then
    '        Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_SubDrawings"), Boolean)
    '        Return tmpBoolean
    '    Else
    '        Return False
    '    End If
    'End Get
    'Set(ByVal value As Boolean)
    '    ViewState("LoadDataEmpty_SubDrawings") = value
    'End Set
    'End Property

    Private Property LoadDataEmpty_DrawingNotifications() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DrawingNotifications") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DrawingNotifications"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DrawingNotifications") = value
        End Set
    End Property

    'Protected Sub odsSubDrawings_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsSubDrawings.Selected

    '    '' From Andrew Robinson's Insert Empty GridView solution
    '    '' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

    '    '' bubble exceptions before we touch e.ReturnValue
    '    'If e.Exception IsNot Nothing Then
    '    '    Throw e.Exception
    '    'End If

    '    '' get the DataTable from the ODS select method
    '    'Console.WriteLine(e.ReturnValue)

    '    ''Dim dt As Projected_Sales.Projected_Sales_Customer_ProgramDataTable = CType(e.ReturnValue, Projected_Sales.Projected_Sales_Customer_ProgramDataTable)
    '    'Dim dt As Drawings.SubDrawing_MaintDataTable = CType(e.ReturnValue, Drawings.SubDrawing_MaintDataTable)

    '    '' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
    '    'If dt.Rows.Count = 0 Then
    '    '    dt.Rows.Add(dt.NewRow())
    '    '    LoadDataEmpty_SubDrawings = True
    '    'Else
    '    '    LoadDataEmpty_SubDrawings = False
    '    'End If

    'End Sub

    Protected Sub odsDrawingNotifications_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDrawingNotifications.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Drawings.DrawingNotifications_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingNotifications_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DrawingNotifications = True
            Else
                LoadDataEmpty_DrawingNotifications = False
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
    'Protected Sub gvSubDrawings_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubDrawings.RowCreated

    '    Try
    '        ' From Andrew Robinson's Insert Empty GridView solution
    '        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
    '        ' when binding a row, look for a zero row condition based on the flag.
    '        ' if we have zero data rows (but a dummy row), hide the grid view row
    '        ' and clear the controls off of that row so they don't cause binding errors

    '        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_SubDrawings
    '        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
    '            e.Row.Visible = False
    '            e.Row.Controls.Clear()
    '        End If
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub gvDrawingNotifications_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingNotifications.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DrawingNotifications
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    'Protected Sub lnkPackagingPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPackagingPreview.Click

    '    Try
    '        Response.Redirect("DrawingPackagingPreview.aspx?DrawingNo=" & ViewState("DrawingNo"), False)
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub txtPackagingRollLength_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPackagingRollLength.TextChanged

        Try
            Call ddPackagingRollLengthUnits_SelectedIndexChanged(sender, e)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub ddPackagingRollLengthUnits_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPackagingRollLengthUnits.SelectedIndexChanged

        Try
            If txtPackagingRollLength.Text = "" Then
                txtPackagingRollLength.Text = 0
            End If

            If ddPackagingRollLengthUnits.SelectedValue = "" Then
                txtPackagingRollLengthRef.Text = ""
            End If

            If ddPackagingRollLengthUnits.SelectedValue = "mm" Then
                txtPackagingRollLengthRef.Text = CStr(Math.Round(CType(txtPackagingRollLength.Text, Single) * 0.0393700787, 2)) & " inches"
            End If

            If ddPackagingRollLengthUnits.SelectedValue = "m" Then
                txtPackagingRollLengthRef.Text = CStr(Math.Round(CType(txtPackagingRollLength.Text, Single) * 3.2808399, 2)) & " feet"
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

    Protected Sub lnkOpenPreviousRevision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOpenPreviousRevision.Click

        Try
            If ViewState("PreviousDrawingNoRevision") <> "" Then
                Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("PreviousDrawingNoRevision"), False)
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

    Protected Sub lnkOpenNextRevision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOpenNextRevision.Click


        Try
            If ViewState("NextDrawingNoRevision") <> "" Then
                Response.Redirect("DrawingDetail.aspx?DrawingNo=" & ViewState("NextDrawingNoRevision"), False)
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

    Protected Sub cbPreviewBOM_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbPreviewBOM.CheckedChanged

        'ViewState("PreviewBOM") = cbPreviewBOM.Checked
        CreatePopUps()

    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Try
            ClearMessages()

            Dim strPreview As String = ""

            'clear crystal reports
            PEModule.CleanPEDMScrystalReports()

            If cbPreviewBOM.Checked = True Then
                strPreview = "DrawingBOMPageSelection.aspx?DrawingNo=" & ViewState("DrawingNo")
            Else

                strPreview = "DMSDrawingPreview.aspx?DrawingNo=" & ViewState("DrawingNo")
            End If

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Show Print Preview", "window.open('" & strPreview & "'," & Now.Ticks & ",'resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=yes,location=no');", True)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvBPCSInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBPCSInfo.RowCommand

        Try

            Dim txtPartNoTemp As TextBox
            Dim txtPartRevisionTemp As TextBox

            Dim intRowsAffected As Integer = 0

            Dim dsPartNo As DataSet
            Dim iRowCounter As Integer = 0
            Dim strTempPartNo As String = ""
            Dim strTempPartName As String = ""
            Dim strTempPartRevision As String = ""

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtPartNoTemp = CType(gvBPCSInfo.FooterRow.FindControl("txtInsertPartNo"), TextBox)
                txtPartRevisionTemp = CType(gvBPCSInfo.FooterRow.FindControl("txtInsertPartRevision"), TextBox)

                odsBPCSInfo.InsertParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
                odsBPCSInfo.InsertParameters("PartNo").DefaultValue = txtPartNoTemp.Text
                odsBPCSInfo.InsertParameters("PartRevision").DefaultValue = "" ''txtPartRevisionTemp.Text

                intRowsAffected = odsBPCSInfo.Insert()

                dsPartNo = commonFunctions.GetBPCSPartNo(txtPartNoTemp.Text, "")
                If commonFunctions.CheckDataSet(dsPartNo) = False Then
                    lblMessage.Text &= "<br />WARNING: The Internal PartNo is not in the Legacy System yet."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvBPCSInfo.ShowFooter = False
            Else
                gvBPCSInfo.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtPartNoTemp = CType(gvBPCSInfo.FooterRow.FindControl("txtInsertPartNo"), TextBox)
                txtPartNoTemp.Text = Nothing

                ' ''txtPartRevisionTemp = CType(gvBPCSInfo.FooterRow.FindControl("txtInsertPartRevision"), TextBox)
                ' ''txtPartRevisionTemp.Text = Nothing
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBPCSassignments.Text = lblMessage.Text

    End Sub

    Protected Sub gvBPCSInfo_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBPCSInfo.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_BPCSInfo
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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

    Protected Sub gvBPCSInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBPCSInfo.RowDataBound

        Try
            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnEditSearchInfo"), ImageButton)
                Dim txtEditPartNo As TextBox = CType(e.Row.FindControl("txtEditPartNo"), TextBox)

                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtEditPartNo.ClientID & "&vcPartRevision="
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnInsertSearchInfo"), ImageButton)
                Dim txtInsertPartNo As TextBox = CType(e.Row.FindControl("txtInsertPartNo"), TextBox)
          
                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                    "../DataMaintenance/PartNoLookUp.aspx?vcPartNo=" & txtInsertPartNo.ClientID & "&vcPartRevision="
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','PartNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
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

    Protected Sub odsBPCSInfo_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsBPCSInfo.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        'Dim dt As Projected_Sales.Projected_Sales_Customer_ProgramDataTable = CType(e.ReturnValue, Projected_Sales.Projected_Sales_Customer_ProgramDataTable)
        Dim dt As Drawings.DrawingBPCS_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingBPCS_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty_BPCSInfo = True
        Else
            LoadDataEmpty_BPCSInfo = False
        End If
    End Sub

    Private Sub ClearCustomerProgramInputFields()

        Try
            ViewState("CurrentCustomerProgramRow") = 0
            ViewState("CurrentCustomerProgramID") = 0

            gvCustomerProgram.DataBind()
            gvCustomerProgram.SelectedIndex = -1
            gvCustomerProgram.Columns(gvCustomerProgram.Columns.Count - 1).Visible = True

            tblMakes.Visible = True

            cddMakes.SelectedValue = Nothing

            'ddProgram.SelectedIndex = -1
            ddYear.SelectedIndex = -1
            'ddCustomer.SelectedIndex = -1

            'txtSOPDate.Text = ""
            'txtEOPDate.Text = ""

            btnCancelEditCustomerProgram.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
    Protected Sub btnCancelEditCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEditCustomerProgram.Click

        Try

            ClearMessages()

            ClearCustomerProgramInputFields()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub btnAddToCustomerProgram_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToCustomerProgram.Click

        Try
            ClearMessages()

            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim dSOP As DateTime
            Dim dEOP As DateTime

            Dim strCustomer As String = ""

            'If ViewState("CurrentCustomerProgramRow") > 0 Then
            '    iProgramID = ViewState("CurrentCustomerProgramID")
            'Else
            ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue

            iProgramID = ViewState("CurrentCustomerProgramID")
            'End If

            If InStr(ddProgram.SelectedItem.Text, "**") > 0 And ViewState("CurrentCustomerProgramRow") = 0 Then
                lblMessage.Text &= "Error: An obsolete program cannot be selected. The information was NOT saved."
                ddModel.SelectedIndex = -1
                ddProgram.SelectedIndex = -1
            Else

                'make sure Year Selected is in range of SOP and EOP
                If ddYear.SelectedIndex > 0 Then
                    iProgramYear = ddYear.SelectedValue

                    If txtSOPDate.Text.Trim <> "" Then
                        dSOP = CType(txtSOPDate.Text.Trim, DateTime)

                        If iProgramYear < dSOP.Year Then
                            iProgramYear = dSOP.Year
                        End If
                    End If

                    If txtEOPDate.Text.Trim <> "" Then
                        dEOP = CType(txtEOPDate.Text.Trim, DateTime)

                        If iProgramYear > dEOP.Year Then
                            iProgramYear = dEOP.Year
                        End If
                    End If
                End If

                If iProgramYear > 0 Then
                    'If ViewState("CurrentCustomerProgramRow") > 0 Then
                    '    strCustomer = ddCustomerEdit.SelectedValue

                    '    PEModule.UpdateDrawingCustomerProgram(ViewState("CurrentCustomerProgramRow"), strCustomer, iProgramID, iProgramYear)
                    'Else
                    'strCustomer = ddCustomer.SelectedValue
                    PEModule.InsertDrawingCustomerProgram(ViewState("DrawingNo"), "", iProgramID, iProgramYear)
                    'End If

                    ClearCustomerProgramInputFields()

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

    Protected Sub lnkPushCustomerProgramToSubDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPushCustomerProgramToSubDrawing.Click

        Try
            ClearMessages()

            Response.Redirect("DrawingCustomerProgramChange.aspx?DrawingNo=" & ViewState("DrawingNo"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageCustomerProgram.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveUploadCustomerImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadCustomerImage.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            If Not uploadCustomerImage.HasFile Then
                ''-- Missing file selection
                lblMessage.Text &= "Please choose a file to upload"
            Else
                If InStr(UCase(uploadCustomerImage.FileName), ".PDF") = 0 Then
                    '-- Selection of non-JPG file
                    lblMessage.Text &= "You can upload only PDF files"
                Else
                    If uploadCustomerImage.PostedFile.ContentLength > 1600000 Then
                        '-- File too large
                        lblMessage.Text &= "Uploaded file size must be less than 1.5 MB"
                    Else
                        '-- File upload and save to DB, rename to DrawingNo, append count to end of string

                        If ViewState("DrawingNo") <> "" And ViewState("DrawingNo") <> "NewDrawing" Then

                            'Load FileUpload's InputStream into Byte array
                            Dim imageBytes(uploadCustomerImage.PostedFile.InputStream.Length) As Byte
                            uploadCustomerImage.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length)

                            PEModule.InsertDrawingCustomerImage(ViewState("DrawingNo"), txtCustomerDrawingNo.Text, imageBytes)

                            ds = PEModule.GetDrawingCustomerImages(ViewState("DrawingNo"))
                            If commonFunctions.CheckDataSet(ds) = True Then
                                hlnkCustomerImage.NavigateUrl = "~/PE/DrawingCustomerImageView.aspx?DrawingNo=" & ViewState("DrawingNo")

                                HandleCADavailableCheckbox()

                                If ViewState("isOverride") = True And txtAppendRevisionNotes.Text.Trim = "" Then
                                    txtAppendRevisionNotes.Text = "Updated Customer Drawing Image"
                                    btnSave_Click(sender, e)
                                End If

                                lblMessage.Text = "File uploaded successfully."

                                CancelEdit()
                            Else
                                lblMessage.Text &= "Error: File uploaded failed."
                            End If
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

        lblMessageSpecifications.Text = lblMessage.Text
        lblMessageCustomerImageUpload.Text = lblMessage.Text

    End Sub

    Protected Sub HandleCADavailableCheckbox()

        Try
            lblCustomerDrawingNo.Visible = cbCADavailable.Checked
            txtCustomerDrawingNo.Visible = cbCADavailable.Checked

            lblCustomerDrawingNo.Enabled = ViewState("isEnabled")
            txtCustomerDrawingNo.Enabled = ViewState("isEnabled")

            If txtCustomerDrawingNo.Text.Trim = "" Then
                txtCustomerDrawingNo.Text = txtCustomerPartNoValue.Text
            End If

            If cbCADavailable.Checked = True Then
                If hlnkCustomerImage.NavigateUrl <> "" Then
                    lblUploadCustomerDrawingImage.Visible = False
                    hlnkCustomerImage.Visible = True
                    uploadCustomerImage.Visible = False
                    btnSaveUploadCustomerImage.Visible = False
                    btnDeleteDrawingCustomerImage.Visible = ViewState("isEnabled")
                Else
                    hlnkCustomerImage.Visible = False
                    btnDeleteDrawingCustomerImage.Visible = False
                    lblUploadCustomerDrawingImage.Visible = ViewState("isEnabled")
                    uploadCustomerImage.Visible = ViewState("isEnabled")
                    btnSaveUploadCustomerImage.Visible = ViewState("isEnabled")
                End If
            Else
                lblUploadCustomerDrawingImage.Visible = False
                hlnkCustomerImage.Visible = False
                uploadCustomerImage.Visible = False
                btnSaveUploadCustomerImage.Visible = False
                btnDeleteDrawingCustomerImage.Visible = False
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
    Protected Sub cbCADavailable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbCADavailable.CheckedChanged

        Try
            ClearMessages()

            HandleCADavailableCheckbox()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Property LoadDataEmpty_DrawingApprovedVendor() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DrawingApprovedVendor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DrawingApprovedVendor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DrawingApprovedVendor") = value
        End Set

    End Property
    Private Property LoadDataEmpty_DrawingUnapprovedVendor() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DrawingUnapprovedVendor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DrawingUnapprovedVendor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DrawingUnapprovedVendor") = value
        End Set

    End Property

    Protected Sub odsDrawingApprovedVendor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDrawingApprovedVendor.Selected

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As Drawings.DrawingApprovedVendor_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingApprovedVendor_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DrawingApprovedVendor = True
            Else
                LoadDataEmpty_DrawingApprovedVendor = False
            End If
        End If

    End Sub

    Protected Sub gvDrawingApprovedVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDrawingApprovedVendor.DataBound

        'hide header of first column
        If gvDrawingApprovedVendor.Rows.Count > 0 Then
            gvDrawingApprovedVendor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvDrawingApprovedVendor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDrawingApprovedVendor.RowCommand

        Try

            ClearMessages()

            Dim ddDrawingApprovedVendorTemp As DropDownList
            Dim txtSubVendorName As TextBox
            Dim txtVendorBrand As TextBox
            Dim txtVendorPartNo As TextBox
            Dim txtVendorNotes As TextBox
            Dim txtVendorApprovalDate As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("DrawingNo") <> "") Then

                ddDrawingApprovedVendorTemp = CType(gvDrawingApprovedVendor.FooterRow.FindControl("ddInsertDrawingApprovedVendor"), DropDownList)
                txtSubVendorName = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertSubVendorName"), TextBox)
                txtVendorBrand = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorBrand"), TextBox)
                txtVendorPartNo = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorPartNo"), TextBox)
                txtVendorNotes = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorNotes"), TextBox)
                txtVendorApprovalDate = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorApprovalDate"), TextBox)

                If ddDrawingApprovedVendorTemp.SelectedIndex > 0 Then
                    odsDrawingApprovedVendor.InsertParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
                    odsDrawingApprovedVendor.InsertParameters("UGNDBVendorID").DefaultValue = ddDrawingApprovedVendorTemp.SelectedValue
                    odsDrawingApprovedVendor.InsertParameters("SubVendorName").DefaultValue = txtSubVendorName.Text
                    odsDrawingApprovedVendor.InsertParameters("VendorBrand").DefaultValue = txtVendorBrand.Text
                    odsDrawingApprovedVendor.InsertParameters("VendorPartNo").DefaultValue = txtVendorPartNo.Text
                    odsDrawingApprovedVendor.InsertParameters("VendorNotes").DefaultValue = txtVendorNotes.Text
                    odsDrawingApprovedVendor.InsertParameters("VendorApprovalDate").DefaultValue = txtVendorApprovalDate.Text

                    intRowsAffected = odsDrawingApprovedVendor.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br />"
                Else
                    lblMessage.Text = "Error: A vendor must be selected. The record was NOT saved.<br />"
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDrawingApprovedVendor.ShowFooter = False
            Else
                gvDrawingApprovedVendor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddDrawingApprovedVendorTemp = CType(gvDrawingApprovedVendor.FooterRow.FindControl("ddInsertDrawingApprovedVendor"), DropDownList)
                ddDrawingApprovedVendorTemp.SelectedIndex = -1

                txtSubVendorName = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertSubVendorName"), TextBox)
                txtSubVendorName.Text = ""

                txtVendorBrand = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorBrand"), TextBox)
                txtVendorBrand.Text = ""

                txtVendorPartNo = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorPartNo"), TextBox)
                txtVendorPartNo.Text = ""

                txtVendorNotes = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorNotes"), TextBox)
                txtVendorNotes.Text = ""

                txtVendorApprovalDate = CType(gvDrawingApprovedVendor.FooterRow.FindControl("txtInsertVendorApprovalDate"), TextBox)
                txtVendorApprovalDate.Text = ""
            End If


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageVendor.Text = lblMessage.Text
        lblMessageVendorBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvDrawingApprovedVendor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingApprovedVendor.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DrawingApprovedVendor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub odsDrawingUnapprovedVendor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDrawingUnapprovedVendor.Selected


        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)

        Dim dt As Drawings.DrawingUnapprovedVendor_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingUnapprovedVendor_MaintDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt IsNot Nothing Then
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DrawingUnapprovedVendor = True
            Else
                LoadDataEmpty_DrawingUnapprovedVendor = False
            End If
        End If

    End Sub

    Protected Sub gvDrawingUnapprovedVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDrawingUnapprovedVendor.DataBound

        'hide header of first column
        If gvDrawingUnapprovedVendor.Rows.Count > 0 Then
            gvDrawingUnapprovedVendor.HeaderRow.Cells(0).Visible = False
        End If

    End Sub

    Protected Sub gvDrawingUnapprovedVendor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDrawingUnapprovedVendor.RowCommand

        Try

            ClearMessages()

            Dim txtDrawingUnapprovedVendorNameTemp As TextBox
            Dim txtDrawingUnapprovedVendorNotesTemp As TextBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert" And ViewState("DrawingNo") <> "") Then

                txtDrawingUnapprovedVendorNameTemp = CType(gvDrawingUnapprovedVendor.FooterRow.FindControl("txtInsertUnapprovedVendorName"), TextBox)
                txtDrawingUnapprovedVendorNotesTemp = CType(gvDrawingUnapprovedVendor.FooterRow.FindControl("txtInsertUnapprovedVendorNotes"), TextBox)

                If txtDrawingUnapprovedVendorNameTemp.Text.Trim <> "" Then
                    odsDrawingUnapprovedVendor.InsertParameters("DrawingNo").DefaultValue = ViewState("DrawingNo")
                    odsDrawingUnapprovedVendor.InsertParameters("VendorName").DefaultValue = txtDrawingUnapprovedVendorNameTemp.Text
                    odsDrawingUnapprovedVendor.InsertParameters("VendorNotes").DefaultValue = txtDrawingUnapprovedVendorNotesTemp.Text

                    intRowsAffected = odsDrawingUnapprovedVendor.Insert()

                    lblMessage.Text = "Record Saved Successfully.<br />"

                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDrawingUnapprovedVendor.ShowFooter = False
            Else
                gvDrawingUnapprovedVendor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtDrawingUnapprovedVendorNameTemp = CType(gvDrawingUnapprovedVendor.FooterRow.FindControl("txtInsertUnapprovedVendorName"), TextBox)
                txtDrawingUnapprovedVendorNameTemp.Text = ""

                txtDrawingUnapprovedVendorNotesTemp = CType(gvDrawingUnapprovedVendor.FooterRow.FindControl("txtInsertUnapprovedVendorNotes"), TextBox)
                txtDrawingUnapprovedVendorNotesTemp.Text = ""
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

    Protected Sub gvDrawingUnapprovedVendor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingUnapprovedVendor.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DrawingUnapprovedVendor
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    Protected Sub ClearSubDrawingInputFields()

        Try
            ViewState("CurrentSubDrawingRow") = 0

            txtSubDrawingNo.Text = ""

            cbSubDrawingCADAvailable.Checked = False

            txtSubDrawingQuantity.Text = ""

            txtSubDrawingNotes.Text = ""

            txtSubDrawingProcess.Text = ""

            txtSubDrawingEquipment.Text = ""

            txtSubDrawingProcessParameters.Text = ""

            btnSaveSubDrawing.Text = "Add SubDrawing"

            btnCancelEditSubDrawing.Visible = False

            'iBtnDecSubDrawing.Visible = False

            'iBtnIncSubDrawing.Visible = False

            lnkViewSubDrawing.Visible = False

            'gvSubDrawings.Columns(gvSubDrawings.Columns.Count - 1).Visible = ViewState("isAdmin")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= "<br />" & ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnCancelEditSubDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEditSubDrawing.Click

        Try
            rfvAppendRevisionNotes.ValidationGroup = "vgDrawing"

            ClearMessages()

            ClearSubDrawingInputFields()

            'gvSubDrawings.SelectedIndex = -1

            ViewState("isOverride") = False

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageBillOfMaterials.Text = lblMessage.Text
        lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnSaveSubDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubDrawing.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim dQuantity As Double = 0

            If txtSubDrawingQuantity.Text.Trim <> "" Then
                dQuantity = CType(txtSubDrawingQuantity.Text.Trim, Double)
            End If

            If txtSubDrawingNo.Text.Trim <> "" Then
                'check for valid subdrawing
                ds = PEModule.GetDrawing(txtSubDrawingNo.Text.Trim)

                'if valid then
                If commonFunctions.CheckDataSet(ds) = True Then

                    If ViewState("CurrentSubDrawingRow") > 0 Then
                        PEModule.UpdateSubDrawing(ViewState("CurrentSubDrawingRow"), txtSubDrawingNo.Text.Trim, _
                            dQuantity, txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, _
                            txtSubDrawingEquipment.Text.Trim, txtSubDrawingProcessParameters.Text.Trim)
                    Else
                        PEModule.InsertSubDrawing(ViewState("DrawingNo"), txtSubDrawingNo.Text.Trim, dQuantity, _
                            txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, _
                            txtSubDrawingEquipment.Text.Trim, txtSubDrawingProcessParameters.Text.Trim)
                    End If

                    rfvAppendRevisionNotes.ValidationGroup = "vgDrawing"

                    'gvSubDrawings.DataBind()

                    lblMessage.Text &= "The SubDrawing information was successfully updated."

                    ClearSubDrawingInputFields()

                    'gvSubDrawings.SelectedIndex = -1

                    If ViewState("isOverride") = True Then
                        If txtAppendRevisionNotes.Text.Trim = "" Then
                            txtAppendRevisionNotes.Text = "Updated BOM"
                        End If

                        btnSave_Click(sender, e)
                    End If

                    Call lnkViewBOMTree_Click(sender, e)
                Else
                    lblMessage.Text &= "<br />Error: This SubDrawing does not exist. Information was not updated."
                End If
            End If

            ViewState("isOverride") = False

            EnableControls()

            CancelEdit()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBillOfMaterials.Text = lblMessage.Text
        lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvDrawingApprovedVendor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingApprovedVendor.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim txtEditVendorNotes As TextBox = TryCast(e.Row.FindControl("txtEditVendorNotes"), TextBox)
                Dim lblEditVendorNotesCharCount As Label = TryCast(e.Row.FindControl("lblEditVendorNotesCharCount"), Label)

                If txtEditVendorNotes IsNot Nothing Then
                    txtEditVendorNotes.Attributes.Add("onkeypress", "return tbLimit();")
                    txtEditVendorNotes.Attributes.Add("onkeyup", "return tbCount(" + lblEditVendorNotesCharCount.ClientID + ");")
                    txtEditVendorNotes.Attributes.Add("maxLength", "100")
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then

                Dim txtInsertVendorNotes As TextBox = CType(e.Row.FindControl("txtInsertVendorNotes"), TextBox)
                Dim lblInsertVendorNotesCharCount As Label = CType(e.Row.FindControl("lblInsertVendorNotesCharCount"), Label)

                If txtInsertVendorNotes IsNot Nothing Then
                    txtInsertVendorNotes.Attributes.Add("onkeypress", "return tbLimit();")
                    txtInsertVendorNotes.Attributes.Add("onkeyup", "return tbCount(" + lblInsertVendorNotesCharCount.ClientID + ");")
                    txtInsertVendorNotes.Attributes.Add("maxLength", "100")
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

        lblMessageBillOfMaterials.Text = lblMessage.Text
        lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvDrawingUnapprovedVendor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingUnapprovedVendor.RowDataBound

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim txtEditUnapprovedVendorNotes As TextBox = TryCast(e.Row.FindControl("txtEditUnapprovedVendorNotes"), TextBox)
                Dim lblEditUnapprovedVendorNotesCharCount As Label = TryCast(e.Row.FindControl("lblEditUnapprovedVendorNotesCharCount"), Label)

                If txtEditUnapprovedVendorNotes IsNot Nothing Then
                    txtEditUnapprovedVendorNotes.Attributes.Add("onkeypress", "return tbLimit();")
                    txtEditUnapprovedVendorNotes.Attributes.Add("onkeyup", "return tbCount(" + lblEditUnapprovedVendorNotesCharCount.ClientID + ");")
                    txtEditUnapprovedVendorNotes.Attributes.Add("maxLength", "100")
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Footer) Then

                Dim txtInsertUnapprovedVendorNotes As TextBox = CType(e.Row.FindControl("txtInsertUnapprovedVendorNotes"), TextBox)
                Dim lblInsertUnapprovedVendorNotesCharCount As Label = CType(e.Row.FindControl("lblInsertUnapprovedVendorNotesCharCount"), Label)

                If txtInsertUnapprovedVendorNotes IsNot Nothing Then
                    txtInsertUnapprovedVendorNotes.Attributes.Add("onkeypress", "return tbLimit();")
                    txtInsertUnapprovedVendorNotes.Attributes.Add("onkeyup", "return tbCount(" + lblInsertUnapprovedVendorNotesCharCount.ClientID + ");")
                    txtInsertUnapprovedVendorNotes.Attributes.Add("maxLength", "100")
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

        lblMessageBillOfMaterials.Text = lblMessage.Text
        lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    End Sub

    Protected Sub btnManageParentDrawings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnManageParentDrawings.Click

        Try
            ClearMessages()

            Response.Redirect("DrawingParentsChange.aspx?DrawingNo=" & ViewState("DrawingNo"), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try

            ClearMessages()

            ViewState("isOverride") = True

            EnableControls()

            lblAppendRevisionNotes.Visible = True
            txtAppendRevisionNotes.Visible = True
            rfvAppendRevisionNotes.Enabled = True

            'btnEdit.Visible = False
            txtAppendRevisionNotes.Text = ""
            'btnDeleteAllCheckedBOM.Visible = ViewState("isOverride")

            Call lnkViewBOMTree_Click(sender, e)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name & "<br />"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_DrawingMaterialSpecRelate() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DrawingMaterialSpecRelate") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DrawingMaterialSpecRelate"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DrawingMaterialSpecRelate") = value
        End Set

    End Property

    Protected Sub odsDrawingMaterialSpecRelate_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDrawingMaterialSpecRelate.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Drawings.DrawingMaterialSpecRelateByDrawingNo_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingMaterialSpecRelateByDrawingNo_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DrawingMaterialSpecRelate = True
            Else
                LoadDataEmpty_DrawingMaterialSpecRelate = False
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
#End Region

    Protected Sub gvDrawingMaterialSpecRelate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDrawingMaterialSpecRelate.RowCommand

        Try
            ClearMessages()

            Dim txtMaterialSpecNoTemp As TextBox
            Dim txtDrawingMaterialSpecNotesTemp As TextBox
            Dim ds As DataSet

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtMaterialSpecNoTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertMaterialSpecNo"), TextBox)
                txtDrawingMaterialSpecNotesTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingMaterialSpecNotes"), TextBox)

                If txtMaterialSpecNoTemp.Text.Trim <> "" And lblDrawingNo.Text.Trim <> "" Then
                    ds = PEModule.GetDrawingMaterialSpec(txtMaterialSpecNoTemp.Text.Trim)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        odsDrawingMaterialSpecRelate.InsertParameters("DrawingNo").DefaultValue = lblDrawingNo.Text.Trim
                        odsDrawingMaterialSpecRelate.InsertParameters("MaterialSpecNo").DefaultValue = txtMaterialSpecNoTemp.Text
                        odsDrawingMaterialSpecRelate.InsertParameters("DrawingMaterialSpecNotes").DefaultValue = txtDrawingMaterialSpecNotesTemp.Text.Trim

                        odsDrawingMaterialSpecRelate.Insert()
                    Else
                        lblMessage.Text = "Error: The Drawing does not exist. Therefore the row could not be saved."
                    End If
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDrawingMaterialSpecRelate.ShowFooter = False
            Else
                gvDrawingMaterialSpecRelate.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtMaterialSpecNoTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertMaterialSpecNo"), TextBox)
                txtMaterialSpecNoTemp.Text = ""

                txtDrawingMaterialSpecNotesTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingMaterialSpecNotes"), TextBox)
                txtDrawingMaterialSpecNotesTemp.Text = ""
            End If


            lblMessageDrawingMaterialRelate.Text = lblMessage.Text

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDrawingMaterialSpecRelate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingMaterialSpecRelate.RowCreated

        Try

            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DrawingMaterialSpecRelate()
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br />"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDrawingMaterialSpecRelate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingMaterialSpecRelate.RowDataBound

        Try

            'hide header columns
            If gvDrawingMaterialSpecRelate.Rows.Count > 0 Then
                gvDrawingMaterialSpecRelate.HeaderRow.Cells(0).Visible = False
            End If

            ' Build the client script to open a popup window containing Material Specifications
            ' Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
               "width=950px," & _
               "height=550px," & _
               "left='+((screen.width-950)/2)+'," & _
               "top='+((screen.height-550)/2)+'," & _
               "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("iBtnSearchMaterialSpecNo"), ImageButton)
                Dim txtInsertMaterialSpecNo As TextBox = CType(e.Row.FindControl("txtInsertMaterialSpecNo"), TextBox)

                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                "../PE/MaterialSpecLookUp.aspx?MaterialSpecNoControlID=" & txtInsertMaterialSpecNo.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','MaterialSpecNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
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

    Protected Sub btnDeleteAllCheckedBOM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteAllCheckedBOM.Click

        Try
            lblMessage.Text = ""

            Dim DrawingBOMListUNparsed As String = Replace(Replace(txtSaveCheckBoxBOMDrawingNo.Text.Trim, " ", ""), ";;", ";")

            If DrawingBOMListUNparsed <> Nothing Then
                Dim DrawingBOMList As String() = DrawingBOMListUNparsed.Split(";")
                Dim ResultList As String = ""

                If ViewState("DrawingNo") <> "" Then

                    For i = 0 To UBound(DrawingBOMList)
                        If DrawingBOMList(i) <> ";" And DrawingBOMList(i).Trim <> "" Then
                            PEModule.DeleteSubDrawingByParentDrawing(ViewState("DrawingNo"), DrawingBOMList(i))
                            ResultList &= "<br />" & DrawingBOMList(i)
                        End If
                    Next i

                    txtSaveCheckBoxBOMDrawingNo.Text = ""

                    Call lnkViewBOMTree_Click(sender, e)

                    lblMessage.Text = "Deleted from BOM: " & ResultList
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

        lblMessageBillOfMaterials.Text = lblMessage.Text
        lblMessageBillOfMaterialsBottom.Text = lblMessage.Text

    End Sub

    Private Sub GetProgramInfo(ByVal ProgramID As Integer)

        Try

            Dim ds As DataSet
            Dim strMake As String = ""

            If ddMakes.SelectedIndex >= 0 Then
                strMake = ddMakes.SelectedValue
            End If

            ds = commonFunctions.GetPlatformProgram(0, ProgramID, "", "", strMake)
            If commonFunctions.CheckDataSet(ds) = True Then
                Dim NoOfDays As String = ""
                Select Case ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim
                    Case "01"
                        NoOfDays = "31"
                    Case "02"
                        NoOfDays = "28"
                    Case "03"
                        NoOfDays = "31"
                    Case "04"
                        NoOfDays = "30"
                    Case "05"
                        NoOfDays = "31"
                    Case "06"
                        NoOfDays = "30"
                    Case "07"
                        NoOfDays = "31"
                    Case "08"
                        NoOfDays = "31"
                    Case "09"
                        NoOfDays = "30"
                    Case 10
                        NoOfDays = "31"
                    Case 11
                        NoOfDays = "30"
                    Case 12
                        NoOfDays = "31"
                End Select

                If ds.Tables(0).Rows(0).Item("EOPMM").ToString.Trim <> "" Then
                    txtEOPDate.Text = ds.Tables(0).Rows(0).Item("EOPMM").ToString() & "/" & NoOfDays & "/" & ds.Tables(0).Rows(0).Item("EOPYY").ToString()
                End If

                If ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim <> "" Then
                    txtSOPDate.Text = ds.Tables(0).Rows(0).Item("SOPMM").ToString.Trim & "/01/" & ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim

                    'pick current year if inside SOP and EOP range 
                    If ds.Tables(0).Rows(0).Item("SOPYY") < Today.Year And Today.Year <= ds.Tables(0).Rows(0).Item("EOPYY") Then
                        ddYear.SelectedValue = Today.Year
                    Else
                        ddYear.SelectedValue = ds.Tables(0).Rows(0).Item("SOPYY").ToString.Trim
                    End If

                End If

                '2012-Mar-03 - temporarily disabled - requested by Lynette
                'iBtnPreviewDetail.Visible = True
                'Dim strPreviewClientScript2 As String = "javascript:void(window.open('../DataMaintenance/ProgramDisplay.aspx?pPlatID=0&pPgmID=" & ProgramID & " '," & Now.Ticks.ToString & ",'top=5,left=5,resizable=yes,status=no,toolbar=no,scrollbars=yes,menubar=yes,location=yes'));"
                'iBtnPreviewDetail.Attributes.Add("Onclick", strPreviewClientScript2)
                'Else
                '    iBtnPreviewDetail.Visible = False
            End If

            'End If 'EOF ddProgram.SelectedValue

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
    Protected Sub ddProgram_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddProgram.SelectedIndexChanged

        Try
            If ddProgram.SelectedIndex >= 0 And ddMakes.SelectedIndex >= 0 Then

                ViewState("CurrentCustomerProgramID") = ddProgram.SelectedValue

                GetProgramInfo(ddProgram.SelectedValue)

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
