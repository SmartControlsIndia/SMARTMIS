<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="AddXRayOperator.aspx.cs" Inherits="SmartMIS.Input.AddXRayOperator" Title="Add X-Ray Operator" %>
 <%@ Register src="~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
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
<table width="100%" class="bder">
<tr>
                    <td class="inputFirstCol"></td>
                    <td class="inputSecondCol"></td>
                    <td class="inputThirdCol"></td>
                    <td class="inputForthCol"></td>
                </tr>
                
 <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Add X-Ray Operator </p>
                        </div>
                    </td>
                </tr>
                </table>
                </br>
                <table width="100%" class="bder">
<tr>
<td class="tablecolumn"> X-Ray :</td>
<td class="tablecolumn" >
 <asp:DropDownList ID="dpdxray" runat="server">
                 <asp:ListItem Value="101"> 7201</asp:ListItem>
                  <asp:ListItem Value="271"> X-Ray 2</asp:ListItem>
                </asp:DropDownList>:</td>
    <td class="tablecolumn">Operator:</td>
 <td class="tablecolumn" > 
   <asp:DropDownList ID="dpdop1" runat="server">
                </asp:DropDownList>
   
</td>
   <td class="tablecolumn">
                        <asp:Button ID="bdSaveButton" runat="server" CssClass="masterButton" 
                            Text="Save" onclick="btnSave_Click"  />&nbsp;
                       &nbsp;                       
                    </td>
   <td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td><td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td><td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>  

    
</tr>

</table>
<%--<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="false" BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="260px"></asp:Label>
--%>
<asp:Label runat="server" ID="lblNoRecord" Text="No Records Found!" Visible="false" BorderStyle= "Solid" BorderWidth="1pt" BackColor="#C3D9FF" Width="100%"></asp:Label>
<table width="100%" class="bder">
<tr>
                    <td class="inputFirstCol"></td>
                    <td class="inputSecondCol"></td>
                    <td class="inputThirdCol"></td>
                    <td class="inputForthCol"></td>
                </tr>
                
 <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Add / Update X-Ray Operator </p>
                        </div>
                    </td>
                </tr>
                </table>
                </br>
<asp:Panel ID="tyreScannigLabel" runat="server" Width="100%" Height="400" ScrollBars="Auto">
<asp:GridView ID="grdOpXray"  HeaderStyle-HorizontalAlign="Left" Width="100%" 
       ShowHeader="true" EmptyDataText="No Record Found"  runat="server">
       <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                            
                                                        
      
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#C3D9FF" Font-Bold="True" ForeColor="black" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        
                </asp:GridView>
</asp:Panel>
</asp:Content>

