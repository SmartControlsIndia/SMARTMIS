<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xrayreport.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.xrayreport" Title="Smart MIS - TBRXrayReport" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>


<asp:Content ID="SmartfinalfinishContent" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
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
</style>

    <table cellpadding="0" cellspacing="0" style="width: 100%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
        <tr >
            <td class="gridViewItems" style="width: 18%; margin-left: 40px;">
                From :
                <mycontrol:calendertextbox ID="reportMasterFromDate" runat="server" 
                    Disabled="false" Width="50%" />
            </td>
            <td class="gridViewItems" style="width: 18%; margin-left: 40px;">
                To :
                &nbsp;<myControl:calenderTextBox ID="reportMasterToDate" runat="server" 
                    Disabled="false" Width="50%" Visible="True" />
                </td>
                      <td class="gridViewItems" style="width: 18%; margin-left: 40px;">
             X-ray<asp:DropDownList ID="dpdxray" runat="server">
             
                          </asp:DropDownList>
                &nbsp; 
                </td>
            <td class="gridViewItems" style="width: 20%; margin-left: 40px;">
                <asp:Button ID="Button2" runat="server"  Text="  View  " CssClass="button" onclick="Button2_Click" 
                    onclientclick="if(!ctl00_masterContentPlaceHolder_reportMasterFromDate_calenderUserControlTextBox.value || !ctl00_masterContentPlaceHolder_reportMasterToDate_calenderUserControlTextBox.value){alert(&quot;Enter Date&quot;);return false;}" />
                <asp:Button ID="export" runat="server" OnClick="Export_click" Text="Export" />
            </td>
            <td>
                <asp:Label ID="showcount" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="Small"></asp:Label>
            </td>
        </tr>
    </table>

    <div style="text-align:center; font-size:large; font-style:normal; font-weight:bold; background-color:Gray"> TBR Xray Report</div>
    <table style="width: 100%">
        <tr class="FinalFinishgridViewHeader">
        <td align="center" width="10%">
                Wcname</td>
            <td align="center" width="10%">
                Barcode</td>
            <td width="40%">
                Name</td>
            <td>
                Date &amp; Time</td>
            <td>
                Shift</td>
        </tr>
    </table>
    
    <asp:Panel ID="Panel1" runat="server" Height="399px" ScrollBars="Auto">
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
        GridLines="None" onrowdatabound="GridView1_RowDataBound" Width="100%" 
            Height="90px" ShowHeader="False">
        <RowStyle BackColor="#EFF3FB"  HorizontalAlign="Center" 
            VerticalAlign="Middle" Font-Bold="True" Font-Names="Arial" 
            Font-Size="Small" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
            CssClass="FinalFinishgridViewHeader" />
        <EditRowStyle BackColor="#2461BF" Font-Bold="True" Font-Names="Arial" 
            Font-Size="Small" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    </asp:Panel>
 
    

   
</asp:Content>