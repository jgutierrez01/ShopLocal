<%@ Page Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="WorkstatusIngenieria.aspx.cs" Inherits="SAM.Web.Ingenieria.WorkstatusIngenieria" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:radajaxmanager runat="server" ID="radManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblIngenieria" CssClass="Titulo" meta:resourcekey="lblIngenieria" />
    </div>
    <div class="contenedorCentral">        
            <div class="cajaFiltros" style="margin-bottom:5px;">
                <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="true" FiltroNumeroControl="true" FiltroOrdenTrabajo="true"
                    ProyectoHeaderID="proyHeader" ProyectoAutoPostBack="true" FiltroNumeroUnico="false"
                    runat="server" ID="filtroGenerico">
                </uc2:Filtro>
                <div class="divIzquierdo" > 
                    <div class ="separador">
                          <samweb:BotonProcesando meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrarClick" CssClass="boton" />                    
                    </div>
                </div>
                <p>
                </p>
            </div>
            <p>
            </p>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />                                    
        <p>
        </p>        
        <telerik:radajaxloadingpanel runat="server" id="ldPanel">
        </telerik:radajaxloadingpanel>
            <mimo:mimossradgrid runat="server" id="grdHistWorkStatus" OnItemDataBound="grdHistWorkStatus_ItemDataBound"
            showfooter="True" visible="false" allowmultirowselection="true">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                ClientDataKeyNames="HistoricoWorkStatusID" DataKeyNames="HistoricoWorkStatusID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" meta:resourcekey="gbcPorSpool" UniqueName="PorSpool" HeaderStyle-Width="50">
                            <ItemTemplate>
                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/imprimirExcel.png" runat="server" ID="hypPorSpool"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="PorJunta" HeaderStyle-Width="50" meta:resourcekey="gbcPorJunta">
                            <ItemTemplate>
                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/imprimirExcel.png" runat="server" ID="hypPorJunta" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="HistoricoWorkStatusID" DataField="HistoricoWorkStatusID" Visible="false" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcSpool" />
                        <telerik:GridBoundColumn UniqueName="Odt" DataField="Odt" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcODT" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroControl" />
                        <telerik:GridBoundColumn UniqueName="RevCliente" DataField="RevCliente" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcRevCliente" />
                        <telerik:GridBoundColumn UniqueName="RevSteelgo" DataField="RevSteelgo" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcRevSteelgo" />
                        <telerik:GridBoundColumn UniqueName="FechaHomologacion" DataField="FechaHomologacion" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFechaHomologacion" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>    
            </mimo:mimossradgrid>
    </div>
</asp:Content>