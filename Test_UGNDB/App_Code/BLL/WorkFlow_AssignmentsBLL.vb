
''******************************************************************************************************
''* WorkFlow_AssignmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 05/28/2008
''* Modified: LRey 08/18/2008 Added SoldTo to Get, Insert, Update and Delete stored procedures.
''******************************************************************************************************

Imports WorkFlowTableAdapters

<System.ComponentModel.DataObject()> _
Public Class WorkFlow_AssignmentsBLL
    Private workflowAdapter As WorkFlow_Assignments_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As WorkFlowTableAdapters.WorkFlow_Assignments_TableAdapter
        Get
            If workflowAdapter Is Nothing Then
                workflowAdapter = New WorkFlow_Assignments_TableAdapter()
            End If
            Return workflowAdapter
        End Get
    End Property
    ''*****
    ''* Select WorkFlow_Assignments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetWorkFlowAssignments(ByVal TeamMemberID As Integer) As WorkFlow.WorkFlow_AssignmentsDataTable
        Return Adapter.GetWorkFlowAssignments(TeamMemberID)
    End Function
    ''*****
    ''* Insert New WorkFlow_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddWorkFlowAssignments(ByVal TeamMemberID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer) As Boolean

        ' Create a new WorkFlow_AssignmentsRow instance
        Dim workflowTable As New WorkFlow.WorkFlow_AssignmentsDataTable()
        Dim workflowRow As WorkFlow.WorkFlow_AssignmentsRow = workflowTable.NewWorkFlow_AssignmentsRow()
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without a null Workflow_Assignments columns

        If CABBV = Nothing Then CABBV = ""

        ' Insert the new Workflow_Assignments row
        Dim rowsAffected As Integer = Adapter.sp_Insert_WorkFlow_Assignments(TeamMemberID, CABBV, SoldTo, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update WorkFlow_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateWorkFlowAssignments(ByVal Original_TeamMemberID As Integer, ByVal Original_CABBV As String, ByVal Original_SoldTo As Integer, ByVal ddCustomerValue As String, ByVal TeamMemberID As Integer) As Boolean

        Dim workflowTable As WorkFlow.WorkFlow_AssignmentsDataTable = Adapter.GetWorkFlowAssignments(TeamMemberID)
        Dim workflowRow As WorkFlow.WorkFlow_AssignmentsRow = workflowTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        If workflowTable.Count = 0 Then
            ' no matching record found, return false
            Return False
        End If

        ' Logical Rule - Cannot update a record without a null WorkFlow_Assignments columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        Dim Pos As Integer = InStr(ddCustomerValue, "|")
        Dim CABBV As String = Nothing
        Dim SoldTo As Integer = Nothing
        If Not (Pos = 0) Then
            CABBV = Microsoft.VisualBasic.Right(ddCustomerValue, Len(ddCustomerValue) - Pos)
            SoldTo = Microsoft.VisualBasic.Left(ddCustomerValue, Pos - 1)
        End If

        If CABBV = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Customer is a required field.")
        End If
        If SoldTo = Nothing Then
            Throw New ApplicationException("Insert Cancelled: SoldTo is a required field.")
        End If

        ' Update the WorkFlow_Assignments row
        Dim rowsAffected As Integer = Adapter.sp_Update_WorkFlow_Assignments(TeamMemberID, CABBV, SoldTo, User, Original_TeamMemberID, Original_CABBV, Original_SoldTo)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Delete WorkFlow_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteWorkFlowAssignments(ByVal TeamMemberID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal Original_TeamMemberID As Integer, ByVal Original_CABBV As String, ByVal Original_SoldTo As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_WorkFlow_Assignments(Original_TeamMemberID, Original_CABBV, Original_SoldTo)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
End Class




