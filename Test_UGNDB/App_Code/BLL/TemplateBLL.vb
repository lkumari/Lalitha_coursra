''******************************************************************************************************
''* TemplateBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/06/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class TemplateBLL
    Private TemplateAdapter As TemplateTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.TemplateTableAdapter
        Get
            If TemplateAdapter Is Nothing Then
                TemplateAdapter = New TemplateTableAdapter()
            End If
            Return TemplateAdapter
        End Get
    End Property
    ''*****
    ''* Select Template returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTemplate(ByVal TemplateID As Integer, ByVal TemplateName As String) As Costing.Template_MaintDataTable

        Try

            If TemplateName Is Nothing Then
                TemplateName = ""
            End If

            Return Adapter.GetTemplate(TemplateID, TemplateName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TemplateID: " & TemplateID & ",TemplateName: " & TemplateName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TemplateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False), "TemplateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Template
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTemplate(ByVal TemplateName As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TemplateName Is Nothing Then
                TemplateName = ""
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertTemplate(TemplateName, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TemplateName:" & TemplateName & ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TemplateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False), "TemplateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Template
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateTemplate(ByVal TemplateName As String, ByVal original_TemplateID As Integer, ByVal TemplateID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TemplateName Is Nothing Then
                TemplateName = ""
            End If

            ''*****
            ' Update the CostingMaterialsBLL record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateTemplate(original_TemplateID, TemplateName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TemplateID:" & original_TemplateID & ", TemplateName: " & TemplateName & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> TemplateBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTemplate : " & commonFunctions.convertSpecialChar(ex.Message, False), "TemplateBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
   
End Class
