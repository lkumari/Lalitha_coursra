' ************************************************************************************************
'
' Name:		DrawingRevisionCompare.aspx.vb
' Purpose:	This Code Behind is for the reportWindow.aspx of the PE Drawings Management System App
'
' Date		 Author	
' 08/14/2008 Roderick Carlson
' 10/22/2008 Roderick Carlson - prevent empty WMD and AMD values from converting to inches when empty
' 11/05/2008 Roderick Carlson - Roll Width Hidden
' 08/21/2009 Roderick Carlson - adjusted BPCS to sub table
' 01/06/2014 LRey   - Replaced "BPCS Part No" to "Part No" wherever used.
' ************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.HttpCookie
Imports System.IO
Imports System.Net
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Math
Imports System.Net.Mail

Partial Class DrawingRevisionCompare
    Inherits System.Web.UI.Page
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Page.Title = "DMS Drawing Revision Comparison"

            If Request.QueryString("DrawingNo") IsNot Nothing Then
                ViewState("DrawingNo") = Request.QueryString("DrawingNo")
                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub BindData()

        Try
            Dim ds As DataSet
            Dim dsImages As DataSet
            Dim dsRev As DataSet
            Dim dsDrawingBPCS As DataSet
            Dim strCurrentDrawingNo As String = ""

            Dim strTempAMD As String = ""
            Dim strTempWMD As String = ""

            Dim strRevision As String = ""
            Dim strDrawingNoWithoutRevision As String = ""
            Dim strPreviousDrawingNo As String = ""
            Dim iCurrentRevision As Integer = 0
            Dim iPrevioustrRevision As Integer = 0
            Dim strPreviousRevision As String = ""
            Dim iFirstLeftParenthesisLocation As Integer = 0

            Dim strPreviousPartNo As String = ""
            Dim strPreviousOldPartName As String = ""
            Dim strPreviousEngineerName As String = ""
            Dim strPreviousDrawingByEngineerName As String = ""
            Dim strPreviousCheckedByEngineerName As String = ""
            Dim strPreviousProcessEngineerName As String = ""
            Dim strPreviousQualityEngineerName As String = ""
            Dim strPreviousFinalApprovalDate As String = ""
            Dim strPreviousConstruction As String = ""
            Dim strPreviousDensityValue As String = ""
            Dim strPreviousDensityTolerance As String = ""
            Dim strPreviousDensityUnits As String = ""
            Dim strPreviousThicknessValue As String = ""
            Dim strPreviousThicknessTolerance As String = ""
            Dim strPreviousThicknessUnits As String = ""
            Dim strPreviousControl As String = ""
            Dim strPreviousNotes As String = ""
            Dim strPreviousLength As String = ""
            Dim strPreviousWidth As String = ""
            Dim strPreviousCommodityName As String = ""
            Dim strPreviousPurchasedGoodName As String = ""
            Dim strPreviousImageURL As String = ""
            Dim strPreviousStatusDecoded As String = ""
            Dim strPreviousSubmittedOn As String = ""

            Dim strPreviousRevisionNotes As String = ""
            Dim strPreviousDrawingLayoutType As String = ""

            Dim strCurrentPartNo As String = ""
            Dim strCurrentOldPartName As String = ""
            Dim sCurrentEngineerName As String = ""
            Dim strCurrentDrawingByEngineerName As String = ""
            Dim strCurrentCheckedByEngineerName As String = ""
            Dim strCurrentProcessEngineerName As String = ""
            Dim strCurrentQualityEngineerName As String = ""
            Dim strCurrentFinalApprovalDate As String = ""
            Dim strCurrentConstruction As String = ""
            Dim strCurrentDensityValue As String = ""
            Dim strCurrentDensityTolerance As String = ""
            Dim strCurrentDensityUnits As String = ""
            Dim strCurrentThicknessValue As String = ""
            Dim strCurrentThicknessTolerance As String = ""
            Dim strCurrentThicknessUnits As String = ""
            Dim strCurrentControl As String = ""
            Dim strCurrentNotes As String = ""
            Dim strCurrentLength As String = ""
            Dim strCurrentLengthUnits As String = ""
            Dim strCurrentLengthRef As String = ""
            Dim strCurrentWidth As String = ""
            Dim strCurrentWidthUnits As String = ""
            Dim strCurrentWidthRef As String = ""
            Dim strCurrentCommodityName As String = ""
            Dim strCurrentPurchasedGoodName As String = ""
            Dim strCurrentImageURL As String = ""
            Dim strCurrentStatusDecoded As String = ""
            Dim strCurrentSubmittedOn As String = ""

            Dim strCurrentRevisionNotes As String = ""
            Dim strCurrentDrawingLayoutType As String = ""

            Dim strTempDrawingPartno As String = ""
            Dim iRowCounter As Integer = 0

            'find the first left parenthesis
            iFirstLeftParenthesisLocation = InStr(ViewState("DrawingNo"), "(")

            'get the drawing number without the revision
            strDrawingNoWithoutRevision = Mid$(ViewState("DrawingNo"), 1, iFirstLeftParenthesisLocation)

            'get the numbers between the parenthesis
            strRevision = Left$(Right$(ViewState("DrawingNo"), 3), 2)

            'convert string to integer
            iCurrentRevision = CInt(strRevision)

            If iCurrentRevision > 0 Then
                iPrevioustrRevision = iCurrentRevision - 1
                strPreviousRevision = CStr(iPrevioustrRevision)

                'if less than 10
                If strPreviousRevision.Length = 1 Then
                    strPreviousRevision = "0" + strPreviousRevision
                End If

                'determine previous Drawing Number
                strPreviousDrawingNo = strDrawingNoWithoutRevision + strPreviousRevision + ")"
                lblPreviousDrawingNo.Text = strPreviousDrawingNo

                'get details of previous Drawing Number
                'ds = PEModule.GetDrawing(strPreviousDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "")
                ds = PEModule.GetDrawing(strPreviousDrawingNo)
                If commonFunctions.CheckDataset(ds) = True Then

                    'common fields on all reports:
                    'strPreviousPartNo = ds.Tables(0).Rows(0).Item("PartNo1").ToString & " : " & ds.Tables(0).Rows(0).Item("PartNo2").ToString & " : " & ds.Tables(0).Rows(0).Item("PartNo3").ToString & " : " & ds.Tables(0).Rows(0).Item("PartNo4").ToString & " : " & ds.Tables(0).Rows(0).Item("PartNo5").ToString

                    dsDrawingBPCS = PEModule.GetDrawingBPCS(strPreviousDrawingNo)

                    'get all BPCS numbers assigned to drawing
                    If commonFunctions.CheckDataset(dsDrawingBPCS) = True Then
                        For iRowCounter = 0 To dsDrawingBPCS.Tables(0).Rows.Count - 1
                            strTempDrawingPartno = dsDrawingBPCS.Tables(0).Rows(iRowCounter).Item("PartNo").ToString

                            If strTempDrawingPartno <> "" Then

                                If strPreviousPartNo <> "" Then
                                    strPreviousPartNo += " : "
                                End If

                                strPreviousPartNo += strTempDrawingPartno
                            End If

                        Next
                    End If

                    strPreviousOldPartName = ds.Tables(0).Rows(0).Item("OldPartName").ToString & " | " & ds.Tables(0).Rows(0).Item("PartName").ToString
                    'strPreviousEngineerName = ds.Tables(0).Rows(0).Item("EngineerFullName").ToString
                    'strPreviousDrawingByEngineerName = ds.Tables(0).Rows(0).Item("DrawingByEngineerFullName").ToString
                    'strPreviousCheckedByEngineerName = ds.Tables(0).Rows(0).Item("CheckedByEngineerFullName").ToString
                    'strPreviousProcessEngineerName = ds.Tables(0).Rows(0).Item("ProcessEngineerFullName").ToString
                    'strPreviousQualityEngineerName = ds.Tables(0).Rows(0).Item("QualityEngineerFullName").ToString
                    strPreviousFinalApprovalDate = ds.Tables(0).Rows(0).Item("finalApprovalDate").ToString
                    strPreviousConstruction = ds.Tables(0).Rows(0).Item("construction").ToString
                    strPreviousDensityValue = ds.Tables(0).Rows(0).Item("densityvalue").ToString
                    strPreviousDensityTolerance = ds.Tables(0).Rows(0).Item("densitytolerance").ToString
                    strPreviousDensityUnits = ds.Tables(0).Rows(0).Item("densityUnits").ToString
                    strPreviousThicknessValue = ds.Tables(0).Rows(0).Item("thicknessvalue").ToString
                    strPreviousThicknessTolerance = ds.Tables(0).Rows(0).Item("thicknesstolerance").ToString
                    strPreviousThicknessUnits = ds.Tables(0).Rows(0).Item("thicknessUnits").ToString
                    strPreviousControl = ds.Tables(0).Rows(0).Item("controlplanref").ToString
                    strPreviousNotes = ds.Tables(0).Rows(0).Item("notes").ToString
                    strPreviousLength = ds.Tables(0).Rows(0).Item("AMDValue").ToString + " " + ds.Tables(0).Rows(0).Item("AMDTolerance").ToString
                    strPreviousWidth = ds.Tables(0).Rows(0).Item("WMDValue").ToString + " " + ds.Tables(0).Rows(0).Item("WMDTolerance").ToString

                    strPreviousCommodityName = ds.Tables(0).Rows(0).Item("CommodityName").ToString
                    strPreviousPurchasedGoodName = ds.Tables(0).Rows(0).Item("PurchasedGoodName").ToString

                    strPreviousDrawingLayoutType = ds.Tables(0).Rows(0).Item("DrawingLayoutType").ToString

                    'get image detail
                    dsImages = PEModule.GetDrawingImages(strPreviousDrawingNo, "")
                    If commonFunctions.CheckDataset(dsImages) = True Then
                        strPreviousImageURL = dsImages.Tables(0).Rows(0).Item("ImageURL").ToString
                    End If

                    strPreviousStatusDecoded = ds.Tables(0).Rows(0).Item("approvalStatusDecoded").ToString
                    strPreviousSubmittedOn = ds.Tables(0).Rows(0).Item("submittedOn").ToString

                    'get All related Part Revision Notes detail for previous Drawing
                    Dim row As DataRow
                    dsRev = PEModule.GetDrawingRevisions(strPreviousDrawingNo)
                    If commonFunctions.CheckDataset(dsRev) = True Then
                        For Each row In dsRev.Tables(0).Rows
                            'get identified approvers
                            If strPreviousRevisionNotes <> "" Then
                                strPreviousRevisionNotes = strPreviousRevisionNotes + "; "
                            End If
                            strPreviousRevisionNotes += row("DrawingNo").ToString + "=" + row("revisionNotes").ToString
                        Next
                    End If ' if dsRev exists

                End If ' if ds exists
            End If ' current revision > 0


            'ds = PEModule.GetDrawing(ViewState("DrawingNo"), "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, False, "", "")
            ds = PEModule.GetDrawing(ViewState("DrawingNo"))
            If commonFunctions.CheckDataset(ds) = True Then

                lblDrawingNo.Text = ViewState("DrawingNo")
                strCurrentDrawingNo = ViewState("DrawingNo")

                dsDrawingBPCS = PEModule.GetDrawingBPCS(strCurrentDrawingNo)
                strTempDrawingPartno = ""

                'get all BPCS numbers assigned to drawing
                If commonFunctions.CheckDataset(dsDrawingBPCS) = True Then
                    For iRowCounter = 0 To dsDrawingBPCS.Tables(0).Rows.Count - 1
                        strTempDrawingPartno = dsDrawingBPCS.Tables(0).Rows(iRowCounter).Item("PartNo").ToString

                        If strTempDrawingPartno <> "" Then

                            If strCurrentPartNo <> "" Then
                                strCurrentPartNo += " : "
                            End If

                            strCurrentPartNo += strTempDrawingPartno
                        End If

                    Next
                End If

                If strCurrentPartNo <> strPreviousPartNo And iCurrentRevision > 0 Then
                    lblPartNo.BackColor = Color.Yellow
                    lblPartNo.Font.Italic = True
                End If
                lblPartNo.Text = strCurrentPartNo

                strCurrentOldPartName = ds.Tables(0).Rows(0).Item("OldPartName").ToString
                If strCurrentOldPartName <> strPreviousOldPartName And iCurrentRevision > 0 Then
                    lblOldPartName.BackColor = Color.Yellow
                    lblOldPartName.Font.Italic = True
                End If
                lblOldPartName.Text = strCurrentOldPartName

                'sCurrentEngineerName = ds.Tables(0).Rows(0).Item("EngineerFullName").ToString
                'If sCurrentEngineerName <> strPreviousEngineerName And iCurrentRevision > 0 Then
                '    lblEngineer.BackColor = Color.Yellow
                '    lblEngineer.Font.Italic = True
                'End If
                'lblEngineer.Text = sCurrentEngineerName

                'strCurrentDrawingByEngineerName = ds.Tables(0).Rows(0).Item("DrawingByEngineerFullName").ToString
                'If strCurrentDrawingByEngineerName <> strPreviousDrawingByEngineerName And iCurrentRevision > 0 Then
                '    lblDrawingByEngineer.BackColor = Color.Yellow
                '    lblDrawingByEngineer.Font.Italic = True
                'End If
                'lblDrawingByEngineer.Text = strCurrentDrawingByEngineerName

                'strCurrentCheckedByEngineerName = ds.Tables(0).Rows(0).Item("CheckedByEngineerFullName").ToString
                'If strCurrentCheckedByEngineerName <> strPreviousCheckedByEngineerName And iCurrentRevision > 0 Then
                '    lblCheckedByEngineer.BackColor = Color.Yellow
                '    lblCheckedByEngineer.Font.Italic = True
                'End If
                'lblCheckedByEngineer.Text = strCurrentCheckedByEngineerName

                'strCurrentProcessEngineerName = ds.Tables(0).Rows(0).Item("ProcessEngineerFullName").ToString
                'If strCurrentProcessEngineerName <> strPreviousProcessEngineerName And iCurrentRevision > 0 Then
                '    lblProcessEngineer.BackColor = Color.Yellow
                '    lblProcessEngineer.Font.Italic = True
                'End If
                'lblProcessEngineer.Text = strCurrentProcessEngineerName

                'strCurrentQualityEngineerName = ds.Tables(0).Rows(0).Item("QualityEngineerFullName").ToString
                'If strCurrentQualityEngineerName <> strPreviousQualityEngineerName And iCurrentRevision > 0 Then
                '    lblQualityEngineer.BackColor = Color.Yellow
                '    lblQualityEngineer.Font.Italic = True
                'End If
                'lblQualityEngineer.Text = strCurrentQualityEngineerName

                strCurrentFinalApprovalDate = ds.Tables(0).Rows(0).Item("SubmittedOn").ToString
                If strCurrentFinalApprovalDate <> strPreviousFinalApprovalDate And iCurrentRevision > 0 Then
                    lblDate.BackColor = Color.Yellow
                    lblDate.Font.Italic = True
                End If
                lblDate.Text = strCurrentFinalApprovalDate

                strCurrentConstruction = ds.Tables(0).Rows(0).Item("construction").ToString
                If strCurrentConstruction <> strPreviousConstruction And iCurrentRevision > 0 Then
                    lblConstruction.BackColor = Color.Yellow
                    lblConstruction.Font.Italic = True
                End If
                lblConstruction.Text = strCurrentConstruction

                strCurrentDensityValue = ds.Tables(0).Rows(0).Item("densityvalue").ToString
                If strPreviousDensityValue <> strCurrentDensityValue And iCurrentRevision > 0 Then
                    lblDValue.BackColor = Color.Yellow
                    lblDValue.Font.Italic = True
                End If
                If strCurrentDensityValue = "0" Then
                    lblDValue.Text = ""
                Else
                    lblDValue.Text = strCurrentDensityValue
                End If

                strCurrentDensityTolerance = ds.Tables(0).Rows(0).Item("densitytolerance").ToString()
                If strCurrentDensityTolerance <> strPreviousDensityTolerance And iCurrentRevision > 0 Then
                    lblDTolerance.BackColor = Color.Yellow
                    lblDTolerance.Font.Italic = True
                End If
                lblDTolerance.Text = strCurrentDensityTolerance

                strCurrentDensityUnits = ds.Tables(0).Rows(0).Item("densityUnits").ToString
                If strCurrentDensityUnits <> strPreviousDensityUnits And iCurrentRevision > 0 Then
                    lblDUnits.BackColor = Color.Yellow
                    lblDUnits.Font.Italic = True
                End If
                lblDUnits.Text = strCurrentDensityUnits

                strCurrentThicknessValue = ds.Tables(0).Rows(0).Item("thicknessvalue").ToString
                If strCurrentThicknessValue <> strPreviousThicknessValue And iCurrentRevision > 0 Then
                    lblTValue.BackColor = Color.Yellow
                    lblTValue.Font.Italic = True
                End If

                If strCurrentThicknessValue = "0" Then
                    lblTValue.Text = ""
                Else
                    lblTValue.Text = strCurrentThicknessValue
                End If

                strCurrentThicknessTolerance = ds.Tables(0).Rows(0).Item("thicknesstolerance").ToString
                If strCurrentThicknessTolerance <> strPreviousThicknessTolerance And iCurrentRevision > 0 Then
                    lblTTolerance.BackColor = Color.Yellow
                    lblTTolerance.Font.Italic = True
                End If
                lblTTolerance.Text = strCurrentThicknessTolerance

                strCurrentThicknessUnits = ds.Tables(0).Rows(0).Item("thicknessUnits").ToString
                If strCurrentThicknessUnits <> strPreviousThicknessUnits And iCurrentRevision > 0 Then
                    lblTUnits.BackColor = Color.Yellow
                    lblTUnits.Font.Italic = True
                End If
                lblTUnits.Text = strCurrentThicknessUnits

                strCurrentControl = ds.Tables(0).Rows(0).Item("controlplanref").ToString
                If strCurrentControl <> strPreviousControl And iCurrentRevision > 0 Then
                    lblControl.BackColor = Color.Yellow
                    lblControl.Font.Italic = True
                End If
                lblControl.Text = strCurrentControl

                strCurrentNotes = ds.Tables(0).Rows(0).Item("notes").ToString.Trim
                If strCurrentNotes <> strPreviousNotes And iCurrentRevision > 0 Then
                    lblNotes.BackColor = Color.Yellow
                    lblNotes.Font.Italic = True
                End If

                lblNotes.Height = 21 + ((strCurrentNotes.Length / 75) * 21)
                lblNotes.Text = strCurrentNotes

                If ds.Tables(0).Rows(0).Item("AMDValue").ToString = "0" Then
                    strCurrentLength = ds.Tables(0).Rows(0).Item("AMDValue").ToString
                Else
                    If ds.Tables(0).Rows(0).Item("AMDValue").ToString <> "" Then
                        strCurrentLength = ds.Tables(0).Rows(0).Item("AMDValue").ToString & " " & ds.Tables(0).Rows(0).Item("AMDTolerance").ToString
                    End If
                End If

                If strCurrentLength <> strPreviousLength And iCurrentRevision > 0 Then
                    lblLength.BackColor = Color.Yellow
                    lblLength.Font.Italic = True
                End If

                strCurrentLengthUnits = ds.Tables(0).Rows(0).Item("AMDUnits").ToString
                If strCurrentLengthUnits = "mm" Then
                    If ds.Tables(0).Rows(0).Item("AMDValue").ToString <> "" Then
                        strCurrentLengthRef = CStr(Math.Round(CType(ds.Tables(0).Rows(0).Item("AMDValue").ToString, Single) * 0.0393700787, 2))
                        strCurrentLengthRef = "(" & strCurrentLengthRef & " inches)"
                    End If
                End If

                If strCurrentLengthUnits = "m" Then
                    strCurrentLengthRef = CStr(Math.Round(CType(ds.Tables(0).Rows(0).Item("AMDValue").ToString, Single) * 3.2808399, 2))
                    strCurrentLengthRef = "(" & strCurrentLengthRef & " feet)"
                End If
                lblLength.Text = strCurrentLength & strCurrentLengthUnits + " " & strCurrentLengthRef

                If ds.Tables(0).Rows(0).Item("WMDValue").ToString = "0" Then
                    strCurrentWidth = ds.Tables(0).Rows(0).Item("WMDValue").ToString
                Else
                    strCurrentWidth = ds.Tables(0).Rows(0).Item("WMDValue").ToString + " " & ds.Tables(0).Rows(0).Item("WMDTolerance").ToString
                End If

                If strCurrentWidth <> strPreviousWidth And iCurrentRevision > 0 Then
                    lblWidth.BackColor = Color.Yellow
                    lblWidth.Font.Italic = True
                End If

                strCurrentWidthUnits = ds.Tables(0).Rows(0).Item("WMDUnits").ToString
                If strCurrentWidthUnits = "mm" Then
                    If ds.Tables(0).Rows(0).Item("WMDValue").ToString <> "" Then
                        strCurrentWidthRef = CStr(Math.Round(CType(ds.Tables(0).Rows(0).Item("WMDValue").ToString, Single) * 0.0393700787, 2))
                        strCurrentWidthRef = "(" & strCurrentWidthRef & " inches)"
                    End If
                End If

                If strCurrentWidthUnits = "m" Then
                    If ds.Tables(0).Rows(0).Item("WMDValue").ToString <> "" Then
                        strCurrentWidthRef = CStr(Math.Round(CType(ds.Tables(0).Rows(0).Item("WMDValue").ToString, Single) * 3.2808399, 2))
                        strCurrentWidthRef = "(" & strCurrentWidthRef & " feet)"
                    End If
                End If
                lblWidth.Text = strCurrentWidth & strCurrentWidthUnits & " " & strCurrentWidthRef

                strCurrentCommodityName = ds.Tables(0).Rows(0).Item("CommodityName").ToString
                If strCurrentCommodityName <> "" Then
                    If strCurrentCommodityName <> strPreviousCommodityName And iCurrentRevision > 0 Then
                        lblCommodityValue.BackColor = Color.Yellow
                        lblCommodityValue.Font.Italic = True
                    End If
                    lblCommodityValue.Text = strCurrentCommodityName
                    lblCommodityValue.Visible = True
                    lblCommidityLabel.Visible = True
                End If

                strCurrentPurchasedGoodName = ds.Tables(0).Rows(0).Item("PurchasedGoodName").ToString
                If strCurrentPurchasedGoodName <> "" Then
                    If strCurrentPurchasedGoodName <> strPreviousPurchasedGoodName And iCurrentRevision > 0 Then
                        lblPurchasedGoodValue.BackColor = Color.Yellow
                        lblPurchasedGoodValue.Font.Italic = True
                    End If
                    lblPurchasedGoodValue.Text = strCurrentPurchasedGoodName
                    lblPurchasedGoodValue.Visible = True
                    lblPurchasedGoodLabel.Visible = True
                End If

                strCurrentDrawingLayoutType = ds.Tables(0).Rows(0).Item("DrawingLayoutType").ToString
                If strCurrentDrawingLayoutType = "" Or strCurrentDrawingLayoutType = "Blank-Standard" Or strCurrentDrawingLayoutType = "Other" Then
                    lblWMDVal.Text = "Dim 1: "
                    lblAMDVal.Text = "Dim 2: "
                End If

                If strCurrentDrawingLayoutType = "Blank-MD-Critical" Or strCurrentDrawingLayoutType = "Other-MD-Critical" Then
                    lblWMDVal.Text = "WMD: "
                    lblAMDVal.Text = "AMD: "
                End If

                If strCurrentDrawingLayoutType = "Rolled-Goods" Then
                    'lblWMDVal.Text = "Roll Width: "
                    lblWMDVal.Text = ""
                    lblWidth.Visible = False
                    lblAMDVal.Text = "Roll Length: "
                End If

                strCurrentDrawingLayoutType = ds.Tables(0).Rows(0).Item("DrawingLayoutType").ToString

                ViewState("AlternativeDrawingNo") = ""
                Select Case strCurrentDrawingLayoutType
                    Case "Blank-Standard"
                        ViewState("AlternativeDrawingNo") = "blankstandard"
                    Case "Rolled-Goods"
                        ViewState("AlternativeDrawingNo") = "rolledgoods"
                    Case "Blank-MD-Critical"
                        ViewState("AlternativeDrawingNo") = "blankmdcritical"
                End Select

                'get image detail
                tbDrawingImage.BorderColor = "White"
                dsImages = PEModule.GetDrawingImages(strCurrentDrawingNo, ViewState("AlternativeDrawingNo"))
                If commonFunctions.CheckDataset(dsImages) = True Then
                    strCurrentImageURL = dsImages.Tables(0).Rows(0).Item("ImageURL").ToString
                    imgDrawing.Src = "DrawingDisplayImage.aspx?DrawingNo=" & strCurrentDrawingNo & "&AlternativeDrawingNo=" & ViewState("AlternativeDrawingNo")
                End If

                If ((strCurrentImageURL <> strPreviousImageURL) Or (strCurrentDrawingLayoutType <> strPreviousDrawingLayoutType)) And iCurrentRevision > 0 Then
                    If ViewState("AlternativeDrawingNo") = "" Then
                        tbDrawingImage.BorderColor = "Yellow"
                    End If
                End If

                Dim dsSubDrawings As DataSet
                'bind existing Part/Drawing data to repeater control at bottom of screen
                dsSubDrawings = PEModule.GetSubDrawing(ViewState("DrawingNo"), "", "", "", "", "", 0, "", False)
                If commonFunctions.CheckDataset(dsSubDrawings) = True Then
                    rpBillOfMaterials.DataSource = dsSubDrawings
                    rpBillOfMaterials.DataBind()
                End If

                strCurrentStatusDecoded = ds.Tables(0).Rows(0).Item("approvalStatusDecoded").ToString
                If strCurrentStatusDecoded <> strPreviousStatusDecoded And iCurrentRevision > 0 Then
                    lblStatus.BackColor = Color.Yellow
                    lblStatus.Font.Italic = True
                End If
                lblStatus.Text = strCurrentStatusDecoded

                'Dim row As DataRow
                dsRev = PEModule.GetDrawingRevisions(ViewState("DrawingNo"))
                If commonFunctions.CheckDataset(dsRev) = True Then

                    'For Each row In dsRev.Tables(0).Rows
                    For iRowCounter = 0 To dsRev.Tables(0).Rows.Count - 1
                        'strCurrentDrawingNo = row("DrawingNo").ToString
                        strCurrentDrawingNo = dsRev.Tables(0).Rows(iRowCounter).Item("DrawingNo").ToString

                        iFirstLeftParenthesisLocation = InStr(strCurrentDrawingNo, "(")

                        'get the drawing number without the revision

                        strDrawingNoWithoutRevision = Mid$(strCurrentDrawingNo, 1, iFirstLeftParenthesisLocation)

                        'get the numbers between the parenthesis

                        strRevision = Left$(Right$(strCurrentDrawingNo, 3), 2)
                        strRevision = strRevision.PadLeft(2, "0")


                        strRevision = "(" + strRevision + ")"
                        'strCurrentRevisionNotes += strRevision + "=" & row("revisionNotes").ToString
                        strCurrentRevisionNotes += strRevision + "=" & dsRev.Tables(0).Rows(iRowCounter).Item("revisionNotes").ToString
                        'If row("DrawingNo").ToString <> ViewState("DrawingNo") Then
                        If strCurrentDrawingNo <> ViewState("DrawingNo") Then
                            strCurrentRevisionNotes += ";      "
                        End If
                    Next

                    If strCurrentRevisionNotes <> strPreviousRevisionNotes And iCurrentRevision > 0 Then
                        lblRevisionNotes.BackColor = Color.Yellow
                        lblRevisionNotes.Font.Italic = True
                    End If
                    lblRevisionNotes.Text = strCurrentRevisionNotes

                End If ' if dsRev exists

            End If ' if ds exists
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

End Class
