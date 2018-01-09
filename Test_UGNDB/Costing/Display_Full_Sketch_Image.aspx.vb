' ************************************************************************************************
'
' Name:	        Display_Full_Sketch_Image.vb
' Purpose:	    This code is used by the Cost Sheet in order to show binary enlarged sketch images in a popup, pulled from the SQL Server Database
' Called From:  Cost_Sheet_Detail.aspx
'
' Date		        Author	    
' 02/02/2009      	RC			Created .Net application
Partial Class Costing_Display_Full_Sketch_Image
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim ds As DataSet

            If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")

                ''bind existing CostSheet data for Sketch Info                  
                ds = CostingModule.GetCostSheetSketchInfo(ViewState("CostSheetID"))
                If ds IsNot Nothing Then
                    If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then

                        imgDrawingPartSketch.Src = "Display_Sketch_Image.aspx?CostSheetID=" & ViewState("CostSheetID")
                    End If 'end Sketch Info tab load table count > 0
                End If 'end Sketch Info tab load ds is not empty
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub
End Class
