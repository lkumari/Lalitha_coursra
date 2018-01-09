' ************************************************************************************************
' Name:	InternalOrderRequestDocument.aspx.vb
' Purpose:	This program is used to help display the file uploads in PDF format.
'
' Date		    Author	    
' 09/13/2010    LRey			Created .Net application
' ************************************************************************************************
Partial Class PUR_InternalOrderRequestDocument
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pIORNo") <> "" Then
                ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
            Else
                ViewState("pIORNo") = ""
            End If

            If HttpContext.Current.Request.QueryString("pDocID") > 0 Then
                ViewState("pDocID") = HttpContext.Current.Request.QueryString("pDocID")
            Else
                ViewState("pDocID") = 0
            End If

            If ViewState("pDocID") > 0 And ViewState("pIORNo") <> "" Then

                Dim ds As DataSet = PURModule.GetInternalOrderRequestDocument(ViewState("pIORNo"), ViewState("pDocID"))

                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then

                        If ds.Tables(0).Rows(0).Item("BinaryFile") IsNot System.DBNull.Value Then
                            Dim imagecontent As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("BinaryFile"), Byte())
                            Response.Clear()
                            Response.Buffer = True
                            Response.ContentType = ds.Tables(0).Rows(0).Item("EncodeType").ToString()

                            Response.AddHeader("Content-Disposition", "attachment;filename=" & ds.Tables(0).Rows(0).Item("FileName"))
                            Response.OutputStream.Write(imagecontent, 0, imagecontent.Length - 1)
                            Response.Flush()
                            Response.Close()

                        Else
                            lblErrors.Text = "File not listed in database.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                            lblErrors.Visible = True
                        End If
                    Else
                        lblErrors.Text = "File not listed in database.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                        lblErrors.Visible = True
                    End If
                Else
                    lblErrors.Text = "File not listed in database.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                    lblErrors.Visible = True
                End If
            Else
                lblErrors.Text = "File not listed in database. <a href='javascript:window.close();'> Close Window</a>"
                lblErrors.Visible = True
            End If

        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
