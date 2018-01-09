' ************************************************************************************************
' Name:	Copy_Vehicle.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'           Applied gridview and detailsview events. The purpose of this program is to allow users
'           to copy current program information and carry the data into a new program to save time in data entry.
'
' Date		    Author	    
' 04/04/2008    LRey			Created .Net application
' ************************************************************************************************
Partial Class PF_Copy_Vehicle
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim sPGMID As String = HttpContext.Current.Request.QueryString("sPGMID")
            Dim sYear As String = HttpContext.Current.Request.QueryString("sYear")
            Dim sPlatID As String = HttpContext.Current.Request.QueryString("sPlatID")
            Dim sCABBV As String = HttpContext.Current.Request.QueryString("sCABBV")
            Dim sSoldTo As String = HttpContext.Current.Request.QueryString("sSoldTo")

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Copy Vehicle Info to a New Program"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > <a href='Vehicle_List.aspx'><b>Vehicle Volume Search</b></a> > <a href='Vehicle_Volume.aspx?sPGMID=" + sPGMID + "&sPlatID=" + sPlatID + "&sYear=" + sYear + "&sCABBV=" + sCABBV + "&sSoldTo=" + sSoldTo + "'><b>Vehicle Volume</b></a> > Copy Vehicle Info"


                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False
            Dim DMPanel As CollapsiblePanelExtender = CType(Master.FindControl("DMExtender"), CollapsiblePanelExtender)
            DMPanel.Collapsed = True

            ''*********
            ''Store query string in viewstate
            ''*********
            ViewState("SourceProgramID") = HttpContext.Current.Request.QueryString("sPGMID")
            ViewState("SourceCABBV") = HttpContext.Current.Request.QueryString("sCABBV")
            ViewState("SourceSoldTo") = HttpContext.Current.Request.QueryString("sSoldTo")
            ViewState("PlanningYear") = HttpContext.Current.Request.QueryString("sYear")

            BindCriteria()


        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        Try
            ''****
            ''Confirm there is data found in the Projected_Sales_Copy table
            ''****
            ds = PFModule.GetVehicleCopy(ViewState("SourceProgramID"), ViewState("SourceCABBV"), ViewState("SourceSoldTo"))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                btnSubmit.Visible = True
            Else
                btnSubmit.Visible = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            lblErrors.Visible = True
        End Try

    End Sub
    Protected Sub gvCopy_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCopy.RowCommand
        Try
            Dim DestinationProgramID As DropDownList

            ''***
            ''This section allows the inserting of a new row when save button is clicked from the footer.
            ''***
            If e.CommandName = "Insert" Then
                ''Insert data

                If gvCopy.Rows.Count = 0 Then
                    '' We are inserting through the DetailsView in the EmptyDataTemplate
                    Return
                End If

                '' Only perform the following logic when inserting through the footer
                DestinationProgramID = CType(gvCopy.FooterRow.FindControl("ddDestinationProgramID"), DropDownList)
                odsCopy.InsertParameters("DestinationProgramID").DefaultValue = DestinationProgramID.Text

                odsCopy.Insert()

            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvCopy.ShowFooter = False
            Else
                gvCopy.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                DestinationProgramID = CType(gvCopy.FooterRow.FindControl("ddDestinationProgramID"), DropDownList)
                DestinationProgramID.Text = Nothing
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvCopy_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCopy.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.DataRow Then
                ' reference the Delete ImageButton
                Dim imgBtn As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                If imgBtn IsNot Nothing Then
                    Dim db As ImageButton = CType(e.Row.Cells(1).Controls(1), ImageButton)

                    ' Get information about the product bound to the row
                    If db.CommandName = "Delete" Then
                        Dim DestinationProgramID As Projected_Sales.Projected_Sales_CopyRow = CType(CType(e.Row.DataItem, System.Data.DataRowView).Row, Projected_Sales.Projected_Sales_CopyRow)

                        db.OnClientClick = String.Format("return confirm('Are you certain you want to delete this Destination Program (" & DataBinder.Eval(e.Row.DataItem, "DestinationProgramName") & ")?');")

                        BindCriteria()
                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim SourceProgramID As String = HttpContext.Current.Request.QueryString("sPartNo")
        Dim ds As DataSet = New DataSet

        Try
            ''****
            ''Confirm there is data found in the Projected_Sales_Copy table
            ''****
            ds = PFModule.GetVehicleCopy(ViewState("SourceProgramID"), ViewState("SourceCABBV"), ViewState("SourceSoldTo"))
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ''****
                ''Process carryover
                ''****
                PFModule.CopyVehicle(ViewState("SourceProgramID"), ViewState("SourceCABBV"), ViewState("SourceSoldTo"))
                Response.Redirect("Vehicle_List.aspx", False)
            Else
                lblErrors.Text = "Submit Cancelled... Destination is undefined."
                lblErrors.Visible = True
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = "Error occurred during save record.  Please contact the IS Application Group." & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            lblErrors.Visible = True
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ''****
        ''Delete any Part Number associated to the SourceProgramID from Projected_Sales_Copy.  
        ''User Decided not to run the copy process.
        ''****
        PFModule.DeleteVehicleCopy(ViewState("SourceProgramID"), ViewState("SourceCABBV"), ViewState("SourceSoldTo"), 0, "", 0)

        Response.Redirect("Vehicle_Volume.aspx?sPGMID=" & ViewState("SourceProgramID") & "&sYear=" & ViewState("PlanningYear") & "&sCABBV=" & ViewState("SourceCABBV") & "&sSoldTo=" & ViewState("SourceSoldTo"), False)

    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        BindCriteria()
    End Sub
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty() As Boolean
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty") = value
        End Set
    End Property

    Protected Sub odsCopy_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsCopy.Selected
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' bubble exceptions before we touch e.ReturnValue
        If e.Exception IsNot Nothing Then
            Throw e.Exception
        End If

        ' get the DataTable from the ODS select method
        Console.WriteLine(e.ReturnValue)
        'Dim ds As DataSet = CType(e.ReturnValue, DataSet)
        'Dim dt As DataTable = ds.Tables(0)
        Dim dt As Projected_Sales.Vehicle_CopyDataTable = CType(e.ReturnValue, Projected_Sales.Vehicle_CopyDataTable)

        ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
        If dt.Rows.Count = 0 Then
            dt.Rows.Add(dt.NewRow())
            LoadDataEmpty = True
        Else
            LoadDataEmpty = False
        End If
    End Sub

    Protected Sub gvCopy_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCopy.RowCreated
        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
        ' when binding a row, look for a zero row condition based on the flag.
        ' if we have zero data rows (but a dummy row), hide the grid view row
        ' and clear the controls off of that row so they don't cause binding errors

        Dim blnLoadDataEmpty As Boolean = LoadDataEmpty
        If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Visible = False
            e.Row.Controls.Clear()
        End If
    End Sub

#End Region ' Insert Empty GridView Work-Around


End Class
