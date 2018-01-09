' ***********************************************************************************************
'
' Name:		RFD_Drawing_BOM_Selection.aspx
' Purpose:	This Code is to allow the user to check off which drawings are desired to inclued in the RFD Child Part Tab
'
' Date		    Author	
' 06/12/2012    Roderick Carlson - Created - copied from PE-DMS 
'' ************************************************************************************************
Partial Class RFD_Drawing_BOM_Selection
    Inherits System.Web.UI.Page

    Protected Sub BuildBOMList(ByVal DrawingNumber As String, ByVal n As TreeNode)

        Dim iRecursionCounter As Integer = Session("sessionRFDDMSBOMRecursionCounter")
        Dim iCurrentRecursionLevel As Integer = Session("sessionRFDDMSBOMCurrentRecursionLevel")

        If Session("sessionRFDDMSBOMRecursionCounter") = Nothing Then
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
            Session("sessionRFDDMSBOMRecursionCounter") = iRecursionCounter + 1

            If iRecursionCounter <= 999 Then

                ds = PEModule.GetSubDrawing(DrawingNumber, "", "", "", "", "", 0, "", False)
                If ds IsNot Nothing Then
                    iSize = ds.Tables(0).Rows.Count

                    'if SubDrawings Exist.
                    If iSize > 0 Then
                        strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString '& " | " & ds.Tables(0).Rows(iCounter).Item("PartName").ToString
                        strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity")
                        strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString

                        Dim root As New TreeNode(DrawingNumber & "  -  " & ViewState("DrawingName")) ' & " - Level: " & 0 & " - Recursion Counter: " & iRecursionCounter)

                        ' start by creating a ROOT node                    
                        If iRecursionCounter = 0 Then
                            tvBOM.Nodes.Add(root)
                        End If

                        For iCounter = 0 To iSize - 1

                            iRecursionCounter += 1
                            Session("sessionRFDDMSBOMRecursionCounter") = iRecursionCounter + 1

                            strSubDrawingNo = ds.Tables(0).Rows(iCounter).Item("SubDrawingNo").ToString
                            strSubDrawingName = ds.Tables(0).Rows(iCounter).Item("OldPartName").ToString '& " | " & ds.Tables(0).Rows(iCounter).Item("PartName").ToString
                            strPartNo = ds.Tables(0).Rows(iCounter).Item("SubPartNo").ToString
                            strPartRevision = ds.Tables(0).Rows(iCounter).Item("SubPart_Revision").ToString
                            strQuantity = ds.Tables(0).Rows(iCounter).Item("DrawingQuantity")
                            strNotes = ds.Tables(0).Rows(iCounter).Item("notes").ToString

                            If strSubDrawingNo.Trim.Length > 0 Then

                                Dim node As New TreeNode(strSubDrawingNo & "  :: " & strSubDrawingName & " :: BPCS Part No - " & strPartNo & " :: BPCS Part Revision - " & strPartRevision & " :: QUANTITY - " & strQuantity & " :: " & strNotes) '& " - Level: " & iCurrentRecursionLevel & " - Recursion Counter: " & iRecursionCounter)

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
                                Session("sessionRFDDMSBOMRecursionLevel") = iCurrentRecursionLevel + 1
                                BuildBOMList(strSubDrawingNo, node)
                                Session("sessionRFDDMSBOMRecursionLevel") = iCurrentRecursionLevel - 1
                            End If 'end SubDrawings
                        Next 'end iCounter Loop
                    Else
                        If iRecursionCounter = 0 Then
                            lblMessage.Text = "There are no sub-drawings currently defined for this drawing."
                            'btnPrintPreview.Text = "Preview Drawing"
                            btnSelectAll.Visible = False
                            btnUnselectAll.Visible = False
                        End If
                    End If 'end iSize                    
                End If
            End If 'end check recursion counter

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not Page.IsPostBack Then

                If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                End If

                If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then
                    ViewState("RFDNo") = HttpContext.Current.Request.QueryString("RFDNo")
                End If

                'need code to clear tree
                tvBOM.Nodes.Clear()

                'clear session variable
                Session("sessionRFDDMSBOMRecursionCounter") = 0
                Session("sessionRFDDMSBOMCurrentRecursionLevel") = 1

                BuildBOMList(ViewState("DrawingNo"), Nothing)

                'Expand the Whole Tree
                tvBOM.ExpandAll()

                'clean session variables
                Session("sessionRFDDMSBOMRecursionCounter") = Nothing
                Session("sessionRFDDMSBOMCurrentRecursionLevel") = Nothing

                btnAddToRFDChildParts.Attributes.Add("onclick", "if(confirm('Are you sure that you want to add these child parts to the RFD? If so, click ok. PLEASE REMEMBER TO WAIT FOR THE COMPLETED MESSAGE before closing the window.')){}else{return false}")
                btnAddToRFDChildPartsBottom.Attributes.Add("onclick", "if(confirm('Are you sure that you want to add these child parts to the RFD? If so, click ok. PLEASE REMEMBER TO WAIT FOR THE COMPLETED MESSAGE before closing the window.')){}else{return false}")

            End If

            PEModule.CleanPEDMScrystalReports()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Private Sub BuildBOMandFindImages()

    '    Try
    '        Dim dsTempDrawingInfo As DataSet
    '        Dim iTempDrawingCounter As Integer = 0
    '        Dim dsAlternativeDrawingImage As DataSet
    '        Dim dsTempDrawingRevisions As DataSet
    '        Dim iTempRevisionCounter As Integer = 0
    '        Dim strTempRevision As String = ""
    '        Dim iLeftParenthesisLocation As Integer
    '        Dim strTempDrawingRevisionNotes As String = ""
    '        Dim strTempDrawingNo As String
    '        Dim strTempDrawingLayoutType As String = ""
    '        Dim strTempAlternativeDrawingNo As String = ""
    '        Dim ImageBytesTemp As Byte()

    '        'loop through each temp drawing
    '        dsTempDrawingInfo = PEModule.GetTempDrawings(ViewState("DrawingNo"), ViewState("IncludeBOM"))
    '        If dsTempDrawingInfo IsNot Nothing Then
    '            If dsTempDrawingInfo.Tables.Count > 0 And dsTempDrawingInfo.Tables(0).Rows.Count > 0 Then

    '                For iTempDrawingCounter = 0 To dsTempDrawingInfo.Tables(0).Rows.Count - 1

    '                    strTempDrawingNo = dsTempDrawingInfo.Tables(0).Rows(iTempDrawingCounter).Item("DrawingNo").ToString

    '                    'check drawing layout type to see if alternative image is needed
    '                    strTempDrawingLayoutType = dsTempDrawingInfo.Tables(0).Rows(iTempDrawingCounter).Item("DrawingLayoutType").ToString
    '                    strTempAlternativeDrawingNo = ""
    '                    Select Case strTempDrawingLayoutType
    '                        Case "Blank-Standard"
    '                            strTempAlternativeDrawingNo = "blankstandard"
    '                        Case "Rolled-Goods"
    '                            strTempAlternativeDrawingNo = "rolledgoods"
    '                        Case "Blank-MD-Critical"
    '                            strTempAlternativeDrawingNo = "blankmdcritical"
    '                        Case "Non-Rectangular"
    '                            strTempAlternativeDrawingNo = "nonrectangularshape"
    '                        Case "No-Shape"
    '                            strTempAlternativeDrawingNo = "noshape"
    '                    End Select

    '                    'get true drawing image or alternative drawing image
    '                    dsAlternativeDrawingImage = PEModule.GetDrawingImages(strTempDrawingNo, strTempAlternativeDrawingNo)
    '                    If dsAlternativeDrawingImage IsNot Nothing Then
    '                        If dsAlternativeDrawingImage.Tables.Count > 0 And dsAlternativeDrawingImage.Tables(0).Rows.Count > 0 Then

    '                            If dsAlternativeDrawingImage.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
    '                                ImageBytesTemp = dsAlternativeDrawingImage.Tables(0).Rows(0).Item("DrawingImage")
    '                                'update image in temp list
    '                                PEModule.UpdateTempDrawingBOMImage(strTempDrawingNo, ImageBytesTemp)
    '                            End If
    '                        End If
    '                    End If

    '                    'Parse each revision of Temp Drawing RevisionNotes, save in a local variable, insert into Temp_Drawing_Maint
    '                    strTempDrawingRevisionNotes = ""
    '                    dsTempDrawingRevisions = PEModule.GetDrawingRevisions(strTempDrawingNo)
    '                    If dsTempDrawingRevisions IsNot Nothing Then
    '                        If dsTempDrawingRevisions.Tables.Count > 0 And dsTempDrawingRevisions.Tables(0).Rows.Count > 0 Then

    '                            For iTempRevisionCounter = 0 To dsTempDrawingRevisions.Tables(0).Rows.Count - 1
    '                                iLeftParenthesisLocation = InStr(dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("DrawingNo").ToString, "(")
    '                                strTempRevision = Mid$(dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("DrawingNo").ToString, iLeftParenthesisLocation)

    '                                strTempDrawingRevisionNotes += strTempRevision & ":" & dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("RevisionNotes").ToString
    '                            Next

    '                            If strTempDrawingNo <> "" And strTempDrawingRevisionNotes <> "" Then
    '                                PEModule.UpdateTempDrawingBOMRevisionNotes(strTempDrawingNo, strTempDrawingRevisionNotes)
    '                            End If
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text = ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    'Protected Sub btnPrintPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintPreview.Click, btnPrintPreviewBottom.Click

    '    Try
    '        lblMessage.Text = ""

    '        Dim strDrawingNo As String = ""
    '        Dim iRightParenthesisPlace As Integer = 0
    '        'Dim dsCheckDrawingExist As DataSet

    '        'PEModule.DeleteTempDrawingBOM()

    '        'put keydrawing at beginning of list
    '        'PEModule.InsertTempDrawingBOM(0, ViewState("DrawingNo"), ViewState("DrawingNo"))

    '        'Checks If Parent Node has Child Node
    '        If tvBOM.CheckedNodes.Count > 0 Then

    '            'Display your selected nodes
    '            For Each node As TreeNode In tvBOM.CheckedNodes
    '                'lblShowMessage.Text += node.Text & " " & node.Parent.Text & " "
    '                iRightParenthesisPlace = InStr(node.Text, ")")
    '                strDrawingNo = Mid$(node.Text, 1, iRightParenthesisPlace)

    '                'dsCheckDrawingExist = PEModule.GetTempDrawings(ViewState("DrawingNo"), strDrawingNo)
    '                'If commonFunctions.CheckDataSet(dsCheckDrawingExist) = False Then
    '                '    'add drawing to list
    '                '    If ViewState("DrawingNo") <> strDrawingNo Then
    '                '        PEModule.InsertTempDrawingBOM(1, ViewState("DrawingNo"), strDrawingNo)
    '                '    End If
    '                'End If

    '                lblShowMessage.Text += strDrawingNo & "  //  "
    '            Next

    '        End If

    '        'dsCheckDrawingExist = PEModule.GetTempDrawings(ViewState("DrawingNo"), "")

    '        ''if a list was built, then go to preview
    '        'If commonFunctions.CheckDataSet(dsCheckDrawingExist) = True Then
    '        '    Response.Redirect("DMSDrawingPreview.aspx?DrawingNo=" & ViewState("DrawingNo"), False)
    '        'End If

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click, btnSelectAllBottom.Click

        Try
            lblMessage.Text = ""

            For Each node As TreeNode In tvBOM.Nodes
                node.Checked = True
                CheckNodes(True, node)
            Next

            lblShowMessage.Text = ""
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click, btnUnselectAllBottom.Click

        Try
            lblMessage.Text = ""

            For Each node As TreeNode In tvBOM.Nodes
                CheckNodes(False, node)
            Next

            lblShowMessage.Text = ""
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

    Protected Sub btnAddToRFDChildParts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddToRFDChildParts.Click, btnAddToRFDChildPartsBottom.Click

        Try
            lblMessage.Text = ""
            lblShowMessage.Text = ""

            Dim strDrawingNo As String = ""
            Dim strCurrentPartNo As String = ""
            Dim strCurrentPartName As String = ""
            Dim bExternalRFQrequired As Boolean = True

            Dim bFoundDrawingRowID As Integer = 0
            Dim iRightParenthesisPlace As Integer = 0
            Dim dt As DataTable
            Dim dsCostingMaterial As DataSet

            'Checks If Parent Node has Child Node
            If tvBOM.CheckedNodes.Count > 0 Then

                'Display your selected nodes
                For Each node As TreeNode In tvBOM.CheckedNodes

                    iRightParenthesisPlace = InStr(node.Text, ")")
                    strDrawingNo = Mid$(node.Text, 1, iRightParenthesisPlace)

                    'check to see if RFD already has this child part listed by DMS Drawing
                    'if the record exists, then update it, else insert it
                    bFoundDrawingRowID = 0
                    Dim objRFDChildPartBLL As RFDChildPartBLL = New RFDChildPartBLL

                    dt = objRFDChildPartBLL.GetRFDChildPart(0, ViewState("RFDNo"))
                    If commonFunctions.CheckDataTable(dt) = True Then

                        For iRowCounter = 0 To dt.Rows.Count - 1
                            If strDrawingNo = dt.Rows(iRowCounter).Item("NewDrawingNo").ToString() Then
                                bFoundDrawingRowID = iRowCounter
                            End If
                        Next
                    End If

                    strCurrentPartNo = ""
                    strCurrentPartName = ""
                    bExternalRFQrequired = True

                    dsCostingMaterial = CostingModule.GetMaterial("", "", "", strDrawingNo, 0, 0, "", "", False, False, False, False, False, True)
                    If commonFunctions.CheckDataSet(dsCostingMaterial) = True Then
                        strCurrentPartNo = dsCostingMaterial.Tables(0).Rows(0).Item("PartNo").ToString
                        strCurrentPartName = dsCostingMaterial.Tables(0).Rows(0).Item("PartName").ToString
                        bExternalRFQrequired = False
                    End If

                    'If strCurrentPartNo = "" Then
                    '    strCurrentPartName = "Need Assignment"
                    'End If

                    If bFoundDrawingRowID > 0 Then
                        'theoretically, this should already be handled by the user so do not do anything here.
                    Else
                        RFDModule.InsertRFDChildPart(ViewState("RFDNo"), strCurrentPartNo, "", strCurrentPartName, strCurrentPartName, strDrawingNo, "", 0, 0, "", "", bExternalRFQrequired, 0, 0, "", "", 0, "", "", "", 0, "", "", "", "", 0, 0, 0, "")
                    End If

                    'lblShowMessage.Text &= strDrawingNo & "  //  "
                    lblShowMessage.Text &= "Added:" & strDrawingNo & "<br>"

                    lblMessage.Text = "Updates complete. Please close this window.<br><br>"
                Next

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
