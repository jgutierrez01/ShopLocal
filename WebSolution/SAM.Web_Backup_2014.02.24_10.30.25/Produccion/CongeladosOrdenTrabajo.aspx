<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" 
CodeBehind="CongeladosOrdenTrabajo.aspx.cs" Inherits="SAM.Web.Produccion.CongeladosOrdenTrabajo" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<div class="paginaHeader">
    <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
</div>
<div class="contenedorCentral">
<telerik:RadAjaxManager ID="radManager" runat="server">
    <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
    <div class="cajaFiltros" style="margin-bottom:5px;">
            <uc2:Filtro ProyectoRequerido="true" NumeroControlRequerido="false" OrdenTrabajoRequerido= "true" FiltroNumeroUnico="false" OrdenTrabajoAutoPostback="true"
                NumeroControlAutoPostBack="true" OnDdlProyecto_SelectedIndexChanged="proyecto_Cambio" ProyectoHeaderID="proyHeader" ProyectoAutoPostBack="true"
                runat="server" ID="filtroGenerico">
            </uc2:Filtro> 
            <div class="divIzquierdo">
                <div class="separador">
                    <samweb:BotonProcesando CssClass="boton" ID="btnMostrar" OnClick="btnMostrarClick"
                        meta:resourcekey="btnMostrar" runat="server" />
                </div>
            </div>
        <p></p>
    </div>
     <div class="separador">
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
    </div>
    <div class="ancho100">
            <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList"
                class="summaryList" meta:resourcekey="valSummary" />
        </div>
    <asp:PlaceHolder runat="server" ID="phSpools" Visible="False">
        <p></p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnItemDataBound="grdSpools_ItemDataBound" OnNeedDataSource="grdSpools_OnNeedDataSource" AllowMultiRowSelection="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
            <CommandItemTemplate>
            </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlTransfCong" runat="server" ID="hlTransfCong" NavigateUrl='<%#"javascript:Sam.Produccion.AbrePopupCongeladoOrdenTrabajo("+Eval("SpoolID") +","+ Eval("MaterialSpoolID") +","+Eval("Cantidad")+","+Eval("ProyectoID")+","+Eval("NumeroUnico")+","+"\""+Eval("Codigo")+"\""+");"%>' ImageUrl="~/Imagenes/Iconos/ico_tranferirCongelado.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn meta:resourcekey="grdSpool" UniqueName="Spool" DataField="Spool" FilterControlWidth="100" HeaderStyle-Width="140"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdEtiqueta" UniqueName="Etiqueta" DataField="Etiqueta" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdItemCode" UniqueName="ItemCode" DataField="ItemCode" FilterControlWidth="100" HeaderStyle-Width="140"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdDescripcion" UniqueName="Descripcion" DataField="Descripcion" FilterControlWidth="150" HeaderStyle-Width="190" />
                    <telerik:GridBoundColumn meta:resourcekey="grdD1" UniqueName="D1" DataField="D1" FilterControlWidth="40" HeaderStyle-Width="80" />                    
                    <telerik:GridBoundColumn meta:resourcekey="grdD2" UniqueName="D2" DataField="D2" FilterControlWidth="40" HeaderStyle-Width="80" />                    
                    <telerik:GridBoundColumn meta:resourcekey="grdCodigo" UniqueName="Codigo" DataField="Codigo" FilterControlWidth="60" HeaderStyle-Width="100" />                    
                    <telerik:GridBoundColumn meta:resourcekey="grdCantidad" UniqueName="Cantidad" DataField="Cantidad" FilterControlWidth="40" HeaderStyle-Width="80" />                    
                    <telerik:GridBoundColumn meta:resourcekey="grdEstatus" UniqueName="Estatus" DataField="Estatus" FilterControlWidth="50" HeaderStyle-Width="90" />                    
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </asp:PlaceHolder>
</div>
<div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" OnClick="btnWrapper_Click" ID="btnRefresh" CausesValidation="false"  />
    </div>
</asp:Content>