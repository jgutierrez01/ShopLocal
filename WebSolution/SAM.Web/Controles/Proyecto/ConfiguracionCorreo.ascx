<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionCorreo.ascx.cs"
     Inherits="SAM.Web.Controles.Proyecto.ConfiguracionCorreo" %>

<div class="divIzquierdo ancho70">
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtNombreCorreo" meta:resourcekey="txtNombreCorreo" />
        </div>
    </div>
    <div class="divIzquierdo ancho45">
        <div class="separador">
             <asp:Label runat="server" ID="lblInformacion" CssClass="bold" meta:resourcekey="lblInformacion" />
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
