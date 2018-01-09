''******************************************************************************************************
''* RFDNetworkFilesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 11/11/2010
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDNetworkFilesBLL
    Private RFDNetworkFilesAdapter As RFDNetworkFilesTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDNetworkFilesTableAdapter
        Get
            If RFDNetworkFilesAdapter Is Nothing Then
                RFDNetworkFilesAdapter = New RFDNetworkFilesTableAdapter()
            End If
            Return RFDNetworkFilesAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDNetworkFiles returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDNetworkFiles(ByVal RFDNo As Integer) As RFD.RFDNetworkFiles_MaintDataTable

        Try

            Return Adapter.GetRFDNetworkFileList(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDNetworkFileList : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDNetworkFilesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDNetworkFileList : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDNetworkFilesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Delete RFDNetworkFiles
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDNetworkFile(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDNetworkFile(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDNetworkFile(original_RowID, RFDNo)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDNetworkFile : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDNetworkFilesBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteRFDNetworkFile : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "RFDNetworkFilesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
