<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="performanceReportSpacWise.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.performanceReportSpacWise" Title="performance report Spec Wise" %>


<asp:Content ID="performanceReportSpecWiseContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder"> 



<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>


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

 <table cellpadding="0" cellspacing="0" style="width: 80%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
         <td style="width: 10%"></td>
         <td style="width: 10%"></td>

        <td style="width: 12%"></td>
        <td style="width: 6%"></td>
        <td style="width: 12%"></td>
        <td style="width: 10%"></td>
        <td style="width: 10%"></td>

    </tr>
    <tr>
       <td class="masterLabel">
            &nbsp;</td>
        <td class="masterLabel">      
                        Parameter
        </td>
        <td class="masterLabel">
            <asp:DropDownList ID="tuoFilterSpecParameterDropDownList" runat="server" 
                CssClass="masterDropDownList" style="width: 90%" AutoPostBack="true"
                onselectedindexchanged="DropDownList_SelectedIndexChanged">
                <asp:ListItem>RFV</asp:ListItem>
                <asp:ListItem>R1H</asp:ListItem>
                <asp:ListItem>LFV</asp:ListItem>
                <asp:ListItem>CON</asp:ListItem>
                <asp:ListItem>LRO</asp:ListItem>
                <asp:ListItem>CRO</asp:ListItem>
                <asp:ListItem>BLG</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="masterLabel">
            Spec
        </td>
        
        <td class="masterTextBox">
            <asp:TextBox ID="SpecTextBox" Width="90%"  CssClass="masterTextBox" runat="server"></asp:TextBox>
        </td>
        <td class="masterTextBox">
            &nbsp;</td>
        <td style="width: 10%">
            &nbsp;</td>
    </tr>
</table>

 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
  <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
         <asp:RadioButton ID="QualityReportTBMWise" runat="server" 
          Text="MachineWise" GroupName="aa" AutoPostBack="True" 
          Checked="True" oncheckedchanged="QualityReportTBMWise_CheckedChanged" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
            <asp:RadioButton ID="QualityReportRecipeWise" runat="server" 
                Text="RecipeWise" GroupName="aa" AutoPostBack="True" 
                oncheckedchanged="QualityReportRecipeWise_CheckedChanged" />
        </td>
  
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Size::</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseSizeDropdownlist"  Width="60%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true"> </asp:DropDownList>
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseRecipeDropdownlist"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
             </asp:DropDownList>
        </td>        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterOptionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="No"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        </td>
        
       
       
    </tr>
</table>

<asp:ScriptManager ID="TuoRejectionDetailScriptManager" runat="server"></asp:ScriptManager>
 
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>

<asp:Panel ID="PerformaceReportSpecMachinewisePanel" runat="server" >
 <table class="innerTable" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:11%; text-align:left; padding:5px" 
                rowspan="2">machine Name</td>
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" colspan="4" style="width:40%; text-align:left; padding:5px"></td>
           
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:11%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec&gt;&amp;&lt;Spec+2</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec+2&gt;&amp;&lt;Spec+4</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec+4&gt;&amp;&lt;Spec+6</td>
                  <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                                      &nbsp;Spec+6&gt;</td>
        </tr>
    </table>
 <asp:GridView ID="performanceReportTBMWiseSpecMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="9%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTBMWiseSpecWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                   <FooterTemplate>
                 <asp:Label ID="lblworkcentertotal" runat="server" CssClass="gridViewfooterItems1" Width="60%" Text=" Grant Total"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportTBMWiseSpecGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTBMWiseSpecTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportTBMWiseSpecChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpecCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpecAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpecBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpec10GradeLabel" runat="server" Text='<%# Eval("specPlus10") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpec20GradeLabel" runat="server" Text='<%# Eval("SpecPlus20") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText=" Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpec30GradeLabel" runat="server" Text='<%# Eval("SpecPlus30") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText=" Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTBMWiseSpecmorethan30GradeLabel" runat="server" Text='<%# Eval("Specgreaterthan30") %>' CssClass="gridViewItems"></asp:Label>
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
                   <table class="innerTable" width="100%">
                   <tr>
                  <td style="width:11.5%; text-align:center; padding:5px">
                 <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems1"   Text=""/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalAQuantity()%>'/>
                 </td>
                 <td style="width:9%; text-align:center; padding:5px">
                 <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalBQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal10PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal20PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal30PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                  <asp:Label ID="Label2" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal30GreaterQuantity()%>'/>
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
  
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="performanceReportSpecRcipeWisePanel" runat="server">
 <table class="innerTable" cellspacing="1">
        <tr>
         
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" colspan="4" style="width:40%; text-align:left; padding:5px"></td>
           
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:11%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec&gt;&amp;&lt;Spec+2</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec+2&gt;&amp;&lt;Spec+4</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
                Spec+4&gt;&amp;&lt;Spec+6</td>
                  <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">
            &nbsp;Spec+6&gt;</td>
        </tr>
    </table>
 
                    
    <asp:GridView ID="performanceReportRecipeWiseSpecGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportRecipeWiseSpecTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportRecipeWiseSpecChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpecCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpecAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpecBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpec10GradeLabel" runat="server" Text='<%# Eval("specPlus10") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpec20GradeLabel" runat="server" Text='<%# Eval("SpecPlus20") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText=" Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpec30GradeLabel" runat="server" Text='<%# Eval("SpecPlus30") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText=" Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseSpecmorethan30GradeLabel" runat="server" Text='<%# Eval("Specgreaterthan30") %>' CssClass="gridViewItems"></asp:Label>
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
                   <tr>
               
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalAQuantity()%>'/>
                 </td>
                 <td style="width:9%; text-align:center; padding:5px">
                 <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalBQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal10PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal20PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                 <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal30PlusQuantity()%>'/>
                 </td>
                 <td style="width:10%; text-align:center; padding:5px">
                  <asp:Label ID="Label2" runat="server" CssClass="gridViewfooterItems1"  Text='<%# Alltotal30GreaterQuantity()%>'/>
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

</ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
