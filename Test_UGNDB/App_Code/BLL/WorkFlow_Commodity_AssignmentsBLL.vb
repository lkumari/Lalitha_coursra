
''******************************************************************************************************
''* WorkFlow_Commodity_AssignmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 08/18/2008
''  Modified: RCarlson 04/17/2012 - Update function changed - fixed error when team member updated row
''******************************************************************************************************

Imports WorkFlowTableAdapters

<System.ComponentModel.DataObject()> _
Public Class WorkFlow_Commodity_AssignmentsBLL
    Private workflowAdapter As WorkFlow_Commodity_Assignments_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As WorkFlowTableAdapters.WorkFlow_Commodity_Assignments_TableAdapter
        Get
            If workflowAdapter Is Nothing Then
                workflowAdapter = New WorkFlow_Commodity_Assignments_TableAdapter()
            End If
            Return workflowAdapter
        End Get
    End Property
    ''*****
    ''* Select WorkFlow_Commodity_Assignments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetWorkFlowCommodityAssignments(ByVal TeamMemberID As Integer) As WorkFlow.WorkFlow_Commodity_AssignmentsDataTable
        Return Adapter.Get_WorkFlow_Commodity_Assignments(TeamMemberID)
    End Function
    ''*****
    ''* Insert New WorkFlow_Commodity_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddWorkFlowCommodityAssignments(ByVal TeamMemberID As Integer, ByVal CommodityID As String) As Boolean

        ' Create a new WorkFlow_Commodity_AssignmentsRow instance
        Dim workflowTable As New WorkFlow.WorkFlow_Commodity_AssignmentsDataTable()
        Dim workflowRow As WorkFlow.WorkFlow_Commodity_AssignmentsRow = workflowTable.NewWorkFlow_Commodity_AssignmentsRow()
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without a null WorkFlow_Commodity_Assignments columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If CommodityID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Commodity is a required field.")
        End If

        ' Insert the new WorkFlow_Commodity_Assignments row
        Dim rowsAffected As Integer = Adapter.sp_Insert_WorkFlow_Commodity_Assignments(TeamMemberID, CommodityID, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update WorkFlow_Commodity_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateWorkFlowCommodityAssignments(ByVal TeamMemberID As Integer, ByVal CommodityID As String, ByVal Original_TeamMemberID As Integer, ByVal Original_CommodityID As String) As Boolean

        'Dim workflowTable As WorkFlow.WorkFlow_Commodity_AssignmentsDataTable = Adapter.Get_WorkFlow_Commodity_Assignments(TeamMemberID)
        'Dim workflowRow As WorkFlow.WorkFlow_Commodity_AssignmentsRow = workflowTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        'If workflowTable.Count = 0 Then
        '    ' no matching record found, return false
        '    Return False
        'End If

        ' Logical Rule - Cannot update a record without a null WorkFlow_Commodity_Assignments columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If CommodityID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Commodity is a required field.")
        End If

        ' Update the WorkFlow_Commodity_Assignments row
        Dim rowsAffected As Integer = Adapter.sp_Update_WorkFlow_Commodity_Assignments(TeamMemberID, CommodityID, User, Original_TeamMemberID, Original_CommodityID)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Delete WorkFlow_Commodity_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteWorkFlowCommodityAssignments(ByVal TeamMemberID As Integer, ByVal CommodityID As String, ByVal SoldTo As Integer, ByVal Original_TeamMemberID As Integer, ByVal Original_CommodityID As String) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_WorkFlow_Commodity_Assignments(Original_TeamMemberID, Original_CommodityID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
End Class




