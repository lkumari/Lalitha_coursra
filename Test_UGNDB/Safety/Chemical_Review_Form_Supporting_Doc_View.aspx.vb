' ************************************************************************************************
'
' Name:	        Chemical_Review_Form_Supporting_Doc_View.vb
' Purpose:	    This code is used to show all PDF Files inside popup windows
' Called From : Chemical_Review_Form_Detail.aspx
'
'' Date		                Author	    
'' 02/10/2010       	    Roderick Carlson			Created .Net application
Partial Class Chemical_Review_Form_Supporting_Doc_View
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If HttpContext.Current.Request.QueryString("RowID") <> "" Then
                ViewState("RowID") = CType(HttpContext.Current.Request.QueryString("RowID"), Integer)
            End If

            If ViewState("RowID") > 0 Then
                Dim ds As DataSet = SafetyModule.GetChemicalReviewFormSupportingDoc(ViewState("RowID"))
                If commonFunctions.CheckDataset(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("SupportingDocBinary") IsNot System.DBNull.Value Then
                        'Response.Buffer = True
                        'Response.Expires = 0
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("Content-Type", "application/pdf")
                        Response.Charset = ""                        
                        Response.AddHeader("Content-Disposition", "inline;filename=" & ViewState("SupportingDocName") & "preview.pdf")
                        Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SupportingDocBinary"))
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
