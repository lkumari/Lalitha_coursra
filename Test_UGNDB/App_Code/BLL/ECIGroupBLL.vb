''******************************************************************************************************
''* ECIGroupBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/24/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIGroupBLL
    Private ECIGroupAdapter As ECIGroupTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIGroupTableAdapter
        Get
            If ECIGroupAdapter Is Nothing Then
                ECIGroupAdapter = New ECIGroupTableAdapter()
            End If
            Return ECIGroupAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIGroup returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIGroup(ByVal GroupID As Integer, ByVal GroupName As String) As ECI.ECIGroup_MaintDataTable

        Try

            If GroupName Is Nothing Then
                GroupName = ""
            End If

            Return Adapter.GetECIGroup(GroupID, GroupName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID _
            & ",GroupName: " & GroupName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECIGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECIGroup(ByVal GroupName As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If GroupName Is Nothing Then
                GroupName = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertECIGroup(GroupName, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupName:" & GroupName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update ECIGroup
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateECIGroup(ByVal GroupName As String, ByVal original_GroupID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If GroupName Is Nothing Then
                GroupName = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateECIGroup(original_GroupID, GroupName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID:" & original_GroupID _
            & ", GroupName: " & GroupName _
            & ", Obsolete: " & Obsolete _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateECIGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
