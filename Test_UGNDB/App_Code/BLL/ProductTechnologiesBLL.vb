''******************************************************************************************************
''* ProductTechnologiesBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 04/16/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************


Imports ProductTechnologiesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ProductTechnologiesBLL
    Private ProductTechnologyAdapter As ProductTechnologyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ProductTechnologiesTableAdapters.ProductTechnologyTableAdapter
        Get
            If ProductTechnologyAdapter Is Nothing Then
                ProductTechnologyAdapter = New ProductTechnologyTableAdapter()
            End If
            Return ProductTechnologyAdapter
        End Get
    End Property
    ''*****
    ''* Select ProductTechnologies returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetProductTechnologies(ByVal ProductTechnologyName As String) As ProductTechnologies.ProductTechnology_MaintDataTable

        Try
            If ProductTechnologyName Is Nothing Then ProductTechnologyName = ""

            Return Adapter.GetProductTechnologies(ProductTechnologyName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProductTechnologyName: " & ProductTechnologyName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProductTechnologiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProductTechnologyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProductTechnologiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Update ProductTechnologies
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateProductTechnologies(ByVal ProductTechnologyName As String, ByVal Obsolete As Boolean, ByVal original_ProductTechnologyID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProductTechnologyName = commonFunctions.convertSpecialChar(ProductTechnologyName, False)

            Dim rowsAffected As Integer = Adapter.UpdateProductTechnology(original_ProductTechnologyID, ProductTechnologyName, Obsolete, UpdatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProductTechnologyName: " & ProductTechnologyName & ", Obsolete: " & Obsolete & ", original_ProductTechnologyID: " & original_ProductTechnologyID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProductTechnologiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProductTechnologyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProductTechnologiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Insert New Subscriptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProductTechnologies(ByVal ProductTechnologyName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProductTechnologyName = commonFunctions.convertSpecialChar(ProductTechnologyName, False)

            Dim rowsAffected As Integer = Adapter.InsertProductTechnology(ProductTechnologyName, CreatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProductTechnologyName: " & ProductTechnologyName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProductTechnologiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProductTechnologyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertProductTechnologies : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProductTechnologiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
