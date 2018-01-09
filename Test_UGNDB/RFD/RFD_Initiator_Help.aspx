<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_Initiator_Help.aspx.vb"
    Inherits="RFD_RFD_Initiator_Help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Request for Development (RFD) INITIATOR Help</title>
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
            Request For Development (RFD) Help FOR INITIATORS</p>
        <br />
        <p class="c_textbold" style="color: Red">
            Required Fields</p>
        <p class="c_textbold" style="color: Blue">
            Desc. Tab</p>
        <li><b>Initiator Name</b></li>
        <li><b>Business Process Type</b></li>
        <br />
        <span class="c_text">&nbsp;&nbsp;&nbsp;- Program Management and Sales can create the
            Business Process Type of RFQ but NOT RFC<br />
            &nbsp;&nbsp;&nbsp;- No other team members can create the Business Process Type of
            RFQ. </span>
        <li><b>Designation Type</b></li>
        <br />
        <span class="c_text">&nbsp;&nbsp;&nbsp;- When a finished good, then <b>Make</b> and
            <b>Account Manager</b> are required.<br />
            &nbsp;&nbsp;&nbsp;- When raw material, then a <b>Family</b> is required.</span>
        <li><b>Description</b> of the RFD</li>
        <br />
        <p class="c_textbold" style="color: Blue">
            Customer PartNo and F.G. PartNo Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- When the designation type is a finished good,
            then the <b>Customer PartNo</b> is required.<br />
            &nbsp;&nbsp;&nbsp;- Searching for an <b>existing Customer PartNo and DMS DrawingNo</b>
            will simplify everyone's work and speed up the entire RFD process.<br />
            &nbsp;&nbsp;&nbsp;- Once finding an existing DMS DrawingNo, selecting which <b>measurement
                or construction details</b> need to be changed in the lower section will also
            help.<br />
            &nbsp;&nbsp;&nbsp;- Nothing is required on this tab for child parts (that are NOT
            finished goods). </span>
        <br />
        <p class="c_textbold" style="color: Blue">
            Child PartNo</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- When the designation type is a raw material,
            formula, or semi-finished good, then either a <b>Current Child BPCS Part Number or New
                Child BPCS Part Name</b> is required.<br />
            &nbsp;&nbsp;&nbsp;- Again, searching for an <b>existing BPCS PartNo and DMS DrawingNo</b>
            will simplify everyone's work and speed up the entire RFD process.<br />
            &nbsp;&nbsp;&nbsp;- Once finding an existing DMS DrawingNo, selecting which <b>measurement
                or construction details</b> need to be changed in the lower section will also
            help.<br />
            &nbsp;&nbsp;&nbsp;- Nothing is required on this tab for parts that ARE finished
            goods.<br />
        </span>
        <br />
        <p class="c_textbold" style="color: Blue">
            Customer Program Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- At least one <b>program</b> and program <b>
            year</b><br />
        </span>
        <br />
        <p class="c_textbold" style="color: Blue">
            UGN Facility Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;- At least one <b>UGN Facility</b></span><br />
        <br />
        <p class="c_textbold" style="color: Blue">
            Approval Status Tab</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;Once the information has been saved, go to the
            Approval Status tab and <b>click the submit approval button</b>.<br />
            &nbsp;&nbsp;&nbsp;First level Approvers will be notified.<br />
            &nbsp;&nbsp;&nbsp;The Director of Quality will be CCed.<br />
            &nbsp;&nbsp;&nbsp;Any team members tied to the Make will be carbon copied in the
            email notification.<br />
            &nbsp;&nbsp;&nbsp;The Account Manager, if not the initiator, will be carbon copied
            in the email notification.<br />
            &nbsp;&nbsp;&nbsp;Initiators will be notified of any <b>rejections</b> and have
            to <b>resubmit the RFD once corrected</b>.<br />
            &nbsp;&nbsp;&nbsp;Once all approvers have completed their tasks and have approved
            the RFD, all will be notified that it is complete.</span><br />
        <br />
        <p class="c_textbold" style="color: Blue">
            Business Awarded Button</p>
        <span class="c_text">&nbsp;&nbsp;&nbsp;For the Business Process Type of RFQ, initiators
            who in Sales or Program Management will <b>click a Business Awarded button once the
            customer approves</b>.<br />
            &nbsp;&nbsp;&nbsp;If the RFD is still pending approval from the customer, the UGN
            Team Members approval routing will stop once the Costing department has been approved.
            Quality Engineering and Purchasing will NOT be notified until the Customer has approved.
        </span>
    </form>
</body>
</html>
