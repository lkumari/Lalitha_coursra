' ************************************************************************************************
'
' Name:	        DrawingCustomerImageView.vb
' Purpose:	    This code is used to show all PDF Files inside popup windows
' Called From : Drawing_Detail.aspx
'
'' Date		                Author	    
'' 10/13/2009       	    Roderick Carlson			Created .Net application
Partial Class DrawingCustomerImageView
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
            End If

            If ViewState("DrawingNo") <> "" Then
                Dim ds As DataSet = PEModule.GetDrawingCustomerImages(ViewState("DrawingNo"))
                If commonFunctions.CheckDataset(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("CustomerImage") IsNot System.DBNull.Value Then
                        'Response.Buffer = True
                        'Response.Expires = 0
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("Content-Type", "application/pdf")
                        Response.AddHeader("Content-Disposition", "inline;filename=" & ViewState("DrawingNo") & " Customer Drawing Image")
                        Response.BinaryWrite(ds.Tables(0).Rows(0).Item("CustomerImage"))
                        'Response.End()
                    End If
                Else
                    lblMessage.Text = "Error: File Not Found"
                End If
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
