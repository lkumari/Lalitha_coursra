''******************************************************************************************************
''* CostSheetPreApprovalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/04/2009
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 11/12/2009 - Added RoutingLevel to the update status function
''* Modified: Roderick Carlson 11/17/2009 - Added CostSheetID parameter to update pre-approval-status
''* Modifide: Roderick Carlson 11/20/2009 - Do not allow approval of row per subscription if approved already
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetPreApprovalBLL
    Private CostingPreApprovalAdapter As CostSheetPreApprovalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetPreApprovalTableAdapter
        Get
            If CostingPreApprovalAdapter Is Nothing Then
                CostingPreApprovalAdapter = New CostSheetPreApprovalTableAdapter()
            End If
            Return CostingPreApprovalAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetPreApprovalList returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetPreApprovalList(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, _
    ByVal RoutingLevel As Integer, ByVal SignedStatus As String, ByVal SubscriptionID As Integer, _
    ByVal FilterNotified As Boolean, ByVal isNotified As Boolean, ByVal isHistorical As Boolean) As Costing.CostSheetPreApproval_MaintDataTable

        Try

            If SignedStatus Is Nothing Then
                SignedStatus = ""
            End If

            Return Adapter.GetCostSheetPreApprovalList(CostSheetID, TeamMemberID, RoutingLevel, SignedStatus, SubscriptionID, FilterNotified, isNotified, isHistorical)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & _
            ", TeamMemberID: " & TeamMemberID & ", RoutingLevel: " & RoutingLevel & _
            ", SignedStatus: " & SignedStatus & ", SubscriptionID: " & SubscriptionID & _
            ", FilterNotified: " & FilterNotified & ", isNotified: " & isNotified & ", isHistorical: " & isHistorical & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPreApprovalList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingPreApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostSheetPreApprovalList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPreApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetPreApprovalList
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetPreApprovalItem(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, _
    ByVal RoutingLevel As Integer, ByVal SignedStatus As String, ByVal SubscriptionID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SignedStatus Is Nothing Then
                SignedStatus = "P"
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetPreApprovalItem(CostSheetID, TeamMemberID, RoutingLevel, _
            SignedStatus, SubscriptionID, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & _
            ", TeamMemberID: " & TeamMemberID & ", RoutingLevel: " & RoutingLevel & ", SignedStatus: " & SignedStatus & _
            ", SubscriptionID: " & SubscriptionID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPreApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetPreApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPreApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPreApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingPreApprovalListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetPreApprovalSubscription(ByVal SubscriptionID As Integer, ByVal original_RowID As Integer, _
        ByVal ddTeamMemberName As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetPreApprovalSubscription(original_RowID, SubscriptionID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", SubscriptionID: " & SubscriptionID & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPreApprovalSubscription : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPreApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPreApprovalSubscription : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPreApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingPreApprovalListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetPreApprovalStatus(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, _
        ByVal Comments As String, ByVal SignedStatus As String, ByVal original_RowID As Integer, _
        ByVal RoutingLevel As Integer, ByVal SubscriptionID As Integer, ByVal isCostReduction As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Comments Is Nothing Then
                Comments = ""
            End If

            If SignedStatus = Nothing Or SignedStatus = "" Then
                SignedStatus = "P"
            End If

            Dim rowsAffected As Integer = 0
            Dim ds As DataSet
            ds = CostingModule.GetCostSheetPreApprovalList(CostSheetID, 0, RoutingLevel, "A", SubscriptionID, False, False, False)
            If commonFunctions.CheckDataSet(ds) = False Then
                If SignedStatus = "A" Or SignedStatus = "R" Then
                    If SignedStatus = "R" And Comments = "" Then
                        'do not update rejected records without comments
                    Else
                        ''*****
                        ' Update the record
                        ''*****
                        rowsAffected = Adapter.UpdateCostSheetPreApprovalStatus(original_RowID, CostSheetID, TeamMemberID, SubscriptionID, RoutingLevel, Comments, SignedStatus, UpdatedBy)
                    End If
                End If
            End If

            ' ''*****
            '' Update the record
            ' ''*****
            'Dim rowsAffected As Integer = Adapter.UpdateCostSheetPreApprovalStatus(original_RowID, Comments, SignedStatus, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            'Return rowsAffected = 1
            Return rowsAffected
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", TeamMemberID: " & TeamMemberID _
            & ", Comments: " & Comments & ", SignedStatus: " & SignedStatus & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetPreApprovalStatus : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPreApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetPreApprovalStatus : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPreApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetPreApprovalListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetPreApprovalItem(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetPreApprovalItem(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetPreApprovalItem(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetPreApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPreApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetPreApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPreApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function
End Class
