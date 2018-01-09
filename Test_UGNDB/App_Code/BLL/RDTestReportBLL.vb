''******************************************************************************************************
''* RDTestReportBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: TestIssuanceDetail.aspx - gvTestReport
''* Author  : LRey 04/02/2009
''******************************************************************************************************
Imports RDTestIssuanceTableAdapters
<System.ComponentModel.DataObject()> _
Public Class RDTestReportBLL
    Private tcAdapter As TestIssuance_TestReport_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As RDTestIssuanceTableAdapters.TestIssuance_TestReport_TableAdapter
        Get
            If tcAdapter Is Nothing Then
                tcAdapter = New TestIssuance_TestReport_TableAdapter()
            End If
            Return tcAdapter
        End Get
    End Property

    ''*****
    ''* Select TestIssuance_TestReport returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetTestIssuanceTestReport(ByVal RequestID As Integer, ByVal TestReportID As Integer) As RDTestIssuance.TestIssuance_TestReportDataTable

        Return Adapter.Get_TestIssuance_TestReport(RequestID, 0)
    End Function

    ''*****
    ''* Delete TestIssuance_TestReport
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteTestIssuanceTestReport(ByVal TestReportID As Integer, ByVal RequestID As Integer, ByVal original_TestReportID As Integer, ByVal original_RequestID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_TestIssuance_TestReport(original_TestReportID, original_RequestID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class
