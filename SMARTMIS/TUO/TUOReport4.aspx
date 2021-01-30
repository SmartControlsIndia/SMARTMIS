<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TUOReport4.aspx.cs" MasterPageFile="~/smartMISTUOReportMaster.Master" Inherits="SmartMIS.TUO.TUOReport4" Title="Performance Report Spec Wise" %>

<asp:Content ID="performanceReportSpecWiseContent" runat="server" ContentPlaceHolderID="tuoReportMasterContentPalceHolder"> 

<style>
tr.hoverRow:hover
{
  background-color:blue;
  color:white;
}
</style>

    <link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
<%@ Register src="../UserControl/reportHeader.ascx" TagName="reportHeader" tagprefix="asp" %>

<asp:HiddenField id="tuoFilterPerformanceReportTUOWiseSizeDropdownlistHidden" runat="server" value="Select" />
<asp:HiddenField id="tuoFilterPerformanceReportTUOWiseRecipeDropdownlistHidden" runat="server" value="Select" />
    
<asp:HiddenField id="magicHidden" runat="server" value="" />
<asp:HiddenField id="tyreTypeHidden" runat="server" value="" />
<asp:HiddenField id="viewQueryHidden" runat="server" value="" />

 <script type="text/javascript" language="javascript">

     function showReport(queryString) {
         document.getElementById('<%= this.magicHidden.ClientID %>').value = queryString.toString();
         document.getElementById('<%= this.magicButton.ClientID %>').click();
     }
    </script>
<asp:Button ID="magicButton" runat="server" Text="Magic" onclick="magicButton_Click" CssClass="masterHiddenButton"></asp:Button>

<asp:Label ID="excelquery" runat="server" Visible="false"></asp:Label><asp:Label ID="excelFromDate" runat="server" Visible="false"></asp:Label><asp:Label ID="excelToDate" runat="server" Visible="false"></asp:Label>
<asp:reportHeader ID="reportHeader" runat="server" />

 <table cellpadding="0" cellspacing="0" style="width: 80%; border-style:solid; border-width:thin; border-color: #507CD1; background-color: #FFFFFF;" align="center">
    <tr>
         <td style="width: 10%"></td>
         <td style="width: 10%"></td>

        <td style="width: 12%"></td>
        <td style="width: 6%"></td>
        <td style="width: 12%"></td>
        <td style="width: 10%"></td>
        <td style="width: 10%"></td>

    </tr>
    <tr>
       <td class="masterLabel">
            &nbsp;</td>
        <td class="masterLabel">      
                        Parameter
        </td>
        <td class="masterLabel">
            <asp:DropDownList ID="tuoFilterSpecParameterDropDownList" runat="server" 
                CssClass="masterDropDownList" style="width: 90%" AutoPostBack="true"
                onselectedindexchanged="DropDownList_SelectedIndexChanged">
                <asp:ListItem>RFV</asp:ListItem>
                <asp:ListItem>R1H</asp:ListItem>
                <asp:ListItem>LFV</asp:ListItem>
                <asp:ListItem>CON</asp:ListItem>
                <asp:ListItem>LRO</asp:ListItem>
                <asp:ListItem>CRO</asp:ListItem>
                <asp:ListItem>BLG</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="masterLabel">
            Spec
        </td>
        
        <td class="masterTextBox">
            <asp:TextBox ID="SpecTextBox" Width="90%"  CssClass="masterTextBox" runat="server"></asp:TextBox>
        </td>
        <td class="masterTextBox">
            &nbsp;</td>
        <td style="width: 10%">
            &nbsp;</td>
    </tr>
</table>

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
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseSizeDropdownlist"  Width="60%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true" Visible="true"> </asp:DropDownList>
        </td>       
          <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Design:</td>        
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterPerformanceReportTUOWiseRecipeDropdownlist"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true" Visible="true">
             </asp:DropDownList>
        </td>        
         <td style="font-weight:bold; font-family:Arial; font-size:small; width:3%">Type:</td>       
           <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
           
             <asp:DropDownList ID="tuoFilterOptionDropDownList"  Width="80%" runat="server" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CausesValidation="false"  AutoPostBack="true">
            <asp:ListItem  Value="0" Text="No"></asp:ListItem>
            <asp:ListItem  Value="1" Text="Percent"></asp:ListItem>
             </asp:DropDownList>
        </td>
        <td style="font-weight:bold; font-family:Arial; font-size:small; width:7%">
             <asp:Button ID="expToExcel" style="background-image:url('../Images/Excel.jpg'); background-color:red; cursor:hand;" runat="server" onclick="expToExcel_Click" width="30" Height="30" />
        </td>
        
       
       
    </tr>
</table>
<asp:ScriptManager ID="TuoRejectionDetailScriptManager" AsyncPostBackTimeout="360000" runat="server"></asp:ScriptManager>
 
<asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
    <ContentTemplate>
<asp:Label ID="showDownload" runat="server" Text=""></asp:Label>
<asp:Panel ID="gvpanel" runat="server">
    <asp:GridView Width="100%" CssClass="TBMTable" ID="MainGridView" OnDataBound = "OnDataBound" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound">
            <Columns>
            </Columns>
    </asp:GridView>
</asp:Panel>
</ContentTemplate>
<Triggers>
        <asp:PostBackTrigger ControlID="expToExcel" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>