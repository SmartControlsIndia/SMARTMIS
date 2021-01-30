<%@ Page Title="" Language="C#" MasterPageFile="~/smartMISMaster.Master" AutoEventWireup="true" CodeBehind="ReceipeFileName.aspx.cs" Inherits="SmartMIS.Master.ReceipeFileName" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="masterContentPlaceHolder" runat="server">

<link rel="stylesheet" type="text/css" href="../Style/master.css" />
   <link rel="stylesheet" type="text/css" href="../Style/masterPage.css" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
   
    <style>
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



    
     }
     
  
    

</script> 

    
    <asp:ScriptManager ID="recipeScriptManager" runat="server"></asp:ScriptManager>
   <%-- <asp:UpdatePanel ID="recipeUpdatePanel"  runat="server">
        <ContentTemplate>--%>
            <table class="masterTable" align="center" cellpadding="0" cellspacing="0">
               
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
                   <td class="masterLabel"> Receipe File Name :</td>
                    <td class="masterTextBox">
                    
                        <asp:TextBox ID="TextBox1" Width="80%" ValidationGroup="a" 
                             AutoComplete="off" runat="server"></asp:TextBox>
                        
                        <asp:Label ID="recipeProcessIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeProcessNameFieldValidator" runat="server" 
                            ControlToValidate="TextBox1" CssClass="reqFieldValidator"
                            ErrorMessage="Receipe File Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>              
                <tr>
                   <td class="masterLabel">Fill Data From File :</td>
                    <td class="masterTextBox">
                       <asp:TextBox ID="TextBox2" runat="server" Width="80%"  ValidationGroup="a"  AutoComplete="off"></asp:TextBox>
                         <cc1:calendarextender ID="CalendarExtender3" 
                                TargetControlID="TextBox2"   
                                runat="server" Format="dd/MMM/yyyy"  ></cc1:calendarextender>
                        <asp:Label ID="recipeProductTypeIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="recipeProductTypeNameFealdValidator" runat="server" 
                            ControlToValidate="TextBox2" CssClass="reqFieldValidator"
                            ErrorMessage="Date is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
           
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="recipeSaveButton" runat="server"  ValidationGroup="a"  CssClass="button" 
                            onclick="Button_Click" Text="Save" />
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
                        
                        <asp:HiddenField ID="recipeIDHidden" runat="server" Value="0" />
                        <asp:Timer ID="recipeNotifyTimer" runat="server" Enabled="false" 
                            Interval="5000" ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        
                    </td>
                   
                    <td>
                        <div ID="recipeNotifyMessageDiv1" runat="server" class="notifyMessageDiv" 
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
                        &nbsp;</td>
                </tr>
               
                <tr>
                    <td colspan="4">
                       
                        <asp:Panel ID="recipePanel" runat="server" CssClass="panel" Height="195px" 
                            ScrollBars="Vertical">
                            <asp:GridView ID="recipeGridView" runat="server" AllowPaging="false" 
                                AllowSorting="false" AutoGenerateColumns="False" CellPadding="3" 
                                ForeColor="#333333" GridLines="Both" PageSize="5" ShowHeader="true" 
                                Width="100%">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                 
                                
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                        HeaderText="File Name" ItemStyle-HorizontalAlign="Center" 
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTuoFileName" runat="server" 
                                                CssClass="gridViewItems" Text='<%# Eval("TuoFileName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" 
                                        HeaderText="Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastUpdate" runat="server" 
                                                CssClass="gridViewItems" Text='<%# Eval("LastUpdate") %>'></asp:Label>
                                            <asp:HiddenField ID="hfdid" Value='<%# Eval("id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="gridViewHeader" HeaderText="Action" 
                                        ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="recipeGridEditImageButton" CausesValidation="false" runat="server" 
                                                
                                                CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" 
                                                OnClick="ImageButton_Click" Text="Edit" ToolTip="Edit" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="#FFFFFF" />
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
        <%--</ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
