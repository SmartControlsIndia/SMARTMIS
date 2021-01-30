<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReport2.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.TUOReport2" Title="Performance Report With Rejection Details" %>

<asp:Content  ID="performanceReportJectionDetailContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder" >

    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
   
    <%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:HiddenField id="tuoFilterPerformanceReportTUOWiseSizeDropdownlistHidden" runat="server" value="All" />
    <asp:HiddenField id="tuoFilterPerformanceReportTUOWiseRecipeDropdownlistHidden" runat="server" value="All" />
    
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
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseSizeDropdownlist"  Width="60%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true" Visible="true"> </asp:DropDownList>
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseRecipeDropdownlist"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true" Visible="true">
             </asp:DropDownList>
        </td>        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterOptionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="No"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        &nbsp;</td>
        
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
             <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" />
        </td>
       
       
    </tr>
</table>

<asp:ScriptManager ID="TuoRejectionDetailScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
 
  
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
<asp:Panel ID="productionReport2TBMWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1250PX" Height="100%" CssClass="panel" >

    <table class="innerTableperformanceReport2" cellspacing="1" style="width:225%;">
        <tr>
         <td class="gridViewHeader" style="width:3.5%; text-align:left; padding:5px" rowspan="2">Machine name</td>
            <td class="gridViewHeader" style="width:5.6%; text-align:left; padding:5px" rowspan="2">TyreType</td>
            <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
            <td class="gridViewHeader" colspan="3" style="width:15%; text-align:center; padding:5px"> Uni Grade</td>           
            <td  class="gridViewHeader"  colspan="8" style="text-align:center; width:24%; padding:5px">C</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">D</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">E</td>
                     


          </tr>
        <tr>
         <td class="gridViewHeader" style="width:3.5%; text-align:center; padding:5px" rowspan="2">A</td>
         <td class="gridViewHeader" style="width:3%; text-align:center; padding:5px" rowspan="2">B</td>
         <td class="gridViewHeader" style="width:3%; text-align:center; padding:5px" rowspan="2">REJ</td>
         <td class="gridViewAlternateHeader" style="width:2.1%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CRO</td> 
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">DEF</td> 
            
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CRO</td> 
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">DEF</td> 
                         
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">RFV </td> 
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">R1H</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LFV</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">BLG</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">LRO</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">CRO</td>
         <td class="gridViewAlternateHeader" style="width:2%; text-align:center; padding:5px">DEF</td> 
         
        </tr>
        </table> 
      
    <asp:GridView ID="performanceReport2TBMWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="225%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" HorizontalAlign="Center">
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="4.5%">
       <ItemTemplate>
       <asp:Label ID="performanceReport2TBMWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130%">
        <ItemTemplate>
        <asp:Label ID="machineEmpty" runat="server" Text=""></asp:Label>
      
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReport2TBMWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                           <Columns>
                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVGradeLabel" runat="server" Text='<%# Eval("RFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HGradeLabel" runat="server" Text='<%# Eval("R1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVGradeLabel" runat="server" Text='<%# Eval("LFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGGradeLabel" runat="server" Text='<%# Eval("BLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLROGradeLabel" runat="server" Text='<%# Eval("LRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("CRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("DEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVDGradeLabel" runat="server" Text='<%# Eval("DRFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HDGradeLabel" runat="server" Text='<%# Eval("DR1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVDGradeLabel" runat="server" Text='<%# Eval("DLFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONDGradeLabel" runat="server" Text='<%# Eval("DCON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGDGradeLabel" runat="server" Text='<%# Eval("DBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLRODGradeLabel" runat="server" Text='<%# Eval("DLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRRODGradeLabel" runat="server" Text='<%# Eval("DCRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRRODGradeLabel" runat="server" Text='<%# Eval("DDEF") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVEGradeLabel" runat="server" Text='<%# Eval("ERFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HEGradeLabel" runat="server" Text='<%# Eval("ER1H") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVEGradeLabel" runat="server" Text='<%# Eval("ELFV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONEGradeLabel" runat="server" Text='<%# Eval("ECON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGEGradeLabel" runat="server" Text='<%# Eval("EBLG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLROEGradeLabel" runat="server" Text='<%# Eval("ELRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROEGradeLabel" runat="server" Text='<%# Eval("ECRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROEGradeLabel" runat="server" Text='<%# Eval("EDEF") %>' CssClass="gridViewItems"></asp:Label>
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
    <% if(grandtotal != 0){ %>
    <table class="innerTable" style="background-color:Gray;width:225%;">
     <tr>
         <td  style="width:4.5%;" class="gridViewItems2"> Grand Total </td>
         
         <td  style="width:130%" class="gridViewItems2">
         <table class="innerTable" style="background-color:Gray;width:100%;">
     <tr>
         <td  style="width:5%;" class="gridViewItems2"> </td>
         <td  style="width:6.5%;" class="gridViewItems2"><%= grandtotal %></td>
            <td  style="width:3.7%" class="gridViewItems2"><%= grandA%></td>
            <td  style="width:3%" class="gridViewItems2"><%= grandB%></td>
            <td  style="width:3%" class="gridViewItems2"><%= grandE%></td>
            <td  style="width:2.5%" class="gridViewItems2"><%= grandRFV%></td>
            <td  style="width:3%" class="gridViewItems2"><%= grandR1H%></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandLFV %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandCON %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandBLG %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandLRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandCRRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDEF %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDRFV %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDR1H %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDLFV %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDCON %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDBLG %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDLRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDCRRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandDDEF %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandERFV %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandER1H %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandELFV %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandECON %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandEBLG %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandELRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandECRRO %></td>
            <td  style="width:2%" class="gridViewItems2"><%= grandEDEF %></td>
            
            </td></tr></table>
     <% } %>
   </tr>
</table>
    </asp:Panel>
 
 <asp:Panel ID="productionReport2RecipeWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1250PX" Height="100%" CssClass="panel" >

    <table class="innerTableperformanceReport2" cellspacing="1" style="width:225%;">
        <tr>
            <td class="gridViewHeader" style="width:6.5%; text-align:left; padding:5px" rowspan="2">TyreType</td>
            <td class="gridViewHeader" style="width:7%; text-align:center; padding:5px" rowspan="2">Check</td>
            <td class="gridViewHeader" colspan="3" style="width:15%; text-align:center; padding:5px"> Uni Grade</td>           
            <td  class="gridViewHeader"  colspan="8" style="text-align:center; width:24%; padding:5px">C</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">D</td>
            <td  class="gridViewHeader" colspan="8" style="text-align:center; width:24%; padding:5px">E</td>                     
          </tr>
        <tr>
         <td class="gridViewHeader" style="width:5.8%; text-align:center; padding:5px" rowspan="2">A</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">B</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px" rowspan="2">REJ</td>
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
    Width="225%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="4.7%">
       <ItemTemplate>
       <asp:Label ID="performanceReport2TBMRecipeWiseRecipeNameLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       
       <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        
                           <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
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
 
 
 </asp:Panel>
 
 
 </ContentTemplate>
 <Triggers>
        <asp:PostBackTrigger ControlID="expToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>