<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recipeNew.aspx.cs" MasterPageFile="~/smartMISMaster.Master" Inherits="SmartMIS.Master.recipeNew" %>

<asp:Content ID="recipeContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.17.1/dist/bootstrap-table.min.css">
   <link rel="stylesheet" type="text/css" href="../Style/master.css" />
   <link rel="stylesheet" type="text/css" href="../Style/masterPage.css" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    <script src="https://unpkg.com/bootstrap-table@1.17.1/dist/bootstrap-table.min.js"></script>
   <script language="javascript" type="text/javascript">

       function setID(value)
       {
               document.getElementById('<%= recipeIDHidden.ClientID %>').value = value.toString();
               document.getElementById('<%=recipeIDLabel.ClientID %>').innerHTML = value;      
       }
            
    </script>
    
    <style type="text/css">
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

table th
{
	text-align:left !important;
}
.panel td 
{
    border :1px solid #000;
    font-size: 13px !important;
}

.panel a {
    background: #507CD1;
    color: #fff;
    text-decoration: none;
    padding: 2px 4px;
    border-radius: 2px;
    margin-left: 2%;
    margin-right: 2%;
}
.hide
{
	display:none !important;
}
    </style>
 <script language="Javascript" type="text/javascript">
    // var isShift = false;
     function isNumeric(keyCode) {
         if ((((keyCode >= 48 && keyCode <= 57 || keyCode == 189) || keyCode == 8 || keyCode == 189 || keyCode == 9 || (keyCode >= 96 && keyCode <= 105))) == true)
             return true
         else {
             alert("Only Numbers Are Allowed for recipeNo");   
         }
     }
</script> 

    
    <asp:ScriptManager ID="recipeScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="recipeUpdatePanel"  runat="server">
        <ContentTemplate>
            <table class="1masterTable" data-toggle="table" align="center" cellpadding="0" cellspacing="0">
               
                <tr>
                    <td class="masterFirstCol"></td>
                    <td class="masterSecondCol"></td>
                    <td class="masterThirdCol"></td>
                    <td class="masterForthCol"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Recipe </p>
                        </div>
                    </td>
                </tr> 
                <tr>
                    <td class="masterLabel">Recipe Name:</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="recipeNameTextBox" CssClass="recipeNameTextBox" runat="server" Width="80%" AutoComplete="off" ></asp:TextBox>
                        <asp:Label ID="recipeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel recipeIDLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeNameFieldValidator" runat="server" 
                            ControlToValidate="recipeNameTextBox" CssClass="reqFieldValidator"
                            ErrorMessage="Recipe Name is Required" ValidationGroup="RR">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>              
                <tr>
                   <td class="masterLabel"> Process Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="recipeProcessNameDropDownList" runat="server" 
                            CssClass="masterDropDownList recipeProcessNameDropDownList" Width="82%" AutoPostBack="false"
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:Label ID="recipeProcessIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeProcessNameFieldValidator" runat="server" 
                            ControlToValidate="recipeProcessNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="Process Name is Required" ValidationGroup="RR" >
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>              
                <tr>
                   <td class="masterLabel">ProductType Name:</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="recipeProductTypeNameDropDownList" runat="server" 
                            CssClass="masterDropDownList recipeProductTypeNameDropDownList" Width="82%" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList_SelectedIndexChanged"  >
                        </asp:DropDownList>
                        <asp:Label ID="recipeProductTypeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeProductTypeNameFealdValidator" runat="server" 
                            ControlToValidate="recipeProductTypeNameDropDownList" CssClass="reqFieldValidator"
                            ErrorMessage="ProductType Name is Required" ValidationGroup="RR">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        OEM Name :</td>
                    <td>
                        <asp:DropDownList ID="OEMDropDownList" runat="server" AutoPostBack="false" 
                            CssClass="masterDropDownList OEMDropDownList" 
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:Label ID="oemIDLabel" runat="server" CssClass="masterHiddenLabel" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Recipe No :</td>
                    <td class="masterTextBox">
                          <asp:TextBox ID="recipeNoTextBox" CssClass="masterTextBox recipeNoTextBox" runat="server" Width="80%" AutoComplete="off" runat="server" onpaste = "return false;" ></asp:TextBox>
                    </td>
                    <td>
                                      <asp:CompareValidator ID="CompareValidator2" ControlToValidate="recipeNoTextBox" runat="server" ErrorMessage="Integers only please" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>

                    </td>
                    <td></td>
                </tr> 
                <tr>
                    <td class="masterLabel">
                        Spec Weight :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="SpecWeightTextBox" CssClass="masterTextBox SpecWeightTextBox" runat="server" AutoComplete="off" CausesValidation="true"
                            onpaste="return false;" Width="80%"></asp:TextBox>
                           
                    </td>
                    <td>
                  <asp:CompareValidator ID="CompareValidator1" ControlToValidate="SpecWeightTextBox" runat="server" ErrorMessage="Enter vailid weight please" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                    </td>
                    <td>
                    </td>
                    </tr> 
                <tr>
                    <td class="masterLabel">
                        TBRPainting Code :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="TBRPCodeTextBox" CssClass="masterTextBox TBRPCodeTextBox" runat="server" AutoComplete="off" CausesValidation="true"
                            onpaste="return false;" Width="80%"></asp:TextBox>
                           
                    </td>
                    <td>
                  <asp:CompareValidator ID="CompareValidator3" ControlToValidate="TBRPCodeTextBox" runat="server" ErrorMessage="Enter vailid PCode please" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                    </td>
                    <td>
                    </td>
                    </tr>
                <tr>
                    <td class="masterLabel">
                        Recipe Size :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="recipeSizeTextBox" CssClass="masterTextBox recipeSizeTextBox" runat="server" AutoComplete="off" 
                            Width="80%"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Tyre Design :</td>
                    <td class="masterTextBox">
                       <asp:DropDownList ID="DesignDropDownList1" runat="server" 
                            CssClass="masterDropDownList DesignDropDownList1" Width="82%" AutoPostBack="false"
                            onselectedindexchanged="DropDownList_SelectedIndexChanged" >
                        </asp:DropDownList>
                         <td>
                        &nbsp;</td>
                   <%-- <td>
                        <asp:Label ID="DesignLabel" runat="server" CssClass="masterHiddenLabel" Text="0"></asp:Label>
                    </td>--%>
                        <%--<asp:TextBox ID="DesignTextBox1" CssClass="masterTextBox DesignTextBox1" runat="server" AutoComplete="off" 
                            Width="80%"></asp:TextBox>--%>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        Description :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="recipeDescriptionTextBox" CssClass="masterTextBox recipeDescriptionTextBox"  runat="server" Rows="3" 
                            TextMode="MultiLine" Width="80%">
                        </asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                        SAP Material Code</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="txtsapmaterialcode" Width="80%" CssClass="masterTextBox txtsapmaterialcode"  runat="server"></asp:TextBox>
                    </td>
                    <td>
                       <span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="sapmaterialcodeFieldValidator" runat="server" 
                            ControlToValidate="txtsapmaterialcode" CssClass="reqFieldValidator" 
                            ErrorMessage="SAP Material Code is Required" ValidationGroup="RR">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">
                       UpperWeight</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="upperweighttxt" Width="80%" CssClass="masterTextBox upperweighttxt"  runat="server"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td class="masterLabel">
                        LowerWeight</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="txtLowerWeight" Width="80%" CssClass="masterTextBox txtLowerWeight"  runat="server"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td class="masterLabel">
                        TBRTUOCode</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="tbrtuocodetxt" Width="80%" CssClass="masterTextBox tbrtuocodetxt" MaxLength=14  runat="server"></asp:TextBox></td>
                        <td>
                      <asp:RegularExpressionValidator runat="server" id="rexNumber" controltovalidate="tbrtuocodetxt" CssClass="reqFieldValidator" validationexpression="^[a-zA-Z0-9\s]{14,14}$" errormessage="Please Enter a 14 digit number!" /></td>

                    
                </tr>
                <tr>
                    <td class="masterLabel">
                        TUOEnable :</td>
                    <td class="masterTextBox">
                       <asp:DropDownList ID="TUOEnableDropDownList1" runat="server" 
                            CssClass="masterDropDownList TUOEnableDropDownList1" Width="82%" AutoPostBack="false"
                             >
                             <asp:ListItem Value="-1">Select</asp:ListItem>
                              <asp:ListItem Value="0">Disable</asp:ListItem>
                              <asp:ListItem Value="1">Enable</asp:ListItem>
                        </asp:DropDownList>
                         <asp:Label ID="LabelTUOEnable" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                         <td>
                        &nbsp;</td>
                   <%-- <td>
                        <asp:Label ID="DesignLabel" runat="server" CssClass="masterHiddenLabel" Text="0"></asp:Label>
                    </td>--%>
                        <%--<asp:TextBox ID="DesignTextBox1" CssClass="masterTextBox DesignTextBox1" runat="server" AutoComplete="off" 
                            Width="80%"></asp:TextBox>--%>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <%-- <tr>
                    <td class="masterLabel">
                       Curing Capacity</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="txtcapacity" Width="80%" CssClass="masterTextBox txtcapacity"  runat="server"></asp:TextBox>
                    </td>
                    
                </tr>--%>
                
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="recipeSaveButton" runat="server" CssClass="button" 
                            onclick="Button_Click" Text="Save" ValidationGroup="RR"  />
                        &nbsp;
                        <asp:Button ID="recipeCancelButton" runat="server" CausesValidation="false" 
                            CssClass="button" OnClick="Button_Click" Text="Cancel" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div ID="modalPage">
                            <div class="modalBackground">
                            </div>
                            <div class="modalContainer">
                                <div class="modal">
                                    <div class="modalTop">
                                        <a href="javascript:hideModal('modalPage')">
                                        <img alt="Close" class="closeImg" src="../Images/cancel.png" /></a></div>
                                    <div class="modalBody">
                                        <table cellspacing="0" class="innerTable">
                                            <tr>
                                                <td style="width: 20%">
                                                </td>
                                                <td style="width: 80%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">
                                                    <img alt="Close" src="../Images/question.png" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="recipeMessageLabel" runat="server" CssClass="masterWelcomeLabel" 
                                                        Text="Do you want to delete recipe."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="recipeDialogOKButton" runat="server" CausesValidation="false" 
                                                        CssClass="button" OnClick="Button_Click" Text="OK" />
                                                    &nbsp;
                                                    <asp:Button ID="recipeDialogCancelButton" runat="server" CssClass="button" 
                                                        OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Timer ID="recipeNotifyTimer" runat="server" Enabled="false" 
                            Interval="5000" ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        <asp:HiddenField ID="recipeIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div ID="recipeNotifyMessageDiv" runat="server" class="notifyMessageDiv" 
                            visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <img ID="recipeNotifyImage" runat="server" alt="notify" class="notifyImg" 
                                            src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="recipeNotifyLabel" runat="server" 
                                            Text="Recipe saved successfully."></asp:Label>
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
             <tr>
                    <td class="masterLabel" colspan="1" style="
    padding-right: 0px;
">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">
                                Select ProcessName :</p>
                        </div>
                    </td>
                    <td colspan="1">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">
                                <asp:DropDownList ID="processDropDownList" runat="server" AutoPostBack="True" 
                                    CssClass="masterDropDownList" 
                                    onselectedindexchanged="DropDownList_SelectedIndexChanged" Width="80%">
                                </asp:DropDownList>
                            </p>
                        </div>
                    </td>
                    <td colspan="2">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">
                                Recipe List Of Selected Process</p>
                        </div>
                    </td>
                    <td colspan="1" width="5%" align="right">
                    <div class="masterHeader"><asp:LinkButton runat="server" ID="ExportExcel" onclick="expToExcel_Click"><img src="../Images/Excel.jpg" alt="Export To Excel" class="imag"  /></asp:LinkButton></div>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="10">
                        <table cellspacing="0" class="table" data-toggle="table">
                            <tr>
                                <td class="gridViewHeader" style="width:8%; padding:5px">
                                    Recipe Name</td>
                                <td class="gridViewHeader" style="width:5%;">
                                    Process Name</td>
                                <td class="gridViewHeader" style="width:5%;">
                                    ProductType Name</td>
                                <td class="gridViewHeader" style="width:5%;">
                                    Oem Name</td>
                                <td class="gridViewHeader" style="width:5%; padding:5px">
                                    Recipe No</td>
                                    <td class="gridViewHeader" style="width:5%; padding:3px;">
                                    Spec Weight</td>
                                     <td class="gridViewHeader" style="width:5%; padding:5px">
                                   TBRPCode</td>
                                <td class="gridViewHeader" style="width:4%; padding:5px">
                                    Recipe Size</td>
                                     <td class="gridViewHeader" style="width:7%;padding:5px;text-align: center;">
                                   Tyre Design</td>
                                <td class="gridViewHeader" style="width:9%; padding:10px">
                                    Description</td>
                               
                                <td class="gridViewHeader" style="width:16%; padding: 5px; padding-left: 39px;">
                                    SapCode</td>
                                     <td class="gridViewHeader" style="width:4%; padding:5px">
                                    Upper Weight</td>
                                     <td class="gridViewHeader" style="width:5%; padding:5px">
                                    Lower Weight</td>
                                     <td class="gridViewHeader" style="width:5%; padding:10px">
                                    TBRTUOCODE</td>
                                    <td class="gridViewHeader" style="width:2%; padding:8px">
                                    TUO Enable</td>
                                <%--<td class="gridViewHeader" style="width:2%; padding:5px">
                                    Recipe ID</td>--%>
                                     <td class="gridViewHeader" style="width:3%;">EDIT
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="recipePanel" runat="server" CssClass="panel" Height="195px" Width="100%" ScrollBars="Vertical">
                            <asp:GridView ID="recipeGridView" runat="server" HeaderStyle-Width="100%"  
                                Width="100%" ShowHeader="false" OnRowDataBound="GridView_RowDataBound"
                                CellPadding="4" ForeColor="#333333" GridLines="None">
                                <RowStyle BackColor="#EFF3FB" />
                               <%-- <Columns>
                                    <asp:ButtonField CommandName="Edit" Text="Edit" />
                                     <asp:ButtonField CommandName="delete" Text="delete" />
                                </Columns>--%>
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
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                </table>
                
                <script type="text/javascript">
                "use strict";
                
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                
                $('.panel table td:nth-child(17)').addClass("hide");
                 
                prm.add_endRequest(function (s, e) {
                    $('.panel table td:nth-child(17)').addClass("hide");
                });
                
                function edit_Click(currentRow)
                {
                    var recipeName   =    $(currentRow).closest("tr").find('td:eq(0)').text();
                    var ProcessName  =    $(currentRow).closest("tr").find('td:eq(1)').text();
                    var ProductType  =    $(currentRow).closest("tr").find('td:eq(2)').text();
                    var OEMName      =    $(currentRow).closest("tr").find('td:eq(3)').text();
                    var recipeNo     =    $(currentRow).closest("tr").find('td:eq(4)').text();
                    var weight       =    $(currentRow).closest("tr").find('td:eq(5)').text();
                    var code         =    $(currentRow).closest("tr").find('td:eq(6)').text();
                    var recipeSize   =    $(currentRow).closest("tr").find('td:eq(7)').text();
                    var tyreSize     =    $(currentRow).closest("tr").find('td:eq(8)').text();
                    var desciption   =    $(currentRow).closest("tr").find('td:eq(9)').text();	 
                    var sapcode      =    $(currentRow).closest("tr").find('td:eq(10)').text();	 
                    var up_weight    =     $(currentRow).closest("tr").find('td:eq(11)').text();	 
                    var lower_weight =   $(currentRow).closest("tr").find('td:eq(12)').text();	
                    var tbrtuocode   =   $(currentRow).closest("tr").find('td:eq(13)').text();	 
                     var tuoenable   =   $(currentRow).closest("tr").find('td:eq(14)').text();	                
                    var id           =   $(currentRow).closest("tr").find('td:eq(16)').text();
                    
             
                    $('.recipeNameTextBox').val(recipeName);
                    $('.recipeProcessNameDropDownList').val(ProcessName);
                    $('.recipeProductTypeNameDropDownList').val(ProductType);
             
                    $('.txtsapmaterialcode').val(sapcode);
                    $('.recipeNoTextBox').val(recipeNo);
                    $('.SpecWeightTextBox').val(weight);
                    $('.TBRPCodeTextBox').val(code);
                    $('.recipeSizeTextBox').val(recipeSize);
                    $('.DesignDropDownList1').val(tyreSize);
                    $('.OEMDropDownList').val(OEMName);
                    $('.recipeDescriptionTextBox').val(desciption);
                    $('.upperweighttxt').val(up_weight);
                    $('.txtLowerWeight').val(lower_weight);
                    $('.tbrtuocodetxt').val(tbrtuocode);
                    $('.TUOEnableDropDownList1').val(tuoenable);
                    $('#ctl00_masterContentPlaceHolder_recipeIDHidden').val(id);
                }
                
           //function delete_Click(currentRow) {
//            if (confirm("Do you want to delete this record?")) {
//              var id           =    $(currentRow).closest("tr").find('td:eq(11)').text();	 
////            var row = $(this).closest("tr");
//            var RecipeId = id;
//            alert(RecipeId);
//            $.ajax({
//                type: "POST",
//                url: "recipe.aspx.cs/delete",
//                data: '{RecipeId: ' + recipeIDLabel + '}',

//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (Result1) {
//                alert(RecipeId);
//                   // fillTabledata();
//                }
//           });
//        }
//        }

   
                </script>        
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="ExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
