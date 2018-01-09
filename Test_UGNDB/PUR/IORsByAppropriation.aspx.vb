' ************************************************************************************************
' Name:		IORsByAppropriation.aspx
' Purpose:	This program is used to display a list of IOR's that are associated to the assigned Appropriation. 
'
' Date		    Author	    
' 06/10/2011    LRey			Created .Net application
' ************************************************************************************************

Partial Class PUR_IORsByAppropriation
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = ""
            End If

            Dim m As ASP.lookupmasterpage_master = Master
            ' ''check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "IOR's by Appropriation #" & ViewState("pProjNo")
                mpTextBox.Font.Size = 18
                mpTextBox.Visible = True
                mpTextBox.Font.Bold = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load
    Protected Sub gvIOR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvIOR.RowDataBound
        Try
            ''***
            ''This section provides the user with the popup for confirming the delete of a record.
            ''Called by the onClientClick event.
            ''***
            If e.Row.RowType = DataControlRowType.Footer Then
                ''Display Totals at footer
                Dim ds2 As DataSet = New DataSet
                ds2 = PURModule.GetInternalOrderRequestCapEx(0, ViewState("pProjNo"))
                If commonFunctions.CheckDataSet(ds2) = True Then
                    e.Row.Cells(2).Wrap = False
                    e.Row.Cells(2).Font.Size = 10
                    e.Row.Cells(2).Text = "Approved Spend:"
                    e.Row.Cells(2).ForeColor = Color.Black
                    e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right

                    e.Row.Cells(3).Wrap = False
                    e.Row.Cells(3).Font.Size = 10
                    e.Row.Cells(3).Text = String.Format("{0:c}", ds2.Tables(0).Rows(0).Item("AllowedToSpend"))
                    e.Row.Cells(3).ForeColor = Color.Red
                    e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right

                    e.Row.Cells(6).Wrap = False
                    e.Row.Cells(6).Font.Size = 10
                    e.Row.Cells(6).Text = "Remaining Balance:"
                    e.Row.Cells(6).ForeColor = Color.Black
                    e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right

                    e.Row.Cells(7).Wrap = False
                    e.Row.Cells(7).Font.Size = 10
                    e.Row.Cells(7).Text = String.Format("{0:c}", ds2.Tables(0).Rows(0).Item("RemSpendAmount")) ''remove duplicate
                    e.Row.Cells(7).ForeColor = Color.Red
                    e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right


                    e.Row.Cells(4).Wrap = False
                    e.Row.Cells(4).Font.Size = 10
                    e.Row.Cells(4).Text = "Total IOR Spend: "
                    e.Row.Cells(4).ForeColor = Color.Black
                    e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right

                    e.Row.Cells(5).Wrap = False
                    e.Row.Cells(5).Font.Size = 10
                    e.Row.Cells(5).Text = String.Format("{0:c}", ds2.Tables(0).Rows(0).Item("IORTotalSpent")) ''remove duplicate
                    e.Row.Cells(5).ForeColor = Color.Red
                    e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
                End If
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblErrors.Text = ex.Message & "<br/>" & mb.Name
            lblErrors.Visible = True

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF gvYearVolume_RowDataBound

End Class
