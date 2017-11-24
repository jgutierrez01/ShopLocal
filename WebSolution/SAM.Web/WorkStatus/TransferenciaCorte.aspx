<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="TransferenciaCorte.aspx.cs" Inherits="SAM.Web.WorkStatus.TransferenciaCorte" %>
    <%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>    
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
         <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="phLocalizacion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdNumeroUnicos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNumeroUnicos" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTransferenciaCorte" CssClass="Titulo" meta:resourcekey="lblTransferenciaCorte"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="true" FiltroNumeroUnico="false"
                ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true" runat="server" ID="filtroGenerico" NumeroControlAutoPostBack="true" OrdenTrabajoAutoPostBack="true"></uc2:Filtro>           
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
        <asp:PlaceHolder ID="phDatos" runat="server">
            <asp:HiddenField ID="hdnPatio" runat="server" />
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummaryResource1"
            CssClass="summaryList" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdNumeroUnicos" OnNeedDataSource="grdNumeroUnicos_OnNeedDataSource"
            OnItemCreated="grdNumerosUnicos_ItemCreated" AllowMultiRowSelection="true" Visible="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="NumeroUnicoSegmentoID" ClientDataKeyNames="NumeroUnicoSegmentoID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="hypTransferir" runat="server" CssClass="link" meta:resourcekey="hypTransferir"></asp:HyperLink>
                        <asp:HyperLink ID="imgTransferir" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_transferirB.png"></asp:HyperLink>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                        HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroUnico" DataField="NumeroUnico.Codigo"
                        meta:resourcekey="hdCodigo" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdSegmento" DataField="Segmento" meta:resourcekey="hdSegmento"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdItemCode" DataField="NumeroUnico.ItemCode.Codigo"
                        meta:resourcekey="hdItemCode" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdICDescripcion" DataField="NumeroUnico.ItemCode.DescripcionEspanol"
                        meta:resourcekey="hdICDescripcion" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro1" DataField="NumeroUnico.Diametro1" DataFormatString="{0:N3}"
                        meta:resourcekey="hdDiametro1" FilterControlWidth="50" HeaderStyle-Width="100" DataType="System.Decimal" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro2" DataField="NumeroUnico.Diametro2" DataFormatString="{0:N3}"
                        meta:resourcekey="hdDiametro2" FilterControlWidth="50" HeaderStyle-Width="100" DataType="System.Decimal" />
                    <telerik:GridBoundColumn UniqueName="hdColada" DataField="NumeroUnico.Colada.NumeroColada" DataFormatString="{0:N0}"
                        meta:resourcekey="hdColada" FilterControlWidth="50" HeaderStyle-Width="135" />
                    <telerik:GridBoundColumn UniqueName="hdCedula" DataField="NumeroUnico.Cedula" meta:resourcekey="hdCedula"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCantidad" DataField="InventarioBuenEstado" DataFormatString="{0:N0}"
                        meta:resourcekey="hdCantidad" FilterControlWidth="100" HeaderStyle-Width="200" />
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
        <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
            OnClick="lnkActualizar_Click" />
    </div>
    </div>
</asp:Content>
