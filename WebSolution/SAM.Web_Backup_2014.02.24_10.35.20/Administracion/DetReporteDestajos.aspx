<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="DetReporteDestajos.aspx.cs" Inherits="SAM.Web.Administracion.DetReporteDestajos" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rvWeb" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Administracion/AdminDefault.aspx"
        meta:resourcekey="lblTitulo" />
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                    <br />
                    <mimo:MappableDropDown ID="ddlProyecto" runat="server" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged"
                        EntityPropertyName="ProyectoID" AutoPostBack="true" CausesValidation="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                        InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button runat="server" ID="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton"
                        meta:resourcekey="btnMostrar" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <br />
        <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
            CssClass="summaryList" />
        <div>
            <asp:Literal runat="server" ID="litReporteNoEncontrado" meta:resourcekey="litReporteNoEncontrado"
                Visible="false" />
        </div>
        <rvWeb:ReportViewer runat="server" ID="rptVisorReporte" Height="500" Width="100%"
            Visible="false" />
    </div>
</asp:Content>
<asp:Content ID="cntFooter" ContentPlaceHolderID="cphInnerFoot" runat="server">
</asp:Content>
