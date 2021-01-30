<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="classificationReport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.classificationReport" Title="Smart MIS - TBR Visual Inspection Classifiation Report" %>

<asp:Content ID="TBRVISummaryReportContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
<script type="text/javascript" language="javascript" src="../Script/download.js"></script>
<style>
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
	border-radius: 12px;
  .box-shadow;
  .transition;
  .transition-delay(.2s);
  &:hover { background: #00d9ff; .transition; }
  }
</style>
<%@ Register src= "~/UserControl/calenderTextBox.ascx" TagName="calendertextbox" tagprefix="myControl" %>
<%@ Register src="~/UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />

<center><h2>TBR Visual Inspection Classifiation Report</h2></center>
<asp:ScriptManager ID="TBRVI2ScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
 <div style="text-align:center">
    <asp:UpdateProgress ID="upProgClaimantSearch" runat="server" AssociatedUpdatePanelID="curingOperatorPlanningUpdatePanel">
        <ProgressTemplate>
        <div style="position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;">
             
             <div style="width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#1B1B1B;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;">
             <img src="../Images/loading.gif"/>

             <h2><font color="#888888">Loading, please wait............</font> </h2> 
             </div>
             </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
 <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
       
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:4%">&nbsp;&nbsp;Date::</td>
        <td style="width: 8%">
        <myControl:calenderTextBox ID="tuoReportMasterFromDateTextBox" runat="server" Width="75%" /></td>
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:2%">&nbsp;&nbsp;To::</td>
         <td style="width: 8%">
         <myControl:calenderTextBox ID="tuoReportMasterToDateTextBox" runat="server" Width="75%" />
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">  &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ViewButton" runat="server" Text="View Report" CssClass="button" onclick="ViewButton_Click" />

            &nbsp;</td>
             
      <td style="font-weight:bold; font-family:Arial; font-size:small; width:12%"> 
          Display Type: 
          <asp:DropDownList ID="displayType" runat="server" AutoPostBack="True" 
              onselectedindexchanged="displayType_SelectedIndexChanged" Width="50%">
              <asp:ListItem Selected="True">Number</asp:ListItem>
              <asp:ListItem>Percent</asp:ListItem>
          </asp:DropDownList>
        </td>

        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:10%">  </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
               <asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag" /></asp:LinkButton>
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
 </td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">          
               &nbsp;</td>    
   </tr>
</table>

   <asp:Panel ID="VIReportRecipeWiseMainPanel" runat="server" ScrollBars="None" Height="100%" CssClass="panel" Visible="false" >
    
    <table class="innerTable" cellspacing="1">
        <tr>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Tyre Type</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Total Checked</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">OK</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">Rework</td>
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
                Downgrade</td>
                 <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
                waiting for disposel</td>
          
            <td class="gridViewHeader" style="width:12%; text-align:center; padding:5px" rowspan="2">
                Scrap</td>
        </tr>
     </table>      
    <asp:GridView ID="VIRecipeWiseGridView" runat="server" AutoGenerateColumns="False"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
           ShowFooter="true" onrowdatabound="VIRecipeWiseGridView_RowDataBound" 
           style="margin-bottom: 0px" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
     
            <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false">
                <ItemTemplate>
                        <asp:Label ID="VISizeWiseTyreTypeIDLabel" runat="server" Text='<%# Eval("ID") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>

        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                <ItemTemplate>
                        <asp:Label ID="VISizeWiseTyreTypeLabel" runat="server" Text='<%# Eval("description") %>' CssClass="gridViewItems"></asp:Label>
                </ItemTemplate>
                
            </asp:TemplateField>
        </Columns>
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="88%">
            
                <ItemTemplate>
                    
                    <%--Result child Gridview--%>
                        
                    <div style="border: solid 1px #C3D9FF;">
                    <asp:GridView ID="VIRecipeWiseChildGridView" runat="server" AutoGenerateColumns="False" 
                        Width="100%" CellPadding="3" ForeColor="#333333" GridLines="both"
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
                                            <asp:Label ID="VIRecipeWiseNotOkLabel" runat="server" Text='<%# Eval("TotalOK") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("Rework") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("Downgrade") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("WaitforDisposal") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <Columns>
                                <asp:TemplateField HeaderText="Checked" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%">
                                    <ItemTemplate>
                                            <asp:Label ID="VIRecipeWiseTotalMinorLabel" runat="server" Text='<%# Eval("ScrapeReport") %>' CssClass="gridViewItems"></asp:Label><%= percent_sign %>
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
                  okcount = (okcount * 100) / totalcheckedcount;
                  reworkcount = (reworkcount * 100) / totalcheckedcount;
                  downgradecount = (downgradecount * 100) / totalcheckedcount;
                  waitingfordisposal = (waitingfordisposal * 100) / totalcheckedcount;
                  scrapereport = (scrapereport * 100) / totalcheckedcount;
              }
          } %>
      <table width="100%">
        <tr>
            <td style="text-align:center; width:12%" class="gridViewItems">Total</td>
            <td style="text-align:center; width:14.6%" class="gridViewItems"><%= totalcheckedcount %></td>
            <td style="text-align:center; width:14.6%" class="gridViewItems"><%= okcount + percent_sign%></td>
            <td style="text-align:center; width:14.6%" class="gridViewItems"><%= reworkcount + percent_sign%></td>
            <td style="text-align:center; width:14.6%" class="gridViewItems"><%= downgradecount + percent_sign%></td>
         <td style="text-align:center; width:14.6%" class="gridViewItems"><%= waitingfordisposal + percent_sign%></td>
           <td style="text-align:center; width:14.6%" class="gridViewItems"><%= scrapereport + percent_sign%></td>
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
</ContentTemplate>
</asp:UpdatePanel>
    
</asp:Content>