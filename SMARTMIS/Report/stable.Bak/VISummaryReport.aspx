<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VISummaryReport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.VISummaryReport" Title="Smart MIS - VISummaryReport" %>


<asp:Content ID="VISummaryReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
<style>
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
    ::-moz-selection
{
  background: #FF0;
  color:#000000;
}

::-webkit-selection
{
  background: #FF0;
  color:#000000;
}

::selection
{
  background: #FF0;
  color:#000000;
}
.button:hover
{
    background-color: #15497C;
    background: -moz-linear-gradient(top, #15497C, #2384D3);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#15497C), to(#2384D3));
}
.button 
{
    font-style: normal;
    font-size: 15px;
    font-family: Calibri,"Trebuchet MS",Verdana,Geneva,Arial,Helvetica,sans-serif;
    color: #fff;
    background: linear-gradient(to bottom, #2384D3, #15497C);
    background-color: #2384D3;
    background: -moz-linear-gradient(top, #2384D3, #15497C);
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#2384D3), to(#15497C));
    padding: 0px 6px;
    border-width: 1px;
    border-style: solid;
    border-right: 1px solid #DDDDEB;
    border-left: 1px solid #DDDDEB;
    -moz-border-top-colors: none;
    -moz-border-right-colors: none;
    -moz-border-bottom-colors: none;
    -moz-border-left-colors: none;
    border-image: none;
    border-color: #FFF #DDDDEB #B3B3BD;
    border-radius: 7px;
    text-align: center;
    box-shadow: 0px 1px 4px 0px #C8C8D2;
    outline: medium none;
    line-height: 21px;
    display: inline-block;
    cursor: pointer;
    box-sizing: border-box;
    height: 28px;
}

#bookG{
width:39px}

.blockG{
background-color:#949494;
border:1px solid #000000;
float:left;
height:28px;
margin-left:2px;
width:7px;
opacity:0.1;
-moz-animation-name:bounceG;
-moz-animation-duration:1s;
-moz-animation-iteration-count:infinite;
-moz-animation-direction:linear;
-moz-transform:scale(0.7);
-webkit-animation-name:bounceG;
-webkit-animation-duration:1s;
-webkit-animation-iteration-count:infinite;
-webkit-animation-direction:linear;
-webkit-transform:scale(0.7);
-ms-animation-name:bounceG;
-ms-animation-duration:1s;
-ms-animation-iteration-count:infinite;
-ms-animation-direction:linear;
-ms-transform:scale(0.7);
-o-animation-name:bounceG;
-o-animation-duration:1s;
-o-animation-iteration-count:infinite;
-o-animation-direction:linear;
-o-transform:scale(0.7);
animation-name:bounceG;
animation-duration:1s;
animation-iteration-count:infinite;
animation-direction:linear;
transform:scale(0.7);
}

#blockG_1{
-moz-animation-delay:0.3s;
-webkit-animation-delay:0.3s;
-ms-animation-delay:0.3s;
-o-animation-delay:0.3s;
animation-delay:0.3s;
}

#blockG_2{
-moz-animation-delay:0.4s;
-webkit-animation-delay:0.4s;
-ms-animation-delay:0.4s;
-o-animation-delay:0.4s;
animation-delay:0.4s;
}

#blockG_3{
-moz-animation-delay:0.5s;
-webkit-animation-delay:0.5s;
-ms-animation-delay:0.5s;
-o-animation-delay:0.5s;
animation-delay:0.5s;
}

@-moz-keyframes bounceG{
0%{
-moz-transform:scale(1.2);
opacity:1}

100%{
-moz-transform:scale(0.7);
opacity:0.1}

}

@-webkit-keyframes bounceG{
0%{
-webkit-transform:scale(1.2);
opacity:1}

100%{
-webkit-transform:scale(0.7);
opacity:0.1}

}

@-ms-keyframes bounceG{
0%{
-ms-transform:scale(1.2);
opacity:1}

100%{
-ms-transform:scale(0.7);
opacity:0.1}

}

@-o-keyframes bounceG{
0%{
-o-transform:scale(1.2);
opacity:1}

100%{
-o-transform:scale(0.7);
opacity:0.1}

}

@keyframes bounceG{
0%{
transform:scale(1.2);
opacity:1}

100%{
transform:scale(0.7);
opacity:0.1}

}
</style>
<script>
    function hideDialog() {
        document.getElementById('ctl00_masterContentPlaceHolder_dialogPanel').style.display = 'none';
        document.getElementById('ctl00_masterContentPlaceHolder_backDiv').style.display = 'none';
    }
</script>
    <%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />
<center><h2>First PCR Visual Inspection Report</h2></center>
<asp:ScriptManager ID= "TUOReprt1ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
<div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="PopupUpdatePanel">
        <ProgressTemplate>
        <div align="center" style="position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;">
             
             <div style="width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#1B1B1B;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;">
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
 <asp:UpdatePanel ID="PopupUpdatePanel" runat="server">
    <ContentTemplate>
    
   <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>   
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:4%">&nbsp;&nbsp;From::</td>
        
        <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" onclick="ViewButton_Click" CssClass="button" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:8%; visibility:hidden"> select category &nbsp; ::</td>
   
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%; visibility:hidden">
           
               <asp:DropDownList ID="FaultTypeDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>MAJOR</asp:ListItem>
                   <asp:ListItem>MINOR</asp:ListItem>
                   <asp:ListItem>REPAIR</asp:ListItem>
                   <asp:ListItem>BUFF</asp:ListItem>
               </asp:DropDownList>
              
          </td>
              
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%; visibility:hidden">  &nbsp;&nbsp;&nbsp; select FaultArea &nbsp; ::</td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%; visibility:hidden">
           
               <asp:DropDownList ID="FaultAreaDropDownList" Width="100%" runat="server" AutoPostBack="true" onselectedindexchanged="FaultTypeDropDownList_SelectedIndexChanged">
                   <asp:ListItem>Select</asp:ListItem>
                   <asp:ListItem>All</asp:ListItem>
                   <asp:ListItem>Tread</asp:ListItem>
                   <asp:ListItem>SideWall</asp:ListItem>
                   <asp:ListItem>Bead</asp:ListItem>
                   <asp:ListItem>Carcass</asp:ListItem>
               </asp:DropDownList>
          </td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
               &nbsp;</td>
        
   </tr>
</table>

<asp:Label ID="backDiv" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
    <asp:Panel ID="dialogPanel" Visible="false" runat="server" Width="80%" Height="480" CssClass="dialogPanelCSS">
    <a href="javascript:void();" class="close" OnClick="hideDialog()">X</a>
    <asp:Label ID="emptyMsg" runat="server" Visible="false" Text="<center><h2>No Data To Display</h2></center>"></asp:Label>
    <asp:Panel ID="innerDialogPanel" runat="server" ScrollBars="Auto" Width="100%" Height="470">
    
   <asp:GridView ID="performanceReportBarcodeDetailGridView" runat="server" AutoGenerateColumns="False" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
    
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
   </asp:Panel>

   <asp:Panel ID="VIReportRecipeWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
           <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">OK</td>

            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Not OK</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Mazor Fault</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Minor Fault</td>
        </tr>
     </table>
     
     
      <asp:GridView ID="VIRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" onrowdatabound="GridView_RowDataBound"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" ShowFooter="False" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%">
                <ItemTemplate>
                        <asp:Label ID="VISizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("curingRecipeName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="84%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%" Visible="false">
                                    <ItemTemplate>
                                            <asp:Label ID="VIinnerCuringRecipeNameLabel" runat="server" Text='<%# Eval("innerCuringRecipeName") %>' CssClass="gridViewItems"></asp:Label>
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
                                            <asp:Label ID="VIRecipeWiseOkLabel" runat="server" Text='<%# Eval("TotalOK") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                 <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseNotOkLabel" runat="server" Text='<%# Eval("TotalNotOK") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:LinkButton ID="VIRecipeWiseTotalMinor" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("TotalMazor") %>' CssClass="gridViewItems"></asp:Label></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:LinkButton ID="VIRecipeWiseTotalMazor" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("TotalMinor") %>' CssClass="gridViewItems"></asp:Label></asp:LinkButton>
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
                <td style="text-align:center; width:15.5%;" class="gridViewItems"></td>
                <td style="text-align:center; width:15.5%;" class="gridViewItems"><asp:Label ID="totalCheckedCountLabel" runat="server" Text=""></asp:Label></td>
                <td style="text-align:center; width:16%;" class="gridViewItems"><asp:Label ID="okcountLabel" runat="server" Text=""></asp:Label></td>
                <td style="text-align:center; width:15.5%;" class="gridViewItems"><asp:Label ID="notokcountLabel" runat="server" Text=""></asp:Label></td>
                <td style="text-align:center; width:15.5%;" class="gridViewItems"><asp:LinkButton ID="VIRecipeWiseTotalMinorTotal" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="majorcountLabel" runat="server" Text=""></asp:Label></asp:LinkButton></td>
                <td style="text-align:center; width:16%;" class="gridViewItems"><asp:LinkButton ID="VIRecipeWiseTotalMajorTotal" OnClick="VIRecipeWiseTotalMinor_Click" runat="server"><asp:Label ID="minorcountLabel" runat="server" Text=""></asp:Label></asp:LinkButton></td>
            </tr>
          </table>
     </asp:Panel> 
   
   <asp:Panel ID="SizeWiseRegionPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="true" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
                <asp:Label ID="FaultTypeLabel" CssClass="gridViewHeader" runat="server" Text="Label"></asp:Label></td>
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
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="13%">
                <ItemTemplate>
                        <asp:Label ID="VISizeFaultWiseTyreTypeLabel" runat="server" Text='<%# Eval("curingRecipeName") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIRecipeFaultWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both" onrowdatabound="GridView_RowDataBound"
                        AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="11%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseCheckedLabel" runat="server" Text='<%# Eval("TotalChecked") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                              <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseOkLabel" runat="server" Text='<%# Eval("TotalRework") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                 <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseNotOkLabel" runat="server" Text='<%# Eval("TreadFault") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("SideWallFault") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                                           <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("Beadfault") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("Carcassfault") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMazorLabel" runat="server" Text='<%# Eval("Othersfault") %>' CssClass="gridViewItems"></asp:Label>
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
         <%--       <table width="100%"><tr>
                    <td style="width:14.28%;" align="center"><%=totalcheckedcount %></td>
                    <td style="width:14.28%;" align="center"><%=reworkcount %></td>
                    <td style="width:14.28%;" align="center"><%=treadcount %></td>
                    <td style="width:14.28%;" align="center"><%=sidewallcount %></td>
                    <td style="width:14.28%;" align="center"><%=beadcount %></td>
                    <td style="width:14.28%;" align="center"><%=carcasscount %></td>
                    <td style="width:14.28%;" align="center"><%=otherscount %></td>
                            </tr></table>--%>
            
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
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="33%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltAreaCheckedLabel" runat="server" Text='<%# Eval("FaultAreaName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            
                                    <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="75%">
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
   
                    <asp:GridView ID="VIFaultNameChildGridView" runat="server" AutoGenerateColumns="False" 
                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" onrowdatabound="GridView_RowDataBound"
                    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                     <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIfaltAreaCheckedLabel" runat="server" Text='<%# Eval("FaultName") %>' CssClass="gridViewItems"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                               <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
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
                <table width="100%"><tr>
                    <td style="width:33.3%;"></td>
                    <td style="width:33.3%;"></td>
                    <td style="width:33.3%;" align="center"><%=tyrecount%></td>
                </tr></table>
            
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
</asp:Content>