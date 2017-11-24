<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="LstEmbarque.aspx.cs" Inherits="SAM.Web.WorkStatus.LstEmbarque"  %>
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
        <asp:Label runat="server" ID="lblEmbarque" CssClass="Titulo" meta:resourcekey="lblEmbarque"></asp:Label>
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
                        meta:resourcekey="ddlAccionResource1">
                        <asp:ListItem Value="-1" Text="" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstPreparar"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstEtiquetar"></asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="lstImprimir"></asp:ListItem>
                        <asp:ListItem Value="3" meta:resourcekey="lstEmbarcar"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valAccion" runat="server" ControlToValidate="ddlAccion"
                        Display="None" meta:resourcekey="valAccion" InitialValue="-1"></asp:RequiredFieldValidator>
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
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="WorkstatusSpoolID, SpoolID"
                ClientDataKeyNames="WorkstatusSpoolID,SpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="hypImprimir" runat="server" CssClass="link" meta:resourcekey="hypImprimir" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgImprimir" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/imprimirHeader.png" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="hypEtiquetar" runat="server" CssClass="link" meta:resourcekey="hypEtiquetar" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgEtiquetar" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_etiquetar.png" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="hypPreparar" runat="server" CssClass="link" meta:resourcekey="hypPreparar" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgPreparar" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_prepararembarque.png" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="hypEmbarcar" runat="server" CssClass="link" meta:resourcekey="hypEmbarcar" Visible="false"></asp:HyperLink>
                        <asp:HyperLink ID="imgEmbarcar" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_embarcar.png" Visible="false"></asp:HyperLink>
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
                    <telerik:GridBoundColumn UniqueName="hdVigenciaAduana" DataField="VigenciaAduana" meta:resourcekey="hdVigenciaAduana"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridCheckBoxColumn UniqueName="hdPreparado" DataField="Preparado"
                        meta:resourcekey="hdPreparado" FilterControlWidth="40" HeaderStyle-Width="130" />
                        <telerik:GridBoundColumn UniqueName="hdFolioPreparacion" DataField="FolioPreparacion" meta:resourcekey="hdFolioPreparacion"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                        <telerik:GridBoundColumn UniqueName="hdEmbarque" DataField="Embarque" meta:resourcekey="hdEmbarque"
                            FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdFechaEstimada" DataField="FechaEstimada" meta:resourcekey="hdFechaEstimada"
                            FilterControlWidth="50" HeaderStyle-Width="100"></telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn UniqueName="hdFechaEmbarque" DataField="FechaEmbarque" meta:resourcekey="hdFechaEmbarque"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
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
