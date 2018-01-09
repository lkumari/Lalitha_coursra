''******************************************************************************************************
''* CostSheetTopLevelBPCSPartInfoBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 01/21/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetTopLevelBPCSPartInfoBLL
    Private CostSheetTopLevelBPCSPartInfoAdapter As CostSheetTopLevelBPCSPartInfoTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CostSheetTopLevelBPCSPartInfoTableAdapter
        Get
            If CostSheetTopLevelBPCSPartInfoAdapter Is Nothing Then
                CostSheetTopLevelBPCSPartInfoAdapter = New CostSheetTopLevelBPCSPartInfoTableAdapter()
            End If
            Return CostSheetTopLevelBPCSPartInfoAdapter
        End Get
    End Property
    ''*****
    ''* Select CostSheetTopLevelBPCSPartInfo returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetTopLevelBPCSPartInfo(ByVal CostSheetID As Integer) As Costing.CostSheetTopLevelBPCSPartInfo_MaintDataTable

        Try

            Return Adapter.GetCostSheetTopLevelBPCSPartInfo(CostSheetID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingTopLevelBPCSPartInfoBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetTopLevelBPCSPartInfoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CostSheetTopLevelBPCSPartInfo
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostSheetTopLevelBPCSPartInfo(ByVal CostSheetID As Integer, ByVal PartNo As String, ByVal PartName As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCostSheetTopLevelBPCSPartInfo(CostSheetID, PartNo, PartName, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID.ToString & ", PartNo: " & PartNo & ", PartName: " & PartName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetTopLevelBPCSPartInfoBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetTopLevelBPCSPartInfoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update CostingTopLevelBPCSPartInfoBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCostSheetTopLevelBPCSPartInfo(ByVal PartName As String, ByVal original_CostSheetID As Integer, ByVal original_PartNo As String, ByVal PartNo As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCostSheetTopLevelBPCSPartInfo(original_CostSheetID, original_PartNo, PartName, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID.ToString & ", PartNo: " & original_PartNo & ", PartName: " & PartName & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostingTopLevelBPCSPartInfoBLLBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetTopLevelBPCSPartInfoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete CostSheetTopLevelBPCSPartInfoBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function DeleteCostSheetTopLevelBPCSPartInfo(ByVal CostSheetID As Integer, ByVal BPCSPartNo As String, ByVal original_CostSheetID As Integer, ByVal original_BPCSPartNo As String) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            'Dim rowsAffected As Integer = Adapter.DeleteCostSheetTopLevelBPCSPartInfo(original_CostSheetID, original_BPCSPartNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteCostSheetTopLevelBPCSPartInfo(original_CostSheetID, original_BPCSPartNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID.ToString & ", BPCSPartNo:" & original_BPCSPartNo & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetTopLevelBPCSPartInfoBLLBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetTopLevelBPCSPartInfo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetTopLevelBPCSPartInfoBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
