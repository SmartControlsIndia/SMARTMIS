<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OAYGRAFReport.ascx.cs" Inherits="SmartMIS.UserControl.OAYGRAFReport" %>

<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<script type="text/javascript" language="javascript">
    
</script>

<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />
 
 <table cellpadding="0" cellspacing="0" style="width: 60%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
        <td style="width: 20%"></td>
        <td style="width: 20%"></td>

    </tr>
    <tr>
        <td  class="masterLabel">
            <asp:RadioButton ID="QualityReportTBMWise" runat="server" 
                Text="MachineWise" GroupName="aa" AutoPostBack="True" 
                Checked="True" oncheckedchanged="QualityReportTBMWise_CheckedChanged" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small"> 
            <asp:RadioButton ID="QualityReportRecipeWise" runat="server" 
                Text="RecipeWise" GroupName="aa" AutoPostBack="True" 
                oncheckedchanged="QualityReportRecipeWise_CheckedChanged" />
        </td>
        
       
       
    </tr>
</table>

 <asp:Panel ID="QualityReportOAYGRAFTBMWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
 
 <table class="innerTable" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:6.5%; text-align:left; padding:5px" 
                rowspan="2">machine Name</td>
            <td class="gridViewHeader" style="width:9%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; width:78%; padding:5px" colspan="13">UNIFORMITY GRADE WITH A & B RANK</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">YTD</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JAN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">FEB</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">APR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAY</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUL</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">AUG</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">SEP</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">OCT</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">NOV</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">DEC</td>
        </tr>
    </table>
  
 <asp:GridView ID="performanceReportOAYGRAFWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="6%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                   <FooterTemplate>
                 <asp:Label ID="lblworkcentertotal" runat="server" CssClass="gridViewfooterItems" Width="60%" Text=" Grant Total"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportOAYGRAFWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="9.5%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportOAYGRAFWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("APR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("MAY") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("JUN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("JUL") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("AUG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("SEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("OCT") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("NOV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("DECE") %>' CssClass="gridViewItems"></asp:Label>
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
    </ItemTemplate>
     <FooterTemplate>
         <div>
                 <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems" Width="9.5%"  Text=""/>

                 <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems" Width="5.5%"  Text='<%# AlltotalcheckedQuantity()%>'/>
                 <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems"  Width="5.5%" Text='<%# AlltotalYTDQuantity()%>'/>
                 <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems" Width="5.8%" Text='<%# AlltotalJANQuantity()%>'/>
                 <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems" Width="5.8%" Text='<%# AlltotalFEBQuantity()%>'/>
                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems" Width="5.5%" Text='<%# AlltotalMARQuantity()%>'/>
                 <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems" Width="5.5%" Text='<%# AlltotalAPRQuantity()%>'/>
                  <asp:Label ID="Label2" runat="server" CssClass="gridViewfooterItems"  Width="5.5%" Text='<%# AlltotalMAYQuantity()%>'/>
                 <asp:Label ID="Label3" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalJUNQuantity()%>'/>
                 <asp:Label ID="Label4" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalJULQuantity()%>'/>
                 <asp:Label ID="Label5" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalAUGQuantity()%>'/>
                 <asp:Label ID="Label6" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalSEPQuantity()%>'/>
                 <asp:Label ID="Label1" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalOCTQuantity()%>'/>
                 <asp:Label ID="Label12" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalNOVQuantity()%>'/>
                 <asp:Label ID="Label13" runat="server" CssClass="gridViewfooterItems" Width="5.5%" Text='<%# AlltotalDECQuantity()%>'/>

            </div>
                </FooterTemplate>
      </asp:TemplateField>
        </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
  
    </asp:GridView>
  <div style="text-align:center">
     <asp:Chart ID="performanceReportOAYGrafTBMChart" runat="server" Width="800PX" Visible="true" BackGradientStyle="None" BorderlineDashStyle="Solid" BorderlineWidth="2" BorderlineColor="AliceBlue" >
     <Series>
     <asp:Series Name="TotalCheckedSeries" ChartType="Column" Color="Green" ShadowOffset="0">
                                                                                    
     </asp:Series>
    </Series>
   
    <Series>
     <asp:Series Name="ABgradeSeries" ChartType="Column" Color="Orange" ShadowOffset="0">
                                                                                    
     </asp:Series>
    </Series>
  <ChartAreas>
  <asp:ChartArea Name="productionReportDailyChart" BackGradientStyle="None" BackSecondaryColor="#B6D6EC" BorderDashStyle="Solid" BorderWidth="2" BorderColor="Blue" Area3DStyle-Enable3D="true">
 
  </asp:ChartArea>
   </ChartAreas>
     </asp:Chart>
     </div>
  
 </asp:Panel>
  
  <asp:Panel ID="QualityReportOAYGRAFRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
  <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:8%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; width:80%; padding:5px" colspan="13">UNIFORMITY GRADE WITH A & B RANK</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">YTD</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JAN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">FEB</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">APR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAY</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUL</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">AUG</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">SEP</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">OCT</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">NOV</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">DEC</td>
        </tr>
    </table>
                       
  <asp:GridView ID="performanceReportOAYGRAFRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFRecipeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                 <FooterTemplate>
                 <asp:Label ID="lblrecipeworkcentertotal" runat="server" CssClass="gridViewfooterItems" Width="60%" Text=" GrantTotal"/>
                </FooterTemplate>

            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="92%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportOAYGRAFrecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAPRGradeLabel" runat="server" Text='<%# Eval("APR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMAYGradeLabel" runat="server" Text='<%# Eval("MAY") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJUNGradeLabel" runat="server" Text='<%# Eval("JUN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJULGradeLabel" runat="server" Text='<%# Eval("JUL") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAUGGradeLabel" runat="server" Text='<%# Eval("AUG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseSEPGradeLabel" runat="server" Text='<%# Eval("SEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseOCTGradeLabel" runat="server" Text='<%# Eval("OCT") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseNOVGradeLabel" runat="server" Text='<%# Eval("NOV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseDECGradeLabel" runat="server" Text='<%# Eval("DECE") %>' CssClass="gridViewItems"></asp:Label>
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
                  <FooterTemplate>
                  <div>

                 <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems" Width="6.8%"  Text='<%# AlltotalcheckedQuantity()%>'/>
                 <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems"  Width="6.5%" Text='<%# AlltotalYTDQuantity()%>'/>
                 <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems" Width="6%" Text='<%# AlltotalJANQuantity()%>'/>
                 <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems" Width="6.5%" Text='<%# AlltotalFEBQuantity()%>'/>
                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems" Width="6%" Text='<%# AlltotalMARQuantity()%>'/>
                 <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems" Width="6.5%" Text='<%# AlltotalAPRQuantity()%>'/>
                  <asp:Label ID="Label2" runat="server" CssClass="gridViewfooterItems"  Width="6%" Text='<%# AlltotalMAYQuantity()%>'/>
                 <asp:Label ID="Label3" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalJUNQuantity()%>'/>
                 <asp:Label ID="Label4" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalJULQuantity()%>'/>
                 <asp:Label ID="Label5" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalAUGQuantity()%>'/>
                 <asp:Label ID="Label6" runat="server" CssClass="gridViewfooterItems" Width="5.7%" Text='<%# AlltotalSEPQuantity()%>'/>
                 <asp:Label ID="Label1" runat="server" CssClass="gridViewfooterItems" Width="6.7%" Text='<%# AlltotalOCTQuantity()%>'/>
                 <asp:Label ID="Label12" runat="server" CssClass="gridViewfooterItems" Width="6.7%" Text='<%# AlltotalNOVQuantity()%>'/>
                 <asp:Label ID="Label13" runat="server" CssClass="gridViewfooterItems" Width="6%" Text='<%# AlltotalDECQuantity()%>'/>

            </div>
        
                </FooterTemplate>
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