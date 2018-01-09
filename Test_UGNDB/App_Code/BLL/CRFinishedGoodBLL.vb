''******************************************************************************************************
''* CRFinishedGoodBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 02/15/2010
''* Modified: Roderick Carlson 12/10/2012 - DB Cleanup
''* Modified: 03/01/2014 LRey   Replaced "BPCS Part No" to "Part No" wherever used. 
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRFinishedGoodBLL
    Private pAdapter As Cost_Reduction_Finished_Good_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Finished_Good_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Finished_Good_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_FinishedGood returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionFinishedGood(ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_Finished_GoodDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Finished_Good(ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRFinishedGoodBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRFinishedGoodBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert Finished Good
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostReductionFinishedGood(ByVal ProjectNo As Integer, ByVal PartNo As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PartNo Is Nothing Then PartNo = ""

            Dim rowsAffected As Integer = Adapter.Insert_Cost_Reduction_Finished_Good(ProjectNo, PartNo, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", PartNo:" & PartNo _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertReductionFinishedGood : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRFinishedGoodBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertReductionFinishedGood : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "CRFinishedGoodBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update Cost Reduction Finished Good
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostReductionFinishedGood(ByVal ProjectNo As Integer, ByVal PartNo As String, ByVal CustomerPartNo As String, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim ds As DataSet
            Dim rowsAffected As Integer = 0
            Dim bContinueUpdate As Boolean = True

            If PartNo Is Nothing Then PartNo = ""

            '' CHECK TO SEE IF THE PARTNO IS VALID BEFORE UPDATING
            If PartNo <> "" Then
                ds = commonFunctions.GetBPCSPartNo(PartNo, "")

                If commonFunctions.CheckDataSet(ds) = True Then

                    If bContinueUpdate = True Then
                        rowsAffected = Adapter.Update_Cost_Reduction_Finished_Good(original_RowID, ProjectNo, PartNo, UpdatedBy)
                    End If
                End If
            End If

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo _
            & ", original_RowID: " & original_RowID & ", original_ProjectNo: " & ProjectNo _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingApprovedVendorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingApprovedVendorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete Cost_Reduction_FinishedGood
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionFinishedGood(ByVal RowID As Integer, ByVal ProjectNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.Delete_Cost_Reduction_Finished_Good(original_RowID, ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", ProjectNo: " & ProjectNo & ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRFinishedGoodBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReductionList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionFinishedGood : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRFinishedGoodBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function
End Class
