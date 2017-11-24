<%@ Page Language="C#" MasterPageFile="~/Masters/Publico.Master" AutoEventWireup="true" CodeBehind="RecuperaPassword.aspx.cs" Inherits="SAM.Web.RecuperaPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:PlaceHolder runat="server" ID="phControles">
<div class="contenedorCentral bordeNegroFull">
    <div>
        <div>
            <asp:Label runat="server" ID="lblTitulo" meta:resourcekey="lblTitulo" CssClass="titulo" />
        </div>
        <div style="margin:5px 0 5px 0;">
            <asp:Label runat="server" ID="lblInstrucciones" meta:resourcekey="lblInstrucciones" />
        </div>
        <div class="clear">
            <div class="divIzquierdo ancho40 bandaAzul" style="height:200px;">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtUsuario" meta:resourcekey="txtUsuario" />
                </div>
                <asp:PlaceHolder runat="server" ID="phSecreto" Visible="false">
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtRespuesta" MaxLength="110" meta:resourcekey="txtRespuesta" />
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="divIzquierdo ancho40">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p>&nbsp;</p>
        </div>
    </div>
    <div>
        <asp:Button CssClass="boton" runat="server" ID="btnSiguiente" OnClick="btnSiguiente_Click" meta:resourcekey="btnSiguiente" />
        <asp:Button CssClass="boton" runat="server" ID="btnRecupera" OnClick="btnRecupera_Click" Visible="false" meta:resourcekey="btnRecupera" />
    </div>
</div>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
    <table class="mensajeExito small" cellpadding="0" cellspacing="0">
        <tr>
            <td rowspan="2" class="icono">
                <img src="/Imagenes/Iconos/mensajeExito.png" />
            </td>
            <td class="titulo">
                <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
            </td>
        </tr>
        <tr>
            <td class="cuerpo">
                <asp:Label runat="server" ID="lblMensajeExito" meta:resourcekey="lblMensajeExito" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ligas">
                <div class="cuadroLigas">
                    <ul>
                        <li>
                            <asp:HyperLink runat="server" ID="hlLogin" NavigateUrl="~/Login.aspx">
                                <asp:Literal runat="server" ID="litLigaLogin" meta:resourcekey="litLigaLogin" />
                            </asp:HyperLink>
                        </li>
                    </ul>
                </div>  
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
</asp:Content>
