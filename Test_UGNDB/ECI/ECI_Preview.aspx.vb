' ************************************************************************************************
'
' Name:		ECI_ECI_Preview
' Purpose:	This Code Behind is for the ECI Previews in Crystal Reports
'
' Date		    Author	    
' 06/11/2009    Roderick Carlson    Created
' 10/09/2009    Roderick Carlson    Modified: added garbage collectin on close
' 06/16/2010    Roderick Carlson    Modified: send view to PDF immediately
' 04/28/2011    Roderick Carlson    Modified: do not let obsolete ECIs be previewed
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class ECI_ECI_Preview

    Inherits System.Web.UI.Page
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim ds As DataSet
            Dim oRpt As ReportDocument = New ReportDocument()
            Dim bObsolete As Boolean = True

            If HttpContext.Current.Request.QueryString("ECINo") <> "" Then

                ViewState("ECINo") = CType(HttpContext.Current.Request.QueryString("ECINo"), Integer)

                ds = ECIModule.GetECI(ViewState("ECINo"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                        bObsolete = ds.Tables(0).Rows(0).Item("Obsolete")
                    End If

                    If ds.Tables(0).Rows(0).Item("ECIType") IsNot System.DBNull.Value Then
                        ViewState("ECIType") = ds.Tables(0).Rows(0).Item("ECIType")
                    Else
                        ViewState("ECIType") = "Internal"
                    End If
                End If


                If bObsolete = False Then
                    If Session("ECIPreviewECINo") <> ViewState("ECINo") Then
                        Session("ECIPreview") = Nothing
                        Session("ECIPreviewECINo") = Nothing
                    End If

                    If (Session("ECIPreview") Is Nothing) Then
                        ' new report document object 
                        If ViewState("ECIType") = "External" Then
                            ViewState("SOPNo") = "Qa-101d"
                            If ViewState("ECINo") >= 200000 Then
                                oRpt.Load(Server.MapPath(".\Forms\") & "ECIExternal.rpt")
                            Else
                                oRpt.Load(Server.MapPath(".\Forms\") & "ECIExternalArchiveRev.rpt")
                            End If
                        Else
                            ViewState("SOPNo") = "Qa-101a"
                            If ViewState("ECINo") >= 200000 Then
                                oRpt.Load(Server.MapPath(".\Forms\") & "ECIInternal.rpt")
                            Else
                                oRpt.Load(Server.MapPath(".\Forms\") & "ECIInternalArchiveRev.rpt")
                            End If

                        End If

                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@ECINo", ViewState("ECINo"))
                        oRpt.SetParameterValue("@SOPNo", ViewState("SOPNo"))

                        Session("ECIPreviewECINo") = ViewState("ECINo")
                        Session("ECIPreview") = oRpt

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=ECI-" & ViewState("ECINo").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    Else
                        oRpt = CType(Session("ECIPreview"), ReportDocument)
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"
                        Response.AddHeader("content-disposition", "attachment;filename=ECI-" & ViewState("ECINo").ToString & "preview.pdf")
                        Response.BinaryWrite(oStream.ToArray())
                    End If
                Else
                    lblMessage.Text = "The ECI has been voided or no longer exists."
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
        If HttpContext.Current.Session("ECIPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("ECIPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("ECIPreview") = Nothing
            HttpContext.Current.Session("ECIPreviewECINo") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
