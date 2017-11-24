<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetDiametro.aspx.cs" Inherits="SAM.Web.Catalogos.DetDiametro" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetDiametro" NavigateUrl="~/Catalogos/LstDiametro.aspx" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro" EntityPropertyName="Valor" meta:resourcekey="txtDiametro"/>
                    <asp:RegularExpressionValidator runat="server" ID="rgvDiametro" ValidationExpression="\d.*" ControlToValidate="txtDiametro" Display="None" meta:resourcekey="rgvDiametro"/>
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox EntityPropertyName="VerificadoPorCalidad" runat="server" ID="chkCalidad" CssClass="checkYTexto" meta:resourcekey="chkCalidad" />
                </div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top:23px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valsummary" CssClass="summary" meta:resourcekey="valsummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" OnClick="btnGuardar_OnClick" CssClass="boton" />
    </div>
</asp:Content>
