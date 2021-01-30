<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReport3.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.TUOReport3" Title="Performance Report With Average Details" %>

<asp:Content  ID="performanceReportAverageContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder" >

    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
  <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />  
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
            <asp:ListItem  Value="1" Text="Standard Deviation"></asp:ListItem>
             </asp:DropDownList>
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">     
            <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" />
        </td>
        
       
       
    </tr>
</table>
<asp:ScriptManager ID="TuoRejectionDetailScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
        
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>

  <asp:Panel ID="performanceAvgReportMainPanel" runat="server" ScrollBars="Horizontal" Width="1250PX" Height="100%" CssClass="panel" >
         
    <asp:GridView ID="performanceAvgReportMainGridView" runat="server" AutoGenerateColumns="False"
    Width="140%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound" 
    AllowPaging="false" AllowSorting="false" PageSize="5" CssClass="TBMTable">
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" />
       <Columns>
           <asp:TemplateField HeaderText="WorkCenter Name" ItemStyle-HorizontalAlign="Left">
               <ItemTemplate>
                <asp:Label ID="performanceAvgReportWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>'></asp:Label>
               </ItemTemplate>
           </asp:TemplateField>
       </Columns>
       <Columns>
                                <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="H1RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="H1RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CONICITY" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="RRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                             
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperDep" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>   
                        
</asp:Panel>
 
    <asp:Panel ID="productionReport2RecipeWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1250PX" Height="100%" CssClass="panel" >
    <asp:GridView ID="performanceAvgReportTBMRecipeWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="135%" CellPadding="3" CssClass="TBMTable" GridLines="Both" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="true" >
       <Columns>
       <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left">
       <ItemTemplate>
       <asp:Label ID="performanceAvgReportTBMRecipeWiseRecipeNameLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       
                          <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>  
                             <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportEGradeLabel" runat="server" Text='<%# Eval("E") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                   
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                             <Columns>
                                <asp:TemplateField HeaderText="RFVCCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportRFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgRFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="H1RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HRFVCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="H1RFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportR1HCCWGradeLabel" runat="server" Text='<%# Eval("AvgH1RFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LFVCCW" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLFVCCWGradeLabel" runat="server" Text='<%# Eval("AvgLFVCCW") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportCONGradeLabel" runat="server" Text='<%# Eval("CON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerBulge" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerBulgeGradeLabel" runat="server" Text='<%# Eval("AvgLowerBulge") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperBulge" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperBulgeGradeLabel" runat="server" Text='<%# Eval("AvgUpperBulge") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerLRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerLROGradeLabel" runat="server" Text='<%# Eval("AvgLowerLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperLRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperLROGradeLabel" runat="server" Text='<%# Eval("AvgUpperLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="RRO" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" Text='<%# Eval("AvgRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="LowerDep" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportLowerDepGradeLabel" runat="server" Text='<%# Eval("AvgLowerDEP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <Columns>
                                <asp:TemplateField HeaderText="UpperDep" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceAvgReportUpperDepGradeLabel" runat="server" Text='<%# Eval("AvgUpperDEP") %>'></asp:Label>
                                    </ItemTemplate>
                                   
      </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:GridView ID="performanceAvgReportTBMRecipeWiseFooterGridView" runat="server" AutoGenerateColumns="False" 
                        Width="137%" CellPadding="3" ForeColor="#333333" GridLines="Both" 
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
                                            <asp:Label ID="performanceAvgReportLowerRROGradeLabel" runat="server" Text='<%# Eval("AvgRRO") %>' CssClass="gridViewItems"></asp:Label>
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
 <Triggers>
        <asp:PostBackTrigger ControlID="expToExcel" />
        </Triggers>
    </asp:UpdatePanel>
 
    </asp:Content>