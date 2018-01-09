''******************************************************************************************************
''* CostSheetGroupBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/17/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetGroupBLL
    Private CostingGroupAdapter As CostingTableAdapters.CostSheetGroupTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetGroupTableAdapter
        Get
            If CostingGroupAdapter Is Nothing Then
                CostingGroupAdapter = New CostSheetGroupTableAdapter()
            End If
            Return CostingGroupAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetGroupList returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetGroup(ByVal GroupID As Integer) As Costing.CostSheetGroup_MaintDataTable

        Try

            Return Adapter.GetCostSheetGroup(GroupID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetGroup : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingGroupBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetCostSheetGroupList : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    
    ''*****
    ''* Update New CostSheetGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostSheetGroup(ByVal GroupName As String, ByVal Obsolete As Boolean, ByVal original_GroupID As Integer) As Boolean

        Try
            Dim updatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetGroup(original_GroupID, GroupName, Obsolete, updatedBy)

            ' Return true if Postcisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID:" & original_GroupID & "GroupName:" & GroupName & "Obsolete:" & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetGroup : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetGroupBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetGroup : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Insert New CostSheetGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetGroup(ByVal GroupName As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetGroup(GroupName, Obsolete, createdBy)

            ' Return true if Postcisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupName:" & GroupName & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetGroupItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostsheetGroupBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertCostSheetGroupItem : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
