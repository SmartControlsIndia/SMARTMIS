<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mould.ascx.cs" Inherits="SmartMIS.UserControl.mould" %>

    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    
<asp:Panel ID="mouldDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
            <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" rowspan="2">Side</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Mould Name</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Bladder Name</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" rowspan="2">Recipe</td>
            <td class="gridViewHeader" style="width:30%; text-align:center; padding:5px" colspan="3">Shift</td>
           <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" colspan="3"></td>

        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">C</td>
           <td class="gridViewAlternateHeader" style= "text-align:center; padding:5px">Total</td>

        </tr>
     </table>
     <asp:Panel ID="mouldReportDateWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
        <asp:GridView ID="mouldReportDateWiseGridView" runat="server" AutoGenerateColumns="False"
        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                    <ItemTemplate>
                            <asp:Label ID="mouldReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>                                 
            <Columns>
                <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                    <ItemTemplate>
                            <asp:Label ID="mouldReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
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
                                        <asp:GridView ID="mouldReportDateWiseChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="mouldReportDateWiseChildMouldNameLabelLH" runat="server" Text='<%#Eval("mouldCodeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="77%">
                                                    <ItemTemplate>
                                                        <asp:GridView ID="mouldReportDateWiseBladderChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Bladder Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="mouldReportDateWiseChildBladderNameLabelLH" runat="server" Text='<%#Eval("bladderCodeLH")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseRecipeChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Bladder Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildRecipeNameLabelLH" runat="server" Text='<%# Eval("recipeName")%>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>    
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftAChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="A" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftALabelLH" runat="server" Text='<%# Eval("AshiftCountLH")%>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftBChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="B" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftBLabelLH" runat="server" Text='<%# Eval("BshiftCountLH")%>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftCChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="C" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftCLabelLH" runat="server" Text='<%#Eval("CshiftCountLH") %>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseTotalChildGridViewLH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseTotalLabelRH" runat="server" Text='<%# Eval("TotalBladderCountLH")%>' CssClass="gridViewItems"></asp:Label>
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
                                        <asp:GridView ID="mouldReportDateWiseChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="mouldReportDateWiseChildMouldNameLabelRH" runat="server" Text='<%# Eval("mouldCodeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="77%">
                                                    <ItemTemplate>
                                                        <asp:GridView ID="mouldReportDateWiseBladderChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Bladder Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="mouldReportDateWiseChildBladderNameLabelRH" runat="server" Text='<%#Eval("bladderCodeRH")%>' CssClass="gridViewItems"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseRecipeChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Bladder Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildRecipeNameLabelRH" runat="server" Text='<%# Eval("recipeName")%>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>    
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftAChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="A" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftALabelRH" runat="server" Text='<%#Eval("AshiftCountRH")%>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftBChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="B" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftBLabelRH" runat="server" Text='<%# Eval("BshiftCountRH") %>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseShiftCChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="C" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseChildShiftCLabelRH" runat="server" Text='<%# Eval("CshiftCountRH") %>' CssClass="gridViewItems"></asp:Label>
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
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Width="12%">
                                                                        <ItemTemplate>
                                                                            <asp:GridView ID="mouldReportDateWiseTotalChildGridViewRH" runat="server" AutoGenerateColumns="False" 
                                                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                                                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="true" ItemStyle-Width="100%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="mouldReportDateWiseTotalLabelRH" runat="server" Text='<%# Eval("TotalBladderCountRH")%>' CssClass="gridViewItems"></asp:Label>
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