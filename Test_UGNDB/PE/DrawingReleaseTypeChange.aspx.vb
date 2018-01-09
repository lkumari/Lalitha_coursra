' ************************************************************************************************
'
' Name:	        DrawingReleaseTypeChange.vb
' Purpose:	    This code is used by the Product Engineering Module to show the BOM of all sub-drawings with the option to check
'               which sub-drawings should have their release-types changed.

' Called From:  DrawingDetail.aspx
'
' Date		        Author	    
' 09/17/2008      	Roderick Carlson			Created .Net application
' 03/06/2009        Roderick Carlson            Adjusted so that only certain release types can be used
' 09/22/2009        Roderick Carlson            Temporarily hiding PartName due to performance hit
' 06/28/2010        Roderick Carlson            PDE-2909 - Release Type Work
' 01/06/2014        LRey                        Replaced "BPCS Part No" to "Part No" wherever used.



Partial Class PE_PE_Drawings_DrawingReleaseTypeChange
    Inherits System.Web.UI.Page
    Private htDrawingList As New System.Collections.Hashtable

    Private Sub BindCriteria()

        Try
            Dim ds As DataSet = New DataSet

            ds = PEModule.GetDrawingReleaseTypeList()
            If commonFunctions.CheckDataset(ds) = True Then
                ddReleaseType.DataSource = ds
                ddReleaseType.DataTextField = ds.Tables(0).Columns("ddReleaseTypeName").ColumnName
                ddReleaseType.DataValueField = ds.Tables(0).Columns("ReleaseTypeID").ColumnName
                ddReleaseType.DataBind()
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

    Private Sub BindData()

        Try
            Dim ds As DataSet

            ds = PEModule.GetDrawing(ViewState("DrawingNo"))

            If commonFunctions.CheckDataset(ds) = True Then

                'ViewState("rootReleaseType") = ds.Tables(0).Rows(0).Item("ReleaseType").ToString()

                'If ViewState("rootReleaseType") = "PAST-Release" Or ViewState("rootReleaseType") = "Design-Intent" Or ViewState("rootReleaseType") = "Study" Then
                '    ddReleaseType.SelectedValue = ViewState("rootReleaseType")
                'End If

                If ds.Tables(0).Rows(0).Item("ReleaseTypeID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ReleaseTypeID") > 0 Then
                        ViewState("rootReleaseType") = ds.Tables(0).Rows(0).Item("ddReleaseTypeName")
                        ddReleaseType.SelectedValue = ds.Tables(0).Rows(0).Item("ReleaseTypeID")
                    End If
                End If

                If Trim(ds.Tables(0).Rows(0).Item("OldPartName").ToString) <> "" Then
                    ViewState("rootDrawingName") = ds.Tables(0).Rows(0).Item("OldPartName") '& " | "
                End If

                ViewState("ECINo") = 0
                If ds.Tables(0).Rows(0).Item("ECINo") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ECINo") > 0 Then
                        ViewState("ECINo") = ds.Tables(0).Rows(0).Item("ECINo")
                    End If
                End If

                'ViewState("rootDrawingName") += ds.Tables(0).Rows(0).Item("PartName").ToString()            
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
    Protected Sub BindList(ByVal sDrawingNumber As String)

        Try
            lblMessage.Text = ""
            lblShowMessage.Text = ""
            lblWarning.Text = ""

            'need code to clear tree
            tvBOM.Nodes.Clear()

            'clear session variable
            Session("sessionDMSChangeReleaseTypeRecursionCounter") = 0
            Session("sessionDMSChangeReleaseTypeCurrentRecursionLevel") = 1

            'add to HashTable
            htDrawingList.Add(sDrawingNumber, sDrawingNumber)

            BuildTree(sDrawingNumber, Nothing)

            'Expand the Whole Tree
            tvBOM.ExpandAll()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub BuildTree(ByVal sDrawingNumber As String, ByVal n As TreeNode)

        Try
            Dim iRecursionCounter As Integer = Session("sessionDMSChangeReleaseTypeRecursionCounter")
            Dim iCurrentRecursionLevel As Integer = Session("sessionDMSChangeReleaseTypeCurrentRecursionLevel")

            If Session("sessionDMSChangeReleaseTypeRecursionCounter") = Nothing Then
                iRecursionCounter = 0
            End If

            Dim ds As DataSet
            Dim iSize As Integer = 0
            Dim iCounter As Integer = 0
            Dim strReleaseType As String = ""
            Dim strSubDrawingNo As String = ""
            Dim strSubDrawingName As String = ""
            Dim sQuantity As String = ""
            Dim sNotes As String = ""

            'preventing an infinite loop
            Session("sessionDMSChangeReleaseTypeRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 999 Then

                ds = PEModule.GetSubDrawing(sDrawingNumber, "", "", "", "", "", 0, "", False)
                If commonFunctions.CheckDataset(ds) = True Then
                    iSize = ds.Tables(0).Rows.Count

                    'if SubDrawings Exist.
                    If iSize > 0 Then
                        strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString.Trim ' & " | " & ds.Tables(0).Rows(iCounter).Item("PartName").ToString.Trim

                        Dim root As New TreeNode(sDrawingNumber & "  -  " & ViewState("rootDrawingName").ToString.Trim & "     (" & ViewState("rootReleaseType").ToString.Trim & ")")  ' & " - Level: " & 0 & " - Recursion Counter: " & iRecursionCounter)

                        ' start by creating a ROOT node                    
                        If iRecursionCounter = 0 Then
                            tvBOM.Nodes.Add(root)
                        End If

                        For iCounter = 0 To iSize - 1

                            iRecursionCounter += 1
                            Session("sessionDMSChangeReleaseTypeRecursionCounter") = iRecursionCounter + 1

                            strReleaseType = ds.Tables(0).Rows(iCounter).Item("ddReleaseTypeName").ToString.Trim
                            strSubDrawingNo = ds.Tables(0).Rows(iCounter).Item("SubDrawingNo").ToString.Trim
                            strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString.Trim '& " | " & ds.Tables(0).Rows(iCounter).Item("PartName").ToString.Trim

                            If strSubDrawingNo.Trim.Length > 0 Then

                                'Add to HashTable List, used for printing later
                                If htDrawingList(strSubDrawingNo) Is Nothing Then
                                    htDrawingList.Add(strSubDrawingNo, strSubDrawingNo)
                                Else
                                    If lblWarning.Text = "" Then
                                        lblWarning.Text = "The following components appear more than once in the Tree View List "
                                    End If
                                    lblWarning.Text += ": " & strSubDrawingNo
                                End If

                                'Dim node As New TreeNode(strSubDrawingNo & "  :: " & strSubDrawingName & " :: Release Type - " & strReleaseType & " :: QUANTITY - " & sQuantity & " :: " & sNotes) '& " - Level: " & iCurrentRecursionLevel & " - Recursion Counter: " & iRecursionCounter)
                                Dim node As New TreeNode(strSubDrawingNo & "  -  " & strSubDrawingName & "     (" & strReleaseType & ")") '& " - Level: " & iCurrentRecursionLevel & " - Recursion Counter: " & iRecursionCounter)
                                If n Is Nothing Then
                                    root.Checked = True
                                    root.SelectAction = TreeNodeSelectAction.None
                                    root.ChildNodes.Add(node)
                                Else
                                    n.Checked = True
                                    n.SelectAction = TreeNodeSelectAction.None
                                    n.ChildNodes.Add(node)
                                End If

                                node.Checked = True
                                node.SelectAction = TreeNodeSelectAction.None

                                Session("sessionDMSChangeReleaseTypeCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                                BuildTree(strSubDrawingNo, node)
                                Session("sessionDMSChangeReleaseTypeCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                            End If 'end SubDrawings
                        Next 'end iCounter Loop
                    Else
                        If iRecursionCounter = 0 Then
                            lblMessage.Text = "There are no sub-drawings currently defined for this drawing."
                            lblDropdownInstructions.Visible = False
                            ddReleaseType.Visible = False
                            lblCheckInstructions.Visible = False
                            btnChangeReleaseType.Visible = False
                            btnSelectAll.Visible = False
                            btnUnselectAll.Visible = False
                            btnRefresh.Visible = False
                            btnChangeReleaseType.Visible = False
                        End If
                    End If 'end iSize                    
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not Page.IsPostBack Then
                ViewState("DrawingNo") = Trim(HttpContext.Current.Request.QueryString("DrawingNo"))

                If ViewState("DrawingNo") IsNot Nothing And ViewState("DrawingNo") <> "" Then
                    lblDrawingNo.Text = ViewState("DrawingNo")
                    BindCriteria()
                    BindData()
                    BindList(ViewState("DrawingNo"))
                End If
            End If

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Change Drawing and Sub-Drawing Release Type"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > <a href='DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & " '><b>Drawing Detail</b></a> > Change Release Type "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMGExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnChangeReleaseType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeReleaseType.Click

        Try
            lblMessage.Text = ""

            Dim strDrawingNo As String = ""
            Dim iRightParenthesisPlace As Integer = 0
            Dim strDrawingList(999) As String
            Dim iDrawingTotalCount As Integer = 0
            Dim iDrawingCounter As Integer = 0
            Dim iCheckDrawingNoCounter As Integer = 0
            Dim bFoundDuplicatePart As Boolean = False

            Dim iReleaseTypeID As Integer = 0
            If ddReleaseType.SelectedIndex > 0 Then
                iReleaseTypeID = ddReleaseType.SelectedValue
            Else
                iReleaseTypeID = 2
            End If

            'Checks If Parent Node has Child Node
            If tvBOM.CheckedNodes.Count > 0 Then
                'Display your selected nodes
                For Each node As TreeNode In tvBOM.CheckedNodes
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
                    lblShowMessage.Text = "The following drawings were changed to release type: " & ddReleaseType.SelectedValue & "<br>"

                    ' insert code here to update each drawing to the new release type
                    For iDrawingCounter = 0 To iDrawingTotalCount - 1
                        If strDrawingList(iDrawingCounter) IsNot Nothing And Trim(strDrawingList(iDrawingCounter)) <> "" And iReleaseTypeID > 0 Then

                            If ViewState("ECINo") > 0 And strDrawingList(iDrawingCounter) = ViewState("DrawingNo") Then
                                'do not update root drawing if ECI was issued
                            Else
                                PEModule.UpdateDrawingReleaseType(strDrawingList(iDrawingCounter), ddReleaseType.SelectedValue)

                                lblShowMessage.Text += strDrawingList(iDrawingCounter) & "<br>"
                            End If

                        End If
                    Next
                End If
            Else
                lblShowMessage.Text = "No changes were made. You did not select any nodes. "
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
    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click

        Try
            lblShowMessage.Text = ""
            lblWarning.Text = ""
            lblMessage.Text = ""

            For Each node As TreeNode In tvBOM.Nodes
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
            lblShowMessage.Text = ""
            lblWarning.Text = ""
            lblMessage.Text = ""

            For Each node As TreeNode In tvBOM.Nodes
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
            lblShowMessage.Text = ""
            lblWarning.Text = ""
            lblMessage.Text = ""

            If tvBOM.CheckedNodes.Count > 0 Then
                'Display your selected nodes
                For Each node As TreeNode In tvBOM.CheckedNodes
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
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'clear session variable
            Session("sessionDMSChangeReleaseTypeRecursionCounter") = Nothing
            Session("sessionDMSChangeReleaseTypeCurrentRecursionLevel") = Nothing
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        Try
            Response.Redirect("DrawingReleaseTypeChange.aspx?DrawingNo=" & ViewState("DrawingNo"), False)
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
