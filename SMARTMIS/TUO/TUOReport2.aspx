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
    <asp:GridView ID="performanceReport2TBMWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="225%" CellPadding="3" GridLines="Both" 
        onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound" 
    AllowPaging="false" AllowSorting="false" PageSize="5" CssClass="TBMTable" 
        ShowFooter="true" HorizontalAlign="Center">
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" />
       <Columns>
       <asp:TemplateField HeaderText="WorkCenter Name" ItemStyle-HorizontalAlign="Left" 
               ItemStyle-Width="4.5%">
       <ItemTemplate>
       <asp:Label ID="performanceReport2TBMWiseWCNameLabel" runat="server" 
               Text='<%# Eval("workCenterName") %>'></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       
                           <Columns>
                        <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" 
                                   ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" 
                                    Text='<%# Eval("tireType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="6.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCheckedLabel" runat="server" 
                                                Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="3.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseAGradeLabel" runat="server" 
                                                Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center"  
                                    ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBGradeLabel" runat="server" 
                                                Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="REJ" ItemStyle-HorizontalAlign="Center"  
                                    ItemStyle-Width="2.7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseEGradeLabel" runat="server" 
                                                Text='<%# Eval("E") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  
                                    ItemStyle-Width="2.6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVGradeLabel" runat="server" 
                                                Text='<%# Eval("RFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HGradeLabel" runat="server" 
                                                Text='<%# Eval("R1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVGradeLabel" runat="server" 
                                                Text='<%# Eval("LFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONGradeLabel" runat="server" 
                                                Text='<%# Eval("CON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGGradeLabel" runat="server" 
                                                Text='<%# Eval("BLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLROGradeLabel" runat="server" 
                                                Text='<%# Eval("LRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROGradeLabel" runat="server" 
                                                Text='<%# Eval("CRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseDEFGradeLabel" runat="server" 
                                                Text='<%# Eval("DEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVDGradeLabel" runat="server" 
                                                Text='<%# Eval("DRFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HDGradeLabel" runat="server" 
                                                Text='<%# Eval("DR1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVDGradeLabel" runat="server" 
                                                Text='<%# Eval("DLFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONDGradeLabel" runat="server" 
                                                Text='<%# Eval("DCON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGDGradeLabel" runat="server" 
                                                Text='<%# Eval("DBLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLRODGradeLabel" runat="server" 
                                                Text='<%# Eval("DLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRRODGradeLabel" runat="server" 
                                                Text='<%# Eval("DCRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseDDEFGradeLabel" runat="server" 
                                                Text='<%# Eval("DDEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseRFVEGradeLabel" runat="server" 
                                                Text='<%# Eval("ERFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseR1HEGradeLabel" runat="server" 
                                                Text='<%# Eval("ER1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLFVEGradeLabel" runat="server" 
                                                Text='<%# Eval("ELFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCONEGradeLabel" runat="server" 
                                                Text='<%# Eval("ECON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseBLGEGradeLabel" runat="server" 
                                                Text='<%# Eval("EBLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseLROEGradeLabel" runat="server" 
                                                Text='<%# Eval("ELRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRRO" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseCRROEGradeLabel" runat="server" 
                                                Text='<%# Eval("ECRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" 
                                    ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMWiseEDEFGradeLabel" runat="server" 
                                                Text='<%# Eval("EDEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>

</asp:Panel>
 
<asp:Panel ID="productionReport2RecipeWiseMainPanel" runat="server" ScrollBars="Horizontal" Width="1250PX" Height="100%" CssClass="panel" >
  
<asp:GridView ID="performanceReport2TBMRecipeWiseMainGridView" CssClass="TBMTable" runat="server" AutoGenerateColumns="False"
    Width="225%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound" 
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" />
       <Columns>
       <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="4.7%">
       <ItemTemplate>
       <asp:Label ID="performanceReport2TBMRecipeWiseRecipeNameLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
       
       <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="3%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="REJ" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        
                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVGradeLabel" runat="server" Text='<%# Eval("RFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HGradeLabel" runat="server" Text='<%# Eval("R1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVGradeLabel" runat="server" Text='<%# Eval("LFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONGradeLabel" runat="server" Text='<%# Eval("CON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGGradeLabel" runat="server" Text='<%# Eval("BLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLROGradeLabel" runat="server" Text='<%# Eval("LRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRROGradeLabel" runat="server" Text='<%# Eval("CRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseDEFGradeLabel" runat="server" Text='<%# Eval("DEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVDGradeLabel" runat="server" Text='<%# Eval("DRFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HDGradeLabel" runat="server" Text='<%# Eval("DR1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVDGradeLabel" runat="server" Text='<%# Eval("DLFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONDGradeLabel" runat="server" Text='<%# Eval("DCON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGDGradeLabel" runat="server" Text='<%# Eval("DBLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLRODGradeLabel" runat="server" Text='<%# Eval("DLRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRRODGradeLabel" runat="server" Text='<%# Eval("DCRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseDdefGradeLabel" runat="server" Text='<%# Eval("DDEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                           <Columns>
                                <asp:TemplateField HeaderText="RFV" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseRFVEGradeLabel" runat="server" Text='<%# Eval("ERFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="R1H" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseR1HEGradeLabel" runat="server" Text='<%# Eval("ER1H") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LFV" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLFVEGradeLabel" runat="server" Text='<%# Eval("ELFV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCONEGradeLabel" runat="server" Text='<%# Eval("ECON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="BLG" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseBLGEGradeLabel" runat="server" Text='<%# Eval("EBLG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseLROEGradeLabel" runat="server" Text='<%# Eval("ELRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CRO" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseCRROEGradeLabel" runat="server" Text='<%# Eval("ECRRO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                  <Columns>
                                <asp:TemplateField HeaderText="DEF" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReport2TBMRecipeWiseEdefGradeLabel" runat="server" Text='<%# Eval("EDEF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>
 
 
 </asp:Panel>
 
 
 </ContentTemplate>
 <Triggers>
        <asp:PostBackTrigger ControlID="expToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>