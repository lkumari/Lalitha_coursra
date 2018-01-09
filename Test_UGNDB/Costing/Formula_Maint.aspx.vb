' ************************************************************************************************
'
' Name:		CostSheetFormulaMaint.aspx
' Purpose:	This Code Behind is to maintain the capital list used by the Costing Module
'
' Date		Author	    
' 10/13/2008 RCarlson
' 11/17/2009 RCarlson   Modified: Refresh query string after initial save
' 06/22/1010 RCarlson   Modified: Use CostingDepartmentList
' 01/11/2011 RCarlson   Modified: Added Email Queue
' 01/10/2012 RCarlson   Modified: Add New Revision fields  
'                                       : DMS Drawing Number Revision should always match, if no DMS Drawing then Formula Revision is 00
'                                       : Only show create revision if a drawing exists - some formulas are not TRUE formulas and have no need for production line testing or DMS drawings
'                                       : Start Date is defaulted to created on date
'                                       : End Date is updated when copied for new revision
'                                       : If End Date exists, then formula is obsolete
'                                       : Lock fields of old revision
'                                       : Require DMS Drawing No, if previous revision had one
'                                       : Show selectable list of previous formula revisions
' 11/05/2012 RCarlson   Modified: For Revision 00, DMS Drawing may be optional since it might be used for Quote Only-Source Quotes
' 01/09/2014    LRey    PartNo field not used. Make fields invisible on webform.
' ************************************************************************************************
Partial Class Formula_Maint
    Inherits System.Web.UI.Page

    Protected Sub ValidateIdentificationNumbers()

        Try
            Dim ds As DataSet

            If txtFormulaDrawingNoValue.Text.Trim <> "" Then
                 ds = PEModule.GetDrawing(txtFormulaDrawingNoValue.Text.Trim)

                If commonFunctions.CheckDataset(ds) = False Then
                    lblMessage.Text &= "<br />WARNING: The DMS drawing number is not in the DMS system. Please contact Product Engineering."
                End If
            End If

            If txtFormulaPartNoValue.Text.Trim <> "" Then
                ds = commonFunctions.GetBPCSPartNo(txtFormulaPartNoValue.Text.Trim, "")
                If commonFunctions.CheckDataset(ds) = False Then
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
    Protected Function SendEmail() As Boolean

        'Many of the crystal reports and calculations depend on the ID's of the formulas. So it would behoove us to track new formulas and templates
        Dim bReturnValue As Boolean = False

        Dim strEmailToAddress As String = ""

        Try

            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            Dim strBody As String = ""
            Dim strSubject As String = ""

            Dim ds As DataSet
            Dim dsTeamMember As DataSet
            Dim iRowCounter As Integer = 0

            'get current user name
            Dim strCurrentUser As String = commonFunctions.getUserName()

            Dim strEmailFromAddress As String = strCurrentUser & "@ugnauto.com"
            Dim strEmailURL As String = strProdOrTestEnvironment & "Costing/Formula_Maint.aspx?FormulaID="

            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strSubject = "TEST PLEASE DISREGARD: "
                strBody = "THIS IS A TEST EMAIL ONLY. PLEASE DISREGARD!!!!!!!!!!!!!!!!!!!!!!!!" & "<br /><br />"
            End If

            'get Formula CC List
            ds = commonFunctions.GetTeamMemberBySubscription(126)
            If commonFunctions.CheckDataSet(ds) = True Then
                For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                    'get email of Team Member
                    If ds.Tables(0).Rows(iRowCounter).Item("TMID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(iRowCounter).Item("TMID") > 0 And ds.Tables(0).Rows(iRowCounter).Item("WorkStatus") = True Then
                            dsTeamMember = SecurityModule.GetTeamMember(ds.Tables(0).Rows(iRowCounter).Item("TMID"), "", "", "", "", "", True, Nothing)
                            If commonFunctions.CheckDataSet(dsTeamMember) = True Then
                                If InStr(strEmailToAddress, dsTeamMember.Tables(0).Rows(0).Item("Email").ToString) <= 0 Then

                                    If strEmailToAddress <> "" Then
                                        strEmailToAddress &= ";"
                                    End If

                                    strEmailToAddress &= dsTeamMember.Tables(0).Rows(0).Item("Email").ToString
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            Dim mail As New MailMessage()

            strSubject &= "A formula was created or updated in the UGNDB Costing Module: " & txtFormulaNameValue.Text

            'If ViewState("OldFormulaID") > 0 Then
            '    strBody &= "It was copied from the Formula ID : " & ViewState("OldFormulaID").ToString & "<br />"
            'End If

            strBody &= "<font size='3' face='Verdana'>A formula was created or updated in the UGNDB Costing Module: " & txtFormulaNameValue.Text & "</font><br /><br />"

            strBody &= "<font size='2' face='Verdana'>The formula ID is: " & lblFormulaIDValue.Text & "</font><br /><br />"

            strBody &= "<font size='2' face='Verdana'>The user who created/updated the formula was " & strCurrentUser & "</font><br /><br />"

            If txtFormulaNameValue.Text.Trim <> ViewState("OriginalFormulaName") Then
                strBody &= "<font size='2' face='Verdana'>The formula name was changed FROM " & ViewState("OriginalFormulaName") & " TO " & txtFormulaNameValue.Text.Trim & "</font><br />"
            End If

            If ViewState("OriginalFormulaStartDate") <> "" And txtFormulaStartDateValue.Text.Trim <> "" Then
                If CType(txtFormulaStartDateValue.Text.Trim, Date) <> CType(ViewState("OriginalFormulaStartDate"), Date) Then
                    strBody &= "<font size='2' face='Verdana'>The formula start date was changed  "

                    If ViewState("OriginalFormulaStartDate") <> "" Then
                        strBody &= " <b>FROM</b> " & ViewState("OriginalFormulaStartDate")
                    End If

                    strBody &= " <b>TO</b> " & txtFormulaStartDateValue.Text.Trim & "</font><br />"
                End If
            End If

            If ViewState("OriginalFormulaEndDate") <> "" And txtFormulaEndDateValue.Text.Trim <> "" Then
                If CType(txtFormulaEndDateValue.Text.Trim, Date) <> CType(ViewState("OriginalFormulaEndDate"), Date) Then

                    strBody &= "<font size='2' face='Verdana'>The formula end date was changed "

                    If ViewState("OriginalFormulaEndDate") <> "" Then
                        strBody &= " <b>FROM</b> " & ViewState("OriginalFormulaEndDate")
                    End If

                    strBody &= " <b>TO</b> " & txtFormulaEndDateValue.Text.Trim & "</font><br />"
                End If
            End If

            If ViewState("OriginalFormulaRevision") <> "" And lblFormulaRevisionValue.Text.Trim <> ViewState("OriginalFormulaRevision") Then
                strBody &= "<font size='2' face='Verdana'>The formula revision was changed "

                If ViewState("OriginalFormulaRevision") <> "" Then
                    strBody &= " <b>FROM</b> " & ViewState("OriginalFormulaRevision")
                End If

                strBody &= " <b>TO</b> " & lblFormulaRevisionValue.Text.Trim & "</font><br />"
            End If

            If txtFormulaDrawingNoValue.Text.Trim <> ViewState("OriginalDrawingNo") Then
                strBody &= "<font size='2' face='Verdana'>The formula DMS drawing number was changed "

                If ViewState("OriginalDrawingNo") <> "" Then
                    strBody &= " <b>FROM</b> " & ViewState("OriginalDrawingNo")
                End If

                strBody &= " <b>TO</b> " & txtFormulaDrawingNoValue.Text.Trim & "</font><br />"
            End If

            If txtFormulaPartNoValue.Text.Trim <> ViewState("OriginalPartNo") Then
                strBody &= "<font size='2' face='Verdana'>The formula BPCS part number was changed  "

                If ViewState("OriginalPartNo") <> "" Then
                    strBody &= "<b>FROM</b> " & ViewState("OriginalPartNo")
                End If

                strBody &= "<b>TO</b> " & txtFormulaPartNoValue.Text.Trim & "</font><br />"
            End If

            If txtFormulaPartRevisionValue.Text.Trim <> ViewState("OriginalPartRevision") Then
                strBody &= "<font size='2' face='Verdana'>The formula BPCS part number was changed "

                If ViewState("OriginalPartRevision") <> "" Then
                    strBody &= "<b>FROM</b> " & ViewState("OriginalPartRevision")
                End If

                strBody &= " <b>TO</b> " & txtFormulaPartRevisionValue.Text.Trim & "</font><br />"
            End If

            strBody &= "<br /><br /><font color='red' size='1' face='Verdana'>HIGHLY RECOMMENDED TIP: OPEN the browser on your computer FIRST. THEN CLICK on the link below.</font><br />"
            strBody &= "<font size='2' face='Verdana'><a href='" & strEmailURL & ViewState("FormulaID") & "'>Click here to review</a></font><br /><br />"

            strBody &= "<font size='2' face='Verdana'>Thank you." & "</font><br />"

            strBody &= "<br /><br /><font size='1' face='Verdana'>"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            strBody &= "<br />If you are receiving this by error, please submit the problem with the UGN Database Requestor, concerning the Costing Module."
            strBody &= "<br />Please <u>do not</u> reply back to this email because you will not receive a response."
            strBody &= "<br />Please use a separate email or phone call the appropriate UGN Team Member(s) for any questions or concerns you may have.<br />"
            strBody &= "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++</font>"

            'When in testing mode, just use developer email address.           
            If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                strBody &= "<br /><br />Email To Address List: " & strEmailToAddress & "<br />"

                strEmailToAddress = "Roderick.Carlson@ugnauto.com"
            End If

            If strEmailToAddress <> "" Then
                'set the content           
                mail.Subject = strSubject
                mail.Body = strBody
                mail.IsBodyHtml = True

                'set the addresses
                mail.From = New MailAddress(strEmailFromAddress)
                Dim i As Integer

                strEmailToAddress = Replace(strEmailToAddress, ";;", ";")

                'to list
                Dim emailList As String() = strEmailToAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        mail.To.Add(emailList(i))
                    End If
                Next i

                'mail.Bcc.Add("Roderick.Carlson@ugnauto.com")

                ' ''send the message 
                Dim smtp As New SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SMTPClient").ToString)

                Try
                    smtp.Send(mail)
                    lblMessage.Text &= "Email Notification sent about formula creation/update."
                Catch ex As Exception
                    lblMessage.Text &= "Email Notification queued about formula creation/update."
                    UGNErrorTrapping.InsertEmailQueue("Formula Maintenance", strEmailFromAddress, strEmailToAddress, "", strSubject, strBody, "")
                End Try

            End If

            bReturnValue = True

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & ", To Email Addresses: " & strEmailToAddress & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        Return bReturnValue

    End Function
    Protected Function HandleBPCSPopUps(ByVal ccPartNo As String, ByVal ccPartRevision As String, ByVal ccPartDescr As String) As String

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
                "../DataMaintenance/PartNoLookUp.aspx?BPCSvcPartNo=" & ccPartNo & "&BPCSvcPartRevision=" & ccPartRevision & "&BPCSvcPartDescr=" & ccPartDescr
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

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 68)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

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

            btnCopy.Visible = False
            btnSave.Visible = False

            iBtnFormulaDrawingNo.Visible = False
            iBtnFormulaPartNo.Visible = False

            lblFormulaIDLabel.Visible = Not ViewState("isRestricted")
            lblFormulaIDValue.Visible = Not ViewState("isRestricted")
            lblFormulaNameLabel.Visible = Not ViewState("isRestricted")
            lblFormulaNameMarker.Visible = Not ViewState("isRestricted")
            txtFormulaNameValue.Visible = Not ViewState("isRestricted")

            lblFormulaRevisionLabel.Visible = Not ViewState("isRestricted")
            lblFormulaRevisionValue.Visible = Not ViewState("isRestricted")

            lblFormulaStartDateMarker.Visible = Not ViewState("isRestricted")
            lblFormulaStartDateLabel.Visible = Not ViewState("isRestricted")
            txtFormulaStartDateValue.Visible = Not ViewState("isRestricted")

            lblFormulaEndDateLabel.Visible = Not ViewState("isRestricted")
            txtFormulaEndDateValue.Visible = Not ViewState("isRestricted")

            'lblFormulaDrawingNoLabelMarker.Visible = Not ViewState("isRestricted")
            lblFormulaDrawingNoLabelMarker.Visible = False
            lblFormulaDrawingNoLabel.Visible = Not ViewState("isRestricted")
            txtFormulaDrawingNoValue.Visible = Not ViewState("isRestricted")
            rfvFormulaDrawingNoValue.Enabled = False

            'lblFormulaDrawingNameLabel.Visible = Not ViewState("isRestricted")
            'lblFormulaDrawingNameValue.Visible = Not ViewState("isRestricted")

            'If lblFormulaDrawingNameValue.Text = "" Then
            lblFormulaDrawingNameValue.Visible = False
            lblFormulaDrawingNameLabel.Visible = False
            'End If

            '(LREY) 01/09/2014
            ' ''lblFormulaPartNoLabel.Visible = Not ViewState("isRestricted")
            ' ''txtFormulaPartNoValue.Visible = Not ViewState("isRestricted")
            ' ''lblFormulaPartRevisionLabel.Visible = Not ViewState("isRestricted")
            ' ''txtFormulaPartRevisionValue.Visible = Not ViewState("isRestricted")

            'If txtFormulaPartRevisionValue.Text = "" Then
            '    txtFormulaPartRevisionValue.Visible = False
            '    lblFormulaPartRevisionLabel.Visible = False
            'End If

            ''lblFormulaPartNameLabel.Visible = Not ViewState("isRestricted")
            ''txtFormulaPartNameValue.Visible = Not ViewState("isRestricted")

            'If txtFormulaPartNameValue.Text = "" Then
            '    txtFormulaPartNameValue.Visible = False
            '    lblFormulaPartNameLabel.Visible = False
            'End If

            lblCreateRevisionReason.Visible = False
            txtCreateRevisionReason.Visible = False
            txtCreateRevisionReason.Enabled = False

            imgFormulaStartDateValue.Visible = False
            imgFormulaEndDateValue.Visible = False

            If cbFleeceTypeValue.Checked = True Then
                lblWeightPerAreaLabel.Visible = Not ViewState("isRestricted")
                txtWeightPerAreaValue.Visible = Not ViewState("isRestricted")
                ddWeightPerAreaUnits.Visible = Not ViewState("isRestricted")

                lblMaximumFormingRateLabel.Visible = Not ViewState("isRestricted")
                txtMaximumFormingRateValue.Visible = Not ViewState("isRestricted")
                ddMaximumFormingRateUnits.Visible = Not ViewState("isRestricted")

                lblSpecificGravityLabel.Visible = False
                txtSpecificGravityValue.Visible = False
                ddSpecificGravityUnits.Visible = False

                lblMaximumMixCapacityLabel.Visible = False
                txtMaximumMixCapacityValue.Visible = False
                ddMaximumMixCapacityUnits.Visible = False
            Else
                lblSpecificGravityLabel.Visible = Not ViewState("isRestricted")
                txtSpecificGravityValue.Visible = Not ViewState("isRestricted")
                ddSpecificGravityUnits.Visible = Not ViewState("isRestricted")

                lblMaximumMixCapacityLabel.Visible = Not ViewState("isRestricted")
                txtMaximumMixCapacityValue.Visible = Not ViewState("isRestricted")
                ddMaximumMixCapacityUnits.Visible = Not ViewState("isRestricted")

                lblWeightPerAreaLabel.Visible = False
                txtWeightPerAreaValue.Visible = False
                ddWeightPerAreaUnits.Visible = False

                lblMaximumFormingRateLabel.Visible = False
                txtMaximumFormingRateValue.Visible = False
                ddMaximumFormingRateUnits.Visible = False
            End If

            lblMaximumLineSpeedLabel.Visible = Not ViewState("isRestricted")
            txtMaximumLineSpeedValue.Visible = Not ViewState("isRestricted")
            ddMaximumLineSpeedUnits.Visible = Not ViewState("isRestricted")
            lblMaximumPressCyclesLabel.Visible = Not ViewState("isRestricted")
            txtMaximumPressCyclesValue.Visible = Not ViewState("isRestricted")

            lblCoatingSidesLabel.Visible = Not ViewState("isRestricted")
            txtCoatingSidesValue.Visible = Not ViewState("isRestricted")

            'lblDepartmentLabel.Visible = Not ViewState("isRestricted")
            'lblDepartmentMarker.Visible = Not ViewState("isRestricted")
            'ddDepartmentValue.Visible = Not ViewState("isRestricted")
            lblDieCutLabel.Visible = Not ViewState("isRestricted")
            cbDiecutValue.Visible = Not ViewState("isRestricted")
            lblProcessLabel.Visible = Not ViewState("isRestricted")
            lblProcessMarker.Visible = Not ViewState("isRestricted")
            ddProcessValue.Visible = Not ViewState("isRestricted")
            lblRecycleReturnLabel.Visible = Not ViewState("isRestricted")
            cbRecycleReturnValue.Visible = Not ViewState("isRestricted")
            lblTemplateLabel.Visible = Not ViewState("isRestricted")
            lblTemplateMarker.Visible = Not ViewState("isRestricted")
            ddTemplateValue.Visible = Not ViewState("isRestricted")
            lblFleeceTypeLabel.Visible = Not ViewState("isRestricted")
            cbFleeceTypeValue.Visible = Not ViewState("isRestricted")
            lblObsoleteLabel.Visible = Not ViewState("isRestricted")
            cbObsoleteValue.Visible = Not ViewState("isRestricted")

            If ViewState("FormulaID") > 0 Then
                menuFormulaTabs.Visible = Not ViewState("isRestricted")

                gvDepartment.Visible = Not ViewState("isRestricted")

                gvFormulaCoatingFactor.Visible = Not ViewState("isRestricted")
                gvFormulaMiscCost.Visible = Not ViewState("isRestricted")
                gvFormulaDeplugFactor.Visible = Not ViewState("isRestricted")
                gvFormulaLabor.Visible = Not ViewState("isRestricted")
                gvFormulaMaterial.Visible = Not ViewState("isRestricted")
                gvFormulaOverhead.Visible = Not ViewState("isRestricted")
                gvFormulaPackaging.Visible = Not ViewState("isRestricted")
            Else
                menuFormulaTabs.Visible = False

                gvDepartment.Visible = False

                gvFormulaCoatingFactor.Visible = False
                gvFormulaMiscCost.Visible = False
                gvFormulaDeplugFactor.Visible = False
                gvFormulaLabor.Visible = False
                gvFormulaMaterial.Visible = False
                gvFormulaOverhead.Visible = False
                gvFormulaPackaging.Visible = False
            End If

            If ViewState("isRestricted") = False Then

                txtFormulaNameValue.Enabled = False

                txtFormulaStartDateValue.Enabled = False
                txtFormulaEndDateValue.Enabled = False

                txtFormulaDrawingNoValue.Enabled = False
                txtFormulaPartNoValue.Enabled = False
                txtFormulaPartRevisionValue.Enabled = False
                txtWeightPerAreaValue.Enabled = False
                txtMaximumFormingRateValue.Enabled = False
                txtSpecificGravityValue.Enabled = False
                txtMaximumMixCapacityValue.Enabled = False
                txtMaximumLineSpeedValue.Enabled = False
                txtMaximumPressCyclesValue.Enabled = False
                txtCoatingSidesValue.Enabled = False
                'ddDepartmentValue.Enabled = ViewState("isAdmin")
                cbDiecutValue.Enabled = False
                ddProcessValue.Enabled = False
                cbRecycleReturnValue.Enabled = False
                ddTemplateValue.Enabled = False
                cbFleeceTypeValue.Enabled = False
                cbObsoleteValue.Enabled = False

                gvDepartment.Columns(gvDepartment.Columns.Count - 1).Visible = False
                If gvDepartment.FooterRow IsNot Nothing Then
                    gvDepartment.FooterRow.Visible = False
                End If

                gvFormulaCoatingFactor.Columns(gvFormulaCoatingFactor.Columns.Count - 1).Visible = False
                If gvFormulaCoatingFactor.FooterRow IsNot Nothing Then
                    gvFormulaCoatingFactor.FooterRow.Visible = False
                End If

                gvFormulaDeplugFactor.Columns(gvFormulaDeplugFactor.Columns.Count - 1).Visible = False
                If gvFormulaDeplugFactor.FooterRow IsNot Nothing Then
                    gvFormulaDeplugFactor.FooterRow.Visible = False
                End If

                gvFormulaMaterial.Columns(gvFormulaMaterial.Columns.Count - 1).Visible = False
                If gvFormulaMaterial.FooterRow IsNot Nothing Then
                    gvFormulaMaterial.FooterRow.Visible = False
                End If

                gvFormulaPackaging.Columns(gvFormulaPackaging.Columns.Count - 1).Visible = False
                If gvFormulaPackaging.FooterRow IsNot Nothing Then
                    gvFormulaPackaging.FooterRow.Visible = False
                End If

                gvFormulaLabor.Columns(gvFormulaLabor.Columns.Count - 1).Visible = False
                If gvFormulaLabor.FooterRow IsNot Nothing Then
                    gvFormulaLabor.FooterRow.Visible = False
                End If

                gvFormulaOverhead.Columns(gvFormulaOverhead.Columns.Count - 1).Visible = False
                If gvFormulaOverhead.FooterRow IsNot Nothing Then
                    gvFormulaOverhead.FooterRow.Visible = False
                End If

                gvFormulaMiscCost.Columns(gvFormulaMiscCost.Columns.Count - 1).Visible = False
                If gvFormulaMiscCost.FooterRow IsNot Nothing Then
                    gvFormulaMiscCost.FooterRow.Visible = False
                End If

                If cbObsoleteValue.Checked = False Then
                    lblFormulaDrawingNoLabelMarker.Visible = ViewState("PreviousRevisionHasDrawing")
                    rfvFormulaDrawingNoValue.Enabled = ViewState("PreviousRevisionHasDrawing")

                    imgFormulaStartDateValue.Visible = ViewState("isAdmin")
                    imgFormulaEndDateValue.Visible = ViewState("isAdmin")

                    btnSave.Visible = ViewState("isAdmin")
                    'iBtnFormulaDrawingNo.Visible = ViewState("isAdmin")
                    ' ''iBtnFormulaPartNo.Visible = ViewState("isAdmin")

                    txtFormulaNameValue.Enabled = ViewState("isAdmin")

                    txtFormulaStartDateValue.Enabled = ViewState("isAdmin")
                    txtFormulaEndDateValue.Enabled = ViewState("isAdmin")

                    'DMS Drawing Number can be updated in same day only or if no drawing number assigned
                    If txtFormulaStartDateValue.Text.Trim <> "" Then
                        If CType(txtFormulaStartDateValue.Text.Trim, Date) = Today.Date Then
                            iBtnFormulaDrawingNo.Visible = ViewState("isAdmin")
                            txtFormulaDrawingNoValue.Enabled = ViewState("isAdmin")
                        End If
                    End If

                    If txtFormulaDrawingNoValue.Text.Trim = "" Then
                        iBtnFormulaDrawingNo.Visible = ViewState("isAdmin")
                        txtFormulaDrawingNoValue.Enabled = ViewState("isAdmin")
                    End If

                    txtFormulaPartNoValue.Enabled = ViewState("isAdmin")
                    txtFormulaPartRevisionValue.Enabled = ViewState("isAdmin")
                    txtWeightPerAreaValue.Enabled = ViewState("isAdmin")
                    txtMaximumFormingRateValue.Enabled = ViewState("isAdmin")
                    txtSpecificGravityValue.Enabled = ViewState("isAdmin")
                    txtMaximumMixCapacityValue.Enabled = ViewState("isAdmin")
                    txtMaximumLineSpeedValue.Enabled = ViewState("isAdmin")
                    txtMaximumPressCyclesValue.Enabled = ViewState("isAdmin")
                    txtCoatingSidesValue.Enabled = ViewState("isAdmin")
                    'ddDepartmentValue.Enabled = ViewState("isAdmin")
                    cbDiecutValue.Enabled = ViewState("isAdmin")
                    ddProcessValue.Enabled = ViewState("isAdmin")
                    cbRecycleReturnValue.Enabled = ViewState("isAdmin")
                    ddTemplateValue.Enabled = ViewState("isAdmin")
                    cbFleeceTypeValue.Enabled = ViewState("isAdmin")
                    cbObsoleteValue.Enabled = ViewState("isAdmin")

                    If ViewState("FormulaID") > 0 Then
                        hlnkNewDrawingNo.Visible = False
                        If txtFormulaDrawingNoValue.Text.Trim <> "" Then
                            'only show create revision if a drawing exists
                            btnCreateRevision.Visible = ViewState("isAdmin")

                            Dim dsDrawing As DataSet
                            dsDrawing = PEModule.GetDrawing(txtFormulaDrawingNoValue.Text.Trim)
                            If commonFunctions.CheckDataSet(dsDrawing) = True Then
                                hlnkNewDrawingNo.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & txtFormulaDrawingNoValue.Text.Trim
                                hlnkNewDrawingNo.Visible = True
                            End If
                        Else
                            txtFormulaNameValue.Enabled = ViewState("isAdmin")
                        End If

                        btnCopy.Visible = ViewState("isAdmin")
                        'btnCreateRevision.Visible = ViewState("isAdmin")

                        gvDepartment.Columns(gvDepartment.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvDepartment.FooterRow IsNot Nothing Then
                            gvDepartment.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaCoatingFactor.Columns(gvFormulaCoatingFactor.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaCoatingFactor.FooterRow IsNot Nothing Then
                            gvFormulaCoatingFactor.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaDeplugFactor.Columns(gvFormulaDeplugFactor.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaDeplugFactor.FooterRow IsNot Nothing Then
                            gvFormulaDeplugFactor.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaMaterial.Columns(gvFormulaMaterial.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaMaterial.FooterRow IsNot Nothing Then
                            gvFormulaMaterial.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaPackaging.Columns(gvFormulaPackaging.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaPackaging.FooterRow IsNot Nothing Then
                            gvFormulaPackaging.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaLabor.Columns(gvFormulaLabor.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaLabor.FooterRow IsNot Nothing Then
                            gvFormulaLabor.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaOverhead.Columns(gvFormulaOverhead.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaOverhead.FooterRow IsNot Nothing Then
                            gvFormulaOverhead.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        gvFormulaMiscCost.Columns(gvFormulaMiscCost.Columns.Count - 1).Visible = ViewState("isAdmin")
                        If gvFormulaMiscCost.FooterRow IsNot Nothing Then
                            gvFormulaMiscCost.FooterRow.Visible = ViewState("isAdmin")
                        End If

                        'If txtCreateRevisionReason.Text.Trim <> "" Then
                        '    txtCreateRevisionReason.CssClass = ""
                        'Else
                        '    txtCreateRevisionReason.CssClass = "none"
                        'End If

                        'lblCreateRevisionReason.Visible = True
                        'txtCreateRevisionReason.Visible = True

                        txtCreateRevisionReason.Enabled = ViewState("isAdmin")


                    End If

                End If

                If txtCreateRevisionReason.Text.Trim <> "" Then
                    lblCreateRevisionReason.Visible = True
                    txtCreateRevisionReason.CssClass = ""
                Else
                    txtCreateRevisionReason.CssClass = "none"
                End If

                txtCreateRevisionReason.Visible = True

                'txtCreateRevisionReason.Enabled = ViewState("isAdmin")

                If ddFormulaRevisions.Items.Count > 1 Then
                    lblFormulaNameRevisionsLabel.Visible = True
                    ddFormulaRevisions.Visible = True
                Else
                    rfvFormulaDrawingNoValue.Enabled = False
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

            'bind existing data to drop down Department 
            ''ds = commonFunctions.GetDepartment("", "", False)
            'ds = CostingModule.GetCostingDepartmentList("", "", False)
            'If commonFunctions.CheckDataset(ds) = True Then                                
            '    ddDepartmentValue.DataSource = ds
            '    ddDepartmentValue.DataTextField = ds.Tables(0).Columns("ddDepartmentName").ColumnName
            '    ddDepartmentValue.DataValueField = ds.Tables(0).Columns("DepartmentID").ColumnName
            '    ddDepartmentValue.DataBind()
            '    ddDepartmentValue.Items.Insert(0, "")            
            'End If

            'bind existing data to drop down Density 
            ds = CostingModule.GetProcess(0, "")
            If commonFunctions.CheckDataset(ds) = True Then
                ddProcessValue.DataSource = ds
                ddProcessValue.DataTextField = ds.Tables(0).Columns("ddProcessName").ColumnName
                ddProcessValue.DataValueField = ds.Tables(0).Columns("ProcessID").ColumnName
                ddProcessValue.DataBind()
                ddProcessValue.Items.Insert(0, "")
            End If

            'bind existing data to drop down Density 
            ds = CostingModule.GetTemplate(0, "")
            If commonFunctions.CheckDataset(ds) = True Then
                ddTemplateValue.DataSource = ds
                ddTemplateValue.DataTextField = ds.Tables(0).Columns("ddTemplateName").ColumnName
                ddTemplateValue.DataValueField = ds.Tables(0).Columns("TemplateID").ColumnName
                ddTemplateValue.DataBind()
                ddTemplateValue.Items.Insert(0, "")
            End If

            'bind units to multiple unit dropdown boxes
            ds = commonFunctions.GetUnit(0, "", "")
            If commonFunctions.CheckDataset(ds) = True Then
                ddSpecificGravityUnits.DataSource = ds
                ddSpecificGravityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddSpecificGravityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddSpecificGravityUnits.DataBind()
                ddSpecificGravityUnits.Items.Insert(0, "")

                ddMaximumLineSpeedUnits.DataSource = ds
                ddMaximumLineSpeedUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMaximumLineSpeedUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMaximumLineSpeedUnits.DataBind()
                ddMaximumLineSpeedUnits.Items.Insert(0, "")

                ddWeightPerAreaUnits.DataSource = ds
                ddWeightPerAreaUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddWeightPerAreaUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddWeightPerAreaUnits.DataBind()
                ddWeightPerAreaUnits.Items.Insert(0, "")

                ddMaximumFormingRateUnits.DataSource = ds
                ddMaximumFormingRateUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMaximumFormingRateUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMaximumFormingRateUnits.DataBind()
                ddMaximumFormingRateUnits.Items.Insert(0, "")

                ddMaximumMixCapacityUnits.DataSource = ds
                ddMaximumMixCapacityUnits.DataTextField = ds.Tables(0).Columns("ddUnitAbbr").ColumnName
                ddMaximumMixCapacityUnits.DataValueField = ds.Tables(0).Columns("UnitID").ColumnName
                ddMaximumMixCapacityUnits.DataBind()
                ddMaximumMixCapacityUnits.Items.Insert(0, "")
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

            Dim ds As DataSet
            Dim dsFormulaRevisions As DataSet
            Dim dsPreviousRevision As DataSet
            Dim iPreviousFormulaID As Integer = 0

            ViewState("PreviousRevisionHasDrawing") = False

            If ViewState("FormulaID") > 0 Then
                'bind existing CostSheet data to for top level cost sheet info                                   
                ds = CostingModule.GetFormula(ViewState("FormulaID"))

                If ViewState("isRestricted") = False Then
                    If commonFunctions.CheckDataset(ds) = True Then

                        txtCreateRevisionReason.Text = ds.Tables(0).Rows(0).Item("CopyReason").ToString

                        If ds.Tables(0).Rows(0).Item("FormulaID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("FormulaID") > 0 Then
                                lblFormulaIDValue.Text = ds.Tables(0).Rows(0).Item("FormulaID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("PreviousFormulaID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("PreviousFormulaID") > 0 Then
                                iPreviousFormulaID = ds.Tables(0).Rows(0).Item("PreviousFormulaID")

                                dsPreviousRevision = CostingModule.GetFormula(iPreviousFormulaID)
                                If commonFunctions.CheckDataSet(dsPreviousRevision) = True Then
                                    If dsPreviousRevision.Tables(0).Rows(0).Item("DrawingNo").ToString.Trim <> "" Then
                                        ViewState("PreviousRevisionHasDrawing") = True
                                    End If
                                End If
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("OriginalFormulaID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("OriginalFormulaID") > 0 Then
                                ViewState("OriginalFormulaID") = ds.Tables(0).Rows(0).Item("OriginalFormulaID")
                            End If
                        End If

                        txtFormulaNameValue.Text = ds.Tables(0).Rows(0).Item("FormulaName").ToString.Trim
                        ViewState("OriginalFormulaName") = txtFormulaNameValue.Text

                        If ds.Tables(0).Rows(0).Item("FormulaName").ToString.Trim <> "" Then
                            dsFormulaRevisions = CostingModule.GetFormulaRevisions(ds.Tables(0).Rows(0).Item("FormulaName").ToString.Trim)

                            If commonFunctions.CheckDataSet(dsFormulaRevisions) = True Then
                                ddFormulaRevisions.DataSource = dsFormulaRevisions
                                ddFormulaRevisions.DataTextField = dsFormulaRevisions.Tables(0).Columns("ddFormulaName").ColumnName
                                ddFormulaRevisions.DataValueField = dsFormulaRevisions.Tables(0).Columns("FormulaID").ColumnName
                                ddFormulaRevisions.DataBind()
                                ddFormulaRevisions.SelectedValue = ViewState("FormulaID")
                            End If

                        End If

                        lblFormulaRevisionValue.Text = ds.Tables(0).Rows(0).Item("FormulaRevision").ToString.Trim
                        ViewState("OriginalFormulaRevision") = lblFormulaRevisionValue.Text

                        txtFormulaStartDateValue.Text = ds.Tables(0).Rows(0).Item("FormulaStartDate").ToString.Trim
                        ViewState("OriginalFormulaStartDate") = txtFormulaStartDateValue.Text

                        txtFormulaEndDateValue.Text = ds.Tables(0).Rows(0).Item("FormulaEndDate").ToString.Trim
                        ViewState("OriginalFormulaEndDate") = txtFormulaEndDateValue.Text

                        txtFormulaDrawingNoValue.Text = ds.Tables(0).Rows(0).Item("DrawingNo").ToString.Trim
                        ViewState("OriginalDrawingNo") = txtFormulaDrawingNoValue.Text

                        txtFormulaPartNoValue.Text = ds.Tables(0).Rows(0).Item("BPCSPartNo").ToString.Trim
                        ViewState("OriginalPartNo") = txtFormulaPartNoValue.Text

                        txtFormulaPartRevisionValue.Text = ds.Tables(0).Rows(0).Item("BPCSPartRevision").ToString.Trim
                        ViewState("OriginalPartRevision") = txtFormulaPartRevisionValue.Text

                        txtFormulaPartNameValue.Text = ds.Tables(0).Rows(0).Item("BPCSPartName").ToString.Trim

                        If ds.Tables(0).Rows(0).Item("SpecificGravity") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("SpecificGravity") > 0 Then
                                txtSpecificGravityValue.Text = ds.Tables(0).Rows(0).Item("SpecificGravity")
                            End If
                        End If

                        'default g/m3
                        ddSpecificGravityUnits.SelectedValue = 16
                        If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("SpecificGravityUnitID") > 0 Then
                                ddSpecificGravityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("SpecificGravityUnitID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("MaxMixCapacity") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxMixCapacity") > 0 Then
                                txtMaximumMixCapacityValue.Text = ds.Tables(0).Rows(0).Item("MaxMixCapacity")
                            End If
                        End If

                        'default kg/hr
                        ddMaximumMixCapacityUnits.SelectedValue = 20
                        If ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID") > 0 Then
                                ddMaximumMixCapacityUnits.SelectedValue = ds.Tables(0).Rows(0).Item("MaxMixCapacityUnitID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("MaxLineSpeed") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxLineSpeed") > 0 Then
                                txtMaximumLineSpeedValue.Text = ds.Tables(0).Rows(0).Item("MaxLineSpeed")
                            End If
                        End If

                        'default m/min
                        ddMaximumLineSpeedUnits.SelectedValue = 19
                        If ds.Tables(0).Rows(0).Item("MaxLineSpeedUnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxLineSpeedUnitID") > 0 Then
                                ddMaximumLineSpeedUnits.SelectedValue = ds.Tables(0).Rows(0).Item("MaxLineSpeedUnitID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("MaxPressCycles") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxPressCycles") > 0 Then
                                txtMaximumPressCyclesValue.Text = ds.Tables(0).Rows(0).Item("MaxPressCycles")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("WeightPerArea") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("WeightPerArea") > 0 Then
                                txtWeightPerAreaValue.Text = ds.Tables(0).Rows(0).Item("WeightPerArea")
                            End If
                        End If

                        'default g/m2
                        ddWeightPerAreaUnits.SelectedValue = 15
                        If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID") > 0 Then
                                ddWeightPerAreaUnits.SelectedValue = ds.Tables(0).Rows(0).Item("WeightPerAreaUnitID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("CoatingSides") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("CoatingSides") > 0 Then
                                txtCoatingSidesValue.Text = ds.Tables(0).Rows(0).Item("CoatingSides")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("MaxFormingRate") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxFormingRate") > 0 Then
                                txtMaximumFormingRateValue.Text = ds.Tables(0).Rows(0).Item("MaxFormingRate")
                            End If
                        End If

                        'default kg/hr
                        ddMaximumFormingRateUnits.SelectedValue = 20
                        If ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID") > 0 Then
                                ddMaximumFormingRateUnits.SelectedValue = ds.Tables(0).Rows(0).Item("MaxFormingRateUnitID")
                            End If
                        End If

                        'If ds.Tables(0).Rows(0).Item("DepartmentID") IsNot System.DBNull.Value Then
                        '    If ds.Tables(0).Rows(0).Item("DepartmentID") > 0 Then
                        '        ddDepartmentValue.SelectedValue = ds.Tables(0).Rows(0).Item("DepartmentID")
                        '    End If
                        'End If

                        If ds.Tables(0).Rows(0).Item("isDiecut") IsNot System.DBNull.Value Then
                            cbDiecutValue.Checked = ds.Tables(0).Rows(0).Item("isDiecut")
                        End If

                        If ds.Tables(0).Rows(0).Item("ProcessID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("ProcessID") > 0 Then
                                ddProcessValue.SelectedValue = ds.Tables(0).Rows(0).Item("ProcessID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("isRecycleReturn") IsNot System.DBNull.Value Then
                            cbRecycleReturnValue.Checked = ds.Tables(0).Rows(0).Item("isRecycleReturn")
                        End If

                        If ds.Tables(0).Rows(0).Item("TemplateID") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("TemplateID") > 0 Then
                                ddTemplateValue.SelectedValue = ds.Tables(0).Rows(0).Item("TemplateID")
                            End If
                        End If

                        If ds.Tables(0).Rows(0).Item("isFleeceType") IsNot System.DBNull.Value Then
                            cbFleeceTypeValue.Checked = ds.Tables(0).Rows(0).Item("isFleeceType")
                        End If

                        If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                            cbObsoleteValue.Checked = ds.Tables(0).Rows(0).Item("Obsolete")
                        End If

                    End If  'end formula load ds is not empty
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Formula Maintenance"


            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("COExtender"), CollapsiblePanelExtender)
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

            If Not Page.IsPostBack Then

                InitializeViewState()

                CheckRights()

                'clear crystal reports
                CostingModule.CleanCostingCrystalReports()

                BindCriteria()

                Dim strClientScript1 As String = HandleBPCSPopUps(txtFormulaPartNoValue.ClientID, txtFormulaPartRevisionValue.ClientID, txtFormulaPartNameValue.ClientID)
                iBtnFormulaPartNo.Attributes.Add("onClick", strClientScript1)

                Dim strClientScript2 As String = HandleDrawingPopUps(txtFormulaDrawingNoValue.ClientID)
                iBtnFormulaDrawingNo.Attributes.Add("onClick", strClientScript2)

                If HttpContext.Current.Request.QueryString("FormulaID") <> "" Then
                    ViewState("FormulaID") = CType(HttpContext.Current.Request.QueryString("FormulaID"), Integer)
                    BindData()

                    'Dim strClientScript1 As String = HandleBPCSPopUps(txtFormulaPartNoValue.ClientID, txtFormulaPartRevisionValue.ClientID, txtFormulaPartNameValue.ClientID)
                    'iBtnFormulaPartNo.Attributes.Add("onClick", strClientScript1)

                    'Dim strClientScript2 As String = HandleDrawingPopUps(txtFormulaDrawingNoValue.ClientID)
                    'iBtnFormulaDrawingNo.Attributes.Add("onClick", strClientScript2)

                    If ViewState("FormulaID") > 0 Then
                        btnCreateRevision.Attributes.Add("onclick", "if(doCreateRevisionReason()){}else{return false};")
                    End If

                    ''***********************************************
                    ''Code Below overrides the breadcrumb navigation 
                    ''***********************************************
                    Dim mpTextBox As Label
                    mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                    If Not mpTextBox Is Nothing Then
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > <a href='Formula_List.aspx'><b> Formula Search </b></a> > Formula Maintenance > <a href='Formula_History.aspx?FormulaID=" & ViewState("FormulaID") & "'> Formula History </a>"
                        mpTextBox.Visible = True
                        Master.FindControl("SiteMapPath1").Visible = False
                    End If
                Else
                    ''***********************************************
                    ''Code Below overrides the breadcrumb navigation 
                    ''***********************************************
                    Dim mpTextBox As Label
                    mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
                    If Not mpTextBox Is Nothing Then
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Costing </b> > <a href='Cost_Sheet_List.aspx'><b> Cost Sheet Search </b></a> > <a href='Formula_List.aspx'><b> Formula Search </b></a> > Formula Maintenance "
                        mpTextBox.Visible = True
                        Master.FindControl("SiteMapPath1").Visible = False
                    End If
                End If

                txtCreateRevisionReason.Attributes.Add("onkeypress", "return tbLimit();")
                txtCreateRevisionReason.Attributes.Add("onkeyup", "return tbCount(" + lblCreateRevisionReasonCharCount.ClientID + ");")
                txtCreateRevisionReason.Attributes.Add("maxLength", "100")

            End If

            If HttpContext.Current.Session("CopyFormula") IsNot Nothing Then
                If HttpContext.Current.Session("CopyFormula").ToString = "copy" Then
                    lblMessage.Text = "The formula was successfully duplicated and saved."
                    HttpContext.Current.Session("CopyFormula") = Nothing
                End If
            End If

            If HttpContext.Current.Session("CopyFormula") IsNot Nothing Then
                If HttpContext.Current.Session("CopyFormula").ToString = "revision" Then
                    lblMessage.Text = "The next revision of the formula was successfully created."
                    HttpContext.Current.Session("CopyFormula") = Nothing
                End If
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

    Protected Sub menuFormulaTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles menuFormulaTabs.MenuItemClick

        Try
            mvFormulas.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvFormulaDeplugFactor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaDeplugFactor.DataBound

        'hide header of first and second column
        If gvFormulaDeplugFactor.Rows.Count > 0 Then
            gvFormulaDeplugFactor.HeaderRow.Cells(0).Visible = False
            gvFormulaDeplugFactor.HeaderRow.Cells(1).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaDeplugFactor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaDeplugFactor.RowCommand

        Try

            Dim txtFormulaDeplugMinimumTemp As TextBox
            Dim txtFormulaDeplugMaximumTemp As TextBox
            Dim txtFormulaDeplugTemp As TextBox
            Dim cbFormulaDeplugFactorObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtFormulaDeplugMinimumTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactorMinimum"), TextBox)
                txtFormulaDeplugMaximumTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactorMaximum"), TextBox)
                txtFormulaDeplugTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactor"), TextBox)
                cbFormulaDeplugFactorObsoleteTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("cbFooterFormulaDeplugFactorObsolete"), CheckBox)

                odsFormulaDeplugFactor.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                odsFormulaDeplugFactor.InsertParameters("MinimumFactor").DefaultValue = txtFormulaDeplugMinimumTemp.Text
                odsFormulaDeplugFactor.InsertParameters("MaximumFactor").DefaultValue = txtFormulaDeplugMaximumTemp.Text
                odsFormulaDeplugFactor.InsertParameters("DeplugFactor").DefaultValue = txtFormulaDeplugTemp.Text
                odsFormulaDeplugFactor.InsertParameters("Obsolete").DefaultValue = cbFormulaDeplugFactorObsoleteTemp.Checked

                intRowsAffected = odsFormulaDeplugFactor.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaDeplugFactor.ShowFooter = False
            Else
                gvFormulaDeplugFactor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtFormulaDeplugMinimumTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactorMinimum"), TextBox)
                txtFormulaDeplugMinimumTemp.Text = Nothing

                txtFormulaDeplugMaximumTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactorMaximum"), TextBox)
                txtFormulaDeplugMaximumTemp.Text = Nothing

                txtFormulaDeplugTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("txtFooterDeplugFactor"), TextBox)
                txtFormulaDeplugTemp.Text = Nothing

                cbFormulaDeplugFactorObsoleteTemp = CType(gvFormulaDeplugFactor.FooterRow.FindControl("cbFooterFormulaDeplugFactorObsolete"), CheckBox)
                cbFormulaDeplugFactorObsoleteTemp.Checked = False

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

    Protected Sub gvFormulaCoatingFactor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaCoatingFactor.DataBound

        'hide header of first and second column
        If gvFormulaCoatingFactor.Rows.Count > 0 Then
            gvFormulaCoatingFactor.HeaderRow.Cells(0).Visible = False
            gvFormulaCoatingFactor.HeaderRow.Cells(1).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaCoatingFactor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaCoatingFactor.RowCommand

        Try

            Dim txtFormulaCoatingMinimumTemp As TextBox
            Dim txtFormulaCoatingMaximumTemp As TextBox
            Dim txtFormulaCoatingTemp As TextBox
            Dim cbFormulaCoatingFactorObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtFormulaCoatingMinimumTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactorMinimum"), TextBox)
                txtFormulaCoatingMaximumTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactorMaximum"), TextBox)
                txtFormulaCoatingTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactor"), TextBox)
                cbFormulaCoatingFactorObsoleteTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("cbFooterFormulaCoatingFactorObsolete"), CheckBox)

                odsFormulaCoatingFactor.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                odsFormulaCoatingFactor.InsertParameters("MinimumFactor").DefaultValue = txtFormulaCoatingMinimumTemp.Text
                odsFormulaCoatingFactor.InsertParameters("MaximumFactor").DefaultValue = txtFormulaCoatingMaximumTemp.Text
                odsFormulaCoatingFactor.InsertParameters("CoatingFactor").DefaultValue = txtFormulaCoatingTemp.Text
                odsFormulaCoatingFactor.InsertParameters("Obsolete").DefaultValue = cbFormulaCoatingFactorObsoleteTemp.Checked

                intRowsAffected = odsFormulaCoatingFactor.Insert()
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaCoatingFactor.ShowFooter = False
            Else
                gvFormulaCoatingFactor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtFormulaCoatingMinimumTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactorMinimum"), TextBox)
                txtFormulaCoatingMinimumTemp.Text = Nothing

                txtFormulaCoatingMaximumTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactorMaximum"), TextBox)
                txtFormulaCoatingMaximumTemp.Text = Nothing

                txtFormulaCoatingTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("txtFooterCoatingFactor"), TextBox)
                txtFormulaCoatingTemp.Text = Nothing

                cbFormulaCoatingFactorObsoleteTemp = CType(gvFormulaCoatingFactor.FooterRow.FindControl("cbFooterFormulaCoatingFactorObsolete"), CheckBox)
                cbFormulaCoatingFactorObsoleteTemp.Checked = False

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

    Protected Sub gvFormulaMaterial_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaMaterial.DataBound

        'hide header of first, second, and third columns
        If gvFormulaMaterial.Rows.Count > 0 Then
            gvFormulaMaterial.HeaderRow.Cells(0).Visible = False
            gvFormulaMaterial.HeaderRow.Cells(1).Visible = False
            'gvFormulaMaterial.HeaderRow.Cells(2).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaMaterial_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaMaterial.RowCommand

        Try

            Dim ddMaterialTemp As DropDownList
            Dim txtMaterialUsageFactorTemp As TextBox
            Dim txtMaterialOrdinalTemp As TextBox
            Dim cbFormulaMaterialObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddMaterialTemp = CType(gvFormulaMaterial.FooterRow.FindControl("ddFooterMaterialName"), DropDownList)

                If ddMaterialTemp.SelectedIndex > 0 Then
                    txtMaterialUsageFactorTemp = CType(gvFormulaMaterial.FooterRow.FindControl("txtFooterFormulaMaterialUsageFactor"), TextBox)
                    txtMaterialOrdinalTemp = CType(gvFormulaMaterial.FooterRow.FindControl("txtFooterFormulaMaterialOrdinal"), TextBox)
                    cbFormulaMaterialObsoleteTemp = CType(gvFormulaMaterial.FooterRow.FindControl("cbFooterFormulaMaterialObsolete"), CheckBox)

                    odsFormulaMaterial.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaMaterial.InsertParameters("MaterialID").DefaultValue = ddMaterialTemp.SelectedValue
                    odsFormulaMaterial.InsertParameters("UsageFactor").DefaultValue = txtMaterialUsageFactorTemp.Text
                    odsFormulaMaterial.InsertParameters("Ordinal").DefaultValue = txtMaterialOrdinalTemp.Text
                    odsFormulaMaterial.InsertParameters("Obsolete").DefaultValue = cbFormulaMaterialObsoleteTemp.Checked

                    intRowsAffected = odsFormulaMaterial.Insert()
                Else
                    lblMessage.Text &= "Error: no material was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaMaterial.ShowFooter = False
            Else
                gvFormulaMaterial.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddMaterialTemp = CType(gvFormulaMaterial.FooterRow.FindControl("ddFooterMaterialName"), DropDownList)
                ddMaterialTemp.SelectedIndex = -1

                txtMaterialUsageFactorTemp = CType(gvFormulaMaterial.FooterRow.FindControl("txtFooterFormulaMaterialUsageFactor"), TextBox)
                txtMaterialUsageFactorTemp.Text = Nothing

                txtMaterialOrdinalTemp = CType(gvFormulaMaterial.FooterRow.FindControl("txtFooterFormulaMaterialOrdinal"), TextBox)
                txtMaterialOrdinalTemp.Text = Nothing

                cbFormulaMaterialObsoleteTemp = CType(gvFormulaMaterial.FooterRow.FindControl("cbFooterFormulaMaterialObsolete"), CheckBox)
                cbFormulaMaterialObsoleteTemp.Checked = False

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvFormulaPackaging_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaPackaging.DataBound

        'hide header of first, second, and third columns
        If gvFormulaPackaging.Rows.Count > 0 Then
            gvFormulaPackaging.HeaderRow.Cells(0).Visible = False
            gvFormulaPackaging.HeaderRow.Cells(1).Visible = False
            'gvFormulaPackaging.HeaderRow.Cells(2).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaPackaging_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaPackaging.RowCommand

        Try

            Dim ddPackagingTemp As DropDownList
            Dim txtPackagingOrdinalTemp As TextBox
            Dim cbFormulaPackagingObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddPackagingTemp = CType(gvFormulaPackaging.FooterRow.FindControl("ddFooterPackagingName"), DropDownList)

                If ddPackagingTemp.SelectedIndex > 0 Then
                    txtPackagingOrdinalTemp = CType(gvFormulaPackaging.FooterRow.FindControl("txtFooterFormulaPackagingOrdinal"), TextBox)
                    cbFormulaPackagingObsoleteTemp = CType(gvFormulaPackaging.FooterRow.FindControl("cbFooterFormulaPackagingObsolete"), CheckBox)

                    odsFormulaPackaging.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaPackaging.InsertParameters("MaterialID").DefaultValue = ddPackagingTemp.SelectedValue
                    odsFormulaPackaging.InsertParameters("Ordinal").DefaultValue = txtPackagingOrdinalTemp.Text
                    odsFormulaPackaging.InsertParameters("Obsolete").DefaultValue = cbFormulaPackagingObsoleteTemp.Checked

                    intRowsAffected = odsFormulaPackaging.Insert()
                Else
                    lblMessage.Text &= "Error: no material packaging was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaPackaging.ShowFooter = False
            Else
                gvFormulaPackaging.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddPackagingTemp = CType(gvFormulaPackaging.FooterRow.FindControl("ddFooterPackagingName"), DropDownList)
                ddPackagingTemp.SelectedIndex = -1

                txtPackagingOrdinalTemp = CType(gvFormulaPackaging.FooterRow.FindControl("txtFooterFormulaPackagingOrdinal"), TextBox)
                txtPackagingOrdinalTemp.Text = Nothing

                cbFormulaPackagingObsoleteTemp = CType(gvFormulaPackaging.FooterRow.FindControl("cbFooterFormulaPackagingObsolete"), CheckBox)
                cbFormulaPackagingObsoleteTemp.Checked = False

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvFormulaLabor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaLabor.DataBound

        'hide header of first, second, and third columns
        If gvFormulaLabor.Rows.Count > 0 Then
            gvFormulaLabor.HeaderRow.Cells(0).Visible = False
            gvFormulaLabor.HeaderRow.Cells(1).Visible = False
            gvFormulaLabor.HeaderRow.Cells(2).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaLabor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaLabor.RowCommand

        Try

            Dim ddLaborDescTemp As DropDownList
            Dim txtLaborOrdinalTemp As TextBox
            Dim cbFormulaLaborObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddLaborDescTemp = CType(gvFormulaLabor.FooterRow.FindControl("ddFooterLaborDesc"), DropDownList)

                If ddLaborDescTemp.SelectedIndex > 0 Then
                    txtLaborOrdinalTemp = CType(gvFormulaLabor.FooterRow.FindControl("txtFooterFormulaLaborOrdinal"), TextBox)
                    cbFormulaLaborObsoleteTemp = CType(gvFormulaLabor.FooterRow.FindControl("cbFooterFormulaLaborObsolete"), CheckBox)

                    odsFormulaLabor.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaLabor.InsertParameters("LaborID").DefaultValue = ddLaborDescTemp.SelectedValue
                    odsFormulaLabor.InsertParameters("Ordinal").DefaultValue = txtLaborOrdinalTemp.Text
                    odsFormulaLabor.InsertParameters("Obsolete").DefaultValue = cbFormulaLaborObsoleteTemp.Checked

                    intRowsAffected = odsFormulaLabor.Insert()
                Else
                    lblMessage.Text &= "Error: no labor was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaLabor.ShowFooter = False
            Else
                gvFormulaLabor.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddLaborDescTemp = CType(gvFormulaLabor.FooterRow.FindControl("ddFooterLaborDesc"), DropDownList)
                ddLaborDescTemp.SelectedIndex = -1

                txtLaborOrdinalTemp = CType(gvFormulaLabor.FooterRow.FindControl("txtFooterFormulaLaborOrdinal"), TextBox)
                txtLaborOrdinalTemp.Text = Nothing

                cbFormulaLaborObsoleteTemp = CType(gvFormulaLabor.FooterRow.FindControl("cbFooterFormulaLaborObsolete"), CheckBox)
                cbFormulaLaborObsoleteTemp.Checked = False

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvFormulaOverhead_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaOverhead.DataBound

        'hide header of first, second, and third columns
        If gvFormulaOverhead.Rows.Count > 0 Then
            gvFormulaOverhead.HeaderRow.Cells(0).Visible = False
            gvFormulaOverhead.HeaderRow.Cells(1).Visible = False
            gvFormulaOverhead.HeaderRow.Cells(2).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaOverhead_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaOverhead.RowCommand

        Try

            Dim ddOverheadTemp As DropDownList
            Dim txtOverheadOrdinalTemp As TextBox
            Dim cbFormulaOverheadObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddOverheadTemp = CType(gvFormulaOverhead.FooterRow.FindControl("ddFooterOverheadDesc"), DropDownList)

                If ddOverheadTemp.SelectedIndex > 0 Then
                    txtOverheadOrdinalTemp = CType(gvFormulaOverhead.FooterRow.FindControl("txtFooterFormulaOverheadOrdinal"), TextBox)
                    cbFormulaOverheadObsoleteTemp = CType(gvFormulaOverhead.FooterRow.FindControl("cbFooterFormulaOverheadObsolete"), CheckBox)

                    odsFormulaOverhead.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaOverhead.InsertParameters("LaborID").DefaultValue = ddOverheadTemp.SelectedValue
                    odsFormulaOverhead.InsertParameters("Ordinal").DefaultValue = txtOverheadOrdinalTemp.Text
                    odsFormulaOverhead.InsertParameters("Obsolete").DefaultValue = cbFormulaOverheadObsoleteTemp.Checked

                    intRowsAffected = odsFormulaOverhead.Insert()
                Else
                    lblMessage.Text &= "Error: no overhead was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaOverhead.ShowFooter = False
            Else
                gvFormulaOverhead.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddOverheadTemp = CType(gvFormulaOverhead.FooterRow.FindControl("ddFooterOverheadDesc"), DropDownList)
                ddOverheadTemp.SelectedIndex = -1

                txtOverheadOrdinalTemp = CType(gvFormulaOverhead.FooterRow.FindControl("txtFooterFormulaOverheadOrdinal"), TextBox)
                txtOverheadOrdinalTemp.Text = Nothing

                cbFormulaOverheadObsoleteTemp = CType(gvFormulaOverhead.FooterRow.FindControl("cbFooterFormulaOverheadObsolete"), CheckBox)
                cbFormulaOverheadObsoleteTemp.Checked = False

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

    Protected Sub gvFormulaMiscCost_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFormulaMiscCost.DataBound

        'hide header of first, second, and third columns
        If gvFormulaMiscCost.Rows.Count > 0 Then
            gvFormulaMiscCost.HeaderRow.Cells(0).Visible = False
            gvFormulaMiscCost.HeaderRow.Cells(1).Visible = False
            gvFormulaMiscCost.HeaderRow.Cells(2).Visible = False
        End If

    End Sub
    Protected Sub gvFormulaMiscCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFormulaMiscCost.RowCommand

        Try

            Dim ddMiscCostDescTemp As DropDownList
            Dim txtMiscCostOrdinalTemp As TextBox
            Dim cbFormulaMiscCostObsoleteTemp As CheckBox

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddMiscCostDescTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("ddFooterMiscCostDesc"), DropDownList)

                If ddMiscCostDescTemp.SelectedIndex > 0 Then
                    txtMiscCostOrdinalTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("txtFooterFormulaMiscCostOrdinal"), TextBox)
                    cbFormulaMiscCostObsoleteTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("cbFooterFormulaMiscCostObsolete"), CheckBox)

                    odsFormulaMiscCost.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaMiscCost.InsertParameters("MiscCostID").DefaultValue = ddMiscCostDescTemp.SelectedValue
                    odsFormulaMiscCost.InsertParameters("Ordinal").DefaultValue = txtMiscCostOrdinalTemp.Text
                    odsFormulaMiscCost.InsertParameters("Obsolete").DefaultValue = cbFormulaMiscCostObsoleteTemp.Checked

                    intRowsAffected = odsFormulaMiscCost.Insert()
                Else
                    lblMessage.Text &= "Error: no misc cost was selected to insert."
                End If

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvFormulaMiscCost.ShowFooter = False
            Else
                gvFormulaMiscCost.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddMiscCostDescTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("ddFooterMiscCostDesc"), DropDownList)
                ddMiscCostDescTemp.SelectedIndex = -1

                txtMiscCostOrdinalTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("txtFooterFormulaMiscCostOrdinal"), TextBox)
                txtMiscCostOrdinalTemp.Text = Nothing

                cbFormulaMiscCostObsoleteTemp = CType(gvFormulaMiscCost.FooterRow.FindControl("cbFooterFormulaMiscCostObsolete"), CheckBox)
                cbFormulaMiscCostObsoleteTemp.Checked = False

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageBottom.Text = lblMessage.Text

    End Sub

#Region "Insert Empty GridView Work-Around"
    Private Property LoadDataEmpty_FormulaCoatingFactor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaCoatingFactor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaCoatingFactor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaCoatingFactor") = value
        End Set

    End Property
    Protected Sub odsFormulaCoatingFactor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaCoatingFactor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaCoatingFactor_MaintDataTable = CType(e.ReturnValue, Costing.FormulaCoatingFactor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaCoatingFactor = True
            Else
                LoadDataEmpty_FormulaCoatingFactor = False
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
    Protected Sub gvFormulaCoatingFactor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaCoatingFactor.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaCoatingFactor
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

    Private Property LoadDataEmpty_FormulaDeplugFactor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaDeplugFactor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaDeplugFactor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaDeplugFactor") = value
        End Set

    End Property
    Protected Sub odsFormulaDeplugFactor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaDeplugFactor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaDeplugFactor_MaintDataTable = CType(e.ReturnValue, Costing.FormulaDeplugFactor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaDeplugFactor = True
            Else
                LoadDataEmpty_FormulaDeplugFactor = False
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
    Protected Sub gvFormulaDeplugFactor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaDeplugFactor.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaDeplugFactor
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

    Private Property LoadDataEmpty_FormulaMaterial() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaMaterial") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaMaterial"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaMaterial") = value
        End Set

    End Property
    Protected Sub odsFormulaMaterial_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaMaterial.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaMaterial_MaintDataTable = CType(e.ReturnValue, Costing.FormulaMaterial_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaMaterial = True
            Else
                LoadDataEmpty_FormulaMaterial = False
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
    Protected Sub gvFormulaMaterial_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaMaterial.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaMaterial
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

    Private Property LoadDataEmpty_FormulaPackaging() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaPackaging") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaPackaging"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaPackaging") = value
        End Set

    End Property
    Protected Sub odsFormulaPackaging_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaPackaging.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaPackaging_MaintDataTable = CType(e.ReturnValue, Costing.FormulaPackaging_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaPackaging = True
            Else
                LoadDataEmpty_FormulaPackaging = False
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
    Protected Sub gvFormulaPackaging_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaPackaging.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaPackaging
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

    Private Property LoadDataEmpty_FormulaLabor() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaLabor") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaLabor"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaLabor") = value
        End Set

    End Property
    Protected Sub odsFormulaLabor_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaLabor.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaLabor_MaintDataTable = CType(e.ReturnValue, Costing.FormulaLabor_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaLabor = True
            Else
                LoadDataEmpty_FormulaLabor = False
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
    Protected Sub gvFormulaLabor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaLabor.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaLabor
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

    Private Property LoadDataEmpty_FormulaOverhead() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaOverhead") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaOverhead"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaOverhead") = value
        End Set

    End Property
    Protected Sub odsFormulaOverhead_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaOverhead.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaOverhead_MaintDataTable = CType(e.ReturnValue, Costing.FormulaOverhead_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaOverhead = True
            Else
                LoadDataEmpty_FormulaOverhead = False
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
    Protected Sub gvFormulaOverhead_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaOverhead.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaOverhead
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

    Private Property LoadDataEmpty_FormulaMiscCost() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaMiscCost") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaMiscCost"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaMiscCost") = value
        End Set

    End Property
    Protected Sub odsFormulaMiscCost_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaMiscCost.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaMiscCost_MaintDataTable = CType(e.ReturnValue, Costing.FormulaMiscCost_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaMiscCost = True
            Else
                LoadDataEmpty_FormulaMiscCost = False
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
    Protected Sub gvFormulaMiscCost_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaMiscCost.RowCreated

        Try
            'hide first and second column
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(2).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaMiscCost
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            lblMessage.Text = ""

            Dim ds As DataSet
            Dim dsPreviousFormula As DataSet

            ValidateIdentificationNumbers()

            Dim dTempSpecificGravity As Double = 0
            If txtSpecificGravityValue.Text.Trim <> "" Then
                dTempSpecificGravity = CType(txtSpecificGravityValue.Text, Double)
            End If

            Dim iTempSpecificGravityUnitID As Integer = 0
            If ddSpecificGravityUnits.SelectedIndex > 0 Then
                iTempSpecificGravityUnitID = ddSpecificGravityUnits.SelectedValue
            End If

            Dim iTempMaximumMixCapacity As Integer = 0
            If txtMaximumMixCapacityValue.Text.Trim <> "" Then
                iTempMaximumMixCapacity = CType(txtMaximumMixCapacityValue.Text, Integer)
            End If

            Dim iTempMaximumMixCapacityUnitID As Integer = 0
            If ddMaximumMixCapacityUnits.SelectedIndex > 0 Then
                iTempMaximumMixCapacityUnitID = ddMaximumMixCapacityUnits.SelectedValue
            End If

            Dim iTempMaximumLineSpeed As Integer = 0
            If txtMaximumLineSpeedValue.Text.Trim <> "" Then
                iTempMaximumLineSpeed = CType(txtMaximumLineSpeedValue.Text, Integer)
            End If

            Dim iTempMaximumLineSpeedUnitID As Integer = 0
            If ddMaximumLineSpeedUnits.SelectedIndex > 0 Then
                iTempMaximumLineSpeedUnitID = ddMaximumLineSpeedUnits.SelectedValue
            End If

            Dim iTempMaximumPressCycles As Integer = 0
            If txtMaximumPressCyclesValue.Text.Trim <> "" Then
                iTempMaximumPressCycles = CType(txtMaximumPressCyclesValue.Text, Integer)
            End If

            Dim iTempCoatingSides As Integer = 0
            If txtCoatingSidesValue.Text.Trim <> "" Then
                iTempCoatingSides = CType(txtCoatingSidesValue.Text, Integer)
            End If

            Dim dTempWeightPerArea As Double = 0
            If txtWeightPerAreaValue.Text.Trim <> "" Then
                dTempWeightPerArea = CType(txtWeightPerAreaValue.Text, Double)
            End If

            Dim iTempWeightPerAreaUnitID As Integer = 0
            If ddWeightPerAreaUnits.SelectedIndex > 0 Then
                iTempweightPerAreaUnitID = ddWeightPerAreaUnits.SelectedValue
            End If

            Dim dTempMaximumFormingRate As Double = 0
            If txtMaximumFormingRateValue.Text.Trim <> "" Then
                dTempMaximumFormingRate = CType(txtMaximumFormingRateValue.Text, Double)
            End If

            Dim iTempMaximumFormingRateUnitID As Integer = 0
            If ddMaximumFormingRateUnits.SelectedIndex > 0 Then
                iTempMaximumFormingRateUnitID = ddMaximumFormingRateUnits.SelectedValue
            End If

            'Dim iTempDepartmentID As Integer = 0
            'If ddDepartmentValue.SelectedIndex > 0 Then
            '    iTempDepartmentID = ddDepartmentValue.SelectedValue
            'End If

            Dim iTempProcessID As Integer = 0
            If ddProcessValue.SelectedIndex > 0 Then
                iTempProcessID = ddProcessValue.SelectedValue
            End If

            Dim iTempTemplateID As Integer = 0
            If ddTemplateValue.SelectedIndex > 0 Then
                iTempTemplateID = ddTemplateValue.SelectedValue
            End If

            'formula revision should always reflect drawing revision
            If txtFormulaDrawingNoValue.Text.Trim <> "" Then
                lblFormulaRevisionValue.Text = Mid(txtFormulaDrawingNoValue.Text.Trim, Len(txtFormulaDrawingNoValue.Text.Trim) - 2, 2)
            Else 'if no revision has been set and drawing is empty then use 00
                If lblFormulaRevisionValue.Text.Trim = "" Then
                    lblFormulaRevisionValue.Text = "00"
                End If

                'we should not allow revisions of formulas that do not have DMS Drawings. They could still be updated, but since some formulas do not need drawings or testing, then they may not need revisions
                'its just the formulas that require testing also require drawings and could allow revisions
            End If

            'if creating a revision of the formula, then the start date can NOT be earlier than the previous formula end date
            If ViewState("InsertType") = "R" And ViewState("PreviousFormulaID") > 0 Then
                dsPreviousFormula = CostingModule.GetFormula(ViewState("PreviousFormulaID"))

                If commonFunctions.CheckDataSet(dsPreviousFormula) = True Then
                    Dim strPreviousFormulaEndDate = dsPreviousFormula.Tables(0).Rows(0).Item("FormulaEndDate").ToString.Trim

                    If strPreviousFormulaEndDate <> "" And txtFormulaStartDateValue.Text.Trim <> "" Then
                        If CType(txtFormulaStartDateValue.Text.Trim, Date) < CType(strPreviousFormulaEndDate, Date) Then
                            txtFormulaStartDateValue.Text = CType(strPreviousFormulaEndDate, Date).AddDays(1)
                        End If
                    End If
                End If
            End If

            If txtFormulaStartDateValue.Text.Trim <> "" And txtFormulaEndDateValue.Text.Trim <> "" Then
                If CType(txtFormulaStartDateValue.Text.Trim, Date) > CType(txtFormulaEndDateValue.Text.Trim, Date) Then
                    txtFormulaEndDateValue.Text = ""
                End If
            End If

            If ViewState("FormulaID") = 0 Then
                'default start date to today if empty or if making a revision
                If txtFormulaStartDateValue.Text.Trim = "" Or ViewState("InsertType") = "R" Then
                    txtFormulaStartDateValue.Text = Today.Date
                End If

                'insert                
                ds = CostingModule.InsertFormula(txtFormulaNameValue.Text.Trim, txtFormulaDrawingNoValue.Text.Trim, _
                txtFormulaPartNoValue.Text.Trim, txtFormulaPartRevisionValue.Text.Trim, _
                dTempSpecificGravity, iTempSpecificGravityUnitID, iTempMaximumMixCapacity, _
                iTempMaximumMixCapacityUnitID, iTempMaximumLineSpeed, iTempMaximumLineSpeedUnitID, _
                iTempMaximumPressCycles, iTempCoatingSides, dTempWeightPerArea, iTempWeightPerAreaUnitID, _
                dTempMaximumFormingRate, iTempMaximumFormingRateUnitID, _
                cbDiecutValue.Checked, iTempProcessID, cbRecycleReturnValue.Checked, _
                iTempTemplateID, cbFleeceTypeValue.Checked, lblFormulaRevisionValue.Text.Trim, _
                txtFormulaStartDateValue.Text.Trim, txtFormulaEndDateValue.Text, txtCreateRevisionReason.Text.Trim, _
                ViewState("PreviousFormulaID"), ViewState("OriginalFormulaID"), ViewState("InsertType"))

                If commonFunctions.CheckDataSet(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("NewFormulaID") IsNot System.DBNull.Value Then
                        ViewState("FormulaID") = ds.Tables(0).Rows(0).Item("NewFormulaID")
                        lblFormulaIDValue.Text = ds.Tables(0).Rows(0).Item("NewFormulaID").ToString

                        CostingModule.InsertFormulaHistory(ViewState("FormulaID"), ViewState("TeamMemberID"), "Formula Created")

                        'notify team members of new formula
                        SendEmail()

                        If ViewState("FormulaID") = 0 Then
                            lblMessage.Text &= "There was an error saving the new formula. Please contact IS."
                        Else
                            Response.Redirect("Formula_Maint.aspx?FormulaID=" & ViewState("FormulaID"), False)
                        End If
                    End If
                End If

            Else
                'if end date exists, then set to obsolete
                If txtFormulaEndDateValue.Text.Trim <> "" Then
                    cbObsoleteValue.Checked = True
                End If

                'update
                CostingModule.UpdateFormula(ViewState("FormulaID"), txtFormulaNameValue.Text.Trim, txtFormulaDrawingNoValue.Text.Trim, _
                txtFormulaPartNoValue.Text.Trim, txtFormulaPartRevisionValue.Text.Trim, _
                dTempSpecificGravity, iTempSpecificGravityUnitID, iTempMaximumMixCapacity, _
                iTempMaximumMixCapacityUnitID, iTempMaximumLineSpeed, _
                iTempMaximumLineSpeedUnitID, iTempMaximumPressCycles, _
                iTempCoatingSides, dTempWeightPerArea, iTempWeightPerAreaUnitID, _
                dTempMaximumFormingRate, iTempMaximumFormingRateUnitID, _
                cbDiecutValue.Checked, iTempProcessID, cbRecycleReturnValue.Checked, _
                iTempTemplateID, cbFleeceTypeValue.Checked, lblFormulaRevisionValue.Text.Trim, _
                txtFormulaStartDateValue.Text.Trim, txtFormulaEndDateValue.Text.Trim, _
                txtCreateRevisionReason.Text.Trim, cbObsoleteValue.Checked)

                'notify team member of CERTAIN formula changes
                '(LREY) email notification will not be used. This was related to ChartSpec
                ' ''If txtFormulaNameValue.Text.Trim <> ViewState("OriginalFormulaName") _
                ' ''    Or txtFormulaStartDateValue.Text.Trim <> ViewState("OriginalFormulaStartDate") _
                ' ''    Or txtFormulaEndDateValue.Text.Trim <> ViewState("OriginalFormulaEndDate") _
                ' ''    Or lblFormulaRevisionValue.Text.Trim <> ViewState("OriginalFormulaRevision") _
                ' ''    Or txtFormulaDrawingNoValue.Text.Trim <> ViewState("OriginalDrawingNo") _
                ' ''    Or txtFormulaPartNoValue.Text.Trim <> ViewState("OriginalPartNo") _
                ' ''    Or txtFormulaPartRevisionValue.Text.Trim <> ViewState("OriginalPartRevision") Then

                ' ''    CostingModule.InsertFormulaHistory(ViewState("FormulaID"), ViewState("TeamMemberID"), "Formula Updated Key Factors")

                ' ''    SendEmail()
                ' ''End If

            End If

            txtFormulaPartNameValue.Text = ""
            txtFormulaPartNameValue.Visible = False
            lblFormulaPartNameLabel.Visible = False

            If InStr(lblMessage.Text, "Saved successfully", CompareMethod.Text) <= 0 Then
                lblMessage.Text &= "<br />Saved successfully."
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
    Private Sub InitializeViewState()

        Try

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isEdit") = False

            ViewState("FormulaID") = 0
            ViewState("PreviousFormulaID") = 0
            ViewState("OriginalFormulaID") = 0
            ViewState("InsertType") = "N"

            ViewState("OriginalFormulaName") = ""
            ViewState("OriginalFormulaStartDate") = ""
            ViewState("OriginalFormulaEndDate") = ""
            ViewState("OriginalFormulaRevision") = ""
            ViewState("OriginalDrawingNo") = ""
            ViewState("OriginalPartNo") = ""
            ViewState("OriginalPartRevision") = ""

            ViewState("PreviousRevisionHasDrawing") = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click

        lblMessage.Text = ""

        Try
            'this is a duplicate formula - reset to new name and 00 revision
            ViewState("InsertType") = "N"

            Dim iPreviousFormulaID As Integer = ViewState("FormulaID")

            ViewState("PreviousFormulaID") = iPreviousFormulaID

            If ViewState("OriginalFormulaID") = 0 Then
                ViewState("OriginalFormulaID") = ViewState("PreviousFormulaID")
            End If

            lblFormulaIDValue.Text = ""
            ViewState("FormulaID") = 0

            txtFormulaNameValue.Text = "Copy Of " & txtFormulaNameValue.Text
            txtFormulaDrawingNoValue.Text = ""
            txtFormulaPartNoValue.Text = ""

            lblFormulaRevisionValue.Text = "00"
            txtCreateRevisionReason.Text = ""

            'save top level formula info
            btnSave_Click(sender, e)

            'Formula ID was updated in save event
            Dim iNewFormulaID As Integer = ViewState("FormulaID")

            If iNewFormulaID <> iPreviousFormulaID Then
                'copy formula grids/child tables
                CostingModule.CopyFormulaDepartment(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaCoatingFactor(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaDeplugFactor(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaMaterial(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaPackaging(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaLabor(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaOverhead(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaMiscCost(iNewFormulaID, iPreviousFormulaID)

                CostingModule.CopyFormulaMaterialReplaceObsolete(iNewFormulaID, iPreviousFormulaID)
                CostingModule.CopyFormulaPackagingReplaceObsolete(iNewFormulaID, iPreviousFormulaID)

                'lblMessage.Text = "The formula has been duplicated and saved."

                HttpContext.Current.Session("CopyFormula") = "copy"

                Response.Redirect("Formula_Maint.aspx?FormulaID=" & iNewFormulaID, False)
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


    Protected Sub cbFleeceTypeValue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbFleeceTypeValue.CheckedChanged

        lblMessage.Text = "Please remember to save your changes."

    End Sub

    Protected Sub gvFormulaMaterial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaMaterial.RowDataBound

        Try
            ' Build the client script to open a popup window containing
            ' Materials. Pass the ClientID of the ddFooterDropdown box and Quote Cost Text Box
            ' (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnGetMaterial"), ImageButton)
                Dim ddTempMaterial As DropDownList = CType(e.Row.FindControl("ddFooterMaterialName"), DropDownList)

                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                        "Material_LookUp.aspx?ddMaterialControlID=" & ddTempMaterial.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','Materials','" & _
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

    Protected Sub gvFormulaPackaging_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFormulaPackaging.RowDataBound

        Try
            ' Build the client script to open a popup window containing
            ' Materials. Pass the ClientID of the ddFooterDropdown box and Quote Cost Text Box
            ' (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
                "width=950px," & _
                "height=550px," & _
                "left='+((screen.width-950)/2)+'," & _
                "top='+((screen.height-550)/2)+'," & _
                "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("ibtnGetPackaging"), ImageButton)
                Dim ddTempPackaging As DropDownList = CType(e.Row.FindControl("ddFooterPackagingName"), DropDownList)

                If ibtn IsNot Nothing Then

                    'call material popup but filter only packaging as a default
                    Dim strPagePath As String = _
                        "Material_LookUp.aspx?ddMaterialControlID=" & ddTempPackaging.ClientID & "&isPackaging=1&filterPackaging=1"
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','Materials','" & _
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

    Private Property LoadDataEmpty_FormulaDepartment() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_FormulaDepartment") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_FormulaDepartment"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_FormulaDepartment") = value
        End Set

    End Property

    Protected Sub odsFormulaDepartment_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFormulaDepartment.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Costing.FormulaDepartment_MaintDataTable = CType(e.ReturnValue, Costing.FormulaDepartment_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_FormulaDepartment = True
            Else
                LoadDataEmpty_FormulaDepartment = False
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

    Protected Sub gvDepartment_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDepartment.DataBound

        Try

            'hide header columns
            If gvDepartment.Rows.Count > 0 Then
                gvDepartment.HeaderRow.Cells(0).Visible = False
                gvDepartment.HeaderRow.Cells(1).Visible = False
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

    Protected Sub gvDepartment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDepartment.RowCommand

        Try

            Dim ddDepartmentIDTemp As DropDownList

            Dim intRowsAffected As Integer = 0

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                ddDepartmentIDTemp = CType(gvDepartment.FooterRow.FindControl("ddFooterDepartment"), DropDownList)

                If ddDepartmentIDTemp.SelectedIndex > 0 Then

                    odsFormulaDepartment.InsertParameters("FormulaID").DefaultValue = ViewState("FormulaID")
                    odsFormulaDepartment.InsertParameters("DepartmentID").DefaultValue = ddDepartmentIDTemp.SelectedValue

                    intRowsAffected = odsFormulaDepartment.Insert()
                Else
                    lblMessage.Text &= "Error: No Department was selected to insert."
                End If
            End If
            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDepartment.ShowFooter = False
            Else
                gvDepartment.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                ddDepartmentIDTemp = CType(gvDepartment.FooterRow.FindControl("ddFooterDepartment"), DropDownList)
                ddDepartmentIDTemp.SelectedIndex = -1
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

    Protected Sub gvDepartment_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDepartment.RowCreated

        Try
            'hide first column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_FormulaDepartment
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

    Protected Sub btnCreateRevision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateRevision.Click

        lblMessage.Text = ""

        Try
            Dim bContinue As Boolean = True

            'get next available drawing number
            Dim strNextAvailableDrawingNo As String = ""
            If txtFormulaDrawingNoValue.Text.Trim <> "" Then
                strNextAvailableDrawingNo = PEModule.GetNextDrawingRevision(txtFormulaDrawingNoValue.Text.Trim)

                If strNextAvailableDrawingNo = "" Then

                    bContinue = False
                End If

            End If

            'if the next drawing is not available then do not create a new formula revision
            If bContinue = True Then
                'obsolete current formula
                CostingModule.UpdateFormulaStatus(ViewState("FormulaID"))

                'this is a revision of a formula - revision will be based on next drawing revision - in the save function
                ViewState("InsertType") = "R"

                Dim iPreviousFormulaID As Integer = ViewState("FormulaID")

                ViewState("PreviousFormulaID") = iPreviousFormulaID

                If ViewState("OriginalFormulaID") = 0 Then
                    ViewState("OriginalFormulaID") = ViewState("PreviousFormulaID")
                End If

                lblFormulaIDValue.Text = ""
                ViewState("FormulaID") = 0

                'txtFormulaDrawingNoValue.Text = ""
                txtFormulaDrawingNoValue.Text = strNextAvailableDrawingNo
                txtFormulaPartNoValue.Text = ""

                'save top level formula info
                btnSave_Click(sender, e)

                'Formula ID was updated in save event
                Dim iNewFormulaID As Integer = ViewState("FormulaID")

                If iNewFormulaID <> iPreviousFormulaID Then
                    'copy formula grids/child tables
                    CostingModule.CopyFormulaDepartment(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaCoatingFactor(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaDeplugFactor(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaMaterial(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaPackaging(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaLabor(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaOverhead(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaMiscCost(iNewFormulaID, iPreviousFormulaID)

                    CostingModule.CopyFormulaMaterialReplaceObsolete(iNewFormulaID, iPreviousFormulaID)
                    CostingModule.CopyFormulaPackagingReplaceObsolete(iNewFormulaID, iPreviousFormulaID)

                    'lblMessage.Text = "The formula has been copied and saved."

                    HttpContext.Current.Session("CopyFormula") = "revise"

                    Response.Redirect("Formula_Maint.aspx?FormulaID=" & iNewFormulaID, False)
                End If
            Else
                lblMessage.Text &= "Error: There is no higher revision of the DMS Drawing. Therefore a new revision of the forumala is not possible. Please ask the product development team to create a new DMS Drawing Revision."
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

    Protected Sub ddFormulaRevisions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFormulaRevisions.SelectedIndexChanged

        Try
            lblMessage.Text = ""

            If ddFormulaRevisions.SelectedIndex >= 0 Then
                Response.Redirect("Formula_Maint.aspx?FormulaID=" & ddFormulaRevisions.SelectedValue, False)
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
