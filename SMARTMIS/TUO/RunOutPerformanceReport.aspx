<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunOutPerformanceReport.aspx.cs" MasterPageFile="~/smartMISMaster.master" Inherits="SmartMIS.TUO.RunOutPerformanceReport" Title="RunOut PreformanceReport" %>

<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>


<asp:Content ID="RunOUTContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    
    
  <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
   <asp:HiddenField id="viewQueryHidden" runat="server" value="" />
     <br />
    <table style="width: 100%">
        <tr>
        <td class="gridViewItems" style="width: 235px"> Select ReportType : 
                <asp:DropDownList ID="searchType" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="searchType_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="dayWise">dayWise</asp:ListItem>
                    <asp:ListItem>monthWise</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 260px">
                <mycontrol:calendertextbox ID="reportMasterDateWiseTextBox" runat="server" 
                    Disabled="false" Visible="false" Width="60%" />
                <asp:Panel ID="monthlypanel" runat="server" Visible="False" Width="314px">
                    <asp:DropDownList ID="month" runat="server" Width="148px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">February</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    
                    &nbsp;<asp:DropDownList ID="year" runat="server" Width="147px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>2010</asp:ListItem>
                        <asp:ListItem>2011</asp:ListItem>
                        <asp:ListItem>2012</asp:ListItem>
                        <asp:ListItem>2013</asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
              
            </td>
            <td>
                <asp:Button ID="show" runat="server" Text="Show" onclick="show_Click" />
            </td>
        </tr>
    </table>
     
     
       
    
 <asp:reportHeader ID="reportHeader" runat="server" />

<asp:Panel ID="RunOutReportRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
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
                       
 <asp:GridView ID="RunOutRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15.5%">
                <ItemTemplate>
                        <asp:Label ID="RunOutSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tyreType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="RunOutRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="RunOutRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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