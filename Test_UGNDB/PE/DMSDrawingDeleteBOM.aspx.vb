' ***********************************************************************************************
'
' Name:		DMSDrawingDeleteBOM.aspx
' Purpose:	This Code Behind is for the Drawing Detail DELETE OF BOM Subdrawings of the DMS
'
' Date		Author	    
' 07/25/2011 Roderick Carlson  
Partial Class DMSDrawingDeleteBOM
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
            ViewState("ParentDrawingStatus") = ""

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
                tmpStatus = dsParentDrawing.Tables(0).Rows(0).Item("approvalstatus").ToString
                ViewState("ParentDrawingStatus") = tmpStatus

                Select Case tmpStatus
                    Case "A", "I"
                        lblAppendRevisionNotes.Visible = ViewState("isAdmin")
                        txtAppendRevisionNotes.Visible = ViewState("isAdmin")
                        rfvAppendRevisionNotes.Enabled = True
                End Select
            End If

            dsSubDrawing = PEModule.GetSubDrawing(ViewState("ParentDrawingNo"), ViewState("ChildDrawingNo"), "", "", "", "", 0, "", False)

            If commonFunctions.CheckDataSet(dsSubDrawing) = True Then

                If dsSubDrawing.Tables(0).Rows(0).Item("Obsolete") = False Then

                    lblChildDrawingName.Text = dsSubDrawing.Tables(0).Rows(0).Item("OldPartName").ToString

                    If dsSubDrawing.Tables(0).Rows(0).Item("RowID") IsNot System.DBNull.Value Then
                        If dsSubDrawing.Tables(0).Rows(0).Item("RowID") > 0 Then
                            ViewState("CurrentSubDrawingRow") = dsSubDrawing.Tables(0).Rows(0).Item("RowID")
                        End If
                    End If
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

    Private Sub EnableControls()

        Try
            btnRemoveSubDrawing.Visible = ViewState("isAdmin")
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
            End If

            If HttpContext.Current.Request.QueryString("ChildDrawingNo") <> "" Then
                ViewState("ChildDrawingNo") = HttpContext.Current.Request.QueryString("ChildDrawingNo")
            End If

            lblParentDrawingNo.Text = ViewState("ParentDrawingNo")
            lblChildDrawingNo.Text = ViewState("ChildDrawingNo")

            If ViewState("ParentDrawingNo") <> "" And ViewState("ChildDrawingNo") <> "" Then
                BindData()
            End If

        End If

        EnableControls()

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

    Protected Sub btnRemoveSubDrawing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveSubDrawing.Click

        Try
            ClearMessages()
            Dim objSubDrawingsBLL As New SubDrawingsBLL

            If ViewState("CurrentSubDrawingRow") > 0 And ViewState("ParentDrawingStatus") <> "" And ViewState("ParentDrawingNo") <> "" And ViewState("ChildDrawingNo") <> "" Then
                If ViewState("ParentDrawingStatus") = "A" Or ViewState("ParentDrawingStatus") = "I" And txtAppendRevisionNotes.Text.Trim = "" Then
                    lblMessage.Text = "Sub-Drawing can NOT be removed until an explanation is entered."
                Else
                    objSubDrawingsBLL.DeleteSubDrawings(ViewState("CurrentSubDrawingRow"), ViewState("ParentDrawingStatus"), txtAppendRevisionNotes.Text.Trim)

                    If txtAppendRevisionNotes.Text.Trim <> "" Then
                        PEModule.UpdateDrawingAppendRevisionNotes(ViewState("ParentDrawingNo"), vbNewLine & txtAppendRevisionNotes.Text.Trim)
                    End If

                    lblMessage.Text = "Sub-Drawing removed from BOM."
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
End Class
