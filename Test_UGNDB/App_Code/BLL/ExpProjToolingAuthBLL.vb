''******************************************************************************************************
''* ExpProjToolingAuthBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : Roderick Carlson 10/22/2012
''*           Roderick Carlson 10/25/2012 - added UnitID to TA Material Insert and Update
''******************************************************************************************************
Imports Microsoft.VisualBasic
Imports ExpProjToolingAuthTableAdapters

Public Class ExpProjToolingAuthBLL

#Region "TA DieShop Material"

    ''*****
    ''* TA DieShop Material Adapter 
    ''*****
    Private pTADieShopMaterialAdapter As TADieShopMaterialTableAdapter = Nothing

    Protected ReadOnly Property TADieShopMaterialAdapter() As ExpProjToolingAuthTableAdapters.TADieShopMaterialTableAdapter
        Get

            If pTADieShopMaterialAdapter Is Nothing Then

                pTADieShopMaterialAdapter = New TADieShopMaterialTableAdapter

            End If

            Return pTADieShopMaterialAdapter

        End Get

    End Property

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTADSMaterial(ByVal TANo As Integer) As ExpProjToolingAuth.TADieShopMaterialDataTable

        Try

            Return TADieShopMaterialAdapter.GetTADSMaterial(TANo)

        Catch ex As Exception

            'on error, collect function dta, erroe, and last page, the redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTADSMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTADSMaterial : " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTADSMaterial(ByVal TANo As Integer, ByVal DSMaterialID As Integer, _
                                       ByVal Notes As String, ByVal Quantity As Decimal, _
                                       ByVal Cost As Decimal, ByVal UnitID As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.convertSpecialChar(Notes, False)
            End If

            Dim rowsAffected As Integer = TADieShopMaterialAdapter.InsertDSTAMaterial(TANo, DSMaterialID, Notes, Quantity, Cost, UnitID, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", DSMaterialID: " & DSMaterialID _
            & ", Notes: " & Notes _
            & ", Quantity " & Quantity _
            & ", Cost " & Cost _
            & ", UnitID " & UnitID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTADSMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTADSMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False

        End Try

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTADSMaterial(ByVal original_RowID As Integer, ByVal TANo As Integer, ByVal DSMaterialID As Integer, _
                                       ByVal Notes As String, ByVal Quantity As Decimal, _
                                       ByVal Cost As Decimal, ByVal UnitID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.convertSpecialChar(Notes, False)
            End If

            Dim rowsAffected As Integer = TADieShopMaterialAdapter.UpdateTADSMaterial(original_RowID, TANo, DSMaterialID, Notes, Quantity, Cost, UnitID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = " TANo: " & TANo _
            & ", DSMaterialID" & DSMaterialID _
            & ", Notes: " & Notes _
            & ", Quantity: " & Quantity _
            & ", Cost: " & Cost _
            & ", UnitID: " & UnitID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTADSMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBBL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTADSMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False

        End Try


    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
       Public Function DeleteTADSMaterial(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = TADieShopMaterialAdapter.DeleteTADSMaterial(original_RowID)

            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTADSMaterial: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTADSMaterial : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False

        End Try

    End Function

#End Region 'TA DieShop Material

#Region "DieShop Material Maint Table"

    ''*****
    ''* DieShop Material Maint Adapter 1
    ''*****
    Private pTADieShopMaterialMaint As TADieShopMaterialMaintTableAdapter = Nothing

    Protected ReadOnly Property TADieShopMaterialMaintAdapter() As ExpProjToolingAuthTableAdapters.TADieShopMaterialMaintTableAdapter
        Get
            If pTADieShopMaterialMaint Is Nothing Then
                pTADieShopMaterialMaint = New TADieShopMaterialMaintTableAdapter
            End If
            Return pTADieShopMaterialMaint
        End Get
    End Property

    ''*****
    ''* Select DSMaterial 
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTADieShopMaterialMaint(ByVal DSMaterialID As Integer, ByVal MaterialName As String) As ExpProjToolingAuth.TADieShopMaterialMaintDataTable
        Try

            If MaterialName Is Nothing Then

                MaterialName = ""

            End If

            Return TADieShopMaterialMaintAdapter.GetTADieShopMaterialMaint(DSMaterialID, MaterialName)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "DSMaterialID: " & DSMaterialID _
            & ", MaterialName: " & MaterialName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDSMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDSMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF Get

    ''*****
    ''* Insert DSMaterial 
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTADieShopMaterialMaint(ByVal MaterialName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            If MaterialName Is Nothing Then
                MaterialName = ""
            End If

            Dim rowsAffected As Integer = TADieShopMaterialMaintAdapter.InsertTADieShopMaterialMaint(MaterialName, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "MaterialName : " & MaterialName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTADieShopMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTADieShopMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF Insert

    ''*****
    ''* Update DSMaterial 
    ''*****

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTADieShopMaterialMaint(ByVal MaterialName As String, ByVal Obsolete As Boolean, ByVal original_DSMaterialID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If MaterialName Is Nothing Then
                MaterialName = ""
            Else
                MaterialName = commonFunctions.convertSpecialChar(MaterialName, False)
            End If

            Dim rowsAffected As Integer = TADieShopMaterialMaintAdapter.UpdateTADieShopMaterialMaint(original_DSMaterialID, MaterialName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DSMaterialID: " & original_DSMaterialID _
            & ",MaterialName: " & MaterialName _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value


            HttpContext.Current.Session("BLLerror") = "UpdateTADieShopMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTADieShopMaterialMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF Update

#End Region 'DieShop Material Maint Table

#Region "TA DieShop Labor"

    ''*****
    ''* TA DieShop Labor Adapter 4
    ''*****
    Private pTADieShopLaborAdapter As TADieShopLaborTableAdapter = Nothing

    Protected ReadOnly Property TADieShopLaborAdapter() As ExpProjToolingAuthTableAdapters.TADieShopLaborTableAdapter
        Get

            If pTADieShopLaborAdapter Is Nothing Then

                pTADieShopLaborAdapter = New TADieShopLaborTableAdapter

            End If

            Return pTADieShopLaborAdapter

        End Get

    End Property

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTADSLabor(ByVal TANo As Integer) As ExpProjToolingAuth.TADieShopLaborDataTable

        Try

            Return TADieShopLaborAdapter.GetTADSLabor(TANo)

        Catch ex As Exception

            'on error, collect function dta, erroe, and last page, the redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTADSLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTADSLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTADSLabor(ByVal TANo As Integer, ByVal DSLaborID As Integer, ByVal NumberHours As Decimal, _
                                    ByVal Notes As String, ByVal Cost As Decimal) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.convertSpecialChar(Notes, False)
            End If

            Dim rowsAffected As Integer = TADieShopLaborAdapter.InsertTADSLabor(TANo, DSLaborID, NumberHours, Notes, Cost, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1


        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = "TANo: " & TANo _
            & ", DSLaborID: " & DSLaborID _
            & ", NumberHours: " & NumberHours _
            & ", Notes: " & Notes _
            & ", Cost: " & Cost _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTADSLabor: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTADSLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False

        End Try

    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTADSLabor(ByVal original_RowID As Integer, ByVal TANo As Integer, ByVal DSLaborID As Integer, ByVal NumberHours As Decimal, _
                                                   ByVal Notes As String, ByVal Cost As Decimal) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Notes Is Nothing Then
                Notes = ""
            Else
                Notes = commonFunctions.convertSpecialChar(Notes, False)
            End If

            Dim rowsAffected As Integer = TADieShopLaborAdapter.UpdateTADSLabor(original_RowID, TANo, DSLaborID, NumberHours, Notes, Cost, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = " TANo: " & TANo _
            & ", DSLaborID" & DSLaborID _
            & ", NumberHours " & NumberHours _
            & ", Notes: " & Notes _
            & ", Cost " & Cost _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTADSLabor: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBBL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTADSLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False

        End Try


    End Function

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
       Public Function DeleteTADSLabor(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = TADieShopLaborAdapter.DeleteTADSLabor(original_RowID)

            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page

            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTADSLabor: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTADSLabor : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False

        End Try

    End Function

#End Region   'TA DieShop Labor

#Region "DieShop Labor Maint Table"

    Private pTADieShopLaborMaintAdapter As TADieShopLaborMaintTableAdapter = Nothing
    ''*****
    ''* DieShop Labor Maint Adapter 2
    ''*****
    Protected ReadOnly Property TADieShopLaborMaintAdapter() As ExpProjToolingAuthTableAdapters.TADieShopLaborMaintTableAdapter
        Get
            If pTADieShopLaborMaintAdapter Is Nothing Then
                pTADieShopLaborMaintAdapter = New TADieShopLaborMaintTableAdapter
            End If
            Return pTADieShopLaborMaintAdapter
        End Get
    End Property

    ''*****
    ''* Select DSLabor 
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTADieShopLaborMaint(ByVal DSLaborID As Integer, ByVal LaborName As String) As ExpProjToolingAuth.TADieShopLaborMaintDataTable
        Try

            If LaborName Is Nothing Then

                LaborName = ""

            End If

            Return TADieShopLaborMaintAdapter.GetTADieShopLaborMaint(DSLaborID, LaborName)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "DSLaborID: " & DSLaborID _
            & ",LaborName : " & LaborName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF Get

    ''*****
    ''* Insert DSLabor 
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTADieShopLaborMaint(ByVal LaborName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If LaborName Is Nothing Then
                LaborName = ""

            Else
                LaborName = commonFunctions.convertSpecialChar(LaborName, False)
            End If

            Dim rowsAffected As Integer = TADieShopLaborMaintAdapter.InsertTADieShopLaborMaint(LaborName, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "LaborName : " & LaborName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Insert

    ''*****
    ''* Update DSLabor 
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTADieShopLaborMaint(ByVal LaborName As String, ByVal Obsolete As Boolean, ByVal original_DSLaborID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If LaborName Is Nothing Then
                LaborName = ""
            Else
                LaborName = commonFunctions.convertSpecialChar(LaborName, False)
            End If

            Dim rowsAffected As Integer = TADieShopLaborMaintAdapter.UpdateTADieShopLaborMaint(original_DSLaborID, LaborName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DSLaborID: " & original_DSLaborID _
            & ",LaborName: " & LaborName _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> TADieShopBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTADieShopLaborMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "TADieShopBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF Update

#End Region 'DieShop Labor Maint Table

#Region "Change Type Maint"

    Private pTAChangeTypeMaintAdapter As TAChangeTypeMaintTableAdapter = Nothing
    Protected ReadOnly Property TAChangeTypeMaintAdapter() As ExpProjToolingAuthTableAdapters.TAChangeTypeMaintTableAdapter
        Get
            If pTAChangeTypeMaintAdapter Is Nothing Then
                pTAChangeTypeMaintAdapter = New TAChangeTypeMaintTableAdapter
            End If
            Return pTAChangeTypeMaintAdapter
        End Get
    End Property

    ''*****
    ''* Select ToolingAuthorizationTask returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTAChangeTypeMaint(ByVal ChangeTypeID As Integer, ByVal ChangeTypeName As String) As ExpProjToolingAuth.TAChangeTypeMaintDataTable
        Try
            If ChangeTypeName Is Nothing Then
                ChangeTypeName = ""
            End If

            Return TAChangeTypeMaintAdapter.GetTAChangeTypeMaint(ChangeTypeID, ChangeTypeName)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChangeTypeID: " & ChangeTypeID _
            & ",ChangeTypeName: " & ChangeTypeName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetToolingAuthorizationChangeType


    ''*****
    ''* Insert ToolingAuthorizationChangeType returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
 Public Function InsertTAChangeTypeMaint(ByVal ChangeTypeName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            If ChangeTypeName Is Nothing Then
                ChangeTypeName = ""
            End If

            Dim rowsAffected As Integer = TAChangeTypeMaintAdapter.InsertTAChangeTypeMaint(ChangeTypeName, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChangeTypeName: " & ChangeTypeName _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF InsertToolingAuthorizationChangeType

    ''*****
    ''* Update ToolingAuthorizationChangeType returning all rows
    ''*****

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTAChangeTypeMaint(ByVal ChangeTypeName As String, ByVal Obsolete As Boolean, ByVal original_ChangeTypeID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If ChangeTypeName Is Nothing Then
                ChangeTypeName = ""
            End If

            Dim rowsAffected As Integer = TAChangeTypeMaintAdapter.UpdateTAChangeTypeMaint(original_ChangeTypeID, ChangeTypeName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChangeTypeID: " & original_ChangeTypeID _
            & ",ChangeTypeName: " & ChangeTypeName _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTAChangeTypeMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)
            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF UpdateToolingAuthorizationChangeType

#End Region 'Change Type Maint

#Region "TA RSS"
    Private pTARSSAdapter As TARSSTableAdapter = Nothing
    Protected ReadOnly Property TARSSAdapter() As ExpProjToolingAuthTableAdapters.TARSSTableAdapter
        Get
            If pTARSSAdapter Is Nothing Then
                pTARSSAdapter = New TARSSTableAdapter
            End If
            Return pTARSSAdapter
        End Get
    End Property
    ''*****
    ''* Select ExpProjToolingAuthorizationRSSBLL returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTARSS(ByVal TANo As Integer, ByVal RSSID As Integer) As ExpProjToolingAuth.TARSSDataTable

        Try
            Return TARSSAdapter.GetTARSS(TANo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTARSS: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTARSS: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetToolingAuthorizationRSS
#End Region 'TA RSS

#Region "TA RSS Reply"

    Private pTARSSReplyAdapter As TARSSReplyTableAdapter

    Protected ReadOnly Property TARSSReplyAdapter() As ExpProjToolingAuthTableAdapters.TARSSReplyTableAdapter
        Get
            If pTARSSReplyAdapter Is Nothing Then
                pTARSSReplyAdapter = New TARSSReplyTableAdapter()
            End If
            Return pTARSSReplyAdapter
        End Get
    End Property
    ''*****
    ''* Select ExpProjToolingAuthorizationRSSReplyBLL returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTARSSReply(ByVal TANo As Integer, ByVal RSSID As Integer) As ExpProjToolingAuth.TARSSReplyDataTable

        Try
            Return TARSSReplyAdapter.GetTARSSReply(TANo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetARSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetARSSReply: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetToolingAuthorizationRSSReply

#End Region 'TA RSS Reply

#Region "TA Team Member Task"
    Private pTATaskAdapter As TATaskTableAdapter

    Protected ReadOnly Property TATaskAdapter() As ExpProjToolingAuthTableAdapters.TATaskTableAdapter
        Get
            If pTATaskAdapter Is Nothing Then
                pTATaskAdapter = New TATaskTableAdapter
            End If
            Return pTATaskAdapter
        End Get
    End Property
    ''*****
    ''* Select all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTATask(ByVal TANo As Integer) As ExpProjToolingAuth.TATaskDataTable

        Try
            Return TATaskAdapter.GetTATask(TANo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo: " & TANo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTATask: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"

            UGNErrorTrapping.InsertErrorLog("GetTATask: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Get

    ''*****
    ''* Insert New
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTATask(ByVal TANo As Integer, ByVal TaskID As Integer, _
            ByVal TeamMemberID As Integer, ByVal TargetDate As String) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = TATaskAdapter.InsertTATask(TANo, TaskID, TeamMemberID, TargetDate, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TANo:" & TANo _
            & ", TaskID:" & TaskID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", TargetDate:" & TargetDate _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTATask: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTATask: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Update 
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateTATask(ByVal original_RowID As Integer, _
            ByVal TANo As Integer, ByVal TaskID As Integer, ByVal TeamMemberID As Integer, _
            ByVal NotificationDate As String, ByVal TargetDate As String, ByVal CompletionDate As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = TATaskAdapter.UpdateTATask(original_RowID, TANo, TaskID, TeamMemberID, NotificationDate, TargetDate, CompletionDate, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID _
            & ", TANo:" & TANo _
            & ", TaskID:" & TaskID _
            & ", TeamMemberID:" & TeamMemberID _
            & ", NotificationDate:" & NotificationDate _
            & ", TargetDate:" & TargetDate _
            & ", CompletionDate:" & CompletionDate _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTATask : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTATask : " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete 
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteTATask(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = TATaskAdapter.DeleteTATask(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTATask: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTATask: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
#End Region 'TA Team Member Task

#Region "Task Maint"

    Private pTATaskMaintAdapter As TATaskMaintTableAdapter = Nothing
    Protected ReadOnly Property TATaskMaintAdapter() As ExpProjToolingAuthTableAdapters.TATaskMaintTableAdapter
        Get
            If pTATaskMaintAdapter Is Nothing Then
                pTATaskMaintAdapter = New TATaskMaintTableAdapter

            End If
            Return pTATaskMaintAdapter
        End Get
    End Property
    ''*****
    ''* Select ToolingAuthorizationTask returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTATaskMaint(ByVal TaskID As Integer, ByVal TaskName As String) As ExpProjToolingAuth.TATaskMaintDataTable

        Try

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Return TATaskMaintAdapter.GetTATaskMaint(TaskID, TaskName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID: " & TaskID _
            & ",TaskName: " & TaskName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetToolingAuthTaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetToolingAuthTaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetToolingAuthorizationTask

    ''*****
    ''* Insert ToolingAuthorizationTask returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertTATaskMaint(ByVal TaskName As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Dim rowsAffected As Integer = TATaskMaintAdapter.InsertTATaskMaint(TaskName, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskName: " & TaskName _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTATaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertTATaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF InsertToolingAuthorizationTask
    ''*****
    ''* Update ToolingAuthorizationTask returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateTATaskMaint(ByVal TaskName As String, ByVal Obsolete As Boolean, ByVal original_TaskID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TaskName Is Nothing Then
                TaskName = ""
            End If

            Dim rowsAffected As Integer = TATaskMaintAdapter.UpdateTATaskMaint(original_TaskID, TaskName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TaskID: " & original_TaskID _
            & ",TaskName: " & TaskName _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateTATaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ExpProjToolingAuthBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateTATaskMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "ExpProjToolingAuthBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF UpdateToolingAuthorizationTask

#End Region 'Task Maint
End Class
