<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true" CodeBehind="DetRecepcion.aspx.cs" Inherits="SAM.Web.Materiales.DetRecepcion"  %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnMostrar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnActualiza">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNumerosUnicos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phLabels" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdNumerosUnicos">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdNumerosUnicos" LoadingPanelID="ldPanel" />
                <telerik:AjaxUpdatedControl ControlID="phLabels" />
                <telerik:AjaxUpdatedControl ControlID="valSummaryGrid" />
            </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader" />   
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblFechaRecepcion" runat="server" meta:resourcekey="lblFechaRecepcion" AssociatedControlID="dtpFechaRecepcion" />
                    <mimo:MappableDatePicker ID="dtpFechaRecepcion" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="dtpFechaRecepcion" Display="None" meta:resourcekey="valFecha" ValidationGroup="vgGuarda" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTransportista" runat="server" meta:resourcekey="lblTransportista" AssociatedControlID="ddlTransportista" />
                    <mimo:MappableDropDown ID="ddlTransportista" runat="server" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valTransportista" runat="server" ControlToValidate="ddlTransportista" Display="None" meta:resourcekey="valTransportista" ValidationGroup="vgGuarda" />
                </div>
            </div>
            <div class="divIzquierdo">
                <asp:PlaceHolder runat="server" ID="phLabels">
                    <div class="separador">
                        &nbsp;
                        <br />
                        <asp:Label ID="lblCantidadNoUnicos" runat="server" meta:resourcekey="lblCantidadNoUnicos" CssClass="bold" />
                        &nbsp;
                        <asp:Label ID="lblCantidadNoUnicosAsignados" runat="server" meta:resourcekey="lblCantidadNoUnicosAsignadosResource1" />
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton"
                        OnClick="btnGuardar_OnClick"  ValidationGroup="vgGuarda"/>
                </div>
            </div>
            <p>
            </p>
        </div>
        <p></p>
    <proy:Encabezado ID="proyEncabezado" runat="server" />
        <div>
            <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary"  ValidationGroup="vgGuarda"/>
            <asp:ValidationSummary ID="valSummaryGrid" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary"  ValidationGroup="vgGrid"/>
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdNumerosUnicos" OnNeedDataSource="grdRecepcion_OnNeedDataSource" OnItemDataBound="grdRecepcion_ItemDataBound"
            OnItemCreated="grdNumerosUnicos_ItemCreated" OnItemCommand="grdRecepcion_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" />
                       
                        <asp:HyperLink ID="lnkImprimir" runat="server" CssClass="link"  Target="_blank" meta:resourcekey="lnkImprimir" />
                        <asp:HyperLink runat="server" ID="imgImprimir" ImageUrl="~/Imagenes/Iconos/imprimirHeader.png" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ID="lnkEditar" runat="server" meta:resourcekey="imgEditar" ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="borrar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("NumeroUnicoID") %>' OnClientClick="return Sam.Confirma(1);" ValidationGroup="vgGrid">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="hdNumeroUnico" DataField="NumeroUnico" meta:resourcekey="hdNumeroUnico"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdItemCode" DataField="ItemCode" meta:resourcekey="hdItemCode"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdDescripcion" DataField="Descripcion" meta:resourcekey="hdDescripcion"
                        FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro1" DataField="Diametro1" DataFormatString="{0:#0.000}" meta:resourcekey="hdDiametro1"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro2" DataField="Diametro2" DataFormatString="{0:#0.000}" meta:resourcekey="hdDiametro2"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdRack" DataField="RackDisplay" meta:resourcekey="hdRack"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdFactura" DataField="Factura" meta:resourcekey="hdFactura"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPartidaFactura" DataField="PartidaFactura"
                        meta:resourcekey="hdPartidaFactura" FilterControlWidth="40" HeaderStyle-Width="50" />
                    <telerik:GridBoundColumn UniqueName="hdOrdenCompra" DataField="OrdenCompra" meta:resourcekey="hdOrdenCompra"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdPartidaOrden" DataField="PartidaOrden" meta:resourcekey="hdPartidaOrden"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroColada" DataField="NumeroColada" meta:resourcekey="hdNumeroColada"
                        FilterControlWidth="40" HeaderStyle-Width="135" />
                    <telerik:GridBoundColumn UniqueName="hdCertificado" DataField="Certificado" meta:resourcekey="hdCertificado"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdAceroNomenclatura" DataField="AceroNomenclatura"
                        meta:resourcekey="hdAceroNomenclatura" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdTotalRecibida" DataField="TotalRecibida" meta:resourcekey="hdTotalRecibida"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdTotalBuenEstado" DataField="TotalBuenEstado"
                        meta:resourcekey="hdTotalBuenEstado" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdTotalDanada" DataField="TotalDanana" meta:resourcekey="hdTotalDanada"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdCedula" DataField="Cedula" meta:resourcekey="hdCedula"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdProfile1" DataField="Profile1" meta:resourcekey="hdProfile1"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdProfile2" DataField="Profile2" meta:resourcekey="hdProfile2"
                        FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdProveedor" DataField="Proveedor" meta:resourcekey="hdProveedor"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="hdFabricante" DataField="Fabricante" meta:resourcekey="hdFabricante"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="hdObservaciones" DataField="Observaciones" meta:resourcekey="hdObservaciones"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
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
    <div id="btnWrapper">
        <asp:Button runat="server" CausesValidation="false" ID="btnRedirectAltaNumUnicos"
            CssClass="oculto" OnClick="btnRedirectAltaNumUnicos_Click" />
    </div>
    <div id="bntActualiza">
        <asp:Button runat="server" CausesValidation="false" ID="btnActualiza"
            CssClass="oculto" OnClick="btnActualiza_Click" />
    </div>
    <asp:HiddenField ID="hdnNumeroUnicoID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCantidadNumUnicos" runat="server" ClientIDMode="Static" />
</asp:Content>
