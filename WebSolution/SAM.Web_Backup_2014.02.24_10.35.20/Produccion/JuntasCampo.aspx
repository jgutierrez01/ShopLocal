<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="JuntasCampo.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCapo" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="filtroGenerico">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="grdSpools">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="loadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="valSummary" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<div class="paginaHeader">
    <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
</div>
    <asp:HiddenField runat="server" ID="hdnProyectoID" ClientIDMode="Static" />
    <div class="contenedorCentral">
    <div class="cajaFiltros" style="margin-bottom:5px;">
            <uc2:Filtro ProyectoRequerido="true"  FiltroNumeroUnico="false" OrdenTrabajoRequerido= "false" OrdenTrabajoAutoPostback="true"
                NumeroControlAutoPostBack="true" ProyectoHeaderID="proyHeader" OnDdlProyecto_SelectedIndexChanged="proyecto_Cambio" ProyectoAutoPostBack="true"
                runat="server" ID="filtroGenerico">
            </uc2:Filtro> 
        <div class="divIzquierdo" >
            <div class ="separador">
                <asp:Label meta:resourcekey="lblSpool" runat="server" ID="lblSpool" CssClass="bold"/>                
                <br />
                <div id="templateSpool" class="sys-template">
                    <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="spool">
                                {{Spool}}
                            </td>
                            <td>
                                {{Etiqueta}}
                            </td>
                        </tr>
                    </table>
                </div>
                <telerik:RadComboBox ID="rcbSpool" runat="server" Width="200px" Height="150px"
                    OnClientItemsRequesting="Sam.WebService.NumControlOnClientItemsRequestingEventHandler" EnableLoadOnDemand="true"
                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false" AutoPostBack="true"
                    OnClientItemDataBound="Sam.WebService.SpoolTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                    DropDownCssClass="liGenerico" DropDownWidth="300px">
                    <WebServiceSettings Method="ListaTablaSpoolPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                            <tr>
                                <th class="spool">
                                    <asp:Literal ID="litSpool" runat="server" meta:resourcekey="litSpool"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="litEtiqueta" runat="server" meta:resourcekey="litEtiqueta"></asp:Literal>
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                </telerik:RadComboBox>                              
           </div>
       </div>   
       <div class="divIzquierdo" > 
            <div class ="separador">
                  <samweb:BotonProcesando meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrarClick" CssClass="boton" />                    
            </div>
       </div>
        <p></p>
    </div>
    <div class="separador">
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
    </div>
    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
    <asp:PlaceHolder runat="server" ID="phSpools" Visible="False">
        <p></p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdSpools" AllowMultiRowSelection="true" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemDataBound="grdSpools_OnItemDataBound" OnItemCommand="grdSpools_ItemCommand">
            <MasterTableView AutoGenerateColumns="false"  AllowMultiColumnSorting="true">
            <CommandItemTemplate>
            </CommandItemTemplate>
                <Columns>     
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>   
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="corte_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgCortar" meta:resourcekey="imgCortar"
                                             ImageUrl="~/Imagenes/Iconos/ico_cortarB.png"
                                             CommandName="cortar" CommandArgument='<%#Eval("JuntaSpoolID") %>'
                                             OnClientClick="return Sam.Confirma(3)"
                                             Visible="false" />
                            <asp:ImageButton runat="server" ID="imgEliminarCorte" meta:resourcekey="imgElimCorte"
                                             ImageUrl="~/Imagenes/Iconos/ico_eliminarcorteB.png"
                                             CommandName="eliminar_corte" CommandArgument='<%#Eval("JuntaSpoolID") %>'
                                             OnClientClick="return Sam.Confirma(8)"
                                             Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>            
                    <telerik:GridBoundColumn UniqueName="JuntaSpoolID" DataField="JuntaSpoolID" Visible="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdSpool" UniqueName="Spool" DataField="Spool" FilterControlWidth="100" HeaderStyle-Width="140"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdOrdenTrabajo" UniqueName="NumeroOrdenTrabajo" DataField="NumeroOrdenTrabajo" FilterControlWidth="80" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn meta:resourcekey="grdNumeroControl" UniqueName="NumeroControl" DataField="NumeroControl" FilterControlWidth="80" HeaderStyle-Width="120"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdEtJunta" UniqueName="EtiquetaProduccion" DataField="EtiquetaProduccion" FilterControlWidth="50" HeaderStyle-Width="90" />
                    <telerik:GridBoundColumn meta:resourcekey="grdLocalizacion" UniqueName="Localizacion" DataField="Localizacion" FilterControlWidth="50" HeaderStyle-Width="90" />                    
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdArmado" UniqueName="ArmadoAprobado" DataField="ArmadoAprobado" HeaderStyle-Width="80" />                    
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdSoldado" UniqueName="SoldaduraAprobada" DataField="SoldaduraAprobada"  HeaderStyle-Width="80" />                    
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdInsp" UniqueName="InspeccionVisualAprobada" DataField="InspeccionVisualAprobada"  HeaderStyle-Width="100" />                    
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdHold" UniqueName="TieneHold" DataField="TieneHold"  HeaderStyle-Width="80" />                    
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
</asp:Content>