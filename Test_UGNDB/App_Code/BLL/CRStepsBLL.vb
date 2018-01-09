''******************************************************************************************************
''* CRStepsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 01/13/2010
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRStepsBLL
    Private pAdapter As Cost_Reduction_Steps_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Steps_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Steps_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_Steps returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionSteps(ByVal StepID As Integer, ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_StepsDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Steps(StepID, ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StepID: " & StepID & ", ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRStepsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRStepsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Delete Cost_Reduction_Steps
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionSteps(ByVal StepID As Integer, ByVal ProjectNo As Integer, ByVal original_StepID As Integer, ByVal original_ProjectNo As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.sp_Delete_Cost_Reduction_Steps(original_StepID, original_ProjectNo, UpdatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StepID: " & StepID & "ProjectNo: " & ProjectNo & ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRStepsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionSteps : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRStepsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function
End Class
