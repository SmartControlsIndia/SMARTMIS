<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReport5.aspx.cs" MasterPageFile="~/smartMISTUOReportGraphMaster.Master" Inherits="SmartMIS.TUO.TUOReport5" Title="Performance OAY Graph Report" %>

<asp:Content ID="performanceReportOAYGrafContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder">

    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />

  <div style="height:20PX;" >
      
          <asp:Label ID="Label14" runat="server" Width="300PX" Height="10px" Text=" "></asp:Label>
     <asp:Label ID="notifyLabel" CssClass="notifyLabel" runat="server" Width="50%" ForeColor="Red" Text="Select Year Option To See Report" Visible="false">
    </asp:Label> 
     </div>
 
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
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
            <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" />
        </td>
       
       
    </tr>
</table>
<script type="text/javascript" language="javascript">
     
        function showReport(queryString) 
        {
            document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
            document.getElementById('<%= this.magicButton.ClientID %>').click();
        }
    </script>
<asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>

<asp:ScriptManager ID="TuoRejectionDetailScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
    <asp:Panel ID="QualityReportOAYGRAFTBMWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
  
 <asp:GridView ID="performanceReportOAYGRAFWiseMainGridView" runat="server" AutoGenerateColumns="False" OnDataBound = "OnDataBound"
    Width="100%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="true" CssClass="TBMTable" >
    <FooterStyle BackColor="#5D7B9D" />
       <Columns>
            <asp:TemplateField HeaderText="WorkCenter Name" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                            <asp:Label ID="performanceReportOAYGRAFWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>'></asp:Label>
                </ItemTemplate>
                  
            </asp:TemplateField>
        </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="YTD" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JAN" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="FEB" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="MAR" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseAPRGradeLabel" runat="server" Text='<%# Eval("APR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="MAY" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMAYGradeLabel" runat="server" Text='<%# Eval("MAY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JUN" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJUNGradeLabel" runat="server" Text='<%# Eval("JUN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JUL" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJULGradeLabel" runat="server" Text='<%# Eval("JUL") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="AUG" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseAUGGradeLabel" runat="server" Text='<%# Eval("AUG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="SEP" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseSEPGradeLabel" runat="server" Text='<%# Eval("SEP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="OCT" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseOCTGradeLabel" runat="server" Text='<%# Eval("OCT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="NOV" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseNOVGradeLabel" runat="server" Text='<%# Eval("NOV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="DEC" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseDECGradeLabel" runat="server" Text='<%# Eval("DECE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>
</asp:Panel>
  
<asp:Panel ID="QualityReportOAYGRAFRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
      
  <asp:GridView ID="performanceReportOAYGRAFRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound"
    AllowPaging="false" AllowSorting="false" CssClass="TBMTable" PageSize="5" ShowFooter="true" >
        <Columns>
            <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFRecipeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                </ItemTemplate>
                 
            </asp:TemplateField>
        </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="YTD" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JAN" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="FEB" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="MAR" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAPRGradeLabel" runat="server" Text='<%# Eval("APR") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="MAY" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMAYGradeLabel" runat="server" Text='<%# Eval("MAY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JUN" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJUNGradeLabel" runat="server" Text='<%# Eval("JUN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="JUL" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJULGradeLabel" runat="server" Text='<%# Eval("JUL") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="AUG" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAUGGradeLabel" runat="server" Text='<%# Eval("AUG") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="SEP" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseSEPGradeLabel" runat="server" Text='<%# Eval("SEP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="OCT" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseOCTGradeLabel" runat="server" Text='<%# Eval("OCT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="NOV" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseNOVGradeLabel" runat="server" Text='<%# Eval("NOV") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="DEC" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseDECGradeLabel" runat="server" Text='<%# Eval("DECE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
        </Columns>
    </asp:GridView>
     
  </asp:Panel>
  <div style="text-align:center">
     <asp:Chart ID="performanceReportOAYGrafTBMChart" runat="server" Width="800PX" Visible="true" BackGradientStyle="None" BorderlineDashStyle="Solid" BorderlineWidth="2" BorderlineColor="AliceBlue" >
     <Series>
     <asp:Series Name="TotalCheckedSeries" ChartType="Column" Color="Green" ShadowOffset="0">
                                                                                    
     </asp:Series>
    </Series>
   
    <Series>
     <asp:Series Name="ABgradeSeries" ChartType="Column" Color="Orange" ShadowOffset="0">
                                                                                    
     </asp:Series>
    </Series>
  <ChartAreas>
  <asp:ChartArea Name="productionReportDailyChart" BackGradientStyle="None" BackSecondaryColor="#B6D6EC" BorderDashStyle="Solid" BorderWidth="2" BorderColor="Blue">
 
  </asp:ChartArea>
   </ChartAreas>
     </asp:Chart>
     </div>

</ContentTemplate>
<Triggers>
        <asp:PostBackTrigger ControlID="expToExcel" />
        </Triggers>
    </asp:UpdatePanel>

 </asp:Content>