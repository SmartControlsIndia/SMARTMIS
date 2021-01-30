<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oee.aspx.cs" MasterPageFile="~/smartMISReportMaster.Master" Inherits="SmartMIS.oeeReport" Title ="OEE Report" %>
<%@ Register src="../UserControl/reportHeader.ascx" tagname="reportHeader" tagprefix="asp" %>

<asp:Content ID="oeeReportContent" runat="server" ContentPlaceHolderID="reportMasterContentPalceHolder">
    <link href="../Style/oeeReport.css" rel="stylesheet" type="text/css" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    
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
     <div id="wcWiseReportDiv" runat="server">
     
    <table class="oeeReportTable" cellpadding="0">  
    <tr> 
    
    <td class="gridViewHeader" style="width:3%;text-align:center; padding:5px">WorkCentre Name </td>
        <td class="gridViewHeader" style="width:2%;text-align:center; padding:5px">Availability Rate</td>
        <td class="gridViewHeader" style="width:2%;text-align:center; padding:5px">Performance Rate</td>
        <td class="gridViewHeader" style="width:2%;text-align:center; padding:5px">Quality Rate </td>
        <td class="gridViewHeader" style="width:2%;text-align:center;padding:5px">OEE </td>
         <td class="gridViewHeader" style="width:11%;text-align:center; padding:5px">Graph </td>
            
    </tr> 
    <tr>
    <td colspan="6">        
        
                <asp:Panel ID="oeeReportPanel" runat="server"  CssClass="panel" >
                    <asp:GridView ID="oeeReportGridView" runat="server" AutoGenerateColumns="False"  
                    Width="100%" CellPadding="5" ForeColor="#333333" GridLines="Both" AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   

                        <Columns>
                            <asp:TemplateField HeaderText="wcNameID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="7%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridwcNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Availibility" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridAvailibilityLabel" runat="server" Text='<%# Eval("AvailabilityRate") %>' CssClass="gridViewItems"></asp:Label>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="performance" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridperformanceLabel" runat="server" Text='<%# Eval("performanceRate") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="avail" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridQualityLabel" runat="server" Text='<%# Eval("qualityRate") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="oee" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridOeeReportLabel" runat="server" Text='<%# Eval("oee") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                            
                           <Columns>
                            <asp:TemplateField HeaderText="oee" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:Chart ID="oeeReportChart" runat="server" Height="100px" Width="300px" Palette="Berry" BackColor="AliceBlue" >                               
                                        <ChartAreas>                                                                                
                                            <asp:ChartArea Name="ChartArea1"  BackGradientStyle="TopBottom" BackSecondaryColor="AliceBlue">
                                            </asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
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
        
 <div id="monthWiseReportDiv" runat="server">
     
  <table class="innerTable" cellspacing="1">
<tr>
    <td class="gridViewHeader" style="width:5%; text-align:center; padding:5px" rowspan="2">Workcenter Name</td>
   
    <td class="gridViewHeader" style="width:5%; text-align:left; padding:5px" rowspan="2"></td>
    <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="31">Days</td>
  
</tr>
<tr>
    <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">1</td>
   <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">2</td>
   <td class="gridViewHeader" style="width:3%; text-align:center; padding:5px">3</td>
    <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">4</td>
     <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">5</td>
      <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">6</td>
       <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">7</td>
       
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">8</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">9</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">10</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">11</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">12</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">13</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">14</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">15</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">16</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">17</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">18</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">19</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">20</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">21</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">22</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">23</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">24</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">25</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">26</td>
    
     <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">27</td>
     <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">28</td>
      <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">29</td>
       <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">30</td>
        <td class="gridViewHeader" style="width:2%; text-align:center; padding:5px">31</td>
</tr>


<asp:Panel ID="oeeReportMonthWisePanel" runat="server"  CssClass="panel" >
    <asp:GridView ID="oeeReportMonthWiseGridView" runat="server" 
        AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" 
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
        <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="oeeReportMonthWiseWCIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                <HeaderStyle CssClass="gridViewHeader" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
        
           
            <asp:TemplateField HeaderText="Workcenter Name">
                <ItemTemplate>
                    <asp:Label ID="oeeReportMonthWiseWCNameLabel" runat="server" 
                        CssClass="gridViewItems" Text='<%# Eval("name") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle CssClass="gridViewHeader" />
                <ItemStyle HorizontalAlign="Center"  />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ChildGrid">
                <ItemTemplate>
                    <%--child Gridview--%>
                     <td>
                                         <asp:GridView ID="oeeReportInnerGridView" runat="server" AutoGenerateColumns="False"  onrowdatabound="GridView_RowDataBound"
                    Width="100%" CellPadding="2" ForeColor="#333333" GridLines="Horizontal" AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />   
                                  
                    
                       <Columns>
                            <asp:TemplateField HeaderText="RateName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridRateNameLable" runat="server" Text='<%# Eval("rateName") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>                                

                        <Columns>
                            <asp:TemplateField HeaderText="1Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day1") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="2Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day2") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                     <Columns>
                            <asp:TemplateField HeaderText="3Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day3") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="4Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day4") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="5Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day5") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                            
                            <Columns>
                            <asp:TemplateField HeaderText="6tDay" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day6") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
            
                         <Columns>
                            <asp:TemplateField HeaderText="7Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day7") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="8Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day8") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="9Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day9") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="10Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day10") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="11Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day11") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="12Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day12") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="13Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day13") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="14Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day14") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="15Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day15") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="16Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day16") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="17Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day17") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="18Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day18") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="19Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day19") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="20Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day20") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="21Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day21") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="22Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day22") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="23Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day23") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="24Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day24") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="25Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day25") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="26Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day26") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="27Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day27") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="28Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day28") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="29Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day29") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="30Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day30") %>' CssClass="gridViewItems"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <Columns>
                            <asp:TemplateField HeaderText="31Day" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%" >
                                <ItemTemplate>
                                    <asp:Label ID="oeeGridFirstDayLabel" runat="server" Text='<%# Eval("day31") %>' CssClass="gridViewItems"></asp:Label>
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


</table>
        
        
   
 </div> 
 
 
    
    
    
</asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     