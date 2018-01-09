' ************************************************************************************************
'
' Name:	        DrawingCustomerProgramChange.vb
' Purpose:	    This code is used by the Product Engineering Module to show the BOM of all sub-drawings with the option to push customer program info to all subdrawings

' Called From:  DrawingDetail.aspx
'
' Date		        Author	    
' 09/22/2008      	Roderick Carlson			Created .Net application
' 12/20/2013        LREY    The page will not be needed now that the Program Update feature is removed in DrawingDetail.aspx.


Partial Class DrawingCustomerProgramChange
    Inherits System.Web.UI.Page
    Private htDrawingList As New System.Collections.Hashtable
  
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
    Protected Sub BindList(ByVal DrawingNumber As String)

        Try
            lblMessage.Text = ""
            lblShowMessage.Text = ""
            lblWarning.Text = ""

            'need code to clear tree
            tvBOM.Nodes.Clear()

            'clear session variable
            Session("sessionDMSChangeCustomerProgramRecursionCounter") = 0
            Session("sessionDMSChangeCustomerProgramCurrentRecursionLevel") = 1

            'add to HashTable
            htDrawingList.Add(DrawingNumber, DrawingNumber)

            BuildTree(DrawingNumber, Nothing)

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
    Protected Sub BuildTree(ByVal DrawingNumber As String, ByVal n As TreeNode)

        Try
            Dim iRecursionCounter As Integer = Session("sessionDMSChangeCustomerProgramRecursionCounter")
            Dim iCurrentRecursionLevel As Integer = Session("sessionDMSChangeCustomerProgramCurrentRecursionLevel")

            If Session("sessionDMSChangeCustomerProgramRecursionCounter") = Nothing Then
                iRecursionCounter = 0
            End If

            Dim ds As DataSet
            Dim iSize As Integer = 0
            Dim iCounter As Integer = 0
            'Dim strReleaseType As String = ""
            Dim strSubDrawingNo As String = ""
            Dim strSubDrawingName As String = ""
            Dim sQuantity As String = ""
            Dim sNotes As String = ""

            'preventing an infinite loop
            Session("sessionDMSChangeCustomerProgramRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 999 Then

                ds = PEModule.GetSubDrawing(DrawingNumber, "", "", "", "", "", 0, "", False)
                If ds IsNot Nothing Then
                    iSize = ds.Tables(0).Rows.Count

                    'if SubDrawings Exist.
                    If iSize > 0 Then
                        strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName")

                        Dim root As New TreeNode(DrawingNumber & "  -  " & ViewState("rootDrawingName"))

                        ' start by creating a ROOT node                    
                        If iRecursionCounter = 0 Then
                            tvBOM.Nodes.Add(root)
                        End If

                        For iCounter = 0 To iSize - 1

                            iRecursionCounter += 1
                            Session("sessionDMSChangeCustomerProgramRecursionCounter") = iRecursionCounter + 1

                            strSubDrawingNo = ds.Tables(0).Rows(iCounter).Item("SubDrawingNo").ToString
                            strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString

                            If strSubDrawingNo.Trim <> "" Then

                                'Add to HashTable List, used for printing later
                                If htDrawingList(strSubDrawingNo) Is Nothing Then
                                    htDrawingList.Add(strSubDrawingNo, strSubDrawingNo)
                                Else
                                    If lblWarning.Text = "" Then
                                        lblWarning.Text = "The following components appear more than once in the Tree View List "
                                    End If
                                    lblWarning.Text += ": " & strSubDrawingNo
                                End If

                                Dim node As New TreeNode(strSubDrawingNo & "  -  " & strSubDrawingName)

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

                                Session("sessionDMSChangeCustomerProgramCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                                BuildTree(strSubDrawingNo, node)
                                Session("sessionDMSChangeCustomerProgramCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                            End If 'end SubDrawings
                        Next 'end iCounter Loop
                    Else
                        If iRecursionCounter = 0 Then
                            lblMessage.Text = "There are no sub-drawings currently defined for this drawing."                            
                            lblCheckInstructions.Visible = False                            
                            btnSelectAll.Visible = False
                            btnUnselectAll.Visible = False
                            btnRefresh.Visible = False
                            btnChange.Visible = False
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

                    BindList(ViewState("DrawingNo"))
                End If
            End If

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Push Drawing Customer Program to SubDrawings"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='DrawingList.aspx'><b>Drawing Search</b></a> > <a href='DrawingDetail.aspx?DrawingNo=" & ViewState("DrawingNo") & " '><b>Drawing Detail</b></a> > Push Customer Program to SubDrawings "
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
    Protected Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChange.Click

        Try
            lblMessage.Text = ""

            Dim dsCustomerProgram As DataSet
            Dim iCustomerProgramRowCounter As Integer = 0
            Dim strCustomer As String = ""
            Dim iProgramID As Integer = 0
            Dim iProgramYear As Integer = 0

            Dim strDrawingNo As String = ""
            Dim iRightParenthesisPlace As Integer = 0
            Dim strDrawingList(999) As String
            Dim iDrawingTotalCount As Integer = 0
            Dim iDrawingCounter As Integer = 0
            Dim iCheckDrawingNoCounter As Integer = 0
            Dim bFoundDuplicatePart As Boolean = False

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
                    lblShowMessage.Text = "The following drawings were changed.<br>"

                    ' insert code here to update each drawing to the new release type
                    For iDrawingCounter = 0 To iDrawingTotalCount - 1
                        If strDrawingList(iDrawingCounter) IsNot Nothing And Trim(strDrawingList(iDrawingCounter)) <> "" Then
                            'PEModule.UpdateDrawingReleaseType(strDrawingList(iDrawingCounter), ddReleaseType.SelectedValue)
                            dsCustomerProgram = PEModule.GetDrawingCustomerProgram(ViewState("DrawingNo"))
                            If commonFunctions.CheckDataset(dsCustomerProgram) = True Then
                                For iCustomerProgramRowCounter = 0 To dsCustomerProgram.Tables(0).Rows.Count - 1
                                    strCustomer = ""
                                    If dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("Customer") IsNot System.DBNull.Value Then
                                        strCustomer = dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("Customer").ToString
                                    End If

                                    iProgramID = 0
                                    If dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramID") IsNot System.DBNull.Value Then
                                        If dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramID") > 0 Then
                                            iProgramID = dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramID")
                                        End If
                                    End If

                                    iProgramYear = 0
                                    If dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramYear") IsNot System.DBNull.Value Then
                                        If dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramYear") > 0 Then
                                            iProgramYear = dsCustomerProgram.Tables(0).Rows(iCustomerProgramRowCounter).Item("ProgramYear")
                                        End If
                                    End If

                                    PEModule.InsertDrawingCustomerProgram(strDrawingList(iDrawingCounter), "", iProgramID, iProgramYear)
                                Next
                            End If

                            lblShowMessage.Text += strDrawingList(iDrawingCounter) & "<br>"
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
            Session("sessionDMSChangeCustomerProgramRecursionCounter") = Nothing
            Session("sessionDMSChangeCustomerProgramCurrentRecursionLevel") = Nothing
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
            Response.Redirect("DrawingCustomerProgramChange.aspx?DrawingNo=" & ViewState("DrawingNo"), False)
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
