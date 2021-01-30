<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="production.aspx.cs" MasterPageFile="~/smartMISReportMaster.Master" Inherits="SmartMIS.productionReport" Title="TBM Production Report" %>


<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">

<%@ Register src="../UserControl/productionReportWCWise.ascx" tagname="productionReportWCWise" tagprefix="asp" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>


    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script type="text/javascript" language="javascript">
    
        function showReport(queryString)
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_productionReportWCDateWise_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
        
    </script>
    
    <asp:HiddenField id="magicHidden" runat=server value="" />   
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    <asp:reportHeader ID="reportHeader" runat="server" />
    <asp:productionReportWCWise ID="productionReportWCDateWise" runat="server" />
      
    <div id="PlantWideDiv" runat="server" visible="false">
    <div id="DateWiseReportDiv" runat="server" visible="false">
    <table class="productionReportTable" cellpadding="0">    
         
          <tr>
            <td class="productionReportDailyFirstCol"></td>
            <td class="productionReportDailySecondCol"></td>
            <td class="productionReportDailyThirdCol"></td>
            <td class="productionReportDailyForthCol"></td>
            <td class="productionReportDailyFifthCol"></td>
            <td class="productionReportDailySixthCol"></td>
            <td class="productionReportDailySeventhCol"></td>
            <td class="productionReportDailyEighthCol"></td>
            <td class="productionReportDailyNinthCol"></td>
            <td class="productionReportDailyTenthCol"></td>
            <td class="productionReportDailyEleventhCol"></td>
            <td class="productionReportDailyTwelvethCol"></td>
            <td class="productionReportDailyThirteenthCol"></td>
        </tr>
         <tr>
            <td colspan="13" align="center" class="productionReportHeader">
             Plant-Wide Production Report (Plan/Actual)
            </td>
        </tr>
        <tr>
            <td colspan="13" class="productionReportHeader">
                Date : <asp:Label ID="productionReportDateLabel" runat="server"></asp:Label>
                 
            </td>
        </tr>
         
        <tr>
            <td class="productionReportShiftHeader" style="width: 10%"></td>
            <td colspan="3" class="productionReportShiftHeader" style="width: 22%">Shift-A</td>
            <td colspan="3" class="productionReportShiftHeader" style="width: 23%">Shift-B</td>
            <td colspan="3" class="productionReportShiftHeader" style="width: 22%">Shift-C</td>
            <td colspan="3" class="productionReportShiftHeader" style="width: 23%">All</td>
        </tr>     
        <tr>
            <td style="width:10%" class="productionReportActualPlanHeader">Type</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:8%" class="productionReportActualPlanHeader">Difference</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:9%" class="productionReportActualPlanHeader">Difference</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:8%" class="productionReportActualPlanHeader">Difference</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:9%" class="productionReportActualPlanHeader">Difference</td>
        </tr>      
    <tr>   
         <td colspan="13">             
                 <asp:GridView ID="dtWiseProductionGridView" runat="server" AutoGenerateColumns="False" 
                 Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                 AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                 <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> 
                     <Columns>
                         <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%"  >
                             <ItemTemplate>
                                 <asp:Label ID="prodRepPlanAGridLabel" runat="server" Text='<%# Eval("processName") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                                                
                     <Columns>
                         <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" >
                             <ItemTemplate>
                                 <asp:Label ID="prodRepPlanAGridLabel" runat="server" Text='<%# Eval("PlanA") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                     <Columns>
                         <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepActualAGridLabel" runat="server" Text='<%# Eval("ActualA") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                     <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="9%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepDifferenceALabel" runat="server" Text='<%# Eval("DifferenceA") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepPlanBGridLabel" runat="server" Text='<%# Eval("PlanB") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepActualBGridLabel" runat="server" Text='<%# Eval("ActualB") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                     
                          <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="8%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepDifferenceBLabel" runat="server" Text='<%# Eval("DifferenceB") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepPlanCGridLabel" runat="server" Text='<%# Eval("PlanC") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                     <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepActualCGridLabel" runat="server" Text='<%# Eval("ActualC") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="10%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepDifferenceCLabel" runat="server" Text='<%# Eval("DifferenceC") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                     
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepPlanAllGridLabel" runat="server" Text='<%# Eval("PlanAll") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepActualAllGridLabel" runat="server" Text='<%# Eval("ActualAll") %>' CssClass="gridViewItems"></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                         <Columns>
                         <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="9%">
                             <ItemTemplate>
                                 <asp:Label ID="prodRepDifferenceAllGridLabel"  runat="server" Text='<%# Eval("DifferenceAll") %>' CssClass="gridViewItems"></asp:Label>
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
                    <td colspan="13" align="center">            
                    </td>                    
                </tr>   
    </table>    
    </div>    
    <br />    
    <div id="MonthlyReportDiv" runat="server" visible="false">
        <table class="productionReportTable">
        <tr>
            <td class="productionReportMonthlyFirstCol"></td>
            <td class="productionReportMonthlySecondCol"></td>
            <td class="productionReportMonthlyThirdCol"></td>
            <td class="productionReportMonthlyForthCol"></td>
            <td class="productionReportMonthlyFifthCol"></td>
            <td class="productionReportMonthlySixthCol"></td>
            <td class="productionReportMonthlySeventhCol"></td>
        </tr>
        <tr>
            <td colspan="13" align="center" class="productionReportHeader">
              Production Report (Plan/Actual)
            </td>
        </tr>
        <tr>
            <td colspan="7" class="productionReportHeader">
                Month : <asp:Label ID="productionReportMonthLabel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:10%" class="productionReportShiftHeader"></td>
            <td style="width:45%" colspan="3" class="productionReportShiftHeader">TBR</td>
            <td style="width:45%" colspan="3" class="productionReportShiftHeader">PCR</td>
           
        </tr>
        <tr>
           
            <td style="width:10%" class="productionReportActualPlanHeader">Day</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Difference</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Plan</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Actual</td>
            <td style="width:15%" class="productionReportActualPlanHeader">Difference</td>
            
        </tr>
       <tr>
                   
          <td colspan="12">
                        <table class="innerTable" cellspacing="0">
                   
                        </table>
                        <asp:Panel ID="monthWiseProdPanel" runat="server"   CssClass="panel" >
                            <asp:GridView ID="monthWiseProdGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                
                                      <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="9.6%" >
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepDateGridLabel" runat="server" Text='<%# Eval("Date") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns> 
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15.1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="PlannedTBRproGridLabel" runat="server" Text='<%# Eval("PlannedTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="ActualTBRGridLabel" runat="server" Text='<%# Eval("ActualTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="18.3%">
                                        <ItemTemplate>
                                            <asp:Label ID="DifferenceTBRGridLabel" runat="server" Text='<%# Eval("DifferenceTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="PlannedPCRGridLabel" runat="server" Text='<%# Eval("PlannedPCR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="ActualPCRGridLabel" runat="server" Text='<%# Eval("ActualPCR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="DifferencePCRGridLabel" runat="server" Text='<%# Eval("DifferencePCR") %>' CssClass="gridViewItems"></asp:Label>
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
    </div>
    <br />    
    <div id="YearlyReportDiv" runat="server" visible="false">
    <table class="productionReportTable">
        <tr>
            <td class="productionReportYearlyFirstCol"></td>
            <td class="productionReportYearlySecondCol"></td>
            <td class="productionReportYearlyThirdCol"></td>
            <td class="productionReportYearlyForthCol"></td>
            <td class="productionReportYearlyFifthCol"></td>
            <td class="productionReportYearlySixthCol"></td>
            <td class="productionReportYearlySeventhCol"></td>
        </tr>
        <tr>
            <td colspan="13" align="center" class="productionReportHeader">
              Production Report (Plan/Actual)
            </td>
        </tr>
        <tr>
            <td colspan="7" class="productionReportHeader">
                Year : <asp:Label ID="productionReportYearLabel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
           <td class="productionReportShiftHeader"></td>
            <td colspan="3" class="productionReportShiftHeader">TBR</td>
            <td colspan="3" class="productionReportShiftHeader">PCR</td>
           
        </tr>
        <tr>
            <td class="productionReportActualPlanHeader">Month</td>
            <td class="productionReportActualPlanHeader">Plan</td>
            <td class="productionReportActualPlanHeader">Actual</td>
            <td class="productionReportActualPlanHeader">Difference</td>
            <td class="productionReportActualPlanHeader">Plan</td>
            <td class="productionReportActualPlanHeader">Actual</td>
            <td class="productionReportActualPlanHeader">Difference</td>
            
        </tr>
        <tr>
                     
          <td colspan="12">
                        <table class="innerTable" cellspacing="0">
                   
                        </table>
                        <asp:Panel ID="yearWiseProdPanel" runat="server"  CssClass="panel" >
                            <asp:GridView ID="yearWiseProdGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                
                                      <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepMonthGridLabel" runat="server" Text='<%# Eval("Month") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns> 
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" >
                                        <ItemTemplate>
                                            <asp:Label ID="PlannedTBRproGridLabel" runat="server" Text='<%# Eval("PlannedTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="ActualTBRGridLabel" runat="server" Text='<%# Eval("ActualTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="DifferenceTBRGridLabel" runat="server" Text='<%# Eval("DifferenceTBR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="PlannedPCRGridLabel" runat="server" Text='<%# Eval("PlannedPCR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="ActualPCRGridLabel" runat="server" Text='<%# Eval("ActualPCR") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="DifferencePCRGridLabel" runat="server" Text='<%# Eval("DifferencePCR") %>' CssClass="gridViewItems"></asp:Label>
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
       <tr>
       <td>
           &nbsp;</td>
       </tr>
        
        
    </table>
    
    </div>
    
    <table class="innerTable">
    <tr>
    <td align="center">
    <asp:Chart ID="PCRChart" runat="server" Width="600px" Height="250px"  Visible="false" >    
        <Legends>                 
            <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" 
                 Title="Legends">                         
            </asp:Legend>
        </Legends>            
         <BorderSkin SkinStyle="Sunken"  />                      
         <ChartAreas>
             <asp:ChartArea Name="ChartArea1">
                 <AxisX>
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
           
    <tr >
    <td align="center">
        <asp:Chart ID="TBRChart" runat="server" Width="600px"  Height="250px" Visible="false" >
        <Legends>
              <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" 
                  Title="Legends">                         
              </asp:Legend>
          </Legends>
         <BorderSkin SkinStyle="Sunken" />                      
         <ChartAreas>
             <asp:ChartArea Name="ChartArea1">
                 <AxisX>
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
    </table>
    
    </div>
    
    
</asp:Content>
