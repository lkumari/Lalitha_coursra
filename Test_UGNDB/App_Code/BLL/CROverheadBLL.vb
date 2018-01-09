''******************************************************************************************************
''* CROverheadBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 02/17/2010
''* Modified: Roderick Carlson 09/13/2011 - Added budget fields
''* Modified: Roderick Carlson 12/10/2012 - DB Cleanup
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CROverheadBLL
    Private pAdapter As Cost_Reduction_Overhead_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.Cost_Reduction_Overhead_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Cost_Reduction_Overhead_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Cost_Reduction_Overhead returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostReductionOverhead(ByVal ProjectNo As Integer) As CostReduction.Cost_Reduction_OverheadDataTable

        Try
            Return Adapter.Get_Cost_Reduction_Overhead(ProjectNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CROverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CROverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Delete Cost_Reduction_Overhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteCostReductionOverhead(ByVal RowID As Integer, ByVal ProjectNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.Delete_Cost_Reduction_Overhead(original_RowID, ProjectNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & "ProjectNo: " & ProjectNo & ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CROverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CROverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function

    ''*****
    ''* Insert Cost_Reduction_Overhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCostReductionOverhead(ByVal ProjectNo As Integer, ByVal ExpensedName As String, _
        ByVal CurrentCostPerUnit As Double, ByVal CurrentCostPerUnitBudget As Double, _
        ByVal CurrentVolume As Integer, ByVal CurrentVolumeBudget As Integer, ByVal ProposedCostPerUnit As Double, _
        ByVal ProposedVolume As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim rowsAffected As Integer = Adapter.Insert_Cost_Reduction_Overhead(ProjectNo, ExpensedName, _
                                                    CurrentCostPerUnit, CurrentCostPerUnitBudget, CurrentVolume, CurrentVolumeBudget, _
                                                    ProposedCostPerUnit, ProposedVolume, CreatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectNo: " & ProjectNo & ", ExpensedName: " & ExpensedName & _
            ", CurrentCostPerUnit: " & CurrentCostPerUnit & _
            ", CurrentCostPerUnitBudget: " & CurrentCostPerUnitBudget & _
            ", CurrentVolume: " & CurrentVolume & _
            ", CurrentVolumeBudget: " & CurrentVolumeBudget & _
            ", ProposedCostPerUnit: " & ProposedCostPerUnit & _
            ", ProposedVolume: " & ProposedVolume & _
            ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CROverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostReductionOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "CROverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function


    ''*****
    ''* Update Cost_Reduction_Overhead CURRENT
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostReductionOverheadCurrent(ByVal ProjectNo As Integer, ByVal ExpensedName As String, _
        ByVal CurrentCostPerUnit As Double, ByVal CurrentCostPerUnitBudget As Double, _
        ByVal CurrentVolume As Integer, ByVal CurrentVolumeBudget As Integer, _
        ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.Update_Cost_Reduction_Overhead_Current(original_RowID, ProjectNo, ExpensedName, _
                                                    CurrentCostPerUnit, CurrentCostPerUnitBudget, _
                                                    CurrentVolume, CurrentVolumeBudget, UpdatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & ", ProjectNo: " & ProjectNo & ", ExpensedName: " & ExpensedName & _
            ", CurrentCostPerUnit: " & CurrentCostPerUnit & _
            ", CurrentVolume: " & CurrentVolume & _
            ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionOverheadCurrent : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CROverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionOverheadCurrent : " & commonFunctions.convertSpecialChar(ex.Message, False), "CROverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function

    ''*****
    ''* Update Cost_Reduction_Overhead PROPOSED
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostReductionOverheadProposed(ByVal ProjectNo As Integer, ByVal ExpensedName As String, _
    ByVal ProposedCostPerUnit As Double, ByVal ProposedVolume As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.Update_Cost_Reduction_Overhead_Proposed(original_RowID, ProjectNo, ExpensedName, _
            ProposedCostPerUnit, ProposedVolume, UpdatedBy)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & ", ProjectNo: " & ProjectNo & ", ExpensedName: " & ExpensedName & _
            ", ProposedCostPerUnit: " & ProposedCostPerUnit & _
            ", ProposedVolume: " & ProposedVolume & _
            ",  User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCostReductionOverheadProposed : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CROverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CostReduction.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostReductionOverheadProposed : " & commonFunctions.convertSpecialChar(ex.Message, False), "CROverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Function

End Class
