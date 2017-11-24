<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="SAM.Web.Controles.Proyecto.Header" %>
<div class="phc">
    <table cellpadding="0" cellspacing="0">       
        <tr>
            <td class="phcColInicial">

            </td>
            <td class="Normal tdLinea toUpper">
                <asp:Label runat="server" CssClass="Titulo" meta:resourcekey="lblProyecto" ID="lblTituloProyecto"></asp:Label>
                <asp:Label runat="server" CssClass="Label" ID="lblNombreProyecto"></asp:Label>
            </td>
            <td class="Normal tdLinea">
                <asp:Label runat="server" CssClass="Titulo" meta:resourcekey="lblColor" ID="lblTituloColor"></asp:Label>
                <asp:Panel ID="pnlColor" runat="server" CssClass="pnlColor">
                </asp:Panel>
                <asp:Label runat="server" CssClass="Label" ID="lblColorProyecto"></asp:Label>
            </td>
            <td class="Normal tdLinea">
                <asp:Label runat="server" CssClass="Titulo" meta:resourcekey="lblCliente" ID="lblTituloCliente"></asp:Label>
                <asp:Label runat="server" CssClass="Label" ID="lblClienteProyecto"></asp:Label>
            </td>
            <td class="Normal">
                <asp:Label runat="server" CssClass="Titulo"  meta:resourcekey="lblPatio" ID="lblTituloPatio"></asp:Label>
                <asp:Label runat="server" CssClass="Label" ID="lblPatioProyecto"></asp:Label>
            </td>
            <td class="phcColFinal">
            
            </td>
        </tr>
        <tr class="sombra">
            <td colspan="6">
                <br />
            </td>
        </tr>
    </table>
</div>
