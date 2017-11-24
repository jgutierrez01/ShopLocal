<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstSpoolPnd.aspx.cs" Inherits="SAM.Web.WorkStatus.LstSpoolPnd" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblPnd" CssClass="Titulo" meta:resourcekey="lblPnd"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="false" FiltroNumeroUnico="false"
                ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true" NumeroControlAutoPostBack="true"
                OrdenTrabajoAutoPostBack="true" runat="server" ID="filtroGenerico"></uc2:Filtro>
               
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTipoPrueba" runat="server" meta:resourcekey="lblTipoPrueba" CssClass="bold"></asp:Label><br />
                    <asp:DropDownList ID="ddlTipoPrueba" runat="server" meta:resourcekey="ddlTipoPruebaResource1" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valTipoPrueba" runat="server" ControlToValidate="ddlTipoPrueba"
                        Display="None" meta:resourcekey="valTipoPrueba"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" meta:resourcekey="btnMostrar" runat="server" CssClass="boton"
                        OnClick="btnMostrar_Click" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <proy:Encabezado ID="proyEncabezado" runat="server" Visible="False" />
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
            CssClass="summaryList" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource"
            OnItemCreated="grdSpools_ItemCreated" AllowMultiRowSelection="true" Visible="false"
            OnItemDataBound="grdSpools_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="WorkstatusSpoolID,RequisicionSpoolID"
                ClientDataKeyNames="WorkstatusSpoolID,RequisicionSpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="hypReporte" runat="server" CssClass="link" meta:resourcekey="hypReporte"></asp:HyperLink>
                        <asp:HyperLink ID="imgReporte" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_generareporte.png"></asp:HyperLink>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                        HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="hdRequisicion" DataField="NumeroRequisicion" meta:resourcekey="hdRequisicion"
                    FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdOrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="hdOrdenTrabajo"
                        FilterControlWidth="50" HeaderStyle-Width="115" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroControl" DataField="NumeroControl" meta:resourcekey="hdNumeroControl"
                        FilterControlWidth="50" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="hdSpool" DataField="NombreSpool" meta:resourcekey="hdSpool"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="hdArea" DataField="Area" meta:resourcekey="hdArea"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPDI" DataField="PDI" meta:resourcekey="hdPDI" DataFormatString="{0:#0.000}"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPeso" DataField="Peso" meta:resourcekey="hdPeso"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdArmado" DataField="Armado" meta:resourcekey="hdArmado"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdSoldadura" DataField="Soldadura" meta:resourcekey="hdSoldadura"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                        <telerik:GridCheckBoxColumn UniqueName="hdInspeccionVisual" DataField="LiberacionDimensional" meta:resourcekey="hdInspeccionVisual"
                        FilterControlWidth="40" HeaderStyle-Width="130" />
                    <telerik:GridCheckBoxColumn UniqueName="hdHold" DataField="Hold" meta:resourcekey="hdHold"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </mimo:MimossRadGrid>
    </div>
    <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="False"
            OnClick="lnkActualizar_Click" meta:resourcekey="btnRefreshResource1" />
    </div>
</asp:Content>
