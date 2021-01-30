<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="matConsumptionControl.ascx.cs" Inherits="SmartMIS.UserControl.matConsumptionControl" %>

<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<asp:HiddenField id="magicHidden" runat="server" value="" />
<table class="innerTable" cellspacing="1">
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">RawMaterialType Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">RawMaterial Name</td>
    <td class="gridViewHeader" style="width:35%; text-align:center; padding:5px" colspan="3">Shift</td>

</tr>
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">A</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">B</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">C</td>
</tr>

</table>
<asp:Panel ID="productionReportDateWisePanel" runat="server"  CssClass="panel" >
    <asp:GridView ID="productionReportDateWiseGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" 
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" >
                <ItemTemplate>
                        <asp:Label ID="productionReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>                                 
        <Columns>
            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="18%">
                <ItemTemplate>
                        <asp:Label ID="productionReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
                <ItemTemplate>
                    
                    <%--child Gridview--%>
                    
                    <asp:GridView ID="productionReportDateWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="productionRe0portDateWiseChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportDateWiseChildMatTypeNameLabel" runat="server" Text='<%# Eval("materialTypeName") %>' CssClass="gridViewItems" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
                            <Columns>
                                <asp:TemplateField HeaderText="Raw MAterial Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="22%">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportDateWiseChildRawMaterialNameLabel" runat="server" Text='<%# Eval("rawMaterialName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
                                    <ItemTemplate>
                                        <table class="innerTable" cellpadding="2" cellspacing="3" style="border: solid 1px #c1c1c1;">
                                            <tr>                                               
                                                <td style="width:10%; text-align:center; padding:5px">
                                                    <asp:Label ID="productionReportDateWiseChildShiftAPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"materialTypeName"),DataBinder.Eval(Container.DataItem,"rawMaterialName"), "A")%>' CssClass="gridViewItems"></asp:Label>
                                                </td>
                                                <td style="width:10%; text-align:center; padding:5px">
                                                    <asp:Label ID="productionReportDateWiseChildShiftBPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"materialTypeName"),DataBinder.Eval(Container.DataItem,"rawMaterialName"), "B")%>' CssClass="gridViewItems"></asp:Label>
                                                </td>
                                                <td style="width:10%; text-align:center; padding:5px">
                                                    <asp:Label ID="productionReportDateWiseChildShiftCPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"materialTypeName"),DataBinder.Eval(Container.DataItem,"rawMaterialName"), "C")%>' CssClass="gridViewItems"></asp:Label>
                                                </td>                                       
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
    <br />
    </asp:Panel>
<p></p>
    
    <table class="innerTable">
<tr>
<td align="center">
 <asp:Chart ID="matConsumptionChart" runat="server" Width="600px" Visible="false"  >    
                          <Legends>
                          <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" 
                              Title="Legends">                         
                          </asp:Legend>
                      </Legends>
                            <BorderSkin SkinStyle="Sunken" />                      
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Title="Raw Material Name" >                                    
                                        <MajorGrid Enabled="False" />
                                    </AxisX>
                                      <AxisY>
                                        <MajorGrid Enabled="False" />
                                    </AxisY>
                                </asp:ChartArea>
                            </ChartAreas>
</asp:Chart>
</td>
</tr>

<tr>
<td align="center">
 <asp:Chart ID="matConsumpRawMatTypeWiseChart" runat="server" Width="600px" Visible="false"  >    
                          <Legends>
                          <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" 
                              Title="Legends">                         
                          </asp:Legend>
                          </Legends>
                            <BorderSkin SkinStyle="Sunken" />                      
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Title="Raw Material Type" ><MajorGrid Enabled="False" /></AxisX>
                                    <AxisY><MajorGrid Enabled="False" />
                                    </AxisY>
                                </asp:ChartArea>
                            </ChartAreas>
</asp:Chart>
</td>
</tr>

  </table>