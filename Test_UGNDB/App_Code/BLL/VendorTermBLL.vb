''****************************************************************************************************
''* VendorTermBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 12/15/2010
''****************************************************************************************************

Imports SupplierTableAdapters
<System.ComponentModel.DataObject()> _
Public Class VendorTermBLL
    Private pAdapter As Vendor_Term_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SupplierTableAdapters.Vendor_Term_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Vendor_Term_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Vendor_Term_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetVendorTerm(ByVal Term As String) As Supplier.Vendor_TermDataTable

        Try
            Return Adapter.Get_Vendor_Term(IIf(Term = Nothing, "", Term))

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Term: " & Term & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Vendor_TermBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/VendorTermMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False), "Vendor_TermBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetVendor_Term

    ''*****
    ''* Insert New Vendor_Term_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertVendorTerm(ByVal Term As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Term = commonFunctions.convertSpecialChar(Term, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_Vendor_Term(Term, Obsolete, CreatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Term: " & Term & ", Obsolete:" & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Vendor_TermBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/VendorTerm.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False), "Vendor_TermBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertVendor_Term

    ''*****
    ''* Update Vendor_Term_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateVendorTerm(ByVal Term As String, ByVal Obsolete As Boolean, ByVal original_Term As String, ByVal original_TID As Integer, ByVal comboUpdateInfo As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Term = commonFunctions.convertSpecialChar(Term, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_Vendor_Term(original_TID, Term, Obsolete, UpdatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Term: " & Term & ", Obsolete:" & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Vendor_TermBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/VendorTermMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateVendor_Term : " & commonFunctions.convertSpecialChar(ex.Message, False), "Vendor_TermBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF UpdateVendor_Term

End Class

