''******************************************************************************************************
''* AREventSupportingDocBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 07/20/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************
Imports Microsoft.VisualBasic

Imports ARTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AREventSupportingDocBLL

    Private AREventSupportingDocTableAdapter As AREventSupportingDocTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ARTableAdapters.AREventSupportingDocTableAdapter
        Get
            If AREventSupportingDocTableAdapter Is Nothing Then
                AREventSupportingDocTableAdapter = New AREventSupportingDocTableAdapter
            End If
            Return AREventSupportingDocTableAdapter
        End Get
    End Property
    ''*****
    ''* Select AREventSupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAREventSupportingDoc(ByVal AREID As Integer) As AR.AREventSupportingDocDataTable

        Try

            Return Adapter.GetAREventSupportingDoc(AREID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventSupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Delete AREventSupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAREventSupportingDoc(ByVal AREID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Return Adapter.DeleteAREventSupportingDoc(original_RowID, AREID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & ", AREID: " & AREID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventSupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREventSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
End Class
