''******************************************************************************************************
''* ECIGroupTeamMemberBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/24/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIGroupTeamMemberBLL
    Private ECIGroupTeamMemberAdapter As ECIGroupTeamMemberTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIGroupTeamMemberTableAdapter
        Get
            If ECIGroupTeamMemberAdapter Is Nothing Then
                ECIGroupTeamMemberAdapter = New ECIGroupTeamMemberTableAdapter()
            End If
            Return ECIGroupTeamMemberAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIGroupTeamMember returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer) As ECI.ECIGroupTeamMember_MaintDataTable

        Try

            Return Adapter.GetECIGroupTeamMember(GroupID, TeamMemberID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID: " & GroupID _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupTeamMemberBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECIGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECIGroupTeamMember(ByVal GroupID As Integer, ByVal TeamMemberID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertECIGroupTeamMember(GroupID, TeamMemberID, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "GroupID:" & GroupID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupTeamMemberBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update ECIGroup
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateECIGroupTeamMember(ByVal original_RowID As Integer, ByVal Obsolete As Boolean, ByVal ddGroupName As String, ByVal ddTeamMemberName As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.UpdateECIGroupTeamMember(original_RowID, Obsolete, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "original_RowID:" & original_RowID _
    '        & ", Obsolete: " & Obsolete _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupTeamMemberBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateECIGroupTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupTeamMemberBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ''*****
    ''* Delete ECIGroupTeamMemberListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteECIGroupTeamMember(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECIGroupTeamMember(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECIGroupTeamMember: " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIGroupTeamMemberBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteECIGroupTeamMember: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECIGroupTeamMemberBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
