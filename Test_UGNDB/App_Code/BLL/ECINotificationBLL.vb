''******************************************************************************************************
''* ECINotificationBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/10/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECINotificationBLL
    Private ECINotificationAdapter As ECINotificationTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECINotificationTableAdapter
        Get
            If ECINotificationAdapter Is Nothing Then
                ECINotificationAdapter = New ECINotificationTableAdapter()
            End If
            Return ECINotificationAdapter
        End Get
    End Property
    ''*****
    ''* Select ECINotification returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECINotification(ByVal ECINo As Integer) As ECI.ECINotification_MaintDataTable

        Try

            Return Adapter.GetECINotification(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECINotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECINotificationBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECINotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECINotificationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
   
End Class
