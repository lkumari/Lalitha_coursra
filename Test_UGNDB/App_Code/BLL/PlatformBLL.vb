''******************************************************************************************************
''* PlatformBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LREY 04/19/2011
''******************************************************************************************************


Imports PlatformTableAdapters

<System.ComponentModel.DataObject()> _
Public Class PlatformBLL
    Private pAdapter As Platform_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As PlatformTableAdapters.Platform_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Platform_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property 'PLATFORM

    Private pAdapter2 As Platform_Program_TableAdapter = Nothing
    Protected ReadOnly Property Adapter2() As PlatformTableAdapters.Platform_Program_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New Platform_Program_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'PLATFORM_PROGRAM

    Private pAdapter3 As Program_Volume_TableAdapter = Nothing
    Protected ReadOnly Property Adapter3() As PlatformTableAdapters.Program_Volume_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New Program_Volume_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'PROGRAM_VOLUME

    ''*****
    ''* Select Platform returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPlatform(ByVal PlatformID As Integer, ByVal PlatformName As String, ByVal OEMManufacturer As String, ByVal DisplayUGNBusiness As String, ByVal DisplayCurrentPlatform As String, ByVal SortBy As String) As Platform.PlatformDataTable

        Try
            If PlatformID = Nothing Then PlatformID = 0

            If PlatformName = Nothing Then PlatformName = ""

            If OEMManufacturer = Nothing Then OEMManufacturer = ""

            If SortBy = Nothing Then SortBy = ""


            Return Adapter.Get_Platform(PlatformID, PlatformName, OEMManufacturer, DisplayUGNBusiness, DisplayCurrentPlatform, SortBy)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformName: " & PlatformName & ", OEMManufacturer: " & OEMManufacturer & ", DisplayUGNBusiness: " & DisplayUGNBusiness & ", DisplayCurrentPlatform: " & DisplayCurrentPlatform & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPlatform : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPlatform : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select Platform returning all rows

    ''*****
    ''* Insert New Platform
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPlatform(ByVal PlatformName As String, ByVal OEMManufacturer As String, ByVal BegYear As Integer, ByVal EndYear As Integer, ByVal UGNBusiness As Boolean, ByVal CurrentPlatform As Boolean, ByVal ServiceYears As Integer, ByVal Notes As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PlatformName Is Nothing Then PlatformName = ""

            If OEMManufacturer Is Nothing Then OEMManufacturer = ""

            If BegYear = 0 Then BegYear = 0

            If EndYear = 0 Then EndYear = 0

            If ServiceYears = 0 Then ServiceYears = 0

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.replaceSpecialChar(Notes, False)
            End If


            Dim rowsAffected As Integer = Adapter.sp_Insert_Platform(PlatformName, OEMManufacturer, BegYear, EndYear, UGNBusiness, CurrentPlatform, ServiceYears, Notes, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformName: " & PlatformName & ", OEMManufacturer: " & OEMManufacturer & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPlatform : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPlatform : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Insert New Platform

    ''*****
    ''* Update Platform
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdatePlatform(ByVal PlatformName As String, ByVal OEMManufacturer As String, ByVal BegYear As Integer, ByVal EndYear As Integer, ByVal Obsolete As Boolean, ByVal UGNBusiness As Boolean, ByVal CurrentPlatform As Boolean, ByVal ServiceYears As Integer, ByVal Notes As String, ByVal original_PlatformID As Integer, ByVal original_PlatformName As String, ByVal original_OEMManufacturer As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PlatformName Is Nothing Then PlatformName = ""

            If OEMManufacturer Is Nothing Then OEMManufacturer = ""

            If BegYear = 0 Then BegYear = 0

            If EndYear = 0 Then EndYear = 0

            If ServiceYears = 0 Then ServiceYears = 0

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.replaceSpecialChar(Notes, False)
            End If

            Dim rowsAffected As Integer = Adapter.sp_Update_Platform(original_PlatformID, PlatformName, OEMManufacturer, BegYear, EndYear, UGNBusiness, CurrentPlatform, ServiceYears, Notes, UpdatedBy, original_PlatformName, original_OEMManufacturer)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformID: " & original_PlatformID & ", PlatformName: " & PlatformName & ", OEMManufacturer: " & OEMManufacturer & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            '& ", CSMPlatform: " & CSMPlatform & ", WAFPlatform: " & WAFPlatform 

            HttpContext.Current.Session("BLLerror") = "UpdatePlatform : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePlatform : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Update Platform

    ''*****
    ''* Select Platform_Program returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPlatformProgram(ByVal PlatformID As Integer, ByVal ProgramID As Integer, ByVal ProgramCode As String, ByVal ModelName As String, ByVal Make As String) As Platform.Platform_ProgramDataTable

        Try
            If PlatformID = Nothing Or PlatformID = 0 Then PlatformID = 0
            If ProgramID = Nothing Or ProgramID = 0 Then ProgramID = 0
            If ProgramCode = Nothing Then ProgramCode = ""
            If ModelName = Nothing Then ModelName = ""
            If Make = Nothing Then Make = ""

            Return Adapter2.Get_Platform_Program(PlatformID, ProgramID, ProgramCode, ModelName, Make)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformID: " & PlatformID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select Platform returning all rows

    ''*****
    ''* Insert New PlatformProgram
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPlatformProgram(ByVal PlatformID As Integer, ByVal Mnemonic_Platform As Integer, ByVal Mnemonic_Vehicle As Integer, ByVal Mnemonic_Vehicle_Plant As Integer, ByVal Make As String, ByVal CSM_Program As String, ByVal CSM_Model_Name As String, ByVal VTID As Integer, ByVal APID As Integer, ByVal SOPMM As Integer, ByVal SOPYY As Integer, ByVal EOPMM As Integer, ByVal EOPYY As Integer, ByVal BPCSProgramRef As String, ByVal ProgramName As String, ByVal ProgramSuffix As String, ByVal UGNBusiness As Boolean, ByVal Notes As String, ByVal ServiceAPID As Integer, ByVal ServiceEOPMM As Integer, ByVal ServiceEOPYY As Integer) As Boolean
        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Make Is Nothing Then Make = ""

            If Mnemonic_Platform = 0 Then Mnemonic_Platform = 0

            If Mnemonic_Vehicle = 0 Then Mnemonic_Vehicle = 0

            If Mnemonic_Vehicle_Plant = 0 Then Mnemonic_Vehicle_Plant = 0

            If CSM_Program Is Nothing Then CSM_Program = ""

            If CSM_Model_Name Is Nothing Then CSM_Model_Name = ""

            If VTID = 0 Then VTID = 0

            If APID = 0 Then APID = 0

            If SOPMM = 0 Then SOPMM = 0

            If SOPYY = 0 Then SOPYY = 0

            If EOPMM = 0 Then EOPMM = 0

            If EOPYY = 0 Then EOPYY = 0

            If BPCSProgramRef Is Nothing Then BPCSProgramRef = ""

            If ProgramName Is Nothing Then ProgramName = ""

            If ProgramSuffix Is Nothing Then ProgramSuffix = ""

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.replaceSpecialChar(Notes, False)
            End If

            If ServiceAPID = 0 Then ServiceAPID = 0

            If ServiceEOPMM = 0 Then ServiceEOPMM = 0

            If ServiceEOPYY = 0 Then ServiceEOPYY = 0


            Dim rowsAffected As Integer = Adapter2.sp_Insert_Program_by_Platform(PlatformID, Mnemonic_Platform, Mnemonic_Vehicle, Mnemonic_Vehicle_Plant, Make, CSM_Program, CSM_Model_Name, VTID, APID, SOPMM, SOPYY, EOPMM, EOPYY, BPCSProgramRef, ProgramName, ProgramSuffix, UGNBusiness, Notes, ServiceAPID, ServiceEOPMM, ServiceEOPYY, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformID: " & PlatformID & ", Make: " & Make & ", Mnemonic_Platform: " & Mnemonic_Platform & ", Mnemonic_Vehicle: " & Mnemonic_Vehicle & ", Mnemonic_Vehicle_Plant: " & Mnemonic_Vehicle_Plant & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertPlatformProgram

    ''*****
    ''* Update New Platform Program
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdatePlatformProgram(ByVal Make As String, ByVal CSM_Program As String, ByVal CSM_Model_Name As String, ByVal WAF_Model_Name As String, ByVal VTID As Integer, ByVal APID As Integer, ByVal SOPMM As Integer, ByVal SOPYY As Integer, ByVal EOPMM As Integer, ByVal EOPYY As Integer, ByVal BPCSProgramRef As String, ByVal ProgramSuffix As String, ByVal UGNBusiness As Boolean, ByVal Notes As String, ByVal ServiceAPID As Integer, ByVal ServiceEOPMM As Integer, ByVal ServiceEOPYY As Integer, ByVal Obsolete As Boolean, ByVal original_ProgramID As Integer, ByVal original_PlatformID As Integer, ByVal ProgramName As String, ByVal PlatformID As Integer, ByVal Mnemonic_Platform As Integer, ByVal Mnemonic_Vehicle As Integer, ByVal Mnemonic_Vehicle_Plant As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If PlatformID = 0 Then PlatformID = 0

            If Make Is Nothing Then Make = ""

            If CSM_Program Is Nothing Then CSM_Program = ""

            If Mnemonic_Platform = 0 Then Mnemonic_Platform = 0

            If Mnemonic_Vehicle = 0 Then Mnemonic_Vehicle = 0

            If Mnemonic_Vehicle_Plant = 0 Then Mnemonic_Vehicle_Plant = 0

            If CSM_Model_Name Is Nothing Then CSM_Model_Name = ""

            If VTID = 0 Then VTID = 0

            If APID = 0 Then APID = ""

            If SOPMM = 0 Then SOPMM = 0

            If SOPYY = 0 Then SOPYY = 0

            If EOPMM = 0 Then EOPMM = 0

            If EOPYY = 0 Then EOPYY = 0

            If BPCSProgramRef Is Nothing Then BPCSProgramRef = ""

            If ProgramName Is Nothing Then ProgramName = ""

            If ProgramSuffix Is Nothing Then ProgramSuffix = ""

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.replaceSpecialChar(Notes, False)
            End If

            If ServiceAPID = 0 Then ServiceAPID = 0

            If ServiceEOPMM = 0 Then ServiceEOPMM = 0

            If ServiceEOPYY = 0 Then ServiceEOPYY = 0

            Dim rowsAffected As Integer = Adapter2.sp_Update_Program_by_Platform(original_PlatformID, PlatformID, original_ProgramID, Mnemonic_Platform, Mnemonic_Vehicle, Mnemonic_Vehicle_Plant, Make, CSM_Program, CSM_Model_Name, VTID, APID, SOPMM, SOPYY, EOPMM, EOPYY, BPCSProgramRef, ProgramName, ProgramSuffix, UGNBusiness, Notes, ServiceAPID, ServiceEOPMM, ServiceEOPYY, Obsolete, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & original_ProgramID & ", PlatformID: " & original_PlatformID & ", Make: " & Make & ", Program: " & BPCSProgramRef & ", Mnemonic_Platform: " & Mnemonic_Platform & ", Mnemonic_Vehicle: " & Mnemonic_Vehicle & ", Mnemonic_Vehicle_Plant: " & Mnemonic_Vehicle_Plant & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF UpdatePlatformProgram

    ''*****
    ''* Delete Program by Platform
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeletePlatformProgram(ByVal PlatformID As Integer, ByVal ProgramID As Integer, ByVal original_PlatformID As Integer, ByVal original_ProgramID As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter2.sp_Delete_Program_by_Platform(original_PlatformID, original_ProgramID)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlatformID: " & original_PlatformID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PlatformMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF DeletePlatformProgram


    ''*****
    ''* Select Program_Volume returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetProgramVolume(ByVal ProgramID As Integer, ByVal YearID As Integer) As Platform.Program_VolumeDataTable

        Try
            If ProgramID = Nothing Or ProgramID = 0 Then ProgramID = 0
            If YearID = Nothing Or YearID = 0 Then YearID = 0

            Return Adapter3.Get_Program_Volume(ProgramID, YearID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", YearID: " & YearID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramVolume.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select ProgramVolume returning all rows

    ''*****
    ''* Insert New ProgramVolume
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProgramVolume(ByVal ProgramID As Integer, ByVal YearID As Integer, ByVal JanVolume As Decimal, ByVal FebVolume As Decimal, ByVal MarVolume As Decimal, ByVal AprVolume As Decimal, ByVal MayVolume As Decimal, ByVal JunVolume As Decimal, ByVal JulVolume As Decimal, ByVal AugVolume As Decimal, ByVal SepVolume As Decimal, ByVal OctVolume As Decimal, ByVal NovVolume As Decimal, ByVal DecVolume As Decimal) As Boolean
        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ProgramID = 0 Then ProgramID = 0
            If YearID = 0 Then YearID = 0

            Dim rowsAffected As Integer = Adapter3.sp_Insert_Program_Volume(ProgramID, YearID, JanVolume, FebVolume, MarVolume, AprVolume, MayVolume, JunVolume, JulVolume, AugVolume, SepVolume, OctVolume, NovVolume, DecVolume, CreatedBy)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", YearID: " & YearID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramVolume.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF InsertProgramVolume

    ''*****
    ''* Update New Platform Program
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateProgramVolume(ByVal JanVolume As Decimal, ByVal FebVolume As Decimal, ByVal MarVolume As Decimal, ByVal AprVolume As Decimal, ByVal MayVolume As Decimal, ByVal JunVolume As Decimal, ByVal JulVolume As Decimal, ByVal AugVolume As Decimal, ByVal SepVolume As Decimal, ByVal OctVolume As Decimal, ByVal NovVolume As Decimal, ByVal DecVolume As Decimal, ByVal original_ProgramID As Integer, ByVal original_YearID As Integer, ByVal YearID As Integer) As Boolean
        Try
            Dim psTable As Platform.Program_VolumeDataTable = Adapter3.Get_Program_Volume(original_ProgramID, original_YearID)
            Dim psRow As Platform.Program_VolumeRow = psTable(0)
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If psTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            If original_ProgramID = 0 Then original_ProgramID = 0
            If YearID = 0 Then YearID = 0

            Dim SOPMM As Integer = HttpContext.Current.Request.QueryString("sSOPMM")
            Dim SOPYY As Integer = HttpContext.Current.Request.QueryString("sSOPYY")
            Dim EOPMM As Integer = HttpContext.Current.Request.QueryString("sEOPMM")
            Dim EOPYY As Integer = HttpContext.Current.Request.QueryString("sEOPYY")

            ''Zero out months that do not fall within the Program SOP and EOP dates
            If ((YearID = SOPYY) And (1 < SOPMM)) Or ((YearID = EOPYY) And (1 > EOPMM)) Then
                JanVolume = 0
            End If
            If ((YearID = SOPYY) And (2 < SOPMM)) Or ((YearID = EOPYY) And (2 > EOPMM)) Then
                FebVolume = 0
            End If
            If ((YearID = SOPYY) And (3 < SOPMM)) Or ((YearID = EOPYY) And (3 > EOPMM)) Then
                MarVolume = 0
            End If
            If ((YearID = SOPYY) And (4 < SOPMM)) Or ((YearID = EOPYY) And (4 > EOPMM)) Then
                AprVolume = 0
            End If
            If ((YearID = SOPYY) And (5 < SOPMM)) Or ((YearID = EOPYY) And (5 > EOPMM)) Then
                MayVolume = 0
            End If
            If ((YearID = SOPYY) And (6 < SOPMM)) Or ((YearID = EOPYY) And (6 > EOPMM)) Then
                JunVolume = 0
            End If
            If ((YearID = SOPYY) And (7 < SOPMM)) Or ((YearID = EOPYY) And (7 > EOPMM)) Then
                JulVolume = 0
            End If
            If ((YearID = SOPYY) And (8 < SOPMM)) Or ((YearID = EOPYY) And (8 > EOPMM)) Then
                AugVolume = 0
            End If
            If ((YearID = SOPYY) And (9 < SOPMM)) Or ((YearID = EOPYY) And (9 > EOPMM)) Then
                SepVolume = 0
            End If
            If ((YearID = SOPYY) And (10 < SOPMM)) Or ((YearID = EOPYY) And (10 > EOPMM)) Then
                OctVolume = 0
            End If
            If ((YearID = SOPYY) And (11 < SOPMM)) Or ((YearID = EOPYY) And (11 > EOPMM)) Then
                NovVolume = 0
            End If
            If ((YearID = SOPYY) And (12 < SOPMM)) Or ((YearID = EOPYY) And (12 > EOPMM)) Then
                DecVolume = 0
            End If

            Dim rowsAffected As Integer = Adapter3.sp_Update_Program_Volume(original_ProgramID, YearID, JanVolume, FebVolume, MarVolume, AprVolume, MayVolume, JunVolume, JulVolume, AugVolume, SepVolume, OctVolume, NovVolume, DecVolume, UpdatedBy, original_YearID)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & original_ProgramID & ", ProgramID: " & original_ProgramID & ", YearID: " & original_YearID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramVolume.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF UpdateProgramVolume

    ''*****
    ''* Delete Program by Platform
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteProgramVolume(ByVal ProgramID As Integer, ByVal YearID As Integer, ByVal original_ProgramID As Integer, ByVal original_YearID As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter3.sp_Delete_Program_Volume(original_ProgramID, original_YearID)

            Return rowsAffected
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & original_ProgramID & ", YearID: " & original_YearID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PlatformBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/ProgramVolume.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False), "PlatformBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF DeleteProgramVolume


End Class
