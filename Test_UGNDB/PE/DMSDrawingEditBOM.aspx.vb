' ***********************************************************************************************
'
' Name:		DMSDrawingEditBOM.aspx
' Purpose:	This Code Behind is for the Drawing Detail EDIT OF BOM Subdrawings of the DMS
'
' Date		Author	    
' 07/18/2011 Roderick Carlson  
' 09/19/2011 Roderick Carlson - Allow Subdrawing to be replaced

Partial Class DMSDrawingEditBOM
    Inherits System.Web.UI.Page

    Private Sub CheckRights()

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
            lblMessage.Text += "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub InitializeViewState()

        Try

            ViewState("isAdmin") = False           
            ViewState("isEnabled") = False

            ViewState("ParentDrawingNo") = ""
            ViewState("ChildDrawingNo") = ""

            ViewState("CurrentSubDrawingRow") = 0

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindData()

        Try

            Dim bFoundVendor As Boolean = False

            Dim dsParentDrawing As DataSet
            Dim dsSubDrawing As DataSet

            Dim tmpStatus As String = ""

            dsParentDrawing = PEModule.GetDrawing(ViewState("ParentDrawingNo"))

            If commonFunctions.CheckDataSet(dsParentDrawing) = True Then
                'If ds.Tables(0).Rows(0).Item("OldCustomerPartName").ToString <> "" Then
                '    lblOldCustomerPartNameValue.Text = ds.Tables(0).Rows(0).Item("OldCustomerPartName").ToString.Trim
                '    lblOldCustomerPartNameValue.Visible = True
                '    lblOldCustomerPartNameLabel.Visible = True
                'End If

                'If ds.Tables(0).Rows(0).Item("UpdatedBy").ToString <> "" Then
                '    lblLastUpdatedByValue.Text = ds.Tables(0).Rows(0).Item("UpdatedBy").ToString.Trim
                '    lblLastUpdatedByValue.Visible = True
                '    lblLastUpdatedByLabel.Visible = True
                '    lblLastUpdatedOnValue.Text = ds.Tables(0).Rows(0).Item("UpdatedOn").ToString.Trim
                '    lblLastUpdatedOnLabel.Visible = True
                '    lblLastUpdatedOnValue.Visible = True
                'End If

                tmpStatus = dsParentDrawing.Tables(0).Rows(0).Item("approvalstatus").ToString
                
                Select Case tmpStatus
                    Case "A", "I"
                        lblAppendRevisionNotes.Visible = ViewState("isAdmin")
                        txtAppendRevisionNotes.Visible = ViewState("isAdmin")
                        rfvAppendRevisionNotes.Enabled = True
                End Select

                'If ds.Tables(0).Rows(0).Item("CADavailable") IsNot System.DBNull.Value Then
                '    cbCADavailable.Checked = ds.Tables(0).Rows(0).Item("CADavailable")
                'End If

                'If ds.Tables(0).Rows(0).Item("ReleaseTypeID") IsNot System.DBNull.Value Then
                '    If ds.Tables(0).Rows(0).Item("ReleaseTypeID") > 0 Then
                '        ddReleaseType.SelectedValue = ds.Tables(0).Rows(0).Item("ReleaseTypeID")
                '    End If
                'End If
            End If

            dsSubDrawing = PEModule.GetSubDrawing(ViewState("ParentDrawingNo"), ViewState("ChildDrawingNo"), "", "", "", "", 0, "", False)

            If commonFunctions.CheckDataSet(dsSubDrawing) = True Then

                If dsSubDrawing.Tables(0).Rows(0).Item("Obsolete") = False Then
                    lnkViewSubDrawing.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & ViewState("ChildDrawingNo")
                    lnkViewSubDrawing.Visible = True

                    lblChildDrawingName.Text = dsSubDrawing.Tables(0).Rows(0).Item("OldPartName").ToString

                    If dsSubDrawing.Tables(0).Rows(0).Item("RowID") IsNot System.DBNull.Value Then
                        If dsSubDrawing.Tables(0).Rows(0).Item("RowID") > 0 Then
                            ViewState("CurrentSubDrawingRow") = dsSubDrawing.Tables(0).Rows(0).Item("RowID")
                        End If
                    End If

                    txtSubDrawingNo.Text = dsSubDrawing.Tables(0).Rows(0).Item("SubDrawingNo").ToString

                    If dsSubDrawing.Tables(0).Rows(0).Item("CADavailable") IsNot System.DBNull.Value Then
                        cbSubDrawingCADAvailable.Checked = dsSubDrawing.Tables(0).Rows(0).Item("CADavailable")
                    End If

                    txtSubDrawingQuantity.Text = dsSubDrawing.Tables(0).Rows(0).Item("DrawingQuantity").ToString
                    txtSubDrawingNotes.Text = dsSubDrawing.Tables(0).Rows(0).Item("Notes").ToString
                    txtSubDrawingProcess.Text = dsSubDrawing.Tables(0).Rows(0).Item("Process").ToString
                    txtSubDrawingProcessParameters.Text = dsSubDrawing.Tables(0).Rows(0).Item("ProcessParameters").ToString

                Else
                    lblMessage.Text &= "<br>Error: The child drawing has been set to obsolete."
                    ViewState("DisableAll") = True
                End If

            Else
                lblMessage.Text = "Error: The DMS Child Drawing does not exist."
                ViewState("DisableAll") = True
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

    Private Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

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
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    Private Sub HandleCommentFields()

        Try

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
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Private Sub EnableControls()

        Try
            btnSaveSubDrawing.Visible = ViewState("isAdmin")

            iBtnIncSubDrawing.Visible = ViewState("isAdmin")
            iBtnDecSubDrawing.Visible = ViewState("isAdmin")
            ibtnSearchSubDrawing.Visible = ViewState("isAdmin")
            lnkViewSubDrawing.Visible = ViewState("isAdmin")

            txtSubDrawingNo.Enabled = ViewState("isAdmin")
            txtSubDrawingQuantity.Enabled = ViewState("isAdmin")
            txtSubDrawingNotes.Enabled = ViewState("isAdmin")
            txtSubDrawingProcess.Enabled = ViewState("isAdmin")
            txtSubDrawingProcessParameters.Enabled = ViewState("isAdmin")

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

        If Not Page.IsPostBack Then

            InitializeViewState()

            CheckRights()

            If HttpContext.Current.Request.QueryString("ParentDrawingNo") <> "" Then
                ViewState("ParentDrawingNo") = HttpContext.Current.Request.QueryString("ParentDrawingNo")

                If Session("LastParentDrawingNo") IsNot Nothing And Session("LastParentDrawingNo") = ViewState("ParentDrawingNo") And Session("AppendRevisionNotes") IsNot Nothing Then
                    txtAppendRevisionNotes.Text = Session("AppendRevisionNotes")
                End If

                Session("LastParentDrawingNo") = ViewState("ParentDrawingNo")
            End If

            If HttpContext.Current.Request.QueryString("ChildDrawingNo") <> "" Then
                ViewState("ChildDrawingNo") = HttpContext.Current.Request.QueryString("ChildDrawingNo")
            End If

            lblParentDrawingNo.Text = ViewState("ParentDrawingNo")
            lblChildDrawingNo.Text = ViewState("ChildDrawingNo")

            If ViewState("ParentDrawingNo") <> "" And ViewState("ChildDrawingNo") <> "" Then
                BindData()
            End If

            'search current drawingno popup
            Dim strCurrentDrawingNoClientScript As String = HandleDrawingPopUps(txtSubDrawingNo.ClientID)
            ibtnSearchSubDrawing.Attributes.Add("onClick", strCurrentDrawingNoClientScript)

            HandleCommentFields()

        End If

        EnableControls()

    End Sub

    Protected Sub iBtnDecSubDrawing_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnDecSubDrawing.Click

        Try

            ClearMessages()

            Dim ds As DataSet
            Dim strNewSubDrawingNoTemp As String = ""
            Dim dQuantity As Double = 0

            If txtSubDrawingQuantity.Text.Trim <> "" Then
                dQuantity = CType(txtSubDrawingQuantity.Text.Trim, Double)
            End If

            lnkViewSubDrawing.Visible = False
            If txtSubDrawingNo.Text.Trim <> "" And ViewState("CurrentSubDrawingRow") > 0 Then
                'check for valid subdrawing

                ds = PEModule.GetDrawing(txtSubDrawingNo.Text.Trim)

                'if valid then
                If commonFunctions.CheckDataSet(ds) = True Then
                    strNewSubDrawingNoTemp = PEModule.GetPreviousDrawingRevision(txtSubDrawingNo.Text.Trim)

                    If strNewSubDrawingNoTemp <> "" Then
                        PEModule.UpdateSubDrawing(ViewState("CurrentSubDrawingRow"), strNewSubDrawingNoTemp, _
                            dQuantity, txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, _
                            "", txtSubDrawingProcessParameters.Text.Trim)

                        txtSubDrawingNo.Text = strNewSubDrawingNoTemp

                        lnkViewSubDrawing.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & strNewSubDrawingNoTemp
                        lnkViewSubDrawing.Visible = True

                        lblMessage.Text += "The SubDrawing information was successfully updated."

                        lblChildDrawingNo.Text = strNewSubDrawingNoTemp                      
                        ViewState("ChildDrawingNo") = strNewSubDrawingNoTemp
                    Else
                        lblMessage.Text = "An older revision of this subdrawing was not found."
                    End If
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

    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub iBtnIncSubDrawing_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnIncSubDrawing.Click

        Try
            ClearMessages()

            Dim ds As DataSet
            Dim strNewSubDrawingNoTemp As String = ""
            Dim dQuantity As Double = 0

            If txtSubDrawingQuantity.Text.Trim <> "" Then
                dQuantity = CType(txtSubDrawingQuantity.Text.Trim, Double)
            End If

            lnkViewSubDrawing.Visible = False
            If txtSubDrawingNo.Text.Trim <> "" And ViewState("CurrentSubDrawingRow") > 0 Then
                'check for valid subdrawing
                ds = PEModule.GetDrawing(txtSubDrawingNo.Text.Trim)

                'if valid then
                If commonFunctions.CheckDataSet(ds) = True Then
                    strNewSubDrawingNoTemp = PEModule.GetNextDrawingRevision(txtSubDrawingNo.Text.Trim)

                    If strNewSubDrawingNoTemp <> "" Then
                        PEModule.UpdateSubDrawing(ViewState("CurrentSubDrawingRow"), strNewSubDrawingNoTemp, _
                            dQuantity, txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, _
                            "", txtSubDrawingProcessParameters.Text.Trim)

                        txtSubDrawingNo.Text = strNewSubDrawingNoTemp

                        lnkViewSubDrawing.NavigateUrl = "~/PE/DrawingDetail.aspx?DrawingNo=" & strNewSubDrawingNoTemp
                        lnkViewSubDrawing.Visible = True

                        lblMessage.Text += "The SubDrawing information was successfully updated."

                        lblChildDrawingNo.Text = strNewSubDrawingNoTemp                       
                        ViewState("ChildDrawingNo") = strNewSubDrawingNoTemp
                    Else
                        lblMessage.Text = "A newer revision of this subdrawing was not found."
                    End If
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

    Protected Sub btnSaveSubDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSubDrawing.Click

        Try
            ClearMessages()

            Dim ds As DataSet

            Dim dQuantity As Double = 0
            Dim objSubDrawingsBLL As New SubDrawingsBLL

            If ViewState("CurrentSubDrawingRow") > 0 _
                And ViewState("ParentDrawingNo") <> "" _
                And ViewState("ChildDrawingNo") <> "" _
                And txtSubDrawingNo.Text.Trim <> "" _
                And ViewState("ParentDrawingNo").trim <> ViewState("ChildDrawingNo").trim _
                And ViewState("ParentDrawingNo").trim <> txtSubDrawingNo.Text.Trim Then

                ds = PEModule.GetDrawing(txtSubDrawingNo.Text.Trim)
                If commonFunctions.CheckDataSet(ds) = True Then
                    If txtSubDrawingQuantity.Text.Trim <> "" Then
                        dQuantity = CType(txtSubDrawingQuantity.Text.Trim, Double)
                    End If

                    'objSubDrawingsBLL.UpdateSubDrawings(dQuantity, txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, "", txtSubDrawingProcessParameters.Text.Trim, ViewState("CurrentSubDrawingRow"), ViewState("ChildDrawingNo"), ViewState("CurrentSubDrawingRow"))
                    objSubDrawingsBLL.UpdateSubDrawings(dQuantity, txtSubDrawingNotes.Text.Trim, txtSubDrawingProcess.Text.Trim, "", txtSubDrawingProcessParameters.Text.Trim, ViewState("CurrentSubDrawingRow"), txtSubDrawingNo.Text.Trim, ViewState("CurrentSubDrawingRow"))

                    If txtAppendRevisionNotes.Text.Trim <> "" Then
                        'do not save the same message multiple times
                        If Session("AppendRevisionNotes") <> txtAppendRevisionNotes.Text.Trim Then
                            Session("AppendRevisionNotes") = txtAppendRevisionNotes.Text.Trim
                            PEModule.UpdateDrawingAppendRevisionNotes(ViewState("ParentDrawingNo"), vbNewLine & txtAppendRevisionNotes.Text.Trim)
                        End If
                    End If

                    lblMessage.Text = "Sub-Drawing updated successfully."
                Else
                    lblMessage.Text = "Error: Subdrawing does not exist."
                End If
            Else
                lblMessage.Text = "Error: Invalid Subdrawing selected"
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
End Class
