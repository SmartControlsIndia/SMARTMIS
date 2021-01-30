<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QualityPerformanceReport2.ascx.cs" Inherits="SmartMIS.UserControl.QualityPerformanceReport2" %>

<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<script type="text/javascript" language="javascript">
    
</script>

<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />
<asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>

<asp:Panel ID="productionReport2WiseMainPanel" runat="server" ScrollBars="Horizontal" Height="100%"  CssClass="panel" >

  <table class="innerTable" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:11%; text-align:left; padding:5px" 
                rowspan="2">Machine name</td>
            <td class="gridViewHeader" style="width:11%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
                  <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" 
                rowspan="2">A</td>
                  <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" 
                rowspan="2">C</td>
                  <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" 
                rowspan="2">F</td>
            <td  class="gridViewHeader" style="text-align:center; padding:5px" colspan="7">C</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">RFV</td>
            <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">R1H</td>
            <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">LFV</td>
            <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">CON</td>
            <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">BLG</td>
           <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">LRO</td>
           <td class="gridViewAlternateHeader" style="width:7%; text-align:center; padding:5px">CRRO</td>


        </tr>
    </table> 
  <asp:GridView ID="performanceReport2SizeWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="11%">
                <ItemTemplate>
                        <asp:Label ID="performanceReport2SizeWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                   <FooterTemplate>
                 <asp:Label ID="lblworkcentertotal" runat="server" CssClass="gridViewfooterItems" Width="100%" Text=" Grand Total"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="89%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReport2SizeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                <ItemTemplate>
                        <asp:Label ID="performanceReport2SizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="88%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReport2SizeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                           <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseFGradeLabel" runat="server" Text='<%# Eval("F") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseRFVGradeLabel" runat="server" Text='<%# Eval("RFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseR1HGradeLabel" runat="server" Text='<%# Eval("R1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseLFVGradeLabel" runat="server" Text='<%# Eval("LFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseBLGGradeLabel" runat="server" Text='<%# Eval("BLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseLROGradeLabel" runat="server" Text='<%# Eval("LRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                                      <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseCRROGradeLabel" runat="server" Text='<%# Eval("CRRO") %>' CssClass="gridViewItems"></asp:Label>
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
                 <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems" Width="18%"  Text='<%# AlltotalcheckedQuantity()%>'/>
                 <asp:Label ID="AllLabel1" runat="server" CssClass="gridViewfooterItems"  Width="7%" Text='<%# AlltotalAQuantity()%>'/>
                 <asp:Label ID="ALlLabel2" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalCQuantity()%>'/>
                 <asp:Label ID="AllLabel3" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalFQuantity()%>'/>
                 <asp:Label ID="AllLabel4" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalRFVQuantity()%>'/>
                 <asp:Label ID="AllLabel5" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalR1HQuantity()%>'/>
                 <asp:Label ID="AllLabel6" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalLFVQuantity()%>'/>
                 <asp:Label ID="AllLabel12" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalCONQuantity()%>'/>
                 <asp:Label ID="AllLabel13" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalBLGQuantity()%>'/>
                 <asp:Label ID="AllLabel14" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalLROQuantity()%>'/>
                   <asp:Label ID="AllLabel15" runat="server" CssClass="gridViewfooterItems" Width="7%" Text='<%# AlltotalCRROQuantity()%>'/>


                </FooterTemplate>
      </asp:TemplateField>
        </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center"  />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
    </asp:GridView>
</asp:Panel>