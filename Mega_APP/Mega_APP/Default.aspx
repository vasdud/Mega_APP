<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Mega_APP._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="Div1" class="contextMenu">
    <table style='width:100%;'>
    <tr><td onclick="fnView();">View Product</td></tr>
    <tr><td onclick="fnDelete();">Delete Product</td></tr>
    <tr>
    <td width = 20% align = left>
    <p>
        <asp:TreeView ID="TreeV" runat="server" 
            NodeIndent="15" ExpandDepth="0" 
            onselectednodechanged="TreeV_SelectedNodeChanged">
            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
            <ParentNodeStyle Font-Bold="False" />
            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                VerticalPadding="0px" />
        </asp:TreeView>
    </td>
    <td width = 80% align = center>
        <asp:GridView ID="GView" runat="server" AllowSorting="True">
        </asp:GridView>
    </td>
    </p>
    </tr>
    </table>
    </div>
</asp:Content>
