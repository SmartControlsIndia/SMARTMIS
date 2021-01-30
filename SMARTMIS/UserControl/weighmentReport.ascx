<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="weighmentReport.ascx.cs" Inherits="SmartMIS.UserControl.weighmentReport" %>


    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/downtime.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Workcenter Name</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Recipe Name</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Material Code</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Batches</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Set Value</td>
            <td class="gridViewHeader" style="width:7%; padding:5px; text-align:left">Lo Limit</td>
            <td class="gridViewHeader" style="width:7%; padding:5px; text-align:left">Hi Limit</td>
            <td class="gridViewHeader" style="width:6%; padding:5px; text-align:left">Average</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">Sigma</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">3 Sigma</td>
            <td class="gridViewHeader" style="width:10%; padding:5px; text-align:left">6 Sigma</td>
        </tr>
    </table>
    <asp:Panel ID="weighmentReportPanel" runat="server" ScrollBars="None" CssClass="panel" >
        <asp:GridView ID="weighmentReportGridView" runat="server" AutoGenerateColumns="False" 
        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportWCNameLabel" runat="server" Text='<%# Eval("name") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportRecipeNameLabel" runat="server" Text='<%# Eval("recipeName") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Material Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportMaterialNameLabel" runat="server" Text='<%# Eval("rawMaterialName") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Batches" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportBatchLabel" runat="server" Text='<%# Eval("batches") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Set Value" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportSetValueLabel" runat="server" Text='<%# Eval("setValue") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Lo Limit" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportLoLimitLabel" runat="server" Text='<%# Eval("loLimit") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Hi Limit" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportHiLimitLabel" runat="server" Text='<%# Eval("hiLimit") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Average" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportAverageLabel" runat="server" Text='<%# Eval("average") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="Sigma" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportSigmaLabel" runat="server" Text='<%# Eval("sigma") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="3 Sigma" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportThreeSigmaLabel" runat="server" Text='<%# Eval("threeSigma") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="6 Sigma" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="weighmentReportSixSigmaLabel" runat="server" Text='<%# Eval("sixSigma") %>' CssClass="gridViewItems"></asp:Label>
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