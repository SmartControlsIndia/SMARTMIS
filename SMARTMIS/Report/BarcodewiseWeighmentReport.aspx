<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="BarcodewiseWeighmentReport.aspx.cs" Inherits="SmartMIS.Report.BarcodewiseWeighmentReport" Title="Smart MIS" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<asp:Content ID="BarcodeWiseWeighmnetContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<table width="100%"><tr><td width="95%" align="center"><h2>Barcodewise Weighment Report</h2></td></tr></table>

<table cellpadding="0"  cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
<tr>
<td class="tablecolumn" style="width:8%;">Select Process :</td>
<td class="tablecolumn" style="width:15%;">
    <asp:DropDownList ID="processDropDownList" runat="server" style="width:100%;">
        <asp:ListItem Value="Select Process"></asp:ListItem>
        <asp:ListItem Value="PCR"></asp:ListItem>
        <asp:ListItem Value="TBR"></asp:ListItem>
    </asp:DropDownList>
</td>

<td class="tablecolumn"style="width:8%;">&nbsp; Select Date:</td>
<td class="tablecolumn" style="width:15%;"><myControl:calendertextbox ID="fromdatecalendertextbox" runat="server"  Disabled="false" /> </td>
<td colspan="5"></td>
<td class="tablecolumn">
   &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="ViewButton" CssClass="button"  runat="server" Text="View " OnClick="ViewButton_Click" />
    </td>
    <td class="tablecolumn"style="width:40%;">
  &nbsp;<td class="tablecolumn">
 <asp:LinkButton runat="server" ID="btnExport"  onclick="excelButton_Click"><img src="../Images/Excel.jpg" class="imag" /></asp:LinkButton>

    </td>
</tr>

</table>
<%--<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="false" BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="260px"></asp:Label>
--%>
<div style ="text-align:center; font-size:20px; font-family:Arial Narrow; padding-top:8px; padding-bottom:10px;"> 
<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="false" BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="260px"></asp:Label>
</div>
<asp:Panel ID="tyreScannigLabel" runat="server" Width="100%" Height="400" ScrollBars="Auto">
<asp:GridView ID="WeighmentGridView" runat="server" Width="100%" Height="400" CssClass="TBMTable" AutoGenerateColumns="false">
    <Columns>
     <asp:BoundField HeaderText="DATE" DataField="Date" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
      <asp:BoundField HeaderText="TIME" DataField="Time" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
     <asp:BoundField HeaderText="BARCODE" DataField="barcode" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/>
      <asp:BoundField HeaderText="WEIGHT" DataField="weight" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/>  
    <asp:BoundField HeaderText="RECIPENAME" DataField="recipeCode" HeaderStyle-CssClass="tableheadercolumn"  ItemStyle-HorizontalAlign="Left"/> 
     <asp:BoundField HeaderText="SPECWEIGHT" DataField="SpecWeight" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
      <asp:BoundField HeaderText="SAPCODE" DataField="SAPCODE" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
   <%-- <asp:BoundField HeaderText="CURING_RECIPE" DataField="CuringRecipe" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
    <asp:BoundField HeaderText="CURING_SAPCODE" DataField="CuringSapcode" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
   --%>
   <asp:BoundField HeaderText="VARIANCE" DataField="Variance" HeaderStyle-CssClass="tableheadercolumn"  ItemStyle-HorizontalAlign="Center"/>
   
    </Columns>
        </asp:GridView>
        
        <asp:GridView ID="WeighmentGridView1" runat="server" Width="100%" Height="400" CssClass="TBMTable" AutoGenerateColumns="false">
    <Columns>
     <asp:BoundField HeaderText="DATE" DataField="Date" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
      <asp:BoundField HeaderText="TIME" DataField="Time" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
     <asp:BoundField HeaderText="BARCODE" DataField="barcode" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/>
      <asp:BoundField HeaderText="WEIGHT" DataField="weight" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/>  
    <asp:BoundField HeaderText="TBM_RECIPE" DataField="recipeCode" HeaderStyle-CssClass="tableheadercolumn"  ItemStyle-HorizontalAlign="Left"/> 
     <asp:BoundField HeaderText="SPECWEIGHT" DataField="SpecWeight" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
      <asp:BoundField HeaderText="TBM_SAPCODE" DataField="SAPCODE" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
    <asp:BoundField HeaderText="CURING_RECIPE" DataField="CuringRecipe" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/> 
    <asp:BoundField HeaderText="CURING_SAPCODE" DataField="CuringSapcode" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Left"/>   
   <asp:BoundField HeaderText="VARIANCE" DataField="Variance" HeaderStyle-CssClass="tableheadercolumn"  ItemStyle-HorizontalAlign="Center"/>   
    </Columns>
        </asp:GridView>
        
        
 </asp:Panel>

</asp:Content>
