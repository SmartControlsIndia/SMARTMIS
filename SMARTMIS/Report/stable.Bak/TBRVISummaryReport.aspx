<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TBRVISummaryReport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.TBRVISummaryReport" Title="Smart MIS - First TBR Visual Inspection Report" %>

<asp:Content ID="TBRVISummaryReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
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
    background: -moz-linear-gradient(top, #E05539, #FCAE41);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#E05539), to(#FCAE41));
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
    padding:7px;width:25%;max-width:25%;height:auto;
    position:fixed;
    z-index: 1080;top:75px;left: 40%;
    -moz-border-radius: 15px;-webkit-border-radius: 15px;border-radius:15px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background:#fff8c4 10px 50%;
    border:1px solid #f2c779;
    color:#555;
    font: bold 12px verdana;
}

</style>
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
<center><h2>First TBR Visual Inspection Report</h2></center>
    <asp:Label ID="ShowWarning" runat="server" Text="" CssClass="alrtPopup" Visible="false"></asp:Label>
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
    <asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
    <asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
 <asp:UpdatePanel ID="PopupUpdatePanel" runat="server">
    <ContentTemplate>
   <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
       
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;From::</td>
        <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:9%"> select category &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
               <asp:DropDownList ID="FaultTypeDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>REWORK</asp:ListItem>
                   <asp:ListItem>NCMR</asp:ListItem>
               </asp:DropDownList>
              
          </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  &nbsp;&nbsp;&nbsp; select FaultArea &nbsp; ::</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
               <asp:DropDownList ID="FaultAreaDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>All</asp:ListItem>
                   <asp:ListItem>Tread</asp:ListItem>
                   <asp:ListItem>SideWall</asp:ListItem>
                   <asp:ListItem>Bead</asp:ListItem>
                   <asp:ListItem>Carcass</asp:ListItem>
                   <asp:ListItem>Others</asp:ListItem>
               </asp:DropDownList>
          </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:11%">          
              Type:
               <asp:DropDownList ID="displayType" runat="server" AutoPostBack="True" Width="100" 
                   onselectedindexchanged="displayType_SelectedIndexChanged">
                   <asp:ListItem Selected="True">Numbers</asp:ListItem>
                   <asp:ListItem>Percent</asp:ListItem>
               </asp:DropDownList>
        </td>    
   </tr>
</table>


    <asp:Label ID="backDiv" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
    <asp:Panel ID="dialogPanel" Visible="false" runat="server" Width="80%" Height="480" CssClass="dialogPanelCSS">
    <a href="javascript:void();" class="close" OnClick="hideDialog()">X</a>
    <asp:Label ID="emptyMsg" runat="server" Visible="false" Text="<center><h2>No Data To Display</h2></center>"></asp:Label>
    <asp:Panel ID="innerDialogPanel" runat="server" ScrollBars="Auto" Width="100%" Height="470">
    
   <asp:GridView ID="performanceReportBarcodeDetailGridView" runat="server" AutoGenerateColumns="False" CssClass="TFtable" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle ForeColor="#333333" />
    
    <Columns>
       <asp:TemplateField HeaderText="TBM WC Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("tbmWCName") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Curing Machine Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="CuringMachineName" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# Eval("curingWCName") %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
    <Columns>
   <asp:TemplateField HeaderText="Mould Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("mouldName") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
     
    <Columns>
   <asp:TemplateField HeaderText="VI WC Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("visualWCName") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
       <Columns>
   <asp:TemplateField HeaderText="Size" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("size") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="barcode" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:HyperLink ID="hlDetails1" Text='<%# Eval("barcode") %>' CssClass="links" runat="server"  NavigateUrl='<%# "~/Report/tyreGeneaology.aspx?gtbarcode=" + Eval("barcode") %>' Target="_blank" /> 
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
      
     <PagerStyle BackColor="#507CD1" ForeColor="White" 
      HorizontalAlign="Center" />
     <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
     <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#999999" />
  </asp:GridView>
  </asp:Panel>
   </asp:Panel>
    
   <asp:Panel ID="VIReportRecipeWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">OK</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Rework</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">NCMR</td>
        </tr>
     </table>      
    <asp:GridView ID="VIRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="False" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
     
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15.5%" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="VISizeWiseTyreTypeIDLabel" runat="server" Text='<%# Eval("curingRecipeID") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>

        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="18.5%">
                <ItemTemplate>
                        <asp:Label ID="VISizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("curingRecipeName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
            
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="18.5%" Visible="false">
                                    <ItemTemplate>
                                            <asp:Label ID="VISizeWiseTyreTypeInnerRecipeCodeLabel" runat="server" Text='<%# Eval("VISizeWiseTyreTypeInnerRecipeCode") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("TotalChecked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                              <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseOkLabel" runat="server" Text='<%# Eval("TotalOK") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                 <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:LinkButton ID="VIRecipeWiseNotOk" CssClass="links" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="VIRecipeWiseNotOkLabel" runat="server" Text='<%# Eval("TotalRepair") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="VIRecipeWiseTotalMinorLink" CssClass="links" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("TotalNCMR") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                   <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                   <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                   <EditRowStyle BackColor="#999999" />
                   <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                    </div>
                </ItemTemplate>
                
      <FooterTemplate>
                  
      
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
   
   <table width="100%" style="background-color:#5D7B9D;">
            <tr>
                <td style="text-align:center; width:18%;" class="gridViewItems"></td>
                <td style="text-align:center; width:18%;" class="gridViewItems"><asp:Label ID="totalCheckedCountLabel" runat="server" Text=""></asp:Label></td>
                <td style="text-align:center; width:18%;" class="gridViewItems"><asp:Label ID="okcountLabel" runat="server" Text=""></asp:Label><%= percent_sign%></td>
                <td style="text-align:center; width:18%;" class="gridViewItems"><asp:Label ID="reworkcountLabel" runat="server" Text=""></asp:Label><%= percent_sign%></td>
                <td style="text-align:center; width:18%;" class="gridViewItems"><asp:Label ID="ncmrcountLabel" runat="server" Text=""></asp:Label><%= percent_sign%></td>
            </tr>
          </table>
     </asp:Panel> 
     
      <asp:Panel ID="SizeWiseRegionPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >  
    <table class="innerTable" cellspacing="1">
        <tr>       
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
                <asp:Label ID="faultType" runat="server" Text=""></asp:Label></td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tread</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">SideWall</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Bead</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Carcass</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Others</td>         
        </tr>
     </table>
         
      <asp:GridView ID="SizeWiseRegionGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />  
            <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="1%">
                <ItemTemplate>
                <asp:Label ID="VISizeFaultWiseTyreTypeIDLabel" runat="server" Text='<%# Eval("curingRecipeID") %>' CssClass="gridViewItems" Visible="false"></asp:Label>
                </ItemTemplate>             
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                <ItemTemplate>
                        <asp:Label ID="VISizeFaultWiseTyreTypeLabel" runat="server" Text='<%# Eval("curingRecipeName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%">
                <ItemTemplate>
                                   
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIRecipeFaultWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("TotalChecked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                              <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseOkLabel" runat="server" Text='<%# Eval("TotalRework") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                 <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseNotOkLabel" runat="server" Text='<%# Eval("TreadFault") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("SideWallFault") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                                           <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("Beadfault") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("Carcassfault") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                           
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalOthersLabel" runat="server" Text='<%# Eval("Othersfault") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                   <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                   <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                   <EditRowStyle BackColor="#999999" />
                   <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                <%
          if (getdisplaytype == "Percent")
          {
              if (totalcheckedcount != 0)
              {
                  totalrework = (totalrework * 100) / totalcheckedcount;
                  treadfault = (treadfault * 100) / totalcheckedcount;
                  sidewallfault = (sidewallfault * 100) / totalcheckedcount;
                  beadfault = (beadfault * 100) / totalcheckedcount;
                  carcassfault = (carcassfault * 100) / totalcheckedcount;
                  othersfault = (othersfault * 100) / totalcheckedcount;
              }
              else
              {
                  totalcheckedcount = 0;
                  totalrework = 0;
                  treadfault = 0;
                  sidewallfault = 0;
                  beadfault = 0;
                  carcassfault = 0;
                  othersfault = 0;
              }
          } %>
     <table width="100%">
        <tr>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= totalcheckedcount %></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= totalrework + percent_sign%></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= treadfault + percent_sign%></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= sidewallfault + percent_sign%></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= beadfault + percent_sign%></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= carcassfault + percent_sign%></td>
            <td style="text-align:center; width:14.28%;" class="gridViewItems"><%= othersfault + percent_sign%></td>
            
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

    </asp:Panel> 
     
      <asp:Panel ID="VIFaultWisePanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
            <asp:Label ID="Label11" CssClass="gridViewHeader" runat="server" Text="Label"></asp:Label></td>         
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">FaultName</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Count</td>
            
        </tr>
     </table>
     
      <div style="border: solid 1px #C3D9FF;">

      <asp:GridView ID="VIFaultWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="true" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />  
            <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="0%">
                <ItemTemplate>
                        <asp:Label ID="VIFaultWiseTyreTypeIDLabel" runat="server" Text='<%# Eval("curingRecipeID") %>' CssClass="gridViewItems" Visible="false"></asp:Label>
                </ItemTemplate>                
            </asp:TemplateField>
        </Columns>    
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                <ItemTemplate>
                        <asp:Label ID="VIFaultWiseTyreTypeLabel" runat="server" Text='<%# Eval("curingRecipeName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                  <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIFaultWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                     <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                                 <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="0%" Visible="false">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltAreaIDLabel" runat="server" Text='<%# Eval("defectAreaID") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          

                     
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="33%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltAreaNameLabel" runat="server" Text='<%# Eval("FaultAreaName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
              <Columns>
             <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
              <ItemTemplate>
                               
                    <%--Result child Gridview--%>   
                    <asp:GridView ID="VIFaultNameChildGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" onrowdatabound="GridView_RowDataBound"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                     <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltFaultNameLabel" runat="server" Text='<%# Eval("FaultName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltAreaCheckedLabel" runat="server" Text='<%# TyreQuantity(DataBinder.Eval(Container.DataItem,"FaultName"))%>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                 
                </ItemTemplate>
               </asp:TemplateField>
                 </Columns>

                            
                    <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                    </asp:GridView>
                    </div>
                  
                </ItemTemplate>
                
                <FooterTemplate>
       <table width="100%">
        <tr>
            <td style="text-align:center; width:33.3%;" class="gridViewItems"></td>
            <td style="text-align:center; width:33.3%;" class="gridViewItems"></td>
            <td style="text-align:center; width:33.3%;" class="gridViewItems"><%= tyreCount %></td>
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
    </div>
     </asp:Panel> 
     
        </ContentTemplate>
    </asp:UpdatePanel>
            <asp:Panel ID="ExcelPanel" runat="server">
                <asp:Label ID="ExcelLabel" runat="server" Text=""></asp:Label>
            </asp:Panel>
     <asp:GridView ID="ExcelGridView" runat="server" AutoGenerateColumns="False" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" ShowHeader="True" Visible="true" >
   
    
    <Columns>
       <asp:TemplateField HeaderText="S. No." HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("SNo") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Press Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# Eval("PressDate") %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
    <Columns>
   <asp:TemplateField HeaderText="Press Time" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("PressTime") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
     
    <Columns>
   <asp:TemplateField HeaderText="Shift" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Shift") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
       <Columns>
   <asp:TemplateField HeaderText="TyreSize" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("TyreSize") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Press No." HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("PressNo") %>' CssClass="gridViewItems"></asp:Label>
    
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
      <Columns>
       <asp:TemplateField HeaderText="Cavity" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Cavity") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Mould No" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# Eval("MouldNo") %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
    <Columns>
   <asp:TemplateField HeaderText="Side" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Side") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
     
    <Columns>
   <asp:TemplateField HeaderText="Building Date" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("BuildingDate") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
       <Columns>
   <asp:TemplateField HeaderText="Building Time" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("BuildingTime") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Builder Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("BuilderName") %>' CssClass="gridViewItems"></asp:Label>
    
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
        
        <Columns>
   <asp:TemplateField HeaderText="Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Barcode") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
       <Columns>
   <asp:TemplateField HeaderText="Defect Location" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("DefectLocation") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
       <asp:TemplateField HeaderText="Defect" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>   
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Defect") %>' CssClass="gridViewItems"></asp:Label>
    
       </ItemTemplate>
       </asp:TemplateField>
        </Columns>
      <Columns>
       <asp:TemplateField HeaderText="Disposal" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
       <ItemTemplate>
       <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Disposal") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="Remark" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="13%">
       <ItemTemplate>
     <asp:Label ID="performanceReportCuringWCNameLabel" runat="server" Text='<%# Eval("Remark") %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
    <Columns>
   <asp:TemplateField HeaderText="Responsibility" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("Responsibility") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>

  </asp:GridView>

</asp:Content>