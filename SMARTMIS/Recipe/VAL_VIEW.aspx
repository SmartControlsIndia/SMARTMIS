﻿<%@ Page Language="C#" MasterPageFile ="~/Recipe/MasterPage.master"  AutoEventWireup="true" CodeFile="VAL_VIEW.aspx.cs" Inherits="VAL_VIEW" %>


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
           <table align="center" width="50%"  style="background-color:White;">
                <tr align="center" >
                    <td colspan="2" bgcolor="#FFCCFF">
                            <h2 style="font-family:'Courier New', Courier, monospace; width:100%; height: 35px;" align="center"><span><u>
                                VALVE NEW</u></span> </h2>
                          
                    </td>
                </tr>   

                <tr align="center">
                   
                    <td align="right" >
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Valve Name</td>
                    <td align="left" >
                        <asp:TextBox ID="Txvalvesname" runat="server" Height="20px" Width="200px"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr align="center">
                   
                    <td align="right">
                        </td>
                    <td align="left" >
                        </td>
                    
                </tr>
            <tr align="center">
                    
                    <td align="right">
                        </td>
                    <td align="left" >
                        <asp:Button ID="btnnew" runat="server" onclick="btnnew_Click1" Text="SAVE" 
                            Width="106px" />
                    </td>
                    
                </tr>
            
            </table>
       
          <table align="center" frame="below" 
              style="width: 50%; background-color:White;">
              <tr align="center">
                  <td align="center" >
                  <asp:Label ID="Label19" align="center"  runat="server" Width="320px" Font-Bold="True" 
            Font-Strikeout="False" ForeColor="#CC0000" Font-Size="Small" ></asp:Label>
        
                      </td>
                  
              </tr>
              
          </table>
 
 
 </td>
 </tr>
 </table>
 </asp:Content>