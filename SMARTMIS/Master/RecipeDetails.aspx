<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="RecipeDetails.aspx.cs" Inherits="SmartMIS.Master.RecipeDetails" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
 <table cellpadding="0" cellspacing="0" style="width: 98%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
<tr> 
<td colspan="10">
<div class="masterHeader">
                    <p class="masterHeaderTagline">Recipe Details</p>
                </div>
</td>
</tr>

<tr>        
<td style="font-weight:bold; font-family:Arial; font-size:small; width:15%">          
              Size:
               <asp:DropDownList ID="ddlRecipe" runat="server"  Width="100" >
               </asp:DropDownList>
                
        </td>
        
    <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" /></td>
 <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp;
  <asp:Button ID="export" runat="server" OnClick="Export_click" Text="Export" />
            &nbsp;</td>
    
<td class="tablecolumn"></td>    
        
        </tr>
 </table>
  <asp:Panel ID="manningPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
<asp:GridView ID="grdRecipe" runat="server" CssClass="TBMTable" 
        HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-BackColor="Gray" 
            ShowHeader="true"  EmptyDataRowStyle-Width="100%" EmptyDataRowStyle-HorizontalAlign="Center"
              Width="100%" EmptyDataText="No Records Found"
            ShowFooter="false"  
       >
     <Columns>
      </Columns>
    </asp:GridView>
    </asp:Panel>



</asp:Content>
