''******************************************************************************************************
''* CRCustomerProgramBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 02/15/2010
''* Modified: Roderick Carlson 12/10/2012 - DB Cleanup
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRCustomerProgramBLL
    Private pAdapter As Cost_Reduction_Customer_Program_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Customer_Program_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Customer_Program_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_CustomerProgram returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionCustomerProgram(ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_Customer_ProgramDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Customer_Program(ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Delete Cost_Reduction_CustomerProgram
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionCustomerProgram(ByVal ProjectNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.Delete_Cost_Reduction_Customer_Program(original_RowID, ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & "ProjectNo: " & ProjectNo & ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRCustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRCustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function
End Class
