''******************************************************************************************************
''* CRHistoryBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 01/29/2010
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRHistoryBLL
    Private pAdapter As Cost_Reduction_History_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_History_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_History_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_History returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionHistory(ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_HistoryDataTable

        Try
            Return Adapter.Get_Cost_Reduction_History(ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRHistoryBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRHistoryBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
End Class
