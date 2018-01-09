''****************************************************************************************************
''* ExpProjDocumentsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 02/08/2011
''****************************************************************************************************

Imports ExpProjTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ExpProjDocumentsBLL
    Private pAdapter As ExpProj_Documents_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ExpProjTableAdapters.ExpProj_Documents_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New ExpProj_Documents_TableAdapter
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select ExpProj_Documents returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjDocuments(ByVal ProjectNo As String) As ExpProj.ExpProj_DocumentsDataTable

        Try
            Return Adapter.Get_ExpProj_Documents(ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjDocumentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjDocuments : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjDocumentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjDocuments
End Class

