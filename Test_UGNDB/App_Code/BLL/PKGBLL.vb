''******************************************************************************************************
''* PKGBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update.
''*
''* Author  : Steven Howard 09/06/2012
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports PKGTableAdapters

<System.ComponentModel.DataObject()> _
Public Class PKGBLL
#Region "Adapters"
    Private pAdapter1 As InstructionMaintTableAdapter = Nothing
    Private pAdapter2 As EquipmentMaintTableAdapter = Nothing
    Private pAdapter3 As ColorMaintTableAdapter = Nothing
    Private pAdapter4 As PKGContainerCustomerTableAdapter = Nothing
    Private pAdapter5 As PKGContainerSupplierTableAdapter = Nothing
    Private pAdapter6 As PKGLayoutSearchTableAdapter = Nothing
    Private pAdapter7 As PKGLayoutPartNoTableAdapter = Nothing
    Private pAdapter8 As PKGLayoutInstructionTableAdapter = Nothing
    Private pAdapter9 As PKGLayoutCustomerTableAdapter = Nothing
    Private pAdapter10 As PKGContainerTableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As PKGTableAdapters.InstructionMaintTableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New InstructionMaintTableAdapter
            End If
            Return pAdapter1
        End Get
    End Property 'EOF InstructionMaintTableAdapter
    Protected ReadOnly Property Adapter2() As PKGTableAdapters.EquipmentMaintTableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New EquipmentMaintTableAdapter
            End If
            Return pAdapter2
        End Get
    End Property 'EOF EquipmentMaintTableAdapter
    Protected ReadOnly Property Adapter3() As PKGTableAdapters.ColorMaintTableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New ColorMaintTableAdapter
            End If
            Return pAdapter3
        End Get
    End Property 'EOF ColorMaintTableAdapter
    Protected ReadOnly Property Adapter4() As PKGTableAdapters.PKGContainerCustomerTableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New PKGContainerCustomerTableAdapter
            End If
            Return pAdapter4
        End Get
    End Property 'EOF PKGContainerCustomerTableAdapter
    Protected ReadOnly Property Adapter5() As PKGTableAdapters.PKGContainerSupplierTableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New PKGContainerSupplierTableAdapter
            End If
            Return pAdapter5
        End Get
    End Property 'EOF PKGContainerSupplierTableAdapter
    Protected ReadOnly Property Adapter6() As PKGTableAdapters.PKGLayoutSearchTableAdapter
        Get
            If pAdapter6 Is Nothing Then
                pAdapter6 = New PKGLayoutSearchTableAdapter
            End If
            Return pAdapter6
        End Get
    End Property 'EOF PKGLayoutSearchTableAdapter
    Protected ReadOnly Property Adapter7() As PKGTableAdapters.PKGLayoutPartNoTableAdapter
        Get
            If pAdapter7 Is Nothing Then
                pAdapter7 = New PKGLayoutPartNoTableAdapter
            End If
            Return pAdapter7
        End Get
    End Property 'EOF PKGLayoutPartNoTableAdapter
    Protected ReadOnly Property Adapter8() As PKGTableAdapters.PKGLayoutInstructionTableAdapter
        Get
            If pAdapter8 Is Nothing Then
                pAdapter8 = New PKGLayoutInstructionTableAdapter
            End If
            Return pAdapter8
        End Get
    End Property 'EOF PKGLayoutInstructionTableAdapter
    Protected ReadOnly Property Adapter9() As PKGTableAdapters.PKGLayoutCustomerTableAdapter
        Get
            If pAdapter9 Is Nothing Then
                pAdapter9 = New PKGLayoutCustomerTableAdapter
            End If
            Return pAdapter9
        End Get
    End Property 'EOF PKGLayoutCustomerTableAdapter
    Protected ReadOnly Property Adapter10() As PKGTableAdapters.PKGContainerTableAdapter
        Get
            If pAdapter10 Is Nothing Then
                pAdapter10 = New PKGContainerTableAdapter
            End If
            Return pAdapter10
        End Get
    End Property 'EOF PKGContainerTableAdapter

#End Region

#Region "I/E/C Maint"

#Region "Instruction Maint"

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetInstruMaint(ByVal IID As Integer, ByVal Instruction As String) As PKG.InstructionMaintDataTable
        Try

            If Instruction Is Nothing Then
                Instruction = ""
            Else
                Instruction = commonFunctions.convertSpecialChar(Instruction, False)
            End If

            Return Adapter1.GetInstructionMaint(IID, Instruction)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "IID: " & IID _
            & ", Instruction: " & Instruction _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF Get Instruction Maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertInstruMaint(ByVal Instruction As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Instruction Is Nothing Then
                Instruction = ""
            Else
                Instruction = commonFunctions.convertSpecialChar(Instruction, False)
            End If

            Dim rowsAffected As Integer = Adapter1.InsertInstructionMaint(Instruction, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "Instruction: " & Instruction _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF Insert Instruction Maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateInstruMaint(ByVal Instruction As String, ByVal Obsolete As Boolean, ByVal original_IID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If Instruction Is Nothing Then
                Instruction = ""
            Else
                Instruction = commonFunctions.convertSpecialChar(Instruction, False)
            End If

            Dim rowsAffected As Integer = Adapter1.UpdateInstructionMaint(original_IID, Instruction, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "IID: " & original_IID _
            & ",Instruction: " & Instruction _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateInstructionMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF Update Instruction Maint

#End Region

#Region "Equipment Maint"

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
Public Function GetEquipMaint(ByVal EQPTID As Integer, ByVal EquipmentDesc As String) As PKG.EquipmentMaintDataTable
        Try

            If EquipmentDesc Is Nothing Then
                EquipmentDesc = ""
            Else
                EquipmentDesc = commonFunctions.convertSpecialChar(EquipmentDesc, False)
            End If

            Return Adapter2.GetEquipmentMaint(EQPTID, EquipmentDesc)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "EQPTID: " & EQPTID _
            & ", EquipmentDesc: " & EquipmentDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF Get Equipment Maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertEquipMaint(ByVal EquipmentDesc As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If EquipmentDesc Is Nothing Then
                EquipmentDesc = ""
            Else
                EquipmentDesc = commonFunctions.convertSpecialChar(EquipmentDesc, False)
            End If

            Dim rowsAffected As Integer = Adapter2.InsertEquipmentMaint(EquipmentDesc, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "EquipmentDesc: " & EquipmentDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF Insert Equipment Maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateEquipMaint(ByVal EquipmentDesc As String, ByVal Obsolete As Boolean, ByVal original_EQPTID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If EquipmentDesc Is Nothing Then
                EquipmentDesc = ""
            Else
                EquipmentDesc = commonFunctions.convertSpecialChar(EquipmentDesc, False)
            End If

            Dim rowsAffected As Integer = Adapter2.UpdateEquipmentMaint(original_EQPTID, EquipmentDesc, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "EQPTID: " & original_EQPTID _
            & ",EquipmentDesc: " & EquipmentDesc _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateEquipmentMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF Update Equipment Maint

#End Region

#Region "Color Maint"

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
Public Function GetColorMaint(ByVal CCode As String, ByVal Color As String) As PKG.ColorMaintDataTable
        Try
            If CCode Is Nothing Then

                CCode = ""
            Else
                CCode = commonFunctions.convertSpecialChar(CCode, False)

            End If

            If Color Is Nothing Then
                Color = ""
            Else
                Color = commonFunctions.convertSpecialChar(Color, False)
            End If

            Return Adapter3.GetColorMaint(CCode, Color)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CCode: " & CCode _
            & ", Color: " & Color _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GET Color maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertColorMaint(ByVal CCode As String, ByVal Color As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If CCode Is Nothing Then
                CCode = ""
            Else
                CCode = commonFunctions.convertSpecialChar(CCode, False)
            End If

            If Color Is Nothing Then
                Color = ""
            Else
                Color = commonFunctions.convertSpecialChar(Color, False)
            End If

            Dim rowsAffected As Integer = Adapter3.InsertColorMaint(CCode, Color, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CCode: " & CCode _
            & ", Color: " & Color _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF Insert Color maint
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateColorMaint(ByVal Color As String, ByVal Obsolete As Boolean, ByVal original_CCode As String, ByVal CCode As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If CCode Is Nothing Then
                CCode = ""
            Else
                CCode = commonFunctions.convertSpecialChar(CCode, False)
            End If

            If Color Is Nothing Then
                Color = ""
            Else
                Color = commonFunctions.convertSpecialChar(Color, False)
            End If

            Dim rowsAffected As Integer = Adapter3.UpdateColorMaint(original_CCode, Color, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CCode: " & original_CCode _
            & ",Color: " & Color _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateColorMaint: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF Update Color maint

#End Region


#End Region

#Region "Container"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
Public Function GetPKGContainer(ByVal CID As Integer, ByVal ContainerNo As String, ByVal Description As String, ByVal Type As String, ByVal OEM As String, ByVal Customer As String, ByVal Vendor As Integer) As PKG.PKGContainerDataTable

        Try
            If ContainerNo = Nothing Then ContainerNo = ""

            If Type = Nothing Then
                Type = ""
            Else
                Type = commonFunctions.convertSpecialChar(Type, False)
            End If
            If Description = Nothing Then
                Description = ""
            Else
                Description = commonFunctions.convertSpecialChar(Description, False)
            End If

            If OEM = Nothing Then OEM = ""
            If Customer = Nothing Then
                Customer = ""
            Else
                Customer = commonFunctions.replaceSpecialChar(Customer, False)
            End If

            Return Adapter10.GetPKGContainer(CID, ContainerNo, Description, Type, OEM, Customer, Vendor)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGContainer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGContainer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF GetPKGContainer
#Region "Container Customer"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
 Public Function GetPKGContainerCustomer(ByVal CID As Integer) As PKG.PKGContainerCustomerDataTable

        Try

            Return Adapter4.GetPKGContainerCustomer(CID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF GetPKGContainerCustomer

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPKGContainerCustomer(ByVal CID As Integer, ByVal Customer As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Customer = commonFunctions.replaceSpecialChar(Customer, False)

            Dim rowsAffected As Integer = Adapter4.InsertPKGContainerCustomer(CID, Customer, CreatedBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & CID _
            & "Customer :" & Customer _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertPKGContainerCustomer

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
 Public Function UpdatePKGContainerCustomer(ByVal original_Customer As String, ByVal original_CID As Integer, ByVal Customer As String) As Boolean
        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Customer = commonFunctions.replaceSpecialChar(Customer, False)
            original_Customer = commonFunctions.replaceSpecialChar(original_Customer, False)

            Dim rowsAffected As Integer = Adapter4.UpdatePKGContainerCustomer(original_CID, Customer, original_Customer, UpdatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & original_CID _
            & ", Customer :" & original_Customer _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing
        End Try

    End Function 'EOF Upadte Container Customer

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
 Public Function DeletePKGContainerCustomer(ByVal CID As Integer, ByVal Customer As String, ByVal original_CID As Integer, ByVal original_Customer As String) As Boolean

        Try
            original_Customer = commonFunctions.replaceSpecialChar(original_Customer, False)

            Dim rowsAffected As Integer = Adapter4.DeletePKGContainerCustomer(original_CID, original_Customer)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID:" & original_CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePKGContainerCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)
            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF DeletePKGContainerCustomer

#End Region

#Region "Container Supplier"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPKGContainerSupplier(ByVal CID As Integer, ByVal VendorNo As Integer) As PKG.PKGContainerSupplierDataTable
        Try

            Return Adapter5.GetPKGContainerSupplier(CID, VendorNo)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & CID _
            & ", VendorNo: " & VendorNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetPKGContainerSupplier

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPKGContainerSupplier(ByVal CID As Integer, ByVal VendorNo As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter5.InsertPKGContainerSupplier(CID, VendorNo, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & CID _
            & "VendorNo :" & VendorNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertPKGContainerSupplier

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
 Public Function UpdatePKGContainerSupplier(ByVal original_VendorNo As Integer, ByVal VendorNo As Integer, ByVal original_CID As Integer) As Boolean
        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter5.UpdatePKGContainerSupplier(original_CID, VendorNo, original_VendorNo, UpdatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "CID: " & original_CID _
            & ", VendorNo :" & original_VendorNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing
        End Try

    End Function 'EOF UpdatePKGContainerSupplier

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
 Public Function DeletePKGContainerSupplier(ByVal CID As Integer, ByVal VendorNo As Integer, ByVal original_VendorNo As Integer, ByVal original_CID As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter5.DeletePKGContainerSupplier(original_CID, original_VendorNo)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID:" & CID _
            & ", VendorNo:" & original_VendorNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePKGContainerSupplier: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF DeletePKGContainerSupplier

#End Region

#End Region

#Region "Packaging Layout "

#Region "Packaging Layout Search"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPKGLayoutSearch(ByVal PKGID As String, ByVal LayoutDesc As String, ByVal ContainerNo As String, ByVal OEMManufacturer As String, ByVal Make As String, ByVal Model As String, ByVal UGNFacility As String, ByVal DepartmentID As Integer, ByVal WorkCenter As Integer, ByVal Customer As String, ByVal PartNo As String) As PKG.PKGLayoutSearchDataTable
        Try
            If PKGID = Nothing Then PKGID = ""

            If LayoutDesc = Nothing Then LayoutDesc = ""

            If ContainerNo = Nothing Then ContainerNo = ""

            If PartNo = Nothing Then PartNo = ""

            If UGNFacility = Nothing Then UGNFacility = ""

            If OEMManufacturer = Nothing Then OEMManufacturer = ""

            If Make = Nothing Then Make = ""

            If Model = Nothing Then Model = ""

            If Customer = Nothing Then
                Customer = ""
            Else
                Customer = commonFunctions.replaceSpecialChar(Customer, False)
            End If

            Return Adapter6.GetPKGLayoutSearch(PKGID, LayoutDesc, ContainerNo, OEMManufacturer, Make, Model, UGNFacility, DepartmentID, WorkCenter, Customer, PartNo)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & ", ContainerNo: " & ContainerNo _
            & ", LayoutDesc :" & LayoutDesc _
            & ", PartNo :" & PartNo _
            & ", OEMManufacturer :" & OEMManufacturer _
            & ", UGNFacility :" & UGNFacility _
            & ", DepartmentID :" & DepartmentID _
            & ", WorkCenter :" & WorkCenter _
            & ", Make :" & Make _
            & ", Model :" & Model _
            & ", Customer :" & Customer _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGLayoutSearch: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGLayoutSearch: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF GetPKGLayoutSearch

#End Region

#Region "Packaging Layout PartNo"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPKGLayoutPartNo(ByVal PKGID As Integer) As PKG.PKGLayoutPartNoDataTable

        Try

            Return Adapter7.GetPKGLayoutPartNo(PKGID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

        Return Nothing

    End Function 'EOF GetPKGLayoutPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPKGLayoutPartNo(ByVal PKGID As Integer, ByVal PartNo As String, ByVal QtyPckd As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter7.InsertPKGLayoutPartNo(PKGID, PartNo, QtyPckd, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & "PartNo :" & PartNo _
            & "QtyPckd :" & QtyPckd _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertPKGLayoutPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
 Public Function UpdatePKGLayoutPartNo(ByVal original_PKGID As Integer, ByVal original_PartNo As String, ByVal QtyPckd As Integer) As Boolean
        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter7.UpdatePKGLayoutPartNo(original_PKGID, original_PartNo, QtyPckd, UpdatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & original_PKGID _
            & "PartNo :" & original_PartNo _
            & "QtyPckd :" & QtyPckd _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing
        End Try

    End Function 'EOF UpdatePKGLayoutPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
 Public Function DeletePKGLayoutPartNo(ByVal PKGID As Integer, ByVal PartNo As String, ByVal original_PKGID As Integer, ByVal original_PartNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter7.DeletePKGLayoutPartNo(original_PKGID, original_PartNo)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PKGID:" & original_PKGID _
            & ", PartNo:" & original_PartNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePKGLayoutPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF DeletePKGLayoutPartNo

#End Region

#Region "Packaging Layout Instruction"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPKGLayoutInstruction(ByVal PKGID As Integer) As PKG.PKGLayoutInstructionDataTable

        Try

            Return Adapter8.GetPKGLayoutInstruction(PKGID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

        Return Nothing

    End Function 'EOF GetPKGLayoutInstruction

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPKGLayoutInstruction(ByVal PKGID As Integer, ByVal SeqNo As Integer, ByVal IID As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter8.InsertPKGLayoutInstruction(PKGID, IID, SeqNo, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & "IID :" & IID _
            & "SeqNo :" & SeqNo _
            & ", Createdby: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertPKGLayoutInstruction

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
 Public Function UpdatePKGLayoutInstruction(ByVal original_PKGID As Integer, ByVal original_SeqID As Integer, ByVal SeqNo As Integer, ByVal IID As Integer) As Boolean
        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter8.UpdatePKGLayoutInstruction(original_PKGID, original_SeqID, SeqNo, IID, UpdatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & original_PKGID _
            & "SeqID :" & original_SeqID _
            & "SeqNo :" & SeqNo _
            & "IID :" & IID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing
        End Try

    End Function 'EOF UpdatePKGLayoutInstruction

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
 Public Function DeletePKGLayoutInstruction(ByVal PKGID As Integer, ByVal SeqID As String, ByVal original_PKGID As Integer, ByVal original_SeqID As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter8.DeletePKGLayoutInstruction(original_PKGID, original_SeqID)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PKGID:" & original_PKGID _
            & ", SeqID:" & original_SeqID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePKGLayoutInstruction: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF DeletePKGLayoutInstruction

#End Region

#Region "Layout Customer"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
 Public Function GetPKGLayoutCustomer(ByVal PKGID As Integer, ByVal CID As Integer) As PKG.PKGLayoutCustomerDataTable

        Try

            Return Adapter9.GetPKGLayoutCustomer(PKGID, CID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID: " & PKGID _
            & "CID: " & CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF GetPKGContainerCustomer

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertPKGLayoutCustomer(ByVal PKGID As Integer, ByVal CID As Integer, ByVal Customer As String) As Boolean

        Try
            If Customer = Nothing Then
                Customer = ""
            Else
                Customer = commonFunctions.replaceSpecialChar(Customer, False)
            End If

            Dim rowsAffected As Integer = Adapter9.InsertPKGLayoutCustomer(PKGID, CID, Customer)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "PKGID" & PKGID _
            & "CID: " & CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertPKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertPKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertPKGLayoutCustomer

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
 Public Function DeletePKGLayoutCustomer(ByVal PKGID As Integer, ByVal CID As Integer, ByVal Customer As String, ByVal SoldTo As Integer, ByVal original_PKGID As Integer, ByVal original_CID As Integer, ByVal original_Customer As Integer) As Boolean

        Try
            If original_Customer = Nothing Then
                original_Customer = ""
            Else
                original_Customer = commonFunctions.replaceSpecialChar(original_Customer, False)
            End If

            Dim rowsAffected As Integer = Adapter9.DeletePKGLayoutCustomer(original_PKGID, original_CID, original_Customer)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID:" & original_CID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PKGBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePKGLayoutCustomer: " & commonFunctions.convertSpecialChar(ex.Message, False), "PKGBLL.vb", strUserEditedData)
            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF DeletePKGLayoutCustomer

#End Region

#End Region

End Class
