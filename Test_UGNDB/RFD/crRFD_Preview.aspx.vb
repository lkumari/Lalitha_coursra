' ************************************************************************************************
'
' Name:		crRFD_Preview
' Purpose:	This Code Behind is for the RFD Previews in Crystal Reports
'
' Date		    Author	    
' 09/14/2010    Roderick Carlson    Created

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class RFD_Preview
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim ds As DataSet
            Dim oRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Request.QueryString("RFDNo") <> "" Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                    commonFunctions.SetUGNDBUser()
                End If

                ViewState("RFDNo") = CType(HttpContext.Current.Request.QueryString("RFDNo"), Integer)

                'if new RFDNo then just search new system
                If ViewState("RFDNo") >= 200000 Then
                    ds = RFDModule.GetRFD(ViewState("RFDNo"))
                Else
                    ds = RFDModule.GetRFDSearch(ViewState("RFDNo"), "", 0, 0, "", 0, "", "", "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", 0, False, False, True)
                End If

                If commonFunctions.CheckDataSet(ds) = True Then

                    ViewState("BusinessProcessTypeID") = 1
                    If ds.Tables(0).Rows(0).Item("BusinessProcessTypeID") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("BusinessProcessTypeID") > 0 Then
                            ViewState("BusinessProcessTypeID") = ds.Tables(0).Rows(0).Item("BusinessProcessTypeID")
                        End If
                    End If

                    'If HttpContext.Current.Request.QueryString("BusinessProcessTypeID") <> "" Then
                    '    ViewState("BusinessProcessTypeID") = CType(HttpContext.Current.Request.QueryString("BusinessProcessTypeID"), Integer)
                    'End If

                    ViewState("SOPNo") = "QA-173b"
                    If ds.Tables(0).Rows(0).Item("SOPNo").ToString <> "" Then
                        ViewState("SOPNo") = ds.Tables(0).Rows(0).Item("SOPNo").ToString
                    End If

                    'If HttpContext.Current.Request.QueryString("SOPNo") <> "" Then
                    '    ViewState("SOPNo") = HttpContext.Current.Request.QueryString("SOPNo")
                    'End If

                    ViewState("SOPRev") = 1
                    If ds.Tables(0).Rows(0).Item("SOPRev") IsNot System.DBNull.Value Then
                        If ds.Tables(0).Rows(0).Item("SOPRev") >= 0 Then
                            ViewState("SOPRev") = ds.Tables(0).Rows(0).Item("SOPRev")
                        End If
                    End If

                    'If HttpContext.Current.Request.QueryString("SOPRev") <> "" Then
                    '    ViewState("SOPRev") = CType(HttpContext.Current.Request.QueryString("SOPRev"), Integer)
                    'End If

                    ' ''ViewState("ArchiveData") = 0
                    ' ''If HttpContext.Current.Request.QueryString("ArchiveData") <> "" Then
                    ' ''    ViewState("ArchiveData") = CType(HttpContext.Current.Request.QueryString("ArchiveData"), Integer)
                    ' ''End If

                    If Session("RFDPreviewRFDNo") <> ViewState("RFDNo") Then
                        Session("RFDPreview") = Nothing
                        Session("RFDPreviewRFDNo") = Nothing
                    End If

                    If (Session("RFDPreview") Is Nothing) Then

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'need logic for old RFCs (with various SOP versions), old RFQs (with various SOP versions), and new RFDs
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                        'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                        'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                        ' new report document object 
                        If ViewState("RFDNo") >= 200000 Then
                            oRpt.Load(Server.MapPath(".\Forms\") & "RFD.rpt")
                        Else
                            If ViewState("BusinessProcessTypeID") = 1 Then
                                oRpt.Load(Server.MapPath(".\Forms\") & "RFQ_Archive.rpt")
                            Else
                                oRpt.Load(Server.MapPath(".\Forms\") & "RFC_Archive.rpt")
                            End If

                        End If


                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@RFDNo", ViewState("RFDNo"))
                        oRpt.SetParameterValue("@SOPNo", ViewState("SOPNo"))
                        oRpt.SetParameterValue("@SOPRev", ViewState("SOPRev"))

                        Session("RFDPreviewRFDNo") = ViewState("RFDNo")
                        Session("RFDPreview") = oRpt

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=RFD-" & ViewState("RFDNo").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    Else
                        oRpt = CType(Session("RFDPreview"), ReportDocument)

                        'crECIPreview.ReportSource = oRpt
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=RFD-" & ViewState("RFDNo").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    End If
                End If

            End If

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("RFDPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("RFDPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("RFDPreview") = Nothing
            HttpContext.Current.Session("RFDPreviewRFDNo") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
