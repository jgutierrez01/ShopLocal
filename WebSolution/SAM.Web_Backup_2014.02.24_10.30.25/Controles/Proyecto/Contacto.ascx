<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contacto.ascx.cs" Inherits="SAM.Web.Controles.Proyecto.Contacto" %>
<div class="divIzquierdo ancho70">
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtNombre" meta:resourcekey="txtNombre" MaxLength="150" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtApellidoPat" meta:resourcekey="txtApellidoPat" MaxLength="150" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtApellidoMat" meta:resourcekey="txtApellidoMat" MaxLength="150" />
        </div>
    </div>
    <div class="divIzquierdo ancho45">
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtCorreo" meta:resourcekey="txtCorreo" MaxLength="50" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtTelefonoOficina" meta:resourcekey="txtTelefonoOficina" MaxLength="50" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtTelefonoParticular" meta:resourcekey="txtTelefonoParticular" MaxLength="50"/>
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtCelular" meta:resourcekey="txtCelular" />
        </div>
    </div>
    <p></p>
</div>
<div class="divIzquierdo ancho30">
    <div class="validacionesRecuadro">
        <div class="validacionesHeader"></div>
        <div class="validacionesMain">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
        </div>
    </div>
</div>
<p>
</p>
