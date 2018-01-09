''******************************************************************************************************
''* CustomersBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select and Update.
''*
''* Author  : RCarlson 04/07/2008
''* Modified: {Name} {Date} - {Notes}
''           RCarlson 08/20/2008 - Customer Maint Table is no longer used. Instead  View is.
''******************************************************************************************************
Imports CustomerTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CustomersBLL
    Private CustomerAdapter As CustomerTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CustomerTableAdapters.CustomerTableAdapter
        Get
            If CustomerAdapter Is Nothing Then
                CustomerAdapter = New CustomerTableAdapter()
            End If
            Return CustomerAdapter
        End Get
    End Property
    '*****
    '* Select Customers returning all rows
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCustomers(ByVal COMPNY As String, ByVal OEM As String, ByVal CABBV As String, ByVal DABBV As String, ByVal SHIPTO As Integer, ByVal SOLDTO As Integer) As Customer.Customer_MaintDataTable

        Try
            If COMPNY Is Nothing Then
                COMPNY = ""
            End If

            If OEM Is Nothing Then
                OEM = ""
            End If

            If CABBV Is Nothing Then
                CABBV = ""
            End If

            If DABBV Is Nothing Then
                DABBV = ""
            End If

            Return Adapter.GetCustomers(COMPNY, OEM, CABBV, DABBV, SHIPTO, SOLDTO)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "COMPNY: " & COMPNY & ", OEM:" & OEM & ", CABBV: " & CABBV & ", DABBV: " & DABBV & ", SHIPTO: " & SHIPTO & ", SOLDTO: " & SOLDTO & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCustomers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CustomersBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CustomerMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomers : " & commonFunctions.convertSpecialChar(ex.Message, False), "CustomersBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

    ' ''*****
    ' ''* Update Customers
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    'Public Function UpdateCustomer(ByVal RowID As Integer, ByVal Obsolete As Boolean) As Boolean

    '    Try
    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        'MsgBox("RowID: " & RowID)
    '        'MsgBox("Obsolete: " & Obsolete)
    '        'MsgBox("UpdatedBy: " & UpdatedBy)

    '        Dim rowsAffected As Integer = Adapter.UpdateCustomer(RowID, Obsolete, UpdatedBy)
    '        'MsgBox("Rows Affected: " & rowsAffected)
    '        '' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "rowID: " & RowID & ", Obsolete:" & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "UpdateCustomer : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CustomersBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CustomerMaintenance.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetCuUpdateCustomerstomers : " & commonFunctions.convertSpecialChar(ex.Message, False), "CustomersBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try
    'End Function

End Class

