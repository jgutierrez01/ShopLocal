<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Certificado.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.Certificado" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho100">
        <div class="separador">            
            <asp:CheckBox ID="CertificadoAprobado" runat="server" CssClass="checkYTexto" meta:resourcekey="certificado" ></asp:CheckBox>
        </div>
    </div>

    <div class="divIzquierdo ancho100">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaCertificado" />
            <asp:TextBox ID="CertificadoFecha" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p>
</div>