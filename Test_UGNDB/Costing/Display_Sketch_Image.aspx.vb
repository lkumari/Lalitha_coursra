' ************************************************************************************************
'
' Name:	        Display_Sketch_Image.vb
' Purpose:	    This code is used by the Cost Sheet in order to show binary sketch images pulled from the SQL Server Database
' Called From:  Cost_Sheet_Detail.aspx
'
' Date		        Author	    
' 02/02/2009      	RC			Created .Net application
Partial Class Costing_DisplaySketchImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")

                Dim ds As DataSet = CostingModule.GetCostSheetSketchInfo(ViewState("CostSheetID"))
                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                        Response.ContentType = "image/pjpeg"
                        'Dim strTemp As String = ds.Tables(0).Rows(0).Item("DrawingNo")

                        If ds.Tables(0).Rows(0).Item("SketchImage") IsNot System.DBNull.Value Then
                            Response.BinaryWrite(ds.Tables(0).Rows(0).Item("SketchImage"))                            
                        End If

                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
End Class
