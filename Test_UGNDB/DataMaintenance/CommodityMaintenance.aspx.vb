' ************************************************************************************************
' Name:	CommodityMaintenance.aspx.vb
' Purpose:	This program is used to view Commodities
'
' Date		    Author	    
' 04/2008       RCarlson			Created .Net application
' 07/22/2008    RCarlson            Cleaned Up Error Trapping
' 10/03/2008    RCarlson            Added Security Role Select Statement
' 10/27/2011    LRey                Added Commodity Classification reference CCID, Function and Predevelopment 
'                                   used for Development CapEx
' ************************************************************************************************

Partial Class DataMaintenance_CommodityMaintenance
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Commodity by Classification"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                If Request.QueryString("sCCID") IsNot Nothing Then
                    ViewState("sCCID") = Server.UrlDecode(Request.QueryString("sCCID").ToString)
                End If

                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > <a href='CommodityClassMaint.aspx?sCCID=" & ViewState("sCCID") & "'><b>Commodity Classification</b></a> > Commodity by Classification"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False



            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                If Request.QueryString("sCName") IsNot Nothing Then
                    txtCommodityNameSearch.Text = Server.UrlDecode(Request.QueryString("sCName").ToString)
                End If

                BindData()
            End If

            CheckRights()

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub CheckRights()

        Try
            'default to hide edit column
            gvCommodityList.Columns(0).Visible = False
            If gvCommodityList.FooterRow IsNot Nothing Then
                gvCommodityList.FooterRow.Visible = False
            End If


            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 19)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")


                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    gvCommodityList.Columns(0).Visible = True
                                    If gvCommodityList.FooterRow IsNot Nothing Then
                                        gvCommodityList.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    gvCommodityList.Columns(0).Visible = True
                                    If gvCommodityList.FooterRow IsNot Nothing Then
                                        gvCommodityList.FooterRow.Visible = True
                                    End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvCommodityList.Columns(0).Visible = True
                                    If gvCommodityList.FooterRow IsNot Nothing Then
                                        gvCommodityList.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                                Case 15 '*** UGNEdit: No Create/Edit/No Delete

                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                            End Select
                        End If
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

    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ViewState("sCCID") <> Nothing Then
                'bind data
                ds = commonFunctions.GetCommodityClass(ViewState("sCCID"), "")

                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblCommodityClassVal.Text = ds.Tables(0).Rows(0).Item("Commodity_Classification").ToString()
                    lblStatusVal.Text = IIf(ds.Tables(0).Rows(0).Item("Obsolete").ToString() = False, "Active", "Inactive")
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData
#Region "GridView Functions"
    Protected Sub gvCommodityList_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim CCID As DropDownList
            Dim CommodityName As TextBox
            Dim BPCSRef As TextBox
            Dim ProjectCode As TextBox
            Dim PreDev As TextBox

            If (e.CommandName = "Insert") Then
                CCID = CType(gvCommodityList.FooterRow.FindControl("ddCommodityClass"), DropDownList)
                odsCommodityList.InsertParameters("CCID").DefaultValue = CCID.SelectedValue

                CommodityName = CType(gvCommodityList.FooterRow.FindControl("txtCommodityName"), TextBox)
                odsCommodityList.InsertParameters("CommodityName").DefaultValue = CommodityName.Text

                BPCSRef = CType(gvCommodityList.FooterRow.FindControl("txtBPCSRef"), TextBox)
                odsCommodityList.InsertParameters("BPCSCommodityRef").DefaultValue = BPCSRef.Text

                ProjectCode = CType(gvCommodityList.FooterRow.FindControl("txtProjectCode"), TextBox)
                odsCommodityList.InsertParameters("ProjectCode").DefaultValue = ProjectCode.Text

                PreDev = CType(gvCommodityList.FooterRow.FindControl("txtPreDev"), TextBox)
                odsCommodityList.InsertParameters("PreDevCode").DefaultValue = PreDev.Text

                odsCommodityList.Insert()
            End If

            If e.CommandName = "Edit" Then
                gvCommodityList.ShowFooter = False
            Else
                gvCommodityList.ShowFooter = True
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCommodityList.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvCommodityList.ShowFooter = True
                Else
                    gvCommodityList.ShowFooter = False
                End If
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                CCID = CType(gvCommodityList.FooterRow.FindControl("ddCommodityClass"), DropDownList)
                CCID.SelectedValue = Nothing

                CommodityName = CType(gvCommodityList.FooterRow.FindControl("txtCommodityName"), TextBox)
                CommodityName.Text = Nothing

                BPCSRef = CType(gvCommodityList.FooterRow.FindControl("txtBPCSRef"), TextBox)
                BPCSRef.Text = Nothing

                ProjectCode = CType(gvCommodityList.FooterRow.FindControl("txtProjectCode"), TextBox)
                ProjectCode.Text = Nothing

                PreDev = CType(gvCommodityList.FooterRow.FindControl("txtPreDev"), TextBox)
                PreDev.Text = Nothing

            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF gvCommodityList_RowCommand

    Protected Sub gvCommodityList_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim strTemp As String

            Dim strKey As String

            For Each strKey In e.NewValues.Keys
                If e.NewValues(strKey) IsNot Nothing Then
                    strTemp = e.NewValues(strKey).ToString

                    If strTemp.Contains(":::") Then
                        e.NewValues(strKey) = CleanBindValue(strTemp)

                    End If
                End If
            Next
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvCommodityList_RowUpdating

    Private Function CleanBindValue(ByVal DirtyValue As String) As String

        'CascadingDropDown returns BIND values as value:::text 

        'and needs to be cleaned prior to database update

        Dim strSplit() As String

        strSplit = DirtyValue.Split(":::")

        Return strSplit(0).ToString

    End Function 'EOF CleanBindValue
#End Region

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_CommodityClass() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_CommodityClass") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_CommodityClass"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_CommodityClass") = value
        End Set

    End Property

    Protected Sub odsCommodityList_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCommodityList.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Commodities.Commodity_MaintDataTable = CType(e.ReturnValue, Commodities.Commodity_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_CommodityClass = True
            Else
                LoadDataEmpty_CommodityClass = False
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

    Protected Sub gvCommodityList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommodityList.RowCreated

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_CommodityClass
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
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
#End Region ' Insert Empty GridView Work-Around

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            Response.Redirect("CommodityMaintenance.aspx?sCCID=" & ViewState("sCCID") & "&sCName=" & Server.UrlEncode(txtCommodityNameSearch.Text.Trim), False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            Response.Redirect("CommodityMaintenance.aspx?sCCID=" & ViewState("sCCID"), False)
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
