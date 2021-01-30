<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinamicBalPerformanceReport.aspx.cs" MasterPageFile="~/smartMISUBReportMaster.master" Inherits="SmartMIS.TUO.DinamicBalPerformanceReport" Title="Dynamic Balancing Summary Report" %>

<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

<asp:Content ID="DinamicBalContent" runat="server" ContentPlaceHolderID="DBReportMasterContentPalceHolder">

<link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
  <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
   <asp:HiddenField id="viewQueryHidden" runat="server" value="" />
     <script type="text/javascript" language="javascript">
     
        function showReport(queryString) 
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
    </script>
    
 <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
 <asp:reportHeader ID="reportHeader" runat="server" />

<asp:Panel ID="dinamicBalReportRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
 <table class="innerTable" cellspacing="1">
        <tr>
        
            <td class="gridViewHeader" style="width:16%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px"></td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Rep</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Rejection</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">C</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">D</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">E</td>
        </tr>
    </table>
                       
 <asp:GridView ID="dinamicBalRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15.5%">
                <ItemTemplate>
                        <asp:Label ID="dinamicBalSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="DinamicBalRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="dinamicBalRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
  <table class="innerTable" width="100%">
     <tr >
     
     <td style="width:11%; text-align:center; padding:5px">
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalAQuantity()%>'/>
    </td>
    <td style="width:11%; text-align:center; padding:5px">
      <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalBQuantity()%>'/>
     </td>
           <td style="width:11%; text-align:center; padding:5px">                 
           <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalCQuantity()%>'/>
           </td>
           <td style="width:10%; text-align:center; padding:5px">

                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalDQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">
           <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalEQuantity()%>'/>
           </td>

</tr>
</table>

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


</asp:Content>