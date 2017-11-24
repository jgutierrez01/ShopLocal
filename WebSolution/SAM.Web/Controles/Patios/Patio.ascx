<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patio.ascx.cs" Inherits="SAM.Web.Controles.Patios.Patio" %>
<div class="divIzquierdo ancho70">
    <div class="separador">
        <mimo:RequiredLabeledTextBox ID="txtNombre" runat="server" EntityPropertyName="Nombre"
            MaxLength="50" meta:resourcekey="lblNombre" />
    </div>
    <div class="separador">
        <mimo:RequiredLabeledTextBox ID="txtPropietario" runat="server" EntityPropertyName="Propietario"
            MaxLength="50" meta:resourcekey="lblPopietario" />
    </div>
    <div class="separador">
        <mimo:LabeledTextBox ID="txtDescripcion" runat="server" EntityPropertyName="Descripcion"
            MaxLength="50" meta:resourcekey="lblDescripcion" />
    </div>
</div>
<div class="divIzquierdo ancho30">
    <div class="validacionesRecuadro" style="margin-top: 23px;">
        <div class="validacionesHeader">
        </div>
        <div class="validacionesMain">
            <asp:ValidationSummary runat="server" ID="valSummary" EnableClienteScript="true"
                DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
        </div>
    </div>
</div>
