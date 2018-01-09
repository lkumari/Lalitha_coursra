''******************************************************************************************************
''* RFDSupportingDocBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDSupportingDocBLL
    Private RFDSupportingDocAdapter As RFDSupportingDocTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDSupportingDocTableAdapter
        Get
            If RFDSupportingDocAdapter Is Nothing Then
                RFDSupportingDocAdapter = New RFDSupportingDocTableAdapter()
            End If
            Return RFDSupportingDocAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDSupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDSupportingDoc(ByVal RFDNo As Integer) As RFD.RFDSupportingDoc_MaintDataTable

        Try

            Return Adapter.GetRFDSupportingDocList(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDSupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
   
    ''*****
    ''* Delete RFDSupportingDoc
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDSupportingDoc(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDSupportingDoc(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDSupportingDoc(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDSupportingDocBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteRFDSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "RFDSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
