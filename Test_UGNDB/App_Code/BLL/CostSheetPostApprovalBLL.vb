''******************************************************************************************************
''* CostSheetPostApprovalBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/17/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetPostApprovalBLL
    Private CostingPostApprovalAdapter As CostSheetPostApprovalTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetPostApprovalTableAdapter
        Get
            If CostingPostApprovalAdapter Is Nothing Then
                CostingPostApprovalAdapter = New CostSheetPostApprovalTableAdapter()
            End If
            Return CostingPostApprovalAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetPostApprovalList returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetPostApprovalList(ByVal CostSheetID As Integer) As Costing.CostSheetPostApproval_MaintDataTable

        Try

            Return Adapter.GetCostSheetPostApprovalList(CostSheetID, False, False)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetPostApprovalList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingPostApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostSheetPostApprovalList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPostApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetPostApprovalList
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetPostApprovalItem(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetPostApprovalItem(CostSheetID, TeamMemberID, createdBy)

            ' Return true if Postcisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID & _
            ", TeamMemberID: " & TeamMemberID  & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetPostApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetPostApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetPostApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPostApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    
    ''*****
    ''* Delete CostSheetPostApprovalListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCostSheetPostApprovalItem(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetPostApprovalItem(original_RowID, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetPostApprovalItem(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetPostApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetPostApprovalBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetPostApprovalItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetPostApprovalBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function
End Class
