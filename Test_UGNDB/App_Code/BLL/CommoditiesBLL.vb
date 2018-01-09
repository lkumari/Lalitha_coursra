''******************************************************************************************************
''* CommoditiesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/08/2008
''* Modified: 10/27/2011    LRey    - Added Commodity_Class functions as the Primary Table 
''*                                 - Commodity_Maint will be the sub-commodity that will tie to the classification.
''******************************************************************************************************

Imports CommoditiesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CommoditiesBLL
    Private CommoditiesAdapter As CommodityTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CommoditiesTableAdapters.CommodityTableAdapter
        Get
            If CommoditiesAdapter Is Nothing Then
                CommoditiesAdapter = New CommodityTableAdapter()
            End If
            Return CommoditiesAdapter
        End Get
    End Property 'COMMODITY_MAINT

    Private pAdapter2 As Commodity_ClassTableAdapter = Nothing
    Protected ReadOnly Property Adapter2() As CommoditiesTableAdapters.Commodity_ClassTableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New Commodity_ClassTableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'COMMODITY_CLASS

    ''*****
    ''* Select Commodities returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCommodities(ByVal CommodityID As Integer, ByVal commodityName As String, ByVal CommodityClass As String, ByVal CCID As Integer) As Commodities.Commodity_MaintDataTable

        Try
            If commodityName Is Nothing Then
                commodityName = ""
            End If

            If CommodityClass Is Nothing Then
                CommodityClass = ""
            End If

            If CCID = 0 Then
                CCID = 0
            End If

            Return Adapter.GetCommodities(CommodityID, commodityName, CommodityClass, CCID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "commodityName: " & commodityName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCommodities : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCommodities : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetCommodities
    ''*****
    ''* Insert New Commodity
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddCommodity(ByVal commodityName As String, ByVal BPCSCommodityRef As String, ByVal CCID As Integer, ByVal ProjectCode As String, ByVal PreDevCode As String, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ProjectCode Is Nothing Then
                ProjectCode = ""
            End If
            If PreDevCode Is Nothing Then
                PreDevCode = ""
            End If
            If CCID = 0 Then
                CCID = 1 'default to N/A
            End If
            ''*****
            ' Insert the Commodity record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCommodity(commodityName, BPCSCommodityRef, CCID, ProjectCode, PreDevCode, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "commodityName: " & commodityName & ", BPCSCommodityRef: " & BPCSCommodityRef & ", CCID: " & CCID & ", ProjectCode: " & ProjectCode & ", PreDevCode: " & PreDevCode & ", createdBy: " & createdBy
            HttpContext.Current.Session("BLLerror") = "AddCommodity : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("AddCommodity : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function 'EOF AddCommodity
    ''*****
    ''* Update Commodity
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCommodity(ByVal commodityID As Integer, ByVal CommodityName As String, ByVal BPCSCommodityRef As String, ByVal CCID As Integer, ByVal PreDevCode As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_CommodityID As Integer, ByVal ProjectCode As String) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            'if commodity ID has no value, get original_commodityID
            ''*****
            Dim tempCommodityID As Integer = commodityID

            If tempCommodityID = 0 Then
                tempCommodityID = original_CommodityID
            End If

            If CCID = 0 Then
                CCID = 1 'Default N/A
            End If
            If ProjectCode Is Nothing Then
                ProjectCode = ""
            End If
            If PreDevCode Is Nothing Then
                PreDevCode = ""
            End If

            ''*****
            ' Update the Commodity record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCommodity(tempCommodityID, CommodityName, BPCSCommodityRef, CCID, ProjectCode, PreDevCode, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "commodityID: " & original_CommodityID & ", commodityName: " & CommodityName & ", BPCSCommodityRef: " & BPCSCommodityRef & ", CCID: " & CCID & ", ProjectCode: " & ProjectCode & ", PreDevCode: " & PreDevCode & ", Obsolete: " & Obsolete & ", UpdatedBy: " & UpdatedBy
            HttpContext.Current.Session("BLLerror") = "UpdateCommodity : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCommodity : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function 'EOF UpdateCommodity

    ''*****
    ''* Select Commodity_Class returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCommodityClass(byval CCID As Integer,ByVal CommodityClass As String) As Commodities.Commodity_ClassDataTable

        Try
            If CCID = 0 Then
                CCID = 0
            End If
            If CommodityClass Is Nothing Then
                CommodityClass = ""

            End If

            Return Adapter2.Get_Commodity_Class(CCID, CommodityClass)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Commodity_Classification: " & CommodityClass & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityClassMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetCommodityClass

    ''*****
    ''* Insert New Commodity_Class
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCommodityClass(ByVal CommodityClass As String, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            CommodityClass = commonFunctions.convertSpecialChar(CommodityClass, False)

            ''*****
            ' Insert the Commodity record
            ''*****
            Dim rowsAffected As Integer = Adapter2.sp_Insert_Commodity_Class(CommodityClass, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Commodity_Classification: " & CommodityClass & ", createdBy: " & createdBy
            HttpContext.Current.Session("BLLerror") = "InsertCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityClassMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function 'EOF InsertCommodityClass

    ''*****
    ''* Update Commodity_Class
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCommodityClass(ByVal CommodityClass As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_CCID As Integer, ByVal Commodity_Classification As String) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Commodity_Classification = commonFunctions.convertSpecialChar(Commodity_Classification, False)

            ''*****
            ' Update the Commodity record
            ''*****
            Dim rowsAffected As Integer = Adapter2.sp_Update_Commodity_Class(original_CCID, Commodity_Classification, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CCID: " & original_CCID & ", Commodity_Classification: " & CommodityClass & ", Obsolete: " & Obsolete & ", UpdatedBy: " & UpdatedBy
            HttpContext.Current.Session("BLLerror") = "UpdateCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommoditiesBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CommodityClassMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommoditiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function 'EOF UpdateCommodityClass
End Class
