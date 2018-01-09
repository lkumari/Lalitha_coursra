''******************************************************************************************************
''* ECIVendorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/08/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIVendorBLL
    Private ECIVendorAdapter As ECIVendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIVendorTableAdapter
        Get
            If ECIVendorAdapter Is Nothing Then
                ECIVendorAdapter = New ECIVendorTableAdapter()
            End If
            Return ECIVendorAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIVendor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIVendor(ByVal ECINo As Integer) As ECI.ECIVendor_MaintDataTable

        Try

            Return Adapter.GetECIVendor(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECIVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECIVendor(ByVal ECINo As Integer, ByVal UGNDBVendorID As Integer, ByVal PPAPDueDate As String, ByVal PPAPCompletionDate As String, ByVal VendorSignedDate As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PPAPDueDate Is Nothing Then
                PPAPDueDate = ""
            End If

            If PPAPCompletionDate Is Nothing Then
                PPAPCompletionDate = ""
            End If

            If VendorSignedDate Is Nothing Then
                VendorSignedDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertECIVendor(ECINo, UGNDBVendorID, PPAPDueDate, PPAPCompletionDate, VendorSignedDate, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo:" & ECINo _
            & ", UGNDBVendorID:" & UGNDBVendorID _
            & ", PPAPDueDate:" & PPAPDueDate _
            & ", PPAPCompletionDate:" & PPAPCompletionDate _
            & ", VendorSignedDate:" & VendorSignedDate _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update ECIVendor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateECIVendor(ByVal original_RowID As Integer, ByVal UGNDBVendorID As Integer, _
        ByVal PPAPDueDate As String, ByVal PPAPCompletionDate As String, ByVal VendorSignedDate As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PPAPDueDate Is Nothing Then
                PPAPDueDate = ""
            End If

            If PPAPCompletionDate Is Nothing Then
                PPAPCompletionDate = ""
            End If

            If VendorSignedDate Is Nothing Then
                VendorSignedDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateECIVendor(original_RowID, UGNDBVendorID, PPAPDueDate, PPAPCompletionDate, VendorSignedDate, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UGNDBVendorID:" & UGNDBVendorID _
            & ", PPAPDueDate:" & PPAPDueDate _
            & ", PPAPCompletionDate:" & PPAPCompletionDate _
            & ", VendorSignedDate:" & VendorSignedDate _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Delete ECIVendor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteECIVendor(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECIVendor(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECIVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
