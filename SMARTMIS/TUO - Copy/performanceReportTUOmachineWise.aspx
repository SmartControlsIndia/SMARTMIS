<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="performanceReportTUOmachineWise.aspx.cs" MasterPageFile="~/NewTUOReportMaster.Master" Inherits="SmartMIS.TUO.performanceReportTUOmachineWise" Title="Performance Report TUO Wise" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

      


<asp:Content ID="plantPerformanceReportContent" runat="server" ContentPlaceHolderID="NewtuoReportMasterContentPalceHolder">
<link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
 <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />  


    

   <%-- &nbsp;<link rel="stylesheet" href="../Style/master.css" type="text/css" charset="utf-8" />
    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <asp:HiddenField id="magicHidden" runat="server" value="" />
    <asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
    <asp:Button ID="magicButton" runat="server" Text="Magic"  CssClass="masterHiddenButton"></asp:Button>

    <asp:tuowise ID="tuoFilter" runat="server" />
   --%>
   

   

<script type="text/javascript" language="javascript">

    function revealModal(divID) 
    {
        window.onscroll = function()
         { 
        document.getElementById(divID).style.top = document.body.scrollTop; };
        document.getElementById(divID).style.display = "block";
        document.getElementById(divID).style.top = document.body.scrollTop;
    }
    function hideModal(divID)
    {
        document.getElementById(divID).style.display = "none";
    }
    
</script>

<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />
<asp:reportHeader ID="reportHeader" runat="server" />

 <asp:ScriptManager ID="curingOperatorPlanningScriptManager" runat="server"></asp:ScriptManager>
 
 
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="curingOperatorPlanningUpdatePanel">
        <ProgressTemplate>
        <div style="height:500PX; width:1100PX; text-align:center; background-color:Aqua; border-color:Blue">
             <img src="../Images/loading.gif"/>

             <h2>Loading Please wait............ </h2> 
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
 

 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:4%">Date::</td>
        <td style="width: 8%">
         <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="80%" />     
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
          <asp:RadioButton ID="QualityReportTUOWise" runat="server" 
          Text="MachineWise" GroupName="aa" AutoPostBack="True" 
          Checked="True" oncheckedchanged="QualityReportTUOWise_CheckedChanged" />

        </td>
        
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
             <asp:RadioButton ID="QualityReportRecipeTUOWise" runat="server" 
                Text="RecipeWise" GroupName="aa" AutoPostBack="True" 
                oncheckedchanged="QualityReportRecipeTUOWise_CheckedChanged" /></td>
        
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
            <asp:Button ID="ViewButton" runat="server" Text="View Report"  onclick="ViewButton_Click" /> 

            <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
            
        </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Size::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="performanceReportTUOWiseSizeDropdownlist"  Width="60%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true"> </asp:DropDownList>
        </td>
        
        
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="performanceReportTUOWiseRecipeDropdownlist"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
             </asp:DropDownList>
        </td>
        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="optionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="No"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        </td>
        
   </tr>
  <%--  <tr>
    <td class="masterLabel"> Select Date::</td>
    <td style="border-width:medium">
     <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="60%" />     
   </td>
   
    <td style="width:10px">
         <asp:RadioButton ID="QualityReportTUOWise" runat="server" 
          Text="MachineWise" GroupName="aa" AutoPostBack="True" 
          Checked="True" oncheckedchanged="QualityReportTUOWise_CheckedChanged" />
        </td>
    <td style="font-weight:bold; font-family:Arial; font-size:small; width:10px"> 
            <asp:RadioButton ID="QualityReportRecipeTUOWise" runat="server" 
                Text="RecipeWise" GroupName="aa" AutoPostBack="True" 
                oncheckedchanged="QualityReportRecipeTUOWise_CheckedChanged" />
        </td>
    <td  style=" width:10%">
    <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" />
    </td>      
    </tr>--%>
  <tr>
  
   <td colspan="4">
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
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound" AllowPaging="false" AllowSorting="true" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
     <Columns>
       <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left"  ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:HyperLink ID="hlDetails1" Text='<%# Eval("barcode") %>' runat="server"  NavigateUrl='<%# "~/Report/tyreGeneaology.aspx?gtbarcode=" + Eval("barcode") %>' Target="_blank" /> 
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
      <Columns>
   <asp:TemplateField HeaderText="TUO Recipe Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("tireType") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
 <Columns>
   <asp:TemplateField HeaderText="TUO Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("machinename") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
     <Columns>
       <asp:TemplateField HeaderText="Curing Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# fillCuringWCName(DataBinder.Eval(Container.DataItem,"Barcode")) %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
     <Columns>
   <asp:TemplateField HeaderText="TBM Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("WcName") %>' CssClass="gridViewItems"></asp:Label>
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
 <tr>
 </tr>
</table>
 </div>
 </div>
   </div>
    </div>
   </td>

    </tr>
</table>

<asp:Panel ID="QualityReportTUOWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
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
           <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px">ScrapDetail</td>

        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:8%; text-align:center; padding:5px">C</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">D</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">E</td>
            <td class="gridViewAlternateHeader" style="width:10%; text-align:center; padding:5px">ViewDetail</td>

        </tr>
    </table>
 <asp:GridView ID="performanceReportTUOWiseMainGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
     <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="11.5%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTUOWiseWCNameLabel" runat="server" Text='<%# Eval("machinename") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                   <FooterTemplate>
                 <asp:Label ID="lblworkcentertotal" runat="server" CssClass="gridViewfooterItems1" Width="60%" Text=" Grand Total"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
     <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
         <ItemTemplate>
                    
    <asp:GridView ID="performanceReportTUOWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
            <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTUOWiseMachineNameLabel" runat="server" Text='<%# Eval("machineName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTUOWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportTUOWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="15%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseEGradeLabel" runat="server"  Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
        <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
        <ItemTemplate>
        <asp:Button ID="ErankViewDetailButton" runat="server"  CssClass="gridViewButtons" OnClick="Button_Click" Text="View" ToolTip="View Scrap Barcode detail onwords TBM" />
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
     <td  style="width:15%; text-align:center; padding:5px">
     <asp:Label ID="Labelblank" runat="server" CssClass="gridViewfooterItems1" Text=""/>
     </td>
     <td style="width:12%; text-align:center; padding:5px">
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedQuantity()%>'/>
    </td>
    <td style="width:12%; text-align:center; padding:5px">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalAQuantity()%>'/>
    </td>
    <td style="width:12%; text-align:center; padding:5px">
      <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalBQuantity()%>'/>
     </td>
     <td style="width:12%; text-align:center; padding:5px">                 
     <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalCQuantity()%>'/>
      </td>
      <td style="width:13%; text-align:center; padding:5px">

       <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalDQuantity()%>'/>
        </td>
        <td style="width:11%; text-align:center; padding:5px">
         <asp:Label ID="Label11" runat="server"  CssClass="gridViewfooterItems1"  Text='<%# AlltotalEQuantity()%>'/>
           </td>
            <td style="width:11%; text-align:center; padding:5px">
         <asp:Button ID="AllErankDetailButton" runat="server" Text="View" OnClick="Button_Click" CssClass="gridViewButtons"  ToolTip="View All Erank barcode Detail"/>
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
 
 
<asp:Panel ID="QualityReportRecipeTUOWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
 <table class="innerTable" cellspacing="1">
        <tr>
        
            <td class="gridViewHeader" style="width:15%; text-align:left; padding:5px" 
                rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" 
                rowspan="2">Checked</td>
            <td class="gridViewHeader" style="text-align:center; padding:5px" colspan="2">OE</td>
            <td class="gridViewHeader" style="width:12%; text-align:left; padding:5px"></td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px">Rep</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px">Scrap</td>
         <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px">ScrapDetail</td>

        </tr>
        <tr>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">A</td>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">B</td>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">C</td>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">D</td>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">E</td>
            <td class="gridViewAlternateHeader" style="width:12%; text-align:center; padding:5px">ViewDetail</td>

        </tr>
    </table>
                       
 <asp:GridView ID="performanceReportRecipeTUOWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="13%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportRecipeTUOWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
       <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="performanceReportrecipeTUOWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="none" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOrecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E Grade" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>' CssClass="gridViewItems"></asp:Label>
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
    <asp:Label ID="Alltotalcheckedquantity" runat="server" CssClass="gridViewfooterItems1"   Text='<%# AlltotalcheckedRecipeWiseQuantity()%>'/>
    </td>
    <td style="width:10%; text-align:center; padding:5px">
    <asp:Label ID="Label7" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalRecipeWiseAQuantity()%>'/>
    </td>
    <td style="width:11%; text-align:center; padding:5px">
      <asp:Label ID="Label8" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalRecipeWiseBQuantity()%>'/>
     </td>
           <td style="width:11%; text-align:center; padding:5px">                 
           <asp:Label ID="Label9" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalRecipeWiseCQuantity()%>'/>
           </td>
           <td style="width:10%; text-align:center; padding:5px">

                 <asp:Label ID="Label10" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalRecipeWiseDQuantity()%>'/>
           </td>
           <td style="width:11%; text-align:center; padding:5px">
           <asp:Label ID="Label11" runat="server" CssClass="gridViewfooterItems1"  Text='<%# AlltotalRecipeWiseEQuantity()%>'/>
           </td>



</tr>
</table>

</FooterTemplate>

           </asp:TemplateField>
         </Columns>
        <Columns>
        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
        <ItemTemplate>
        <asp:Button ID="ErankRecipeWiseViewDetailButton" runat="server"  CssClass="gridViewButtons" OnClick="Button_Click" Text="View" ToolTip="View Scrap Barcode detail onwords TBM" />
        </ItemTemplate>
        <FooterTemplate>
      <asp:Button ID="AllRecipeErankDetailButton" runat="server"  Text="View" OnClick="Button_Click" CssClass="gridViewButtons"  ToolTip="View All Erank barcode Detail"/>
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