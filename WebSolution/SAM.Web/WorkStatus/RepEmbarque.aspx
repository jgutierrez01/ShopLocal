<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="RepEmbarque.aspx.cs" Inherits="SAM.Web.WorkStatus.RepEmbarque" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnGuardar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="panelFechas" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro FiltroProyecto="true" ProyectoAutoPostBack="true" ProyectoRequerido="true"
                FiltroNumeroEmbarque="true" EmbarqueRequerido="true" EmbarqueAutoPostBack="true"
                FiltroCuadrante="false"
                FiltroNumeroControl="false"
                FiltroNumeroUnico="false"
                FiltroOrdenTrabajo="false"
                ProyectoHeaderID="headerProyecto"    
                runat="server"
                ID="filtroGenerico">

            </uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" meta:resourcekey="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
    </div>    
    <telerik:RadAjaxPanel ID="panelFechas" runat="server" Visible="false">
         <asp:ValidationSummary runat="server" ID="ValidationSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="separador">
                <div class="divIzquierdo">
                    <asp:Label ID="lblFechaEstimada" runat="server" meta:resourcekey="lblFechaEstimadaCarga" CssClass="bold"></asp:Label>
                    <br />
                    <mimo:MappableDatePicker ID="rdpFechaEstimada" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false" Enabled ="false">
                    </mimo:MappableDatePicker>
                </div>
                <div class="divIzquierdo">
                    <asp:Label ID="lblFechaEmbarque" runat="server" meta:resourcekey="lblFechaEmbarqueReal" CssClass="bold"></asp:Label>
                    <br />
                    <mimo:MappableDatePicker ID="rdpFechaEmbarque" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false">
                    </mimo:MappableDatePicker>
                </div>
                <div class="divIzquierdo">
                    <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton" />
                </div>
            </div>
        </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
