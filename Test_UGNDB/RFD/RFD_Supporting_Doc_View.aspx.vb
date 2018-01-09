' ************************************************************************************************
'
' Name:	        RFD_Supporting_Doc_View.vb
' Purpose:	    This code is used to show all PDF Files inside popup windows
' Called From : RFD_Detail.aspx
'
'' Date		                Author	    
'' 02/01/2009       	    Roderick Carlson			Created .Net application
'' 08/01/2011               Roderick Carlson            Modified - Refresh User Name cookies if blank, such as clicking on a link in email
'' 11/30/2011               Roderick Carlson            Allow Word 2007 and Excel 2007 to be viewed
Partial Class RFD_Supporting_Doc_View
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim strSupportingFileName As String = ""

            If HttpContext.Current.Request.QueryString("RowID") <> "" Then
                ViewState("RowID") = CType(HttpContext.Current.Request.QueryString("RowID"), Integer)
            End If

            If ViewState("RowID") > 0 Then
                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                    commonFunctions.SetUGNDBUser()
                End If

                Dim ds As DataSet = RFDModule.GetRFDSupportingDoc(ViewState("RowID"))
                If commonFunctions.CheckDataSet(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("SupportingDocBinary") IsNot System.DBNull.Value Then

                        strSupportingFileName = ds.Tables(0).Rows(0).Item("SupportingDocName").ToString

                        If strSupportingFileName.Trim = "" Then
                            strSupportingFileName = "RFD-SupportingDoc.pdf"
                        End If

                        ' ''Response.Buffer = True
                        ' ''Response.Expires = 0
                        ''Response.ContentType = "application/pdf"
                        ''Response.AddHeader("Content-Type", "application/pdf")
                        ''Response.AddHeader("Content-Disposition", "inline;filename=" & ViewState("SupportingDocName"))

                        'Response.Clear()
                        'Response.Buffer = True
                        ''Response.ContentType = "application/pdf"
                        'Response.AddHeader("Content-Type", ds.Tables(0).Rows(0).Item("EncodeType"))
                        'Response.Charset = ""
                        ''Response.AddHeader("Content-Disposition", "inline;filename=" & ds.Tables(0).Rows(0).Item("FileName"))
                        'Response.AddHeader("Content-Disposition", "inline;filename=" & strSupportingFileName)
                        ''Response.BinaryWrite(ds.Tables(0).Rows(0).Item("BinaryFile"))
                        'Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SupportingDocBinary"))

                        ''Response.End()

                        Dim imagecontent As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("SupportingDocBinary"), Byte())
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = ds.Tables(0).Rows(0).Item("EncodeType").ToString()

                        'avoid the prompt if PDF of JPEG
                        If ds.Tables(0).Rows(0).Item("EncodeType").ToString() = "application/pdf" _
                            Or ds.Tables(0).Rows(0).Item("EncodeType").ToString() = "image/pjpeg" Then
                            Response.AddHeader("Content-Disposition", "inline;filename=" & strSupportingFileName)
                        Else
                            Response.AddHeader("Content-Disposition", "attachment;filename=" & strSupportingFileName)
                        End If

                        Response.OutputStream.Write(imagecontent, 0, imagecontent.Length - 1)
                        Response.Flush()
                        Response.Close()
                        'Response.End()

                    End If
                End If
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
