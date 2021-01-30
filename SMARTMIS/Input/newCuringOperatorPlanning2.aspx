<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newCuringOperatorPlanning2.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Input.newCuringOperatorPlanning2" %>

<asp:Content ID="newcuringOperatorPlanningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

<style>
.listBoxCss
{
    background:none;
}
.listBoxCss:hover
{
    background:none;
}
.link
{
    text-decoration: none;    
    color: #1B1B1B;
}
.link:hover
{
    text-decoration:underline;
}
.dialogCSS
{
    width:60%;
    max-width:60%;
    height:450px;
    position:fixed;
    z-index: 2050;
    top:45px;
    left: 280px;
    -moz-border-radius: 10px;
    -webkit-border-radius: 10px;
    border-radius: 5px;
    border:5px solid #888888;
    box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    
    background: #1B1B1B;
	background: -moz-linear-gradient(top, #B2FCFF, #DAE7ED);
	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#B2FCFF), to(#DAE7ED));
}
.blurback
    {
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        z-index: 1040;
        background: rgba(0, 0, 0, 0.7);
        margin: 0px;padding: 0px;display: block;cursor: auto;
    }
.alrtPopup
{
    color :#B5B3B5; padding:7px;width:35%;max-width:35%;height:auto;
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;
    position:fixed;z-index: 1080;top:75px;left: 450px;
    -moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;
    border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );
    background: #1B1B1B;
	background: -moz-linear-gradient(top, #1B1B1B, #0033CC);
	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#1B1B1B), to(#0033CC));
	z-index:2060;
}
.popupBut
{
    background: rgba(255, 255, 255, 1);background-color:#1B1B1B;color:#5B5B5B;
}

.dropdown
{
    background: #FF9933;
    background-color:#FF9933;
    color:#000000;
    -moz-border-radius: 3px;-webkit-border-radius: 3px;border-radius: 3px;
}
.dropdown:hover
{
    background: #FF9933;
    background-color:#FF9933;
    color:#000000;
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
.editbutton {
  padding:2px;
  background-color: #FF9933;
  border: 1px solid #666;
  color:#000;
  text-decoration:none;
}
 .close {
	background: #606061;
	color: #FFFFFF;
	line-height: 25px;
	position: absolute;
	right: -10px;
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
<script type="text/javascript">
    function closebox() {
        document.getElementById('ctl00_masterContentPlaceHolder_showDialogPanel').style.display = "none";
        document.getElementById('ctl00_masterContentPlaceHolder_backDiv').style.display = "none";
    }
    function closePopup() {
        document.getElementById('ctl00_masterContentPlaceHolder_alertUser').style.display = "none";
    }
</script>
    <link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
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
        <div id="dialog"></div>
        
       <asp:Panel ID="BackPanel" runat="server" CssClass="blurback" Visible="false"></asp:Panel> 
   <asp:Panel ID="UpdateGroupPanel" runat="server" Visible="false" CssClass="dialogCSS">
       <asp:LinkButton ID="CloseLinkButton" runat="server" CssClass="close" 
           onclick="CloseLinkButton_Click">X</asp:LinkButton>
       <center>
            <h3>Configure Groups</h3>
            <hr />
            <table width="80%" border=1 style="border-collapse:collapse;">
                <tr>
                    <td align="center"><strong>WorkCenterGroup</strong></td>
                    <td align="center"><strong>WorkCenters</strong></td>
                </tr>
                <tr>
                    <td valign="middle" align="center" style="width:50%;max-width:50%;">
                        
                        <asp:ListBox ID="GroupListBox" runat="server" Height="250" Width="150" 
                            CssClass="listBoxCss" AutoPostBack="True" 
                            onselectedindexchanged="GroupListBox_SelectedIndexChanged"></asp:ListBox>
                            
                            <br />
                            
                        <asp:Label ID="GroupLabel" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="center" style="width:50%;max-width:50%;">
                        
                        <asp:ListBox ID="WCListBox" runat="server" Height="300" Width="100" CssClass="listBoxCss" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="save_group" runat="server" Text="Save" CssClass="button" 
                            onclick="save_group_Click" /></td>
                </tr>
            </table>
            
       </center>
   </asp:Panel>
   
   <div style="text-align:left; background-color:ActiveBorder; font-family:Arial; font-size:medium; font-weight:bold; border-style:solid"> TBR & PCR Curing Men Power Planning Screen:: </div>
        
      <table class="innerTable">
        <tr>
        
         <td bgcolor="#99CCFF" align="left">
         
             <asp:LinkButton ID="UpdateGroupsLink" runat="server" CssClass="link"  
                 onclick="UpdateGroupsLink_Click">Update Groups</asp:LinkButton>
       <asp:CheckBox ID="workGroupChangeType" runat="server" 
                 oncheckedchanged="workGroupChangeType_CheckedChanged" Text="Add New WorkGroup" CssClass="gridViewButtons"
                 AutoPostBack="True" />
         </td>
         <td bgcolor="#99CCFF" align="right">
       <asp:RadioButton ID="PCRCuringRedioButton" runat="server" AutoPostBack="true" 
                 GroupName="aa" Text="PCRCuring" CssClass="gridViewButtons" 
                 oncheckedchanged="PCRCuringRedioButton_CheckedChanged" Checked="True" />
                
       <asp:RadioButton ID="TBRCuringRedioButton" runat="server" AutoPostBack="true" 
                 GroupName="aa"   Text="TBRCuring" CssClass="gridViewButtons" 
                 oncheckedchanged="TBRCuringRedioButton_CheckedChanged" />
       
       </td>
        </tr>
            </table>
            <asp:Panel ID="addNewWorkGroup" runat="server" Visible="False">
                
      <table class="innerTable" border="1" frame="box">
        <tr>
       
        <td style="width:5%">
            <asp:Label ID="Label" runat="server" Text="SelectWCGroup:" CssClass="masterLabel"></asp:Label>
              <asp:Label ID="WcgroupDetailLabel" runat="server" CssClass="masterLabel"
                Text="Machines Name:"></asp:Label>
            </td>
        <td style="width:20%" align="left">
            <asp:DropDownList ID="WcGroupDropDownList" runat="server" Width="40%" 
                onselectedindexchanged="WcGroupDropDownList_SelectedIndexChanged" 
                AutoPostBack="True" Font-Bold="true">
            </asp:DropDownList>
             <asp:Label ID="WcNameLabel" runat="server" CssClass="masterLabel"
                Text="------------------------------------------------------------------"></asp:Label>
                <asp:Label ID="groupIDlabel" runat="server" CssClass="masterLabel" Text="" Visible="false"></asp:Label>
            </td>
        <td style="width:5%" >
            <asp:Label ID="manningLabel" runat="server" CssClass="masterLabel"
                Text="SelectOperator:"></asp:Label>
                <asp:Label ID="operatorname" runat="server" CssClass="masterLabel"
                Text="OperatorName:"></asp:Label>
            </td>
        <td align="left" style="width:10%" >
            <asp:DropDownList ID="manningDropDownList" runat="server" Width="100%"
                CssClass="masterDropDownList" 
                onselectedindexchanged="WcGroupDropDownList0_SelectedIndexChanged" 
                AutoPostBack="True">
            </asp:DropDownList>
          <asp:Label ID="OperatornameLabel" runat="server" Text="----------" Font-Bold="true"  Width="100%"></asp:Label>
                <asp:Label ID="manningIDlabel" runat="server" CssClass="masterLabel" Text="" Visible="false"></asp:Label>

            </td>
           
        <td align="left" style="width:3%" >
       <asp:Label ID="shiftLabel" runat="server" Text="Shift:" CssClass="masterLabel" Width="100%"></asp:Label>

        </td>
        <td align="left" style="width:8%" >
             <asp:DropDownList ID="shiftDropDownList" runat="server" Width="100%" 
                CssClass="masterDropDownList" 
                 onselectedindexchanged="shiftDropDownList_SelectedIndexChanged" 
                 AutoPostBack="True">
                 <asp:ListItem Selected="True">Select</asp:ListItem>
                 <asp:ListItem>A</asp:ListItem>
                 <asp:ListItem>B</asp:ListItem>
                 <asp:ListItem>C</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td align="left" style="width:5%">
            <asp:Button ID="AddButton" runat="server" Text="SAVE" 
                CssClass="masterButton" onclick="AddButton_Click" />
       </td>
       
        </tr>
        
        </table>
        </asp:Panel> 
        
                
                <asp:Label ID="alertUser" runat="server" CssClass="alrtPopup" Visible="False" 
                   Font-Bold="True" Text="">
                    </asp:Label>
                <asp:Panel ID="GridViewPanel" Width="100%" Height="100%" runat="server">
                      
            
                    
            <asp:Table ID="editOP" runat="server" Width="100%">
                <asp:TableRow><asp:TableHeaderCell CssClass="gridViewAlternateHeader" style="width:11.9%; text-align:center; padding:5px">wcGroupName</asp:TableHeaderCell><asp:TableHeaderCell CssClass="gridViewAlternateHeader" style="width:25%; text-align:center; padding:5px">ShiftA</asp:TableHeaderCell><asp:TableHeaderCell CssClass="gridViewAlternateHeader" style="width:25%; text-align:center; padding:5px">shiftB</asp:TableHeaderCell><asp:TableHeaderCell CssClass="gridViewAlternateHeader" style="width:25%; text-align:center; padding:5px">ShiftC</asp:TableHeaderCell></asp:TableRow>
            </asp:Table>
                   
            <asp:GridView ID="operatorPlanningMainGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="GridView_RowDataBound"
    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Both"
    AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false" 
            ShowFooter="false" >
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FCFCFC" ForeColor="#333333" />
           
           <Columns>
            <asp:TemplateField HeaderText="wcGroupID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="1%">
                <ItemTemplate>
                        <asp:Label ID="wcGroupID" runat="server" Text='<%# Eval("wcgroupID") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
            <asp:TemplateField HeaderText="wcGroupName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                <ItemTemplate>
                    <asp:LinkButton ID="wcNames" runat="server" OnClick="showWCNames_Click"><asp:Label ID="wcGroupName" runat="server" Text='<%# Eval("wcgroupName") %>' CssClass="gridViewItems"></asp:Label></asp:LinkButton></ItemTemplate></asp:TemplateField></Columns><Columns>
            
            <asp:TemplateField HeaderText="shiftAoperatorsapCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="1%">
                <ItemTemplate>
                        <asp:Label ID="shiftAoperatorsapCode" runat="server" Text='<%# Eval("shiftAoperatorsapCode") %>' Visible="false"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
                <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
            <ItemTemplate>
         
            <asp:DropDownList ID="DropDownListA" runat="server" onselectedindexchanged="DropDownListA_SelectedIndexChanged" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
            <asp:Label ID="shiftAoperatorCodeLabel" runat="server" Text='<%# Eval("ShiftAoperatorCode") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
     
            <asp:TemplateField HeaderText="shiftBoperatorsapCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="1%">
                <ItemTemplate>
                        <asp:Label ID="shiftBoperatorsapCode" runat="server" Text='<%# Eval("shiftBoperatorsapCode") %>' Visible="false"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
          <asp:TemplateField HeaderText="shiftBOperatorCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
           <ItemTemplate>
           <asp:DropDownList ID="DropDownListB" runat="server" onselectedindexchanged="DropDownListB_SelectedIndexChanged" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
           <asp:Label ID="shiftBoperatorCodeLabel" runat="server" Text='<%# Eval("shiftBoperatorCode") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
           <asp:TemplateField HeaderText="shiftCoperatorsapCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Width="1%">
                <ItemTemplate>
                        <asp:Label ID="shiftCoperatorsapCode" runat="server" Text='<%# Eval("shiftCoperatorsapCode") %>' Visible="false"></asp:Label></ItemTemplate></asp:TemplateField></Columns><Columns>
           <asp:TemplateField HeaderText="shiftCoperatorCode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
           <ItemTemplate>
           <asp:DropDownList ID="DropDownListC" runat="server" onselectedindexchanged="DropDownListC_SelectedIndexChanged" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
           <asp:Label ID="shiftCoperatorCodeLabel" runat="server" Text='<%# Eval("shiftCoperatorCode") %>' CssClass="gridViewItems"></asp:Label></ItemTemplate></asp:TemplateField></Columns><PagerStyle BackColor="#507CD1" ForeColor="White" 
        HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="#FFFFFF" />
    </asp:GridView>

    <br />
                    <center><asp:LinkButton ID="saveAll" runat="server" Width="100%" CssClass="saveLink" OnClick="saveAll_Click"> 
                        
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Save&nbsp; </asp:LinkButton>
                    </center>
            
            </asp:Panel>
        
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>