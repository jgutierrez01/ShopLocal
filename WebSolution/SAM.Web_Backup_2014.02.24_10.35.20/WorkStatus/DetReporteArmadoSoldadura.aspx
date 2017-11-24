<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="DetReporteArmadoSoldadura.aspx.cs" Inherits="SAM.Web.WorkStatus.DetReporteArmadoSoldadura" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rvWeb" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Workstatus/WkStDefault.aspx"
        meta:resourcekey="lblTitulo" />
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                    <br />
                    <mimo:MappableDropDown ID="ddlProyecto" runat="server" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                        InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" meta:resourcekey="lblDesde" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvFechaDesde" ControlToValidate="dtpDesde"
                        Display="None" meta:resourcekey="rfvFechaDesde" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" meta:resourcekey="lblHasta" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvFechaHasta" ControlToValidate="dtpDesde"
                        Display="None" meta:resourcekey="rfvFechaHasta" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTipoReporte" runat="server" CssClass="bold" meta:resourcekey="lblTipoReporte" />
                    <br />
                    <asp:DropDownList runat="server" ID="ddlTipoReporte">
                        <asp:ListItem Value="" Text="" />
                        <asp:ListItem Value="1" meta:resourcekey="liArmado" />
                        <asp:ListItem Value="2" meta:resourcekey="liSoldadura" />
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvTipoReporte" ControlToValidate="ddlTipoReporte"
                        InitialValue="" Display="None" meta:resourcekey="rfvTipoReporte" />
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
