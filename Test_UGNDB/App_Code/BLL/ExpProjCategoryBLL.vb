''******************************************************************************************************
''* ExpProjCategoryBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LRey 08/27/2009
''******************************************************************************************************

Imports ExpProjTableAdapters
<System.ComponentModel.DataObject()> _
Public Class ExpProjCategoryBLL
    Private pAdapter As ExpProj_Category_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ExpProjTableAdapters.ExpProj_Category_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New ExpProj_Category_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select ExpProj_Category returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetExpProjCategory(ByVal CategoryName As String) As ExpProj.ExpProj_CategoryDataTable

        Try
            Return Adapter.Get_ExpProj_Category(CategoryName)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CategoryName: " & CategoryName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCategoryBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Category.aspx"

            UGNErrorTrapping.InsertErrorLog("GetExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetExpProjCategory

    ''*****
    ''* Insert New ExpProj_Category
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertExpProjCategory(ByVal CategoryName As String, ByVal GLNo As Integer, ByVal UsefulLife As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            CategoryName = commonFunctions.replaceSpecialChar(CategoryName, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_ExpProj_Category(CategoryName, GLNo, UsefulLife, CreatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CategoryName: " & CategoryName & ", GLNo: " & GLNo & ", UsefulLife: " & UsefulLife & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCategoryBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Category.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertExpProjCategory

    ''*****
    ''* Update ExpProj_Category
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateExpProjCategory(ByVal CategoryName As String, ByVal GLNo As Integer, ByVal UsefulLife As Integer, ByVal Obsolete As Boolean, ByVal original_CategoryID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            CategoryName = commonFunctions.replaceSpecialChar(CategoryName, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_ExpProj_Category(original_CategoryID, CategoryName, GLNo, UsefulLife, Obsolete, UpdatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CategoryName: " & CategoryName & ", GLNo: " & GLNo & ", UsefulLife: " & UsefulLife & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> ExpProjCategoryBll.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/EXP/ExpProj_Category.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateExpProjCategory : " & commonFunctions.replaceSpecialChar(ex.Message, False), "ExpProjCategoryBll.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function 'EOF UpdateExpProjCategory
End Class
