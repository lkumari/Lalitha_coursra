''******************************************************************************************************
''* ARRSSReplyBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 04/1/2010
''******************************************************************************************************

Imports ARTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ARRSSReplyBLL
    Private pAdapter As ARRSSReplyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ARTableAdapters.ARRSSReplyTableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New ARRSSReplyTableAdapter()
            End If
            Return pAdapter
        End Get
    End Property

    ''*****
    ''* Select ARRSSReply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetARRSSReply(ByVal AREID As Integer, ByVal RSSID As Integer) As AR.ARRSSReplyDataTable

        Try
            Return Adapter.GetARRSSReply(AREID, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetARRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ARRSSReplyBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "ARRSSReplyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetARRSSReply
End Class
