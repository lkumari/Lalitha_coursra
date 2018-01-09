''****************************************************************************************************
''* SupplierRequiredFormsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 09/22/2010
''****************************************************************************************************

Imports SupplierTableAdapters
<System.ComponentModel.DataObject()> _
Public Class SupplierRequiredFormsBLL
    Private pAdapter As Supplier_Required_Forms_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SupplierTableAdapters.Supplier_Required_Forms_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Supplier_Required_Forms_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Supplier_Required_Forms_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSupplierRequiredForms(ByVal FormName As String, ByVal VendorType As String) As Supplier.Supplier_Required_FormsDataTable

        Try
            Return Adapter.Get_Supplier_Required_Forms(FormName, VendorType, True)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormName: " & FormName & "VendorType: " & VendorType & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequiredFormsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequiredFormsMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequiredFormsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetSupplierRequiredForms

    ''*****
    ''* Insert New Supplier_Required_Forms_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertSupplierRequiredForms(ByVal FormName As String, ByVal VendorType As String, ByVal RequiredForm As Boolean, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            FormName = commonFunctions.replaceSpecialChar(FormName, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_Supplier_Required_Forms(FormName, VendorType, RequiredForm, Obsolete, CreatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormName: " & FormName & ", VendorType:" & VendorType & ", RequiredForm:" & RequiredForm & ", Obsolete:" & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequiredFormsBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/Supplier_Required_Forms.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequiredFormsBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertSupplierRequiredForms

    ''*****
    ''* Update Supplier_Required_Forms_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSupplierRequiredForms(ByVal FormName As String, ByVal VendorType As String, ByVal RequiredForm As Boolean, ByVal Obsolete As Boolean, ByVal original_FormName As String, ByVal original_VendorType As String, ByVal original_SRFID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            FormName = commonFunctions.replaceSpecialChar(FormName, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_Supplier_Required_Forms(original_SRFID, FormName, VendorType, RequiredForm, Obsolete, UpdatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FormName: " & FormName & ", VendorType:" & VendorType & ", RequiredForm:" & RequiredForm & ", Obsolete:" & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequiredFormsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequiredFormsMaint.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSupplierRequiredForms : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequiredFormsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF UpdateSupplierRequiredForms

End Class

