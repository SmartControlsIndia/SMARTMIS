<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TBMProductionReport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.TBMProductionReport" %>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

<asp:Content ID="tuoReportMasterContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

<asp:ScriptManager ID= "TBMReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>


<asp:UpdatePanel ID="TBMUpdatePanel" runat="server">
<ContentTemplate>
    
   <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>   
    
     <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%"> select Area &nbsp; ::</td>
     <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">
      <asp:DropDownList ID="AreaDropDownList" Width="100%" runat="server" AutoPostBack="true">
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>TBMTBR</asp:ListItem>    
                   <asp:ListItem>TBMPCR</asp:ListItem>                     
               </asp:DropDownList>             
     </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:5%">&nbsp;&nbsp;From::</td>
        
        <td style="width: 10%">
         <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 10%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
            <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" />

         </td>
                   
   </tr>
</table>

</ContentTemplate>
</asp:UpdatePanel>

<asp:Panel ID="productionReportDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
<table class="innerTable" cellspacing="1">
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Product Name</td>
    <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px" rowspan="2">Recipe Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" rowspan="2"></td>
    <td class="gridViewHeader" style="width:24%; text-align:center; padding:5px" colspan="4">Shift</td>
    <td class="gridViewHeader" style="width:34%; text-align:center; padding:5px" rowspan="2"></td>
</tr>
<tr>
    <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px">A</td>
    <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px">B</td>
    <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px">C</td>
        <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px">Total</td>

     

    
</tr>

</table>

<asp:Panel ID="productionReportDateWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >

    <asp:GridView ID="productionReportDateWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="productionReportDateWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>                                 
        <Columns>
            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                <ItemTemplate>
                        <asp:Label ID="productionReportDateWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="Product Type Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90%">
                <ItemTemplate>
                
                <%--Product Type child Gridview--%>
                <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="productionReportDateWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportDateWiseChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="ProductType ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportDateWiseChildProductTypeIDLabel" runat="server" Text='<%# Eval("ProductTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
                            <Columns>
                                <asp:TemplateField HeaderText="Product Type" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportDateWiseChildProductTypeLabel" runat="server" Text='<%# Eval("ProductName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="89%">
                                    <ItemTemplate>
                                        
                                        <%--child Gridview--%>
                                        
                                        <asp:GridView ID="productionReportDateWiseInnerChildGridView" runat="server" AutoGenerateColumns="False" 
                                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportDateWiseInnerChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportDateWiseInnerChildRecipeIDLabel" runat="server" Text='<%# Eval("RecipeID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportDateWiseInnerChildProductTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>                       
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportDateWiseInnerChildRecipeNameLabel" runat="server" Text='<%# Eval("RecipeName") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                 <Columns>
                                                    <asp:TemplateField HeaderText="plandtandTime" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportDateWiseInnerChildplandtandTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="65%">
                                                        <ItemTemplate>
                                                            <table class="innerTable" cellpadding="2" cellspacing="3" style="border: solid 1px #C3D9FF;">
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">Plan</td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftAPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftBPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftCPlanLabel" runat="server" Text='<%# plannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildTotalPlanLable" runat="server" Text='<%# totalplannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"),DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                    <td style="width:300px; text-align:center; padding:5px" rowspan="3">
                                                                        <asp:Chart ID="productionReportDailyChart" runat="server" AlternateText="Daily Production Chart" IsSoftShadows="true"
                                                                            RenderType="ImageTag" Height="120px" Width="250px" >
                                                                            <Series>
                                                                                <asp:Series Name="Plan" ChartType="Column" Color="Green" ShadowOffset="0">
                                                                                    
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <Series>
                                                                                <asp:Series Name="Actual" ChartType="Column" Color="Orange">
                                                                                   
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <Series>
                                                                                <asp:Series Name="Difference" ChartType="Column" Color="Red">
                                                                                    
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <ChartAreas>
                                                                                <asp:ChartArea Name="productionReportDailyChartArea" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                                                                                    BorderDashStyle="Solid" BorderWidth="1">
                                                                                    <AxisX>
                                                                                        <MajorGrid Enabled="false"/>
                                                                                    </AxisX>
                                                                                </asp:ChartArea>
                                                                            </ChartAreas>
                                                                        </asp:Chart>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="text-align:center;width:12%; padding:5px">Actual</td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftAActualLabel" runat="server" Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftBActualLabel" runat="server" Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftCActualLabel" runat="server" Text='<%# actualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildTotalActualLable" runat="server" Text='<%# totalActualQuantity(DataBinder.Eval(Container.DataItem,"wcID"),DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"),DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="width:14%; text-align:center; padding:5px">Difference</td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftADifferenceLabel" runat="server" Text='<%# differenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftBDifferenceLabel" runat="server" Text='<%# differenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildShiftCDifferenceLabel" runat="server" Text='<%# differenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C",DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                     <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportDateWiseInnerChildTotalDifferenceLable" runat="server" Text='<%# totalDifferenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"),DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"),DataBinder.Eval(Container.DataItem,"dtandTime"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
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
                </div>
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

<asp:Panel ID="ProductionReportMonthWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >

    <asp:GridView ID="productionReportMonthWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="productionReportMonthWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>                                 
        <Columns>
            <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                <ItemTemplate>
                        <asp:Label ID="productionReportMonthWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="Product Type Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90%">
                <ItemTemplate>
                
                <%--Product Type child Gridview--%>
                <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="productionReportMonthWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportMonthWiseChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="ProductType ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportMonthWiseChildProductTypeIDLabel" runat="server" Text='<%# Eval("ProductTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
                            <Columns>
                                <asp:TemplateField HeaderText="Product Type" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                        <asp:Label ID="productionReportMonthWiseChildProductTypeLabel" runat="server" Text='<%# Eval("ProductName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="89%">
                                    <ItemTemplate>
                                        
                                        <%--child Gridview--%>
                                        
                                        <asp:GridView ID="productionReportMonthWiseInnerChildGridView" runat="server" AutoGenerateColumns="False" 
                                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="WC ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportMonthWiseInnerChildWCIDLabel" runat="server" Text='<%# Eval("wcID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportMonthWiseInnerChildRecipeIDLabel" runat="server" Text='<%# Eval("RecipeID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportMonthWiseInnerChildProductTypeIDLabel" runat="server" Text='<%# Eval("productTypeID") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>                       
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="productionReportMonthWiseInnerChildRecipeNameLabel" runat="server" Text='<%# Eval("RecipeName") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                              
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="65%">
                                                        <ItemTemplate>
                                                            <table class="innerTable" cellpadding="2" cellspacing="3" style="border: solid 1px #C3D9FF;">
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">Plan</td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftAPlanLabel" runat="server" Text='<%# monthPlannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftBPlanLabel" runat="server" Text='<%# monthPlannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftCPlanLabel" runat="server" Text='<%# monthPlannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C")%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildTotalPlanLable" runat="server" Text='<%# totalMonthPlannedQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                    <td style="width:300px; text-align:center; padding:5px" rowspan="3">
                                                                        <asp:Chart ID="productionReportMonthlyChart" runat="server" AlternateText="Monthly Production Chart" IsSoftShadows="true"
                                                                            RenderType="ImageTag" Height="120px" Width="250px" >
                                                                            <Series>
                                                                                <asp:Series Name="Plan" ChartType="Column" Color="Green" ShadowOffset="0">
                                                                                    
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <Series>
                                                                                <asp:Series Name="Actual" ChartType="Column" Color="Orange">
                                                                                   
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <Series>
                                                                                <asp:Series Name="Difference" ChartType="Column" Color="Red">
                                                                                    
                                                                                </asp:Series>
                                                                            </Series>
                                                                            <ChartAreas>
                                                                                <asp:ChartArea Name="productionReportMonthlyChartArea" BackGradientStyle="TopBottom" BackSecondaryColor="#B6D6EC"
                                                                                    BorderDashStyle="Solid" BorderWidth="1">
                                                                                    <AxisX>
                                                                                        <MajorGrid Enabled="false"/>
                                                                                    </AxisX>
                                                                                </asp:ChartArea>
                                                                            </ChartAreas>
                                                                        </asp:Chart>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="text-align:center;width:12%; padding:5px">Actual</td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftAActualLabel" runat="server" Text='<%# monthActualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftBActualLabel" runat="server" Text='<%# monthActualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftCActualLabel" runat="server" Text='<%# monthActualQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildTotalActualLable" runat="server" Text='<%# totalMonthActualQuantity(DataBinder.Eval(Container.DataItem,"wcID"),DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="gridViewAlternateHeader" style="width:14%; text-align:center; padding:5px">Difference</td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftADifferenceLabel" runat="server" Text='<%# monthDifferenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "A")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftBDifferenceLabel" runat="server" Text='<%# monthDifferenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "B")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                    <td style="width:9%;text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildShiftCDifferenceLabel" runat="server" Text='<%# monthDifferenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"), DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"), "C")%>' CssClass="gridViewItems"></asp:Label>
                                                                    </td>
                                                                     <td style="width:9%; text-align:center; padding:5px">
                                                                        <asp:Label ID="productionReportMonthWiseInnerChildTotalDifferenceLable" runat="server" Text='<%# totalMonthDifferenceQuantity(DataBinder.Eval(Container.DataItem,"wcID"),DataBinder.Eval(Container.DataItem,"recipeID"), DataBinder.Eval(Container.DataItem,"productTypeID"))%>' CssClass="gridViewItems"></asp:Label>
                                                                        
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
                </div>
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
    <table width="100%">
    <tr>
        <td width="45%" class="gridViewItems">Total</td>
        <td width="6%" align="center" class="gridViewItems"><%= shift_a_count %></td>
        <td width="6%" align="center" class="gridViewItems"><%= shift_b_count %></td>
        <td width="6%" align="center" class="gridViewItems"><%= shift_c_count %> </td>
        <td width="40%" align="center" class="gridViewItems"><%= (shift_a_count+shift_b_count+shift_c_count)%></td>
    </tr>
</table>
</asp:Panel>


</asp:Panel>

</asp:content>

