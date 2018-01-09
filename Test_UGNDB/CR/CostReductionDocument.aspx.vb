' ************************************************************************************************
' Name:	CostReductionDocument.aspx.vb
' Purpose:	This program is used to help display the file uploads in PDF format.
'
' Date		    Author	    
' 02/09/2011    LRey			    Created .Net application
' 03/01/2011    Roderick Carlson    Modified : added extra code to hide PDF Prompts
' 01/23/2012    Roderick Carlson    Modified : Allow Word 2007 and Excel 2007 to be viewed
' ************************************************************************************************
Partial Class CR_CostReductionDocument
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'get the file name and location, then redirect to it.
            'Dim sFile As String = "file://" & System.Configuration.ConfigurationManager.AppSettings("ARpdfLocation") & Session("PDFName")
            'Response.Redirect(sFile, False)

            If Not Page.IsPostBack Then
                Dim strSupportingFileName As String = ""

                If HttpContext.Current.Request.QueryString("pProjNo") <> "" Then
                    ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
                Else
                    ViewState("pProjNo") = ""
                End If

                If HttpContext.Current.Request.QueryString("pDocID") > 0 Then
                    ViewState("pDocID") = HttpContext.Current.Request.QueryString("pDocID")
                Else
                    ViewState("pDocID") = 0
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

                If ViewState("pDocID") > 0 And ViewState("pProjNo") <> "" Then

                    Dim ds As DataSet = CRModule.GetCostReductionDocument(ViewState("pProjNo"), ViewState("pDocID"))

                    If commonFunctions.CheckDataSet(ds) = True Then

                        If ds.Tables(0).Rows(0).Item("BinaryFile") IsNot System.DBNull.Value Then

                            'Response.AddHeader("Content-Type", ds.Tables(0).Rows(0).Item("EncodeType"))
                            'Response.Charset = ""
                            'Response.AddHeader("Content-Disposition", "inline;filename=" & ds.Tables(0).Rows(0).Item("FileName"))
                            'Response.BinaryWrite(ds.Tables(0).Rows(0).Item("BinaryFile"))


                            Dim imagecontent As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("BinaryFile"), Byte())
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
                        Else
                            lblErrors.Text = "No binary file is found for this record.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                            'lblErrors.Visible = True
                        End If
                    
                    Else
                        lblErrors.Text = "No supporting document record is found for this project.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                        'lblErrors.Visible = True
                    End If
                Else
                    lblErrors.Text = "No project or record id is found to get a supporting document. <a href='javascript:window.close();'> Close Window</a>"
                    'lblErrors.Visible = True
                End If
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
