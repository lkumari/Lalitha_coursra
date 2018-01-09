''******************************************************************************************************
''* CRProjCategoryBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 01/13/2010
''******************************************************************************************************

Imports CostReductionTableAdapters
<System.ComponentModel.DataObject()> _
Public Class CRProjCategoryBLL
    Private pAdapter As CR_Project_Category_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostReductionTableAdapters.CR_Project_Category_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New CR_Project_Category_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select CRProj_Category returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCRProjCategory(ByVal ProjectCategoryName As String) As CostReduction.CR_Project_CategoryDataTable

        Try
            If ProjectCategoryName = Nothing Then ProjectCategoryName = ""

            Return Adapter.Get_CR_Project_Category(ProjectCategoryName)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectCategoryName: " & ProjectCategoryName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRProjCategoryBll.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CRProjectCategory.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Insert New CRProj_Category
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCRProjCategory(ByVal ProjectCategoryName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProjectCategoryName = commonFunctions.convertSpecialChar(ProjectCategoryName, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_CR_Project_Category(ProjectCategoryName, CreatedBy)

            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectCategoryName: " & ProjectCategoryName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRProjCategoryBll.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CRProjectCategory.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Update CRProj_Category
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateCRProjCategory(ByVal ProjectCategoryName As String, ByVal Obsolete As Boolean, ByVal original_PCID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProjectCategoryName = commonFunctions.convertSpecialChar(ProjectCategoryName, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_CR_Project_Category(original_PCID, ProjectCategoryName, Obsolete, UpdatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProjectCategoryName: " & ProjectCategoryName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CRProjCategoryBll.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CR/CRProjectCategory.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCRProjCategory : " & commonFunctions.convertSpecialChar(ex.Message, False), "CRProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function
End Class
