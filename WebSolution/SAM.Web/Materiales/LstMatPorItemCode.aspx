<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true" CodeBehind="LstMatPorItemCode.aspx.cs" Inherits="SAM.Web.Materiales.lstMatrialesPorItemCode" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">

 <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                    <telerik:AjaxUpdatedControl ControlID="pnCodigos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdItemCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdItemCode" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" /><br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        OnSelectedIndexChanged="ddlProyectoSelectedItemChanged" AutoPostBack="True" meta:resourcekey="ddlProyectoResource1" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" Display="None" meta:resourcekey="valProyecto"></asp:RequiredFieldValidator>
                </div>
            </div>
          
          
            <div class="divIzquierdo">
                <div class="separador">
                    <samweb:BotonProcesando ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        OnClick="btnMOstrar_Click" CssClass="boton" />
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
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummaryResource1"
            CssClass="summaryList" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdItemCode" OnNeedDataSource="grdItemCode_OnNeedDataSource" OnDetailTableDataBind="grdItemCode_OnDetailTableDataBind" Visible="false" OnItemDataBound="grdItemCode_OnDataBound" OnItemCreated="grdItemCode_ItemCreated">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="ItemCodeID,Diametro1,Diametro2"> 
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="lnkExportar" Text="Exportar a Excel" runat="server" meta:resourcekey="lnkExportar"></asp:HyperLink>
                        <asp:HyperLink runat="server" ID="imgExportar" ImageUrl="~/Imagenes/Iconos/excel.png"  AlternateText="ExportaImagenNumeroUnico" meta:resourcekey="imgExportar"/>
                        <span>&nbsp&nbsp&nbsp&nbsp</span>
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick"
                            CausesValidation="false" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                            CausesValidation="false" OnClick="lnkActualizar_onClick" AlternateText="Actualizar"
                            CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                   <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="numerosUnicos_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/info.png" runat="server" ID="hypNumerosUnicos"></asp:HyperLink>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="hdItemCode" DataField="CodigoItemCode" meta:resourcekey="hdItemCode"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdICDescripcion" DataType="System.String" DataField="DescripcionEspanol" meta:resourcekey="hdICDescripcion"
                        FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro1" DataField="Diametro1" DataFormatString="{0:N3}" meta:resourcekey="hdDiametro1"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro2" DataField="Diametro2" DataFormatString="{0:N3}" meta:resourcekey="hdDiametro2"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTipoMaterial" DataField="TipoMaterialNombreEspañol" meta:resourcekey="hdTipoMaterial"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalIngenieria" DataField="CantidadIngenieria" DataFormatString="{0:N}" meta:resourcekey="hdTotalIngenieria"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                     <telerik:GridBoundColumn UniqueName="hdTotalRecibida" DataField="CantidadRecibida" DataFormatString="{0:N}" meta:resourcekey="hdTotalRecibida"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                         <telerik:GridBoundColumn UniqueName="hdTotalOtrasEntradas" DataField="TotalEntradaOtrosProcesos" DataFormatString="{0:N}" meta:resourcekey="hdTotalOtrasEntradas"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                       <telerik:GridBoundColumn UniqueName="hdTotalCondicionada" DataField="TotalCondicionada" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalCondicionada" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalRechazada" DataField="TotalRechazada" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalRechazada" FilterControlWidth="50" HeaderStyle-Width="100" />   
                       <telerik:GridBoundColumn UniqueName="hdTotalDanado" DataField="CantidadDanada" meta:resourcekey="hdTotalDanado" DataFormatString="{0:N}"
                        FilterControlWidth="50" HeaderStyle-Width="100" />                                                      
                    <telerik:GridBoundColumn UniqueName="hdRecibidoNeto" DataField="RecibidoNeto" DataFormatString="{0:N}"
                        meta:resourcekey="hdRecibidoNeto" FilterControlWidth="50" HeaderStyle-Width="100" />                 
                         <telerik:GridBoundColumn UniqueName="hdEntradasICE" DataField="CantidadDespachadaEquivalente" DataFormatString="{0:N}"
                        meta:resourcekey="hdEntradasICE" FilterControlWidth="50" HeaderStyle-Width="100" />  
                        <telerik:GridBoundColumn UniqueName="hdSalidasTemporales" DataField="TotalSalidasTemporales" DataFormatString="{0:N}"
                        meta:resourcekey="hdSalidasTemporales" FilterControlWidth="50" HeaderStyle-Width="100" />
                            <telerik:GridBoundColumn UniqueName="hdTotalOtrasSalidas" DataField="TotalOtrasSalidas" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalOtrasSalidas" FilterControlWidth="50" HeaderStyle-Width="100" />
                     <telerik:GridBoundColumn UniqueName="hdTotalMermas" DataField="TotalMerma" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalMermas" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalOrdenTrabajo" DataField="CantidadOrdenTrabajo" DataFormatString="{0:N}" meta:resourcekey="hdTotalOrdenTrabajo"
                        FilterControlWidth="50" HeaderStyle-Width="100" />      
                    <telerik:GridBoundColumn UniqueName="hdPreparacionICE" DataField="CantidadEnPreparacionEquivalente" DataFormatString="{0:N}"
                        meta:resourcekey="hdPreparacionICE" FilterControlWidth="50" HeaderStyle-Width="100" />                                                
                        <telerik:GridBoundColumn UniqueName="hdCorteICE" DataField="CantidadCortadaICE" DataFormatString="{0:N}"
                        meta:resourcekey="hdCorteICE" FilterControlWidth="50" HeaderStyle-Width="100" />      
                        <%--<telerik:GridBoundColumn UniqueName="hdPreparacionDesdeICE" DataField="CantidadEnPreparacionDesdeICE"
                        meta:resourcekey="hdPreparacionDesdeICE" FilterControlWidth="50" HeaderStyle-Width="100" />
                        <telerik:GridBoundColumn UniqueName="hdCorteDesdeICE" DataField="CantidadCortadoDesdeICE"
                        meta:resourcekey="hdCorteDesdeICE" FilterControlWidth="50" HeaderStyle-Width="100" />--%>              
                     <telerik:GridBoundColumn UniqueName="hdEnTransferencia" DataField="InventarioTransferenciaCorte" DataFormatString="{0:N}"
                        meta:resourcekey="hdEnTransferencia" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalCorteSinDespachado" DataField="TotalCorteSinDespacho" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalCorteSinDespachado" FilterControlWidth="50" HeaderStyle-Width="100" /> 
                        <telerik:GridBoundColumn UniqueName="hdDespachadoICE" DataField="CantidadDespachadaEquivalente" DataFormatString="{0:N}"
                        meta:resourcekey="hdDespachadoICE" FilterControlWidth="50" HeaderStyle-Width="100" />      
                        <telerik:GridBoundColumn UniqueName="hdTotalDespachado" DataField="TotalDespachado" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalDespachado" FilterControlWidth="50" HeaderStyle-Width="100" />      
                        <telerik:GridBoundColumn UniqueName="hdSalidaICE" DataField="TotalDespachadoParaICE" DataFormatString="{0:N}"
                        meta:resourcekey="hdSalidaICE" FilterControlWidth="50" HeaderStyle-Width="100" />      
                        <telerik:GridBoundColumn UniqueName="hdTotalPorDespachar" DataField="TotalPorDespachar" DataFormatString="{0:N}"
                        meta:resourcekey="hdTotalPorDespachar" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdInventarioActual" DataField="InventarioFisico" DataFormatString="{0:N}"
                        meta:resourcekey="hdInventarioActual" FilterControlWidth="50" HeaderStyle-Width="100" />                    
                     <telerik:GridBoundColumn UniqueName="hdInventarioCongeladoEquivalente" DataField="CantidadCongeladaEquivalente" DataFormatString="{0:N}"
                        meta:resourcekey="hdInventarioCongeladoEquivalente" FilterControlWidth="50" HeaderStyle-Width="100" />                  
                    <telerik:GridBoundColumn UniqueName="hdInventarioCongelado" DataField="InventarioCongelado" DataFormatString="{0:N}"
                        meta:resourcekey="hdInventarioCongelado" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdInventarioDisponibleCruce" DataField="InventarioDisponibleCruce" DataFormatString="{0:N}"
                        meta:resourcekey="hdInventarioDisponibleCruce" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdInventarioDisponibleCruceEquivalente" DataField="InventarioDisponibleCruceEquivalente" DataFormatString="{0:N}"
                        meta:resourcekey="hdInventarioDisponibleCruceEquivalente" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalDisponibleCruce" DataField="TotalDisponibleCruce" meta:resourcekey="hdTotalDisponibleCruce" DataFormatString="{0:N}"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>

                 <DetailTables>
                    <telerik:GridTableView AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" EnableHeaderContextFilterMenu="false" EnableHeaderContextMenu="false" AutoGenerateColumns="false" Width="700">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="CodigoEq" DataField="CodigoEq" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="htCodigo" />
                            <telerik:GridBoundColumn UniqueName="DescripcionEq" DataField="DescripcionEq" HeaderStyle-Width="350" FilterControlWidth="100" Groupable="false" meta:resourcekey="htDescripcion" />
                            <telerik:GridBoundColumn UniqueName="D1Eq" DataField="D1Eq" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro1" />
                            <telerik:GridBoundColumn UniqueName="D2Eq" DataField="D2Eq" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro2" />
                            <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>

</asp:Content>
