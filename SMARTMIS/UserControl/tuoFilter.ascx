<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="tuoFilter.ascx.cs" Inherits="SmartMIS.UserControl.tuoFilter" %>
<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

    
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

<script type="text/javascript" language="javascript">
    function onload() {

      var date=document.getElementById("reportMasterFromDateTextBox").value;
      if (date == "") {
          var currentTime = new Date()

          var day = currentTime.getDate();
          var month = currentTime.getMonth() + 1;
          var year = currentTime.getFullYear();

          if (day.toString().length < 2) {
              day = "0" + day;
          }
          if (month.toString().length < 2) {
              month = "0" + month;
          }

          currentTime = (day + "-" + month + "-" + year);

          document.getElementById("reportMasterFromDateTextBox").value = currentTime;
      }
      else
      { }
    }
</script>    

<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />
<asp:reportHeader ID="reportHeader" runat="server" />




 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
  <td style="font-weight:bold; font-family:Arial; font-size:small; width:4%">Date::</td>
   <td style="width: 8%">
      <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="80%" />     
    </td>
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
    <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">

         <asp:Button ID="ViewButton" runat="server" Text="View Report" OnClientClick="javascript:onload();" onclick="ViewButton_Click"  />
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

<asp:Panel ID="QualityReportTBMWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
 <table class="innerTable" cellspacing="1">
        <tr>
         <td class="gridViewHeader" style="width:14%; text-align:left; padding:5px" 
                rowspan="2">machine Name</td>
            <td class="gridViewHeader" style="width:15%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:14%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px"></td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Rep</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Scrap</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">C</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">D</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">E</td>
        </tr>
    </table>
 <asp:GridView ID="performanceReportSizeWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
     <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportSizeWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                   <FooterTemplate>
                 <asp:Label ID="lblworkcentertotal" runat="server" CssClass="gridViewfooterItems1" Width="60%" Text=" Grand Total"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
     <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportSizeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportSizeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>'  CssClass="gridViewItems"></asp:Label>
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
     <tr >
     <td  style="width:13%; text-align:center; padding:5px">
     <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems1" Text=""/>
     </td>
     <td style="width:12%; text-align:center; padding:5px">
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalAQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
      <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalBQuantity()%>'/>
     </td>
           <td style="width:11%; text-align:center; padding:5px">                 
           <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalCQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">

                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalDQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">
           <asp:Label ID="Label11" runat="server"  CssClass="gridViewfooterItems1"  Text='<%# AlltotalEQuantity()%>'/>
           </td>

   </tr>
</table>
                </FooterTemplate>
      </asp:TemplateField>
        </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
  
    </asp:GridView>
  <div class="gridViewHeader" style="height:20px;"> Without Barcode scanning data  </div>
  <asp:GridView ID="UnknownWCMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportUnKnownWiseWCNameLabel" runat="server" Text='<%# Eval("WCName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
              
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportUnknownWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportUnknownWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportUnknownWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
     <tr >
     <td  style="width:13%; text-align:center; padding:5px">
     <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems1" Text=""/>
     </td>
     <td style="width:12%; text-align:center; padding:5px">
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AllTotalUnknowncheckedQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownAQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
      <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownBQuantity()%>'/>
     </td>
           <td style="width:11%; text-align:center; padding:5px">                 
           <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownCQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">

                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownDQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">
           <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownEQuantity()%>'/>
           </td>

   </tr>
</table>
     </FooterTemplate>
    </asp:TemplateField>
     </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
  
    </asp:GridView>
  </asp:Panel>
  
<asp:Panel ID="QualityReportRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
 <table class="innerTable" cellspacing="1">
        <tr>
        
            <td class="gridViewHeader" style="width:16%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" style="width:13%; text-align:left; padding:5px"></td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Rep</td>
            <td class="gridViewHeader" style="width:13%; text-align:center; padding:5px">Scrap</td>
        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">C</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">D</td>
            <td class="gridViewAlternateHeader" style="width:13%; text-align:center; padding:5px">E</td>
        </tr>
    </table>
                       
 <asp:GridView ID="performanceReportRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15.5%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportrecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportrecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
  <div class="gridViewHeader" style="height:20px;"> Without Barcode scanning data  </div>
  <asp:GridView ID="UnknownRecipeMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
       <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="performanceReportUnKnownRecipeWiseWCNameLabel" runat="server" Text='<%# Eval("WCName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
              
            </asp:TemplateField>
        </Columns>
       <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportUnknownRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportUnknownRecipeWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportUnknownRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportUnknownRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
     <tr >
     <td  style="width:16.5%; text-align:center" >
     <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems" Text=""/>
     </td>
     <td style="width:14%; text-align:center">
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AllTotalUnknowncheckedQuantity()%>'/>
    </td>
    <td style="width:12%; text-align:center">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownAQuantity()%>'/>
    </td>
    <td style="width:12%; text-align:center">
    <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownBQuantity()%>'/>
    </td>
    <td style="width:13%; text-align:center">                 
    <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownCQuantity()%>'/>
    </td>
    <td style="width:13%; text-align:center">
    <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownDQuantity()%>'/>
    </td>
   <td style="width:13%; text-align:center">
   <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AllTotalUnknownEQuantity()%>'/>
   </td>

   </tr>
</table>
     </FooterTemplate>
    </asp:TemplateField>
     </Columns>
     <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
  
    </asp:GridView>
     
  </asp:Panel>