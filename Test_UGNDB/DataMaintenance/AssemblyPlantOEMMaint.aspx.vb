' ************************************************************************************************
' Name:	AssemblyPlantOEMMaint.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 05/25/2011    LREY			Created .Net application
' ************************************************************************************************

Partial Class DataMaintenance_AssemblyPlantOEMMaint
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "OEM Model Types by Assembly Plant"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                If Request.QueryString("pAPID") IsNot Nothing Then
                    ViewState("pAPID") = Server.UrlDecode(Request.QueryString("pAPID").ToString)
                End If
                If Request.QueryString("sAPL") IsNot Nothing Then
                    ViewState("sAPL") = Server.UrlDecode(Request.QueryString("sAPL").ToString)
                End If
                If Request.QueryString("sCtry") IsNot Nothing Then
                    ViewState("sCtry") = Server.UrlDecode(Request.QueryString("sCtry").ToString)
                End If
                If Request.QueryString("sOMfg") IsNot Nothing Then
                    ViewState("sOMfg") = Server.UrlDecode(Request.QueryString("sOMfg").ToString)
                End If

                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > <a href='AssemblyPlantLocationMaint.aspx?sAPL=" & ViewState("sAPL") & "&sCtry=" & ViewState("sCtry") & "&sOMfg=" & ViewState("sOMfg") & "'><b>Assembly Plant Location </b></a> > OEM Model Types by Assembly Plant"

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
                BindData()
            End If

            CheckRights()

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Protected Sub CheckRights()
        Try
            'default to hide edit column
            gvAPLOEM.Columns(1).Visible = False
            If gvAPLOEM.FooterRow IsNot Nothing Then
                gvAPLOEM.FooterRow.Visible = False
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
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 124)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("ObjectRole") = True
                                    gvAPLOEM.Columns(1).Visible = True
                                    If gvAPLOEM.FooterRow IsNot Nothing Then
                                        gvAPLOEM.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    gvAPLOEM.Columns(1).Visible = True
                                    If gvAPLOEM.FooterRow IsNot Nothing Then
                                        gvAPLOEM.FooterRow.Visible = True
                                    End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvAPLOEM.Columns(1).Visible = False
                                    If gvAPLOEM.FooterRow IsNot Nothing Then
                                        gvAPLOEM.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvAPLOEM.Columns(0).Visible = False
                                    If gvAPLOEM.FooterRow IsNot Nothing Then
                                        gvAPLOEM.FooterRow.Visible = False
                                    End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    gvAPLOEM.Columns(1).Visible = False
                                    If gvAPLOEM.FooterRow IsNot Nothing Then
                                        gvAPLOEM.FooterRow.Visible = False
                                    End If
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

    End Sub 'EOF CheckRights

    Private Sub BindData()
        Try
            lblMessage.Text = ""
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If ViewState("pAPID") <> Nothing Then
                'bind data
                ds = commonFunctions.GetAssemblyPlantLocation(ViewState("pAPID"), "", "", "", "")
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblAssembly.Text = ds.Tables(0).Rows(0).Item("Assembly_Plant_Location").ToString()
                    lblStateVal.Text = ds.Tables(0).Rows(0).Item("State").ToString()
                    lblCountryVal.Text = ds.Tables(0).Rows(0).Item("Country").ToString()
                    lblOEMManufacturerVal.Text = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    lblUGNBiz.Text = IIf(ds.Tables(0).Rows(0).Item("UGNBusiness") = True, "Yes", "No")
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

#Region "GridView Events"
    Protected Sub gvAPLOEM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAPLOEM.RowDataBound
        Try
            '***
            'This section provides the user with the popup for confirming the delete of a record.
            'Called by the onClientClick event.
            '***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim price As AssemblyPlantLocation.Assembly_Plant_OEMRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, AssemblyPlantLocation.Assembly_Plant_OEMRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record for """ & DataBinder.Eval(e.Row.DataItem, "OEMModelType") & """?');")
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
    End Sub 'EOF gvAPLOEM_RowDataBound

    Protected Sub gvAPLOEM_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False
            lblRaiseError.Text = Nothing
            lblRaiseError.Visible = False

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim OEMModelType As TextBox
            Dim Make As DropDownList
            Dim ModelName As DropDownList
            Dim PlatformID As DropDownList

            If (e.CommandName = "Insert") Then
                odsAPL.InsertParameters("APID").DefaultValue = ViewState("pAPID")

                OEMModelType = CType(gvAPLOEM.FooterRow.FindControl("txtOEMModelType"), TextBox)
                odsAPL.InsertParameters("OEMModelType").DefaultValue = OEMModelType.Text

                Make = CType(gvAPLOEM.FooterRow.FindControl("ddMake"), DropDownList)
                odsAPL.InsertParameters("Make").DefaultValue = Make.SelectedValue

                ModelName = CType(gvAPLOEM.FooterRow.FindControl("ddModel"), DropDownList)
                odsAPL.InsertParameters("ModelName").DefaultValue = ModelName.SelectedValue

                PlatformID = CType(gvAPLOEM.FooterRow.FindControl("ddPlatform"), DropDownList)
                odsAPL.InsertParameters("PlatformID").DefaultValue = PlatformID.SelectedValue

                odsAPL.Insert()
            End If 'EOF Insert

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAPLOEM.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvAPLOEM.ShowFooter = True
                Else
                    gvAPLOEM.ShowFooter = False
                End If
            End If 'EOF Edit

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                OEMModelType = CType(gvAPLOEM.FooterRow.FindControl("txtOEMModelType"), TextBox)
                OEMModelType.Text = Nothing

                Make = CType(gvAPLOEM.FooterRow.FindControl("ddMake"), DropDownList)
                Make.SelectedValue = Nothing

                ModelName = CType(gvAPLOEM.FooterRow.FindControl("ddModel"), DropDownList)
                ModelName.SelectedValue = Nothing

                PlatformID = CType(gvAPLOEM.FooterRow.FindControl("ddPlatform"), DropDownList)
                PlatformID.SelectedValue = Nothing

            End If 'EOF Undo

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvAPLOEM_RowCommand

    Protected Sub gvAPLOEM_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Try
            Dim strTemp As String

            Dim strKey As String

            For Each strKey In e.NewValues.Keys

                strTemp = e.NewValues(strKey).ToString

                If strTemp.Contains(":::") Then

                    e.NewValues(strKey) = CleanBindValue(strTemp)

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
    End Sub 'EOF gvAPLOEM_RowUpdating

    Private Function CleanBindValue(ByVal DirtyValue As String) As String

        'CascadingDropDown returns BIND values as value:::text 

        'and needs to be cleaned prior to database update

        Dim strSplit() As String

        strSplit = DirtyValue.Split(":::")

        Return strSplit(0).ToString

    End Function 'EOF CleanBindValue

    Protected Sub gvAPLOEM_RowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        lblRaiseError.Text = Nothing
        lblRaiseError.Visible = False

        If e.Exception Is Nothing Then
            If e.AffectedRows > 0 Then
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            Else
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            End If
        Else
            lblRaiseError.Text = "An error occured while attempting to delete a row."
            lblRaiseError.Visible = True
        End If

    End Sub 'EOF gvAPLOEM_RowDeleted


#End Region

#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_APL() As Boolean
        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_APL") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_APL"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_APL") = value
        End Set

    End Property

    Protected Sub odsAPL_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsAPL.Selected

        Try
            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As AssemblyPlantLocation.Assembly_Plant_OEMDataTable = CType(e.ReturnValue, AssemblyPlantLocation.Assembly_Plant_OEMDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_APL = True
            Else
                LoadDataEmpty_APL = False
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

    Protected Sub gvAPLOEM_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAPLOEM.RowCreated

        Try
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_APL
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

End Class
