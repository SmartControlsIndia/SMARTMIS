<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="qualityWcWise.ascx.cs" Inherits="SmartMIS.UserControl.qualityWcWise" %>

<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<asp:HiddenField id="magicHidden" runat="server" value="" />

<table class="innerTable" cellspacing="1">
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Product Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Recipe Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" rowspan="2"></td>
    <td class="gridViewHeader" style="width:35%; text-align:center; padding:5px" colspan="3">Shift</td>
    <%--<td class="gridViewHeader" style="width:35%; text-align:center; padding:5px" rowspan="2"></td>--%>
</tr>
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">A</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">B</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px">C</td>
</tr>
</table>

<asp:Panel ID="productionReportDateWisePanel" runat="server"  CssClass="panel" >

    <asp:GridView ID="productionReportDateWiseGridView" runat="server" 
        AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" 
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="qualityReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                <HeaderStyle CssClass="gridViewHeader" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Workcenter Name">
                <ItemTemplate>
                    <asp:Label ID="productionReportDateWiseWCNameLabel" runat="server" 
                        CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle CssClass="gridViewHeader" />
                <ItemStyle HorizontalAlign="Center" Width="14%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Recipe Name">
                <ItemTemplate>
                    <%--child Gridview--%>
                    <asp:GridView ID="productionReportDateWiseChildGridView" runat="server" 
                        AllowPaging="false" AllowSorting="false" AutoGenerateColumns="False" 
                        CellPadding="3" ForeColor="#333333" GridLines="None" 
                        onrowdatabound="GridView_RowDataBound" PageSize="5" ShowHeader="false" 
                        Width="100%">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" HeaderText="WC ID" 
                                ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="productionReportDateWiseChildWCIDLabel" runat="server" 
                                        CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" HeaderText="Recipe ID" 
                                ItemStyle-HorizontalAlign="center" ItemStyle-Width="15%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="productionReportDateWiseChildMatTypeNameLabel" runat="server" 
                                        CssClass="gridViewItems" Text='<%# Eval("productTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                HeaderText="Raw MAterial Name" ItemStyle-HorizontalAlign="Center" 
                                ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="productionReportDateWiseChildRawMaterialNameLabel" 
                                        runat="server" CssClass="gridViewItems" Text='<%# Eval("recipeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" HeaderText="" 
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                                <ItemTemplate>
                                    <table cellpadding="2" cellspacing="3" class="innerTable" 
                                        style="border: solid 1px #c1c1c1;">
                                        <tr>
                                            <td class="gridViewAlternateHeader" 
                                                style="width:13%; text-align:center; padding:5px">
                                                Good</td>
                                            <td style="width:14%; text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftAPlanLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "A",0)%>'></asp:Label>
                                            </td>
                                            <td style="width:14%; text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftBPlanLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "B",0)%>'></asp:Label>
                                            </td>
                                            <td style="width:14%; text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftCPlanLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "C",0)%>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">
                                                Rejected</td>
                                            <td style="text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftAActualLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "A",1)%>'></asp:Label>
                                            </td>
                                            <td style="text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftBActualLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "B",1)%>'></asp:Label>
                                            </td>
                                            <td style="text-align:center; padding:5px">
                                                <asp:Label ID="productionReportDateWiseChildShiftCActualLabel" runat="server" 
                                                    CssClass="gridViewItems" 
                                                    Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"productTypeName"),DataBinder.Eval(Container.DataItem,"recipeName"), "C",1)%>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                </ItemTemplate>
                <HeaderStyle CssClass="gridViewHeader" />
                <ItemStyle HorizontalAlign="Left" Width="90%" />
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

<br /><br />


<div id="ChartDiv">   
<table class="innerTable">
<tr>
    <td>  
            <asp:Chart ID="qualityWCWiseChart" runat="server" Width="800px" Height="300px"  Visible="false"
            PaletteCustomColors="255, 128, 128; Red; 192, 255, 192; 128, 255, 128; 255, 255, 192; 255, 255, 128" 
            Palette="None">            
            <Legends>
            <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" Title="Legends">                         
            </asp:Legend>
            </Legends> 
                             <Series><asp:Series Name="ShiftA-Good"></asp:Series></Series> 
                             <Series><asp:Series Name="ShiftA-Rej"></asp:Series></Series> 
                             <Series><asp:Series Name="ShiftB-Good"></asp:Series></Series> 
                             <Series><asp:Series Name="ShiftB-Rej"></asp:Series></Series> 
                             <Series><asp:Series Name="ShiftC-Good"></asp:Series></Series> 
                             <Series><asp:Series Name="ShiftC-Rej"></asp:Series></Series> 

<BorderSkin SkinStyle="Sunken" />   
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                 <AxisX>
                                        <MajorGrid Enabled="False" />
                                    </AxisX>
                                      <AxisY>
                                        <MajorGrid Enabled="False" />
                                    </AxisY>
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
    </td>
</tr>    
</table> 
    
    </div>








