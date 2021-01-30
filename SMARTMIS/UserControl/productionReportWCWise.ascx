<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="productionReportWCWise.ascx.cs" Inherits="SmartMIS.UserControl.productionReportWCWise" %>

<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
<style>
.ErrorMsgCSS
{
    color :#B5B3B5; padding:7px;width:35%;max-width:35%;height:auto;
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;
    position:fixed;z-index: 1080;top:75px;left: 450px;
    -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background: #1B1B1B;
	background: -moz-linear-gradient(top, #1B1B1B, #0033CC);
	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#1B1B1B), to(#0033CC));
	text-align:center;
}
.popupBut
{
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;color:#5B5B5B;border: 1px solid #666;
}
.colorLabel
{
    height:25px;
    width:40px;
    -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;
    border:0px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
}
.colorLabel:hover
{
    height:28px;
    width:47px;
    -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;
    border:0px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
}
    </style>
<script type="text/javascript">
    function closePopup() {
        document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_productionReportWCDateWise_ErrorMsg').style.display = "none";
    }
</script>
<asp:HiddenField id="magicHidden" runat="server" value="" />

<asp:ScriptManager ID="curingOperatorPlanningScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 <asp:Label ID="queryStringSave" runat="server" Text="" Visible="false"></asp:Label>
 <table cellpadding="2" cellspacing="0">
    <tr>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:6%"></td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:20%">
           
             
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:6%"><!--Operator:--></td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:20%">
           
             <asp:DropDownList ID="TBMProductionReportRecipeDropdownlist" Visible="false" 
                   Width="90%" runat="server" CausesValidation="false"  AutoPostBack="true" AppendDataBoundItems="True"
                   onselectedindexchanged="DropDownList_SelectedIndexChanged">
             </asp:DropDownList>
        </td> 
        <td style="width:48%"></td>       
    </tr>
</table>
<asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="ErrorMsgCSS" Visible="false"></asp:Label>
<asp:Panel ID="productionReportDatewWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
<table class="innerTable" cellspacing="1">
<tr>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:center; padding:5px" rowspan="2">Product Name</td>
    <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px" rowspan="2">Recipe Name</td>
    <td class="gridViewHeader" style="width:10%; text-align:left; padding:5px" rowspan="2"></td>
    <td class="gridViewHeader" style="width:24%; text-align:center; padding:5px" colspan="4">Shift</td>
    <td class="gridViewHeader" style="width:34%; text-align:center; padding:5px" rowspan="2">
        <center><table width="80%">
            <tr align="center">
                <td style="width:33.3%;"><div class="colorLabel" style="background-color:#008000;"></div></td>
                <td style="width:33.3%;"><div class="colorLabel" style="background-color:#FFA500;"></div></td>
                <td style="width:33.3%;"><div class="colorLabel" style="background-color:#FF0000;"></div></td>
            </tr>
            <tr align="center">
                <td>Plan</td>
                <td>Actual</td>
                <td>Difference</td>
            </tr>
        </table></center>
    </td>
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