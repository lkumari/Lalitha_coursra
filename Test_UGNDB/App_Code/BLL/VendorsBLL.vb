''******************************************************************************************************
''* VendorsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 03/24/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************


Imports VendorsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class VendorsBLL
    Private VendorAdapter As VendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As VendorsTableAdapters.VendorTableAdapter
        Get
            If VendorAdapter Is Nothing Then
                VendorAdapter = New VendorTableAdapter()
            End If
            Return VendorAdapter
        End Get
    End Property
    ''*****
    ''* Select Vendors returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetVendors(ByVal Vendor As Integer, ByVal VendorName As String, ByVal VendorAddress As String, ByVal VendorState As String, ByVal VendorZipCode As String, ByVal VendorCountry As String, ByVal VendorPhone As String, ByVal VendorFAX As String, ByVal VendorType As String) As Vendors.Vendor_MaintDataTable

        Try
            If VendorName Is Nothing Then VendorName = ""

            If VendorAddress Is Nothing Then VendorAddress = ""

            If VendorState Is Nothing Then VendorState = ""

            If VendorCountry Is Nothing Then VendorCountry = ""

            If VendorZipCode Is Nothing Then VendorZipCode = ""

            If VendorPhone Is Nothing Then VendorPhone = ""

            If VendorFAX Is Nothing Then VendorFAX = ""

            If VendorType Is Nothing Then VendorType = ""

            Return Adapter.GetVendors(Vendor, VendorName, VendorAddress, VendorState, VendorZipCode, VendorCountry, VendorPhone, VendorFAX, VendorType)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Vendor: " & Vendor & ", VendorName:" & VendorName & "VendorAddress: " & VendorAddress & "VendorState: " & VendorState & "VendorZipCode: " & VendorZipCode & "VendorCountry: " & VendorCountry & "VendorPhone: " & VendorPhone & "VendorFAX: " & VendorFAX & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetVendors : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> VendorsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/VendorMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetVendors : " & commonFunctions.convertSpecialChar(ex.Message, False), "VendorsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ' ''*****
    ' ''* Update Vendors
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    'Public Function UpdateVendors(ByVal Obsolete As Boolean, ByVal original_Vendor As Integer) As Boolean

    '    Try
    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.UpdateVendor(original_Vendor, Obsolete, UpdatedBy)
    '        'MsgBox("Rows Affected: " & rowsAffected)
    '        '' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "Obsolete: " & Obsolete & ", original_Vendor:" & original_Vendor & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "UpdateVendors : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> VendorsBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/VendorMaintenance.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateVendors : " & commonFunctions.convertSpecialChar(ex.Message, False), "VendorsBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

End Class
