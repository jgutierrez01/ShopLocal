<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="LstTransferenciaSpool.aspx.cs" Inherits="SAM.Web.WorkStatus.LstTransferenciaSpool" %>

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
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTransferencia" CssClass="Titulo" meta:resourcekey="lblTransferencia"></asp:Label>
    </div>
        <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" FiltroNumeroUnico="false" FiltroNumeroControl="false"
                ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true" runat="server"
                ID="filtroGenerico"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblAccion" runat="server" CssClass="bold" meta:resourcekey="lblAccion"></asp:Label><br />
                    <asp:DropDownList ID="ddlAccion" runat="server" 
                        meta:resourcekey="ddlAccionResource">                      
                        <asp:ListItem Value="0" meta:resourcekey="lstPreparar"></asp:ListItem>                       
                        <asp:ListItem Value="1" meta:resourcekey="lstTransferencia"></asp:ListItem>
                    </asp:DropDownList>                   
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
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="TransferenciaSpoolID,SpoolID"
                ClientDataKeyNames="TransferenciaSpoolID,SpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">                                               
                        <asp:HyperLink ID="hypPreparar" runat="server" CssClass="link" meta:resourcekey="hypPreparar" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgPreparar" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_prepararembarque.png" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="hypTransferencia" runat="server" CssClass="link" meta:resourcekey="hypTransferencia" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgTransferencia" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_transferir.png" Visible="false"></asp:HyperLink>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                        HeaderStyle-Width="30" />  
                     <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" Resizable="false" Reorderable="false"
                            UniqueName="Ver_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlVer" meta:resourcekey="hlVer" 
                                    ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="inventario_h" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server"
                                    ID="hdReporte" meta:resourcekey="hdReporte" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                                            
                    <telerik:GridBoundColumn UniqueName="hdOrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="hdOrdenTrabajo"
                        FilterControlWidth="50" HeaderStyle-Width="115" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroControl" DataField="NumeroControl" meta:resourcekey="hdNumeroControl"
                        FilterControlWidth="50" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="hdSpool" DataField="NombreSpool" meta:resourcekey="hdSpool"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="hdArea" DataField="Area" meta:resourcekey="hdArea" DataFormatString="{0:N2}"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPDI" DataField="PDI" meta:resourcekey="hdPDI" DataFormatString="{0:N3}"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPeso" DataField="Peso" meta:resourcekey="hdPeso" DataFormatString="{0:N2}"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdEtiqueta" DataField="Etiqueta" meta:resourcekey="hdEtiqueta"
                        FilterControlWidth="50" HeaderStyle-Width="100" />                   
                    <telerik:GridCheckBoxColumn UniqueName="hdPreparado" DataField="Preparado"
                        meta:resourcekey="hdPreparado" FilterControlWidth="40" HeaderStyle-Width="130" />                    
                    <telerik:GridBoundColumn UniqueName="hdFechaPreparacion" DataField="FechaPreparacion" DataFormatString="{0:d}" meta:resourcekey="hdFechaPreparacion"
                        FilterControlWidth="50" HeaderStyle-Width="100" />  
                     <telerik:GridBoundColumn UniqueName="hdNumeroTransferencia" DataField="NumeroTransferencia" meta:resourcekey="hdNumeroTransferencia"
                            FilterControlWidth="50" HeaderStyle-Width="100" />  
                    <telerik:GridBoundColumn UniqueName="hdFechaTransferencia" DataField="FechaTransferencia" DataFormatString="{0:d}" meta:resourcekey="hdFechaTransferencia"
                        FilterControlWidth="50" HeaderStyle-Width="100" />                                 
                    <telerik:GridCheckBoxColumn UniqueName="hdHold" DataField="Hold" meta:resourcekey="hdHold"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridCheckBoxColumn UniqueName="hdTransferencia" DataField="Transferencia"
                        meta:resourcekey="hdTransferencia" FilterControlWidth="40" HeaderStyle-Width="130"  Visible="false"/>
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
