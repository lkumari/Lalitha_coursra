''****************************************************************************************************
''* CostReductionDocumentsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 02/08/2011
''****************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRDocumentsBLL
    Private pAdapter As Cost_Reduction_Documents_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Documents_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Documents_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionDocuments(ByVal ProjectNo As Integer, ByVal DocID As Integer) As CostReduction.Cost_Reduction_DocumentsDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Documents(ProjectNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostReductionDocumentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostReductionDocumentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetCostReductionDocuments
    ''*****
    ''* Delete Cost_Reduction_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionDocuments(ByVal ProjectNo As Integer, ByVal Original_DocID As Integer, ByVal Original_ProjectNo As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Cost_Reduction_Documents(Original_DocID, Original_ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostReductionDocumentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostReductionDocumentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteCostReductionDocuments

End Class

