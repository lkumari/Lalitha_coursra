
''******************************************************************************************************
''* WorkFlow_Make_AssignmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 11/29/2010
''******************************************************************************************************

Imports WorkFlowTableAdapters

<System.ComponentModel.DataObject()> _
Public Class WorkFlowMakeAssignmentsBLL
    Private workflowAdapter As WorkFlow_Make_Assignments_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As WorkFlowTableAdapters.WorkFlow_Make_Assignments_TableAdapter
        Get
            If workflowAdapter Is Nothing Then
                workflowAdapter = New WorkFlow_Make_Assignments_TableAdapter()
            End If
            Return workflowAdapter
        End Get
    End Property
    ''*****
    ''* Select WorkFlow_Make_Assignments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetWorkFlowMakeAssignments(ByVal Make As String, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As WorkFlow.WorkFlow_Make_AssignmentsDataTable

        Try
            If Make Is Nothing Then
                Make = ""
            End If

            Return Adapter.GetWorkflowMakeAssignments(Make, TeamMemberID, SubscriptionID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Make: " & Make _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Make_AssignmentsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Make_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New WorkFlow_Make_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertWorkFlowMakeAssignments(ByVal TeamMemberID As Integer, ByVal Make As String) As Boolean

        Try
            ' Create a new WorkFlow_Make_AssignmentsRow instance
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Make Is Nothing Then
                Make = ""
            End If

            ' Insert the new WorkFlow_Make_Purchasing_Assignments row
            Dim rowsAffected As Integer = Adapter.InsertWorkFlowMakeAssignments(TeamMemberID, Make, CreatedBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Make: " & Make _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Make_AssignmentsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Make_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Update WorkFlow_Make_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateWorkFlowMakeAssignments(ByVal TeamMemberID As Integer, ByVal Make As String, ByVal Original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Make Is Nothing Then
                Make = ""
            End If

            ' Update the WorkFlow_Make_Purchasing_Assignments row
            Dim rowsAffected As Integer = Adapter.UpdateWorkFlowMakeAssignments(Original_RowID, TeamMemberID, Make, UpdatedBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & Original_RowID _
            & ", Make: " & Make _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Make_AssignmentsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Make_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Delete WorkFlow_Make_Assignments
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteWorkFlowMakeAssignments(ByVal RowID As Integer, ByVal Original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteWorkFlowMakeAssignments(Original_RowID, UpdatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> WorkFlow_Make_AssignmentsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False), "WorkFlow_Make_AssignmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
End Class




