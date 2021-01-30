<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="curingOperatorPlanning.aspx.cs" MasterPageFile="~/smartMISHMIMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.curingOperatorPlanning" %>

<asp:Content ID="curingOperatorPlanningContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    
    <link rel="Stylesheet" href="../Style/curing.css" type="text/css" charset="utf-8" />
    <link rel="Stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
    
       
    
    <script language="javascript" type="text/javascript">

        function change_state(obj)
         {
             if (obj.name == "false") {
           
                //if checkbox is being checked, add a "checked" class
                obj.name = "true"
                obj.style.background = "red";
            }
            else
            {
                //else remove it
                obj.name = "false"
                obj.style.background = "#0099FF";
            }
        }


        function getWCIDFromGridView() 
        {        
            document.getElementById('<%= this.curingOperatorPlanningWCIDHidden.ClientID %>').value = "";
            var tables = document.getElementById("ctl00_masterContentPlaceHolder_curingOperatorPlanningWCNamePanel").getElementsByTagName("table")

            for (i = 0; i < tables.length; i++) {
                for (j = 0; j < tables[i].rows[0].cells.length; j++) 
                {

                    var el
                    var val = navigator.userAgent.toLowerCase();

                    if (val.indexOf("firefox") > -1) 
                    {
                        el = tables[i].rows[0].cells[j].childNodes[1];
                    }
                    else if (val.indexOf("msie") > -1) 
                    {
                        el = tables[i].rows[0].cells[j].childNodes[0];
                    }
                    //if childNode type is Button
                    if (el.name == "true") {
                        document.getElementById('<%= this.curingOperatorPlanningWCIDHidden.ClientID %>').value = document.getElementById('<%= this.curingOperatorPlanningWCIDHidden.ClientID %>').value + "#" + el.title;
                    }
                }
            }
        }

    </script>
    
<%--    <script runat="server">

        protected void ListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            String[] wcname ;            
            wcname=curingOperatorPlannedWcID.Value.Split(new char[]{','});
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HtmlInputControl CountryLabel = (HtmlInputControl)e.Item.FindControl("curingOperatorPlanningWCButton");
                     for(int i=0; i<wcname.Length; i++)
                     {
                         if (CountryLabel.Value.ToString().Equals(wcname[i].TrimStart()))
                         {
                             CountryLabel.Style.Add(HtmlTextWriterStyle.BackgroundColor, "red");
                             CountryLabel.Name = "true";

                         }
                         else
                             CountryLabel.Name = "false";
                }
            }
        }

</script>
--%>    
    <asp:ScriptManager ID="curingOperatorPlanningScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="curingOperatorPlanningUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="curingTable" align="center" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 30%"></td>
                    <td style="width: 20%"></td>
                    <td style="width: 20%"></td>
                    <td style="width: 30%"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="curingOperatorPlanningMagicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" CausesValidation="false" />
                    </td>
                    <td>
                        <asp:HiddenField id="curingOperatorPlanningWCIDHidden" runat="server" Value="" />&nbsp;
                        <asp:HiddenField id="curingOperatorPlanningManningIDHidden" runat="server" Value="" />&nbsp;
                        <asp:HiddenField id="curingOperatorPlannedWcID" runat="server" Value="" />&nbsp;

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="modalPageForWorkCenter">
                            <div class="modalBackground">
                            </div>
                            <div class="curingHMIWCModalContainer">
                                <div class="curingHMIWCModal">
                                    <div class="curingHMIWCModalTop"><a href="javascript:hideModal('modalPageForWorkCenter')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                    <div class="modalBody">
                                        <table id="aa" class="innerTable" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 100%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="curingOperatorPlanningWCNamePanel" runat="server" ScrollBars="Vertical" Height="380" CssClass="panel" >
                                                        <asp:ListView ID="curingOperatorPlanningWCGridView" runat="server" GroupItemCount="5" OnItemDataBound="ListView_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <asp:PlaceHolder ID="groupPlaceholder" runat="server"></asp:PlaceHolder>
                                                            </LayoutTemplate>
                                                            <GroupTemplate>
                                                                <table id="xyz" cellspacing="1" cellpadding="1" width="100%">
                                                                    <tr>
                                                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                                                    </tr>
                                                                </table>
                                                            </GroupTemplate>
                                                                <ItemTemplate>
                                                                    <td >                                                    
                                                                        <input id="curingOperatorPlanningWCButton" runat="server" type="button" class="curingButton"
                                                                            style="width: 80%" value='<%# Eval("name") %>' title='<%# Eval("iD") %>' 
                                                                            name="false" />
                                                                          <asp:CheckBox ID="selectwccheckbox" runat="server" />
                                                                    </td>
                                                                </ItemTemplate>
                                                        </asp:ListView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-top: 8px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="curingOperatorPlanningWCAssignButton" runat="server" CssClass="curingButton" Text="Assign Workcenter" style="width: 100%"
                                                        OnClick="Button_Click" />
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
                <td colspan="4" align="center">
                <div id="modalPageForAuthentication">
                    <div class="modalBackground">
                        </div>
                            <div class="modalContainer">
                               <div class="modal">
                                   <div class="modalTop"><a href="javascript:hideModal('modalPageForAuthentication')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                   <div class="modalBody">
                                       <table class="innerTable" cellspacing="0">
                                           <tr>
                                               <td style="width: 20%"></td>
                                               <td style="width: 80%"></td>
                                           </tr>
                                           <tr>
                                               <td class="masterLabel">
                                                   <img alt="Information" src="../Images/exclamation.png" />
                                               </td>
                                               <td>
                                                   <span class="masterWelcomeLabel">You have insufficient rights.</span>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td colspan="2">
                                                   <asp:Button ID="exclamationOKButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPageForAuthentication'); return false" Text="OK" CausesValidation="false" />&nbsp;
                                               </td>
                                           </tr>
                                       </table>
                                   </div>
                               </div>
                           </div>
                       </div>
                
                    <asp:RadioButton ID="A_Shift" runat="server" AutoPostBack="true" CssClass="gridViewButtons" 
                        GroupName="aa" oncheckedchanged="Shift_CheckedChanged"  Text="A SHIFT" />
                    <asp:RadioButton ID="B_Shift" runat="server" AutoPostBack="true" oncheckedchanged="Shift_CheckedChanged" CssClass="gridViewButtons" 
                        GroupName="aa" Text="B SHIFT" />
                    <asp:RadioButton ID="C_Shift" runat="server" AutoPostBack="true" oncheckedchanged="Shift_CheckedChanged" CssClass="gridViewButtons" 
                        GroupName="aa" Text="C SHIFT" />
                
                </td> 
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="curingTable" align="center" cellpadding="2" cellspacing="0">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 45%"></td>
                                <td style="width: 13%"></td>
                            </tr>
                            <tr  style="background-color: #C3D9FF">
                                <td style="text-align: Center" class="curingHMILabel">
                                    E-Code
                                </td>
                                <td style="text-align: Left" class="curingHMILabel">
                                    Operator Name
                                </td>
                                <td style="text-align: left" class="curingHMILabel">
                                    Workcenter Name
                                    </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                            </tr>
                        </table>
                        <asp:Panel ID="curingOperatorPlanningPanel" runat="server" ScrollBars="Vertical" Height="300px" CssClass="panel" >
                            <asp:GridView ID="curingOperatorPlanningGridView" runat="server" AutoGenerateColumns="False"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None"
                                AllowPaging="false" AllowSorting="false" PageSize="5" ShowHeader="false"  >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="manningID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingOperatorPlanningManningIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="manningID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingOperatorPlanningsapCodeLabel" runat="server" Text='<%# Eval("sapCode") %>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Operator Name" SortExpression="ProcessName" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Button ID="curingOperatorPlanningOperatorNameButton" runat="server" Text='<%# Eval("firstName") + " " + Eval("lastName") %>' style="width: 70%"
                                                    CssClass="curingButton" OnClick="Button_Click"></asp:Button>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Workcenter Name" SortExpression="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:Label ID="curingOperatorPlanningWCNameNameLabel" runat="server" Text='<%# displayWorkCenter(DataBinder.Eval(Container.DataItem,"iD"))%>' CssClass="curingHMILabel"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>           
                                    <Columns>
                                        <asp:TemplateField HeaderText="" SortExpression="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="13%">
                                            <ItemTemplate>        
                                                <asp:Button ID="curingOperatorPlanningWCLogOutButton" runat="server" CssClass="curingButton"
                                                    style="width: 100%" Text="Add/Remove WC" OnClick="Button_Click" /> 
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