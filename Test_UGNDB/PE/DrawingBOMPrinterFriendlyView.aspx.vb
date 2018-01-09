' ***********************************************************************************************
'
' Name:		Drawings_DrawingBOMPrinterFriendlyView.aspx
' Purpose:	This Code Behind is for the printer friendly view of the BOM for a Drawing, called from DrawingDetail.aspx
'
' Date		        Author	    
' 09/22/2009        Roderick Carlson            Modified - Temporarily hiding PartName due to performance hit
' 12/10/2009        Roderick Carlson            Modified - Put Drawing No at top - temporarily hide tree
' 09/15/2010        Roderick Carlson            Modified - PDE-2979 - prevented child of itself being in the BOM
' 12/19/2013        LRey   	            - Replaced "BPCS Part No" to "Part No" wherever used.
' ************************************************************************************************

Partial Class PE_PE_Drawings_DrawingBOMPrinterFriendlyView
    Inherits System.Web.UI.Page
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

        Try
            'preventing an infinite loop
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 500 Then '999 

                ds = PEModule.GetSubDrawing(sDrawingNumber, "", "", "", "", "", 0, "", False)
                If ds IsNot Nothing Then
                    iSize = ds.Tables(0).Rows.Count

                    'if SubDrawings Exist.
                    If iSize > 0 Then
                        strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString
                        strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity")
                        strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString

                        Dim root As New TreeNode(sDrawingNumber & "  -  " & ViewState("DrawingName"))

                        ' start by creating a ROOT node                    
                        If iRecursionCounter = 0 Then
                            tvCurrentDrawingAsTop.Nodes.Add(root)
                        End If

                        For iCounter = 0 To iSize - 1

                            iRecursionCounter += 1
                            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = iRecursionCounter + 1

                            strSubDrawingNo = ds.Tables(0).Rows(iCounter).Item("SubDrawingNo").ToString
                            strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString
                            strPartNo = ds.Tables(0).Rows(iCounter).Item("SubPartNo").ToString
                            strPartRevision = ds.Tables(0).Rows(iCounter).Item("SubPart_Revision").ToString
                            strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity")
                            strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString

                            If strSubDrawingNo.Trim.Length > 0 Then

                                Dim node As New TreeNode(strSubDrawingNo & "  :: " & strSubDrawingName & " :: Internal Part No - " & strPartNo & " :: Part Revision - " & strPartRevision & " :: QUANTITY - " & strQuantity & " :: " & strNotes)

                                If n Is Nothing Then
                                    root.SelectAction = TreeNodeSelectAction.None
                                    root.ChildNodes.Add(node)
                                Else
                                    n.SelectAction = TreeNodeSelectAction.None
                                    n.ChildNodes.Add(node)
                                End If

                                node.SelectAction = TreeNodeSelectAction.None
                                Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = iCurrentRecursionLevel + 1
                                If strSubDrawingNo <> sDrawingNumber Then
                                    BuildCurrentDrawingAsTopTree(strSubDrawingNo, node)
                                End If

                                Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = iCurrentRecursionLevel - 1
                            End If 'end SubDrawings
                        Next 'end iCounter Loop
                    Else
                        If iRecursionCounter = 0 Then
                            lblMessage.Text = "There are no sub-drawings currently defined for this drawing."
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

            If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                lblDrawingNoValue.Text = ViewState("DrawingNo")
            End If

            If HttpContext.Current.Request.QueryString("DrawingName") <> "" Then
                ViewState("DrawingName") = HttpContext.Current.Request.QueryString("DrawingName")                
            End If

            'need code to clear tree
            tvCurrentDrawingAsTop.Nodes.Clear()

            'clear session variable
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = 0
            Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = 1

            BuildCurrentDrawingAsTopTree(ViewState("DrawingNo"), Nothing)

            'Expand the Whole Tree
            tvCurrentDrawingAsTop.ExpandAll()

            'clean session variables
            Session("sessionDMSCurrentDrawingAsTopRecursionCounter") = Nothing
            Session("sessionDMSCurrentDrawingAsTopCurrentRecursionLevel") = Nothing
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cbShowTree_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbShowTree.CheckedChanged

        tvCurrentDrawingAsTop.Visible = cbShowTree.Checked

    End Sub
End Class
