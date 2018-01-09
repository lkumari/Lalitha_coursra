''******************************************************************************************************
''* AssemblyPlantOEMBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LREY 05/25/2011
''* Modifed : LREY 07/14/2011 - Added GetPartNoByOEM function
''******************************************************************************************************

Imports AssemblyPlantLocationTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AssemblyPlantOEMBLL
    Private pAdapter As Assembly_Plant_OEM_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As AssemblyPlantLocationTableAdapters.Assembly_Plant_OEM_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Assembly_Plant_OEM_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property

    Private pAdapter2 As PartNo_by_OEM_TableAdapter = Nothing
    Protected ReadOnly Property Adapter2() As AssemblyPlantLocationTableAdapters.PartNo_by_OEM_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New PartNo_by_OEM_TableAdapter
            End If
            Return pAdapter2
        End Get
    End Property

    ''*****
    ''* Select Assembly_Plant_OEM returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAssemblyPlantOEM(ByVal APID As Integer, ByVal ModelName As String, ByVal PlatformID As Integer) As AssemblyPlantLocation.Assembly_Plant_OEMDataTable

        Try
            If APID = 0 Then APID = 0

            If ModelName = Nothing Then ModelName = ""

            If PlatformID = 0 Then PlatformID = 0

            Return Adapter.Get_Assembly_Plant_OEM(APID, ModelName, PlatformID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantOEMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantOEMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Get Assembly_Plant_OEM

    ''*****
    ''* Insert New Assembly_Plant_Location
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertAssemblyPlantOEM(ByVal APID As Integer, ByVal PlatformID As Integer, ByVal OEMModelType As String, ByVal Make As String, ByVal ModelName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            OEMModelType = commonFunctions.convertSpecialChar(OEMModelType, False)
            Make = commonFunctions.convertSpecialChar(Make, False)
            ModelName = commonFunctions.convertSpecialChar(ModelName, False)

            Dim rowsAffected As Integer = Adapter.sp_Insert_Assembly_Plant_OEM(APID, PlatformID, OEMModelType, Make, ModelName, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & ", OEMModelType: " & OEMModelType & ", Make: " & Make & ", ModelName: " & ModelName & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantOEMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantOEMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Insert Assembly_Plant_OEM

    ''*****
    ''* Update Assembly_Plant_OEM
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateAssemblyPlantOEM(ByVal Obsolete As Boolean, ByVal original_OEMModelType As String, ByVal original_Make As String, ByVal original_ModelName As String, ByVal original_APID As Integer, ByVal original_PlatformID As Integer, ByVal OEMModelType As String, ByVal Make As String, ByVal ModelName As String, ByVal PlatformID As Integer) As Boolean

        Try
            'ByVal APID As Integer,
            Dim psTable As AssemblyPlantLocation.Assembly_Plant_OEMDataTable = Adapter.Get_Assembly_Plant_OEM(original_APID, original_ModelName, original_PlatformID)
            Dim psRow As AssemblyPlantLocation.Assembly_Plant_OEMRow = psTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If psTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_OEMModelType = Nothing Then
                Throw New ApplicationException("Update Cancelled: Original OEM Model Type is a required field.")
            End If

            OEMModelType = commonFunctions.convertSpecialChar(OEMModelType, False)

            'Dim sMake = Make.Trim
            'Dim iMakeLen As Integer = Len(sMake)
            'Dim iMakeStartPos As Integer = InStr(Make, ":")
            'sMake = sMake.Substring(0, iMakeStartPos - 1)
            'Make = commonFunctions.convertSpecialChar(sMake, False)

            'Dim sModelName = ModelName.Trim
            'Dim iModelNameLen As Integer = Len(sModelName)
            'Dim iModelNameStartPos As Integer = InStr(ModelName, ":")
            'sModelName = sModelName.Substring(0, iModelNameStartPos - 1)
            'ModelName = commonFunctions.convertSpecialChar(sModelName, False)

            Dim rowsAffected As Integer = Adapter.sp_Update_Assembly_Plant_OEM(original_APID, PlatformID, OEMModelType, Make, ModelName, Obsolete, User, original_OEMModelType, original_Make, original_ModelName)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & original_APID & ", OEMModelType: " & OEMModelType & ", Make: " & Make & ", ModelName: " & ModelName & ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateAssemblyPlantOEM: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantOEMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantOEMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Update Assembly_Plant_OEM

    ''*****
    ''* Delete Assembly_Plant_OEM
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAssemblyPlantOEM(ByVal APID As Integer, ByVal OEMModelType As String, ByVal Make As String, ByVal ModelName As String, ByVal original_APID As Integer, ByVal original_OEMModelType As String, ByVal original_Make As String, ByVal original_ModelName As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Assembly_Plant_OEM(original_APID, original_OEMModelType, original_Make, original_ModelName)

            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & original_APID & ", OEMModelType: " & original_OEMModelType & ", Make: " & original_Make & ", ModelName: " & original_ModelName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantOEMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAssemblyPlantOEM : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantOEMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Delete Assembly_Plant_OEM


    ''*****
    ''* Select PartNo_By_OEM returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPartNoByOEM(ByVal APID As Integer, ByVal ModelName As String, ByVal OEMModelType As String, ByVal PARTNO As String, ByVal CPART As String, ByVal COMPNY As String, ByVal PRCCDE As String) As AssemblyPlantLocation.PartNo_by_OEMDataTable

        Try
            If APID = 0 Then
                APID = 0
            End If

            If ModelName = Nothing Then
                ModelName = ""
            End If

            If OEMModelType = Nothing Then
                OEMModelType = ""
            End If

            If PARTNO = Nothing Then
                PARTNO = ""
            End If

            If CPART = Nothing Then
                CPART = ""
            End If

            If COMPNY = Nothing Then
                COMPNY = ""
            End If

            If PRCCDE = Nothing Then
                PRCCDE = ""
            End If

            Return Adapter2.Get_PartNo_by_OEM(APID, ModelName, OEMModelType, PARTNO, CPART, COMPNY, PRCCDE)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & ", ModelName: " & ModelName & ", OEMModelType: " & OEMModelType & ", PARTNO: " & PARTNO & ", CPART: " & CPART & ", COMPNY: " & COMPNY & ", PRCCDE: " & PRCCDE & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPartNoByOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantOEMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantDisplay.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPartNoByOEM : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantOEMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Get PartNo_By_OEM
End Class
