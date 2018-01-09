''******************************************************************************************************
''* RDTestIssuanceAssignmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: TestIssuanceDetail.aspx - gvTMAssignments
''* Author  : LRey 03/13/2009
''******************************************************************************************************
Imports RDTestIssuanceTableAdapters

<System.ComponentModel.DataObject()> _
Public Class TestIssuanceAssignmentsBLL
    Private rdAdapter As TestIssuance_Assignments_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As RDTestIssuanceTableAdapters.TestIssuance_Assignments_TableAdapter
        Get
            If rdAdapter Is Nothing Then
                rdAdapter = New TestIssuance_Assignments_TableAdapter()
            End If
            Return rdAdapter
        End Get
    End Property


    ''*****
    ''* Select TestIssuance_Assignments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
         Public Function GetTestIssuanceAssignments(ByVal RequestID As Integer) As RDTestIssuance.TestIssuance_AssignmentsDataTable
        'If RequestID = 0 And HttpContext.Current.Request.QueryString("pReqID") <> Nothing Then
        '    RequestID = HttpContext.Current.Request.QueryString("pReqID")
        'End If
        Return Adapter.GetData_TestIssuanceAssignments(RequestID)
    End Function

    ''*****
    ''* Insert a New row to TestIssuance_Assignments table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTestIssuanceAssignments(ByVal RequestID As Integer, ByVal TeamMemberID As Integer) As Boolean

        ' Create a new pscpRow instance
        Dim pscpTable As New RDTestIssuance.TestIssuance_AssignmentsDataTable
        Dim pscpRow As RDTestIssuance.TestIssuance_AssignmentsRow = pscpTable.NewTestIssuance_AssignmentsRow
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without null columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If

        ' Insert the new TestIssuance_Assignments row
        Dim rowsAffected As Integer = Adapter.sp_Insert_Test_Issuance_Assignments(RequestID, TeamMemberID, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function

    ''*****
    ''* Delete TestIssuance_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteTestIssuanceAssignments(ByVal RequestID As Integer, ByVal TeamMemberID As Integer, ByVal original_RequestID As Integer, ByVal original_TeamMemberID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Test_Issuance_Assignments(original_RequestID, original_TeamMemberID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class

