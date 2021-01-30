<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="utility.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.utility" %>

<asp:Content ID="utilityContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
<link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="masterFirstCol"></td>
            <td class="masterSecondCol"></td>
            <td class="masterThirdCol"></td>
            <td class="masterForthCol"></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Utility</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">Utility Name :</td>
            <td class="masterTextBox">
                <asp:TextBox ID="utilityNameTextBox" runat="server" Width="80%"></asp:TextBox>
                <span class="errorSpan">*</span>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Unit :</td>
            <td class="masterTextBox">
            <asp:DropDownList ID="utilityUnitDropDownList" runat="server" Width="82%" > </asp:DropDownList>
        </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td class="masterLabel">Description :</td>
            <td class="masterTextBox">
            <asp:TextBox ID="utilityDescriptionTextBox" runat="server" TextMode="MultiLine" Rows="3" Width="80%"></asp:TextBox>
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
            <td></td>
            <td>
                <asp:Button ID="utilityAddButton" runat="server" CssClass="masterButton" Text="Add" />&nbsp;
                <asp:Button ID="utilityCancelButton" runat="server" CssClass="masterButton" Text="Cancel" />
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">Utility Details</p>
                </div>
            </td>
        </tr>
 
        <tr>
            <td colspan="4">
                <table class="innerTable" cellspacing="0">
                <tr>
                    <td class="gridViewHeader" style="width:30%; padding:5px">Utility Name</td>
                    <td class="gridViewHeader" style="width:10%; padding:5px">Unit</td>
                    <td class="gridViewHeader" style="width:40%; padding:5px">Description</td>
                    <td class="gridViewHeader" style="width:20%; padding:5px"></td>
                </tr>
                </table>
                <asp:Panel ID="utilityPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                    <asp:GridView ID="utilityGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="utilityIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Utility Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="utilityNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Unit" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="utilityUnitLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label ID="utilityDescriptionLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Button ID="utilityEditButton" runat="server" Text="Edit" CssClass="gridViewButtons" />
                                    <asp:Button ID="utilityDeleteButton" runat="server" Text="Delete" CssClass="gridViewButtons" />
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