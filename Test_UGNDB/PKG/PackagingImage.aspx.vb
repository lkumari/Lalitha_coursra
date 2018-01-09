' ************************************************************************************************
' Name:	    PackagingImage.vb
' Purpose:  This code is used by the PKG_Layout in order to show binary Packaging images pulled from the SQL Server Database
' Called From:  Packaging.aspx
'
' Date		        Author	    
' 09/26/2012        LREY  			Created .Net application

Partial Class PackagingImage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If HttpContext.Current.Request.QueryString("pPKGID") <> "" Then
                ViewState("pPKGID") = HttpContext.Current.Request.QueryString("pPKGID")
            End If

            Dim ds As DataSet = PKGModule.GetPKGLayout(ViewState("pPKGID"))
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                    Response.ContentType = "image/pjpeg"
                    If ds.Tables(0).Rows(0).Item("BinaryFile") IsNot System.DBNull.Value Then
                        Response.BinaryWrite(ds.Tables(0).Rows(0).Item("BinaryFile"))
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
