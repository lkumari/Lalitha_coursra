''******************************************************************************************************
''* ProcessBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/06/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ProcessBLL
    Private ProcessAdapter As ProcessTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.ProcessTableAdapter
        Get
            If ProcessAdapter Is Nothing Then
                ProcessAdapter = New ProcessTableAdapter()
            End If
            Return ProcessAdapter
        End Get
    End Property
    ''*****
    ''* Select Process returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetProcess(ByVal ProcessID As Integer, ByVal ProcessName As String) As Costing.Process_MaintDataTable

        Try

            If ProcessName Is Nothing Then
                ProcessName = ""
            End If

            Return Adapter.GetProcess(ProcessID, ProcessName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProcessID: " & ProcessID & ",ProcessName: " & ProcessName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProcessBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProcessBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Process
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProcess(ByVal ProcessName As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ProcessName Is Nothing Then
                ProcessName = ""
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertProcess(ProcessName, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProcessName:" & ProcessName & ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProcessBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProcessBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Process
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateProcess(ByVal ProcessName As String, ByVal original_ProcessID As Integer, ByVal ProcessID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ProcessName Is Nothing Then
                ProcessName = ""
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateProcess(original_ProcessID, ProcessName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProcessID:" & original_ProcessID & ", ProcessName: " & ProcessName & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProcessBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProcessBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
  
End Class
