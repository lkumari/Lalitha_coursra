' ************************************************************************************************
'
' Name:	        DrawingParentsChange.vb
' Purpose:	    This code is used by the Product Engineering Module to show the BOM of all sub-drawings with the option to check
'               which sub-drawings should have their release-types changed.

' Called From:  DrawingDetail.aspx
'
' Date		        Author	    
' 01/31/2011      	Roderick Carlson			Created  
' 08/22/2011        Roderick Carlson            Set checkbox default to UNchecked
' 12/20/2013        LRey                        Corrected the GetDrawingSearch parameter list
Partial Class DrawingParentsChange
    Inherits System.Web.UI.Page

    Private htDrawingList As New System.Collections.Hashtable

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet
            Dim strNewDrawingNoWithoutRevision As String = ""
            Dim iLeftParenthesisPosition As Integer = 0

            Dim iRowCounter As Integer = 0

            ds = PEModule.GetDrawingReleaseTypeList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddReleaseType.DataSource = ds
                ddReleaseType.DataTextField = ds.Tables(0).Columns("ddReleaseTypeName").ColumnName
                ddReleaseType.DataValueField = ds.Tables(0).Columns("ReleaseTypeID").ColumnName
                ddReleaseType.DataBind()
            End If

            ddCurrentChildDrawingNo.Items.Clear()
            ddNewChildDrawingNo.Items.Clear()

            If ViewState("NewDrawingNo") <> "" Then
                iLeftParenthesisPosition = InStr(ViewState("NewDrawingNo"), "(")

                strNewDrawingNoWithoutRevision = Mid$(ViewState("NewDrawingNo"), 1, iLeftParenthesisPosition - 1)

                ds = PEModule.GetDrawingSearch(strNewDrawingNoWithoutRevision & "%", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, "", "", "", 0)
                'ds = PEModule.GetDrawingRevisions(ViewState("NewDrawingNo"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    For iRowCounter = 0 To ds.Tables(0).Rows.Count - 1
                        Dim liCurrentListItem As New System.Web.UI.WebControls.ListItem
                        Dim liNewListItem As New System.Web.UI.WebControls.ListItem
                        liCurrentListItem.Text = ds.Tables(0).Rows(iRowCounter).Item("DrawingNo").ToString.ToUpper
                        liCurrentListItem.Value = ds.Tables(0).Rows(iRowCounter).Item("DrawingNo").ToString.ToUpper
                        liNewListItem.Text = ds.Tables(0).Rows(iRowCounter).Item("DrawingNo").ToString.ToUpper
                        liNewListItem.Value = ds.Tables(0).Rows(iRowCounter).Item("DrawingNo").ToString.ToUpper
                        ddCurrentChildDrawingNo.Items.Add(liCurrentListItem)
                        ddNewChildDrawingNo.Items.Add(liNewListItem)
                    Next

                    ViewState("CurrentDrawingNo") = PEModule.GetPreviousDrawingRevision(ViewState("NewDrawingNo")).ToUpper
                End If          
            End If

            If ViewState("CurrentDrawingNo") = "" Then
                ViewState("CurrentDrawingNo") = ViewState("NewDrawingNo").ToUpper
            End If

            If ViewState("CurrentDrawingNo") <> "" Then
                ddCurrentChildDrawingNo.SelectedValue = ViewState("CurrentDrawingNo").ToUpper
            End If

            If ViewState("NewDrawingNo") <> "" Then
                ddNewChildDrawingNo.SelectedValue = ViewState("NewDrawingNo").ToUpper
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

    Private Sub BindData(ByVal strDrawingNo As String)

        Try
            Dim ds As DataSet

            ds = PEModule.GetDrawing(strDrawingNo)

            If commonFunctions.CheckDataSet(ds) = True Then

                If ds.Tables(0).Rows(0).Item("ReleaseTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ReleaseTypeID") > 0 Then
                        'ViewState("rootReleaseType") = ds.Tables(0).Rows(0).Item("ddReleaseTypeName")
                        ddReleaseType.SelectedValue = ds.Tables(0).Rows(0).Item("ReleaseTypeID")
                    End If
                End If

                If Trim(ds.Tables(0).Rows(0).Item("OldPartName").ToString) <> "" Then
                    ViewState("rootDrawingName") = ds.Tables(0).Rows(0).Item("OldPartName")
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

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    'If iTeamMemberID = 530 Then
                    '    iTeamMemberID = 694 ' Adam.Miller 
                    'End If

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 35)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True                                   
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                   
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
                    End If
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

    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageButtons.Text = ""
            lblShowMessage.Text = ""
            lblWarning.Text = ""

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

            btnUpdateParent.Visible = ViewState("isAdmin")
            btnSelectAll.Visible = ViewState("isAdmin")
            btnUnselectAll.Visible = ViewState("isAdmin")
            lblCheckInstructions.Visible = ViewState("isAdmin")
            lblParentNewRevisionNotes.Visible = ViewState("isAdmin")
            lblGrandParentNewRevisionNotes.Visible = ViewState("isAdmin")
            txtParentNewRevisionNotes.Visible = ViewState("isAdmin")
            txtGrandParentNewRevisionNotes.Visible = ViewState("isAdmin")

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub DisableFields()

        Try

            btnUpdateParent.Visible = False
            btnSelectAll.Visible = False
            btnUnselectAll.Visible = False

            lblCheckInstructions.Visible = False
            lblParentNewRevisionNotes.Visible = False
            lblGrandParentNewRevisionNotes.Visible = False
            lblNote1.Visible = False
            lblNote2.Visible = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub CheckNodes(ByVal check As Boolean, ByVal node As TreeNode)

        Try
            node.Checked = check

            For Each child As TreeNode In node.ChildNodes
                If Not child.ChildNodes Is Nothing Then
                    CheckNodes(check, child)
                End If
            Next

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function HandleDrawingPopUps(ByVal DrawingControlID As String) As String

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
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & DrawingControlID
            Dim strClientScript As String = _
                "window.open('" & strPagePath & "','DrawingPartNos','" & _
                strWindowAttribs & "');return false;"

            HandleDrawingPopUps = strClientScript

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
            HandleDrawingPopUps = ""
        End Try

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not Page.IsPostBack Then
                'ResetFields()

                CheckRights()

                If ViewState("isAdmin") = True Then
                    'search new drawingno popup
                    Dim strAltDrawingNoClientScript As String = HandleDrawingPopUps(txtAltDrawingNo.ClientID)
                    iBtnAltDrawingNoSearch.Attributes.Add("onClick", strAltDrawingNoClientScript)

                    ViewState("NewDrawingNo") = Trim(HttpContext.Current.Request.QueryString("DrawingNo").ToString)
                    ViewState("OriginalDrawingNo") = Trim(HttpContext.Current.Request.QueryString("DrawingNo").ToString)

                    If ViewState("NewDrawingNo") <> "" Then
                        BindCriteria()

                        If ViewState("CurrentDrawingNo") <> "" Then
                            BindData(ViewState("CurrentDrawingNo"))
                            StartParentList(ViewState("CurrentDrawingNo"))
                        Else
                            lblMessage.Text = "No Previous Revision was found."
                        End If

                    End If

                End If

                EnableControls()

            End If

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manage Parents of Child Drawing"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then

                If ViewState("NewDrawingNo") <> "" Then
                    mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > <a href='DrawingDetail.aspx?DrawingNo=" & ViewState("NewDrawingNo") & " '><b>Drawing Detail</b></a> > Manage Parents of Child Drawing "
                Else
                    If ViewState("OriginalDrawingNo") <> "" Then
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > <a href='DrawingDetail.aspx?DrawingNo=" & ViewState("OriginalDrawingNo") & " '><b>Drawing Detail</b></a> > Manage Parents of Child Drawing "
                    Else
                        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > <a href='DrawingList.aspx'><b>Drawing Detail</b></a> > Manage Parents of Child Drawing "
                    End If

                End If

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

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click

        Try

            ClearMessages()

            For Each node As TreeNode In tvDrawingWhereUsed.Nodes
                node.Checked = True
                CheckNodes(True, node)
            Next

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click

        Try
            ClearMessages

            For Each node As TreeNode In tvDrawingWhereUsed.Nodes
                CheckNodes(False, node)
            Next

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub cmdCheckSelectNode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCheckSelectNode.Click

        Try
            'Checks If Parent Node has Child Node
           ClearMessages

            If tvDrawingWhereUsed.CheckedNodes.Count > 0 Then
                'Display your selected nodes
                For Each node As TreeNode In tvDrawingWhereUsed.CheckedNodes
                    lblShowMessage.Text += node.Text & " "
                Next
            Else
                lblShowMessage.Text = "You did not select any nodes."
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

    Protected Sub BuildTree(ByVal ChildDrawingNo As String, ByVal n As TreeNode)

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
        
            'preventing an infinite loop
            Session("sessionDMSChangeParentRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 999 Then

                ds = PEModule.GetSubDrawing("", ChildDrawingNo, "", "", "", "", 0, "", False)
                If commonFunctions.CheckDataset(ds) = True Then

                    For iCounter = 0 To ds.Tables(0).Rows.Count - 1
                        iRecursionCounter += 1
                        Session("sessionDMSChangeParentRecursionCounter") = iRecursionCounter + 1

                        strParentDrawingNo = ds.Tables(0).Rows(iCounter).Item("DrawingNo").ToString.Trim

                        If strParentDrawingNo <> "" Then
                            dsParent = PEModule.GetDrawing(strParentDrawingNo)

                            If commonFunctions.CheckDataSet(dsParent) = True Then
                                strParentDrawingName = dsParent.Tables(0).Rows(0).Item("OldPartName").ToString.Trim
                            End If

                            'Add to HashTable List, used for printing later
                            If htDrawingList(strParentDrawingNo) Is Nothing Then
                                htDrawingList.Add(strParentDrawingNo, strParentDrawingNo)
                            Else
                                If lblWarning.Text = "" Then
                                    lblWarning.Text = "The following components appear more than once in the Tree View List "
                                End If
                                lblWarning.Text += ": " & strParentDrawingNo
                            End If

                            Dim node As New TreeNode(strParentDrawingNo & "  -  " & strParentDrawingName)  '& " - Level: " & iCurrentRecursionLevel & " - Recursion Counter: " & iRecursionCounter)
                            If n Is Nothing Then
                                'root.Checked = True
                                'root.SelectAction = TreeNodeSelectAction.None
                                'root.ChildNodes.Add(node)
                            Else
                                n.ChildNodes.Add(node)
                                'n.Checked = True                                
                            End If

                            node.Checked = False

                            Session("sessionDMSChangeParentCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                            BuildTree(strParentDrawingNo, node)
                            Session("sessionDMSChangeParentCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                        End If 'end SubDrawings
                    Next 'end iCounter Loop                                                
                End If
            End If 'end check recursion counter

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

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

                'open drawing in new window
                Page.ClientScript.RegisterStartupScript(Me.GetType(), strDrawingNo, "window.open('DrawingDetail.aspx?DrawingNo=" & strDrawingNo & "' ," & Now.Ticks & " ,'resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=yes');", True)
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
    Protected Sub tvNewParentDrawings_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvNewParentDrawings.SelectedNodeChanged

        Try

            Dim iRightParenthesisPlace As Integer = 0
            Dim strDrawingNo As String = ""

            If tvNewParentDrawings.SelectedNode.Text <> "" Then
                iRightParenthesisPlace = InStr(tvNewParentDrawings.SelectedNode.Text, ")")
                strDrawingNo = Mid$(tvNewParentDrawings.SelectedNode.Text, 1, iRightParenthesisPlace)

                'open drawing in new window
                Page.ClientScript.RegisterStartupScript(Me.GetType(), strDrawingNo, "window.open('DrawingDetail.aspx?DrawingNo=" & strDrawingNo & "' ," & Now.Ticks & " ,'resizable=yes,status=yes,toolbar=yes,scrollbars=yes,menubar=yes,location=yes');", True)
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
    Protected Sub StartParentList(ByVal childPartNo As String)

        Try

            tvDrawingWhereUsed.Nodes.Clear()

            'add root node
            Dim strDrawingName As String = ""
            
            Dim rootnode As TreeNode = New TreeNode(childPartNo & " - " & ViewState("rootDrawingName"))
            rootnode.Checked = False
            rootnode.ShowCheckBox = False
            tvDrawingWhereUsed.Nodes.Add(rootnode)

            BuildTree(childPartNo, rootnode)

            'do not show tree if no child nodes exist (no parent parts exist)
            If rootnode.ChildNodes.Count > 0 Then
                tvDrawingWhereUsed.Visible = True                 
            Else
                tvDrawingWhereUsed.Visible = False
                lblMessage.Text &= "<br>No Parent Drawings exist for this child."                
            End If

            tvDrawingWhereUsed.ExpandAll()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub StartNewParentList(ByVal childPartNo As String)

        Try

            tvNewParentDrawings.Nodes.Clear()

            'add root node
            Dim strDrawingName As String = ""

            Dim rootnode As TreeNode = New TreeNode(childPartNo & " - " & ViewState("rootDrawingName"))
            rootnode.Checked = False
            rootnode.ShowCheckBox = False
            tvNewParentDrawings.Nodes.Add(rootnode)

            BuildTree(childPartNo, rootnode)

            'do not show tree if no child nodes exist (no parent parts exist)
            If rootnode.ChildNodes.Count > 0 Then
                tvNewParentDrawings.Visible = True               
                lblNote3.Visible = True
            Else               
                lblMessage.Text &= "<br>No New Parent Drawings exist for this new child."
            End If

            tvNewParentDrawings.ExpandAll()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub ddCurrentChildDrawingNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddCurrentChildDrawingNo.SelectedIndexChanged

        Try
            ClearMessages()

            'ResetFields()

            ViewState("CurrentDrawingNo") = ddCurrentChildDrawingNo.SelectedValue.ToUpper

            ViewState("NewDrawingNo") = PEModule.GetNextDrawingRevision(ViewState("CurrentDrawingNo")).ToUpper

            tvDrawingWhereUsed.Nodes.Clear()

            If ViewState("NewDrawingNo") <> "" Then
                ddNewChildDrawingNo.SelectedValue = ViewState("NewDrawingNo").ToUpper

                If ViewState("CurrentDrawingNo") <> "" Then
                    StartParentList(ViewState("CurrentDrawingNo"))                                        
                End If
            Else
                lblMessage.Text = "There are no higher revisions."
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

    Protected Sub ddNewChildDrawingNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNewChildDrawingNo.SelectedIndexChanged

        Try
            ClearMessages()

            ViewState("NewDrawingNo") = ddNewChildDrawingNo.SelectedValue.ToUpper

            ViewState("CurrentDrawingNo") = PEModule.GetPreviousDrawingRevision(ViewState("NewDrawingNo")).ToUpper

            tvDrawingWhereUsed.Nodes.Clear()

            If ViewState("CurrentDrawingNo") <> "" Then
                BindData(ViewState("CurrentDrawingNo"))
                ddCurrentChildDrawingNo.SelectedValue = ViewState("CurrentDrawingNo").ToUpper

                StartParentList(ViewState("CurrentDrawingNo"))               
            Else
                lblMessage.Text = "No Previous Revision was found."               
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

    Protected Sub btnUpdateParent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateParent.Click

        Try
            ClearMessages()

            Dim strDrawingNo As String = ""
            Dim strNewMiddleParentDrawingNo As String = ""           
            Dim strDrawingList(999) As String

            Dim iDrawingTotalCount As Integer = 0
            Dim iRightParenthesisPlace As Integer = 0

            Dim iCheckDrawingNoCounter As Integer = 0
            Dim iDrawingCounter As Integer = 0
            Dim jDrawingCounter As Integer = 0

            Dim bContinue As Boolean = True
            Dim bFoundDuplicatePart As Boolean = False
         
            Dim ds As DataSet

            Dim iReleaseTypeID As Integer = 0
            If ddReleaseType.SelectedIndex > 0 Then
                iReleaseTypeID = ddReleaseType.SelectedValue
            Else
                iReleaseTypeID = 2
            End If

            If txtAltDrawingNo.Text.Trim <> "" Then
                ds = PEModule.GetDrawing(txtAltDrawingNo.Text.Trim)

                If commonFunctions.CheckDataSet(ds) = True Then
                    ViewState("NewDrawingNo") = txtAltDrawingNo.Text.Trim                    
                Else
                    lblMessage.Text = "Error: The alternate DMS drawing is not available."
                    bContinue = False
                End If            
            End If

            If Session("DMS-Parent-Complete") <> True Then

                If ViewState("CurrentDrawingNo") = ViewState("NewDrawingNo") Then
                    lblMessage.Text = "Error: The current and replacement drawings cannot be the same."
                    bContinue = False
                End If

                If ViewState("NewDrawingNo") = "" Then
                    lblMessage.Text = "Error: Please make sure to select a new revision or alternative drawing."
                    bContinue = False
                End If

                tvNewParentDrawings.Visible = False
                lblNote3.Visible = False

                If bContinue = True _
                    And ViewState("CurrentDrawingNo") <> "" _
                    And ViewState("NewDrawingNo") <> "" _
                    And ViewState("CurrentDrawingNo") <> ViewState("NewDrawingNo") Then

                    'Checks If Parent Node has Child Node
                    If tvDrawingWhereUsed.CheckedNodes.Count > 0 Then
                        'Display your selected nodes
                        For Each node As TreeNode In tvDrawingWhereUsed.CheckedNodes
                            'lblShowMessage.Text += node.Text & " " & node.Parent.Text & " "
                            iRightParenthesisPlace = InStr(node.Text, ")")

                            'get Drawing Number from Current Node
                            strDrawingNo = Mid$(node.Text, 1, iRightParenthesisPlace)

                            'before adding the part to the list, make sure it is not already in there. prevent duplicates
                            For iCheckDrawingNoCounter = 0 To iDrawingTotalCount
                                If strDrawingNo = strDrawingList(iCheckDrawingNoCounter) Then
                                    bFoundDuplicatePart = True
                                End If
                            Next

                            'if a part was found, do not add it again
                            If bFoundDuplicatePart = False Then
                                strDrawingList(iDrawingTotalCount) = strDrawingNo
                                iDrawingTotalCount += 1
                            End If
                            bFoundDuplicatePart = False

                            'lblShowMessage.Text += sPartId & "  //  "
                        Next

                        If iDrawingTotalCount > 0 Then
                            lblNote3.Visible = True

                            ' insert code here to update each drawing to the new release type
                            For iDrawingCounter = 0 To iDrawingTotalCount - 1
                                strNewMiddleParentDrawingNo = ""
                               
                                If strDrawingList(iDrawingCounter) IsNot Nothing And Trim(strDrawingList(iDrawingCounter)) <> "" And iReleaseTypeID > 0 Then

                                    'do not update root drawing (currrent or new child drawing)                             
                                    If strDrawingList(iDrawingCounter) <> ViewState("CurrentDrawingNo") And strDrawingList(iDrawingCounter) <> ViewState("NewDrawingNo") Then

                                        'check if Old Child is a direct Child of Checked Parent
                                        ds = PEModule.GetSubDrawing(strDrawingList(iDrawingCounter), ViewState("CurrentDrawingNo"), "", "", "", "", 0, "", False)

                                        If commonFunctions.CheckDataSet(ds) = True Then
                                            'if the child is a direct child of the checked parent, then create a new parent and insert the new child into the BOM                                    
                                            strNewMiddleParentDrawingNo = CreateNewParentDrawingRevisionForChild(strDrawingList(iDrawingCounter))
                                        End If

                                        'If the child is NOT a direct child of the checked parent, then it must be a grand-child
                                        'Therefore search the checked list to see which new grand parent should be made
                                        'A new parent will be made to be placed into a new grand parent BOM
                                        'HOWEVER, if the new grandparent is NOT checked, then this will NOT occur - no new parent nor grand parent will be made.
                                        'So, for each middle drawing (parent that is used by a grand parent), both the middle (parent) drawing AND the top (grand parent) drawing must be selected to work
                                        'This requires a second loop of the checked list

                                        'strOldMiddleParentDrawingNo = strDrawingList(iDrawingCounter)

                                        'for each parent of middle parent (for each grand parent to child part)
                                        For jDrawingCounter = 0 To iDrawingTotalCount - 1
                                            If strDrawingList(jDrawingCounter) <> ViewState("CurrentDrawingNo") _
                                                And strDrawingList(jDrawingCounter) <> ViewState("NewDrawingNo") _
                                                And strDrawingList(jDrawingCounter) <> strDrawingList(iDrawingCounter) Then

                                                'is the parent/middle part a child of the top/grand parent part?
                                                'ds = PEModule.GetSubDrawing(strDrawingList(jDrawingCounter), strOldMiddleParentDrawingNo, "", "", "", "", 0, "", False)
                                                ds = PEModule.GetSubDrawing(strDrawingList(jDrawingCounter), strDrawingList(iDrawingCounter), "", "", "", "", 0, "", False)

                                                If commonFunctions.CheckDataSet(ds) = True Then

                                                    ''only create new middle part once
                                                    'If bFoundParentOfMiddlePart = False Then
                                                    '    strNewMiddleParentDrawingNo = CreateNewMiddleParentDrawingRevision(strDrawingList(iDrawingCounter))
                                                    '    bFoundParentOfMiddlePart = True
                                                    'End If

                                                    'CreateNewGrandParentDrawingRevisionForMiddleDrawing(strDrawingList(jDrawingCounter), strOldMiddleParentDrawingNo, strNewMiddleParentDrawingNo)
                                                    CreateNewGrandParentDrawingRevisionForMiddleDrawing(strDrawingList(jDrawingCounter), strDrawingList(iDrawingCounter), strNewMiddleParentDrawingNo)
                                                End If
                                            End If
                                        Next

                                        'lblShowMessage.Text &= strDrawingList(iDrawingCounter) & "<br>"
                                    End If

                                End If
                            Next

                            StartNewParentList(ViewState("NewDrawingNo"))

                            DisableFields()

                            Session("DMS-Parent-Complete") = True
                        End If
                    Else
                        lblShowMessage.Text = "No changes were made. You did not select any nodes. "
                    End If
                
                End If
            Else
                DisableFields()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        lblMessageButtons.Text = lblMessage.Text

    End Sub

    Private Function CreateNewParentDrawingRevisionForChild(ByVal OriginalParentDrawingNo As String) As String

        Dim strNewParentDrawingNo As String = ""

        Try
            Dim ds As DataSet
            Dim bNewPartMade As Boolean = False


            'create part revision of existing part
            ds = PEModule.CopyDrawing(OriginalParentDrawingNo, "Rev")

            If commonFunctions.CheckDataSet(ds) = True Then
                strNewParentDrawingNo = ds.Tables(0).Rows(0).Item("newPart")

                'update release type
                PEModule.UpdateDrawingReleaseType(strNewParentDrawingNo, ddReleaseType.SelectedValue)

                'update revision notes of parent
                PEModule.AppendDrawingRevisionNotes(strNewParentDrawingNo, txtParentNewRevisionNotes.Text.Trim)

                'copy image of old parent to new parent
                CopyImage(strNewParentDrawingNo, OriginalParentDrawingNo)

                'copy program/customer info of old parent to new parent
                PEModule.CopyDrawingCustomerProgram(strNewParentDrawingNo, OriginalParentDrawingNo)

                'copy approved vendors of old parent to new parent
                PEModule.CopyDrawingApprovedVendor(strNewParentDrawingNo, OriginalParentDrawingNo)

                'copy unapproved vendors of old parent to new parent
                PEModule.CopyDrawingUnapprovedVendor(strNewParentDrawingNo, OriginalParentDrawingNo)

                'copy bill of materials of old parent to new parent
                PEModule.CopyDrawingBOM(strNewParentDrawingNo, OriginalParentDrawingNo)

                'replace child part in new parent
                PEModule.ReplaceSubDrawing(strNewParentDrawingNo, ViewState("NewDrawingNo"), ViewState("CurrentDrawingNo"))

                bNewPartMade = True
            End If

            If bNewPartMade = False Then
                lblMessage.Text &= "<br>An error occurred while creating a revision of " & OriginalParentDrawingNo & ".  It is possible that the maximum number of revisions has been reached. If not, please contact IS Support."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        CreateNewParentDrawingRevisionForChild = strNewParentDrawingNo

    End Function

    Private Function CreateNewMiddleParentDrawingRevision(ByVal OriginalMiddleParentDrawing As String) As String

        Dim strNewMiddleParentDrawingNo As String = ""

        Try
            Dim dsMiddleParent As DataSet
            Dim bNewPartMade As Boolean = False

            'create part revision of existing part
            dsMiddleParent = PEModule.CopyDrawing(OriginalMiddleParentDrawing, "Rev")

            If commonFunctions.CheckDataSet(dsMiddleParent) = True Then
                strNewMiddleParentDrawingNo = dsMiddleParent.Tables(0).Rows(0).Item("newPart")

                'update release type
                PEModule.UpdateDrawingReleaseType(strNewMiddleParentDrawingNo, ddReleaseType.SelectedValue)

                'update revision notes of parent
                PEModule.AppendDrawingRevisionNotes(strNewMiddleParentDrawingNo, txtGrandParentNewRevisionNotes.Text.Trim)

                'copy image of old parent to new parent
                CopyImage(strNewMiddleParentDrawingNo, OriginalMiddleParentDrawing)

                'copy program/customer info of old parent to new parent
                PEModule.CopyDrawingCustomerProgram(strNewMiddleParentDrawingNo, OriginalMiddleParentDrawing)

                'copy approved vendors of old parent to new parent
                PEModule.CopyDrawingApprovedVendor(strNewMiddleParentDrawingNo, OriginalMiddleParentDrawing)

                'copy unapproved vendors of old parent to new parent
                PEModule.CopyDrawingUnapprovedVendor(strNewMiddleParentDrawingNo, OriginalMiddleParentDrawing)

                'copy bill of materials of old parent to new parent
                PEModule.CopyDrawingBOM(strNewMiddleParentDrawingNo, OriginalMiddleParentDrawing)

                'DO NOT UPDATE BOM OF GRANDPARENT HERE

                bNewPartMade = True

            End If

            If bNewPartMade = False Then
                lblMessage.Text &= "<br>An error occurred while creating a revision of " & OriginalMiddleParentDrawing & ".  It is possible that the maximum number of revisions has been reached. If not, please contact IS Support."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        CreateNewMiddleParentDrawingRevision = strNewMiddleParentDrawingNo

    End Function

    Private Sub CreateNewGrandParentDrawingRevisionForMiddleDrawing(ByVal OriginalGrandParentDrawingNo As String, ByVal OriginalMiddleParentDrawing As String, ByVal NewMiddleParentDrawing As String)

        Try

            Dim dsGrandParent As DataSet
            Dim bNewPartMade As Boolean = False          
            Dim strNewGrandParentDrawingNo As String = ""

            'create part revision of existing part
            dsGrandParent = PEModule.CopyDrawing(OriginalGrandParentDrawingNo, "Rev")

            If commonFunctions.CheckDataSet(dsGrandParent) = True Then
                strNewGrandParentDrawingNo = dsGrandParent.Tables(0).Rows(0).Item("newPart")

                'update release type
                PEModule.UpdateDrawingReleaseType(strNewGrandParentDrawingNo, ddReleaseType.SelectedValue)

                'update revision notes of parent
                PEModule.AppendDrawingRevisionNotes(strNewGrandParentDrawingNo, txtGrandParentNewRevisionNotes.Text.Trim)

                'copy image of old parent to new parent
                CopyImage(strNewGrandParentDrawingNo, OriginalGrandParentDrawingNo)

                'copy program/customer info of old parent to new parent
                PEModule.CopyDrawingCustomerProgram(strNewGrandParentDrawingNo, OriginalGrandParentDrawingNo)

                'copy approved vendors of old parent to new parent
                PEModule.CopyDrawingApprovedVendor(strNewGrandParentDrawingNo, OriginalGrandParentDrawingNo)

                'copy unapproved vendors of old parent to new parent
                PEModule.CopyDrawingUnapprovedVendor(strNewGrandParentDrawingNo, OriginalGrandParentDrawingNo)

                'copy bill of materials of old parent to new parent
                PEModule.CopyDrawingBOM(strNewGrandParentDrawingNo, OriginalGrandParentDrawingNo)

                ''replace child part in new parent
                PEModule.ReplaceSubDrawing(strNewGrandParentDrawingNo, NewMiddleParentDrawing, OriginalMiddleParentDrawing)

                bNewPartMade = True

            End If

            If bNewPartMade = False Then
                lblMessage.Text &= "<br>An error occurred while creating a revision of " & OriginalMiddleParentDrawing & ".  It is possible that the maximum number of revisions has been reached. If not, please contact IS Support."
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

    Protected Sub CopyImage(ByVal NewDrawingNo As String, ByVal OriginalDrawingNo As String)

        Try
            Dim dsImages As DataSet
            Dim TempImageURL As String = ""
            Dim TempImageBytes As Byte()

            dsImages = PEModule.GetDrawingImages(OriginalDrawingNo, "")

            If commonFunctions.CheckDataSet(dsImages) = True Then              

                If dsImages.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
                    TempImageBytes = dsImages.Tables(0).Rows(0).Item("DrawingImage")
                    TempImageURL = dsImages.Tables(0).Rows(0).Item("ImageURL")
                    PEModule.InsertDrawingImage(NewDrawingNo, TempImageURL, TempImageBytes)
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
