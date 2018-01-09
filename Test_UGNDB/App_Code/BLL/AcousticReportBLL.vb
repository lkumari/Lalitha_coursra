''******************************************************************************************************
''* AcousticReportBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Acoustic_Project_Detail.aspx - gvProjectReport
''* Author  : LRey 05/06/2009
''******************************************************************************************************
Imports AcousticTableAdapters
<System.ComponentModel.DataObject()> _
Public Class AcousticReportBLL
    Private tcAdapter As Acoustic_Project_Report_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As AcousticTableAdapters.Acoustic_Project_Report_TableAdapter
        Get
            If tcAdapter Is Nothing Then
                tcAdapter = New Acoustic_Project_Report_TableAdapter()
            End If
            Return tcAdapter
        End Get
    End Property

    ''*****
    ''* Select Acoustic_Project_Report returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetAcousticProjectReport(ByVal ProjectID As Integer, ByVal ReportID As Integer) As Acoustic.Acoustic_Project_ReportDataTable

        Return Adapter.GetAcousticProjectReport(ProjectID, 0)
    End Function

    ''*****
    ''* Delete Acoustic_Project_Report
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAcousticProjectReport(ByVal ReportID As Integer, ByVal ProjectID As Integer, ByVal original_ReportID As Integer, ByVal original_ProjectID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Acoustic_Project_Report(original_ReportID, original_ProjectID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class

