''******************************************************************************************************
''* ProgramsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 03/24/2008
''* Modified: LREY 05/06/2009 - Added ProgramSuffix, BPCSProgramRef and Make to the get/insert/update stmts.
''******************************************************************************************************


Imports ProgramsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ProgramsBLL
    Private programAdapter As ProgramTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ProgramsTableAdapters.ProgramTableAdapter
        Get
            If programAdapter Is Nothing Then
                programAdapter = New ProgramTableAdapter()
            End If
            Return programAdapter
        End Get
    End Property
    ''*****
    ''* Select Programs returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPrograms(ByVal ProgramName As String, ByVal ProgramCode As String, ByVal Make As String) As Programs.Program_MaintDataTable

        Try
            If ProgramName Is Nothing Then
                ProgramName = ""
            End If

            Return Adapter.GetPrograms(ProgramName, ProgramCode, Make)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramName: " & ProgramName & ", ProgramCode: " & ProgramCode & ", Make: " & Make & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPrograms : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProgramsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPrograms : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProgramsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Update Programs
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdatePrograms(ByVal ProgramID As Integer, ByVal ProgramName As String, ByVal BPCSProgramRef As String, ByVal ProgramSuffix As String, ByVal Make As String, ByVal Obsolete As Boolean, ByVal original_ProgramID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProgramName = commonFunctions.convertSpecialChar(ProgramName, False)
            If ProgramSuffix = Nothing Then
                ProgramSuffix = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateProgram(original_ProgramID, ProgramName, BPCSProgramRef, ProgramSuffix, Make, Obsolete, UpdatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & "ProgramName: " & ProgramName & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdatePrograms : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProgramsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePrograms : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProgramsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Insert New Subscriptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPrograms(ByVal ProgramName As String, ByVal BPCSProgramRef As String, ByVal ProgramSuffix As String, ByVal Make As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ProgramName = commonFunctions.convertSpecialChar(ProgramName, False)
            If ProgramSuffix = Nothing Then
                ProgramSuffix = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertProgram(ProgramName, BPCSProgramRef, ProgramSuffix, Make, CreatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramName: " & ProgramName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertPrograms : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ProgramsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPrograms : " & commonFunctions.convertSpecialChar(ex.Message, False), "ProgramsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
