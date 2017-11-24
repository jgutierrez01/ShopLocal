<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetConsumibles.aspx.cs" Inherits="SAM.Web.Catalogos.DetConsumibles" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
<sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetConsumible" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
            <p>
            <asp:Label ID="lblPatioTitulo" runat="server" CssClass="bold" meta:resourcekey="lblPatioTitulo"></asp:Label>&nbsp;
            <asp:Label ID="lblPatio" runat="server"></asp:Label>
            </p>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox EntityPropertyName="Codigo" runat="server" id="txtCodigo" meta:resourcekey="txtCodigo" MaxLength="50" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="Kilogramos" runat="server" ID="txtKilogramos" meta:resourcekey="txtKilogramos" MaxLength="7" />
                    <asp:RangeValidator meta:resourcekey="rngKilogramos" runat="server" ID="rngKilogramos" Display="None" Type="Integer" MinimumValue="1" MaximumValue="9999999" ControlToValidate="txtKilogramos" />
                </div>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
     <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" CssClass="boton" meta:resourcekey="btnGuardar"/>    
    </div>
</asp:Content>
