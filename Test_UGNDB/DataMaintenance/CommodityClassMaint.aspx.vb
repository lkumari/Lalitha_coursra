' ************************************************************************************************
' Name:	CommodityClassMaint.aspx.vb
' Purpose:	This program is used to view Commodities
'
' Date		    Author	    
' 10/27/2011    LRey			Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_CommodityClassMaint
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Commodity Classification"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Commodity Classification"
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
                If Request.QueryString("sClass") IsNot Nothing Then
                    txtCommodityClassification.Text = Server.UrlDecode(Request.QueryString("sClass").ToString)
                End If
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
            gvCommodityClass.Columns(0).Visible = False
            If gvCommodityClass.FooterRow IsNot Nothing Then
                gvCommodityClass.FooterRow.Visible = False
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
                                    gvCommodityClass.Columns(0).Visible = True
                                    If gvCommodityClass.FooterRow IsNot Nothing Then
                                        gvCommodityClass.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    gvCommodityClass.Columns(0).Visible = True
                                    If gvCommodityClass.FooterRow IsNot Nothing Then
                                        gvCommodityClass.FooterRow.Visible = True
                                    End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvCommodityClass.Columns(0).Visible = True
                                    If gvCommodityClass.FooterRow IsNot Nothing Then
                                        gvCommodityClass.FooterRow.Visible = True
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
    Protected Sub gvCommodityClass_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim CommodityClass As TextBox
            If (e.CommandName = "Insert") Then
                CommodityClass = CType(gvCommodityClass.FooterRow.FindControl("txtCommodityClass"), TextBox)
                odsCommodityClass.InsertParameters("CommodityClass").DefaultValue = CommodityClass.Text

                odsCommodityClass.Insert()
            End If

            If e.CommandName = "Edit" Then
                gvCommodityClass.ShowFooter = False
            Else
                gvCommodityClass.ShowFooter = True
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCommodityClass.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvCommodityClass.ShowFooter = True
                Else
                    gvCommodityClass.ShowFooter = False
                End If

            End If
            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                CommodityClass = CType(gvCommodityClass.FooterRow.FindControl("txtCommodityClass"), TextBox)
                CommodityClass.Text = Nothing
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

    Protected Sub odsCommodityClass_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCommodityClass.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Commodities.Commodity_ClassDataTable = CType(e.ReturnValue, Commodities.Commodity_ClassDataTable)

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

    Protected Sub gvCommodityClass_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCommodityClass.RowCreated

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
            Response.Redirect("CommodityClassMaint.aspx?sClass=" & Server.UrlEncode(txtCommodityClassification.Text.Trim), False)
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
            Response.Redirect("CommodityClassMaint.aspx", False)
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
