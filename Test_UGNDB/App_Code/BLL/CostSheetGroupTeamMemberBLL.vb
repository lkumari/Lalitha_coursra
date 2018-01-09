''******************************************************************************************************
''* CostSheetGroupTeamMemberBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/17/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetGroupTeamMemberBLL
    Private CostingGroupTeamMemberAdapter As CostSheetGroupTeamMemberTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetGroupTeamMemberTableAdapter
        Get
            If CostingGroupTeamMemberAdapter Is Nothing Then
                CostingGroupTeamMemberAdapter = New CostSheetGroupTeamMemberTableAdapter()
            End If
            Return CostingGroupTeamMemberAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetGroupTeamMemberList returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As Costing.CostSheetGroupTeamMember_MaintDataTable

        Try

            Return Adapter.GetCostSheetGroupTeamMember(GroupID, TeamMemberID, SubscriptionID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetGroupTeamMember : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetGroupTeamMemberBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostSheetGroupTeamMemberList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Update New CostSheetGroupTeamMemberList
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostSheetGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer, _
    ByVal SubscriptionID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim updatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetGroupTeamMember(original_RowID, GroupID, TeamMemberID, SubscriptionID, updatedBy)

            ' Return true if Postcisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", GroupID:" & GroupID & ", SubscriptionID:" & SubscriptionID & _
            ", TeamMemberID: " & TeamMemberID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetGroupTeamMemberBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function


    ''*****
    ''* Insert New CostSheetGroupTeamMemberList
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetGroupTeamMember(GroupID, TeamMemberID, SubscriptionID, createdBy)

            ' Return true if Postcisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID:" & GroupID & ", SubscriptionID:" & SubscriptionID & _
            ", TeamMemberID: " & TeamMemberID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetGroupTeamMemberBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Delete CostSheetGroupTeamMemberListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetGroupTeamMember(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetGroupTeamMember(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetGroupTeamMember(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetGroupTeamMemberBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetGroupTeamMemberItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function
End Class
