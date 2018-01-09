''******************************************************************************************************
''* RFDRSSBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 8/11/2010
''******************************************************************************************************

Imports RFDTableAdapters
<System.ComponentModel.DataObject()> _
Public Class RFDRSSBLL
    Private pAdapter As RFDRSSTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDRSSTableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New RFDRSSTableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDRSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDRSS(ByVal RFDNo As Integer, ByVal RSSID As Integer) As RFD.RFDRSS_MaintDataTable

        Try
            Return Adapter.GetRFDRSS(RFDNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDRSS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDRSSBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetRFDRSS : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDRSSBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetRFDRSS

End Class

