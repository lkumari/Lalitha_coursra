''******************************************************************************************************
''* AssemblyPlantLocationBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LREY 05/20/2011
''******************************************************************************************************


Imports AssemblyPlantLocationTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AssemblyPlantLocationBLL
    Private pAdapter As Assembly_Plant_Location_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As AssemblyPlantLocationTableAdapters.Assembly_Plant_Location_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Assembly_Plant_Location_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property
    ''*****
    ''* Select Assembly_Plant_Location returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAssemblyPlantLocation(ByVal APID As Integer, ByVal Assembly As String, ByVal Country As String, ByVal OEMMfg As String, ByVal AssemblyType As String) As AssemblyPlantLocation.Assembly_Plant_LocationDataTable

        Try
            If APID = 0 Then APID = 0
            If Assembly = Nothing Then Assembly = ""
            If Country = Nothing Then Country = ""
            If OEMMfg = Nothing Then OEMMfg = ""
            If AssemblyType = Nothing Then AssemblyType = ""


            Return Adapter.Get_Assembly_Plant_Location(APID, Assembly, Country, OEMMfg, "")

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & ", Assembly: " & Assembly & ", Country: " & Country & ", OEMMfg: " & OEMMfg & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantLocationBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantLocationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Get Assembly_Plant_Location

    ''*****
    ''* Insert New Assembly_Plant_Location
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertAssemblyPlantLocation(ByVal Assembly_Plant_Location As String, ByVal OEMManufacturer As String, ByVal State As String, ByVal Country As String, ByVal UGNBusiness As Boolean, ByVal AssemblyType As String, ByVal IHS_Assembly_Plant As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Assembly_Plant_Location = commonFunctions.convertSpecialChar(Assembly_Plant_Location, False)
            IHS_Assembly_Plant = commonFunctions.convertSpecialChar(IHS_Assembly_Plant, False)


            Dim rowsAffected As Integer = Adapter.sp_Insert_Assembly_Plant_Location(Assembly_Plant_Location, OEMManufacturer, State, Country, UGNBusiness, AssemblyType, IHS_Assembly_Plant, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Assembly_Plant_Location: " & Assembly_Plant_Location & ", OEMManufacturer: " & OEMManufacturer & ", State: " & State & ", Country: " & Country & ", IHS_Assembly_Plant: " & IHS_Assembly_Plant & ", UGNBusiness: " & UGNBusiness & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantLocationBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantLocationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Insert Assembly_Plant_Location

    ''*****
    ''* Update Assembly_Plant_Location
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateAssemblyPlantLocation(ByVal APID As Integer, ByVal Assembly_Plant_Location As String, ByVal OEMManufacturer As String, ByVal State As String, ByVal Country As String, ByVal UGNBusiness As Boolean, ByVal AssemblyType As String, ByVal IHS_Assembly_Plant As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Assembly_Plant_Location = commonFunctions.convertSpecialChar(Assembly_Plant_Location, False)

            If IHS_Assembly_Plant <> Nothing Then
                IHS_Assembly_Plant = commonFunctions.convertSpecialChar(IHS_Assembly_Plant, False)
            End If

            Dim rowsAffected As Integer = Adapter.sp_Update_Assembly_Plant_Location(APID, Assembly_Plant_Location, OEMManufacturer, State, Country, UGNBusiness, AssemblyType, IHS_Assembly_Plant, Obsolete, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & "Assembly_Plant_Location: " & Assembly_Plant_Location & ", OEMManufacturer: " & OEMManufacturer & ", State: " & State & ", Country: " & Country & ", IHS_Assembly_Plant: " & IHS_Assembly_Plant & ", UGNBusiness: " & UGNBusiness & ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantLocationBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantLocationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Update Assembly_Plant_Location

    ''*****
    ''* Delete Assembly_Plant_Location
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAssemblyPlantLocation(ByVal APID As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Assembly_Plant_Location(APID)

            Return rowsAffected '= 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "APID: " & APID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AssemblyPlantLocationBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/AssemblyPlantLocationMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False), "AssemblyPlantLocationBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function 'EOF Delete Assembly_Plant_Location
End Class
