<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetCedula.aspx.cs" Inherits="SAM.Web.Catalogos.DetCedula" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetCedula" NavigateUrl="~/Catalogos/LstCedula.aspx" />

    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtCedula" EntityPropertyName="Codigo" ErrorMessage="La cédula es requerida" Label="Cédula:" meta:resourcekey="txtCedula"/>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtOrden" EntityPropertyName="Orden" Label="Órden:" meta:resourcekey="txtOrden"/>
                    <asp:RegularExpressionValidator runat="server" ID="rgvOrden" ValidationExpression="\d.*" ControlToValidate="txtOrden" ErrorMessage="Capture un número entero para orden" Display="None" meta:resourcekey="rgvOrden"/>
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox runat="server" ID="chkCalidad" meta:resourcekey="chkCalidad" EntityPropertyName="VerificadoPorCalidad" CssClass="checkYTexto" />
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
        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" meta:resourcekey="btnGuardar" OnClick="btnGuardar_OnClick" CssClass="boton" />
    </div>
</asp:Content>
