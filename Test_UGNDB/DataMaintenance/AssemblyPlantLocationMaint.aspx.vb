' ************************************************************************************************
' Name:	AssemblyPlantLocationMaint.aspx.vb
' Purpose:	This program is used to view, insert, update Make
'
' Date		    Author	    
' 05/20/2011    LREY			Created .Net application
' ************************************************************************************************


Partial Class DataMaintenance_AssemblyPlantLocationMaint
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc.: Data Maintenance"
            m.ContentLabel = "Assembly Plant Location"

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Data Maintenance</b> > Assembly Plant Location"
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
                ViewState("sAPL") = ""
                ViewState("sCtry") = ""
                ViewState("sOMfg") = ""

                If Not Request.Cookies("APLM_APL") Is Nothing Then
                    txtAssembly.Text = Server.HtmlEncode(Request.Cookies("APLM_APL").Value)
                    ViewState("sAPL") = Server.HtmlEncode(Request.Cookies("APLM_APL").Value)
                End If

                If Not Request.Cookies("APLM_Ctry") Is Nothing Then
                    txtCountry.Text = Server.HtmlEncode(Request.Cookies("APLM_Ctry").Value)
                    ViewState("sCtry") = Server.HtmlEncode(Request.Cookies("APLM_Ctry").Value)
                End If

                If Not Request.Cookies("APLM_OMfg") Is Nothing Then
                    txtOEMMfg.Text = Server.HtmlEncode(Request.Cookies("APLM_OMfg").Value)
                    ViewState("sOMfg") = Server.HtmlEncode(Request.Cookies("APLM_OMfg").Value)
                End If
            Else
                ViewState("sAPL") = txtAssembly.Text
                ViewState("sCtry") = txtCountry.Text
                ViewState("sOMfg") = txtOEMMfg.Text
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
            gvAPL.Columns(1).Visible = False
            If gvAPL.FooterRow IsNot Nothing Then
                gvAPL.FooterRow.Visible = False
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
                                    gvAPL.Columns(1).Visible = True
                                    If gvAPL.FooterRow IsNot Nothing Then
                                        gvAPL.FooterRow.Visible = True
                                    End If
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("ObjectRole") = True
                                    gvAPL.Columns(1).Visible = True
                                    If gvAPL.FooterRow IsNot Nothing Then
                                        gvAPL.FooterRow.Visible = True
                                    End If
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("ObjectRole") = True
                                    gvAPL.Columns(1).Visible = False
                                    If gvAPL.FooterRow IsNot Nothing Then
                                        gvAPL.FooterRow.Visible = True
                                    End If
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    gvAPL.Columns(0).Visible = False
                                    If gvAPL.FooterRow IsNot Nothing Then
                                        gvAPL.FooterRow.Visible = False
                                    End If
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    gvAPL.Columns(1).Visible = False
                                    If gvAPL.FooterRow IsNot Nothing Then
                                        gvAPL.FooterRow.Visible = False
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try

            Response.Cookies("APLM_APL").Value = txtAssembly.Text
            Response.Cookies("APLM_Ctry").Value = txtCountry.Text
            Response.Cookies("APLM_OMfg").Value = txtOEMMfg.Text

            Response.Redirect("AssemblyPlantLocationMaint.aspx?sAPL=" & ViewState("sAPL") & "&sCtry=" & ViewState("sCtry") & "&sOMfg=" & ViewState("sOMfg"), False)

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnSearch_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try
            commonFunctions.DeleteAssemblyPlantLocationCookies()

            Response.Redirect("AssemblyPlantLocationMaint.aspx", False)
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF btnReset_Click

#Region "GridView Events"
    Protected Sub gvAPL_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAPL.RowDataBound
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
                        Dim price As AssemblyPlantLocation.Assembly_Plant_LocationRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, AssemblyPlantLocation.Assembly_Plant_LocationRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this record " & " for """ & DataBinder.Eval(e.Row.DataItem, "Assembly_Plant_Location") & """?');")
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
    End Sub 'EOF gvPlatformProgramList_RowDataBound

    Protected Sub gvAPL_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False

            ''***
            ''This section allows the inserting of a new row when called by the OnInserting event call.
            ''***
            Dim Assembly As TextBox
            Dim IHSAP As TextBox
            Dim State As TextBox
            Dim Country As DropDownList
            Dim OEMMfg As DropDownList
            Dim UGNBusiness As DropDownList
            Dim AssemblyType As DropDownList

            If (e.CommandName = "Insert") Then
                Assembly = CType(gvAPL.FooterRow.FindControl("txtAPL"), TextBox)
                odsAPL.InsertParameters("Assembly_Plant_Location").DefaultValue = Assembly.Text


                State = CType(gvAPL.FooterRow.FindControl("txtState"), TextBox)
                odsAPL.InsertParameters("State").DefaultValue = State.Text

                Country = CType(gvAPL.FooterRow.FindControl("ddCountry"), DropDownList)
                odsAPL.InsertParameters("Country").DefaultValue = Country.SelectedValue

                OEMMfg = CType(gvAPL.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                odsAPL.InsertParameters("OEMManufacturer").DefaultValue = OEMMfg.SelectedValue

                UGNBusiness = CType(gvAPL.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                odsAPL.InsertParameters("UGNBusiness").DefaultValue = UGNBusiness.SelectedValue

                AssemblyType = CType(gvAPL.FooterRow.FindControl("ddAssemblyTypeGV"), DropDownList)
                odsAPL.InsertParameters("AssemblyType").DefaultValue = AssemblyType.SelectedValue

                IHSAP = CType(gvAPL.FooterRow.FindControl("txtIHSAP"), TextBox)
                odsAPL.InsertParameters("IHS_Assembly_Plant").DefaultValue = IHSAP.Text

                odsAPL.Insert()
            End If 'EOF Insert

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvAPL.ShowFooter = False
            Else
                If ViewState("ObjectRole") = True Then
                    gvAPL.ShowFooter = True
                Else
                    gvAPL.ShowFooter = False
                End If
            End If 'EOF Edit

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                Assembly = CType(gvAPL.FooterRow.FindControl("txtAPL"), TextBox)
                Assembly.Text = Nothing

                State = CType(gvAPL.FooterRow.FindControl("txtState"), TextBox)
                State.Text = Nothing

                Country = CType(gvAPL.FooterRow.FindControl("txtCountry"), DropDownList)
                Country.SelectedValue = Nothing

                OEMMfg = CType(gvAPL.FooterRow.FindControl("ddOEMMfg"), DropDownList)
                OEMMfg.SelectedValue = Nothing

                UGNBusiness = CType(gvAPL.FooterRow.FindControl("ddUGNBusinessGV"), DropDownList)
                UGNBusiness.SelectedValue = Nothing

                AssemblyType = CType(gvAPL.FooterRow.FindControl("ddAssemblyTypeGV"), DropDownList)
                AssemblyType.SelectedValue = Nothing

                IHSAP = CType(gvAPL.FooterRow.FindControl("txtIHSAP"), TextBox)
                IHSAP.Text = Nothing

            End If 'EOF Undo

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvAPL_RowCommand

    Protected Sub gvAPL_RowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        lblRaiseError.Text = Nothing
        lblRaiseError.Visible = False

        If e.Exception Is Nothing Then
            If e.AffectedRows > 0 Then
                lblRaiseError.Text = "Row deleted successfully."
                lblRaiseError.Visible = True
            Else
                lblRaiseError.Text = "The Assembly Plant Location you attempted to delete is assigned to a Program. "
                lblRaiseError.Visible = True
            End If
        Else
            lblRaiseError.Text = "An error occured while attempting to delete a row."
            lblRaiseError.Visible = True
        End If

    End Sub

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

            Dim dt As AssemblyPlantLocation.Assembly_Plant_LocationDataTable = CType(e.ReturnValue, AssemblyPlantLocation.Assembly_Plant_LocationDataTable)

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

    Protected Sub gvAPL_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAPL.RowCreated

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
