''******************************************************************************************************
''* CRStatusBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 01/19/2010
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRStatusBLL
    Private pAdapter As Cost_Reduction_Status_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Status_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Status_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_Status returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionStatus(ByVal StatusID As Integer, ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_StatusDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Status(StatusID, ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRStatusBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRStatusBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Delete Cost_Reduction_Status
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionStatus(ByVal StatusID As Integer, ByVal ProjectNo As Integer, ByVal original_StatusID As Integer, ByVal original_ProjectNo As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.sp_Delete_Cost_Reduction_Status(original_StatusID, original_ProjectNo, UpdatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StatusID: " & StatusID & "ProjectNo: " & ProjectNo & ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRStatusBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionStatus : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRStatusBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function
End Class
