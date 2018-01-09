''******************************************************************************************************
''* UGNDBVendorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/19/2009
''* Modified: 01/03/2014 LREY   Replaced SupplierNo with SupplierNo
''******************************************************************************************************

Imports UGNDBVendorTableAdapters

<System.ComponentModel.DataObject()> _
Public Class UGNDBVendorBLL
    Private UGNDBVendorAdapter As UGNDBVendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As UGNDBVendorTableAdapters.UGNDBVendorTableAdapter
        Get
            If UGNDBVendorAdapter Is Nothing Then
                UGNDBVendorAdapter = New UGNDBVendorTableAdapter()
            End If
            Return UGNDBVendorAdapter
        End Get
    End Property
    ''*****
    ''* Select UGNDBVendor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetUGNDBVendor(ByVal UGNDBVendorID As Integer, ByVal SupplierNo As Integer, _
        ByVal SupplierName As String, ByVal isActiveBPCS As Boolean) As UGNDBVendor.UGNDBVendor_MaintDataTable

        Try
            If SupplierName Is Nothing Then SupplierName = ""

            Return Adapter.GetUGNDBVendor(UGNDBVendorID, SupplierNo, SupplierName, isActiveBPCS)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNDBVendorID: " & UGNDBVendorID _
            & ", SupplierNo: " & SupplierNo _
            & ", SupplierName: " & SupplierName _
            & ", isActiveBPCS: " & isActiveBPCS _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUGNDBVendor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNDBVendorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetUGNDBVendor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "UGNDBVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New UGNDBVendor
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertUGNDBVendor(ByVal UGNDBVendorName As String, ByVal SupplierNo As Integer, ByVal Obsolete As Boolean) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If UGNDBVendorName Is Nothing Then
    '            UGNDBVendorName = ""
    '        End If

    '        ''*****
    '        ' Insert the record
    '        ''*****
    '        Dim rowsAffected As Integer = Adapter.InsertUGNDBVendor(UGNDBVendorName, SupplierNo, Obsolete, createdBy)

    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "UGNDBVendorName:" & UGNDBVendorName & ", SupplierNo: " & SupplierNo & _
    '        ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertUGNDBVendor : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNDBVendorBLL.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

    '        UGNErrorTrapping.InsertErrorLog("InsertCostingVendor : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "UGNDBVendorBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    ''* Update UGNDBVendorBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateUGNDBVendor(ByVal original_UGNDBVendorID As Integer, ByVal UGNDBVendorName As String, ByVal SupplierNo As Integer, ByVal Obsolete As Boolean, ByVal SupplierName As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNDBVendorName Is Nothing Then UGNDBVendorName = ""

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateUGNDBVendor(original_UGNDBVendorID, UGNDBVendorName, SupplierNo, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNDBVendorID:" & original_UGNDBVendorID _
            & ", UGNDBVendorName: " & UGNDBVendorName _
            & ", SupplierNo: " & SupplierNo _
            & ", Obsolete: " & Obsolete _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateUGNDBVendor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNDBVendorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostingVendor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostingUGNDBBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
