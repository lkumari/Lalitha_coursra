
Partial Class Acoustic_ProjectReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'get the file name and location, then redirect to it.
            If HttpContext.Current.Request.QueryString("pRptID") > 0 Then
                ViewState("pRptID") = HttpContext.Current.Request.QueryString("pRptID")
            Else
                ViewState("pRptID") = 0
            End If

            If HttpContext.Current.Request.QueryString("pProjID") > 0 Then
                ViewState("pProjID") = HttpContext.Current.Request.QueryString("pProjID")
            Else
                ViewState("pProjID") = 0
            End If

            If ViewState("pRptID") > 0 And ViewState("pProjID") > 0 Then

                Dim ds As DataSet = AcousticModule.GetAcousticProjectReport(ViewState("pProjID"), ViewState("pRptID"))

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
