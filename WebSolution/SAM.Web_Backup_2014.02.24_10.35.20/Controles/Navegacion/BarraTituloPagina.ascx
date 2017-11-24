<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BarraTituloPagina.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.BarraTituloPagina" %>
<div class="paginaHeader">
    <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" />
    <asp:HyperLink runat="server" ID="hlRegresar" ImageUrl="~/Imagenes/Iconos/btn_regresar.png" CssClass="regresar" meta:resourcekey="hlRegresar" />
    <asp:HyperLink runat="server" ID="hlRegTxt" CssClass="rgTxt" meta:resourcekey="hlRegTxt" />
</div>

