
''******************************************************************************************************
''* WorkFlow_Family_Purchasing_AssignmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/14/2009
''  Modified: Roderick Carlson 11/11/2010 - cleaned up unused code
''******************************************************************************************************

Imports WorkFlowTableAdapters

<System.ComponentModel.DataObject()> _
Public Class WorkFlow_Family_Purchasing_AssignmentsBLL
    Private workflowAdapter As WorkFlow_Family_Purchasing_Assignments_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As WorkFlowTableAdapters.WorkFlow_Family_Purchasing_Assignments_TableAdapter
        Get
            If workflowAdapter Is Nothing Then
                workflowAdapter = New WorkFlow_Family_Purchasing_Assignments_TableAdapter()
            End If
            Return workflowAdapter
        End Get
    End Property
    ''*****
    ''* Select WorkFlow_Family_Purchasing_Assignments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetWorkFlowFamily_PurchasingAssignments(ByVal FamilyID As Integer, ByVal TeamMemberID As Integer) As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsDataTable
        Return Adapter.GetWorkflowFamilyPurchasingAssignments(FamilyID, TeamMemberID)
    End Function
    ''*****
    ''* Insert New WorkFlow_Family_Purchasing_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertWorkFlowFamilyPurchasingAssignments(ByVal TeamMemberID As Integer, ByVal FamilyID As String) As Boolean
        Try
            ' Create a new WorkFlow_Family_Purchasing_AssignmentsRow instance
            'Dim workflowTable As New WorkFlow.WorkFlow_Family_Purchasing_AssignmentsDataTable()
            'Dim workflowRow As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsRow = workflowTable.NewWorkFlow_Family_Purchasing_AssignmentsRow()
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without a null WorkFlow_Family_Purchasing_Assignments columns
            If TeamMemberID = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
            End If
            If FamilyID = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Family is a required field.")
            End If

            ' Insert the new WorkFlow_Family_Purchasing_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Insert_WorkFlow_Family_Purchasing_Assignments(TeamMemberID, FamilyID, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & "FamilyID: " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Family_Purchasing_AssignmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Team_Member_Family_Purchasing.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Family_Purchasing_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertWorkFlowFamily_PurchasingAssignments
    ''*****
    ''* Update WorkFlow_Family_Purchasing_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateWorkFlowFamilyPurchasingAssignments(ByVal TeamMemberID As Integer, ByVal FamilyID As String, ByVal Original_TeamMemberID As Integer, ByVal Original_FamilyID As String) As Boolean

        Try

            'Dim workflowTable As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsDataTable = Adapter.GetWorkflowFamilyPurchasingAssignments(FamilyID, TeamMemberID)
            'Dim workflowRow As WorkFlow.WorkFlow_Family_Purchasing_AssignmentsRow = workflowTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'If workflowTable.Count = 0 Then
            '    ' no matching record found, return false
            '    Return False
            'End If

            ' Logical Rule - Cannot update a record without a null WorkFlow_Family_Purchasing_Assignments columns
            If TeamMemberID = Nothing Then
                Throw New ApplicationException("Update Cancelled: Team Member is a required field.")
            End If
            If FamilyID = Nothing Then
                Throw New ApplicationException("Update Cancelled: Family is a required field.")
            End If

            ' Update the WorkFlow_Family_Purchasing_Assignments row
            Dim rowsAffected As Integer = Adapter.sp_Update_WorkFlow_Family_Purchasing_Assignments(TeamMemberID, FamilyID, User, Original_TeamMemberID, Original_FamilyID)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & "FamilyID: " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Family_Purchasing_AssignmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Team_Member_Family_Purchasing.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Family_Purchasing_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateWorkFlowFamilyPurchasingAssignments
    ''*****
    ''* Delete WorkFlow_Family_Purchasing_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteWorkFlowFamilyPurchasingAssignments(ByVal TeamMemberID As Integer, ByVal FamilyID As String, ByVal SoldTo As Integer, ByVal Original_TeamMemberID As Integer, ByVal Original_FamilyID As String) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.sp_Delete_WorkFlow_Family_Purchasing_Assignments(Original_TeamMemberID, Original_FamilyID)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & "FamilyID: " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Family_Purchasing_AssignmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Team_Member_Family_Purchasing.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Family_Purchasing_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function
End Class




