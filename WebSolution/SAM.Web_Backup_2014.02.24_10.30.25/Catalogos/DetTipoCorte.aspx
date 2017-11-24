<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetTipoCorte.aspx.cs" Inherits="SAM.Web.Catalogos.DetTipoCorte" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetTipoCorte" NavigateUrl="~/Catalogos/LstTipoCorte.aspx" />

    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtCodigo" EntityPropertyName="Codigo" ErrorMessage="El código es requerido" 
                                                 Label="Código:" meta:resourcekey="txtCodigo"/>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre" Label="Nombre:" meta:resourcekey="txtNombre"/>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtDescripcion" EntityPropertyName="Descripcion" Label="Descripción:" meta:resourcekey="txtDescripcion"/>
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox runat="server" ID="chkCalidad" EntityPropertyName="VerificadoPorCalidad" CssClass="checkYTexto" meta:resourcekey="chkCalidad" />
                </div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top:23px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valsummary" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" meta:resourcekey="btnGuardar" OnClick="btnGuardar_OnClick" CssClass="boton" />
    </div>
</asp:Content>
