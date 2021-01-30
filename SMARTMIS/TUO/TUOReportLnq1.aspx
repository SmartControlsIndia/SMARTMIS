<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReportLnq1.aspx.cs" MasterPageFile="~/NewTUOReportMaster.Master"  Inherits="SmartMIS.TUO.TUOReportLnq1" Title="Performance Report TUO Wise" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>

<asp:Content ID="plantPerformanceReportContent" runat="server" ContentPlaceHolderID="NewtuoReportMasterContentPalceHolder">
    <link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
 <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />  
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
    .close {
	background: #606061;
	color: #FFFFFF;
	line-height: 25px;
	position: absolute;
	right: -12px;
	text-align: center;
	top: -10px;
	width: 24px;
	text-decoration: none;
	font-weight: bold;
	-webkit-border-radius: 12px;
	-moz-border-radius: 12px;
	border-radius:12px;
  .box-shadow;
  .transition;
  .transition-delay(.2s);
  &:hover { background: #00d9ff; .transition; }
}
    </style>
<script type="text/javascript" language="javascript" src="../Script/download.js"></script>
    
    

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


 <asp:ScriptManager ID="curingOperatorPlanningScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="curingOperatorPlanningUpdatePanel">
        <ProgressTemplate>
                <div class="backDiv">             
             <div align="center" class="waitBox">
             
<div id="bookG">
<div id="blockG_1" class="blockG">
</div>
<div id="blockG_2" class="blockG">
</div>
<div id="blockG_3" class="blockG">
</div>
</div>

<br />

             <h2><font color="#888888">Loading, please wait............</font> </h2> 
             </div>
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
        
 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
 <tr style="background-color:#507CD1">

 <td colspan="14" style="font-weight:bold; font-family:Arial; font-size:large; color:White;">TUO PERFORMANCE REPORT </td>
 </tr>
    <tr>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">From::</td>
        <td style="width: 10%">
         <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="80%" />     
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">To::</td>
        <td style="width: 10%">
         <myControl:calenderTextBox ID="reportMasterToDateTextBox" runat="server" Disabled="false" Width="80%" />     
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
            <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" /> 

            <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
            
        </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Size::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:5%">
           
             <asp:DropDownList ID="performanceReportTUOWiseSizeDropdownlist"  Width="95%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true" Visible="true" > </asp:DropDownList>
        </td>
        
        
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="performanceReportTUOWiseRecipeDropdownlist"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true" Visible="true">
             </asp:DropDownList>
        </td>
        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:5%">
           
             <asp:DropDownList ID="optionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="Numbers"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        </td>
         <td style="width:3%"> <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" /></td>    
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
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound" AllowPaging="false" AllowSorting="true" PageSize="5" ShowHeader="True" >
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
       <asp:TemplateField HeaderText="Curing Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="CuringMachineName" ItemStyle-Width="13%">
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
</table>
 </div>
 </div>
   </div>
    </div>
   </td>

    </tr>
</table>

<asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound">
            <Columns>
            </Columns>
    </asp:GridView>
</asp:Panel>


<asp:Panel ID="QualityReportTUOWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >

 <asp:GridView ID="performanceReportTUOWiseMainGridView" runat="server" AutoGenerateColumns="False" CssClass="TBMTable"
    Width="100%" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound">
     <Columns>
            <asp:TemplateField HeaderText="Machine Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="11.5%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportTUOWiseWCNameLabel" runat="server" Text='<%# Eval("machinename") %>'></asp:Label>
                </ItemTemplate>                   
            </asp:TemplateField>
        </Columns>                     
                        <Columns>
                            <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="16%">
                                <ItemTemplate>
                                        <asp:Label ID="performanceReportTUOWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <Columns>            
                                <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="11%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="14%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOWiseEGradeLabel" runat="server"  Text='<%# Eval("E") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                    
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                                <ItemTemplate>
                                <asp:Button ID="ErankViewDetailButton" runat="server" CssClass="saveLink" OnClick="Button_Click" Text="View" ToolTip="View Scrap Barcode detail onwords TBM" />
                                </ItemTemplate>                                       
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>
  </asp:Panel>
 
 
<asp:Panel ID="QualityReportRecipeTUOWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" >
       
<asp:GridView ID="performanceReportRecipeTUOWiseGridView" runat="server" AutoGenerateColumns="False" CssClass="TBMTable"
    Width="100%" CellPadding="3" GridLines="Both" onrowdatabound="GridView_RowDataBound" OnDataBound = "OnDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="true" >
        <Columns>
            <asp:TemplateField HeaderText="Tyre Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14.2%">
                <ItemTemplate>
                        <asp:Label ID="performanceReportRecipeTUOWiseTyreTypeLabel" runat="server" Text='<%# Eval("tireType") %>'></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
       <asp:TemplateField HeaderText="Checked" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11.5%">
                <ItemTemplate>
                    
                   <asp:Label ID="performanceReportTUORecipeWiseCheckedLabel" runat="server" Text='<%# Eval("Checked") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="A" ItemStyle-HorizontalAlign="center" ItemStyle-Width="11.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUOrecipeWiseAGradeLabel" runat="server" Text='<%# Eval("A") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="B" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="11.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseBGradeLabel" runat="server" Text='<%# Eval("B") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="C" ItemStyle-HorizontalAlign="center" ItemStyle-Width="11.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseCGradeLabel" runat="server" Text='<%# Eval("C") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="D" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="11.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseDGradeLabel" runat="server" Text='<%# Eval("D") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="center" ItemStyle-Width="11.5%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportTUORecipeWiseEGradeLabel" runat="server" Text='<%# Eval("E") %>'></asp:Label>
                                   </ItemTemplate>
                              

                                </asp:TemplateField>
                            </Columns>
                                    
  
        <Columns>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="11.5%">
        <ItemTemplate>
        <asp:Button ID="ErankRecipeWiseViewDetailButton" runat="server" CssClass="saveLink" OnClick="Button_Click" Text="View" ToolTip="View Scrap Barcode detail onwords TBM" />
        </ItemTemplate>
        <FooterTemplate>
      
        </FooterTemplate>
   

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