<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/smartMISMaster.Master" CodeBehind="userDecision.aspx.cs" Inherits="SmartMIS.Input.userDecision" %>


<asp:Content ID="xRayContent" runat="server" ContentPlaceHolderID="masterContentPlaceHolder">
    <link rel="stylesheet" href="../Style/input.css" type="text/css" charset="utf-8" />
    <link rel="SHORTCUT ICON" href="../Images/favicon.ico" />
<link rel="stylesheet" href="../Style/masterPage.css" type="text/css" charset="utf-8" />

    <script language="javascript" type="text/javascript">

        function setID(value)
        {
            document.getElementById('<%= udIDHidden.ClientID %>').value = value.toString();
            document.getElementById('<%= udIDLabel.ClientID %>').innerHTML = value;
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
            if (value == "OK") {
                document.getElementById('<%= modalStatusDropDownList.ClientID %>').disabled = false;
            }
            else if (value == "Not OK") 
            {
                document.getElementById('<%= modalStatusDropDownList.ClientID %>').disabled = false;
            }

            objDropDownList = document.getElementById('<%= modalStatusDropDownList.ClientID %>')
            
            for (i = objDropDownList.length - 1; i >= 0; i--)
            {
                if (objDropDownList.options[i].value == "".trim())
                {
                    objDropDownList.options[i] = null;
                }
            }
        }
        
    </script>
    
    <asp:ScriptManager ID="xRayScriptManager" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="xRayUpdatePanel" runat="server">
        
        <ContentTemplate>        
            <table class="inputTable" align="center" cellpadding="0" cellspacing="0">                           
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
                                                     <asp:Label ID="udMessageLabel" runat="server" CssClass="masterWelcomeLabel" Text="Do you want to delete the Record."></asp:Label>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td colspan="2">
                                                     <asp:Button ID="udDialogDelOKButton" runat="server" CssClass="masterButton" Text="OK" CausesValidation="false" OnClick="Button_Click" />&nbsp;
                                                     <asp:Button ID="udDialogDelCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('modalPage'); return false" Text="Cancel" />
                                                 </td>
                                             </tr>
                                         </table>
                                     </div>
                                 </div>
                             </div>
                         </div>
                       
                       <div id="ModalPage2">
                           <div class="modalBackground">
                           </div>
                            <div class="modalContainer">
                               <div class="modal">
                                   <div class="modalTop">                                       
                                   <a href="javascript:hideModal('ModalPage2')"><img alt="Close" src="../Images/cancel.png" class="closeImg" /></a></div>
                                   <div class="modalBody">
                                       <table class="innerTable" cellspacing="0">
                                           <tr>
                                               <td style="width: 20%"></td>
                                               <td style="width: 80%"></td>
                                           </tr>
                                           <tr>
                                             
                                               <td class="masterLabel">
                                                   <asp:Label ID="Label3" runat="server" CssClass="masterWelcomeLabel" Text="GTBarCode:"></asp:Label>
                                               </td>
                                                <td class="masterTextBox">
                                                    <asp:Label ID="modalGTBarCodeLabel" runat="server" CssClass="masterLabel" Text=""></asp:Label>
                                                </td>
                                               <td>
                                                         <asp:Label ID="Label4" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                                               </td>
                                           </tr> 
                                                    <tr>                                             
                                               <td class="masterlabel">
                                                   <asp:Label ID="Label2" runat="server" CssClass="masterWelcomeLabel"  Text="Status:"></asp:Label>
                                               </td>
                                                 <td class="masterTextBox">
                                                     <asp:DropDownList ID="modalStatusDropDownList"  Width=50% CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this); javascript:enableDisableReason(this.value);" OnSelectedIndexChanged="DropDown_IndexChanged" runat="server">
                                                     </asp:DropDownList>
                                               </td>
                                                <td><span class="errorSpan">*</span></td>
                                           </tr>  
                                           <tr>
                                             
                                               <td class="masterLabel">
                                                   <asp:Label ID="Label1" runat="server" CssClass="masterWelcomeLabel" Text="Reason:"></asp:Label>
                                               </td>
                                                 <td class="masterTextBox">
                                                     <asp:DropDownList ID="modalReasonDropDownList" CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this)" OnSelectedIndexChanged="DropDown_IndexChanged" Width=50% runat="server">
                                                     </asp:DropDownList>
                                               </td>
                                                <td><span class="errorSpan">*</span></td>
                                               <td>
                                                         <asp:Label ID="udReasonIDLabel" runat="server" Text="0" CssClass="masterHiddenLabel"></asp:Label>
                                               </td>
                                           </tr> 
                                           
                                                <tr>
                                                <td class="masterLabel">User Decision :</td>
                                                <td class="masterTextBox">
                                                    <asp:DropDownList ID="ModalUserDecisionDropDownList" runat="server" CssClass="masterDropDownList" 
                                                        AutoPostBack="false" onChange="javascript:removeDropDownItem(this);" Width="50%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td><span class="errorSpan">*</span></td>
                                            </tr>                                         
                                                                  
                                           
                                           <tr>
                                               <td colspan="2">
                                                   <asp:Button ID="udDialogOKButton" runat="server" CssClass="masterButton" Text="OK" OnClick="Button_Click" CausesValidation="false" />&nbsp;
                                                   <asp:Button ID="udDialogCancelButton" runat="server" CssClass="masterButton" OnClientClick="javascript:hideModal('ModalPage2'); return false" Text="Cancel" />
                                               </td>
                                           </tr>
                                       </table>
                                   </div>
                               </div>
                           </div>
                       </div>               
                        <asp:Label ID="udIDLabel" runat="server" CssClass="masterHiddenLabel" Text="Label"></asp:Label>                    
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Timer ID="viNotifyTimer" runat="server" Interval="5000" Enabled="false" 
                            ontick="xRayNotifyTimer_Tick"   >
                        </asp:Timer>
                        <asp:HiddenField ID="udIDHidden" runat="server" Value="0" />
                         
                        
                    </td>
                         <td align="center">
                        <div id="viNotifyMessageDiv" runat="server" visible="false" class="notifyMessageDiv">
                            <table>
                                <tr>
                                    <td>
                                        <img id="viNotifyImage" runat="server" alt="notify" class="notifyImg" src="../Images/notifyCircle.png" />
                                    </td>
                                    <td>
                                        <asp:Label id="viNotifyLabel" runat="server" Text="User Decision Record saved successfully."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td align="center">
                        &nbsp;</td>
                    <td>
                     <asp:Button ID="viMagicButton" runat="server" CssClass="masterHiddenButton" Text="Magic" CausesValidation="false" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div class="masterHeader">
                            <p class="masterHeaderTagline">
                                UserDecision Detail for X-Ray&nbsp;
                                <asp:DropDownList ID="udStatusChangeDropDown" runat="server" 
                                    AutoPostBack="True" CssClass="masterDropDownList" 
                                    onselectedindexchanged="DropDown_IndexChanged">
                                </asp:DropDownList>
                            </p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table class="innerTable" cellspacing="0">
                            <tr>                                
                                <td class="gridViewHeader" style="width:20%; padding:5px">GT Barcode</td>
                                <td class="gridViewHeader" style="width:15%; padding:5px">FaultStatus Code</td>
                                <td class="gridViewHeader" style="width:15%; padding:5px">Reason Name</td>                                
                                <td class="gridViewHeader" style="width:10%; padding:5px">Status</td>
                                <td class="gridViewHeader" style="width:10%; padding:5px"></td>
                        </tr>
                        </table>
                        <asp:Panel ID="xRayPanel" runat="server" ScrollBars="Vertical" Height="195px" CssClass="panel" >
                            <asp:GridView ID="userDecisionGridView" runat="server" AutoGenerateColumns="False"
                            Width="100%" CellPadding="3" ForeColor="#333333" GridLines="Horizontal" 
                                PageSize="5" ShowHeader="False" >
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />                          
                           
                               <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="udGridIDLabel" runat="server" Text='<%# Eval("iD") %>' CssClass="gridViewItems"></asp:Label>
                                            </ItemTemplate>
                                      </asp:TemplateField>
                                    </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Workcenter ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridWCIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Workcenter Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridWCNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("wcName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="GT Barcode" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridGTBarcodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("gtBarcode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Fault Status ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridFaultStatusIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("faultStatusID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            
                                <Columns>
                                    <asp:TemplateField HeaderText="Reason ID" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridReasonIDLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Reason Name" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridReasonNameLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("reasonName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Inspector Code" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="udGridInspectorCodeLabel" runat="server" CssClass="gridViewItems" Text='<%# Eval("manningID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="udStatusLabel" runat="server" CssClass="gridViewItems" Text='<%# displayStatus(DataBinder.Eval(Container.DataItem,"status"))%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                               
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gridViewHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                       
                                            <asp:ImageButton ID="udGridEditImageButton" runat="server" Text="Edit" CausesValidation="false" ImageAlign="Left" ToolTip="Edit" CssClass="gridImageButton" ImageUrl="~/Images/Edit.gif" Visible="false"  />
                                            <asp:ImageButton ID="udGridDeleteImageButton" runat="server" Text="Delete" ImageAlign="Right" ToolTip="Delete" CausesValidation="false" CssClass="gridImageButton" ImageUrl="~/Images/Delete.gif" Value='<%# Eval("iD") %>' OnClientClick="javascript:revealModal('modalPage'); javascript:setID(this.value); return false;"   />
                                             <asp:ImageButton ID="udGridDecisionImageButton" runat="server" Text="Decision" ImageAlign="Right" CausesValidation="false" ToolTip="Decision" OnClick="ImageButton_Click" Visible='<%# displayReview(DataBinder.Eval(Container.DataItem,"gtbarcode"), DataBinder.Eval(Container.DataItem,"status"))%>' Value='<%# Eval("iD") %>'  CssClass="gridImageButton" ImageUrl="~/Images/Plussign.gif" />
                                            
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
</asp:Content>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               