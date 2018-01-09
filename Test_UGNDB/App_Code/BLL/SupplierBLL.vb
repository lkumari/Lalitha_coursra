''****************************************************************************************************
''* SupplierBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 05/12/2011
''****************************************************************************************************

Imports SupplierTableAdapters
<System.ComponentModel.DataObject()> _
Public Class SupplierBLL
    Private pAdapter As Supplier_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SupplierTableAdapters.Supplier_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Supplier_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Supplier_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSupplierLookUp(ByVal SUPNo As String, ByVal VendorName As String, ByVal VendorType As String, ByVal RecStatus As String, ByVal VendorNo As String, ByVal BtnSrch As Boolean) As Supplier.SupplierDataTable

        Try
            If SUPNo = Nothing Then SUPNo = ""
            If VendorName = Nothing Then VendorName = ""
            If VendorType = Nothing Then VendorType = ""
            If RecStatus = Nothing Then RecStatus = ""
            If VendorNo = Nothing Then VendorNo = ""

            Return Adapter.Get_Supplier(SUPNo, VendorName, VendorType, RecStatus, VendorNo, BtnSrch)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & "VendorName: " & VendorName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierLookUp : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierLookUp.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierLookUp : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetSupplierLookUp

End Class

