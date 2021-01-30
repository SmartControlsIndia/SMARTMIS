<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="leftOverMaterial.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.materialLeftOver" %>

<asp:Content ID="leftOverMaterialContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
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
                    <p class="masterHeaderTagline">Matarial Left Over</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Work Center Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="leftOverMaterialWCDropDownList" runat="server" Width="82%">
                </asp:DropDownList></td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Raw Material Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="leftOverMaterialRawMaterialDropDownList" runat="server" Width="82%">
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Quantity</td>
            <td>
                <asp:TextBox ID="leftOverMaterialQuantityTextBox" runat="server" Width="80%"></asp:TextBox>
               <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td>
                <asp:RequiredFieldValidator ID="leftOverMaterialRequiredFieldValidator" runat="server" 
                    ErrorMessage="Quantity is required" ControlToValidate="leftOverMaterialQuantityTextBox"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="leftOverMaterialAddButton" runat="server" CssClass="masterButton" Text="Add" />&nbsp;
                <asp:Button ID="leftOverMaterialCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
            </td>
            <td></td>
            <td></td>
        </tr>
         <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Left Over Material List</p>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:40%; padding:5px">Work Center</td>
                    <td class="gridViewHeader" style="width:30%; padding:5px">Raw Matarial Name</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Quantity</td>
                    <td class="gridViewHeader" style="width:20%; padding:5px"></td>
                </tr>
                </table>
                <asp:Panel ID="leftOverMaterialPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="leftOverMaterialGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="leftOverMaterialIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Work Center" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="leftOverMaterialWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="LeftOver Material Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="leftOverMaterialRawMaterialNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="leftOverMaterialQuantityLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Button ID="leftOverMaterialEditButton" CausesValidation="false" runat="server" Text="Edit" CssClass="gridViewButtons" />
                                    <asp:Button ID="leftOverMaterialDeleteButton" CausesValidation="false" runat="server" Text="Delete" CssClass="gridViewButtons" />
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
