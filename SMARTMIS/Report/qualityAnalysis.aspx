<%@ Page Language="C#" MasterPageFile="~/smartMISReportMaster.Master" AutoEventWireup="true" CodeBehind="qualityAnalysis.aspx.cs" Inherits="SmartMIS.Report.qualityAnalysis" Title="Quality Analysis Report" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>


<asp:Content ID="content1" ContentPlaceHolderID="reportMasterContentPalceHolder" runat="server">
      <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
     <script type="text/javascript" language="javascript">
        function showReport(queryString)
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
     </script>    
       
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    <asp:reportHeader ID="reportHeader" runat="server" />

     <div id="chartDiv" runat="server"  >
    <table class="productionReportTable" cellpadding="0">   
    <tr>
    <td>
        &nbsp;</td>
    </tr>          
        <tr>
           <td>
            <asp:Chart ID="qualityAnalysisUniChart" runat="server">
            <BorderSkin SkinStyle="Sunken" />                 
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
          </td>
        </tr>   
             
        <tr>
            <td>
                <asp:Chart ID="curedTireScrapChart" runat="server"  > 
                <Legends>
                  <asp:Legend BackColor="Gainsboro" BackGradientStyle="TopBottom" Docking="Bottom" Alignment="Center"  Name="Legend1" 
                              Title="Legends">                         
                          </asp:Legend> 
                          </Legends>
                          <BorderSkin SkinStyle="Sunken" />                            
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>            
            </td>       
        </tr>
        
    </table>
    </div>  
    
</asp:Content>

