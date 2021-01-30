<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="tuoFilterRep2.ascx.cs" Inherits="SmartMIS.UserControl.tuoFilterRep2" %>
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />


<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />

<asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
<style type="text/css">
    .style1
    {
        font-family: Arial;
        font-size: 11px;
        font-weight: bold;
        text-align: left;
        white-space: normal;
        color: White;
        background-color: #507CD1;
        font-weight: bold;
        height: 12px;
    }
</style>
<table cellpadding="0" cellspacing="0" style="width: 60%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
        <td style="width: 10%"></td>
        <td style="width: 20%"></td>
        <td style="width: 10%"></td>
        <td style="width: 30%"></td>
        <td style="width: 10%"></td>
        <td style="width: 20%"></td>
    </tr>
    <tr>
        <td class="masterLabel">
            NOM
        </td>
        <td class="masterTextBox">
            <asp:DropDownList ID="tuoFilterRep2NOMDropDownList" runat="server" 
                CssClass="masterDropDownList" style="width: 90%" AutoPostBack="true"
                onselectedindexchanged="DropDownList_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td class="masterLabel">
            Design
        </td>
        <td class="masterTextBox">
            <asp:DropDownList ID="tuoFilterRep2DesignDropDownList" runat="server" 
                CssClass="masterDropDownList" style="width: 90%" AutoPostBack="true"
                onselectedindexchanged="DropDownList_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td class="masterLabel">
            Size
        </td>
        <td class="masterTextBox">
            <asp:DropDownList ID="tuoFilterRep2SizeDropDownList" runat="server" AutoPostBack="true" onselectedindexchanged="DropDownList_SelectedIndexChanged"
                CssClass="masterDropDownList" style="width: 90%"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2"></td>
        <td class="masterLabel">
            Result
        </td>
        <td class="masterTextBox">
            <asp:DropDownList ID="tuoFilterRep2ResultDropDownList" runat="server" AutoPostBack="true" onselectedindexchanged="DropDownList_SelectedIndexChanged"
                CssClass="masterDropDownList" style="width: 90%">
                <asp:ListItem Text ="Nos" Value="1"></asp:ListItem>
                <asp:ListItem Text="Percentage" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td colspan="2"></td>
    </tr>
</table>
<table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
                  <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="2">A</td>
                  <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="2">C</td>
                  <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" 
                rowspan="2">F</td>
            <td class="style1" style="text-align:center; padding:5px" colspan="7">C</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">RFV</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">R1H</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">LFV</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">CON</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">BLG</td>
           <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">LRO</td>
           <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">CRRO</td>


        </tr>
    </table>
<asp:GridView ID="performanceReport2SizeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                <ItemTemplate>
                        <asp:Label ID="performanceReport2SizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReport2SizeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
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
                                            <asp:Label ID="performanceReport2SizeWiseBGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2SizeWiseCGradeLabel" runat="server" Text='<%# Eval("F") %>' CssClass="gridViewItems"></asp:Label>
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
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="8%">
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
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="8%">
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
