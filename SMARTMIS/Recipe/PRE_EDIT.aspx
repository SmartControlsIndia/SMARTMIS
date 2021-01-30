<%@ Page Language="C#" MasterPageFile="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="PRE_EDIT.aspx.cs" Inherits="PRE_EDIT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 287px;
        }
        .style5
        {
            width: 126px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    


    <table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="50%" style="background-color:White;">
                        <tr align="center" >
                            <td colspan="2" bgcolor="#FFCCFF">
                                    <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>PRESS EDIT</u></span> </h2>
                                  
                            </td>
                        </tr>
                        <tr>
                            
                        
                            <td align="right">Press Make</td>
                            <td align="left" >
                                <asp:DropDownList ID="txtpressname" runat="server" Height="20px" Width="200px" 
                                    AutoPostBack="True" onselectedindexchanged="txtpressname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                         </tr>
               
                        <tr align="center">
                            <td align="right">
                                &nbsp;</td>
                            <td align="left" >
                                <asp:TextBox ID="TextBox1" runat="server" Height="20px" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right">
                                &nbsp;</td>
                            <td align="left" >
                                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                                    Text="EDIT PRESS" Width="126px" />
                            </td>
                        </tr>
                         <tr align="center">
                          <td align="center" colspan="2">
                            <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
                    Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
                
                              </td>
                          
                      </tr>
              </table>
        </td>
      </tr>
  </table>
      
     </asp:Content>