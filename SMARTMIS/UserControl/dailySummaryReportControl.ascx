<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dailySummaryReportControl.ascx.cs" Inherits="SmartMIS.dailySummaryReportControl" %>

    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    
        <style type="text/css">
            .style1
            {
                background-color: #5D90E7;
                color : #FFFFFF;
                font-family: Arial;
                font-size: 12px;
                font-weight: bold;                
                text-align: center;
                padding: 3px;
                height: 23px;
            }
        </style>
    
        <table class="productionReportTable" cellpadding="0">
        <tr>
            <td style="width: 10%"></td>
            <td style="width: 15%"></td>
            <td style="width: 11%"></td>
            <td style="width: 11%"></td>
            <td style="width: 11%"></td>        
            <td style="width: 51%"></td>  
        </tr>
           <tr>
            <td colspan="14" align="center" class="productionReportHeader">
             Plant-Wide Production Summary (Plan/Actual)
            </td>
        </tr>   
        <tr>
            <td class="style1">Type</td>
            <td class="style1"></td>
            <td class="style1">Day</td>
            <td class="style1">Month</td>
            <td class="style1">Year</td>             
            <td class="style1"><asp:Image ID="Image1" runat="server" Width="60%" Height="30px" ImageUrl="~/Images/legend.JPG" /></td> 
        </tr>   
    <tr>
        <td colspan="6" align="center">
        <asp:GridView ID="prodSummaryReportGridView" runat="server" AutoGenerateColumns="False" 
        Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="Gridview_RowBound" 
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Type"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Label ID="prodSummaryTypeLabel" runat="server" Text='<%# Eval("Type") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>            
            <Columns>
             <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="46%">
             <ItemTemplate>
             <table class="innerTable">
             <tr>
             <td class="gridViewAlternateHeader" style="width:31%; text-align:center; padding:5px">Plan</td>
             <td style="width:23%; text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildDayPlanLabel" runat="server" Text='<%# plannedQuantityDay(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; width:23%; padding:3px">
             <asp:Label ID="productionReportDateWiseInnerChildMonthPlanLabel" runat="server" Text='<%# plannedQuantityMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center;width:23%; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildYearPlanLabel" runat="server" Text='<%# plannedQuantityYear(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>     
             </td>
             </tr> 
             <tr>
             <td class="gridViewAlternateHeader" style="text-align:center;padding:5px">Actual</td>
             <td style="text-align:center;  padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildDayActualLabel" runat="server" Text='<%# actualQuantityDay(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center;padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildMonthActualLabel" runat="server" Text='<%# actualQuantityMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildYearActualLabel" runat="server" Text='<%# actualQuantityYear(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             </tr>
             <tr>
             <td class="gridViewAlternateHeader" style=" text-align:center; padding:5px">Difference</td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildDayDifferenceLabel" runat="server" Text='<%# differenceQuantityDay(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildMonthDifferenceLabel" runat="server" Text='<%# differenceQuantityMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseInnerChildYearDifferenceLabel" runat="server" Text='<%# differenceQuantityYear(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             </tr>                                                   
             </table>
             </ItemTemplate>
             </asp:TemplateField>
           </Columns>
           <Columns>
            <asp:TemplateField ItemStyle-Width="40%">
                <ItemTemplate>
                     <asp:Chart ID="prodSummaryReportDailyChart" runat="server" AlternateText="Daily Production Chart" IsSoftShadows="true" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                         RenderType="ImageTag" Height="110px" Width="300px"  Palette="BrightPastel">                
                         <ChartAreas><asp:ChartArea Name="ChartArea1"></asp:ChartArea></ChartAreas>
                         <Series><asp:Series Name="Plan"></asp:Series></Series>
                         <Series><asp:Series Name="Actual"></asp:Series></Series>
                         <Series><asp:Series Name="Difference"></asp:Series></Series>                         
                        </asp:Chart>
                </ItemTemplate>
            </asp:TemplateField>
           </Columns>                               
        <PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle CssClass="productionReportActualPlanHeader" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
        </asp:GridView>
        </td>    
    </tr>    
    <tr>
     <td colspan="6" align="center">
         <asp:GridView ID="totalGridView" runat="server" AutoGenerateColumns="False" 
        Width="100%" CellPadding="3" CellSpacing="3" ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="Gridview_RowTotalBound" 
        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Type"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Label ID="prodSummaryTypeLabel" runat="server" Text='<%# Eval("Type") %>' CssClass="gridViewItems"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns> 
                       
            <Columns>
             <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="46%">
             <ItemTemplate>
             <table class="innerTable" cellpadding="2" cellspacing="3" >
             <tr>
             <td class="gridViewAlternateHeader" style="width:31%; text-align:center; padding:5px">Plan</td>
             <td style="width:23%; text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildDayPlanLabel" runat="server" Text='<%# plannedQuantityTotalDay(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; width:23%; padding:3px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildMonthPlanLabel" runat="server" Text='<%# plannedQuantityTotalMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center;width:23%; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildYearPlanLabel" runat="server" Text='<%# plannedQuantityTotalYear(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>   
             </tr> 
             <tr>
             <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">Actual</td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildDayActualLabel" runat="server" Text='<%# actualQuantityTotalDay(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildMonthActualLabel" runat="server" Text='<%# actualQuantityTotalMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildYearActualLabel" runat="server" Text='<%# actualQuantityTotalYear(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             </tr>
             <tr>
             <td class="gridViewAlternateHeader" style="text-align:center; padding:5px">Difference</td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildDayDifferenceLabel" runat="server" Text='<%# differenceQuantityTotalDay(DataBinder.Eval(Container.DataItem,"Type")) %>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildMonthDifferenceLabel" runat="server" Text='<%# differenceQuantityTotalMonth(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             <td style="text-align:center; padding:5px">
             <asp:Label ID="productionReportDateWiseTotalInnerChildYearDifferenceLabel" runat="server" Text='<%# differenceQuantityTotalYear(DataBinder.Eval(Container.DataItem,"Type"))%>' CssClass="gridViewItems"></asp:Label>
             </td>
             </tr>                                                   
             </table>
             </ItemTemplate>
             </asp:TemplateField>
           </Columns>
           <Columns>
            <asp:TemplateField ItemStyle-Width="40%">
                <ItemTemplate>
                     <asp:Chart ID="prodSummaryReportTotalChart" runat="server" AlternateText="Daily Production Chart" IsSoftShadows="true" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                         RenderType="ImageTag" Height="110px" Width="300px"  Palette="BrightPastel">                
                         <ChartAreas><asp:ChartArea Name="ChartArea1"></asp:ChartArea></ChartAreas>
                         <Series><asp:Series Name="Plan"></asp:Series></Series>
                         <Series><asp:Series Name="Actual"></asp:Series></Series>
                         <Series><asp:Series Name="Difference"></asp:Series></Series>                        
                        </asp:Chart>
                </ItemTemplate>
            </asp:TemplateField>
           </Columns>
                               
        <PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle CssClass="productionReportActualPlanHeader" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
        </asp:GridView>
        </td>    
    </tr>
   </table>    
<p>&nbsp;</p>