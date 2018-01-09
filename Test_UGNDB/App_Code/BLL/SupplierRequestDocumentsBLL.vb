''****************************************************************************************************
''* SupplierRequestDocumentsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 09/21/2010
''****************************************************************************************************

Imports SupplierTableAdapters
<System.ComponentModel.DataObject()> _
Public Class SupplierRequestDocumentsBLL
    Private pAdapter As Supplier_Request_Documents_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SupplierTableAdapters.Supplier_Request_Documents_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Supplier_Request_Documents_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Supplier_Request_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSupplierRequestDocuments(ByVal SUPNo As Integer, ByVal DocID As Integer) As Supplier.Supplier_Request_DocumentsDataTable

        Try
            If DocID = Nothing Then DocID = 0

            Return Adapter.Get_Supplier_Request_Documents(SUPNo, DocID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", DocID: " & DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequestDocumentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequestDocumentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetSupplierRequestDocuments

    ''*****
    ''* Delete Supplier_Request_Documents
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteSupplierRequestDocuments(ByVal SUPNo As Integer, ByVal Original_DocID As Integer, ByVal Original_SUPNo As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Supplier_Request_Documents(Original_DocID, Original_SUPNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SUPNo: " & SUPNo & ", DocID: " & Original_DocID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> SupplierRequestDocumentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/SUP/SupplierRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSupplierRequestDocuments : " & commonFunctions.replaceSpecialChar(ex.Message, False), "SupplierRequestDocumentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteSupplierRequestDocuments

End Class

