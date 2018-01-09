''******************************************************************************************************
''* PGMBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey  01/25/2013
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports PGMTableAdapters

Public Class PGMBLL
#Region "Adapters"
    Private pAdapter1 As SampleTrialEvent_TableAdapter = Nothing
    Private pAdapter2 As SampleMtrlReq_PartNo_TableAdapter = Nothing
    Private pAdapter3 As SampleMtrlReq_Documents_TableAdapter = Nothing
    Private pAdapter4 As SampleMtrlReq_Shipping_TableAdapter = Nothing
    Private pAdapter5 As SampleMtrlReq_Approval_TableAdapter = Nothing
    Private pAdapter6 As SampleMtrlReq_RSS_TableAdapter = Nothing
    Private pAdapter7 As SampleMtrlReq_RSS_Reply_TableAdapter = Nothing
    Private pAdapter8 As SampleMtrlReqTableAdapter = Nothing

    Protected ReadOnly Property Adapter1() As PGMTableAdapters.SampleTrialEvent_TableAdapter
        Get
            If pAdapter1 Is Nothing Then
                pAdapter1 = New SampleTrialEvent_TableAdapter
            End If
            Return pAdapter1
        End Get
    End Property 'EOF SampleTrialEvent_TableAdapter

    Protected ReadOnly Property Adapter2() As PGMTableAdapters.SampleMtrlReq_PartNo_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New SampleMtrlReq_PartNo_TableAdapter
            End If
            Return pAdapter2
        End Get
    End Property 'EOF SampleMtrlReq_PartNo_TableAdapter

    Protected ReadOnly Property Adapter3() As PGMTableAdapters.SampleMtrlReq_Documents_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New SampleMtrlReq_Documents_TableAdapter
            End If
            Return pAdapter3
        End Get
    End Property 'EOF SampleMtrlReq_Documents_TableAdapter

    Protected ReadOnly Property Adapter4() As PGMTableAdapters.SampleMtrlReq_Shipping_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New SampleMtrlReq_Shipping_TableAdapter
            End If
            Return pAdapter4
        End Get
    End Property 'EOF SampleMtrlReq_Documents_TableAdapter

    Protected ReadOnly Property Adapter5() As PGMTableAdapters.SampleMtrlReq_Approval_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New SampleMtrlReq_Approval_TableAdapter
            End If
            Return pAdapter5
        End Get
    End Property 'EOF SampleMtrlReq_Approval_TableAdapter

    Protected ReadOnly Property Adapter6() As PGMTableAdapters.SampleMtrlReq_RSS_TableAdapter
        Get
            If pAdapter6 Is Nothing Then
                pAdapter6 = New SampleMtrlReq_RSS_TableAdapter
            End If
            Return pAdapter6
        End Get
    End Property 'EOF SampleMtrlReq_RSS_TableAdapter

    Protected ReadOnly Property Adapter7() As PGMTableAdapters.SampleMtrlReq_RSS_Reply_TableAdapter
        Get
            If pAdapter7 Is Nothing Then
                pAdapter7 = New SampleMtrlReq_RSS_Reply_TableAdapter
            End If
            Return pAdapter7
        End Get
    End Property 'EOF SampleMtrlReq_RSS_Reply_TableAdapter

    Protected ReadOnly Property Adapter8() As PGMTableAdapters.SampleMtrlReqTableAdapter
        Get
            If pAdapter8 Is Nothing Then
                pAdapter8 = New SampleMtrlReqTableAdapter
            End If
            Return pAdapter8
        End Get
    End Property 'EOF SampleMtrlReqTableAdapter

#End Region 'EOF "Adapters"

#Region "Trial Event"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
   Public Function GetSampleTrialEvent(ByVal TrialEvent As String, ByVal OEMMfg As String) As PGM.SampleTrialEventDataTable
        Try

            If OEMMfg Is Nothing Then OEMMfg = ""

            If TrialEvent Is Nothing Then
                TrialEvent = ""
            Else
                TrialEvent = commonFunctions.replaceSpecialChar(TrialEvent, False)
            End If

            Return Adapter1.Get_Sample_TrialEvent(TrialEvent, OEMMfg)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "TrialEvent: " & TrialEvent _
            & ", OEMMfg: " & OEMMfg _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleTrialEvent

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
   Public Function InsertSampleTrialEvent(ByVal TrialEvent As String, ByVal OEMManufacturer As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TrialEvent Is Nothing Then
                TrialEvent = ""
            Else
                TrialEvent = commonFunctions.replaceSpecialChar(TrialEvent, False)
            End If

            If OEMManufacturer Is Nothing Then OEMManufacturer = ""

            Dim rowsAffected As Integer = Adapter1.sp_Insert_SampleTrialEvent(TrialEvent, OEMManufacturer, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "TrialEvent: " & TrialEvent _
            & ",OEMManufacturer: " & OEMManufacturer _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertSampleTrialEvent

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSampleTrialEvent(ByVal TrialEvent As String, ByVal Obsolete As Boolean, ByVal original_TEID As Integer, ByVal original_TrialEvent As String, ByVal OEMManufacturer As String, ByVal original_OEMManufacturer As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If TrialEvent Is Nothing Then
                TrialEvent = ""
            Else
                TrialEvent = commonFunctions.replaceSpecialChar(TrialEvent, False)
            End If

            If OEMManufacturer Is Nothing Then OEMManufacturer = ""

            Dim rowsAffected As Integer = Adapter1.sp_Update_SampleTrialEvent(original_TEID, TrialEvent, OEMManufacturer, Obsolete, original_TrialEvent, original_OEMManufacturer, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TEID: " & original_TEID _
            & ",TrialEvent: " & TrialEvent _
            & ",OEMManufacturer: " & OEMManufacturer _
            & ",Obsolete: " & Obsolete _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSampleTrialEvent: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF UpdateSampleTrialEvent
#End Region 'EOF "Trial Event"

#Region "SampleMtrlReq_PartNo"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
   Public Function GetSampleMtrlReqPartNo(ByVal SMRNo As Integer, ByVal RowID As Integer) As PGM.SampleMtrlReq_PartNoDataTable
        Try

            If SMRNo = Nothing Then SMRNo = 0
            If RowID = Nothing Then RowID = 0

            Return Adapter2.Get_SampleMtrlReq_PartNo(SMRNo, RowID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleMtrlReqPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
   Public Function InsertSampleMtrlReqPartNo(ByVal SMRNo As Integer, ByVal PartNo As String, ByVal DesignLevel As String, ByVal SizeThickness As String, ByVal Qty As Decimal, ByVal Price As Decimal, ByVal RecoveryAmt As Decimal, ByVal PONo As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SMRNo = Nothing Then SMRNo = 0
            If PartNo Is Nothing Then PartNo = ""

            If DesignLevel Is Nothing Then
                DesignLevel = ""
            Else
                DesignLevel = commonFunctions.replaceSpecialChar(DesignLevel, False)
            End If

            If SizeThickness Is Nothing Then
                SizeThickness = ""
            Else
                SizeThickness = commonFunctions.replaceSpecialChar(SizeThickness, False)
            End If

            If Qty = Nothing Then Qty = 0
            If Price = Nothing Then Price = 0
            If RecoveryAmt = Nothing Then RecoveryAmt = 0
            If PONo Is Nothing Then PONo = ""

            Dim rowsAffected As Integer = Adapter2.sp_Insert_SampleMtrlReq_PartNo(SMRNo, PartNo, DesignLevel, SizeThickness, Qty, Price, RecoveryAmt, PONo, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", PartNo: " & PartNo & ", DesignLevel: " & DesignLevel & ", SizeThickness: " & SizeThickness & ", Qty: " & Qty & ", Price: " & Price & ", RecoveryAmt: " & RecoveryAmt & ", POno: " & PONo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertSampleMtrlReqPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSampleMtrlReqPartNo(ByVal DesignLevel As String, ByVal SizeThickness As String, ByVal Qty As Decimal, ByVal Price As Decimal, ByVal RecoveryAmt As Decimal, ByVal PONo As String, ByVal original_SMRNo As Integer, ByVal original_RowID As Integer, ByVal original_PartNo As String, ByVal PartNo As String) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If original_SMRNo = Nothing Then original_SMRNo = 0
            If PartNo Is Nothing Then PartNo = ""

            If DesignLevel Is Nothing Then
                DesignLevel = ""
            Else
                DesignLevel = commonFunctions.replaceSpecialChar(DesignLevel, False)
            End If

            If SizeThickness Is Nothing Then
                SizeThickness = ""
            Else
                SizeThickness = commonFunctions.replaceSpecialChar(SizeThickness, False)
            End If

            If Qty = Nothing Then Qty = 0
            If Price = Nothing Then Price = 0
            If RecoveryAmt = Nothing Then RecoveryAmt = 0
            If PONo Is Nothing Then PONo = ""

            Dim rowsAffected As Integer = Adapter2.sp_Update_SampleMtrlReq_PartNo(original_SMRNo, original_RowID, PartNo, DesignLevel, SizeThickness, Qty, Price, RecoveryAmt, PONo, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo & ", PartNo: " & PartNo & ", DesignLevel: " & DesignLevel & ", SizeThickness: " & SizeThickness & ", Qty: " & Qty & ", Price: " & Price & ", RecoveryAmt: " & RecoveryAmt & ", POno: " & PONo & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF UpdateSampleMtrlReqPartNo

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
   Public Function DeleteSampleMtrlReqPartNo(ByVal SMRNo As Integer, ByVal RowID As Integer, ByVal original_SMRNo As Integer, ByVal original_RowID As Integer, ByVal original_PartNo As String) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter2.sp_Delete_SampleMtrlReq_PartNo(original_SMRNo, original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo _
            & ", RowID: " & original_RowID _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqPartNo: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF DeleteSampleMtrlReqPartNo
#End Region 'EOF "SampleMtrlReq_PartNo"

#Region "SampleMtrlReq_Documents"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
Public Function GetSampleMtrlReqDocuments(ByVal SMRNo As Integer, ByVal DocID As Integer, ByVal Section As String) As PGM.SampleMtrlReq_DocumentsDataTable
        Try

            If SMRNo = Nothing Then SMRNo = 0
            If DocID = Nothing Then DocID = 0
            If Section Is Nothing Then Section = ""

            Return Adapter3.Get_SampleMtrlReq_Documents(SMRNo, DocID, Section)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", DocID: " & DocID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqDocuments: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqDocuments: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleMtrlReqDocuments

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function DeleteSampleMtrlReqDocuments(ByVal SMRNo As Integer, ByVal DocID As Integer, ByVal Section As String, ByVal original_SMRNo As Integer, ByVal original_DocID As Integer) As Boolean

        Try
            If Section Is Nothing Then Section = ""

            Dim rowsAffected As Integer = Adapter3.sp_Delete_SampleMtrlReq_Documents(original_SMRNo, original_DocID, Section)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo _
            & ", DocID: " & original_DocID _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqDocuments: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqDocuments: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF DeleteSampleMtrlReqDocuments

#End Region 'EOF "SampleMtrlReq_Documents"

#Region "SampleMtrlReq_Shipping"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
   Public Function GetSampleMtrlReqShipping(ByVal SMRNo As Integer, ByVal RowID As Integer) As PGM.SampleMtrlReq_ShippingDataTable
        Try

            If SMRNo = Nothing Then SMRNo = 0
            If RowID = Nothing Then RowID = 0

            Return Adapter4.Get_SampleMtrlReq_Shipping(SMRNo, RowID)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", RowID: " & RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleMtrlReqShipping

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
   Public Function InsertSampleMtrlReqShipping(ByVal SMRNo As Integer, ByVal ShipperNo As Integer, ByVal TotalShippingCost As Decimal, ByVal FreightBillProNo As String) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If SMRNo = Nothing Then SMRNo = 0
            If ShipperNo = Nothing Then ShipperNo = 0
            If TotalShippingCost = Nothing Then TotalShippingCost = 0
            If FreightBillProNo Is Nothing Then FreightBillProNo = ""

            Dim rowsAffected As Integer = Adapter4.sp_Insert_SampleMtrlReq_Shipping(SMRNo, ShipperNo, TotalShippingCost, FreightBillProNo, CreatedBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", ShipperNo: " & ShipperNo & ", TotalShippingCost: " & TotalShippingCost & ", FreightBillProNo: " & FreightBillProNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try

    End Function 'EOF InsertSampleMtrlReqShipping

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSampleMtrlReqShipping(ByVal ShipperNo As Integer, ByVal TotalShippingCost As Decimal, ByVal FreightBillProNo As String, ByVal original_SMRNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'If SMRNo = Nothing Then SMRNo = 0
            If ShipperNo = Nothing Then ShipperNo = 0
            If TotalShippingCost = Nothing Then TotalShippingCost = 0
            If FreightBillProNo Is Nothing Then FreightBillProNo = ""

            Dim rowsAffected As Integer = Adapter4.sp_Update_SampleMtrlReq_Shipping(original_SMRNo, original_RowID, ShipperNo, TotalShippingCost, FreightBillProNo, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo & ", RowID: " & original_RowID & ", ShipperNo: " & ShipperNo & ", TotalShippingCost: " & TotalShippingCost & ", FreightBillProNo: " & FreightBillProNo & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF UpdateSampleMtrlReqShipping

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
   Public Function DeleteSampleMtrlReqShipping(ByVal SMRNo As Integer, ByVal RowID As Integer, ByVal original_SMRNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter4.sp_Delete_SampleMtrlReq_Shipping(original_SMRNo, original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo _
            & ", RowID: " & original_RowID _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqShipping: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF DeleteSampleMtrlReqShipping
#End Region 'EOF "SampleMtrlReq_Shipping"

#Region "SampleMtrlReq_Approval"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
   Public Function GetSampleMtrlReqApproval(ByVal SMRNo As Integer, ByVal Sequence As Integer, ByVal ResponsibleTMID As Integer, ByVal PendingApprovals As Boolean, ByVal RejectedTM As Boolean) As PGM.SampleMtrlReq_ApprovalDataTable
        Try

            If SMRNo = Nothing Then SMRNo = 0
            If Sequence = Nothing Then Sequence = 0
            If ResponsibleTMID = Nothing Then ResponsibleTMID = 0

            Return Adapter5.Get_SampleMtrlReq_Approval(SMRNo, Sequence, ResponsibleTMID, PendingApprovals, RejectedTM)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", Sequence: " & Sequence _
  & ", ResponsibleTMID: " & ResponsibleTMID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleMtrlReqApproval

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function InsertSampleMtrlReqAddLvl1Aprvl(ByVal SMRNo As Integer, ByVal SeqNo As Integer, ByVal ResponsibleTMID As Integer, ByVal OriginalTMID As Integer) As Boolean

        Try
            Dim pTable As PGM.SampleMtrlReq_ApprovalDataTable = Adapter5.Get_SampleMtrlReq_Approval(SMRNo, 0, 0, 0, 0)
            Dim pscpRow As PGM.SampleMtrlReq_ApprovalRow = pTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
            Dim TMSigned As Boolean = False

            If pTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            ' Update the SampleMtrlReq_Approval record
            Dim rowsAffected As Integer = Adapter5.sp_Insert_SampleMtrlReq_AddLvl1Aprvl(SMRNo, 1, ResponsibleTMID, OriginalTMID, User, Date.Today)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo _
            & ", ResponsibleTMID: " & ResponsibleTMID _
            & ", SeqNo: " & SeqNo _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertSampleMtrlReqAddLvl1Aprvl: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertSampleMtrlReqAddLvl1Aprvl: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF InsertSampleMtrlReqAddLvl1Aprvl

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateSampleMtrlReqApproval(ByVal Status As String, ByVal Comments As String, ByVal original_SMRNo As Integer, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal DateNotified As String, ByVal TeamMemberName As String) As Boolean

        Try
            ''Check for Shipping/EDI Coordinator prior to update
            ''if found, allow saving the update
            ''if not found, kick user back to the form to generate the error.
            Dim ds As DataSet = New DataSet
            Dim ShipEdiCoordTMID As Integer = 0
            ds = PGMModule.GetSampleMtrlReq(original_SMRNo, "", 0, 0, "", "", "", "", "", "")
            If commonFunctions.CheckDataSet(ds) = True Then
                ShipEdiCoordTMID = IIf(IsDBNull(ds.Tables(0).Rows(0).Item("ShipEdiCoordTMID")), 0, ds.Tables(0).Rows(0).Item("ShipEdiCoordTMID"))
            End If
            If ShipEdiCoordTMID <> 0 Then

                Dim pTable As PGM.SampleMtrlReq_ApprovalDataTable = Adapter5.Get_SampleMtrlReq_Approval(original_SMRNo, 0, 0, 0, 0)
                Dim pscpRow As PGM.SampleMtrlReq_ApprovalRow = pTable(0)
                Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
                Dim DefaultTMID As String = HttpContext.Current.Request.Cookies("UGNDB_TMID").Value
                Dim TMSigned As Boolean = False

                If pTable.Count = 0 Then
                    ' no matching record found, return false
                    Return False
                End If


                ' Logical Rule - Cannot update a record without null columns
                If original_SMRNo = Nothing Then
                    Throw New ApplicationException("Update Cancelled: Sample Reference No is a required field.")
                End If
                If Status = Nothing Then
                    Throw New ApplicationException("Update Cancelled: Status is a required field.")
                End If
                If Comments = Nothing And Status = "Rejected" Then
                    Throw New ApplicationException("Update Cancelled: Comments is a required field.")
                End If

                Comments = commonFunctions.replaceSpecialChar(Comments, False)

                If Status <> "Pending" Then
                    TMSigned = True
                End If
                If Comments <> "" Or Comments <> Nothing Then
                    ' Update the SampleMtrlReq_Approval record
                    Dim rowsAffected As Integer = Adapter5.sp_Update_SampleMtrlReq_Approval(original_SMRNo, original_TeamMemberID, TMSigned, Status, Comments, original_SeqNo, User, Date.Today)

                    ' Return true if precisely one row was updated, otherwise false
                    Return rowsAffected = 1
                Else
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo _
            & ", TeamMemberID: " & original_TeamMemberID _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF UpdateSampleMtrlReqApproval

    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
   Public Function DeleteSampleMtrlReqApproval(ByVal Status As String, ByVal Comments As String, ByVal original_SMRNo As Integer, ByVal original_SeqNo As Integer, ByVal original_TeamMemberID As Integer, ByVal original_OrigTeamMemberID As Integer, ByVal DateNotified As String, ByVal SeqNo As Integer) As Boolean

        Try
            Dim rowsAffected As Integer = Adapter5.sp_Delete_SampleMtrlReq_Approval(original_SMRNo, original_TeamMemberID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & original_SMRNo _
            & ", TeamMemberID: " & original_TeamMemberID _
            & ",UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSampleMtrlReqApproval: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing

        End Try

    End Function 'EOF DeleteSampleMtrlReqApproval
#End Region 'EOF "SampleMtrlReq_Approval"

#Region "SampleMtrlReq_RSS"
    ''*****
    ''* Select SampleMtrlReq_RSS returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSampleMtrlReqRSS(ByVal SMRNo As String, ByVal RSSID As Integer) As PGM.SampleMtrlReq_RSSDataTable

        Try
            If SMRNo = Nothing Then SMRNo = 0
            If RSSID = Nothing Then RSSID = 0

            Return Adapter6.Get_SampleMtrlReq_RSS(SMRNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqRSS : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetSampleMtrlReqRSS
#End Region 'EOF "SampleMtrlReq_RSS"

#Region "SampleMtrlReq_RSS_Reply"
    ''*****
    ''* Select SampleMtrlReq_RSS_Reply returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSampleMtrlReqRSSReply(ByVal SMRNo As String, ByVal RSSID As Integer) As PGM.SampleMtrlReq_RSS_ReplyDataTable

        Try
            ' Logical Rule - Cannot insert a record without null columns
            If SMRNo = Nothing Then SMRNo = 0
            If RSSID = Nothing Then RSSID = 0

            Return Adapter7.Get_SampleMtrlReq_RSS_Reply(SMRNo, RSSID)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", RSSID: " & RSSID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False) & " :<br/> PGMBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReqRSSReply : " & commonFunctions.replaceSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF GetSampleMtrlReqRSSReply
#End Region 'EOF "SampleMtrlReq_RSS_Reply"

#Region "SampleMtrlReq"
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
   Public Function GetSampleMtrlReq(ByVal SMRNo As String, ByVal SampleDesc As String, ByVal RequestorTMID As Integer, ByVal AccountMgrTMID As Integer, ByVal UGNFacility As String, ByVal Customer As String, ByVal PartNo As String, ByVal IntExt As String, ByVal PONo As String, ByVal RecStatus As String) As PGM.SampleMtrlReqDataTable
        Try

            If SMRNo Is Nothing Then SMRNo = ""
            If SampleDesc Is Nothing Then SampleDesc = ""
            If RequestorTMID = Nothing Then RequestorTMID = 0
            If AccountMgrTMID = Nothing Then AccountMgrTMID = 0
            If UGNFacility Is Nothing Then UGNFacility = ""
            If Customer Is Nothing Then Customer = ""
            If PartNo Is Nothing Then PartNo = ""
            If IntExt Is Nothing Then IntExt = ""
            If PONo Is Nothing Then PONo = ""
            If RecStatus Is Nothing Then RecStatus = ""


            Return Adapter8.GetSampleMtrlReq(SMRNo, SampleDesc, RequestorTMID, AccountMgrTMID, UGNFacility, Customer, PartNo, IntExt, PONo, RecStatus)

        Catch ex As Exception

            'on error, collect function data, error, and last page, the redirect to error page
            Dim strUserEditedData As String = "SMRNo: " & SMRNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSampleMtrlReq: " & commonFunctions.convertSpecialChar(ex.Message, False) & ":<br/> PGMBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PGM/SampleMaterialRequestList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSampleMtrlReq: " & commonFunctions.convertSpecialChar(ex.Message, False), "PGMBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing

        End Try
    End Function 'EOF GetSampleMtrlReq

#End Region
End Class