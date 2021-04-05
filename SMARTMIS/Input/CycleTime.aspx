<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="CycleTime.aspx.cs" Inherits="SmartMIS.Input.CycleTime" Title="TBM Manual Cycle Time" %>

<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">
    <style type="text/css">
    
   #headerdive
   {
       background-color:#C3D9FF;
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       
   }
      .tablecolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:13%;
       
   }
   .gridcolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
      
   }
   
   tr
   {
      border:1pt solid black;
      
     
   }
   
   .tableheadercolumn
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:15%;
       background-color:#C3D9FF;
       
   }
  .bder
  {
      background-color: inherit; 
   font-size:20px; 
   font-family:Arial Narrow;
   border:1pt solid black;
    border-top-color:black;
    border-width:thin;
    border-bottom-color:black;
    height:30px;
}
 
   .button
{
    cursor: pointer;    
    filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
 endColorstr='#579AEB'); /* for IE */
    -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=' #85B6F0',
    endColorstr='#579AEB'); /* for IE 8 and above */
    background: -webkit-gradient(linear, left top, left bottom, from(#85B6F0),
    to(#579AEB)); /* for webkit browsers */
    background: -moz-linear-gradient(top, #85B6F0, #579AEB); /* for firefox 3.6+ */
    background: -o-linear-gradient(top, #85B6F0, #579AEB); /* for Opera */
    width:100PX;
}
   
</style>
<script type="text/javascript">
        function Test(val) {
            if (isNaN(val)) {
               // alert("Enter Only Numeric Value");
            }
            
        }
    </script>
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
   <%-- <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= bdIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= bdIDLabel.ClientID %>').innerHTML = value;
        }       
    </script>--%>
   <div >
       <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
<table width="100%" class="bder">
<tr>
<td class="tablecolumn">Select Process :</td>
<td class="tablecolumn" >
    <asp:DropDownList ID="processDropDownList" runat="server" Width="100%" AutoPostBack="true" 
        onselectedindexchanged="processDropDownList_SelectedIndexChanged">
        <asp:ListItem Value="Select Process"></asp:ListItem>
        <asp:ListItem Value="TBMTBR"></asp:ListItem>
        <asp:ListItem Value="TBMPCR"></asp:ListItem>
    </asp:DropDownList>
</td>

<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>
<td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td><td class="tablecolumn">
     &nbsp;</td>
<td class="tablecolumn"></td>
</tr>
</div>
&nbsp;
</br>
  <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
   <table class="inputTable" align="center" cellpadding="0" cellspacing="0" style="padding-bottom: 30px;">
                <tr>
                    <td class="inputFirstCol"></td>
                    <td class="inputSecondCol"></td>
                    <td class="inputThirdCol"></td>
                    <td class="inputForthCol"></td>
                </tr>
                
                <tr>
                    <td colspan="4">
                        <div class="masterHeader" style=" background-color: #217cc8;">
                            <p class="masterHeaderTagline" style=" color: white;">TBM Cycle Time Recipe Wise</p>
                        </div>
                    </td>
                </tr>
                <asp:Label ID="bdIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
             
                
                <tr>
                    <td class="masterLabel">Recipe Code :</td>
                    <td>
                    
                        <asp:DropDownList ID="bdrecipeCodeDropDownList" runat="server" 
                            CssClass="masterDropDownList" 
                            Width="82%" >
                        </asp:DropDownList>
                        
                    <asp:Label ID="bdrecipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span></td>                  
                    <td>   <asp:RequiredFieldValidator ID="bdRecipeCodeValidator" runat="server" 
                            ControlToValidate="bdrecipeCodeDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Recipe Code is Required">
                        </asp:RequiredFieldValidator>
                        </span></td>
                </tr>
              
                
                <tr>
                    <td class="masterLabel">
                        Cycle Time 1:
                    </td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="bdBarcodeTextBox" runat="server" Width="45%" MaxLength="10"   onchange="Test(this.value)"></asp:TextBox>
                        <span style=" color: red;">(In a Sec.)</span> 
                  
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                          
            </td>
                     
                </tr>
                
                  <tr>
                    <td class="masterLabel">
                        Cycle Time 2:
                    </td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="bdBarcodeTextBox1" runat="server" Width="45%" MaxLength="10"   onchange="Test(this.value)"></asp:TextBox>
                        <span style=" color: red;">(In a Sec.)</span> 
                  
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                          
            </td>
                     
                </tr>
               
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="bdSaveButton" runat="server" CssClass="masterButton" 
                            Text="Save" onclick="Button_Click"  />&nbsp;
                        <asp:Button ID="bdCancelButton" runat="server" CssClass="masterButton" Text="Cancel" onclick="Button_Click" CausesValidation="false"  />&nbsp;                       
                    </td>
                    <td></td>
                    <td></td>
                </tr>
               
                <tr>
                    <td style="padding-bottom: 40px">
                        <asp:Timer ID="bdNotifyTimer" runat="server" OnTick="NotifyTimer_Tick" Interval="5000" Enabled="false" >
                        </asp:Timer>
                        <asp:HiddenField ID="bdIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="bdNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="bdNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="bdNotifyLabel" runat="server" Text="Record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr><td colspan="10" style="margin-top:40px;padding: 10px;background: #175188;color: white;font-size: 18px;font-family: Arial;">List of Recipe Cycle Time</td></tr>
               <tr>
              
                    <td colspan="10">
                        <table cellspacing="0" class="innerTable" >
                            <tr>
                                <td class="gridViewHeader" style="width:6%; padding:5px;font-size: 13px;">
                                     Process Name </td>
                                <td class="gridViewHeader" style="width:3%;font-size: 13px;">Recipe Name
                                   </td>
                                
                                <td class="gridViewHeader" style="width:4%;font-size: 13px;">
                                    Cycle Time 1</td>
                                    <td class="gridViewHeader" style="width:4%;font-size: 13px;">
                                    Cycle Time 2</td>
                                <td class="gridViewHeader" style="width:4%; padding:5px;font-size: 13px;">
                                    Last Updated Date</td>
                               <%-- <td class="gridViewHeader" style="width:3%;">EDIT
                                </td>--%>
                            </tr>
                        </table>
                        <asp:Panel ID="recipePanel" runat="server" CssClass="panel" Height="195px" Width="100%" ScrollBars="Vertical">
                            <asp:GridView ID="recipeGridView" runat="server" HeaderStyle-Width="100%"  
                                Width="100%" ShowHeader="false" OnRowDataBound="GridView_RowDataBound"
                                CellPadding="4" ForeColor="#333333" GridLines="None">
                           
                                <RowStyle BackColor="#EFF3FB" />
                               
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    Width="100%" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                                
                            </asp:GridView>
                          
                            
                            </asp:Panel>
                    </td>
                </tr>
            </table>




</asp:Content>
