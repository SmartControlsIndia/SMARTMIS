<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QualityPerformanceReportTBMRecipeWise.ascx.cs" Inherits="SmartMIS.UserControl.QualityPerformanceReportTBMRecipeWise" %>


<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<script type="text/javascript" language="javascript">
    
</script>

<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />
<asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>

<asp:Panel ID="productionReport2RecipeWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1000PX" Height="100%" CssClass="panel" >

    <table class="innerTableperformanceReport2" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:7.5%; text-align:left; padding:5px" rowspan="2">TyreType</td>
            <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" rowspan="2">Check</td>
            <td class="gridViewHeader" colspan="2" style="width:15%; text-align:center; padding:5px"> Uni Grade</td>           
            <td  class="gridViewHeader"  colspan="8" style="text-align:center; width:24%; padding:5px">C</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">D</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">E</td>
                     


          </tr>
        <tr>
         <td class="gridViewHeader" style="width:6.2%; text-align:center; padding:5px" rowspan="2">A</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">B</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">DEF</td> 
            
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">DEF</td> 
                         
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CRO</td>
         <td class="gridViewAlternateHeader" style="width:4%; text-align:center; padding:5px">DEF</td> 

        </tr>
        </table> 
  
  
<asp:GridView ID="performanceReport2TBMRecipeWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="135%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="7%">
       <ItemTemplate>
       <asp:Label ID="performanceReport2TBMRecipeWiseRecipeNameLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
        <ItemTemplate>                       
       <div style="border: solid 1px #C3D9FF;">
       <asp:GridView ID="performanceReport2TBMRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                           <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVGradeLabel" runat="server" Text='<%# Eval("RFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HGradeLabel" runat="server" Text='<%# Eval("R1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVGradeLabel" runat="server" Text='<%# Eval("LFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGGradeLabel" runat="server" Text='<%# Eval("BLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLROGradeLabel" runat="server" Text='<%# Eval("LRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRROGradeLabel" runat="server" Text='<%# Eval("CRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2RecipeWiseCRROGradeLabel" runat="server" Text='<%# Eval("DEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVDGradeLabel" runat="server" Text='<%# Eval("DRFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HDGradeLabel" runat="server" Text='<%# Eval("DR1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVDGradeLabel" runat="server" Text='<%# Eval("DLFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONDGradeLabel" runat="server" Text='<%# Eval("DCON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGDGradeLabel" runat="server" Text='<%# Eval("DBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLRODGradeLabel" runat="server" Text='<%# Eval("DLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRRODGradeLabel" runat="server" Text='<%# Eval("DCRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2RecipeWiseDdefGradeLabel" runat="server" Text='<%# Eval("DDEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVEGradeLabel" runat="server" Text='<%# Eval("ERFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HEGradeLabel" runat="server" Text='<%# Eval("ER1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVEGradeLabel" runat="server" Text='<%# Eval("ELFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONEGradeLabel" runat="server" Text='<%# Eval("ECON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGEGradeLabel" runat="server" Text='<%# Eval("EBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLROEGradeLabel" runat="server" Text='<%# Eval("ELRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRROEGradeLabel" runat="server" Text='<%# Eval("ECRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                  <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2RecipeWiseEdefGradeLabel" runat="server" Text='<%# Eval("EDEF") %>' CssClass="gridViewItems"></asp:Label>
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
 
     <asp:GridView ID="performanceReport2TBMRecipeWiseFooterGridView" runat="server" AutoGenerateColumns="False" 
                        Width="135%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="AliceBlue" ForeColor="#333333" />
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%" Visible="True">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseFooterLabel1" runat="server" Text='Grand Total' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>


                           <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterRFVGradeLabel" runat="server" Text='<%# Eval("RFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterR1HGradeLabel" runat="server" Text='<%# Eval("R1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLFVGradeLabel" runat="server" Text='<%# Eval("LFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterBLGGradeLabel" runat="server" Text='<%# Eval("BLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLROGradeLabel" runat="server" Text='<%# Eval("LRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCRROGradeLabel" runat="server" Text='<%# Eval("CRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterDEFGradeLabel" runat="server" Text='<%# Eval("DEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterRFVDGradeLabel" runat="server" Text='<%# Eval("DRFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterR1HDGradeLabel" runat="server" Text='<%# Eval("DR1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLFVDGradeLabel" runat="server" Text='<%# Eval("DLFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCONDGradeLabel" runat="server" Text='<%# Eval("DCON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterBLGDGradeLabel" runat="server" Text='<%# Eval("DBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLRODGradeLabel" runat="server" Text='<%# Eval("DLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCRRODGradeLabel" runat="server" Text='<%# Eval("DCRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCRRODGradeLabel" runat="server" Text='<%# Eval("DDEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterRFVEGradeLabel" runat="server" Text='<%# Eval("ERFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterR1HEGradeLabel" runat="server" Text='<%# Eval("ER1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLFVEGradeLabel" runat="server" Text='<%# Eval("ELFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCONEGradeLabel" runat="server" Text='<%# Eval("ECON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterBLGEGradeLabel" runat="server" Text='<%# Eval("EBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLROEGradeLabel" runat="server" Text='<%# Eval("ELRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCRROEGradeLabel" runat="server" Text='<%# Eval("ECRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterCRROEGradeLabel" runat="server" Text='<%# Eval("EDEF") %>' CssClass="gridViewItems"></asp:Label>
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