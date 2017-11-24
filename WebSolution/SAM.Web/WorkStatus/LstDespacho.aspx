<%@ Page Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstDespacho.aspx.cs" Inherits="SAM.Web.WorkStatus.LstDespacho" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajaxMgr">
        <ajaxsettings>
            <telerik:AjaxSetting AjaxControlID="grdDespachos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDespachos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnWrapper">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDespachos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnQuitaSeleccionados">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDespachos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </ajaxsettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblDespacho" CssClass="Titulo" meta:resourcekey="lblDespacho"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <asp:CustomValidator runat="server" ID="cvSeleccionFiltro" OnServerValidate="cvSeleccionFiltro_ServerValidate"
                Display="None" meta:resourcekey="cvSeleccionFiltro" />
            <uc2:Filtro ProyectoRequerido="true" NumeroControlRequerido="false" FiltroNumeroUnico="true"
                FiltroOrdenTrabajo="false" ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true"
                NumeroControlAutoPostBack="true" NumeroUnicoAutoPostBack="true" runat="server"
                ID="filtroGenerico">
            </uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        OnClick="btnMostrar_Click" CssClass="boton" />
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
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary"
            CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdDespachos" OnNeedDataSource="grdDespachos_OnNeedDataSource"
            OnItemCommand="grdDespachos_ItemCommand" Visible="false" OnItemDataBound="grdDespachos_ItemDataBound">
            <mastertableview autogeneratecolumns="false" allowmulticolumnsorting="true">
                <CommandItemTemplate>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ver_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink meta:resourcekey="hlVer" runat="server" ID="hlVer" ImageUrl="~/Imagenes/Iconos/info.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="editarRegistro" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlEditarDespacho"
                                ImageUrl="~/Imagenes/Iconos/editar.png" meta:resourcekey="hlEditarDespacho" runat="server" Visible="false"></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="cancelar_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgCancelar" meta:resourcekey="imgCancelar" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CommandName="cancelar" CommandArgument='<%#Eval("DespachoID") %>' OnClientClick="return Sam.Confirma(6);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="FechaDespacho" DataField="FechaDespacho" meta:resourcekey="hdFechaDespacho"
                        HeaderStyle-Width="120" FilterControlWidth="50" Groupable="false" DataFormatString="{0:d}" />
                    <telerik:GridBoundColumn UniqueName="NumeroOrden" DataField="NumeroOrden" meta:resourcekey="hdOrdenTrabajo"
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" meta:resourcekey="hdNumeroControl"
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Etiqueta" DataField="Etiqueta" meta:resourcekey="hdEtiqueta"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" meta:resourcekey="hdItemCode"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="hdDescripcion"
                        HeaderStyle-Width="300" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridCheckBoxColumn UniqueName="EsEquivalente" DataField="EsEquivalente"
                        meta:resourcekey="hdEsEquivalente" HeaderStyle-Width="100" FilterControlWidth="50"
                        Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="NumeroUnico" DataField="NumeroUnico" meta:resourcekey="hdNumeroUnico"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Cantidad" DataField="Cantidad" meta:resourcekey="hdCantidad"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Estatus" DataField="Estatus" meta:resourcekey="hdEstatus"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </mastertableview>
        </mimo:MimossRadGrid>
    </div>
    <div id="btnWrapper">
        <asp:Button runat="server" CausesValidation="false" ID="btnQuitaSeleccionados" CssClass="oculto" OnClick="btnQuitaSeleccionados_Click" />
    </div>
</asp:Content>
