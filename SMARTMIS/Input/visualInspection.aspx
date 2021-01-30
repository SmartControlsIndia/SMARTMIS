<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="visualInspection.aspx.cs" MasterPageFile="~/smartMISMaster.Master" MaintainScrollPositionOnPostback="true" Inherits="SmartMIS.visualInspection" %>

<asp:Content ID="viContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">

    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />

    <script language="javascript" type="text/javascript">

        function setID(authentication, value) 
        {
            if (authentication == "True") 
            {
                document.getElementById('<%= viIDHidden.ClientID %>').value = value.toString();
                document.getElementById('<%= viIDLabel.ClientID %>').innerHTML = value;
                revealModal('modalPage');
            }
            else 
            {
                revealModal('modalPageForAuthentication');
            }
        }

        function removeDropDownItem(id) 
        {
            var objDropDownList = id;

            for (i = objDropDownList.length - 1; i >= 0; i--) 
            {
                if (objDropDownList.options[i].value == "".trim())
                {
                    objDropDownList.options[i] = null;
                }
            }
        }
        function enableDisableReason(value) 
        {
            if (value == "OK")
            {
                document.getElementById('<%= viReviewDialogUserDecisionDropDownList.ClientID %>').disabled = true;
                document.getElementById('<%= viReviewDialogUserDecisionDropDownList.ClientID %>').value = "-";
            }
            else if (value == "Not OK") 
            {
                document.getElementById('<%= viReviewDialogUserDecisionDropDownList.ClientID %>').disabled = false;
            }

            objDropDownList = document.getElementById('<%= viReviewDialogUserDecisionDropDownList.ClientID %>')
            
            for (i = objDropDownList.length - 1; i >= 0; i--)
            {
                if (objDropDownList.options[i].value == "".trim())
                {
                    objDropDownList.options[i] = null;
                }
            }
        }
        
    </script>

    <asp:ScriptManager ID="viScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="viUpdatePanel" runat="server">
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
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Visual Inspection Input</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Work center Name :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="viWCNameDropDownList" runat="server" AutoPostBack="true" Enabled="false"
                            CssClass="masterDropDownList" Width="82%" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="viWCIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel" Visible="false"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="viWCNameFieldValidator" runat="server" 
                            ControlToValidate="viWCNameDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Workcenter Name is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">GTBarcode :</td>
                    <td class="masterTextBox">
                        <asp:TextBox ID="viGTBarcodeTextBox" runat="server" AutoComplete="off" Width="80%"></asp:TextBox>
                        <asp:Label ID="viIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="viGTBarcodeFieldValidator" runat="server" 
                            ControlToValidate="viGTBarcodeTextBox" CssClass="reqFieldValidator" 
                            ErrorMessage="GT Barcode is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Status :</td>
                    <td>
                        <asp:DropDownList ID="viStatusDropDownList" runat="server" AutoPostBack="true" 
                            OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList" Width="40%">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="errorSpan">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="viStatusFieldValidator" runat="server" 
                            ControlToValidate="viStatusDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Reason is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Defect Code :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="viFaultStatusDropDownList" runat="server" CssClass="masterDropDownList" 
                            AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" Width="82%">
                        </asp:DropDownList>
                        <asp:Label ID="viFaultStatusIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="viFaultStatusFieldValidator" runat="server" 
                            ControlToValidate="viFaultStatusDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Fault Code is Required">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="masterLabel">Defect Location :</td>
                    <td class="masterTextBox">
                        <asp:DropDownList ID="viFaultDirectionDropDownList" runat="server" Enabled="false"
                            AutoPostBack="true" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" CssClass="masterDropDownList" Width="82%">
                        </asp:DropDownList>
                    </td>
                    <td><span class="errorSpan">*</span></td>
                    <td>
                        <asp:RequiredFieldValidator ID="viFaultDirectionFieldValidator" runat="server" 
                            ControlToValidate="viFaultDirectionDropDownList" CssClass="reqFieldValidator" 
                            ErrorMessage="Fault Direction is Required">
                        </asp:RequiredFieldValidator>
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
                        <asp:Button ID="viSaveButton" runat="server" CssClass="masterButton" Text="Save" onclick="Button_Click" />&nbsp;
                        <asp:Button ID="viCancelButton" runat="server" CssClass="masterButton" Text="Cancel" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                        <asp:Button ID="viMagicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" CausesValidation="false" />
                    </td>
                    <td></td>
                    <td></td>
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
                                                     <asp:Label ID="viMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete Visualization Record."></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="viDialogOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="viDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
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
                                                    <asp:Label ID="viReviewDialogGTBarcodeLabel" runat="server" CssClass="masterLabel" Text=""></asp:Label>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">User Decision :</td>
                                                <td class="masterTextBox">
                                                    <asp:DropDownList ID="viReviewDialogUserDecisionDropDownList" runat="server" CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this)" Width="98%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td><span class="errorSpan">*</span></td>
                                            </tr>
                                            <tr>
                                                <td class="masterLabel">Reason Name :</td>
                                                <td class="masterTextBox">
                                                    <asp:DropDownList ID="viReviewDialogReasonNameDropDownList" runat="server" CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this)" Width="98%">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="viReviewDialogReasonIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                                                </td>
                                                <td><span class="errorSpan">*</span></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:HiddenField ID="viHiddenFaultStatusID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="viHiddenManningID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="viHiddenFaultDirection" runat="server" Value="" />
                                                    <asp:HiddenField ID="viHiddenUserDecision" runat="server" Value="" />
                                                    <asp:HiddenField ID="viHiddenStatus" runat="server" Value="0" />
                                                    <asp:HiddenField ID="viHiddenReasonID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="viHiddenWCID" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Button ID="viReviewDialogSaveButton" runat="server" Text="Save" CausesValidation="false" CssClass="masterButton" OnClick="Button_Click" />&nbsp;
                                                    <asp:Button ID="viReviewDialogCancelButton" runat="server" CausesValidation="false" Text="Cancel" CssClass="masterButton" OnClick="Button_Click" />
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
                        <asp:Timer ID="viNotifyTimer" runat="server" Interval="5000" Enabled="false" ontick="NotifyTimer_Tick">
                        </asp:Timer>
                        <asp:HiddenField ID="viIDHidden" runat="server" Value="0" />
                    </td>
                    <td align="center">
                        <div id="viNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="viNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="viNotifyLabel" runat="server" Text="Visualization record saved successfully."></asp:Label>
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
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">Visual Inspection Detail 
                                <asp:DropDownList ID="viStatusSearchDropDownList" runat="server" CssClass="masterDropDownList" 
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
                               <td class="gridViewHeader" style="width:15%; padding:5px">GT Barcode</td>
                               <td class="gridViewHeader" style="width:15%; padding:5px">Inspector Code</td>
                               <td class="gridViewHeader" style="width:15%; padding:5px">FaultStatus Code</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Fault Direction</td>
                               <td class="gridViewHeader" style="width:15%; padding:5px">Reason Name</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Status</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px">Date Time</td>
                               <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                           </tr>  
                        </table>
                           <asp:Panel ID="viPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                                <asp:GridView ID="viGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView_RowDataBound"
                                Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" PageSize="5" ShowHeader="False" >
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                                   
                                    <Columns>
                                         <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                             <ItemTemplate>
                                                 <asp:Label ID="viGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="85%">
                                            <ItemTemplate>
                                                
                                                <asp:GridView ID="viInnerGridView" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" CellPadding="3" ForeColor="#333333" GridLines="None" PageSize="5" ShowHeader="False" >
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                                            </ItemTemplate>
                                                      </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridHiddenGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridInspectorCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("sapCode") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Fault Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridFaultStatusIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("defectStatusID") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Fault Status Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridFaultStatusCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("faultStatusCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                     </Columns>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Fault Direction" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="viGridFaultDirectionLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("faultDirection") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridReasonIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonID") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                     </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Reason Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridReasonNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonName") %>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                     </Columns>
                                                     <Columns>
                                                         <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# displayStatus(DataBinder.Eval(Container.DataItem,"status"))%>'></asp:Label>
                                                             </ItemTemplate>
                                                         </asp:TemplateField>
                                                     </Columns>
                                                    <Columns>
                                                         <asp:TemplateField HeaderText="Date Time" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                             <ItemTemplate>
                                                                 <asp:Label ID="viGridDateTimeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("dtandTime") %>'></asp:Label>
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
                                                                            <asp:ImageButton ID="viGridEditImageButton" runat="server" Text="Edit" AlternateText="104" CausesValidation="false" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" OnClick="ImageButton_Click" Visible="false" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="viGridReviewNotReqdImageButton" runat="server" Text="Review" AlternateText="104" ToolTip="Decision Not Required" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/tick.png"  Value='<%# Eval("iD") %>' Visible='<%# displayReviewNotReqd(DataBinder.Eval(Container.DataItem,"gtbarcode"))%>'/>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="viGridReviewImageButton" runat="server" Text="Review" AlternateText="104#102" ToolTip="Review" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Review.png" Value='<%# Eval("iD") %>' Visible='<%# displayReview(DataBinder.Eval(Container.DataItem,"gtbarcode"), DataBinder.Eval(Container.DataItem,"iD"),  DataBinder.Eval(Container.DataItem,"status"))%>' OnClick="ImageButton_Click" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="viGridDeleteImageButton" runat="server" Text="Delete" AlternateText='<%# isAuthenticate("104")%>' ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:setID(this.alt, this.value); return false;" Visible='<%# displayDelete(DataBinder.Eval(Container.DataItem,"gtbarcode"), DataBinder.Eval(Container.DataItem,"iD"))%>' />
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