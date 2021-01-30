<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="utility.aspx.cs" Inherits="SmartMIS.utilityReport" MasterPageFile="~/smartMISReportMaster.Master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <table class="productionReportTable" cellpadding="0">
        <tr>
            <td class="productionReportDailySecondCol">&nbsp;</td>
            <td class="productionReportDailySecondCol"></td>
            <td class="productionReportDailyThirdCol"></td>
            <td class="productionReportDailyForthCol"></td>
            <td class="productionReportDailyFifthCol"></td>
            <td class="productionReportDailySixthCol"></td>
            <td class="productionReportDailySeventhCol"></td>
            <td class="productionReportDailyEighthCol"></td>
            <td class="productionReportDailyNinthCol"></td>
            <td class="productionReportDailyTenthCol"></td>
        </tr>
        <tr>
           
            <td colspan="10" class="productionReportHeader">
                Date : <asp:Label ID="energyConsumptionReportDateLabel" runat="server">03/03/2011</asp:Label>
            </td>
        </tr>
        <tr>         
            <td colspan="4" class="productionReportShiftHeader">Shift-A</td>
            <td colspan="4" class="productionReportShiftHeader">Shift-B</td>
            <td colspan="4" class="productionReportShiftHeader">Shift-C</td>
        </tr>
        <tr>
            <td class="productionReportActualPlanHeader">S.No.&nbsp;</td>
            <td class="productionReportActualPlanHeader">Utility</td>
            <td class="productionReportActualPlanHeader">Area</td>
            <td class="productionReportActualPlanHeader">Consumption</td>
            <td class="productionReportActualPlanHeader">Utility</td>
            <td class="productionReportActualPlanHeader">Area</td>
            <td class="productionReportActualPlanHeader">Consumption</td>
            <td class="productionReportActualPlanHeader">Utility</td>
            <td class="productionReportActualPlanHeader">Area</td>
            <td class="productionReportActualPlanHeader">Consumption</td>
        </tr>
           
  
        <tr>
            <td class="masterLabel">1.&nbsp;</td>
            <td class="masterLabel">Steam</td>
            <td class="masterLabel">HVAC</td>
            <td class="masterLabel">345</td>
            <td class="masterLabel">Steam</td>
            <td class="masterLabel">Curing</td>
            <td class="masterLabel">245</td>
            <td class="masterLabel">Steam</td>
            <td class="masterLabel">Curing</td>
            <td class="masterLabel">145</td>
        </tr>
        
            <tr>
            <td>&nbsp;</td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>        
            <tr>
            <td>&nbsp;</td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        
            <tr>
            <td class="masterLabel">2.&nbsp;</td>
            <td class="masterLabel">Air</td>
            <td class="masterLabel">TBM</td>
            <td class="masterLabel"> 2.36&nbsp;</td>
            <td class="masterLabel">Water&nbsp;</td>
            <td class="masterLabel">TBRCuring&nbsp;</td>
            <td class="masterLabel">21</td>
            <td class="masterLabel">Water&nbsp;</td>
            <td class="masterLabel">TBRCuring&nbsp;</td>
            <td class="masterLabel">41&nbsp;</td>
        </tr>
        
    </table>
    <br />
        
    <br />
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
            <td colspan=7 align="center" >
            
                <asp:Chart ID="Chart1" runat="server" Width="700px" Height="500px">
                    <Legends>
                        <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Name="Legend1" 
                            Title="Legends">
                        </asp:Legend>
                    </Legends>
                    <BorderSkin BackColor="Silver" SkinStyle="Emboss" />
                <titles>
                          <asp:Title Font="Arial, 11.25pt, style=Bold" Name="Title1" Text="Utility Report of Steam Utility for all three shifts of date:03/03/2011">
                          </asp:Title>
                      </titles>
                      
                    <Series>
                        <asp:Series Name="Shift-A " ChartArea="ChartArea1" Legend="Legend1" 
                            Font="Microsoft Sans Serif, 8pt" IsValueShownAsLabel="True">
                        <Points>
                             <asp:DataPoint AxisLabel="Steam Utility" YValues="345" MapAreaAttributes="" 
                                 ToolTip="" Url=""/>
                        </Points>
                        </asp:Series>
                        
                          <asp:Series Name="Shift-B" ChartArea="ChartArea1" Legend="Legend1" 
                            IsValueShownAsLabel="True">
                            <Points>
                             <asp:DataPoint  YValues="245" MapAreaAttributes="" ToolTip="" Url=""/> 
                        </Points>
                        </asp:Series>
                        
                             <asp:Series Name="Shift-C" ChartArea="ChartArea1" Legend="Legend1" 
                            IsValueShownAsLabel="True">
                            <Points>
                             <asp:DataPoint  YValues="145" MapAreaAttributes="" ToolTip="" Url=""/> 
                        </Points>
                        </asp:Series>
                        
                        
                     
                        
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackGradientStyle="TopBottom" >
                         
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>    
</asp:Content>


