<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true"
    CodeBehind="DossierCalidad.aspx.cs" Inherits="SAM.Web.Proyectos.DossierCalidad" %>

<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDossierProyecto" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="listaCheck clear ancho90" style="padding-top: 15px;">
                <asp:CheckBoxList runat="server" ID="chkDossier" CssClass="cajaAzul" RepeatColumns="3"
                    Width="100%">
                    <asp:ListItem Value="0" meta:resourcekey="rptTrazabilidad" />
                    <asp:ListItem Value="1" meta:resourcekey="rptWPS" />
                    <asp:ListItem Value="2" meta:resourcekey="rptMTR" />
                    <asp:ListItem Value="3" meta:resourcekey="rptInspVisual" />
                    <asp:ListItem Value="4" meta:resourcekey="rptLibDim" />
                    <asp:ListItem Value="5" meta:resourcekey="rptEspesores" />
                    <asp:ListItem Value="6" meta:resourcekey="rprRT" />
                    <asp:ListItem Value="7" meta:resourcekey="rptPT" />
                    <asp:ListItem Value="8" meta:resourcekey="rptPWHT" />
                    <asp:ListItem Value="9" meta:resourcekey="rptDurezas" />
                    <asp:ListItem Value="10" meta:resourcekey="rptRTpTT" />
                    <asp:ListItem Value="11" meta:resourcekey="rptPTpTT" />
                    <asp:ListItem Value="12" meta:resourcekey="rptPreheat" />
                    <asp:ListItem Value="13" meta:resourcekey="rptUT" />
                    <asp:ListItem Value="14" meta:resourcekey="rptPintura" />
                    <asp:ListItem Value="15" meta:resourcekey="rptEmbarque" />
                    <asp:ListItem Value="16" meta:resourcekey="rptMTRSold" />
                    <asp:ListItem Value="17" meta:resourcekey="rptDrawing" />
                    <asp:ListItem Value="18" meta:resourcekey="rptPMI" />
                    <asp:ListItem Value="19" meta:resourcekey="rptPruebaHidro" />
                </asp:CheckBoxList>
            </div>
            <br />
            <div>
                <asp:Label ID="lblMTR" runat="server" meta:resourcekey="lblMTR" CssClass="bold"></asp:Label>
                <asp:RadioButtonList runat="server" ID="rdbMTR" CssClass="checkYTexto">
                    <asp:ListItem Value="0" meta:resourcekey="NumeroCertificado" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="NumeroColada"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <br />
            <div>
                <asp:Label ID="lblLD" runat="server" meta:resourcekey="lblLD" CssClass="bold"></asp:Label>
                <asp:RadioButtonList runat="server" ID="rdbLD" CssClass="checkYTexto">
                    <asp:ListItem Value="0" meta:resourcekey="NumeroReporte" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="NumeroControl"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton"
            OnClick="btnGuardar_OnClick" />
    </div>
</asp:Content>
