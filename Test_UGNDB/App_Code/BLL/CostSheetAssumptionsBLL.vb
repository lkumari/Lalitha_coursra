''******************************************************************************************************
''* CostSheetAssumptionsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : 04/24/2014    LREY
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CostSheetAssumptionsBLL
    Private pAdapter1 As Cost_Sheet_AssumptionsTableAdapter = Nothing
    Private pAdapter2 As Cost_Sheet_Assumptions_ApprovalTableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As CostingTableAdapters.Cost_Sheet_AssumptionsTableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New Cost_Sheet_AssumptionsTableAdapter
            End If
            Return pAdapter1
        End Get
    End Property

    Protected ReadOnly Property Adapter2() As CostingTableAdapters.Cost_Sheet_Assumptions_ApprovalTableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New Cost_Sheet_Assumptions_ApprovalTableAdapter
            End If
            Return pAdapter2
        End Get
    End Property

    ''*****
    ''* Select CostSheetAssumptions returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetAssumptions(ByVal CostSheetID As Integer) As Costing.Cost_Sheet_AssumptionsDataTable

        Try

            Return Adapter1.GetCostSheetAssumptions(CostSheetID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetAssumptions: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetAssumptions: " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function ''EOF GetCostSheetAssumptions

    ''*****
    ''* Insert CostSheetAssumptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function InsertCostSheetAssumptions(ByVal CostSheetID As Integer, ByVal Category As String, ByVal Notes As String) As Boolean

        Try

            Dim rowsAffected As Integer = 0
            If Category = Nothing Then Category = ""
            If Notes = Nothing Then Notes = ""


            rowsAffected = Adapter1.sp_Insert_Cost_Sheet_Assumptions(CostSheetID, Category, Notes)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID _
             & ", Category:" & Category _
             & ", Notes:" & Notes

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetAssumptions: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetAssumptions : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function ''EOF InsertCostSheetAssumptions

    ''*****
    ''* Update CostSheetAssumptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostSheetAssumptions(ByVal Category As String, ByVal Notes As String, ByVal original_CostSheetID As Integer, ByVal original_AID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = 0
            If Category = Nothing Then Category = ""
            If Notes = Nothing Then Notes = ""


            rowsAffected = Adapter1.sp_Update_Cost_Sheet_Assumptions(original_CostSheetID, original_AID, Category, Notes)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID _
            & ", AID:" & original_AID _
            & ", Category:" & Category _
            & ", Notes:" & Notes

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetAssumptions: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetAssumptions : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function ''EOF UpdateCostSheetAssumptions

    ''*****
    ''* Delete CostSheetAssumptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function DeleteCostSheetAssumptions(ByVal CostSheetID As Integer, ByVal AID As Integer, ByVal original_CostSheetID As Integer, ByVal original_AID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = 0

            rowsAffected = Adapter1.sp_Delete_Cost_Sheet_Assumptions(original_CostSheetID, original_AID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID _
            & ", AID:" & original_AID

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetAssumptions: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetAssumptions : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF DeleteCostSheetAssumptions

    ''*****
    ''* Select CostSheetAssumptionsApproval returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCostSheetAssumptionsApproval(ByVal CostSheetID As Integer) As Costing.Cost_Sheet_Assumptions_ApprovalDataTable

        Try

            Return Adapter2.GetCostSheetAssumptionsApproval(CostSheetID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID: " & CostSheetID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCostSheetAssumptionsApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCostSheetAssumptionsApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function ''EOF GetCostSheetAssumptionsApproval

    ''*****
    ''* Insert CostSheetAssumptionsApproval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function InsertCostSheetAssumptionsApproval(ByVal CostSheetID As Integer, ByVal TeamMemberID As Integer, ByVal ApprovalDate As String) As Boolean

        Try

            Dim rowsAffected As Integer = 0
            If ApprovalDate = Nothing Then ApprovalDate = ""

            rowsAffected = Adapter2.sp_Insert_Cost_Sheet_Assumptions_Approval(CostSheetID, TeamMemberID, ApprovalDate)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & CostSheetID _
             & ", TeamMemberID:" & TeamMemberID _
             & ", ApprovalDate:" & ApprovalDate

            HttpContext.Current.Session("BLLerror") = "InsertCostSheetAssumptionsApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCostSheetAssumptionsApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function ''EOF InsertCostSheetAssumptionsApproval

    ''*****
    ''* Update CostSheetAssumptionsApproval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCostSheetAssumptionsApproval(ByVal TeamMemberID As Integer, ByVal ApprovalDate As String, ByVal original_CostSheetID As Integer, ByVal original_Department As String) As Boolean

        Try

            Dim rowsAffected As Integer = 0
            If ApprovalDate = Nothing Then ApprovalDate = ""

            rowsAffected = Adapter2.sp_Update_Cost_Sheet_Assumptions_Approval(original_CostSheetID, TeamMemberID, ApprovalDate, original_Department)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", ApprovalDate:" & ApprovalDate _
            & ", Department:" & original_Department

            HttpContext.Current.Session("BLLerror") = "UpdateCostSheetAssumptionsApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCostSheetAssumptionsApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function ''EOF UpdateCostSheetAssumptionsApproval

    ''*****
    ''* Delete CostSheetAssumptionsApproval
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function DeleteCostSheetAssumptionsApproval(ByVal original_CostSheetID As Integer, ByVal original_Department As String) As Boolean

        Try

            Dim rowsAffected As Integer = 0

            rowsAffected = Adapter2.sp_Delete_Cost_Sheet_Assumptions_Approval(original_CostSheetID, original_Department)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CostSheetID:" & original_CostSheetID _
            & ", Department:" & original_Department

            HttpContext.Current.Session("BLLerror") = "DeleteCostSheetAssumptionsApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CostSheetAssumptionsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteCostSheetAssumptionsApproval : " & commonFunctions.convertSpecialChar(ex.Message, False), "CostSheetAssumptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF DeleteCostSheetAssumptionsApproval

End Class
