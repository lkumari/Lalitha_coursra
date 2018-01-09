' ************************************************************************************************
'
' Name:	        PDFview.vb
' Purpose:	    This code is used to show all PDF Files inside popup windows
' Called From : ECI_Detail.aspx
'
'' Date		                Author	    
'' 06/30/2009       	    Roderick Carlson			Created .Net application
'' 12/06/2011               Roderick Carlson            Allow many types of documents
Partial Class ECI_Supporting_Doc_View
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim strSupportingFileName As String = ""

            If HttpContext.Current.Request.QueryString("RowID") <> "" Then
                ViewState("RowID") = CType(HttpContext.Current.Request.QueryString("RowID"), Integer)
            End If

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                ' commonFunctions.SetUGNDBUser()
                Dim FullName As String = commonFunctions.getUserName()
                Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If
                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                    Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                    Response.Cookies("UGNDB_UserFullName").Value = FullName

                End If
            End If

            If ViewState("RowID") > 0 Then
                Dim ds As DataSet = ECIModule.GetECISupportingDoc(ViewState("RowID"))
                If commonFunctions.CheckDataSet(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("SupportingDocBinary") IsNot System.DBNull.Value Then
                        'Response.ContentType = "application/pdf"
                        'Response.AddHeader("Content-Type", "application/pdf")
                        'Response.Charset = ""
                        'Response.AddHeader("Content-Disposition", "inline;filename=" & ViewState("SupportingDocName"))
                        'Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SupportingDocBinary"))

                        strSupportingFileName = ds.Tables(0).Rows(0).Item("SupportingDocName").ToString

                        If strSupportingFileName.Trim = "" Then
                            strSupportingFileName = "ECI-SupportingDoc.pdf"
                        End If

                        Dim imagecontent As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("SupportingDocBinary"), Byte())
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = ds.Tables(0).Rows(0).Item("SupportingDocEncodeType").ToString()

                        'avoid the prompt if PDF of JPEG
                        If ds.Tables(0).Rows(0).Item("SupportingDocEncodeType").ToString() = "application/pdf" _
                            Or ds.Tables(0).Rows(0).Item("SupportingDocEncodeType").ToString() = "image/pjpeg" Then
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
