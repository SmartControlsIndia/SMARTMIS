<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uniformityBalancing.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.Input.uniformityBalancing" %>

<asp:Content ID="uniBalContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script language="javascript" type="text/javascript">

        function setID(authentication, value)
         {
            if (authentication == "True") {
                document.getElementById('<%= uniBalIDHidden.ClientID %>').value = value.toString();
                revealModal('modalPage');
            }
            else {
                revealModal('modalPageForAuthentication');
            }
        }

        function removeDropDownItem(id) {
            var objDropDownList = id;

            for (i = objDropDownList.length - 1; i >= 0; i--) {
                if (objDropDownList.options[i].value == "".trim()) {
                    objDropDownList.options[i] = null;
                }
            }
        }
        function enableDisableReason(value) {
            if (value == "OK") {
                document.getElementById('<%= uniBalReviewDialogUserDecisionDropDownList.ClientID %>').disabled = true;
                document.getElementById('<%= uniBalReviewDialogUserDecisionDropDownList.ClientID %>').value = "-";
            }
            else if (value == "Not OK") {
                document.getElementById('<%= uniBalReviewDialogUserDecisionDropDownList.ClientID %>').disabled = false;
            }

            objDropDownList = document.getElementById('<%= uniBalReviewDialogUserDecisionDropDownList.ClientID %>')

            for (i = objDropDownList.length - 1; i >= 0; i--) {
                if (objDropDownList.options[i].value == "".trim()) {
                    objDropDownList.options[i] = null;
                }
            }
        }        
    </script>
    
    <asp:ScriptManager ID="uniBalScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uniBalUpdatePanel" runat="server">
        <ContentTemplate>
            <table class="inputTable" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="inputFirstCol"></td>
                    <td class="inputSecondCol"></td>
                    <td class="inputThirdCol"></td>
                    <td class="inputForthCol"></td>
                </tr>
                <tr>
                    <td colspan="4">
                       <div id="modalPage">
                           <div class="modalBackground">
                           </div>
                            <div class="modalContainer">
                               <div class="modal">
                                   <div class="modalTop"><a href="javascript:hideModal('modalPage')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                   <div class="modalBody">
                                       <table class="innerTable" cellspacing="0">
                                           <tr>
                                               <td style="width: 20%"></td>
                                               <td style="width: 80%"></td>
                                           </tr>
                                           <tr>
                                               <td class="masterLabel">
                                                   <img alt="Close" src="../Images/question.png" />
                                               </td>
                                               <td>
                                                   <asp:Label ID="uniBalMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Uniformity Record."></asp:Label>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td colspan="2">
                                                   <asp:Button ID="uniBalDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                   <asp:Button ID="uniBalDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                               </td>
                                           </tr>
                                       </table>
                                   </div>
                               </div>
                           </div>
                       </div>
                       <div id="modalPageForReview">
                             <div class="modalBackground">
                             </div>
                              <div class="modalContainer">
                                 <div class="modal">
                                     <div class="modalTop"><a href="javascript:hideModal('modalPageForReview')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                     <div class="modalBody">
                                         <table class="innerTable" cellspacing="0">
                                            <tr>
                                                <td class="inputFirstCol"></td>
                                                <td class="inputSecondCol"></td>
                                                <td class="inputThirdCol"></td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">GTBarcode :</td>
                                                <td class="masterTextBox">
                                                    <asp:Label ID="uniBalReviewDialogGTBarcodeLabel" runat="server" CssClass="masterLabel" Text=""></asp:Label>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">User Decision :</td>
                                                <td class="masterTextBox">
                                                    <asp:DropDownList ID="uniBalReviewDialogUserDecisionDropDownList" runat="server" CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this)" Width="98%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td><span class="errorSpan">*</span></td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">Action :</td>
                                                <td class="masterTextBox">
                                                    <asp:TextBox ID="uniBalReviewDialogActionTextBox" runat="server" CssClass="masterTextBox" TextMode="MultiLine" 
                                                        Width="96%" Rows="1">
                                                    </asp:TextBox>
                                                </td>
                                                <td><span class="errorSpan">*</span></td>
                                            </tr>                                          
                                            <tr>
                                                <td colspan="3">
                                                    <asp:HiddenField ID="uniBalHiddenRecipeCode" runat="server" Value="" />
                                                    <asp:HiddenField ID="uniBalHiddenFaultStatusID" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Button ID="uniBalReviewDialogSaveButton" runat="server" Text="Save" CausesValidation="false" CssClass="masterButton" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="uniBalReviewDialogCancelButton" runat="server" CausesValidation="false" Text="Cancel" CssClass="masterButton" OnClick="Button_Click" />
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
                        <asp:Timer ID="uniBalNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                            ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        <asp:HiddenField ID="uniBalIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center" colspan ="2">
                        <div id="uniBalNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table class="innerTable">
                                <tr>
                                    <td class="inputThirdCol">
                                    </td>
                                    <td>
                                        <img id="uniBalNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label id="uniBalNotifyLabel" runat="server" Text="Uniformity and Balancing record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="uniBalWCIDLabel" runat="server" Text="86" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                        <asp:Label ID="uniBalIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                        <asp:Button ID="uniBalMagicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" CausesValidation="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Uniformity &amp; Balancing :&nbsp;
                                <asp:DropDownList ID="uniBalStatusSearchDropDownList" runat="server" CssClass="masterDropDownList" 
                                    AutoPostBack="true" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="10%">
                                </asp:DropDownList>
                            </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                            <tr>
                                <td class="gridViewHeader" style="width:20%; padding:5px">GT Barcode</td>
                                <td class="gridViewHeader" style="width:20%; padding:5px">Recipe Code</td>
                                <td class="gridViewHeader" style="width:10%; padding:5px">Inspector Code</td>
                                <td class="gridViewHeader" style="width:20%; padding:5px">Action</td>
                                <td class="gridViewHeader" style="width:10%; padding:5px">Status</td>
                                <td class="gridViewHeader" style="width:10%; padding:5px">Date Time</td> 
                                <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="uniBalPanel" runat="server" ScrollBars="Vertical" Height="295px" CssClass="panel" >
                            <asp:GridView ID="uniBalGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView_RowDataBound"
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#FFFFFF" ForeColor="#333333" />                                   
                                <Columns>
                                    <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="uniBalGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="uniBalGridRecipeCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("recipeCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>         
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="60%">
                                        <ItemTemplate>
                                            <asp:GridView ID="uniBalInnerGridView" runat="server" AutoGenerateColumns="False"
                                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" PageSize="5" ShowHeader="False" >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridHiddenGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Recipe Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridHiddenRecipeCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("recipeCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridInspectorCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("sapCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridReasonNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("action") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# displayStatus(DataBinder.Eval(Container.DataItem,"status"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>  
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date Time" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="uniBalGridDateTimeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("dtandTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>                  
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 25%"></td>
                                                                    <td style="width: 25%"></td>
                                                                    <td style="width: 25%"></td>
                                                                    <td style="width: 25%"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="uniBalGridEditImageButton" runat="server" Text="Edit" AlternateText="106" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" Visible="false" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="uniBalGridReviewNotReqdImageButton" runat="server" Text="Review" AlternateText="102" ToolTip="Decision Not Required" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/tick.png"  Value='<%# Eval("iD") %>' Visible='<%# displayReviewNotReqd(DataBinder.Eval(Container.DataItem,"gtbarcode"))%>'/>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="uniBalGridReviewImageButton" runat="server" Text="Review" AlternateText="102" ToolTip="User Decision" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Review.png" Value='<%# Eval("iD") %>' Visible='<%# displayReview(DataBinder.Eval(Container.DataItem,"gtbarcode"), DataBinder.Eval(Container.DataItem,"iD"),  DataBinder.Eval(Container.DataItem,"status"))%>'  OnClick="ImageButton_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="uniBalGridDeleteImageButton" runat="server" Text="Delete" AlternateText='<%# isAuthenticate("102")%>' ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:setID(this.alt, this.value); return false;" Visible='<%# displayDelete(DataBinder.Eval(Container.DataItem,"gtbarcode"), DataBinder.Eval(Container.DataItem,"iD"))%>' />
                                                                    </td>
                                                                </tr>
                                                            </table>
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
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>