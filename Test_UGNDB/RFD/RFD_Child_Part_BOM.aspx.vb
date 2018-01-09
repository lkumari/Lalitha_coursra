' ************************************************************************************************
'
' Name:		RFD_Child_Part_BOM.aspx
' Purpose:	This Code Behind is for the Request for Development BOM of a part.
'
' Date		Author	    
' 09/30/2010 Roderick Carlson
' ************************************************************************************************

Partial Class RFD_Child_Part_BOM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            'need code to clear tree
            tvBOM.Nodes.Clear()

            'clear session variable
            Session("sessionRecursionCounter") = 0
            Session("sessionCurrentRecursionLevel") = 1

            If Request.QueryString("PartNo") IsNot Nothing Then
                If Trim(Request.QueryString("PartNo")) <> "" Then
                    ViewState("PartNo") = Request.QueryString("PartNo").ToString

                    lblPartNo.Text = "Bill of Materials for PartNo: " & ViewState("PartNo")

                    BuildTree(ViewState("PartNo"), Nothing)

                    'Expand the Whole Tree
                    tvBOM.ExpandAll()
                End If
            End If
        End If

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
                    If iRecursionCounter = 0 And ViewState("PartNo") <> "" Then
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
End Class
