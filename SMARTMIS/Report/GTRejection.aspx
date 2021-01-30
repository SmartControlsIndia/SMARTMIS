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
       width:13%;
       
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
      
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
       width:15%;
       background-color:#C3D9FF;
       
   }
  .bder
  {
      background-color: inherit; 
   font-size:20px; 
   font-family:Arial Narrow;
   border:1pt solid black;
    border-top-color:black;
    border-width:thin;
    border-bottom-color:black;
    height:30px;
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

<div id="headerdive"> GTRejection Report : </div>
<div>
<div >
<table width="100%" class="bder">
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
    &nbsp;<td class="tablecolumn">
 <asp:Button ID="ExcelButton" CssClass="button"  runat="server" Text="Excel" OnClick="excelButton_Click"/>
    </td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>

</tr>
</div>
   </table><CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
<table width="100%">

<tr>
<td colspan="10">
<div style ="text-align:center; font-size:20px; font-family:Arial Narrow; padding-top:8px; padding-bottom:10px;"> 
<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="false" BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="260px"></asp:Label>
</div>
<div style="height:400px; overflow: scroll;">

    <asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="gridcolumn" AutoGenerateColumns="false">
    <Columns>
    <asp:BoundField HeaderText="Status" DataField="Status" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="DefectName" DataField="DefectName" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="InspectorName" DataField="InspectorName" HeaderStyle-CssClass="tableheadercolumn"/> 
   <asp:TemplateField HeaderText="GTbarcode" HeaderStyle-CssClass="tableheadercolumn">
        <ItemTemplate>
              <asp:HyperLink ID="hlDetails1" Text='<%# Eval("GTbarcode") %>' CssClass="links" runat="server"  NavigateUrl='<%# "~/Report/tyreGeneaology.aspx?gtbarcode=" + Eval("GTbarcode") %>' Target="_blank" /> 

         </ItemTemplate>
       </asp:TemplateField>
  <%--<asp:BoundField HeaderText="GTbarcode" DataField="GTbarcode" HeaderStyle-CssClass="tableheadercolumn" /> --%>
    <asp:BoundField HeaderText="InspectionDate" DataField="InspectionDate" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="InspectionTime" DataField="InspectionTime" HeaderStyle-CssClass="tableheadercolumn" /> 
    <asp:BoundField HeaderText="TBM WCName" DataField="TBM WCName" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="RecipeCode" DataField="RecipeCode" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="Date O.P." DataField="Date O.P." HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="Shift" DataField="Shift" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="Builder1" DataField="Builder1" HeaderStyle-CssClass="tableheadercolumn" /> 
    <asp:BoundField HeaderText="Builder2" DataField="Builder2" HeaderStyle-CssClass="tableheadercolumn"/> 
    <asp:BoundField HeaderText="Builder3" DataField="Builder3" HeaderStyle-CssClass="tableheadercolumn" />

    
    </Columns>
        </asp:GridView>
    
<asp:PlaceHolder ID="GTRejectiondataplaceholder" EnableViewState="true" runat="server"> </asp:PlaceHolder>
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
