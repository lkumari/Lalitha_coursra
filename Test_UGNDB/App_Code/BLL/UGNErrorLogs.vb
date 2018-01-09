''******************************************************************************************************
''* UGNErrorLogsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/15/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports UGNErrorLogsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class UGNErrorLogsBLL
    Private UGNErrorLogsAdapter As UGNErrorLogTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As UGNErrorLogsTableAdapters.UGNErrorLogTableAdapter
        Get
            If UGNErrorLogsAdapter Is Nothing Then
                UGNErrorLogsAdapter = New UGNErrorLogTableAdapter
            End If
            Return UGNErrorLogsAdapter
        End Get
    End Property
    ''*****
    ''* Select UGNErrorLogs returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetErrorLogs(ByVal RowID As Integer, ByVal ErrorMessage As String, ByVal FormName As String, ByVal CreatedBy As String, ByVal StartDate As String, ByVal EndDate As String) As UGNErrorLogs.UGNErrorLog_MaintDataTable

        If ErrorMessage Is Nothing Then
            ErrorMessage = ""
        End If

        If FormName Is Nothing Then
            FormName = ""
        End If

        If CreatedBy Is Nothing Then
            CreatedBy = ""
        End If

        If StartDate Is Nothing Then
            StartDate = ""
        End If

        If EndDate Is Nothing Then
            EndDate = ""
        End If

        Return Adapter.GetUGNErrorLogs(RowID, ErrorMessage, FormName, CreatedBy, StartDate, EndDate)
    End Function
    ''*****
    ''* Insert New Cell
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertErrorLog(ByVal ErrorMessage As String, ByVal FormName As String, ByVal ScreenData As String, ByVal CreatedBy As String) As Boolean

        CreatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ''*****
        ' Insert the Cell record
        ''*****
        Dim rowsAffected As Integer = Adapter.InsertUGNErrorLog(ErrorMessage, FormName, ScreenData, CreatedBy)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update Cell
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateCell(ByVal DepartmentID As Integer, ByVal UGNFacility As String, ByVal CellName As String, ByVal PlannerCode As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_CellID As Integer, ByVal original_DepartmentID As Integer, ByVal original_UGNFacility As String, ByVal original_CellName As String) As Boolean

    '    UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '    ''*****
    '    ' Update the Cell record
    '    ''*****
    '    Dim rowsAffected As Integer = Adapter.UpdateCell(original_CellID, DepartmentID, UGNFacility, CellName, PlannerCode, original_DepartmentID, original_UGNFacility, original_CellName, Obsolete, UpdatedBy)

    '    ' Return true if precisely one row was updated, otherwise false
    '    Return rowsAffected = 1
    'End Function
End Class
