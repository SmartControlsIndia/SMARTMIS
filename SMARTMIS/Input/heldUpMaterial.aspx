<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="heldUpMaterial.aspx.cs" Inherits="SmartMIS.heldUpMaterialInput" %>

<asp:Content ID="heldUpMaterialContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />


    <table class="inputTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="inputFirstCol"></td>
            <td class="inputSecondCol"></td>
            <td class="inputThirdCol"></td>
            <td class="inputForthCol"></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Heldup Material</p>
                </div>
            </td>            
        </tr>
        <tr>
            <td class="masterLabel">WorkCentre Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="heldUpMaterialWCNameDropDownList" runat="server" Width="82%">
                </asp:DropDownList>
            </td>
            <td></td>
            <td> </td>
        </tr>
        <tr>
            <td class="masterLabel">Product Code:</td>
            <td>
                <asp:DropDownList ID="heldUpMaterialProductCodeDropDownList" Width="82%" runat="server">
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Recipe Code: </td>
            <td class="masterTextBox">
                <asp:TextBox ID="heldUpMaterialRecipeCodeTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Quantity:</td>
            <td class="masterTextBox">
                <asp:TextBox ID="heldUpMaterialQuantityTextBox" Width="80%" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Unit of Measure:</td>
            <td class="masterTextBox">
                <asp:Label ID="heldUpMaterialUnitLabel" runat="server" Width="80%" Text="####"></asp:Label>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">MHE Code:</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="heldUpMaterialMHENameDropDownList" Width="82%" runat="server">
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Held Up Reason:</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="heldUpMaterialReasonDropDownList" Width="82%" runat="server">
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="heldUpMaterialAddInButton" runat="server" CssClass="masterButton" Text="Add" />&nbsp;
                <asp:Button ID="heldUpMaterialCancelInButton" runat="server" CssClass="masterButton" Text="Cancel" />
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">HeldUp Material Details:</p>
                </div>
            </td>   
        </tr>
         <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                    <tr>
                        <td class="gridViewHeader" style="width:15%; padding:5px">Product Code</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px">Recipe Code</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px">Quantity</td>
                        <td class="gridViewHeader" style="width:5%; padding:5px">Units</td>
                        <td class="gridViewHeader" style="width:20%; padding:5px">MHE Code</td>
                        <td class="gridViewHeader" style="width:20%; padding:5px">Reason</td>
                        <td class="gridViewHeader" style="width:20%; padding:5px"></td>
                    </tr>
                    </table>
                    <asp:Panel ID="heldUpMaterialPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                        <asp:GridView ID="heldUpMaterialGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                   
                            <Columns>
                                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader"  Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="heldUpMaterialIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Product Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="heldUpMaterialProductCodeLabel" runat="server" Text='<%# Eval("eventID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                            <Columns>
                                <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="heldUpMaterialRecipeCodeLabel"  runat="server" Text='<%# Eval("heldUpMaterialID") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="heldUpMaterialQuantityLabel"  runat="server" Text='<%# Eval("event Name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="heldUpMaterialUnitLabel"  runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="MHE Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="heldUpMaterialMHECodeLabel"  runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Reason" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="heldUpMaterialReasonLabel"  runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                           <Columns>
                            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Button ID="heldUpMaterialEditButton" runat="server" Text="Edit" CssClass="gridViewButtons" />
                                    <asp:Button ID="heldUpMaterialDeleteButton" runat="server" Text="Delete" CssClass="gridViewButtons" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>        
                        <PagerStyle BackColor="#507CD1" ForeColor="White" 
                            HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                </asp:Panel> 
            </td>
         </tr>  
    </table>
</asp:Content>