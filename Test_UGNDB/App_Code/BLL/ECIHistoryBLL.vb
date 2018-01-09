''******************************************************************************************************
''* ECIHistoryBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 11/17/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIHistoryBLL
    Private ECIHistoryAdapter As ECIHistoryTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIHistoryTableAdapter
        Get
            If ECIHistoryAdapter Is Nothing Then
                ECIHistoryAdapter = New ECIHistoryTableAdapter()
            End If
            Return ECIHistoryAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIHistory returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIHistory(ByVal ECINo As Integer) As ECI.ECIHistory_MaintDataTable

        Try

            Return Adapter.GetECIHistory(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIHistory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIHistoryBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIHistory : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIHistoryBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

End Class