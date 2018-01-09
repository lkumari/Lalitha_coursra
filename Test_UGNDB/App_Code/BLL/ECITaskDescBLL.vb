''******************************************************************************************************
''* ECITaskDescBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/24/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECITaskDescBLL
    Private ECITaskDescAdapter As ECITaskDescTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECITaskDescTableAdapter
        Get
            If ECITaskDescAdapter Is Nothing Then
                ECITaskDescAdapter = New ECITaskDescTableAdapter()
            End If
            Return ECITaskDescAdapter
        End Get
    End Property
    ''*****
    ''* Select ECITaskDesc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECITaskDesc(ByVal TaskID As Integer, ByVal TaskName As String) As ECI.ECITaskDesc_MaintDataTable

        Try

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Return Adapter.GetECITaskDesc(TaskID, TaskName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID: " & TaskID _
            & ",TaskName: " & TaskName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskDescBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskDescBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECITaskDesc
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECITaskDesc(ByVal TaskName As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertECITaskDesc(TaskName, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskName:" & TaskName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskDescBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskDescBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update ECITaskDesc
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateECITaskDesc(ByVal TaskName As String, ByVal original_TaskID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateECITaskDesc(original_TaskID, TaskName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID:" & original_TaskID _
            & ", TaskName: " & TaskName _
            & ", Obsolete: " & Obsolete _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECITaskDescBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECITaskDesc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECITaskDescBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
