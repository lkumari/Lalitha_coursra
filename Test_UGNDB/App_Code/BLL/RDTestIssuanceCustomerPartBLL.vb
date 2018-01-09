''******************************************************************************************************
''* RDTestIssuanceCustomerPartBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: TestIssuanceDetail.aspx - gvCustomerPart
''* Author  : LRey 02/27/2009
''******************************************************************************************************
Imports RDTestIssuanceTableAdapters

<System.ComponentModel.DataObject()> _
Public Class TestIssuanceCustomerPartBLL
    Private pscpAdapter As TestIssuance_CustomerPart_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As Global.RDTestIssuanceTableAdapters.TestIssuance_CustomerPart_TableAdapter
        Get
            If pscpAdapter Is Nothing Then
                pscpAdapter = New TestIssuance_CustomerPart_TableAdapter()
            End If
            Return pscpAdapter
        End Get
    End Property

    ''*****
    ''* Select TestIssuance_CustomerPartNo returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetTestIssuanceCustomerPart(ByVal RequestID As Integer) As RDTestIssuance.TestIssuance_CustomerPartDataTable
        ' ''Public Function GetTestIssuanceCustomerPart(ByVal RequestID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer, ByVal PartNo As String) As RDTestIssuance.TestIssuance_CustomerPartNoDataTable
        Try

            Return Adapter.Get_TestIssuance_CustomerPart(RequestID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RequestID: " & RequestID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTestIssuanceCustomerPart : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RDTestIssuanceCustomerPartBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RND/TestIssuanceList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTestIssuanceCustomerPart : " & commonFunctions.convertSpecialChar(ex.Message, False), "RDTestIssuanceCustomerPartBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

    ''*****
    ''* Delete TestIssuance_CustomerPartNo
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteTestIssuanceCustomerPart(ByVal RequestID As Integer, ByVal RowID As Integer, ByVal original_RequestID As Integer, ByVal original_RowID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_TestIssuance_CustomerPart(original_RequestID, original_RowID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class

