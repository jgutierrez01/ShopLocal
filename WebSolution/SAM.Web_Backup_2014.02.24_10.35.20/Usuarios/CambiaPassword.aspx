<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="CambiaPassword.aspx.cs" Inherits="SAM.Web.Usuarios.CambiaPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:PlaceHolder runat="server" ID="phControles">
        <div style="width:500px;">
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </h4>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtPasswordActual" MaxLength="20" TextMode="Password" meta:resourcekey="lblPasswordActual" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtPasswordNuevo" MaxLength="20" TextMode="Password" meta:resourcekey="lblPasswordNuevo" />
                </div>
                <div class="notaPassword" style="width:240px;">
                    <asp:Literal runat="server" ID="litMsgContrasena" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtConfPassword" MaxLength="20" TextMode="Password" meta:resourcekey="lblConfPassword" />
                    <asp:CompareValidator runat="server" ID="cmpPassword" Display="None" ControlToValidate="txtPasswordNuevo" ControlToCompare="txtConfPassword" meta:resourcekey="cmpPassword" />
                    <asp:RegularExpressionValidator runat="server" ID="regPassword" Display="None" ControlToValidate="txtPasswordNuevo" meta:resourcekey="lblregPassword" />
                </div>
            </div>
            <div class="divIzquierdo ancho50">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
        <div class="separador">
            <asp:Button ID="btnCambiar" CssClass="boton" runat="server" OnClick="btnCambiar_Click" meta:resourcekey="btnCambiar" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin:20px auto 0 auto;">
            <tr>
                <td rowspan="2" class="icono">
                    <img src="/Imagenes/Iconos/mensajeExito.png" />
                </td>
                <td class="titulo">
                    <asp:Label runat="server" ID="lblTituloExito" Text="Conceptos Agregados" meta:resourcekey="lblTituloExito" />
                </td>
            </tr>
            <tr>
                <td class="cuerpo">
                    <asp:Label runat="server" ID="lblMensajeExito" Text="los conceptos se agregaron exitosamente" meta:resourcekey="lblMensajeExito" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
