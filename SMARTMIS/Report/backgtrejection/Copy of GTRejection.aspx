<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" CodeBehind="GTRejection.aspx.cs" Inherits="SmartMIS.Report.WebForm1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>

<asp:Content ID="GTRejectionContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server" Visible="true">
    <style type="text/css">
    
   #headerdive
   {
       background-color:#C3D9FF;
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       
   }
   .tablecolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:10%;
       
   }
   tr
   {
      border:1pt solid black;
      
     
   }
   
   .tableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:12%;
       background-color:#C3D9FF;
       
   }
   .button
{
    cursor: pointer;    
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
 endColorstr='#579AEB'); /* for IE */
    -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
    endColorstr='#579AEB'); /* for IE 8 and above */
    background: -webkit-gradient(linear, left top, left bottom, from(#85B6F0),
    to(#579AEB)); /* for webkit browsers */
    background: -moz-linear-gradient(top, #85B6F0, #579AEB); /* for firefox 3.6+ */
    background: -o-linear-gradient(top, #85B6F0, #579AEB); /* for Opera */
    width:100PX;
}
   
</style>
<%--
 <asp:ScriptManager ID="curingOperatorPlanningScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
 <ContentTemplate>--%>
<div id="headerdive"> GTRejection Report : </div>
<div>
<table width="100%">
<tr>
<td class="tablecolumn">Select Process :</td>
<td class="tablecolumn" >
    <asp:DropDownList ID="processDropDownList" runat="server" Width="100%">
        <asp:ListItem Value="Select Process"></asp:ListItem>
        <asp:ListItem Value="PCRGTScrap"></asp:ListItem>
        <asp:ListItem Value="TBRGTScrap"></asp:ListItem>
    </asp:DropDownList>
</td>
<td class="tablecolumn">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date From :</td>
<td class="tablecolumn"><myControl:calendertextbox ID="fromdatecalendertextbox" runat="server" Width="80%" Disabled="false" /> </td>
<td class="tablecolumn"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date To :</td>
<td class="tablecolumn"><myControl:calendertextbox ID="TodateCalendertextbox" runat="server" Width="80%" Disabled="false"/></td>
<td class="tablecolumn">
    <asp:Button ID="ViewButton" CssClass="button"  runat="server" Text="View " 
        onclick="ViewButton_Click"/>
    </td>
<td class="tablecolumn">&nbsp; </td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>

</tr>
<tr>
<td colspan="10">
<div>
<asp:PlaceHolder ID="GTRejectiondataplaceholder" runat="server"> </asp:PlaceHolder>
</div>
<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" vi AutoDataBind="true" />

</div>

</td>
</tr>
</table></div>
 <%--</ContentTemplate>
    <Triggers>
    </Triggers>
    </asp:UpdatePanel>
--%>
</asp:Content>
