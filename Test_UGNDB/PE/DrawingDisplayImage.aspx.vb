' ************************************************************************************************
'
' Name:	        DrawingDisplayImage.vb
' Purpose:	    This code is used by the DrawingDetail in order to show binary drawing images pulled from the SQL Server Database
' Called From:  DrawingDetail.aspx
'
' Date		        Author	    
' 08/18/2008      	RC			Created .Net application
' 11/13/2008        RC          If image is empty, do not display it.

Partial Class DrawingDisplayImage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
            End If

            If HttpContext.Current.Request.QueryString("AlternativeDrawingNo") <> "" Then
                ViewState("AlternativeDrawingNo") = HttpContext.Current.Request.QueryString("AlternativeDrawingNo")
            End If

            Dim ds As DataSet = PEModule.GetDrawingImages(ViewState("DrawingNo"), ViewState("AlternativeDrawingNo"))
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                    Response.ContentType = "image/pjpeg"
                    Dim strTemp As String = ds.Tables(0).Rows(0).Item("DrawingNo")
                    If ds.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
                        Response.BinaryWrite(ds.Tables(0).Rows(0).Item("DrawingImage"))                   
                    End If
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblImageMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
