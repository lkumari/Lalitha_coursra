''******************************************************************************************************
''* RFDVendorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDVendorBLL
    Private RFDVendorAdapter As RFDVendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDVendorTableAdapter
        Get
            If RFDVendorAdapter Is Nothing Then
                RFDVendorAdapter = New RFDVendorTableAdapter()
            End If
            Return RFDVendorAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDVendor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDVendor(ByVal RFDNo As Integer) As RFD.RFDVendor_MaintDataTable

        Try

            Return Adapter.GetRFDVendor(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New RFDVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRFDVendor(ByVal RFDNo As Integer, ByVal UGNDBVendorID As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertRFDVendor(RFDNo, UGNDBVendorID, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", UGNDBVendorID:" & UGNDBVendorID _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update RFDVendor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDVendor(ByVal original_RowID As Integer, ByVal RFDNo As Integer, ByVal UGNDBVendorID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.UpdateRFDVendor(original_RowID, RFDNo, UGNDBVendorID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo _
            & ", UGNDBVendorID:" & UGNDBVendorID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Delete RFDVendor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDVendor(ByVal RowID As Integer, ByVal original_RowID As Integer, ByVal RFDNo As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDVendor(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDVendor(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
