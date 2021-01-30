<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TUOReport5.aspx.cs" MasterPageFile="~/smartMISTUOReportGraphMaster.Master" Inherits="SmartMIS.TUO.TUOReport5" Title="Performance OAY Graph Report" %>

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
 
 
 <table class="innerTable" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:6.5%; text-align:left; padding:5px" 
                rowspan="2">machine Name</td>
            <td class="gridViewHeader" style="width:9%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; width:78%; padding:5px" colspan="13">UNIFORMITY GRADE WITH A & B RANK</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">YTD</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JAN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">FEB</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">APR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAY</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUL</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">AUG</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">SEP</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">OCT</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">NOV</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">DEC</td>
        </tr>
    </table>
  
 <asp:GridView ID="performanceReportOAYGRAFWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="6%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                  
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90%">
         <ItemTemplate>
                    <asp:Label ID="machineEmpty" runat="server" Text=""></asp:Label>
    
                            
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportOAYGRAFWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="9.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("APR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("MAY") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("JUN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("JUL") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("AUG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseJANGradeLabel" runat="server" Text='<%# Eval("SEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseFEBGradeLabel" runat="server" Text='<%# Eval("OCT") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFWiseMARGradeLabel" runat="server" Text='<%# Eval("NOV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAAPRWiseEGradeLabel" runat="server" Text='<%# Eval("DECE") %>' CssClass="gridViewItems"></asp:Label>
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
     
    <table class="innerTable" style="background-color:Gray" width="100%">
     <tr >
         <td  style="width:5%;" class="gridViewItems2">Grand Total</td>
         <td style="width:2%;" class="gridViewItems2"><%= grandtotal %></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandYTD %></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandJAN %></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandFEB%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandMAR%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandAPR%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandMAY%></td>
         
         <td style="width:2%;" class="gridViewItems2"><%= grandJUN%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandJUL%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandAUG%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandSEP%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandOCT%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandNOV%></td>
         <td style="width:2%;" class="gridViewItems2"><%= grandDECE%></td>
         
   </tr>
</table>
  
 </asp:Panel>
  
    <asp:Panel ID="QualityReportOAYGRAFRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
  <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:8%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:6%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; width:80%; padding:5px" colspan="13">UNIFORMITY GRADE WITH A & B RANK</td>
            
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">YTD</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JAN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">FEB</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">APR</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">MAY</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUN</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">JUL</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">AUG</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">SEP</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">OCT</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">NOV</td>
            <td class="gridViewAlternateHeader" style="width:6%; text-align:center; padding:5px">DEC</td>
        </tr>
    </table>
                       
  <asp:GridView ID="performanceReportOAYGRAFRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="8%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportOAYGRAFRecipeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                 
            </asp:TemplateField>
        </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseYTDGradeLabel" runat="server" Text='<%# Eval("YTD") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJANGradeLabel" runat="server" Text='<%# Eval("JAN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseFEBGradeLabel" runat="server" Text='<%# Eval("FEB") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMARGradeLabel" runat="server" Text='<%# Eval("MAR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="APR Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAPRGradeLabel" runat="server" Text='<%# Eval("APR") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseMAYGradeLabel" runat="server" Text='<%# Eval("MAY") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJUNGradeLabel" runat="server" Text='<%# Eval("JUN") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseJULGradeLabel" runat="server" Text='<%# Eval("JUL") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseAUGGradeLabel" runat="server" Text='<%# Eval("AUG") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseSEPGradeLabel" runat="server" Text='<%# Eval("SEP") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseOCTGradeLabel" runat="server" Text='<%# Eval("OCT") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="6%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseNOVGradeLabel" runat="server" Text='<%# Eval("NOV") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportOAYGRAFrecipeWiseDECGradeLabel" runat="server" Text='<%# Eval("DECE") %>' CssClass="gridViewItems"></asp:Label>
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
    </asp:UpdatePanel>

 </asp:Content>