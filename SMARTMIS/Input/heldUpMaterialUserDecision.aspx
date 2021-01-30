<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" CodeBehind="heldUpMaterialUserDecision.aspx.cs" Inherits="SmartMIS.heldUpMaterialUserDecision" %>

<asp:Content ID="heldUpMaterialUserDecisionContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
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
                    <p class="masterHeaderTagline">HeldUp Material User Decision:</p>
                </div>
            </td>
        </tr>
        <tr>
            <td class="masterLabel">WorkCenter Name :</td>
            <td class="masterTextBox">
                <asp:DropDownList ID="heldUpMaterialDropDownList" runat="server" Width="82%">
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
            <td colspan="4">
                <div class="masterHeader">
                    <p class="masterHeaderTagline">HeldUp Material Details:</p>
                </div>
            </td>
         
        </tr>
        <tr>
            <td colspan="4">
                <table class="innerTable">
                    <tr>
                        <td class="gridViewHeader" style="width:20%; padding:5px">Product Code</td>
                        <td class="gridViewHeader" style="width:20%; padding:5px">Quantity</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px">MHE No.</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px">Reason</td>                   
                        <td class="gridViewHeader" style="width:10%; padding:5px">User Decision</td>
                        <td class="gridViewHeader" style="width:10%; padding:5px">Description</td>
                        <td class="gridViewHeader" style="width:20%; padding:5px"></td>
                    </tr>
                </table>
                <asp:Panel ID="wcPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                <asp:GridView ID="heldUpMaterialUserDecisionGridView" runat="server" AutoGenerateColumns="False" 
                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />               
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader"  Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                            <ItemTemplate>
                                <asp:Label ID="ID" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="heldUpMaterialUserDecisionID" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                        <ItemTemplate>
                            <asp:Label ID="heldUpMaterialUserDecisionID" runat="server" Text='<%# Eval("heldUpMaterialUserDecisionID") %>' CssClass="gridViewItems"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="categoryID" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                        <ItemTemplate>
                            <asp:Label ID="categoryID"  runat="server" Text='<%# Eval("categoryID") %>' CssClass="gridViewItems"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="heldUpMaterialUserDecisionName" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                            <ItemTemplate>
                                <asp:Label ID="heldUpMaterialUserDecisionName"  runat="server" Text='<%# Eval("heldUpMaterialUserDecision Name") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="heldUpMaterialUserDecisionDescription" SortExpression="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%">
                            <ItemTemplate>
                                <asp:Label ID="heldUpMaterialUserDecisionDescription"  runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList2" CssClass="mastertextbox" runat="server" >
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>                        
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                        <asp:TemplateField HeaderText="" SortExpression="PRODORD" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" Text="Save" />                           
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