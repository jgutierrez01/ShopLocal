<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SteelgoMenu.ascx.cs" Inherits="SAM.Web.Controles.Login.SteelgoMenu" %>
<div class="menuSteelgo">
    <div class="divIzquierdo ancho25 barraIzquierda">
        <asp:HyperLink runat="server" ID="hlContacto" meta:resourcekey="hlContacto" Target="_blank" />
    </div>
    <div class="divIzquierdo ancho25 barraIzquierda">
        <asp:HyperLink runat="server" ID="hlQuienesSomos" meta:resourcekey="hlQuienesSomos" Target="_blank" />
    </div>
    <div class="divDerecho ancho25">
        <asp:HyperLink ID="hlSteelgo" runat="server" ImageUrl="~/Imagenes/Logos/steelgoSmall.jpg" Width="78" Height="25" Target="_blank" />
    </div>
    <div class="divDerecho ancho25 barraIzquierda">
        <asp:HyperLink runat="server" ID="hlServicios" meta:resourcekey="hlServicios" Target="_blank" />
    </div>
</div>