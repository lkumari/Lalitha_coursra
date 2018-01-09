''******************************************************************************************************
''* ExpProjCapitalSpendingBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 03/24/2010
''******************************************************************************************************

Imports ExpProjTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ExpProjCapitalSpendingBLL
    Private pAdapter As ExpProj_Capital_Spending_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ExpProjTableAdapters.ExpProj_Capital_Spending_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New ExpProj_Capital_Spending_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select ExpProj_Capital_Spending returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjCapitalSpending(ByVal CapitalSpendingName As String) As ExpProj.ExpProj_Capital_SpendingDataTable

        Try
            Return Adapter.Get_ExpProj_Capital_Spending(CapitalSpendingName)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalSpendingName: " & CapitalSpendingName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCapitalSpendingBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Capital_Spending.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCapitalSpendingBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjCapitalSpending

    ''*****
    ''* Insert New ExpProj_Capital_Spending
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertExpProjCapitalSpending(ByVal CapitalSpendingName As String, ByVal CSCode As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            CapitalSpendingName = commonFunctions.replaceSpecialChar(CapitalSpendingName, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_ExpProj_Capital_Spending(CapitalSpendingName, CSCode, CreatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalSpendingName: " & CapitalSpendingName & ", CSCode:" & CSCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCapitalSpendingBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Capital_Spending.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCapitalSpendingBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertExpProjCapitalSpending

    ''*****
    ''* Update ExpProj_Capital_Spending
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateExpProjCapitalSpending(ByVal CapitalSpendingName As String, ByVal CSCode As String, ByVal Obsolete As Boolean, ByVal original_CapitalSpendingName As String, ByVal original_CSCode As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            CapitalSpendingName = commonFunctions.replaceSpecialChar(CapitalSpendingName, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_ExpProj_Capital_Spending(CapitalSpendingName, CSCode, Obsolete, original_CapitalSpendingName, original_CSCode, UpdatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CapitalSpendingName: " & CapitalSpendingName & ", CSCode:" & CSCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCapitalSpendingBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Capital_Spending.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjCapitalSpending : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCapitalSpendingBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function 'EOF UpdateExpProjCapitalSpending
End Class
