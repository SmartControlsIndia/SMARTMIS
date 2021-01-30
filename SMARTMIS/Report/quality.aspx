<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quality.aspx.cs" MasterPageFile="~/smartMISReportMaster.Master" Inherits="SmartMIS.QualityReport" Title="Quality Report" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>
<%@ Register Src="~/UserControl/qualityWcWise.ascx" TagName="qualityUC"  TagPrefix="UC9" %>

<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
  
      
    <script type="text/javascript" language="javascript">   
      function showReport(queryString) 
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('ctl00_ctl00_masterContentPlaceHolder_reportMasterContentPalceHolder_qualityUC_magicHidden').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }    
    </script>
 
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>       
    <asp:reportHeader ID="reportHeader" runat="server" />
     <UC9:qualityUC ID="qualityUC" runat="server" />
  
    <div id="PlantWideDiv" runat="server"> 
      
    <div id="DateWiseReportDiv" runat="server">
   
    <table class="productionReportTable" cellpadding="0">        
          <tr>
            <td class="productionReportDailyFirstCol"></td>
            <td class="productionReportDailySecondCol">
                
            </td>
            <td class="productionReportDailyThirdCol"></td>
            <td class="productionReportDailyForthCol"></td>
            <td class="productionReportDailyFifthCol"></td>
            <td class="productionReportDailySixthCol"></td>
            <td class="productionReportDailySeventhCol"></td>
            <td class="productionReportDailyEighthCol"></td>
            <td class="productionReportDailyNinthCol"></td>
        
        </tr>
         <tr>
            <td colspan="13" align="center" class="productionReportHeader">
              Quality Plant-Wide Report (Good/Rejected)
            </td>
        </tr>
        <tr>
            <td colspan="13" class="productionReportHeader">
                Date : <asp:Label ID="productionReportDateLabel" runat="server"></asp:Label>
                 
            </td>
        </tr>
         
        <tr>
            <td class="productionReportShiftHeader"></td>
            <td colspan="2" class="productionReportShiftHeader">Shift-A</td>
            <td colspan="2" class="productionReportShiftHeader">Shift-B</td>
            <td colspan="2" class="productionReportShiftHeader">Shift-C</td>
            <td colspan="2" class="productionReportShiftHeader">All</td>
        </tr>
        
        
        <tr>
            <td style="width:11%" class="productionReportActualPlanHeader">Type</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Good</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Rejected</td>            
            <td style="width:7%" class="productionReportActualPlanHeader">Good</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Rejected</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Good</td>
            <td style="width:7%" class="productionReportActualPlanHeader">Rejected</td>
            <td style="width:7%" class="productionReportActualPlanHeader">GoodAll</td>
            <td style="width:7%" class="productionReportActualPlanHeader">RejectedAll</td>

        </tr>      
    <tr>
   
                    <td colspan="13">
                        <table class="innerTable" cellspacing="0">                   
                        </table>
                                                
                        <asp:Panel ID="dtWiseQualityPanel" runat="server"  Height="50px" CssClass="panel" >
                            <asp:GridView ID="dtWiseQualityGridView" runat="server" AutoGenerateColumns="False" 
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
                            AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" /> 
                            
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%"  >
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepPlanAGridLabel" runat="server" Text='<%# Eval("processName") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                                                           
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepPlanAGridLabel" runat="server" Text='<%# Eval("GoodA") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepActualAGridLabel" runat="server" Text='<%# Eval("RejectedA") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                               
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepPlanBGridLabel" runat="server" Text='<%# Eval("GoodB") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepActualBGridLabel" runat="server" Text='<%# Eval("RejectedB") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                              
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepPlanCGridLabel" runat="server" Text='<%# Eval("GoodC") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepActualCGridLabel" runat="server" Text='<%# Eval("RejectedC") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                           
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepPlanAllGridLabel" runat="server" Text='<%# Eval("GoodAll") %>' CssClass="gridViewItems"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                    <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepActualAllGridLabel" runat="server" Text='<%# Eval("RejectedAll") %>' CssClass="gridViewItems"></asp:Label>
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
                    <td colspan="13" align="center">            
                    </td>                    
                </tr>   
    </table>    
    </div>    
    <br />
    
    <div id="MonthlyReportDiv" runat="server">
        <table class="productionReportTable">
        <tr>
            <td class="productionReportMonthlyFirstCol"></td>
            <td class="productionReportMonthlySecondCol"></td>
            <td class="productionReportMonthlyThirdCol"></td>
            <td class="productionReportMonthlyForthCol"></td>
            <td class="productionReportMonthlyFifthCol"></td>
         
        </tr>
        <tr>
            <td colspan="13" align="center" class="productionReportHeader">
              Quality Report (Good/Rejected)
            </td>
        </tr>
        <tr>
            <td colspan="7" class="productionReportHeader">
                Month : <asp:Label ID="productionReportMonthLabel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  class="productionReportShiftHeader"></td>
            <td colspan="2" class="productionReportShiftHeader">TBR</td>
            <td colspan="2" class="productionReportShiftHeader">PCR</td>
           
        </tr>
        <tr>
           
            <td class="productionReportActualPlanHeader">Day</td>
            <td class="productionReportActualPlanHeader">Good</td>
            <td class="productionReportActualPlanHeader">Rejected</td>

            <td class="productionReportActualPlanHeader">Good</td>
            <td class="productionReportActualPlanHeader">Rejected</td>
      
            
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
                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="2%" >
                                        <ItemTemplate>
                                            <asp:Label ID="prodRepDateGridLabel" runat="server" Text='<%# Eval("Date") %>' CssClass="gridViewItems"></asp:Label>
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
    
    <div id="YearlyReportDiv" runat="server">
    <table class="productionReportTable">
        <tr>
            <td class="productionReportYearlyFirstCol"></td>
            <td class="productionReportYearlySecondCol"></td>
            <td class="productionReportYearlyThirdCol"></td>
            <td class="productionReportYearlyForthCol"></td>
            <td class="productionReportYearlyFifthCol"></td>

        </tr>
        <tr>
            <td colspan="13" align="center" class="productionReportHeader">
              Quality Report (Good/Rejected)
            </td>
        </tr>
        <tr>
            <td colspan="7" class="productionReportHeader">
                Year : <asp:Label ID="productionReportYearLabel" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
           <td class="productionReportShiftHeader"></td>
            <td colspan="2" class="productionReportShiftHeader">TBR</td>
            <td colspan="2" class="productionReportShiftHeader">PCR</td>
           
        </tr>
        <tr>
            <td class="productionReportActualPlanHeader">Month</td>
            <td class="productionReportActualPlanHeader">Good</td>
            <td class="productionReportActualPlanHeader">Rejected</td>
            <td class="productionReportActualPlanHeader">Good</td>
            <td class="productionReportActualPlanHeader">Rejected</td>

            
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
    <tr align="center">
    <td></td>
    <td align="center">    
    <asp:Chart ID="TBRChart" runat="server" Width="600px" Height="250px" Visible="false" >    
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
                        </asp:Chart></td></tr>     
    <tr >
    <td></td>
       
        <td align="center">
    <asp:Chart ID="PCRChart" runat="server" Width="600px" Height="250px" Visible="false" >    
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
    </table>    
    
    </div>   
    
   
    
</asp:Content>


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  