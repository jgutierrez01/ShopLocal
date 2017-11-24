<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetCortador.aspx.cs" Inherits="SAM.Web.Catalogos.DetCortador" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
    <!-- <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" /> -->
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstCortador.aspx"
        meta:resourcekey="lblDetCortador" />
    <div class="cntCentralForma">
    <div class="dashboardCentral">
        <div class="divIzquierdo ancho70">

            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                    MaxLength="50" meta:resourcekey="lblNombre" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtApPaterno" EntityPropertyName="ApPaterno"
                    MaxLength="50" meta:resourcekey="lblApPaterno" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtApMaterno" EntityPropertyName="ApMaterno"
                    MaxLength="50" meta:resourcekey="lblApMaterno" />
            </div>
            <div class="separador">
                <asp:Label ID="Label1" runat="server" CssClass="bold" meta:resourcekey="lblPatio" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlPatios" EntityPropertyName="PatioID" AutoPostBack="true" OnSelectedIndexChanged="ddlPatios_SelectedIndexChanged" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="rfvPatio" runat="server" ControlToValidate="ddlPatios"
                    InitialValue="" Display="None" meta:resourcekey="rfvPatio" />
            </div>

            <div class="separador">
                <asp:Label ID="Label2" runat="server" CssClass="bold" meta:resourcekey="lblTaller" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlTaller" EntityPropertyName="TallerID" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="rfvTaller" runat="server" ControlToValidate="ddlTaller"
                    InitialValue="" Display="None" meta:resourcekey="rfvTaller" />
            </div>        





            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumeroEmpleado" EntityPropertyName="NumeroEmpleado"
                    MaxLength="50" meta:resourcekey="lblNumeroEmpleado" />
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
