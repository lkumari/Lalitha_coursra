
' ************************************************************************************************
'
' Name:		RFD_Child_Part_Parents.aspx
' Purpose:	This Code Behind is for the Request for Development Parents of a part.
'
' Date		Author	    
' 09/30/2010 Roderick Carlson
' ************************************************************************************************

Partial Class RFD_Child_Part_Parents
    Inherits System.Web.UI.Page

    Private ParentList(-1) As String
    Private ParentCounter As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'need code to clear tree
            tvBOM.Nodes.Clear()

            'clear session variable
            Session("sessionRecursionCounter") = 0
            Session("sessionCurrentRecursionLevel") = 1

            If Request.QueryString("PartNo") <> "" Then

                ViewState("PartNo") = Request.QueryString("PartNo")
                ViewState("TopLevelPartNo") = ViewState("PartNo")

                lblPartNo.Text = ViewState("PartNo") & " is used in the following parts."

                'StartParentList(ViewState("PartNo"))

                ''Expand the Whole Tree
                'tvBOM.CollapseAll()

            End If
        End If

    End Sub

    Protected Sub StartParentList(ByVal childPartNo As String)

        Try
            Dim iCounter As Integer
            Dim node As TreeNode
            'clear session variable
            Session("sessionRecursionCounter") = 1
            Session("sessionCurrentRecursionLevel") = 1

            BuildParentList(childPartNo)

            For iCounter = 0 To ParentList.Length - 1
                If ParentList(iCounter) IsNot Nothing Then
                    node = New TreeNode(ParentList(iCounter))
                    tvBOM.Nodes.Add(node)
                    BuildTree(ParentList(iCounter), node)
                End If
            Next
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub BuildParentList(ByVal childPartNo As String)

        Try
            Dim dsParentList As DataSet
            Dim iCounter As Integer

            dsParentList = commonFunctions.GetBillOfMaterials("", childPartNo)
            If dsParentList Is Nothing Or dsParentList.Tables.Count = 0 Or dsParentList.Tables(0).Rows.Count = 0 Then
                'add to Parent List Array
                ReDim Preserve ParentList(ParentList.Length)
                ParentList(ParentList.Length - 1) = childPartNo
                ParentCounter += 1
            Else
                For iCounter = 0 To dsParentList.Tables.Item(0).Rows.Count - 1
                    BuildParentList(dsParentList.Tables(0).Rows(iCounter).Item("PartNo"))
                Next
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

    Protected Sub BuildTree(ByVal ParentPartNo As String, ByVal n As TreeNode)

        Try
            lblMessage.Text = ""

            Dim iRecursionCounter As Integer = Session("sessionRecursionCounter")
            Dim iCurrentRecursionLevel As Integer = Session("sessionCurrentRecursionLevel")

            If Session("sessionRecursionCounter") = Nothing Then
                iRecursionCounter = 0
            End If

            Dim dsBOM As DataSet
            Dim iCounter As Integer = 0
            Dim strChildPartNo As String = ""
            Dim dblBuildRequired As Double = 0

            'preventing an infinite loop
            Session("sessionRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 9999 Then
                dsBOM = commonFunctions.GetBillOfMaterials(ParentPartNo, "")

                'if SubComponents Exist.
                If dsBOM.Tables(0).Rows.Count > 0 Then
                    Dim root As New TreeNode(ViewState("PartNo"))

                    'start by creating a ROOT node
                    If iRecursionCounter = 0 And ViewState("TopLevelPartNo") <> "" Then
                        tvBOM.Nodes.Add(root)
                    End If

                    For iCounter = 0 To dsBOM.Tables(0).Rows.Count - 1

                        iRecursionCounter += 1
                        Session("sessionRecursionCounter") = iRecursionCounter + 1

                        strChildPartNo = dsBOM.Tables(0).Rows(iCounter).Item("SubPartNo")
                        dblBuildRequired = dsBOM.Tables(0).Rows(iCounter).Item("BPCSQuantity")

                        If strChildPartNo.Trim.Length > 0 Then

                            Dim node As New TreeNode(strChildPartNo & "  :: " & " Build Required " & dblBuildRequired)
                            If n Is Nothing Then
                                'root.Checked = True
                                root.SelectAction = TreeNodeSelectAction.None
                                root.ChildNodes.Add(node)
                            Else
                                'n.Checked = True
                                n.SelectAction = TreeNodeSelectAction.None
                                n.ChildNodes.Add(node)
                            End If

                            'node.Checked = True
                            node.SelectAction = TreeNodeSelectAction.None

                            Session("sessionCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                            BuildTree(strChildPartNo, node)
                            Session("sessionCurrentRecursionLevel") = iCurrentRecursionLevel - 1

                        End If 'end SubComonent
                    Next 'end iCounter Loop
                Else
                    If iRecursionCounter = 0 Then
                        lblMessage.Text = "There are no children currently defined for this part."
                    End If
                End If 'end iSize                
            End If 'end check recursion counter
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub iBtnViewSingleBOM_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Try

            lblMessage.Text = ""

            Dim iTempButton As ImageButton
            Dim strPartNo As String = ""

            iTempButton = CType(sender, ImageButton)

            lblMessage.Text = iTempButton.ToolTip.Trim & " was selected."
            strPartNo = iTempButton.ToolTip.Trim

            If strPartNo <> "" Then
                'need code to clear tree
                tvBOM.Nodes.Clear()

                'clear session variable
                Session("sessionRecursionCounter") = 1
                Session("sessionCurrentRecursionLevel") = 1

                ViewState("TopLevelPartNo") = strPartNo

                'StartParentList(strPartNo)

                Dim node As TreeNode

                ReDim Preserve ParentList(1)

                node = New TreeNode(ParentList(0))

                node.Text = strPartNo

                tvBOM.Nodes.Add(node)

                ParentList(0) = strPartNo

                BuildTree(ParentList(0), node)

                'Expand the Whole Tree
                tvBOM.CollapseAll()

                ViewState("TopLevelPartNo") = ViewState("PartNo")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnViewBOM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewBOM.Click

        Try
            'need code to clear tree
            tvBOM.Nodes.Clear()

            'clear session variable
            Session("sessionRecursionCounter") = 1
            Session("sessionCurrentRecursionLevel") = 1

            If ViewState("PartNo") <> "" Then

                ViewState("PartNo") = Request.QueryString("PartNo")

                StartParentList(ViewState("PartNo"))

                'Expand the Whole Tree
                tvBOM.CollapseAll()

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
