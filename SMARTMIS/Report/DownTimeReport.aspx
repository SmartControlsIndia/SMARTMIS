<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownTimeReport.aspx.cs" MasterPageFile="~/smartMISDownTimeReportMaster.Master" Inherits="SmartMIS.Report.DownTimeReport" %>
<%@ MasterType VirtualPath="~/smartMISDownTimeReportMaster.Master"%>

<asp:Content ID="DowntimeReportContent" runat="server" ContentPlaceHolderID="DownTimeMasterContentPlaceHolder">
<link href="../Style/reportMaster.css" rel="stylesheet" type="text/css" />
<link href="../Style/master.css" rel="stylesheet" type="text/css" />
<link href="../Style/masterPage.css" rel="stylesheet" type="text/css" />

<style>
.ErrorMsgCSS
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
<script type="text/javascript">
    function closePopup() {
        setTimeout(function() {
            $('.ErrorMsgCSS').fadeOut(1500);
        }, 4000);

    }
    setTimeout(function() {
        $('.ErrorMsgCSS').fadeOut(1500);
    }, 4000);
</script>

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
    .close
     {
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
<script type="text/javascript">
    function hideDialog() {
        $('#ctl00_ctl00_masterContentPlaceHolder_DownTimeMasterContentPlaceHolder_dialogPanel').fadeOut(1500);
        $('#ctl00_ctl00_masterContentPlaceHolder_DownTimeMasterContentPlaceHolder_backDiv').fadeOut(1500);
    }
    setTimeout(function() {
        $('.alrtPopup').fadeOut(1500);
    }, 4000);

    //document.documentElement.style.height = "100%";
    //document.body.style.height = "100%";
</script>






    <link rel="stylesheet" href="../Style/productionReport.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
<asp:Label ID="ErrorMsg" runat="server" Text="" CssClass="ErrorMsgCSS" Visible="false"></asp:Label>
    
   <asp:Label ID="HeaderText" runat="server" Text=""></asp:Label>




<asp:UpdatePanel ID="weighmentUpdatepanel" runat="server">
<ContentTemplate>

 <asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" runat="server" AutoGenerateColumns="false"  >
      <Columns>
                <asp:TemplateField HeaderText="WCName" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="DownTimeWCnamelabel" runat="server" Text='<%# Eval("wcName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
         <Columns>
                                <asp:TemplateField HeaderText="TotalDownTime in Minuts " ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="DownTimeTDlabel" runat="server" Text='<%# Eval("TotalDownTime") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                        
        <Columns>
                                <asp:TemplateField HeaderText="Show detail" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:LinkButton ID="DownTimeDetaillabel" Text="View Detail" OnClick="DownTimeDetaillabel_Click" runat="server" Visible="true"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
    </asp:GridView>
    </asp:Panel>
    

    <center>
    <asp:Chart ID="TBMChart" runat="server" Width="1100px" EnableViewState="true">
        <Series>
            <asp:Series Name="TBMSeries">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="TBMChartArea">
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>

 <asp:Label ID="backDiv" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
 
<asp:Label ID="Label1" runat="server" CssClass="modalBackground" Visible="false" Text=""></asp:Label>
<asp:Panel ID="dialogPanel" Visible="false" runat="server" Width="80%" Height="480" CssClass="dialogPanelCSS">
    <a href="javascript:void();" class="close" onclick="hideDialog()">X</a>
    <asp:Label ID="emptyMsg" runat="server" Visible="false" Text="<center><h2>No Data To Display</h2></center>"></asp:Label>
    <asp:Panel ID="innerDialogPanel" runat="server" ScrollBars="Auto" Width="100%" Height="470">
    
    <asp:GridView ID="DownTimeReportDetailGridView" runat="server" AutoGenerateColumns="False" CssClass="TFtable" 
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both" AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="True" >
   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
   <RowStyle ForeColor="#333333" />
    
    <Columns>
       <asp:TemplateField HeaderText="WCName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="wcName"  ItemStyle-Width="8%">
       <ItemTemplate>
       <asp:Label ID="DownTImeReportWCNameLabel" runat="server" Text='<%# Eval("WCName") %>' CssClass="gridViewItems"></asp:Label>
       </ItemTemplate>
       </asp:TemplateField>
       </Columns>
          <Columns>
       <asp:TemplateField HeaderText="DownTimeStart" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="CuringMachineName" ItemStyle-Width="8%">
       <ItemTemplate>
     <asp:Label ID="DownTimeReportStartDownTimeLabel" runat="server" Text='<%# Eval("startTime") %>' CssClass="gridViewItems"></asp:Label>
     </ItemTemplate>
      </asp:TemplateField>
      </Columns>
    <Columns>
   <asp:TemplateField HeaderText="DownTimeEnd" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="12%">
   <ItemTemplate>
   <asp:Label ID="DownTimeReportStopDownTimeLabel" runat="server" Text='<%# Eval("stopTime") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
     </asp:TemplateField>
       </Columns>
    <Columns>
   <asp:TemplateField HeaderText="Downtime In Minuts" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="18%">
   <ItemTemplate>
   <asp:Label ID="performanceReportTBMWCNameLabel" runat="server" Text='<%# Eval("totalDownTime") %>' CssClass="gridViewItems"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>

    <Columns>
   <asp:TemplateField HeaderText="Shift" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="left" SortExpression="tiretype"  ItemStyle-Width="8%">
   <ItemTemplate>
   <asp:Label ID="DownTimeShiftLabel" runat="server" Text='<%# Eval("shift") %>' CssClass="gridViewItems"></asp:Label>
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

</ContentTemplate>


</asp:UpdatePanel>
    

    
    
    
</asp:Content>
