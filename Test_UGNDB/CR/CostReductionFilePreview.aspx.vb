' *********************************************************************************************
' Name:	CR_CostReductionFilePreview.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update/delete commands.
'
' Date		    Author	    
' 03/01/2011    Roderick Carlson    Adjusted PDF code to avoid prompt
Partial Class CR_CostReductionFilePreview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If HttpContext.Current.Request.QueryString("pProjNo") > 0 Then
                ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
            Else
                ViewState("pProjNo") = 0
            End If

            If ViewState("pProjNo") > 0 Then

                Dim ds As DataSet = CRModule.GetCostReduction(ViewState("pProjNo"), 0, "", 0, 0, "", 0, False, False, "")
                If commonFunctions.CheckDataSet(ds) = True Then

                    If ds.Tables(0).Rows(0).Item("BinaryFile") IsNot System.DBNull.Value Then

                        Response.AddHeader("Content-Type", ds.Tables(0).Rows(0).Item("EncodeType"))
                        Response.Charset = ""
                        Response.AddHeader("Content-Disposition", "inline;filename=" & ds.Tables(0).Rows(0).Item("FileName"))
                        Response.BinaryWrite(ds.Tables(0).Rows(0).Item("BinaryFile"))
                    Else
                        lblErrors.Text = "No supporting documents exist for this project.<br/><br/><a href='javascript:window.close();'><u>Close Window</u></a>"
                        lblErrors.Visible = True
                    End If
                
                Else
                    lblErrors.Text = "File not listed in database. <a href='javascript:window.close();'> Close Window</a>"
                    lblErrors.Visible = True
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
