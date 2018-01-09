' ************************************************************************************************
' Name:	DMSDrawingPreview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Drawing, Drawing Images, and Display an entire Bill of Materials (BOM)
'
' Date		    Author	    
' 09/15/2008    Roderick Carlson			Created .Net application
' 10/23/2008    Roderick Carlson            Modified: For some reason,  strTempAlternativeDrawingNo = "blankstandard" was a default. That should not be the case.
' 06/29/2009    Roderick Carlson            Modified: Checked first if image is empty from DB before assigning it to a variable
' 07/08/2009    Roderick Carlson            Modified: PDE # 2728 - added nonrectagular and noshape to DrawingLayoutType
' 07/28/2009    Roderick Carlson            Modified: Turned the page into a popup
' 08/03/2009    Roderick Carlson            Modified: Adjusted BOM to be built on separate page with selectable SubDrawings.
' 09/17/2009    Roderick Carlson            Modified: Check to see if drawing exists
' 10/09/2009    Roderick Carlson            Modified: Clean cached crystal reports on close
' 06/16/2010    Roderick Carlson            Modified: send view to PDF immediately
' 03/08/2011    Roderick Carlson            Modified: do not allow obsolete drawings to be viewed
' 07/25/2011    Roderick Carlson            Modified: Order Revision Notes in descending order, cleaned up preview to have CRLF per revision, added BOM on second page
' 12/19/2013    LRey                Removed unused code
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class PE_DMSDrawingPreview
    Inherits System.Web.UI.Page

    Private Sub BuildBOMandFindImages()

        Try
            Dim dsTempDrawingInfo As DataSet
            Dim iTempDrawingCounter As Integer = 0
            Dim dsAlternativeDrawingImage As DataSet
            Dim dsTempDrawingRevisions As DataSet
            Dim iTempRevisionCounter As Integer = 0
            Dim strTempRevision As String = ""
            Dim iLeftParenthesisLocation As Integer
            Dim strTempDrawingRevisionNotes As String = ""
            Dim strTempDrawingNo As String
            Dim strTempDrawingLayoutType As String = ""
            Dim strTempAlternativeDrawingNo As String = ""
            Dim ImageBytesTemp As Byte()

            'loop through each temp drawing           
            dsTempDrawingInfo = PEModule.GetTempDrawings(ViewState("DrawingNo"), "")
            If commonFunctions.CheckDataset(dsTempDrawingInfo) = True Then

                For iTempDrawingCounter = 0 To dsTempDrawingInfo.Tables(0).Rows.Count - 1

                    strTempDrawingNo = dsTempDrawingInfo.Tables(0).Rows(iTempDrawingCounter).Item("DrawingNo").ToString

                    'check drawing layout type to see if alternative image is needed
                    strTempDrawingLayoutType = dsTempDrawingInfo.Tables(0).Rows(iTempDrawingCounter).Item("DrawingLayoutType").ToString
                    strTempAlternativeDrawingNo = ""
                    Select Case strTempDrawingLayoutType
                        Case "Blank-Standard"
                            strTempAlternativeDrawingNo = "blankstandard"
                        Case "Rolled-Goods"
                            strTempAlternativeDrawingNo = "rolledgoods"
                        Case "Blank-MD-Critical"
                            strTempAlternativeDrawingNo = "blankmdcritical"
                        Case "Non-Rectangular"
                            strTempAlternativeDrawingNo = "nonrectangularshape"
                        Case "No-Shape"
                            strTempAlternativeDrawingNo = "noshape"
                    End Select

                    'get true drawing image or alternative drawing image
                    dsAlternativeDrawingImage = PEModule.GetDrawingImages(strTempDrawingNo, strTempAlternativeDrawingNo)
                    If commonFunctions.CheckDataset(dsAlternativeDrawingImage) = True Then

                        If dsAlternativeDrawingImage.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
                            ImageBytesTemp = dsAlternativeDrawingImage.Tables(0).Rows(0).Item("DrawingImage")
                            'update image in temp list
                            PEModule.UpdateTempDrawingBOMImage(strTempDrawingNo, ImageBytesTemp)
                        End If

                    End If

                    'Parse each revision of Temp Drawing RevisionNotes, save in a local variable, insert into Temp_Drawing_Maint
                    strTempDrawingRevisionNotes = ""
                    dsTempDrawingRevisions = PEModule.GetDrawingRevisions(strTempDrawingNo)
                    If commonFunctions.CheckDataset(dsTempDrawingRevisions) = True Then

                        For iTempRevisionCounter = dsTempDrawingRevisions.Tables(0).Rows.Count - 1 To 0 Step -1
                            iLeftParenthesisLocation = InStr(dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("DrawingNo").ToString, "(")
                            strTempRevision = Mid$(dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("DrawingNo").ToString, iLeftParenthesisLocation)

                            If dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("RevisionNotes").ToString.Trim <> "" Then
                                strTempDrawingRevisionNotes = strTempRevision & ":" & dsTempDrawingRevisions.Tables(0).Rows(iTempRevisionCounter).Item("RevisionNotes").ToString & vbNewLine & strTempDrawingRevisionNotes
                            End If

                        Next

                        If strTempDrawingNo <> "" And strTempDrawingRevisionNotes <> "" Then
                            PEModule.UpdateTempDrawingBOMRevisionNotes(strTempDrawingNo, strTempDrawingRevisionNotes)
                        End If

                    End If
                Next

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Dim strDebug As String = "Page Load Debug"

        Try
            Dim dsCheckDrawingExist As DataSet
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then
                ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")
                'strDebug += "<br>Drawing No:" & ViewState("DrawingNo")

                dsCheckDrawingExist = PEModule.GetDrawing(ViewState("DrawingNo"))

                If commonFunctions.CheckDataSet(dsCheckDrawingExist) = True Then
                    If dsCheckDrawingExist.Tables(0).Rows(0).Item("Obsolete") = False Then
                        'strDebug += "<br>Drawing Exists"

                        Dim oRpt As ReportDocument = New ReportDocument()

                        If ViewState("DrawingNo") <> Session("DMSDrawingPreviewLastDrawingNoViewed") Then
                            'strDebug += "<br>clearing crystal cache"
                            Session("DMSDrawingPreview") = Nothing
                            Session("DMSDrawingPreviewLastDrawingNoViewed") = Nothing
                        End If

                        If (Session("DMSDrawingPreview") Is Nothing) Then
                            'strDebug += "<br>New Crystal"

                            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                                commonFunctions.SetUGNDBUser()
                            End If

                            'strDebug += "<br>User:" & HttpContext.Current.Request.Cookies("UGNDB_User").Value

                            dsCheckDrawingExist = Nothing
                            'strDebug += "<br>clear object"

                            dsCheckDrawingExist = PEModule.GetTempDrawings(ViewState("DrawingNo"), "")

                            'strDebug += "<br>About to check temp values"

                            'put keydrawing at beginning of list if it is not in there already
                            If commonFunctions.CheckDataSet(dsCheckDrawingExist) = False Then
                                'lblMessage.Text += "checking...."
                                PEModule.DeleteTempDrawingBOM()
                                PEModule.InsertTempDrawingBOM(0, ViewState("DrawingNo"), ViewState("DrawingNo"))
                                'Else
                                '    lblMessage.Text += "no temp values...."
                            End If

                            'strDebug += "<br>Load All"

                            ViewState("CreatedBy") = HttpContext.Current.Request.Cookies("UGNDB_User").Value

                            BuildBOMandFindImages()

                            ' new report document object 
                            oRpt.Load(Server.MapPath(".\Forms\") & "DMSDrawingPreview.rpt")


                            Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                            Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                            Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                            Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                            oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                            oRpt.SetParameterValue("@KeyDrawingNo", ViewState("DrawingNo"))
                            oRpt.SetParameterValue("@DrawingNo", "")
                            oRpt.SetParameterValue("@createdBy", ViewState("CreatedBy"))
                            oRpt.SetParameterValue("@ugndbEnvironment", strProdOrTestEnvironment)

                            Session("DMSDrawingPreview") = oRpt
                            Session("DMSDrawingPreviewLastDrawingNoViewed") = ViewState("DrawingNo")

                            Dim oStream As New System.IO.MemoryStream
                            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                            Response.Clear()
                            Response.Buffer = True
                            Response.ContentType = "application/pdf"

                            Response.Charset = ""
                            Response.AddHeader("content-disposition", "inline;filename=DMS-" & ViewState("DrawingNo").ToString & "preview.pdf")

                            Response.BinaryWrite(oStream.ToArray())
                            'Response.End()

                        Else
                            'strDebug += "<br>Load Cached Crystal"

                            oRpt = CType(Session("DMSDrawingPreview"), ReportDocument)

                            'crDMSDrawingPreview.ReportSource = oRpt

                            Dim oStream As New System.IO.MemoryStream
                            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                            Response.Clear()
                            Response.Buffer = True
                            Response.ContentType = "application/pdf"

                            Response.Charset = ""
                            Response.AddHeader("content-disposition", "inline;filename=DMS-" & ViewState("DrawingNo").ToString & "preview.pdf")

                            Response.BinaryWrite(oStream.ToArray())
                            'Response.End()

                        End If
                    Else
                        lblMessage.Text += "<br><br><br>Error: The DMS Drawing " & ViewState("DrawingNo") & " has been set to obsolete"
                        ViewState("DrawingNo") = ""

                    End If
                Else
                    'strDebug += "<br>No Drawing Found"
                    lblMessage.Text += "<br><br><br>Error: The DMS Drawing " & ViewState("DrawingNo") & " does not exist."
                    ViewState("DrawingNo") = ""
                End If
                'strDebug += "<br>QueryString Empty"

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<Br>" & mb.Name '& strDebug & HttpContext.Current.Session("BLLerror")

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Drawing Preview
            If HttpContext.Current.Session("DMSDrawingPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("DMSDrawingPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DMSDrawingPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<Br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnClosingWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClosingWork.Click

        'PEModule.CleanPEDMScrystalReports()

    End Sub
End Class
