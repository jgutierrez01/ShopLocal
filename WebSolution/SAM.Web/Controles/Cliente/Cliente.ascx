<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Cliente.ascx.cs" Inherits="SAM.Web.Controles.Cliente.Cliente" %>
<div class="divIzquierdo ancho70">
    <div class="separador">
        <mimo:RequiredLabeledTextBox ID="txtNombreCliente" runat="server" EntityPropertyName="Nombre"
            MaxLength="50" meta:resourcekey="lblNombre" />
    </div>
    <div class="separador">
        <mimo:LabeledTextBox ID="txtDireccion" runat="server" EntityPropertyName="Direccion"
            MaxLength="50" meta:resourcekey="lblDireccion" />
    </div>
    <div class="separador">
        <mimo:LabeledTextBox ID="txtCiudad" runat="server" EntityPropertyName="Ciudad" MaxLength="50"
            meta:resourcekey="lblCiudad" />
    </div>
    <div class="separador">
        <mimo:LabeledTextBox ID="txtEstado" runat="server" EntityPropertyName="Estado" MaxLength="50"
            meta:resourcekey="lblEstado" />
    </div>
    <div class="separador">
        <mimo:LabeledTextBox ID="txtPais" runat="server" EntityPropertyName="Pais" MaxLength="50"
            meta:resourcekey="lblPais" />
    </div>
</div>
<div class="divDerecho ancho30">
    <div class="validacionesRecuadro" style="margin-top: 23px;">
        <div class="validacionesHeader">
        </div>
        <div class="validacionesMain">
            <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true"
                DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
        </div>
    </div>
</div>
