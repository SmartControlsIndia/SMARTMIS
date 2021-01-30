<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="curingHMI.aspx.cs" MasterPageFile="~/smartMISHMIMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.curingHMI" %>

<asp:Content ID="curingHMIContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
    <script type="text/javascript">
        
        function set_Keys(obj) {

            var tempString = document.getElementById('<%= this.curingHMIPasswordTextBox.ClientID %>').value;
            
            if (obj == "CE") {
                document.getElementById('<%= this.curingHMIPasswordTextBox.ClientID %>').value = "";
            }
            else if (obj == "<<") {
                if (tempString.length == 0) {
                }
                else {
                    document.getElementById('<%= this.curingHMIPasswordTextBox.ClientID %>').value = tempString.substr(0, tempString.length - 1);
                }
            }
            else {
                if (tempString.length == 5) {
                }
                else {
                    document.getElementById('<%= this.curingHMIPasswordTextBox.ClientID %>').value = tempString + obj;
                }
            }
        }

    </script>

    <asp:ScriptManager ID="curingHMIScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="curingHMIUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="curingTable" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 30%"></td>
                    <td style="width: 20%"></td>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <asp:Button ID="curingHMIWCLoginButton" runat="server" CssClass="curingButton" 
                            Text="Workcenter Login" style="width: 90%" 
                            onclick="Button_Click" />
                        <asp:Button ID="curingHMIMagicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" CausesValidation="false" />
                    </td>
                    <td>
                        <asp:HiddenField id="curingHMIWCIDHidden" runat="server" Value="" />&nbsp;
                        <asp:HiddenField id="curingHMIManningIDHidden" runat="server" Value="" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="modalPageForLogin">
                            <div class="modalBackground">
                            </div>
                            <div class="curingHMIModalContainer">
                                <div class="curingHMIModal">
                                    <div class="curingHMIModalTop"><a href="javascript:hideModal('modalPageForLogin')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                    <div class="modalBody">
                                        <table class="innerTable" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 100%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table align="center" style="width: 100%">
                                                        <tr>
                                                            <td style="width: 30%"></td>
                                                            <td style="width: 70%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="curingHMILabel">
                                                                E-Code:</td>
                                                            <td>
                                                                <input id="curingHMIPasswordTextBox" runat="server" readonly="true" class="curingHMITextBox" AutoComplete="off" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table align="center" style="width: 80%">
                                                                    <tr>
                                                                        <td style="width:25%"></td>
                                                                        <td style="width:25%"></td>
                                                                        <td style="width:25%"></td>
                                                                        <td style="width:25%"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" value="1" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)" />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="2" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="3" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="<<" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" value="4" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="5" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="6" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="CE" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" value="7" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="8" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="9" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" value="0" style="width: 90%" class="curingButton" onclick="javascript:set_Keys(this.value)"  />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Button ID="curingHMILoginButton" runat="server" CssClass="curingButton" Text="Sign in" style="width: 40%" OnClick="Button_Click" />
                                                                <input id="curingHMICancelButton" type="button" class="curingButton" value="Cancel" style="width: 40%" onclick="javascript:hideModal('modalPageForLogin')" />
                                                            </td>                              
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Label ID="curingHMIloginErrorLabel" runat="server" CssClass="curingHMIInvalidMessageLabel" Text="" style="width: 80%" />
                                                            </td>                              
                                                        </tr>
                                                    </table>
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
                    <td colspan="4">
                        <table class="curingTable" align="center" cellpadding="2" cellspacing="0">
                            <tr>
                                <td style="width: 30%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 30%"></td>
                            </tr>
                            <tr  style="background-color: #C3D9FF">
                                <td style="text-align:left" class="curingHMILabel">
                                    Operator Name
                                </td>
                                <td style="text-align:left" colspan="2" class="curingHMILabel">
                                    Workcenter Name
                                </td>
                                <td class="curingHMILabel">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                        </table>
                        <asp:Panel ID="curingHMIPanel" runat="server" ScrollBars="Vertical" Height="200px" CssClass="panel" >
                            <asp:GridView ID="curingHMIGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="manningID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingHMIManningIDLabel" runat="server" Text='<%# Eval("manningID") %>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Operator Name" SortExpression="ProcessName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingHMIOperatorNameLabel" runat="server" Text='<%# Eval("firstName") + " " + Eval("lastName") %>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter Name" SortExpression="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingHMIWCNameNameLabel" runat="server" Text='<%# displayWorkCenter(DataBinder.Eval(Container.DataItem,"manningID"))%>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>           
                                    <Columns>
                                        <asp:TemplateField HeaderText="" SortExpression="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%">
                                            <ItemTemplate>        
                                                <asp:Button ID="curingHMIWCLogOutButton" runat="server" CssClass="curingButton"
                                                    style="width: 70%" Text="Logout" OnClick="Button_Click" /> 
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                <PagerStyle BackColor="#507CD1" ForeColor="White" 
                                    HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="#FFFFFF" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>