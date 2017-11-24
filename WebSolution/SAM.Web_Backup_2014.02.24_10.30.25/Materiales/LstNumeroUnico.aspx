<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="LstNumeroUnico.aspx.cs" Inherits="SAM.Web.Materiales.LstNumeroUnico" meta:resourcekey="PageResource1" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
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
            <telerik:AjaxSetting AjaxControlID="btnActualiza">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNumeroUnicos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phLabels" />
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
        <asp:Label runat="server" ID="lblListaNumeroUnico" CssClass="Titulo" meta:resourcekey="lblListaNumeroUnico"></asp:Label>
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
                    <mimo:LabeledTextBox ID="txtColada" runat="server" meta:resourcekey="txtColada" MaxLength="20">
                    </mimo:LabeledTextBox>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtItemCode" runat="server" meta:resourcekey="txtItemCode"
                        MaxLength="50">
                    </mimo:LabeledTextBox>
                </div>
            </div>
            <asp:Panel ID="pnCodigos" runat="server">
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblNumeroInicial" runat="server" meta:resourcekey="lblNumUnicoInicial"
                            CssClass="bold"></asp:Label>
                        <br />
                        <asp:Label ID="lblCodigo" runat="server" CssClass="bold"></asp:Label>
                        <asp:TextBox ID="txtNumUnicoInicial" runat="server" MaxLength="20">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblNumUnicoFinal" runat="server" meta:resourcekey="lblNumUnicoFinal"
                            CssClass="bold"></asp:Label>
                        <br />
                        <asp:Label ID="lblCodigo2" runat="server" CssClass="bold"></asp:Label>
                        <asp:TextBox ID="txtNumUnicoFinal" runat="server" MaxLength="20">
                        </asp:TextBox>
                    </div>
                </div>
            </asp:Panel>
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
        <mimo:MimossRadGrid runat="server" ID="grdNumeroUnicos" OnNeedDataSource="grdNumeroUnicos_OnNeedDataSource"
            OnItemDataBound="grdNumeroUnicos_ItemDataBound" Visible="false" OnItemCreated="grdNumeroUnicos_ItemCreated">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
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
                        UniqueName="inventario_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/info.png" runat="server" ID="hypInventarios"
                                meta:resourcekey="imgInventarios" NavigateUrl="#"></asp:HyperLink>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ID="lnkEditar" runat="server" meta:resourcekey="imgEditar" ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridDateTimeColumn UniqueName="hdFechaRecepcion" DataField="FechaRecepcion" meta:resourcekey="hdFechaRecepcion"
                        FilterControlWidth="100" HeaderStyle-Width="150" DataFormatString="{0:d}" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroUnico" DataField="NumeroUnico" meta:resourcekey="hdCodigo"
                        FilterControlWidth="50" HeaderStyle-Width="100" />                    
                        <telerik:GridBoundColumn UniqueName="hdTipoMaterial" DataField="TipoMaterial" meta:resourcekey="hdTipoMaterial"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdItemCode" DataField="ItemCode" meta:resourcekey="hdItemCode"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdICDescripcion" DataField="Descripcion" meta:resourcekey="hdICDescripcion"
                        FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro1" DataField="Diametro1" DataFormatString="{0:N3}" meta:resourcekey="hdDiametro1"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro2" DataField="Diametro2" DataFormatString="{0:N3}" meta:resourcekey="hdDiametro2"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdRack" DataField="RackDisplay" meta:resourcekey="hdRack"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdFactura" DataField="Factura" meta:resourcekey="hdFactura"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdPartidaFactura" DataField="PartidaFactura"
                        meta:resourcekey="hdPartidaFactura" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdOrdenCompra" DataField="OrdenCompra" meta:resourcekey="hdOrdenCompra"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdPartidaOrdenCompra" DataField="PartidaOrden"
                        meta:resourcekey="hdPartidaOrdenCompra" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdColada" DataField="NumeroColada" meta:resourcekey="hdColada" DataFormatString="{0:N3}"
                        FilterControlWidth="50" HeaderStyle-Width="135" />
                    <telerik:GridBoundColumn UniqueName="hdCertificado" DataField="Certificado" meta:resourcekey="hdCertificado"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdAcero" DataField="AceroNomenclatura" meta:resourcekey="hdAcero"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCedula" DataField="Cedula" meta:resourcekey="hdCedula"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdProfile1" DataField="Profile1" meta:resourcekey="hdProfile1"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdProfile2" DataField="Profile2" meta:resourcekey="hdProfile2"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdProveedor" DataField="Proveedor" meta:resourcekey="hdProveedor"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTransportista" DataField="Transportista" meta:resourcekey="hdTransportista"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdFabricante" DataField="Fabricante" meta:resourcekey="hdFabricante"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalRecibida" DataField="TotalRecibida" meta:resourcekey="hdTotalRecibida" DataFormatString="{0:N3}"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalOtrasEntradas" DataField="TotalOtrasEntradas" meta:resourcekey="hdTotalOtrasEntradas" DataFormatString="{0:N3}"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalCondicionada" DataField="TotalCondicionada" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalCondicionada" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalRechazada" DataField="TotalRechazada" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalRechazada" FilterControlWidth="50" HeaderStyle-Width="100" />
                        <telerik:GridBoundColumn UniqueName="hdTotalDanado" DataField="TotalDanada" meta:resourcekey="hdTotalDanado" DataFormatString="{0:N3}"
                        FilterControlWidth="50" HeaderStyle-Width="100" /> 
                        <telerik:GridBoundColumn UniqueName="hdRecibidoNeto" DataField="TotalRecibidoNeto" meta:resourcekey="hdRecibidoNeto" DataFormatString="{0:N3}"
                        FilterControlWidth="50" HeaderStyle-Width="100" /> 
                    <telerik:GridBoundColumn UniqueName="hdTotalSalidasTemporales" DataField="TotalSalidasTemporales" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalSalidasTemporales" FilterControlWidth="50" HeaderStyle-Width="100" /> 
                        <telerik:GridBoundColumn UniqueName="hdTotalOtrasSalidas" DataField="TotalOtrasSalidas" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalOtrasSalidas" FilterControlWidth="50" HeaderStyle-Width="100" /> 
                     <telerik:GridBoundColumn UniqueName="hdTotalMerma" DataField="TotalMerma" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalMerma" FilterControlWidth="50" HeaderStyle-Width="100" />                                                               
                    <telerik:GridBoundColumn UniqueName="hdEnTransferencia" DataField="TotalEnTransferencia" DataFormatString="{0:N3}"
                        meta:resourcekey="hdEnTransferencia" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdTotalCorteSinDespachado" DataField="TotalCorteSinDespachada" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalCorteSinDespachado" FilterControlWidth="50" HeaderStyle-Width="100" /> 
                    <telerik:GridBoundColumn UniqueName="hdTotalDespachado" DataField="TotalDespachada" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalDespachado" FilterControlWidth="50" HeaderStyle-Width="100" />  
                        <telerik:GridBoundColumn UniqueName="hdTotalDespachadoICE" DataField="TotalDespachadaICE" DataFormatString="{0:N3}"
                        meta:resourcekey="hdTotalDespachadoICE" FilterControlWidth="50" HeaderStyle-Width="100" />  
                        <telerik:GridBoundColumn UniqueName="hdInventarioActual" DataField="TotalInventarioActual" DataFormatString="{0:N3}"
                        meta:resourcekey="hdInventarioActual" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridCheckBoxColumn UniqueName="hdMarcadoAsme" DataField="MarcadoAsme" meta:resourcekey="hdMarcadoAsme"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridCheckBoxColumn UniqueName="hdMarcadoGolpe" DataField="MarcadoGolpe"
                        meta:resourcekey="hdMarcadoGolpe" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridCheckBoxColumn UniqueName="hdMarcadoPintura" DataField="MarcadoPintura"
                        meta:resourcekey="hdMarcadoPintura" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdPruebas" DataField="PruebasHidrostaticas"
                        meta:resourcekey="hdPruebas" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdEstatus" DataField="Estatus" meta:resourcekey="hdEstatus"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdObservaciones" DataField="Observaciones" meta:resourcekey="hdObservaciones"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre1" DataField="CampoLibre1" meta:resourcekey="hdCampoLibre1"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre2" DataField="CampoLibre2" meta:resourcekey="hdCampoLibre2"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre3" DataField="CampoLibre3" meta:resourcekey="hdCampoLibre3"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre4" DataField="CampoLibre4" meta:resourcekey="hdCampoLibre4"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre5" DataField="CampoLibre5" meta:resourcekey="hdCampoLibre5"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre6" DataField="CampoLibre6" meta:resourcekey="hdCampoLibre6"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre7" DataField="CampoLibre7" meta:resourcekey="hdCampoLibre7"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre8" DataField="CampoLibre8" meta:resourcekey="hdCampoLibre8"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre9" DataField="CampoLibre9" meta:resourcekey="hdCampoLibre9"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCampoLibre10" DataField="CampoLibre10" meta:resourcekey="hdCampoLibre10"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
    <div id="bntActualiza">
        <asp:Button runat="server" CausesValidation="false" ID="btnActualiza"
            CssClass="oculto" OnClick="btnActualiza_Click" />
    </div>
</asp:Content>
