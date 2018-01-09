''******************************************************************************************************
''* ECICustomerProgramBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/20/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECICustomerProgramBLL
    Private ECICustomerProgramAdapter As ECICustomerProgramTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECICustomerProgramTableAdapter
        Get
            If ECICustomerProgramAdapter Is Nothing Then
                ECICustomerProgramAdapter = New ECICustomerProgramTableAdapter()
            End If
            Return ECICustomerProgramAdapter
        End Get
    End Property
    ''*****
    ''* Select ECICustomerProgram returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECICustomerProgram(ByVal ECINo As Integer) As ECI.ECICustomerProgram_MaintDataTable

        Try

            Return Adapter.GetECICustomerProgram(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECICustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECICustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Insert New ECICustomerProgram
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    'Public Function InsertECICustomerProgram(ByVal ECINo As Integer, ByVal CABBV As String, _
    'ByVal SoldTo As Integer, ByVal isCustomerApprovalRequired As Boolean, ByVal CustomerApprovalDate As String, _
    'ByVal CustomerApprovalNo As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer, _
    'ByVal SOPDate As String, ByVal EOPDate As String) As Boolean

    '    Try
    '        Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If CustomerApprovalDate Is Nothing Then
    '            CustomerApprovalDate = ""
    '        End If

    '        If CustomerApprovalNo Is Nothing Then
    '            CustomerApprovalNo = ""
    '        End If

    '        If SOPDate Is Nothing Then
    '            SOPDate = ""
    '        End If

    '        If EOPDate Is Nothing Then
    '            EOPDate = ""
    '        End If

    '        Dim rowsAffected As Integer = Adapter.InsertECICustomerProgram(ECINo, CABBV, SoldTo, isCustomerApprovalRequired, CustomerApprovalDate, _
    '        CustomerApprovalNo, ProgramID, ProgramYear, SOPDate, EOPDate, createdBy)
    '        ' Return true if precisely one row was inserted, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "ECINo:" & ECINo _
    '        & ", CABBV:" & CABBV & ", SoldTo:" & SoldTo _
    '        & ", ProgramID:" & ProgramID _
    '        & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "InsertECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECICustomerProgramBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("InsertECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECICustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function
    ' ''*****
    ''* Update ECICustomerProgram
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    'Public Function UpdateECICustomerProgram(ByVal original_RowID As Integer, ByVal isCustomerApprovalRequired As Boolean, _
    'ByVal CustomerApprovalDate As String, ByVal CustomerApprovalNo As String, _
    'ByVal ProgramID As Integer, ByVal ProgramYear As Integer, ByVal SOPDate As String, ByVal EOPDate As String, _
    'ByVal RowID As Integer, ByVal ECINo As Integer, ByVal ddCustomerDesc As String) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If CustomerApprovalDate Is Nothing Then
    '            CustomerApprovalDate = ""
    '        End If

    '        If CustomerApprovalNo Is Nothing Then
    '            CustomerApprovalNo = ""
    '        End If

    '        If SOPDate Is Nothing Then
    '            SOPDate = ""
    '        End If

    '        If EOPDate Is Nothing Then
    '            EOPDate = ""
    '        End If

    '        Dim rowsAffected As Integer = Adapter.UpdateECICustomerProgram(original_RowID, isCustomerApprovalRequired, CustomerApprovalDate, _
    '        CustomerApprovalNo, ProgramID, ProgramYear, SOPDate, EOPDate, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & original_RowID & ", isCustomerApprovalRequired:" & isCustomerApprovalRequired _
    '         & ", CustomerApprovalDate:" & CustomerApprovalDate & ", CustomerApprovalNo:" & CustomerApprovalNo _
    '         & ", ProgramID:" & ProgramID & ", ProgramYear:" & ProgramYear _
    '         & ", SOPDate:" & SOPDate & ", EOPDate:" & EOPDate _
    '         & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECICustomerProgramBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECICustomerProgramBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete ECICustomerProgram
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteECICustomerProgram(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECICustomerProgram(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECICustomerProgram: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECICustomerProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECICustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECICustomerProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
