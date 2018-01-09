''******************************************************************************************************
''* MfgProd.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : LREY 09/26/2011
''* NOTES   : Need to add Production_Inspection to this BLL. 
''* Modified: 05/08/2012 LREY   Added MFG_Capacity_Process_TableAdapter
''* Modified: 07/10/2012 LREY   Added Planner_Code_Maint_TableAdapter
''******************************************************************************************************
Imports Microsoft.VisualBasic
Imports MfgProdTableAdapters
<System.ComponentModel.DataObject()> _
Public Class MfgProdBLL
    Private pAdapter As Chart_Spec_TableAdapter = Nothing
    Private pAdapter2 As MFG_Capacity_Process_TableAdapter = Nothing
    Private pAdapter3 As MFG_Capacity_Process_WC_TableAdapter = Nothing
    Private pAdapter4 As Chart_Spec_FrmTmplt_TableAdapter = Nothing
    Private pAdapter5 As Planner_Code_Maint_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As MfgProdTableAdapters.Chart_Spec_TableAdapter
        Get
            If pAdapter Is Nothing Then
                pAdapter = New Chart_Spec_TableAdapter()
            End If
            Return pAdapter
        End Get
    End Property 'CHART_SPEC
    Protected ReadOnly Property Adapter2() As MfgProdTableAdapters.MFG_Capacity_Process_TableAdapter
        Get
            If pAdapter2 Is Nothing Then
                pAdapter2 = New MFG_Capacity_Process_TableAdapter()
            End If
            Return pAdapter2
        End Get
    End Property 'MFG_Capacity_Process_TableAdapter

    Protected ReadOnly Property Adapter3() As MfgProdTableAdapters.MFG_Capacity_Process_WC_TableAdapter
        Get
            If pAdapter3 Is Nothing Then
                pAdapter3 = New MFG_Capacity_Process_WC_TableAdapter()
            End If
            Return pAdapter3
        End Get
    End Property 'MFG_Capacity_Process_WC_TableAdapter

    Protected ReadOnly Property Adapter4() As MfgProdTableAdapters.Chart_Spec_FrmTmplt_TableAdapter
        Get
            If pAdapter4 Is Nothing Then
                pAdapter4 = New Chart_Spec_FrmTmplt_TableAdapter()
            End If
            Return pAdapter4
        End Get
    End Property 'Chart_Spec_FrmTmplt_TableAdapter

    Protected ReadOnly Property Adapter5() As MfgProdTableAdapters.Planner_Code_Maint_TableAdapter
        Get
            If pAdapter5 Is Nothing Then
                pAdapter5 = New Planner_Code_Maint_TableAdapter()
            End If
            Return pAdapter5
        End Get
    End Property 'Planner_Code_Maint_TableAdapter

    ''*****
    ''* Select Chart_Spec returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetChartSpec(ByVal CSID As Integer, ByVal UGNFacility As String, ByVal OEMManufacturer As String, ByVal CustLoc As String, ByVal DesignationType As String, ByVal PartNo As String, ByVal Department As Integer, ByVal WorkCenter As Integer, ByVal Formula As String, ByVal Obsolete As Boolean) As MfgProd.Chart_SpecDataTable

        Try
            If CSID = Nothing Then
                CSID = 0
            End If

            If UGNFacility = Nothing Then
                UGNFacility = ""
            End If

            If OEMManufacturer = Nothing Then
                OEMManufacturer = ""
            End If

            If CustLoc = Nothing Then
                CustLoc = ""
            End If

            If DesignationType = Nothing Then
                DesignationType = ""
            End If

            If PartNo = Nothing Then
                PartNo = ""
            End If

            If Department = Nothing Then
                Department = 0
            End If

            If WorkCenter = Nothing Then
                WorkCenter = 0
            End If

            If Formula = Nothing Then
                Formula = ""
            End If

            Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
            If UGNFacility = "" And UGNDB_TMLoc <> "UT" Then
                UGNFacility = UGNDB_TMLoc
            ElseIf UGNFacility = "UT" And UGNDB_TMLoc = "UT" Then
                UGNFacility = ""
            End If

            Return Adapter.Get_Chart_Spec(CSID, UGNFacility, OEMManufacturer, CustLoc, DesignationType, PartNo, Department, WorkCenter, Formula, Obsolete)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CSID: " & CSID & ", UGNFacility: " & UGNFacility & ", OEMManufacturer: " & OEMManufacturer & ", CustLoc: " & CustLoc & ", DesignationType: " & DesignationType & ", PartNo: " & PartNo & ", WorkCenter: " & WorkCenter & ", Formula: " & Formula & ", Obsolete: " & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select Chart_Spec returning all rows

    ''*****
    ''* Select Chart_Spec_Listing returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetChartSpecListing(ByVal CSID As Integer, ByVal UGNFacility As String, ByVal OEMManufacturer As String, ByVal CustLoc As String, ByVal DesignationType As String, ByVal PartNo As String, ByVal Department As Integer, ByVal WorkCenter As Integer, ByVal Formula As String, ByVal Obsolete As Boolean) As MfgProd.Chart_SpecDataTable

        Try
            If CSID = Nothing Then
                CSID = 0
            End If

            If UGNFacility = Nothing Then
                UGNFacility = ""
            End If

            If OEMManufacturer = Nothing Then
                OEMManufacturer = ""
            End If

            If CustLoc = Nothing Then
                CustLoc = ""
            End If

            If DesignationType = Nothing Then
                DesignationType = ""
            End If

            If PartNo = Nothing Then
                PartNo = ""
            End If

            If Department = Nothing Then
                Department = 0
            End If

            If WorkCenter = Nothing Then
                WorkCenter = 0
            End If

            If Formula = Nothing Then
                Formula = ""
            End If

            Dim UGNDB_TMLoc As String = HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value
            If UGNFacility = "" And UGNDB_TMLoc <> "UT" Then
                UGNFacility = UGNDB_TMLoc
            ElseIf UGNFacility = "UT" And UGNDB_TMLoc = "UT" Then
                UGNFacility = ""
            End If

            Return Adapter.Get_Chart_Spec_Listing(CSID, UGNFacility, OEMManufacturer, CustLoc, DesignationType, PartNo, Department, WorkCenter, Formula, Obsolete)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CSID: " & CSID & ", UGNFacility: " & UGNFacility & ", OEMManufacturer: " & OEMManufacturer & ", CustLoc: " & CustLoc & ", DesignationType: " & DesignationType & ", PartNo: " & PartNo & ", WorkCenter: " & WorkCenter & ", Formula: " & Formula & ", Obsolete: " & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChartSpecListing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpec.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpecListing : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select Chart_Spec_Listing returning all rows

    ''*****
    ''* Select MFG_Capacity_Process returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetMFGCapacityProcess(ByVal PID As Integer, ByVal MFGProcessName As String) As MfgProd.MFG_Capacity_ProcessDataTable

        Try
            If PID = Nothing Then
                PID = 0
            End If

            If MFGProcessName = Nothing Then
                MFGProcessName = ""
            Else
                MFGProcessName = commonFunctions.convertSpecialChar(MFGProcessName, False)
            End If

            Return Adapter2.Get_MFG_Capacity_Process(PID, MFGProcessName)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PID: " & PID & ", MFGProcessName: " & MFGProcessName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select MFG_Capacity_Process_TableAdapter returning all rows

    ''*****
    ''* Insert New MFG_Capacity_Process
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertMFGCapacityProcess(ByVal MFGProcessName As String, ByVal HalfSplit As Boolean) As Boolean
        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            MFGProcessName = commonFunctions.convertSpecialChar(MFGProcessName, False)

            Dim rowsAffected As Integer = Adapter2.sp_Insert_MFG_Capacity_Process(MFGProcessName, HalfSplit, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MFGProcessName: " & MFGProcessName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Insert New MFG_Capacity_Process

    ''*****
    ''* Update MFG_Capacity_Process
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdateMFGCapacityProcess(ByVal MFGProcessName As String, ByVal HalfSplit As Boolean, ByVal CurrentQtyShftsPerDay As Decimal, ByVal original_MFGProcessName As String, ByVal Obsolete As Boolean, ByVal original_PID As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            MFGProcessName = commonFunctions.convertSpecialChar(MFGProcessName, False)
            Dim OrigMFGProcessName As String = Nothing
            If original_MFGProcessName <> Nothing Then
                OrigMFGProcessName = (commonFunctions.convertSpecialChar(original_MFGProcessName, False))
            End If

            Dim rowsAffected As Integer = Adapter2.sp_Update_MFG_Capacity_Process(original_PID, MFGProcessName, HalfSplit, CurrentQtyShftsPerDay, Obsolete, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MFGProcessName: " & MFGProcessName & ", HalfSplit: " & HalfSplit & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateMFGCapacityProcess : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Update New MFG_Capacity_Process

    ''*****
    ''* Select MFG_Capacity_Process_WC returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
Public Function GetMFGCapacityProcessWC(ByVal PID As Integer, ByVal WorkCenter As Integer, ByVal UGNFacility As String) As MfgProd.MFG_Capacity_Process_WCDataTable

        Try
            If PID = Nothing Then
                PID = 0
            End If

            If WorkCenter = Nothing Then
                WorkCenter = 0
            End If

            If UGNFacility = Nothing Then
                UGNFacility = ""
            End If

            Return Adapter3.Get_MFG_Capacity_Process_WC(PID, WorkCenter, UGNFacility)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PID: " & PID & ", WorkCenter: " & WorkCenter & ", UGNFacility: " & UGNFacility & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessWCMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select MFG_Capacity_Process_WC_TableAdapter returning all rows

    ''*****
    ''* Insert New MFG_Capacity_Process_WC
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertMFGCapacityProcessWC(ByVal PID As Integer, ByVal WorkCenter As Integer, ByVal UGNFacility As String, ByVal NoOfShifts As Decimal, ByVal HrsPerShift As Decimal) As Boolean
        Try
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter3.sp_Insert_MFG_Capacity_Process_WC(PID, WorkCenter, UGNFacility, NoOfShifts, HrsPerShift, User)
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PID: " & PID & ", WorkCenter: " & WorkCenter & ", UGNFacility: " & UGNFacility & ", NoOfShifts: " & NoOfShifts & ", HrsPerShift: " & HrsPerShift & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessWCMaint.aspx?pPID=" & PID
            UGNErrorTrapping.InsertErrorLog("InsertMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Insert New MFG_Capacity_Process_WC

    ''*****
    ''* Update MFG_Capacity_Process_WC
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdateMFGCapacityProcessWC(ByVal WorkCenter As Integer, ByVal UGNFacility As String, ByVal NoOfShifts As Decimal, ByVal HrsPerShift As Decimal, ByVal original_PID As Integer, ByVal original_WorkCenter As Integer, ByVal original_UGNFacility As String) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter3.sp_Update_MFG_Capacity_Process_WC(original_PID, WorkCenter, UGNFacility, NoOfShifts, HrsPerShift, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PID: " & original_PID & ", WorkCenter: " & WorkCenter & ", UGNFacility: " & UGNFacility & ", NoOfShifts: " & NoOfShifts & ", HrsPerShift: " & HrsPerShift & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessWCMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Update MFG_Capacity_Process_WC

    ''*****
    ''* Delete MFG_Capacity_Process_WC
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function DeleteMFGCapacityProcessWC(ByVal WorkCenter As Integer, ByVal original_PID As Integer, ByVal original_WorkCenter As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter3.sp_Delete_MFG_Capacity_Process_WC(original_PID, original_WorkCenter, UpdatedBy)

            Return rowsAffected '= 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PID: " & original_PID & ", WorkCenter: " & WorkCenter & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/MFGCapacityProcessWCMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteMFGCapacityProcessWC : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Delete MFG_Capacity_Process_WC

    ''*****
    ''* Select Chart_Spec_FrmTmplt returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetChartSpecFrmTmplt(ByVal RowID As Integer, ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal Obsolete As Boolean) As MfgProd.Chart_Spec_FrmTmpltDataTable

        Try
            If RowID = Nothing Then
                RowID = 0
            End If

            If FormulaID = Nothing Then
                FormulaID = 0
            End If

            If FormulaName = Nothing Then
                FormulaName = ""
            End If

            Return Adapter4.Get_Chart_Spec_FrmTmplt(RowID, FormulaID, FormulaName, Obsolete)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", FormulaID: " & FormulaID & ", FormulaName: " & FormulaName & ", Obsolete: " & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecFrmTmplt.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select Chart_Spec_FrmTmplt returning all rows

    ''*****
    ''* Insert Chart_Spec_FrmTmplt
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertChartSpecFrmTmplt(ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal LabelName As String, ByVal ColumnName As Integer, ByVal FldObjName As String, ByVal FldType As String, ByVal DfltVal As String, ByVal ReqFld As Boolean, ByVal [ReadOnly] As Boolean, ByVal Notes As String) As Boolean
        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter4.sp_Insert_Chart_Spec_FrmTmplt(FormulaID, FormulaName, LabelName, ColumnName, FldObjName, FldType, DfltVal, ReqFld, [ReadOnly], 0, Notes, CreatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecFrmTmplt.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Insert Chart_Spec_FrmTmplt

    ''*****
    ''* Update Chart_Spec_FrmTmplt
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdateChartSpecFrmTmplt(ByVal FormulaID As Integer, ByVal FormulaName As String, ByVal LabelName As String, ByVal ColumnName As Integer, ByVal FldObjName As String, ByVal FldType As String, ByVal DfltVal As String, ByVal ReqFld As Boolean, ByVal [ReadOnly] As Boolean, ByVal Notes As String, ByVal original_RowID As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter4.sp_Update_Chart_Spec_FrmTmplt(original_RowID, FormulaID, FormulaName, LabelName, ColumnName, FldObjName, FldType, DfltVal, ReqFld, [ReadOnly], 0, Notes, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecFrmTmplt.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Update Chart_Spec_FrmTmplt

    ''*****
    ''* Delete Chart_Spec_FrmTmplt
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function DeleteChartSpecFrmTmplt(ByVal original_RowID As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter4.sp_Delete_Chart_Spec_FrmTmplt(original_RowID)

            Return rowsAffected '= 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & original_RowID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/MfgProd/ChartSpecFrmTmplt.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteChartSpecFrmTmplt : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Delete Chart_Spec_FrmTmplt

    ''*****
    ''* Select Planner_Code_Maint returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetPlannerCode(ByVal UGNFacility As String) As MfgProd.Planner_Code_MaintDataTable
        Try

            If UGNFacility = Nothing Then
                UGNFacility = ""
            End If

            Return Adapter5.Get_Planner_Code(UGNFacility)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPlannerCode : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CCM/PlannerCodeMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPlannerCode : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF Select GetPlannerCode returning all rows

    ''*****
    ''* Update Planner_Code_Maint
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function UpdatePlannerCode(ByVal DeptCell As String, ByVal PlannerDesc As String, ByVal NotUsed As Boolean, ByVal original_PlannerID As Integer) As Boolean
        Try
            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If DeptCell = Nothing Then
                DeptCell = ""
            Else
                DeptCell = commonFunctions.convertSpecialChar(DeptCell, False)
            End If

            If PlannerDesc = Nothing Then
                PlannerDesc = ""
            Else
                PlannerDesc = commonFunctions.convertSpecialChar(PlannerDesc, False)
            End If

            Dim rowsAffected As Integer = Adapter5.sp_Update_Planner_Code(original_PlannerID, DeptCell, PlannerDesc, NotUsed, UpdatedBy)

            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PlannerID: " & original_PlannerID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdatePlannerCode : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MfgProdBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/CCM/PlannerCodeMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdatePlannerCode : " & commonFunctions.convertSpecialChar(ex.Message, False), "MfgProdBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Function ' EOF Update UpdatePlannerCode
End Class
