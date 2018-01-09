
''******************************************************************************************************
''* DrawingMaterialSpecRelateBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 07/26/2011
''            Roderick Carlson 08/26/2011 - Delete function only needs RowID

''******************************************************************************************************
Imports Microsoft.VisualBasic
Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingMaterialSpecRelateBLL
    Private DrawingMaterialSpecRelateByMaterialSpecNoAdapter As DrawingMaterialSpecRelateByMaterialSpecNoTableAdapter

    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingMaterialSpecRelateByMaterialSpecNoTableAdapter
        Get
            If DrawingMaterialSpecRelateByMaterialSpecNoAdapter Is Nothing Then
                DrawingMaterialSpecRelateByMaterialSpecNoAdapter = New DrawingMaterialSpecRelateByMaterialSpecNoTableAdapter
            End If
            Return DrawingMaterialSpecRelateByMaterialSpecNoAdapter
        End Get
    End Property

    ''*****
    ''* Select SubDrawings returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingMaterialSpecRelateByMaterialSpecNo(ByVal MaterialSpecNo As String) As Drawings.DrawingMaterialSpecRelateByMaterialSpecNo_MaintDataTable

        Try
            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            Return Adapter.GetDrawingMaterialSpecRelateByMaterialSpecNo(MaterialSpecNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecRelateByMaterialSpecNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingMaterialSpecRelateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecRelateByMaterialSpecNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecRelateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Insert DrawingMaterialSpecRelate
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingMaterialSpecRelate(ByVal MaterialSpecNo As String, ByVal DrawingNo As String, _
        ByVal DrawingMaterialSpecNotes As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If DrawingMaterialSpecNotes Is Nothing Then
                DrawingMaterialSpecNotes = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertDrawingMaterialSpecRelate(MaterialSpecNo, DrawingNo, DrawingMaterialSpecNotes, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", DrawingNo:" & DrawingNo _
            & ", DrawingMaterialSpecNotes: " & DrawingMaterialSpecNotes _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingMaterialSpecRelate : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingMaterialSpecRelateBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingMaterialSpecRelate : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecRelateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Update DrawingMaterialSpecRelate
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateDrawingMaterialSpecRelate(ByVal original_RowID As Integer, ByVal MaterialSpecNo As String, _
        ByVal DrawingNo As String, ByVal DrawingMaterialSpecNotes As String, ByVal RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If DrawingMaterialSpecNotes Is Nothing Then
                DrawingMaterialSpecNotes = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateDrawingMaterialSpecRelate(original_RowID, MaterialSpecNo, DrawingNo, DrawingMaterialSpecNotes, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID: " & original_RowID _
            & ", MaterialSpecNo:" & MaterialSpecNo _
            & ", DrawingNo: " & DrawingNo _
            & ", DrawingMaterialSpecNotes: " _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingMaterialSpecRelate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingMaterialSpecRelateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingMaterialSpecRelate : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecRelateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Delete DrawingMaterialSpecRelate
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingMaterialSpecRelate(ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingMaterialSpecRelate(original_RowID)

            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawingMaterialSpecRelate : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) _
            & " :<br/> DrawingMaterialSpecRelateBLL.vb :<br/> " _
            & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteDrawingMaterialSpecRelate : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecRelateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
