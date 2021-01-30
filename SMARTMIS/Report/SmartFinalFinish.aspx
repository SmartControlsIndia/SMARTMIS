<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartFinalFinish.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Report.SmartFinalFinish" %>

<%@ Register src="~/UserControl/calenderTextBox.ascx" tagname="calenderTextBox" tagprefix="myControl" %>


<asp:Content ID="SmartfinalfinishContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder" >
  
        <table style=" height: 73px; margin-right: 136px; font-family: Arial; font-size: small;" cellpadding="2" cellspacing="1" width="1000">
            <tr>
                <td width="100">
                    <asp:Label ID="Label1" runat="server" Text="Type :" Font-Bold="True" 
                        Font-Names="Arial" Font-Size="Small"></asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem>TBR</asp:ListItem>
                        <asp:ListItem>PCR</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style3" width="190">
                    <asp:Label ID="Label2" runat="server" Text="From :" Font-Bold="True" 
                        Font-Names="Arial" Font-Size="Small"></asp:Label>
                    
            <myControl:calenderTextBox ID="reportMasterFromDateTextBox" runat="server" Disabled="false" Width="60%" />     
       </td>
                <td width="170">
                    <asp:Label ID="Label4" runat="server" Text="To :" Font-Bold="True" 
                        Font-Names="Arial" Font-Size="Small"></asp:Label>
            <myControl:calenderTextBox ID="reportMasterToDateTextBox" runat="server" Disabled="false" Width="60%" />     

                </td>
                <td width="150">
                    <asp:Label ID="Label3" runat="server" Text="Status : " Font-Bold="True" 
                        Font-Names="Arial" Font-Size="Small"></asp:Label>
                     <asp:DropDownList ID="DropDownList2" runat="server" Height="20px">
                        <asp:ListItem>OK</asp:ListItem>
                        <asp:ListItem>Not Ok</asp:ListItem>
                        <asp:ListItem Selected="True">Show All</asp:ListItem>
                    </asp:DropDownList>
&nbsp;</td>
                <td width="80" >
                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text=" Submit " 
                        Width="60px" 
                        
                        
                        onclientclick="if(!ctl00_masterContentPlaceHolder_reportMasterFromDateTextBox_calenderUserControlTextBox.value || !ctl00_masterContentPlaceHolder_reportMasterToDateTextBox_calenderUserControlTextBox.value){alert(&quot;Enter Date&quot;); return false;}" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="Small" />
                </td>
                <td align="center">
                
                    <asp:Label ID="SummaryDataView" runat="server" ></asp:Label>
                
                </td>
            </tr>
            <tr>
               
                  <td align="center" colspan="6"><h2>Final Finish Report </h2></td>
            </tr>
        </table>
       <asp:Label ID="messageLabel" runat="server"></asp:Label>
        
        <table width="100%">
            <tr>
                <td width="30%" class="FinalFinishgridViewHeader">Work Center Name</td>
                <td width="15%" class="FinalFinishgridViewHeader">Tyre Count</td>
                <td width="10%" class="FinalFinishgridViewHeader">Lot Number</td>
                <td width="20%" class="FinalFinishgridViewHeader">Status</td>
                <td width="30%" class="FinalFinishgridViewHeader">Date & Time</td>
           </tr>
        </table>
        <asp:Panel ID="FinalFinishPanel"  runat="server" ScrollBars="Auto" Height="300"> 

        <asp:Label ID="DataViewLabel" runat="server" Font-Bold="True" Font-Names="Arial"></asp:Label></asp:Panel>
        <br />
    
  
</asp:Content>