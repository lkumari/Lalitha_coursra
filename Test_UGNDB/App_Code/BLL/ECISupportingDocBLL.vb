''******************************************************************************************************
''* ECISupportingDocBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/30/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECISupportingDocBLL
    Private ECISupportingDocAdapter As ECISupportingDocTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECISupportingDocTableAdapter
        Get
            If ECISupportingDocAdapter Is Nothing Then
                ECISupportingDocAdapter = New ECISupportingDocTableAdapter()
            End If
            Return ECISupportingDocAdapter
        End Get
    End Property
    ''*****
    ''* Select ECISupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECISupportingDoc(ByVal ECINo As Integer) As ECI.ECISupportingDoc_MaintDataTable

        Try

            Return Adapter.GetECISupportingDocList(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECISupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECISupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New ECIGroup
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertECISupportingDoc(ByVal GroupID As Integer, ByVal TeamMemberID As Integer) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.InsertECISupportingDoc(GroupID, TeamMemberID, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "GroupID:" & GroupID _
    '        & ", TeamMemberID:" & TeamMemberID _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECISupportingDocBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECISupportingDocBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ' ''*****
    ''* Update ECIGroup
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    '    Public Function UpdateECISupportingDoc(ByVal original_RowID As Integer, ByVal Obsolete As Boolean, ByVal ddGroupName As String, ByVal ddTeamMemberName As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        Dim rowsAffected As Integer = Adapter.UpdateECISupportingDoc(original_RowID, Obsolete, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "original_RowID:" & original_RowID _
    '        & ", Obsolete: " & Obsolete _
    '        & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECISupportingDocBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateECISupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECISupportingDocBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ''*****
    ''* Delete ECISupportingDocListBLL
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteECISupportingDoc(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECISupportingDoc(original_RowID)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECISupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECISupportingDocBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteECISupportingDoc: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ECISupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
