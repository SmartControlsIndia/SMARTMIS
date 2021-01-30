<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TUOBarcodeWiseReport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.TUO.TUOBarcodeWiseReport" Title="TUO BarcodeWise Report" %>

<asp:Content ID="TUObarcodeWiseReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <style>
.links
{
    text-decoration:none;
    color:#0000EE;
    font-family:Verdana;
	font-weight: bold;
	font-size:12px;
	text-align:left;
	padding:2px;
}
.links:hover
{
      text-decoration:underline;      
}
    .close {
	background-color: #4C4C4C;
    background: -moz-linear-gradient(top, #272727, #4C4C4C);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#272727), to(#4C4C4C));
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
	border-radius: 12px;
  .box-shadow;
  .transition;
  .transition-delay(.2s);
  &:hover { background: #00d9ff; .transition; }
}
    .close:hover
    {
        background-color: #272727;
    background: -moz-linear-gradient(top, #4C4C4C, #272727);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#4C4C4C), to(#272727));
        }
.dialogPanelCSS
{
    padding:12px;
    left:10%;
    top:50px;
    z-index:2000;
    position:fixed;
    background-color: #FF9933;
    background: -moz-linear-gradient(top, #C5DEE1, #E8EDFF);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#C5DEE1), to(#E8EDFF));
    -webkit-border-radius: 5px;
	-moz-border-radius: 5px;
	border-radius: 5px;
    }
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
.alrtPopup
{
    padding:7px;width:30%;max-width:30%;height:auto;
    position:fixed;
    z-index: 1080;top:75px;left: 35%;
    -moz-border-radius: 15px;-webkit-border-radius: 15px;border-radius:15px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background:#fff8c4 10px 50%;
    border:1px solid #f2c779;
    color:#555;
    font: bold 12px verdana;
}

</style>
<script>

    function onlyNumbers(event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }


</script>


 
<script>
    function hideDialog() {
        $('#ctl00_masterContentPlaceHolder_dialogPanel').fadeOut(1500);
        $('#ctl00_masterContentPlaceHolder_backDiv').fadeOut(1500);
    }
    setTimeout(function() {
    $('.alrtPopup').fadeOut(1500);
}, 4000);

//document.documentElement.style.height = "100%";
//document.body.style.height = "100%";
</script>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
<table width="100%"><tr><td width="95%" align="center"><h2>TUO BarCode Wise Report</h2></td>
<td width="5%" align="right"><div><asp:LinkButton runat="server" ID="ExportExcel" 
        onclick="tbExportToexcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton></div>
</td></tr></table>
<asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
<div style="text-align:center">
 <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="PopupUpdatePanel">
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
 <asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
 <asp:UpdatePanel ID="PopupUpdatePanel" runat="server">
 <ContentTemplate>
<asp:Label ID="ShowWarning" runat="server" Text="" CssClass="alrtPopup" Visible="false"></asp:Label>
 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
  <div id="RediobuttonDiv" runat="server" style="text-align:center; border-width:thin; border-style:solid">  
     <asp:RadioButton ID="BarcodeFromTOWiseReport" Text="BarcodeFromToWiseReport" 
          GroupName="BarcodeReport" Checked="true" runat="server" 
          oncheckedchanged="BarcodeFromTOWiseReport_CheckedChanged" AutoPostBack="true" /> <asp:RadioButton ID="DateFromTOReport" Text=" DateFromToWiseReport" GroupName="barcodeReport"
         runat="server"  oncheckedchanged="BarcodeFromTOWiseReport_CheckedChanged" AutoPostBack="true" /> </div>
         
 <div id="DatefromtoDiv" visible="false" runat="server">
 <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
      <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%"/></td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
      <td style="width: 8%">
      <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
      <asp:Button ID="ViewButton" runat="server" CausesValidation="false" Text="View Report" onclick="ViewButton_Click"/>&nbsp;</td>             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%"> select Machine &nbsp; ::</td>      
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">         
       <asp:DropDownList ID="MachineDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>All</asp:ListItem>
                   <asp:ListItem>TUO-1</asp:ListItem>
                   <asp:ListItem>TUO-2</asp:ListItem>
                   <asp:ListItem>TUO-3</asp:ListItem>
               </asp:DropDownList>            
       </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; select Grade &nbsp; ::</td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
       <asp:DropDownList ID="GradeDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>E</asp:ListItem>
                   <asp:ListItem>D</asp:ListItem>
                   <asp:ListItem>C</asp:ListItem>
                   <asp:ListItem>B</asp:ListItem>
                   <asp:ListItem>A</asp:ListItem>
                   <asp:ListItem>All</asp:ListItem>
               </asp:DropDownList>
        </td>       
   </tr>
  </div>
    <div id="BarcodeFromToDiv" runat="server">
     <tr>    
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
      <td style="width: 8%">
          <asp:TextBox ID="BarcodeFromTextBox" runat="server" MaxLength="10" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
      <td style="width: 8%">
          <asp:TextBox ID="barcodeToTextBox" runat="server" MaxLength="3" onkeypress="return onlyNumbers(event);"></asp:TextBox>
      </td>
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
      <asp:Button ID="BarcodeWiseButton" runat="server" CausesValidation="false" Text="View Report" onclick="ViewButton_Click"/>&nbsp;</td>             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%">  &nbsp; </td>      
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">         
       </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; &nbsp; </td>       
       <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
        </td>       
   </tr>
    
   </div>

   
</table>
<asp:Panel ID="Panel1" runat="server" Width=1100PX ScrollBars="Both" Height=500PX >

<%--<table style="width:240%">
        <tr>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">MachineName</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">DateTime</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RecipeCode</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">BarCode </td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">Total_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWLFVOA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFV_OA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_Deg</td>        
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CWRFVOA_1H_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_N</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_Deg</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWRFVOA_1H_Rank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_N</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_Deg</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CCWLFVOA_Rank</td>  
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CON_N </td>         
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">CON_Rank </td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">PLY_N</td>      
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">PLY_Rank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">TotalRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RORank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1OARank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1OAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOAAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOARank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">RROCOAAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1BulgeRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeAmount</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeAngle</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1BulgeRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentAmount</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentAngle</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROT1DentRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentAmount</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentAngle</td>                          
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LROB1DentRank</td> 
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">UpperRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerAmount</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerAngle</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">LowerRank</td>
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticAmount</td>               
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticAngle</td>               
         <td class="gridViewHeader" style="width:4%; text-align:center; padding:5px">StaticRank</td>               
        </tr>
</table>--%>
                    <asp:GridView ID="MainGridView" runat="server" AutoGenerateColumns="False" 
                        Width="240%" CellPadding="3" ForeColor="#333333" GridLines="Both" 
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="True" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                           <Columns>
                        <asp:TemplateField HeaderText="MachineName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <asp:Label ID="performanceReportBarcodeWiseWcNameLabel" runat="server" Text='<%# Eval("wcName") %>' CssClass="gridViewItems"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="DateTime" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarcodewiseDateTimeLabel" runat="server" Text='<%# Eval("dtandTime") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RecipeCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarcodeWiseRecipeLabel" runat="server" Text='<%# Eval("recipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="Tyre Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarCodeWiseBarcodeLabel" runat="server" Text='<%# Eval("barcode") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="TotalRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarcodeWiseTotalRankLabel" runat="server" Text='<%# Eval("Total_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CW_LFVOA_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWLFVOA_NLabel" runat="server" Text='<%# Eval("CWLFVOA_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CW_LFVOA_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWLFVOA_RankLabel" runat="server" Text='<%# Eval("CWLFVOA_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CWRFV_OA_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWRFV_OA_NLabel" runat="server" Text='<%# Eval("CWRFV_OA_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CWRFV_OA_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWRFV_OA_RankLabel" runat="server" Text='<%# Eval("CWRFV_OA_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CWRFVOA_1H_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWRFVOA_1H_NLabel" runat="server" Text='<%# Eval("CWRFVOA_1H_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CWRFVOA_1H_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCWRFVOA_1H_RankLabel" runat="server" Text='<%# Eval("CWRFVOA_1H_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns> 
                           <Columns>
                                <asp:TemplateField HeaderText="CCWRFVOA_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarcodeWiseCONCCWRFVOA_NLabel" runat="server" Text='<%# Eval("CCWRFVOA_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CCWRFVOA_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportBarcodeWiseCONCCWRFVOA_RankLabel" runat="server" Text='<%# Eval("CCWRFVOA_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CCWRFVOA_1H_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseBLGCCWRFVOA_1H_NLabel" runat="server" Text='<%# Eval("CCWRFVOA_1H_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CCWRFVOA_1H_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseBLGCCWRFVOA_1H_RankLabel" runat="server" Text='<%# Eval("CCWRFVOA_1H_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CCWLFVOA_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLROCCWLFVOA_NLabel" runat="server" Text='<%# Eval("CCWLFVOA_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CCWLFVOA_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLROCCWLFVOA_RankLabel" runat="server" Text='<%# Eval("CCWLFVOA_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROCON_NLabel" runat="server" Text='<%# Eval("CON_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="CON_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROCON_RankLabel" runat="server" Text='<%# Eval("CON_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="PLY_N" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROPLY_NLabel" runat="server" Text='<%# Eval("PLY_N") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>         
                           <Columns>
                                <asp:TemplateField HeaderText="PLY_Rank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROPLY_RankLabel" runat="server" Text='<%# Eval("PLY_Rank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="TOTALRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROtOTAL_RankLabel" runat="server" Text='<%# Eval("TotalRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RORank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRRORO_RankLabel" runat="server" Text='<%# Eval("RORank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1OAAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseRFVLROT1OAAmountLabel" runat="server" Text='<%# Eval("LROT1OAAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1OARank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseRFVLROT1OARankLabel" runat="server" Text='<%# Eval("LROT1OARank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1OAAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseR1HLROB1OAAmountLabel" runat="server" Text='<%# Eval("LROB1OAAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1OARank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseR1HLROB1OARankLabel" runat="server" Text='<%# Eval("LROB1OARank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RROCOAAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLFVRROCOAAmountLabel" runat="server" Text='<%# Eval("RROCOAAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="RROCOARank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLFVRROCOARankLabel" runat="server" Text='<%# Eval("RROCOARank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1BulgeAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCONLROT1BulgeAmountLabel" runat="server" Text='<%# Eval("LROT1BulgeAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1BulgeRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCONLROT1BulgeRankLabel" runat="server" Text='<%# Eval("LROT1BulgeRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1BulgeAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseBLGLROB1BulgeAmountLabel" runat="server" Text='<%# Eval("LROB1BulgeAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1BulgeRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseBLGLROB1BulgeRankLabel" runat="server" Text='<%# Eval("LROB1BulgeRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1DentAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLROLROT1DentAmountLabel" runat="server" Text='<%# Eval("LROT1DentAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROT1DentRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseLROLROT1DentRankLabel" runat="server" Text='<%# Eval("LROT1DentRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1DentAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROLROB1DentAmountLabel" runat="server" Text='<%# Eval("LROB1DentAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LROB1DentRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROLROB1DentRankLabel" runat="server" Text='<%# Eval("LROB1DentRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="UpperAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROUpperAmountLabel" runat="server" Text='<%# Eval("UpperAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="UpperRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseCRROUpperRankLabel" runat="server" Text='<%# Eval("UpperRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LowerAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseRFVLowerAmountLabel" runat="server" Text='<%# Eval("LowerAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="LowerRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseRFVLowerRankLabel" runat="server" Text='<%# Eval("LowerRank") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="StaticAmount" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseR1HStaticAmountLabel" runat="server" Text='<%# Eval("StaticAmount") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                           <Columns>
                                <asp:TemplateField HeaderText="StaticRank" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                            <asp:Label ID="performanceReportbarcodeWiseR1HStaticRankLabel" runat="server" Text='<%# Eval("StaticRank") %>' CssClass="gridViewItems"></asp:Label>
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


<asp:Panel ID="ExcelPanel" runat="server">
<asp:GridView Width="100%" ID="ExcelGridView" runat="server" AutoGenerateColumns="False">
<Columns>
</Columns>
</asp:GridView>
</asp:Panel>

<%--
</asp:UpdatePanel>
<asp:Panel ID="ExcelPanel" runat="server">
<asp:Label ID="ExcelLabel" runat="server" Text=""></asp:Label>
</asp:Panel>--%>

</asp:Content>