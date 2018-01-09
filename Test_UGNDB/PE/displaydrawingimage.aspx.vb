' ************************************************************************************************
'
' Name:	        displaydrawingimage.vb
' Purpose:	    This code is used by the DrawingDetail in order to show binary drawing images pulled from the SQL Server Database
' Called From:  DrawingDetail.aspx
'
' Date		        Author	    
' 08/18/2008      	RC			Created .Net application

Partial Class displaydrawingimage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
            ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
        End If

        If HttpContext.Current.Request.QueryString("AlternativeDrawingNo") <> "" Then
            ViewState("AlternativeDrawingNo") = HttpContext.Current.Request.QueryString("AlternativeDrawingNo")
        End If

        Try

            Dim ds As DataSet = PEModule.GetDrawingImages(ViewState("DrawingNo"), ViewState("AlternativeDrawingNo"))
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                    Response.ContentType = "image/pjpeg"
                    Dim strTemp As String = ds.Tables(0).Rows(0).Item("DrawingNo")
                    Response.BinaryWrite(ds.Tables(0).Rows(0).Item("DrawingImage"))
                End If
            End If

        Catch ex As Exception

        End Try


    End Sub
End Class
