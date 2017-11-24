<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="AltaPeriodoDestajo.aspx.cs" Inherits="SAM.Web.Administracion.AltaPeriodoDestajo" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" BotonRegresarVisible="false" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho30">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtAnio" runat="server" ID="txtAnio" MaxLength="5" />
                    <asp:RangeValidator meta:resourcekey="rngAnio" runat="server" ID="rngAnio" ControlToValidate="txtAnio" Type="Integer" MinimumValue="1" MaximumValue="99999" Display="None" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtSemana" runat="server" ID="txtSemana" MaxLength="5" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtDiasFestivos" runat="server" ID="txtDiasFestivos" MaxLength="1" Text="0" />
                    <asp:RangeValidator meta:resourcekey="rngDiasFestivos" runat="server" ID="rngDiasFestivos" ControlToValidate="txtDiasFestivos" Type="Integer" MinimumValue="0" MaximumValue="9" Display="None" />
                </div>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblFechaInicial" runat="server" ID="lblFechaInicial" AssociatedControlID="dtpInicio" />
                    <mimo:MappableDatePicker runat="server" ID="dtpInicio" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqFechaInicio" runat="server" ID="reqFechaInicio" Display="None" ControlToValidate="dtpInicio" />
                </div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblFechaFinal" runat="server" ID="lblFechaFinal" AssociatedControlID="dtpFinal" />
                    <mimo:MappableDatePicker runat="server" ID="dtpFinal" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqFechaFinal" runat="server" ID="reqFechaFinal" Display="None" ControlToValidate="dtpFinal" />
                    <asp:CompareValidator meta:resourcekey="cmpFechas" runat="server" ID="cmpFechas" ControlToValidate="dtpInicio" ControlToCompare="dtpFinal" Display="None" Operator="LessThan" Type="Date" />
                </div>
            </div>
            <div class="divIzquierdo ancho40">
                <div class="validacionesRecuadro" style="margin-top:24px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>            
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <samweb:BotonProcesando meta:resourcekey="btnGenerar" CssClass="boton" runat="server" ID="btnGenerar" OnClick="btnGenerar_Click" />    
    </div>
</asp:Content>
