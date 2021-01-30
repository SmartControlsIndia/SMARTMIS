<%@ Page Language="C#" MasterPageFile="~/Recipe/MasterPage.master"  CodeFile="PRE_VIEW.aspx.cs" Inherits="PRE_VIEW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

 
     <table width="100%" align="center">
    <tr >
        <td style="width: 100%" align="center">
           <table align="center" width="50%" style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>
                                PRESS VIEW</u></span> </h2>
                          
                    </td>
                </tr>
                <tr align="center">
                    <td align="right">
                        Press Make</td>
                    <td align="left" >
                        <asp:DropDownList ID="txtpressname" runat="server" Height="20px" Width="200px" 
                            onselectedindexchanged="txtpressname_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr align="center">
                    <td align="right">
                       </td>
                    <td align="left">
                        </td>
                </tr>
                <tr align="center">
                    <td align="right">
                       </td>
                    <td align="left" >
                    <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
            Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
        
                        </td>
                </tr>
              
            </table>
         </td>
    </tr>
</table>        
 
</asp:Content>