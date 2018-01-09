''******************************************************************************************************
''* DrawingUnapprovedVendorBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 01/18/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************


Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingUnapprovedVendorBLL
    Private DrawingUnapprovedVendorAdapter As DrawingUnapprovedVendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingUnapprovedVendorTableAdapter
        Get
            If DrawingUnapprovedVendorAdapter Is Nothing Then
                DrawingUnapprovedVendorAdapter = New DrawingUnapprovedVendorTableAdapter()
            End If
            Return DrawingUnapprovedVendorAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingUnapprovedVendor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingUnapprovedVendor(ByVal DrawingNo As String) As Drawings.DrawingUnapprovedVendor_MaintDataTable

        Try

            Return Adapter.GetDrawingUnapprovedVendor(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UnapprovedVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "UnapprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert DrawingUnapprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingUnapprovedVendor(ByVal DrawingNo As String, ByVal VendorName As String, ByVal VendorNotes As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If VendorName Is Nothing Then
                VendorName = ""
            End If

            If VendorNotes Is Nothing Then
                VendorNotes = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertDrawingUnapprovedVendor(DrawingNo, VendorName, VendorNotes, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", VendorName:" & VendorName _
            & ", VendorNotes:" & VendorNotes _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingUnapprovedVendorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingUnapprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update DrawingUnapprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateDrawingUnapprovedVendor(ByVal VendorName As String, ByVal VendorNotes As String, _
        ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If VendorName Is Nothing Then
                VendorName = ""
            End If

            If VendorNotes Is Nothing Then
                VendorNotes = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateDrawingUnapprovedVendor(original_RowID, VendorName, VendorNotes, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "VendorName: " & VendorName _
            & ", VendorNotes: " & VendorNotes _
            & ", original_RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingUnapprovedVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingUnapprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete DrawingUnapprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingUnapprovedVendor(ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingUnapprovedVendor(original_RowID)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawingUnapprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) _
            & " :<br/> DrawingUnapprovedVendorBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteDrawingUnapprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DeleteUnapprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
