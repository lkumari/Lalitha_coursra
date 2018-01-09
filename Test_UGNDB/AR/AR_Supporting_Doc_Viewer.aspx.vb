' ************************************************************************************************
'
' Name:	        AR_Supporting_Doc_Viewer.vb
' Purpose:	    This code is used to show all PDF Files inside popup windows
' Called From : AR_Event_Detail.aspx
'
'' Date		       Author	    
'' 03/31/2010      Roderick Carlson			Created .Net application
'' 11/29/2011      Roderick Carlson         Allow Word 2007 and Excel 2007 to be viewed
Partial Class AR_Supporting_Doc_Viewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try            

            If Not Page.IsPostBack Then
                Dim strSupportingFileName As String = ""

                If HttpContext.Current.Request.QueryString("RowID") <> "" Then
                    ViewState("RowID") = CType(HttpContext.Current.Request.QueryString("RowID"), Integer)
                End If

                If HttpContext.Current.Request.QueryString("AREID") <> "" Then
                    ViewState("AREID") = CType(HttpContext.Current.Request.QueryString("AREID"), Integer)
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

                If ViewState("AREID") > 0 And ViewState("RowID") > 0 Then
                    Dim ds As DataSet = ARGroupModule.GetAREventSupportingDoc(ViewState("RowID"), ViewState("AREID"))
                    If commonFunctions.CheckDataSet(ds) = True Then

                        If ds.Tables(0).Rows(0).Item("SupportingDocBinary") IsNot System.DBNull.Value Then

                            strSupportingFileName = ds.Tables(0).Rows(0).Item("SupportingDocName").ToString

                            If strSupportingFileName.Trim = "" Then
                                strSupportingFileName = "AR-SupportingDoc.pdf"
                            End If

                            ' '' ''Response.Buffer = True
                            ' '' ''Response.Expires = 0
                            '' ''Response.ContentType = "application/pdf"
                            '' ''Response.AddHeader("Content-Type", "application/pdf")
                            '' ''Response.AddHeader("Content-Disposition", "inline;filename=" & ViewState("SupportingDocName"))
                            '' ''Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SupportingDocBinary"))
                            ' '' ''Response.End()

                            ' ''Response.Clear()
                            ' ''Response.Buffer = True                            
                            ' ''Response.AddHeader("Content-Type", ds.Tables(0).Rows(0).Item("SupportingDocEncodeType"))
                            ' ''Response.Charset = ""                          
                            ' ''Response.AddHeader("Content-Disposition", "inline;filename=" & strSupportingFileName)                            
                            ' ''Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SupportingDocBinary"))

                            ''Dim imagecontent As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("SupportingDocBinary"), Byte())
                            ''Response.ContentType = ds.Tables(0).Rows(0).Item("SupportingDocEncodeType").ToString()
                            ''Response.AddHeader("Content-Disposition", "attachment;filename=" & strSupportingFileName)
                            ''Context.Response.BinaryWrite(imagecontent)

                            'common content types
                            'application/msword
                            'application/pdf
                            'application/vnd.ms-excel
                            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
                            'application/vnd.openxmlformats-officedocument.wordprocessingml.document
                            'image/pjpeg

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
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
