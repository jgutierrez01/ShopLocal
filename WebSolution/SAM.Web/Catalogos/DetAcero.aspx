<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetAcero.aspx.cs" Inherits="SAM.Web.Catalogos.DetAcero" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstAcero.aspx"
        meta:resourcekey="lblDetAcero" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNomenclatura" EntityPropertyName="Nomenclatura"
                        MaxLength="50" meta:resourcekey="lblNomenclatura" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblFamilia" runat="server" meta:resourcekey="lblFamilia" CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlFamiliaAcero" EntityPropertyName="FamiliaAceroID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvFamilia" ControlToValidate="ddlFamiliaAcero"
                        InitialValue="" Display="None" meta:resourcekey="rfvFamilia" />
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox runat="server" ID="chkCalidad" EntityPropertyName="VerificadoPorCalidad"
                        meta:resourcekey="chkCalidad" CssClass="checkYTexto" />
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
            <p>
            </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            meta:resourcekey="btnGuardar" CssClass="boton" />
    </div>
</asp:Content>
