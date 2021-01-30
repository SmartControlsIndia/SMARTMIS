<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="BudhayTyreScanningReport.aspx.cs" Inherits="SmartMIS.Report.BudhayTyreScanningReport" Title="SmartMIS" %>

    <%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
<table width="100%"><tr><td width="95%" align="center"><h2>TBR BUDDE EXIT STATION REPORT</h2></td></tr></table>
<table cellpadding="0"  cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
<tr>
 
<td class="tablecolumn" >  &nbsp;Duration
<asp:DropDownList ID="ddlmonthselection" runat="server" AutoPostBack="true" onselectedindexchanged="ddlmonthselection_SelectedIndexChanged"
    Width="90px">
       <asp:ListItem Value="Daily">Daily</asp:ListItem>
       <asp:ListItem Value="Monthly" >Range</asp:ListItem>
       
    </asp:DropDownList> :</td>
    <td class="tablecolumn"><asp:Panel id="pnlshift" runat="server">  Select Shift:
   <asp:DropDownList ID="ddlshift" runat="server" AutoPostBack="true"  
    Width="90px">
     <asp:ListItem Value="0">ALL</asp:ListItem>
       <asp:ListItem Value="1">A</asp:ListItem>
       <asp:ListItem Value="2" >B</asp:ListItem>
        <asp:ListItem Value="3" >C</asp:ListItem>
    </asp:DropDownList>
    </asp:Panel>
</td>
     
<td class="tablecolumn" >
&nbsp; Date From :<myControl:calendertextbox ID="fromdatecalendertextbox" runat="server"  Disabled="false" /> 
  
</td>
 
<td class="tablecolumn" > 
<asp:Panel id="pnlto" Visible="false" runat="server"> 
&nbsp;&nbsp;&nbsp; Date To : <myControl:calendertextbox ID="TodateCalendertextbox" runat="server" Disabled="false"/>
</asp:Panel>
</td>
<td class="tablecolumn"  ></td>
<td colspan="5"></td>
<td class="tablecolumn">
    &nbsp;&nbsp;<asp:Button ID="ViewButton" CssClass="button"  runat="server" Text="View " OnClick="ViewButton_Click" />
    </td>
    <td class="tablecolumn" style="width:40%;"></td>
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
<asp:GridView ID="GridView1" runat="server" Width="100%" CssClass="TBMTable" AutoGenerateColumns="false">
    <Columns>
    <asp:BoundField HeaderText="RecipeName" DataField="recipeCode" HeaderStyle-CssClass="tableheadercolumn"  ItemStyle-HorizontalAlign="Center"/> 
    <asp:BoundField HeaderText="Destination" DataField="destination" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
    <asp:BoundField HeaderText="TyreCount" DataField="TyreCount" HeaderStyle-CssClass="tableheadercolumn" ItemStyle-HorizontalAlign="Center"/> 
  
   
    </Columns>
        </asp:GridView>
</asp:Panel>
</asp:Content>
