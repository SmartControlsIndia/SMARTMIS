<%@ Page Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="ColorCode.aspx.cs" Inherits="SmartMIS.Input.ColorCode" Title="TBM Manual Color Code"%>


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
   
       .tablecolumnNew
   {
       font-family:Arial;
       font-size:small;
       font-weight:bold;
       text-align:left;
       padding:5px;
       width:39%;
       
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
  
  
  
  function checkone(val)
  {
    var value1 = document.getElementById("<%=DropDownList1.ClientID%>");  
    var getvalue1 = value1.options[value1.selectedIndex].value;  
    //var gettext1 = value1.options[value1.selectedIndex].text;
    if(getvalue1 =="No")
    {
     $("#ctl00_masterContentPlaceHolder_DropDownList2").attr("disabled", true);
     $("#ctl00_masterContentPlaceHolder_ddlRed").attr("disabled", true);
      $("#ctl00_masterContentPlaceHolder_ddlBlue").attr("disabled", true);
       $("#ctl00_masterContentPlaceHolder_ddlYellow").attr("disabled", true);
        $("#ctl00_masterContentPlaceHolder_ddlGreen").attr("disabled", true);
    }
    else{
       $("#ctl00_masterContentPlaceHolder_DropDownList2").attr("disabled", false);
         $("#ctl00_masterContentPlaceHolder_ddlRed").attr("disabled", false);
      $("#ctl00_masterContentPlaceHolder_ddlBlue").attr("disabled", false);
       $("#ctl00_masterContentPlaceHolder_ddlYellow").attr("disabled", false);
        $("#ctl00_masterContentPlaceHolder_ddlGreen").attr("disabled", false);
    
    }
       
  }
  
 
window.onload = function () { 

    var value = document.getElementById("<%=DropDownList2.ClientID%>");  
    var getvalue = value.options[value.selectedIndex].value;  
    var gettext = value.options[value.selectedIndex].text;    
};

</script>

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
    <style type="text/css">
        #ctl00_masterContentPlaceHolder_Checkmy_color_pickerBoxList1_0
        {
        	color:Red;
        	background:Red;
        	}
        
    </style>
   <div>
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

<td class="tablecolumnNew"> <asp:Label ID="lbmsg" runat="server" style="font-weight:600;color:#ff0000;font-size:15px;"></asp:Label></td>
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
                            <p class="masterHeaderTagline" style=" color: white;">Painting Colour Code Form</p>
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
                        
                        </td>
                </tr>
              
                
                <tr>
                    <td class="masterLabel">
                        Colour Code :
                    </td>
                    <td class="masterTextBox">
                     <asp:DropDownList ID="DropDownList1" runat="server" 
                            CssClass="masterDropDownList"  onchange="checkone(this.val)"
                            Width="82%">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                             <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    
                    
                        <asp:TextBox ID="bdBarcodeTextBox" runat="server" Width="45%" MaxLength="10" style="display:none;" onchange="Test(this.value)"></asp:TextBox>
                        <span style=" color: red;"</span> 
                  
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                          
            </td>
                     
                </tr>
                
                  <tr>
                    <td class="masterLabel">
                        Number Of Colour :
                    </td>
                    <td class="masterTextBox">
                    
                      <asp:DropDownList ID="DropDownList2" runat="server" 
                            CssClass="masterDropDownList" 
                            Width="82%">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                             <asp:ListItem Text="1" Value="1"></asp:ListItem>
                             <asp:ListItem Text="2" Value="2"></asp:ListItem>
                             <asp:ListItem Text="3" Value="3"></asp:ListItem>
                             <asp:ListItem Text="4" Value="4"></asp:ListItem>
                             <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    
                        <asp:TextBox ID="bdBarcodeTextBox1" runat="server" Width="45%" MaxLength="10" style="display:none;"   onchange="Test(this.value)"></asp:TextBox>
                        <span style=" color: red;"></span> 
                  
                    </td>
                <td><span class="errorSpan">*</span></td> 
            <td>
                          
            </td>
                     
                </tr>
                
                <tr>
                    <td class="masterLabel">
                    <br />
                        Select Colours :
                    </td>
                    <td class="masterTextBox">
                       
                       
                       <table>
                       <tr>
                       <td style="padding-top:5px;">
                      <center><b style="color: white;
    background: red;
    padding: 2px 28px;">Red</b></center>
                               <asp:DropDownList ID="ddlRed" runat="server" 
                            CssClass="masterDropDownList" 
                            Width="100px" style="margin-top: 4px;">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                             <asp:ListItem Text="1 Red " Value="Red"></asp:ListItem>
                             <asp:ListItem Text="2 Red" Value="Red,Red"></asp:ListItem>
                             <asp:ListItem Text="3 Red" Value="Red,Red,Red"></asp:ListItem>
                             <asp:ListItem Text="4 Red" Value="Red,Red,Red,Red"></asp:ListItem>
                             <asp:ListItem Text="5 Red" Value="Red,Red,Red,Red,Red"></asp:ListItem>
                        </asp:DropDownList>
                       </td>
                         <td>
                           <center><b style="color: #ffffff;
    background: #28aaf7;
    padding: 2px 28px;">Blue</b></center>
                               <asp:DropDownList ID="ddlBlue" runat="server" 
                            CssClass="masterDropDownList" 
                             Width="100px" style="margin-top: 4px;">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                             <asp:ListItem Text="1 Blue" Value="Blue"></asp:ListItem>
                             <asp:ListItem Text="2 Blue" Value="Blue,Blue"></asp:ListItem>
                             <asp:ListItem Text="3 Blue" Value="Blue,Blue,Blue"></asp:ListItem>
                             <asp:ListItem Text="4 Blue" Value="Blue,Blue,Blue,Blue"></asp:ListItem>
                             <asp:ListItem Text="5 Blue" Value="Blue,Blue,Blue,Blue,Blue"></asp:ListItem>
                        </asp:DropDownList>
                         
                         </td>
                           <td>
                           <center><b style="color: #ffffff;
    background: #efe820;
    padding: 2px 28px;">Yellow</b></center>
                               <asp:DropDownList ID="ddlYellow" runat="server" 
                            CssClass="masterDropDownList" 
                             Width="100px" style="margin-top: 4px;">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                             <asp:ListItem Text="1 Yellow" Value="Yellow"></asp:ListItem>
                             <asp:ListItem Text="2 Yellow" Value="Yellow,Yellow"></asp:ListItem>
                             <asp:ListItem Text="3 Yellow" Value="Yellow,Yellow,Yellow"></asp:ListItem>
                             <asp:ListItem Text="4 Yellow" Value="Yellow,Yellow,Yellow,Yellow"></asp:ListItem>
                             <asp:ListItem Text="5 Yellow" Value="Yellow,Yellow,Yellow,Yellow,Yellow"></asp:ListItem>
                        </asp:DropDownList>
                           </td>
                             <td>
                              <center><b style="color: white; background: #11d211;padding: 2px 28px;">Green</b></center>
                               <asp:DropDownList ID="ddlGreen" runat="server" 
                            CssClass="masterDropDownList" 
                             Width="100px" style="margin-top: 4px;">
                             <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                             <asp:ListItem Text="1 Green" Value="Green"></asp:ListItem>
                             <asp:ListItem Text="2 Green" Value="Green,Green"></asp:ListItem>
                             <asp:ListItem Text="3 Green" Value="Green,Green,Green"></asp:ListItem>
                             <asp:ListItem Text="4 Green" Value="Green,Green,Green,Green"></asp:ListItem>
                             <asp:ListItem Text="5 Green" Value="Green,Green,Green,Green,Green"></asp:ListItem>
                        </asp:DropDownList>
                             </td>
                       </tr>
                       </table>
                     
                               
                       
                        <span style=" color: red;"></span> 
                  
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
                <tr><td colspan="10" style="margin-top:40px;padding: 10px;background: #175188;color: white;font-size: 17px;font-family: Arial;font-weight: 600;">List of Painting Colour Code</td></tr>
               <tr>
              
                    <td colspan="10">
                        <table cellspacing="0" class="innerTable" >
                            <tr>
                                <td class="gridViewHeader" style="width:6%; padding:5px;font-size: 13px;">
                                     Process Name </td>
                                <td class="gridViewHeader" style="width:3%;font-size: 13px;">Recipe Name
                                   </td>
                                
                                <td class="gridViewHeader" style="width:3%;font-size: 13px;">
                                    Colour Code</td>
                                    <td class="gridViewHeader" style="width:4%;font-size: 13px;">
                                      Number Of Colour</td>
                                       <td class="gridViewHeader" style="width:4%; padding:5px;font-size: 13px;">
                                    Colour Name</td>
                                       <td class="gridViewHeader" style="width:4%; padding:5px;font-size: 13px;">
                                    Last Updated Date</td>
                               
                              
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