<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BladderReport.ascx.cs" Inherits="SmartMIS.UserControl.BladderReport" %>

  <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
         <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    
<asp:Panel ID="bladderReportDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:16%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
            <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Side</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Current Bladder Heat</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" colspan="2">Old Bladder Heat </td>

        </tr>
     </table>
     <asp:Panel ID="bladderReportDateWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
        <asp:GridView ID="bladderReportDateWiseGridView" runat="server" AutoGenerateColumns="False"
        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                    <ItemTemplate>
                            <asp:Label ID="bladderReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>                                 
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="35%">
                    <ItemTemplate>
                            <asp:Label ID="bladderReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>  
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90%">
                    <ItemTemplate>
                            <table class="innerTable" style="border: solid 1px #C3D9FF;" cellspacing="1">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 95%"></td>
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="text-align: center; padding-top: 2px; padding-bottom: 2px;">
                                        LH
                                    </td>
                                    <td>
                                        <asp:GridView ID="bladderReportDateWiseChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldHeatLabelLH" runat="server" Text='<%#Eval("CurrentBladderHeatLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldHeatLabelLH" runat="server" Text='<%#Eval("OldBladderHeatLH")%>' CssClass="gridViewItems"></asp:Label>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td class="gridViewAlternateHeader" style="text-align: center; padding-top: 2px; padding-bottom: 2px;">
                                        RH
                                    </td>
                                    <td>
                                        <asp:GridView ID="bladderReportDateWiseChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                           <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildCurrentMouldHeatLabelRH" runat="server" Text='<%#Eval("CurrentBladderHeatRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                                 <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="bladderReportDateWiseChildOldMouldHeatLabelRH" runat="server" Text='<%#Eval("OldBladderHeatRH")%>' CssClass="gridViewItems"></asp:Label>
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
                                    </td>
                                </tr>
                             </table>
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
    
</asp:Panel>   