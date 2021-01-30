<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="SmartMIS.Input.Input" Title="TBMManual BarcodeEntry" %>
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
                alert("Enter Only Numeric Value");
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
   <table class="inputTable" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="inputFirstCol"></td>
                    <td class="inputSecondCol"></td>
                    <td class="inputThirdCol"></td>
                    <td class="inputForthCol"></td>
                </tr>
                
                <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">TBM MANUAL BARCODE ENTRY</p>
                        </div>
                    </td>
                </tr>
                <asp:Label ID="bdIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                <tr>
                    <td class="masterLabel">Work center Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="bdWCNameDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" Width="82%" 
                            onselectedindexchanged="bdWCNameDropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="WCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="bdWCNameFieldValidator" runat="server" 
                            ControlToValidate="bdWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td class="masterLabel">Recipe Code :</td>
                    <td>
                    
                        <asp:DropDownList ID="bdrecipeCodeDropDownList" runat="server" AutoPostBack="true" 
                            CssClass="masterDropDownList" 
                            Width="82%"  onselectedindexchanged="bdWCNameDropDownList_SelectedIndexChanged" >
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
                    <td class="masterLabel">Operator Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="bdOperatorDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true"   Width="82%" onselectedindexchanged="bdWCNameDropDownList_SelectedIndexChanged" >
                            <asp:ListItem>test</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="OpIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                
                <tr>
                    <td class="masterLabel">
                        Barcode from:
                    </td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="bdBarcodeTextBox" runat="server" Width="45%" MaxLength="10"   onchange="Test(this.value)"></asp:TextBox>
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
      ControlToValidate="bdBarcodeTextBox" ErrorMessage="Enter 10 digit barcode" 
    ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                    <%--<asp:RegularExpressionValidator id="RegularExpressionValidator3"
                   ControlToValidate="bdBarcodeTextBox" ValidationExpression="\d+" Display="Static"
                   EnableClientScript="true"
                   ErrorMessage="Please enter numbers only"
                   runat="server"/>   --%>
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="bdBarcodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="Barcode is Required">
                        </asp:RequiredFieldValidator>            
            </td>
                     
                </tr>
                <tr> <td class="masterLabel"> <b> To Count:</b><td>
                   
                                    <asp:TextBox ID="bdBarcodeToTextBox" runat="server" Width="40%" MaxLength="2" 
                            CausesValidation="True"></asp:TextBox>
                                               
                        <td class="masterTextBox">
                            <asp:RegularExpressionValidator id="RegularExpressionValidator1"
                   ControlToValidate="bdBarcodeToTextBox" ValidationExpression="\d+" Display="Static"
                   EnableClientScript="true"
                   ErrorMessage="Enter Numbers only"
                   runat="server"/></td>
                  
                   
                   
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
                    <td>
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
               
            </table>




</asp:Content>
