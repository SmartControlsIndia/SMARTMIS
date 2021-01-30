<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageANDstdPerformanceReport.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.averageANDstdPerformanceReport" %>

<asp:Content  ID="performanceReportAverageContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder" >

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
        
              <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Grade:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="GradeDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="SelectGrade"></asp:ListItem>
            <asp:ListItem  Value="1" Text="AllChecked"></asp:ListItem>
            <asp:ListItem  Value="2" Text="A"></asp:ListItem>
           <asp:ListItem  Value="3" Text="B"></asp:ListItem>
           <asp:ListItem  Value="4" Text="C"></asp:ListItem>
           <asp:ListItem  Value="5" Text="D"></asp:ListItem>
           <asp:ListItem  Value="6" Text="E"></asp:ListItem>


             </asp:DropDownList>
        </td>
             
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterOptionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="Average"></asp:ListItem>
            <asp:ListItem  Value="1" Text="  "></asp:ListItem>
             </asp:DropDownList>
        </td>
        
       
       
    </tr>
</table>
<asp:ScriptManager ID="TuoRejectionDetailScriptManager" runat="server"></asp:ScriptManager>
 
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>


  <asp:Panel ID="performanceAvgReportMainPanel" runat="server" ScrollBars="Horizontal" Width="1000PX" Height="100%" CssClass="panel" >
    <table class="innerTableperformanceReport2" width="150%" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:10.5%; text-align:left; padding-left:15PX; padding-right:15PX" rowspan="2">Machine name</td>
            <td class="gridViewHeader" style="width:10.5%; text-align:left; padding-left:15PX; padding-right:15PX" rowspan="2">TyreType</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding-left:15PX; padding-right:15PX" rowspan="2">AllChecked</td>
            <td class="gridViewHeader" colspan="5" style="width:25%; text-align:center; padding:5px"> Uni Grade</td>           
            <td  class="gridViewHeader"  colspan="15" style="text-align:center; width:60%; padding:5px">Average of selected parameter </td>
          </tr>
        <tr>
         <td class="gridViewHeader" style="width:6.2%; text-align:center; padding-left:12PX; padding-right:12PX" rowspan="2">GradeA</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding-left:12PX; padding-right:12PX" rowspan="2">GradeB</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding-left:12PX; padding-right:12PX" rowspan="2">GradeC</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding-left:12PX; padding-right:12PX"rowspan="2">GradeD</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding-left:12PX; padding-right:12PX" rowspan="2">GradeE</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">RFVCW </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">RFVCCW </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">H1RFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">H1RFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LFVCCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">CONICITY</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LowerBulge</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">UpperBulge</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LowerLRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">UpperLRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LowerRRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">UpperRRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">LowerDep</td>      
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding-left:12PX; padding-right:12PX">UpperDep</td>      
        </tr>
        </table>     
    <asp:GridView ID="performanceAvgReportMainGridView" runat="server" AutoGenerateColumns="False"
    Width="140%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false">
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
       <ItemTemplate>
       <asp:Label ID="performanceAvgReportWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="135%">
        <ItemTemplate>
                    
    <asp:GridView ID="performanceAvgReportGridView" runat="server" AutoGenerateColumns="False"
    Width="127%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                <ItemTemplate>
                        <asp:Label ID="performanceAvgReportTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceAvgReportChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HRFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CON Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerRRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgLowerRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("AvgUpperRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>' CssClass="gridViewItems"></asp:Label>
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
       
      </asp:TemplateField>
      </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
    </asp:GridView>   
    
    <asp:GridView ID="performanceAvgReportTBMWiseFooterGridView" runat="server" AutoGenerateColumns="False" 
                        Width="175%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="AliceBlue" ForeColor="#333333" />
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="5%" Visible="True">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseFooterLabel1" runat="server" Text='Grand Total' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                              <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HRFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CON Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerRRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgLowerRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("AvgUpperRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>' CssClass="gridViewItems"></asp:Label>
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
 
    <asp:Panel ID="productionReport2RecipeWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1000PX" Height="400PX" CssClass="panel" >
    <table class="innerTableperformanceReport2" width="100%" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:10.5%; text-align:left; padding-left:15PX; padding-right:15PX" rowspan="2">TyreType</td>
            <td class="gridViewHeader" style="width:8%; text-align:center; padding:5px" rowspan="2">AllChecked</td>
            <td class="gridViewHeader" colspan="5" style="width:25%; text-align:center; padding:5px"> Uni Grade</td>           
            <td  class="gridViewHeader"  colspan="15" style="text-align:center; width:50%; padding:5px">Average of selected parameter </td>
          </tr>
        <tr>
         <td class="gridViewHeader" style="width:6.2%; text-align:center; padding:5px" rowspan="2">GradeA</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">GradeB</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">GradeC</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">GradeD</td>
         <td class="gridViewHeader" style="width:4.6%; text-align:center; padding:5px" rowspan="2">GradeE</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">RFVCW </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">RFVCCW </td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">H1RFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">H1RFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LFVCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LFVCCW</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">CON</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LowerBulge</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">UpperBulge</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LowerLRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">UpperLRO</td>
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LowerRRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">UpperRRO</td> 
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">LowerDep</td>      
         <td class="gridViewAlternateHeader" style="width:3%; text-align:center; padding:5px">UpperDep</td>      
        </tr>
        </table>     
    <asp:GridView ID="performanceAvgReportTBMRecipeWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="135%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="6%">
       <ItemTemplate>
       <asp:Label ID="performanceAvgReportTBMRecipeWiseRecipeNameLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
        <ItemTemplate>                       
       <div style="border: solid 1px #C3D9FF;">
       <asp:GridView ID="performanceAvgReportTBMRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="102%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                          <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HRFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CON Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerRRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgLowerRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("AvgUpperRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>' CssClass="gridViewItems"></asp:Label>
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
    
    <asp:GridView ID="performanceAvgReportTBMRecipeWiseFooterGridView" runat="server" AutoGenerateColumns="False" 
                        Width="137%" CellPadding="3" ForeColor="#333333" GridLines="None" 
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="AliceBlue" ForeColor="#333333" />
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="4.2%" Visible="True">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportTBMRecipeWiseFooterLabel1" runat="server" Text='Grand Total' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HRFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="R1HCCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CON Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerRRO Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgLowerRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("AvgUpperRRO") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>' CssClass="gridViewItems"></asp:Label>
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
    </asp:UpdatePanel>
 
    </asp:Content>