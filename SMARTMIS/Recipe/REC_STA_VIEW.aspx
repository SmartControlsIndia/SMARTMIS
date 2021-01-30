<%@ Page Language="C#"  MasterPageFile="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="REC_STA_VIEW.aspx.cs" Inherits="REC_STA_VIEW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">


        .style3
        {
            width: 287px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="35%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>RECIPE STRUCTURE VIEW</u></span> </h2>
                          
                    </td>
                </tr>   
                <tr align="center">
                    <td align="left" >
                        Press Make</td>
                    <td align="right" >
                        <asp:DropDownList ID="txtpressname" runat="server" AutoPostBack="True" 
                            Height="20px" onselectedindexchanged="txtpressname_SelectedIndexChanged" 
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" >
                        Step</td>
                    <td align="right" >
                        <asp:DropDownList ID="txtstep" runat="server" Height="20px" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="left" >
                        Valves</td>
                    <td align="right" >
                        <asp:DropDownList ID="txtvalve" runat="server" AutoPostBack="True" 
                            Height="20px" onselectedindexchanged="txtvalve_SelectedIndexChanged" 
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
     
    
        <br />
    
        <table align="center" width="90%" frame="box" style="border-style: outset">
            <tr align="center">
                <td align="left" >
                    VALVE -1</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve1" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -5</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve5" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -9</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve9" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -13</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve13" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
            
            </tr>
            <tr align="center">
                <td align="left" >
                    VALVE -2</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve2" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -6</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve6" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -10</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve10" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                 <td align="left" >
                    VALVE -14</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve14" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
             
            </tr>
           <tr align="center">
                <td align="left" >
                    VALVE -3</td>
                <td align="left" >
                    <asp:DropDownList ID="txtvalve3" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                </td>
                <td align="left" >
                    VALVE -7</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve7" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -11</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve11" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                 <td align="left" >
                    VALVE -15</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve15" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                
               
            </tr>
            <tr align="center">
                <td align="left" >
                    VALVE -4</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve4" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -8</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve8" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                <td align="left" >
                    VALVE -12</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve12" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
                  <td align="left" >
                    VALVE -16</td>
                <td align="left" >
                    
                    <asp:DropDownList ID="txtvalve16" runat="server" Height="16px" Width="182px">
                    </asp:DropDownList>
                    
                </td>
              
            </tr>
            
        </table>
    </td>
         </tr>
     </table>
    
</asp:Content>