<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="LstCuadrantes.aspx.cs" Inherits="SAM.Web.WorkStatus.LstCuadrantes" %>

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
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral" >
        <div class="cajaFiltros ">
            <uc2:Filtro FiltroProyecto ="true" ProyectoAutoPostBack="true" ProyectoRequerido="true"
                        FiltroNumeroControl="true" NumeroControlRequerido="true" NumeroControlAutoPostBack="true"
                        FiltroCuadrante="true" CuadranteRequerido="true" CuadranteAutoPostBack="true"
                        FiltroNumeroUnico="false"
                        FiltroOrdenTrabajo="false" 
                        ProyectoHeaderID="headerProyecto" runat="server"
                        ID="filtroGenerico" ></uc2:Filtro>                     
              <div class="separador">
            
                <div class="divIzquierdo">
                    <asp:Label runat="server" ID="lblFechaCuadrante" meta:resourcekey="lblFechaCuadrante"
                        CssClass="bold" />   <br />  
                    <mimo:MappableDatePicker ID="mdpFechaCuadrante" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false">
                    </mimo:MappableDatePicker><span class="required">*</span>
                </div>
                <div class="divIzquierdo">
                    <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton" />
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
</asp:Content>
