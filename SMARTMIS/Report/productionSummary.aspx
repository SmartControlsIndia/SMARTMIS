<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productionSummary.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.productionReport" %>

<asp:Content ID="productionReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
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
           
        </tr>
        
        <tr>
            <td></td>
            <td colspan="3" class="productionReportShiftHeader">Day</td>
            <td colspan="3" class="productionReportShiftHeader">Month</td>
            <td colspan="3" class="productionReportShiftHeader">Year</td>
           
        </tr>
        <tr>
            <td></td>
            <td class="productionReportActualPlanHeader">Plan</td>
            <td class="productionReportActualPlanHeader">Actual</td>
            <td class="productionReportActualPlanHeader">Difference</td>
            <td class="productionReportActualPlanHeader">Plan</td>
            <td class="productionReportActualPlanHeader">Actual</td>
            <td class="productionReportActualPlanHeader">Difference</td>
            <td class="productionReportActualPlanHeader">Plan</td>
            <td class="productionReportActualPlanHeader">Actual</td>
            <td class="productionReportActualPlanHeader">Difference</td>
           
        </tr>
        <tr>
            <td class="masterLabel">PCR :</td>
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
            <td class="masterLabel">TBR :</td>
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
            <td class="masterLabel">Tonnage :</td>
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
            <td class="masterLabel">Total :</td>
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
            <td></td>
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
    </table>
   
    
</asp:Content>


