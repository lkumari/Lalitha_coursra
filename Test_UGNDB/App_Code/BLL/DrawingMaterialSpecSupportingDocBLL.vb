''******************************************************************************************************
''* DrawingMaterialSpecSupportingDocBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 03/04/2011
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingMaterialSpecSupportingDocBLL
    Private DrawingMaterialSpecSupportingDocAdapter As DrawingMaterialSpecSupportingDocTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingMaterialSpecSupportingDocTableAdapter
        Get
            If DrawingMaterialSpecSupportingDocAdapter Is Nothing Then
                DrawingMaterialSpecSupportingDocAdapter = New DrawingMaterialSpecSupportingDocTableAdapter()
            End If
            Return DrawingMaterialSpecSupportingDocAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingMaterialSpecSupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingMaterialSpecSupportingDoc(ByVal MaterialSpecNo As String) As Drawings.DrawingMaterialSpecSupportingDoc_MaintDataTable

        Try

            Return Adapter.GetDrawingMaterialSpecSupportingDocList(MaterialSpecNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingMaterialSpecSupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Delete DrawingMaterialSpecSupportingDoc
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteDrawingMaterialSpecSupportingDoc(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingMaterialSpecSupportingDoc(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawingMaterialSpecSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingMaterialSpecSupportingDocBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecList.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteDrawingMaterialSpecSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "DrawingMaterialSpecSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
