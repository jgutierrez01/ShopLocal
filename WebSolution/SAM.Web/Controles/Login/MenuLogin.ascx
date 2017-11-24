<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuLogin.ascx.cs" Inherits="SAM.Web.Controles.Login.MenuLogin" %>
<div class="menuSuperior">
     <asp:Panel runat="server" ID="pnLogin" CssClass="Inicial">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Login.aspx" ID="hlLogin" meta:resourcekey="hlLogin"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnRecuperar" CssClass="ElementoEspecial" style="width:150px;" >
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/RecuperaPassword.aspx" ID="hlRecuperar" meta:resourcekey="hlRecuperar"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnRellenado" CssClass="ElementoEspecial" style="width:100;">
        <span>&nbsp;&nbsp;&nbsp;</span>
    </asp:Panel>
    <div class="Cierre">
    </div>
</div>