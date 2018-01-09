''******************************************************************************************************
''* RFDRSSReplyBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 08/11/2010
''******************************************************************************************************

Imports RFDTableAdapters
<System.ComponentModel.DataObject()> _
Public Class RFDRSSReplyBLL
    Private pAdapter As RFDRSSReplyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDRSSReplyTableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New RFDRSSReplyTableAdapter()
            End If
            Return pAdapter
        End Get
    End Property

    ''*****
    ''* Select RFDRSSReply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDRSSReply(ByVal RFDNo As Integer, ByVal RSSID As Integer) As RFD.RFDRSSReply_MaintDataTable

        Try
            Return Adapter.GetRFDRSSReply(RFDNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRFDRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDRSSReplyBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetRFDRSSReply : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDRSSReplyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetRFDRSSReply
End Class
