''******************************************************************************************************
''* DrawingApprovedVendorBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 01/18/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************


Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingApprovedVendorBLL
    Private DrawingApprovedVendorAdapter As DrawingApprovedVendorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingApprovedVendorTableAdapter
        Get
            If DrawingApprovedVendorAdapter Is Nothing Then
                DrawingApprovedVendorAdapter = New DrawingApprovedVendorTableAdapter()
            End If
            Return DrawingApprovedVendorAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingApprovedVendor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingApprovedVendor(ByVal DrawingNo As String) As Drawings.DrawingApprovedVendor_MaintDataTable

        Try

            Return Adapter.GetDrawingApprovedVendor(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ApprovedVendorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "ApprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert DrawingApprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingApprovedVendor(ByVal DrawingNo As String, ByVal UGNDBVendorID As Integer, ByVal SubVendorName As String, _
        ByVal VendorBrand As String, ByVal VendorPartNo As String, ByVal VendorNotes As String, ByVal VendorApprovalDate As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SubVendorName Is Nothing Then
                SubVendorName = ""
            End If

            If VendorBrand Is Nothing Then
                VendorBrand = ""
            End If

            If VendorPartNo Is Nothing Then
                VendorPartNo = ""
            End If

            If VendorNotes Is Nothing Then
                VendorNotes = ""
            End If

            If VendorApprovalDate Is Nothing Then
                VendorApprovalDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertDrawingApprovedVendor(DrawingNo, UGNDBVendorID, SubVendorName, _
            VendorBrand, VendorPartNo, VendorNotes, VendorApprovalDate, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", UGNDBVendorID:" & UGNDBVendorID _
            & ", SubVendorName: " & SubVendorName & ", VendorBrand: " & VendorBrand _
            & ", VendorPartNo: " & VendorPartNo & ", VendorNotes: " & VendorNotes _
            & ", VendorApprovalDate: " & VendorApprovalDate _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingApprovedVendorBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingApprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update DrawingApprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateDrawingApprovedVendor(ByVal UGNDBVendorID As Integer, ByVal SubVendorName As String, _
        ByVal VendorBrand As String, ByVal VendorPartNo As String, ByVal VendorNotes As String, ByVal VendorApprovalDate As String, _
        ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SubVendorName Is Nothing Then
                SubVendorName = ""
            End If

            If VendorBrand Is Nothing Then
                VendorBrand = ""
            End If

            If VendorPartNo Is Nothing Then
                VendorPartNo = ""
            End If

            If VendorNotes Is Nothing Then
                VendorNotes = ""
            End If

            If VendorApprovalDate Is Nothing Then
                VendorApprovalDate = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateDrawingApprovedVendor(original_RowID, UGNDBVendorID, SubVendorName, _
            VendorBrand, VendorPartNo, VendorNotes, VendorApprovalDate, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNDBVendorID: " & UGNDBVendorID _
            & ", SubVendorName: " & SubVendorName & ", VendorBrand: " & VendorBrand _
            & ", VendorPartNo: " & VendorPartNo & ", VendorNotes: " & VendorNotes _
            & ", VendorApprovalDate: " & VendorApprovalDate & ", original_RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingApprovedVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingApprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete DrawingApprovedVendor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingApprovedVendor(ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingApprovedVendor(original_RowID)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) _
            & " :<br/> DrawingApprovedVendorBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DeleteApprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
