<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReport1.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.TUOReport1" Title="Performance Report Curing WC Wise" %>

<asp:Content ID="PerformanceReportCuringWiseContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder">
    <style>
    .saveLink {
  padding:5px;
  background-color: #FF9933;
  background: -moz-linear-gradient(top, #FCAE41, #FF9933);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#FCAE41), to(#FF9933));
  border: 1px solid #666;
  color:#000;
  text-decoration:none;
   font-weight:bold;
}
    </style>
    <link href="../Style/master.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
    <link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
    <%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
    <asp:HiddenField id="tuoFilterPerformanceReportTUOWiseSizeDropdownlistHidden" runat="server" value="All" />
    <asp:HiddenField id="tuoFilterPerformanceReportTUOWiseRecipeDropdownlistHidden" runat="server" value="All" />
    
    <asp:HiddenField id="magicHidden" runat="server" value="" />
  <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
   <asp:HiddenField id="viewQueryHidden" runat="server" value="" />
     <script type="text/javascript" language="javascript">

         function showReport(queryString) {
             document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
             document.getElementById('<%= this.magicButton.ClientID %>').click();
         }
    </script>
    <asp:reportHeader ID="reportHeader" runat="server" />
     <asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>
    
    <asp:Label ID="fromDate" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="toDate" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="queryStringLabel" runat="server" Visible="false"></asp:Label>
    
    <div id="modalPageForWorkCenter">
   <div class="modalBackground">
   </div>
  <div class="curingHMIWCModalContainer">
    <div class="curingHMIWCModal">
   <div class="curingHMIWCModalTop"><a href="javascript:hideModal('modalPageForWorkCenter')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
   <div class="modalBody">
   <table id="aa" class="innerTable" cellspacing="0" cellpadding="0">
   <tr>
   <td style="width: 100%"></td>
   </tr>
   <tr>
   <td>
    
   <asp:Panel ID="curingOperatorPlanningWCNamePanel" runat="server" ScrollBars="Vertical" Height="380" CssClass="panel" >
   <asp:GridView ID="performanceReportBarcodeDetailGridView" runat="server" AutoGenerateColumns="False" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" AllowPaging="false" AllowSorting="true" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
    
    <Columns>
       <asp:TemplateField HeaderText="TBM Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("wcname") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
    <Columns>
   <asp:TemplateField HeaderText="TUO Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="machinename"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("machinename") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Curing Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="CuringMachineName" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# fillCuringWCName(DataBinder.Eval(Container.DataItem,"Barcode")) %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
     
    <Columns>
   <asp:TemplateField HeaderText="TUO Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="barcode" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:HyperLink ID="hlDetails1" Text='<%# Eval("barcode") %>' runat="server"  NavigateUrl='<%# "~/Report/tyreGeneaology.aspx?gtbarcode=" + Eval("barcode") %>' Target="_blank" /> 
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
  </td>
  </tr>
  <tr>
  <td style="padding-top: 8px">
  </td>
  </tr>
</table>
 </div>
 </div>
   </div>
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
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseSizeDropdownlist"  
                   Width="60%" runat="server" 
                   CausesValidation="false" AutoPostBack="True" 
                   style="margin-bottom: 0px" 
                   onselectedindexchanged="DropDownList_SelectedIndexChanged"> </asp:DropDownList>
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseRecipeDropdownlist"  
                   Width="80%" runat="server" CausesValidation="false"  AutoPostBack="true" 
                   onselectedindexchanged="DropDownList_SelectedIndexChanged">
             </asp:DropDownList>
        </td>        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterOptionDropDownList"  Width="80%" runat="server" 
                   CausesValidation="false"  AutoPostBack="true" 
                   onselectedindexchanged="DropDownList_SelectedIndexChanged">
            <asp:ListItem  Value="0" Text="No"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        </td>
   <td style="width:3%"> <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" /></td>    

       <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
            <asp:Button ID="ViewButton" runat="server" Text="View Report"  onclick="ViewButton_Click" Visible="false" /> 
            <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
            
       </td>
       
    </tr>
</table>

<asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 

    <asp:UpdatePanel ID="TUOReport1UpdatePanel" runat="server">
        <ContentTemplate>
 
            <asp:Label ID="showDownload" runat="server" Text=""></asp:Label>


    <asp:Panel ID="QualityReportTBMWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
  
 <asp:GridView ID="performanceReportSizeWiseMainGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound"
    Width="100%" CellPadding="3" GridLines="Both" OnDataBound = "OnDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" CssClass="TBMTable"
            ShowFooter="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" />
     <Columns>
            <asp:TemplateField HeaderText="WorkCenter Name" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <asp:Label ID="performanceReportSizeWiseWCNameLabel" runat="server" Text='<%# Eval("workCenterName") %>'></asp:Label>
                </ItemTemplate>
                   
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true">
                <ItemTemplate>
                    <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
          <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center">
           <ItemTemplate>
           <asp:Label ID="performanceReportSizeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
           </ItemTemplate>
           </asp:TemplateField>
           </Columns>
        <Columns>
           <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center">
           <ItemTemplate>
           <asp:Label ID="performanceReportSizeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
            </ItemTemplate>
           </asp:TemplateField>
                            </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportSizeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
        <Columns>
                                <asp:TemplateField HeaderText="View Grade" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Button ID="ErankViewDetailButton" runat="server" OnClick="Button_Click" Text="View" CssClass="saveLink" ToolTip="View Scrap Barcode detail onwords TBM" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>
</asp:Panel>
  
  <asp:Panel ID="QualityReportRecipeWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
                                       
    <asp:GridView ID="performanceReportrecipeWiseChildGridView" CssClass="TBMTable" runat="server" AutoGenerateColumns="False" OnDataBound = "OnDataBound" 
                        Width="100%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="true" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" />
                        
                    <Columns>
                  <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15.5%">
                  <ItemTemplate>
                        <asp:Label ID="performanceReportSizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                   </ItemTemplate>
                
                   </asp:TemplateField>
                           </Columns>

                            <Columns>
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportrecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportRecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>'></asp:Label>
                                   </ItemTemplate>
                

                                </asp:TemplateField>
                            </Columns>
                            
        <Columns>
                                <asp:TemplateField HeaderText="View Grade" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Button ID="ErankRecipeWiseViewDetailButton" runat="server" CssClass="saveLink" OnClick="Button_Click" Text="View" ToolTip="View Scrap Barcode detail onwords TBM" />
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