' ************************************************************************************************
' Name:	BillOfMaterialsTree.aspx.vb
' Purpose:	This program is used to view the Bill of Materials as a Tree View.
'
' Date		    Author	    
' 07/08/2008    Roderick Carlson			Created .Net application
' 07/22/2008    Roderick Carlson            Cleaned Up Error Trapping
' 11/19/2012    Roderick Carlson            Do not search by subBPCSPartNo
' 12/19/2013    LRey                        Replaced "BPCSPartNo" to "PartNo" wherever used.

Partial Class DataMaintenance_BillOfMaterialsTree
    Inherits System.Web.UI.Page

    Private ParentList(-1) As String
    Private ParentCounter As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'NO SECURITY NEEDED SINCE ALL FIELDS ARE FOR SEARCHING OR DISPLAYING

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Bill Of Materials Tree View"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Bill Of Materials Tree View"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                BindCriteria()

                'need code to clear tree
                tvBOM.Nodes.Clear()

                'clear session variable
                Session("sessionRecursionCounter") = 0
                Session("sessionCurrentRecursionLevel") = 1

                If Request.QueryString("PartNo") IsNot Nothing Then
                    If Trim(Request.QueryString("PartNo")) <> "" Then
                        ViewState("PartNo") = Server.UrlDecode(Request.QueryString("PartNo").ToString)
                        'BindList(ViewState("PartNo"))
                        BuildTree(ViewState("PartNo"), Nothing)
                        ddFGPartNo.SelectedValue = ViewState("PartNo")

                        'Expand the Whole Tree
                        tvBOM.ExpandAll()
                    End If
                End If

              
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
    Protected Sub BindCriteria()

        Try
            Dim dsPartNo As DataSet

            'bind existing data to drop down PartNo control for selection criteria for search
            dsPartNo = commonFunctions.GetAllFinishedGoods()
            If commonFunctions.CheckDataSet(dsPartNo) = True Then
                ddFGPartNo.DataSource = dsPartNo
                ddFGPartNo.DataTextField = dsPartNo.Tables(0).Columns("PartNo").ColumnName.ToString()
                ddFGPartNo.DataValueField = dsPartNo.Tables(0).Columns("PartNo").ColumnName.ToString()
                ddFGPartNo.DataBind()
                ddFGPartNo.Items.Insert(0, "")
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
    Protected Sub StartParentList(ByVal childPartNo As String)

        Try
            Dim iCounter As Integer
            Dim node As TreeNode
            'clear session variable
            Session("sessionRecursionCounter") = 0
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

            If iRecursionCounter <= 99999 Then
                dsBOM = commonFunctions.GetBillOfMaterials(ParentPartNo, "")

                'if SubComponents Exist.
                If dsBOM.Tables(0).Rows.Count > 0 Then
                    Dim root As New TreeNode(ViewState("PartNo"))

                    'start by creating a ROOT node
                    If iRecursionCounter = 0 And Request.QueryString("PartNo") IsNot Nothing And Request.QueryString("PartNo") <> "" Then
                        tvBOM.Nodes.Add(root)
                    End If

                    For iCounter = 0 To dsBOM.Tables(0).Rows.Count - 1

                        iRecursionCounter += 1
                        Session("sessionRecursionCounter") = iRecursionCounter + 1

                        strChildPartNo = dsBOM.Tables(0).Rows(iCounter).Item("SubPartNo")
                        dblBuildRequired = dsBOM.Tables(0).Rows(iCounter).Item("Quantity")

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

    Protected Sub BPCSPartNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFGPartNo.SelectedIndexChanged

        Try
            Response.Redirect("BillOfMaterialsTree.aspx?PartNo=" & Server.UrlEncode(ddFGPartNo.SelectedValue), False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            If ddFGPartNo.SelectedIndex > 0 Then
                Response.Redirect("BillOfMaterialsTree.aspx?PartNo=" & Server.UrlEncode(ddFGPartNo.SelectedValue), False)
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

    Protected Sub btnGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoBack.Click

        Try
            If ddFGPartNo.SelectedIndex > 0 Then
                Response.Redirect("BillOfMaterials.aspx?PartNo=" & Server.UrlEncode(ddFGPartNo.SelectedValue), False)
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
