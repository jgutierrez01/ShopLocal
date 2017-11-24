<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true" CodeBehind="LiberacionCalidad.aspx.cs" Inherits="SAM.Web.Calidad.LiberacionCalidad" %>

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
            <telerik:AjaxSetting AjaxControlID="btnBorrar">
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
                FiltroNumeroEmbarque="false"
                FiltroCuadrante="false"
                FiltroNumeroControl="false"
                FiltroNumeroUnico="false"
                FiltroOrdenTrabajo="false"
                ProyectoHeaderID="headerProyecto"
                runat="server"
                ID="filtroGenerico"></uc2:Filtro>
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
        <div class="contenedorCentral">
            <div class="cajaFiltros">
                <div class="separador">
                    <div class="divIzquierdo">
                        <asp:Label ID="lblFechaLiberacion" runat="server" meta:resourcekey="lblFechaLiberacion" CssClass="bold"></asp:Label>
                        <br />
                        <mimo:MappableDatePicker ID="rdpFechaLiberacion" runat="server" MinDate="01/01/1960"
                            MaxDate="01/01/2050" EnableEmbeddedSkins="false">
                        </mimo:MappableDatePicker>
                    </div>
                    <div class="divIzquierdo">
                        <asp:Label runat="server" meta:resourcekey="lblNumeroControl" ID="lblNumeroControl" CssClass="labelHack bold"></asp:Label>
                        <telerik:RadComboBox ID="radCmbNumeroControl" runat="server" Height="150px"
                            EnableLoadOnDemand="true"
                            ShowMoreResultsBox="true"
                            EnableVirtualScrolling="true"
                            OnClientItemsRequesting="Sam.Filtro.NumControlOnClientItemsRequestingEventHandler"
                            OnSelectedIndexChanged="radCmbNumeroControl_SelectedIndexChanged"
                            meta:resourcekey="radCmbNumeroControl"
                            CausesValidation="false"
                            AutoPostBack="true" 
                            AllowCustomText="true"
                            OnDataBound="radCmbNumeroControl_DataBound"  >
                            <WebServiceSettings Method="ListaNumerosControlLiberacionCalidad" Path="~/WebServices/ComboboxWebService.asmx" />
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator
                            meta:resourcekey="valNumeroControl"
                            runat="server"
                            ID="rfvNumControl"
                            Display="None"
                            ControlToValidate="radCmbNumeroControl"
                            ValidateEmptyText="true"
                            ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                            CssClass="bold"
                            Enabled="true"
                            OnServerValidate="rfvNumControl_ServerValidate"
                            ValidationGroup="valLiberacion" />
                    </div>
                    <div class="divIzquierdo">
                        <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton"
                             ValidationGroup="valLiberacion" />
                    </div>
                    <div class="divIzquierdo">
                        <asp:Button ID="btnBorrar" runat="server" meta:resourcekey="btnBorrar" OnClick="btnBorrar_Click" CssClass="boton"
                                 ValidationGroup="valLiberacion" />
                    </div>
                </div>
            </div>
        </div>
        <div>
            <asp:ValidationSummary ID="valSummary1" runat="server" ValidationGroup="valLiberacion" meta:resourcekey="valSummary"
                EnableClientScript="true" DisplayMode="BulletList"
                class="summaryList" />
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
