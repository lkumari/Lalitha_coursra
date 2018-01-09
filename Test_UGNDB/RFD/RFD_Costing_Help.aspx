<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_Costing_Help.aspx.vb" Inherits="RFD_RFD_Costing_Help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Request for Development (RFD) COSTING Help</title>
</head>
<body>
     <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <a href="javascript:window.print()">
            <img src="../images/printer.jpg" alt="Print" style="border: 0" />Click to Print
            This Page</a><br />
        <br />
        <p class="p_bigtextbold">
            Request For Development (RFD) Help FOR COSTING</p>
        <br />
        <p class="c_textbold" style="color: Red">
            Required Fields</p>
        <br />
        <p class="c_textbold" style="color: Blue">
            Customer PartNo and F.G. PartNo Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- When the designation type is a finished good,
            then the <b>New Cost Sheet ID</b> is required.<br />           
            &nbsp;&nbsp;&nbsp;- Nothing is required on this tab for child parts (that are NOT
            finished goods). </span>
        <br />
        <p class="c_textbold" style="color: Blue">
            Child PartNo</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- When the designation type is a raw material,
            formula, or semi-finished good, then either a <b>New Cost Sheet ID or External RFQ Number</b> is required for
            each child part.<br />            
            &nbsp;&nbsp;&nbsp;- Nothing is required on this tab for parts that ARE finished
            goods.<br />
        </span>
        <br />
        <p class="c_textbold" style="color: Blue">
            Approval Status Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;Once the information has been saved, go to the
            Approval Status tab and <b>click the submit button</b>.<br />
            &nbsp;&nbsp;&nbsp;Rejections require a comment.<br />
            &nbsp;&nbsp;&nbsp;<b>EACH RAW MATERIAL WILL REQUIRE A NEW BPCS PART NUMBER OR REVISION BEFORE APPROVAL</b>.<br />
            &nbsp;&nbsp;&nbsp;Initiators will be notified of any <b>rejections</b> and have
            to <b>resubmit the RFD once corrected</b>.<br />
            &nbsp;&nbsp;&nbsp;Once all approvers have completed their tasks and have approved
            the RFD, all will be notified that it is complete.</span><br />
        <br />
        <p class="c_textbold" style="color: Blue">
            Communication Board Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;If a team member has a question for the group
            of approvers and the initiator before actually completing tasks or approving the
            RFD, then the question can be sent to all in the communication board. All questions
            and answers will be saved and even shown on the history page. </span>
    </form>
</body>
</html>
