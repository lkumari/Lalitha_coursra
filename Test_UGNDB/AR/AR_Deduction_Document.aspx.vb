' ************************************************************************************************
' Name:	AR_Deduction_Document.aspx.vb
' Purpose:	This program is used to help display the file uploads in PDF format.
'
' Date		   Author	    
' 04/23/2012   LRey			Created .Net application
' ************************************************************************************************
Partial Class AR_Deduction_Document
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'get the file name and location, then redirect to it.
        If HttpContext.Current.Request.QueryString("pARDID") <> "" Then
            ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
        Else
            ViewState("pARDID") = ""
        End If

        If HttpContext.Current.Request.QueryString("pDocID") > 0 Then
            ViewState("pDocID") = HttpContext.Current.Request.QueryString("pDocID")
        Else
            ViewState("pDocID") = 0
        End If

        If ViewState("pDocID") > 0 And ViewState("pARDID") <> "" Then

            Dim ds As DataSet = ARGroupModule.GetARDeductionDocuments(ViewState("pARDID"), ViewState("pDocID"), False)

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
    End Sub
End Class
