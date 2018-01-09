''******************************************************************************************************
''* ECIBPCSParentPartsAffectedBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/01/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIBPCSParentPartsAffectedBLL
    Private ECIBPCSParentPartsAffectedAdapter As ECIBPCSParentPartsAffectedTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIBPCSParentPartsAffectedTableAdapter
        Get
            If ECIBPCSParentPartsAffectedAdapter Is Nothing Then
                ECIBPCSParentPartsAffectedAdapter = New ECIBPCSParentPartsAffectedTableAdapter()
            End If
            Return ECIBPCSParentPartsAffectedAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIBPCSParentPartsAffected returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIBPCSParentPartsAffected(ByVal ECINo As Integer) As ECI.ECIBPCSParentPartsAffected_MaintDataTable

        Try

            Return Adapter.GetECIBPCSParentPartsAffected(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIBPCSParentPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIBPCSParentPartsAffectedBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIBPCSParentPartsAffected : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIBPCSParentPartsAffectedBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

End Class
