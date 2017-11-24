<%@ Page Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstRequisicionesPruebas.aspx.cs" Inherits="SAM.Web.WorkStatus.LstRequisicionesPruebas" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdJuntas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdJuntas" LoadingPanelID="ldPanel" />
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
                    <telerik:AjaxUpdatedControl ControlID="grdJuntas" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblRequisicionPruebas" CssClass="Titulo" meta:resourcekey="lblRequisicionPruebas"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="true" FiltroNumeroUnico="false"
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
        <mimo:MimossRadGrid runat="server" ID="grdJuntas" OnNeedDataSource="grdJuntas_OnNeedDataSource"
            OnItemCreated="grdJuntas_ItemCreated" AllowMultiRowSelection="true" Visible="false"
            OnItemDataBound="grdJuntas_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="JuntaWorkstatusID"
                ClientDataKeyNames="JuntaWorkstatusID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="hypRequisicion" runat="server" CssClass="link" meta:resourcekey="hypRequisicion"></asp:HyperLink>
                        <asp:HyperLink ID="imgRequisicion" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_generarequisicion.png"></asp:HyperLink>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                        HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="hdOrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="hdOrdenTrabajo"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdNombreSpool" DataField="NombreSpool" meta:resourcekey="hdNombreSpool"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroControl" DataField="NumeroControl" meta:resourcekey="hdNumeroControl"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdEtiquetaJunta" DataField="EtiquetaJunta" meta:resourcekey="hdEtiquetaJunta"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdLocalizacion" DataField="Localizacion" meta:resourcekey="hdLocalizacion"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTipo" DataField="TipoJunta" meta:resourcekey="hdTipo"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCedula" DataField="Cedula" meta:resourcekey="hdCedula"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdFamiliaAcero1" DataField="FamiliaAceroMaterial1"
                        meta:resourcekey="hdFamiliaAcero1" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdFamiliaAcero2" DataField="FamiliaAceroMaterial2"
                        meta:resourcekey="hdFamiliaAcero2" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro" DataField="Diametro" DataFormatString="{0:N3}"
                        meta:resourcekey="hdDiametro" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdArmado" DataField="Armado" meta:resourcekey="hdArmado"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdSoldadura" DataField="Soldadura" meta:resourcekey="hdSoldadura"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdInspeccionVisual" DataField="InspeccionVisual"
                        meta:resourcekey="hdInspeccionVisual" FilterControlWidth="40" HeaderStyle-Width="70" />
                    <telerik:GridCheckBoxColumn UniqueName="hdInspeccionDimensional" DataField="InspeccionDimensional"
                        meta:resourcekey="hdInspeccionDimensional" FilterControlWidth="40" HeaderStyle-Width="90" />
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
        <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
            OnClick="lnkActualizar_Click" />
    </div>
</asp:Content>
