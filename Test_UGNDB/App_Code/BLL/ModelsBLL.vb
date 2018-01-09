''******************************************************************************************************
''* ModelsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/10/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ModelsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ModelsBLL
    Private ModelsAdapter As ModelTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ModelsTableAdapters.ModelTableAdapter
        Get
            If ModelsAdapter Is Nothing Then
                ModelsAdapter = New ModelTableAdapter()
            End If
            Return ModelsAdapter
        End Get
    End Property
    ''*****
    ''* Select Models returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetModels(ByVal ModelName As String, ByVal Make As String) As Models.Model_MaintDataTable

        Try
            If ModelName Is Nothing Then
                ModelName = ""
            End If

            If Make Is Nothing Then
                Make = ""
            End If

            Return Adapter.GetModels(ModelName, Make)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ModelName: " & ModelName & ", Make: " & Make & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetModels : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ModelsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ModelMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetModels : " & commonFunctions.convertSpecialChar(ex.Message, False), "ModelsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Model
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertModel(ByVal ModelName As String, ByVal Make As String, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the Model record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertModel(ModelName, Make, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ModelName: " & ModelName & ", Make: " & Make & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertModel : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ModelsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ModelMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertModel : " & commonFunctions.convertSpecialChar(ex.Message, False), "ModelsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update Model
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateModel(ByVal ModelName As String, ByVal Make As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_ModelID As Integer) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the Model record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateModel(original_ModelID, ModelName, Make, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ModelName: " & ModelName & ", Make: " & Make & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateModel : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ModelsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ModelMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateModel : " & commonFunctions.convertSpecialChar(ex.Message, False), "ModelsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
