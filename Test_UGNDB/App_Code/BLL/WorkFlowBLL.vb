''******************************************************************************************************
''* WorkFlowBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 05/28/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports WorkFlowTableAdapters

<System.ComponentModel.DataObject()> _
Public Class WorkFlowBLL
    Private workflowAdapter As WorkFlow_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As WorkFlowTableAdapters.WorkFlow_TableAdapter
        Get
            If workflowAdapter Is Nothing Then
                workflowAdapter = New WorkFlow_TableAdapter()
            End If
            Return workflowAdapter
        End Get
    End Property
    ''*****
    ''* Select WorkFlow returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetWorkFlow(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As WorkFlow.WorkFlowDataTable
        Return Adapter.GetWorkFlow(TeamMemberID, SubscriptionID)
    End Function
    ''*****
    ''* Insert New WorkFlow
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddWorkFlow(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer, ByVal BackupTeamMemberID As Integer, ByVal DeptInChargeTMID As Integer) As Boolean

        ' Create a new WorkFlowRow instance
        Dim workflowTable As New WorkFlow.WorkFlowDataTable()
        Dim workflowRow As WorkFlow.WorkFlowRow = workflowTable.NewWorkFlowRow()
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without a null Workflow columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If SubscriptionID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Subscription is a required field.")
        End If
        If BackupTeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Backup Team Member is a required field.")
        End If
        If DeptInChargeTMID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Department in Charge is a required field.")
        End If

        ' Insert the new Workflow row
        Dim rowsAffected As Integer = Adapter.sp_Insert_WorkFlow(TeamMemberID, SubscriptionID, BackupTeamMemberID, DeptInChargeTMID, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update WorkFlow
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateWorkFlow(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer, ByVal BackupTeamMemberID As Integer, ByVal DeptInChargeTMID As Integer, ByVal original_TeamMemberID As Integer, ByVal original_SubscriptionID As Integer) As Boolean

        Dim workflowTable As WorkFlow.WorkFlowDataTable = Adapter.GetWorkFlow(original_TeamMemberID, original_SubscriptionID)
        Dim workflowRow As WorkFlow.WorkFlowRow = workflowTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        If workflowTable.Count = 0 Then
            ' no matching record found, return false
            Return False
        End If

        ' Logical Rule - Cannot update a record without a null WorkFlow column
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If SubscriptionID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Subscription is a required field.")
        End If
        If BackupTeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Backup Team Member is a required field.")
        End If
        If DeptInChargeTMID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Department in Charge is a required field.")
        End If

        ' Update the Workflow row
        Dim rowsAffected As Integer = Adapter.sp_Update_WorkFlow(TeamMemberID, SubscriptionID, BackupTeamMemberID, DeptInChargeTMID, original_TeamMemberID, original_SubscriptionID, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Delete WorkFlow
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteWorkFlow(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer, ByVal original_TeamMemberID As Integer, ByVal original_SubscriptionID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_WorkFlow(original_TeamMemberID, original_SubscriptionID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
End Class



