<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="Despacho.aspx.cs" Inherits="SAM.Web.WorkStatus.Despacho" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="ajxMgr" runat="server" style="display:none;">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdMateriales">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMateriales" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdMateriales" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros" style="margin-bottom: 10px;">
            <uc2:Filtro FiltroNumeroUnico="false" OrdenTrabajoRequerido="true" ProyectoRequerido="true"
                ProyectoHeaderID="headerProyecto" ProyectoAutoPostBack="true" runat="server"
                NumeroControlRequerido="true" NumeroControlAutoPostBack="true" ID="filtroGenerico"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrar_Click"
                        CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <sam:Header ID="headerProyecto" runat="server" Visible="False" />
        <asp:ValidationSummary runat="server" ID="valGrid" meta:resourcekey="valGrid" CssClass="summaryList" />
        <asp:PlaceHolder runat="server" ID="phListado" Visible="False">
            <p>
            </p>
            <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdMateriales" OnNeedDataSource="grdMateriales_OnNeedDataSource"
                OnItemCommand="grdMateriales_OnItemCommand" OnItemDataBound="grdMateriales_OnItemDataBound">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar"
                                OnClick="lnkActualizar_Click" CssClass="link" CausesValidation="false" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar"
                                ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_Click" CssClass="imgEncabezado"
                                CausesValidation="false" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" UniqueName="accion_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDespacho" meta:resourcekey="hlDespacho" ImageUrl="~/Imagenes/Iconos/ico_despachar.png" Visible="false" />
                                <asp:ImageButton runat="server" ID="imgCancelar" meta:resourcekey="imgCancelar" CommandName="cancelar" Visible="false" ImageUrl="~/Imagenes/Iconos/borrar.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn meta:resourcekey="grdColEtiqueta" HeaderStyle-Width="80"
                            FilterControlWidth="40" DataField="EtiquetaMaterial" UniqueName="EtiquetaMaterial" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColIc" HeaderStyle-Width="100" FilterControlWidth="60"
                            DataField="CodigoItemCode" UniqueName="CodigoItemCode" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColDescIc" HeaderStyle-Width="350"
                            FilterControlWidth="120" DataField="DescripcionItemCode" UniqueName="DescripcionItemCode" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColD1" HeaderStyle-Width="70" FilterControlWidth="30"
                            DataField="Diametro1" DataFormatString="{0:N3}" UniqueName="Diametro1" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColD2" HeaderStyle-Width="70" FilterControlWidth="30"
                            DataField="Diametro2" DataFormatString="{0:N3}" UniqueName="Diametro2" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColCantidad" HeaderStyle-Width="80"
                            FilterControlWidth="40" DataField="CantidadRequerida" DataFormatString="{0:N}" UniqueName="CantidadRequerida" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColEstatus" HeaderStyle-Width="80"
                            FilterControlWidth="40" DataField="EstatusTexto" UniqueName="EstatusTexto" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="tieneHold" HeaderStyle-Width="60" FilterControlWidth="40" DataField="TieneHold">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
                    OnClick="lnkActualizar_Click" />
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
