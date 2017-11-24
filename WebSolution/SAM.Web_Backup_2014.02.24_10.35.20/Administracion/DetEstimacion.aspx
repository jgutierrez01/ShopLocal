<%@ Page Language="C#" MasterPageFile="~/Masters/Administracion.Master" AutoEventWireup="true"
    CodeBehind="DetEstimacion.aspx.cs" Inherits="SAM.Web.Administracion.DetEstimacion" %>

<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdEstimacionSpool">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdEstimacionSpool" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalSpools" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdEstimacionJunta">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdEstimacionJunta" />
                    <telerik:AjaxUpdatedControl ControlID="lblTotalJuntas" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader" NavigateUrl="~/Administracion/LstEstimado.aspx" />
    <div class="contenedorCentral">
        <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <div class="cajaFiltros">
            <div class="divIzquierdo ancho80">
                <div class="divIzquierdo ancho30">
                    <asp:Label ID="lblNumeroEstimacionLabel" runat="server" meta:resourcekey="lblNumeroEstimacionLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblNumeroEstimacion" runat="server" meta:resourcekey="lblNumeroEstimacion" />
                    <p>
                    </p>
                    <asp:Label ID="lblFechaEstimacionLabel" runat="server" meta:resourcekey="lblFechaEstimacionLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblFechaEstimacion" runat="server" meta:resourcekey="lblFechaEstimacion" />
                </div>
                <div class="divIzquierdo ancho30">
                    <div class="divIzquierdo ancho40">
                        <asp:Label ID="lblTotalJuntasLabel" runat="server" meta:resourcekey="lblTotalJuntasLabel"
                            CssClass="bold" />
                    </div>
                    <div class="divIzquierdo ancho40">
                        <asp:Label ID="lblTotalJuntas" runat="server" meta:resourcekey="lblTotalJuntas" />
                        <p>
                        </p>
                    </div>
                    <p>
                    </p>
                    <div class="divIzquierdo ancho40">
                        <asp:Label ID="lblTotalSpoolslabel" runat="server" meta:resourcekey="lblTotalSpoolslabel"
                            CssClass="bold" />
                    </div>
                    <div class="divIzquierdo ancho40">
                        <asp:Label ID="lblTotalSpools" runat="server" meta:resourcekey="lblTotalSpools" />
                    </div>
                </div>
                <p>
                </p>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div class="divIzquierdo ancho45">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
            </telerik:RadAjaxLoadingPanel>
            <mimo:MimossRadGrid ID="grdEstimacionJunta" runat="server" OnNeedDataSource="grdEstimacionJunta_OnNeedDataSource"
                OnItemCreated="grdEstimacionJunta_ItemCreated" AllowMultiRowSelection="true">
                <ClientSettings Selecting-AllowRowSelect="true" />
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ClientDataKeyNames="EstimadoJuntaID"
                    DataKeyNames="EstimadoJuntaID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <div class="divIzquierdo link">
                                <asp:Label runat="server" ID="lblJuntas" CssClass="Titulo" meta:resourcekey="lblJuntas"
                                    Text="   Juntas"></asp:Label>
                            </div>
                            <asp:LinkButton runat="server" ID="lnkBorrarSeleccion" CausesValidation="false" meta:resourcekey="lnkBorrarSeleccion"
                                OnClick="lnkBorrarSeleccionJunta_onClick" OnClientClick="return Sam.Confirma(1);"
                                CssClass="link" Text="Borrar Seleccion" />
                            <asp:ImageButton runat="server" ID="imgBorrarSeleccion1" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CausesValidation="false" AlternateText="BorrarSeleccion" CssClass="imgEncabezado"
                                OnClick="imeBorrarSeleccionJunta_onClick" meta:resourcekey="imgBorrarSeleccion"
                                OnClientClick="return Sam.Confirma(1);" />
                            <asp:HyperLink runat="server" ID="hlExporta" Text="Exportar a Excel" CssClass="link" />
                            <asp:HyperLink runat="server" ID="hlExportaImagen" ImageUrl="~/Imagenes/Iconos/excel.png"
                                CssClass="imgEncabezado" />
                        </div>
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h"
                            HeaderStyle-Width="30" />
                        <telerik:GridBoundColumn UniqueName="HdNumeroControl" DataField="NumeroControl" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdNumeroControl" />
                        <telerik:GridBoundColumn UniqueName="HdNombreSpool" DataField="NombreSpool" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdNombreSpool" />
                        <telerik:GridBoundColumn UniqueName="HdEtiqueta" DataField="Etiqueta" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdEtiqueta" />
                        <telerik:GridBoundColumn UniqueName="HdTipoJunta" DataField="TipoJunta" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdTipoJunta" />
                        <telerik:GridBoundColumn UniqueName="HdDiametro" DataField="Diametro" DataFormatString="{0:#0.000}"
                            HeaderStyle-Width="100" FilterControlWidth="50" meta:resourcekey="HdDiametro" />
                        <telerik:GridBoundColumn UniqueName="HdMaterial" DataField="Material" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdMaterial" />
                        <telerik:GridBoundColumn UniqueName="HdCedula" DataField="Cedula" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdCedula" />
                        <telerik:GridBoundColumn UniqueName="HdConcepto" DataField="Concepto" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdConcepto" />
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
        <%---------------------------------------------------------------------------------------------------------------------------------------------%>
        <div class="divDerecho ancho45">
            <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
            </telerik:RadAjaxLoadingPanel>
            <mimo:MimossRadGrid ID="grdEstimacionSpool" OnNeedDataSource="grdEstimacionSpool_OnNeedDataSource"
                OnItemCreated="grdEstimacionSpool_ItemCreated" runat="server" AllowMultiRowSelection="true">
                <ClientSettings Selecting-AllowRowSelect="true" />
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ClientDataKeyNames="EstimacionSpoolId"
                    DataKeyNames="EstimacionSpoolID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <div class="divIzquierdo link">
                                <asp:Label runat="server" ID="lblSpool" CssClass="Titulo" meta:resourcekey="lblSpool"
                                    Text="Spools"></asp:Label>
                            </div>
                            <asp:LinkButton runat="server" ID="lnkBorrarSeleccionSpool" CausesValidation="false"
                                OnClick="lnkBorrarSeleccionSpool_onClick" meta:resourcekey="lnkBorrarSeleccionSpool"
                                CssClass="link" Text="Borrar Seleccion" OnClientClick="return Sam.Confirma(1);" />
                            <asp:ImageButton runat="server" ID="imgBorrarSeleccionSpool" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CausesValidation="false" AlternateText="BorrarSeleccionSpool" CssClass="imgEncabezado"
                                OnClick="imeBorrarSeleccionSpool_onClick" meta:resourcekey="imgBorrarSeleccionSpool"
                                OnClientClick="return Sam.Confirma(1);" />
                            <asp:HyperLink ID="hlExportarSool" Text="Exportar Exel" runat="server" CssClass="link"></asp:HyperLink>
                            <asp:HyperLink runat="server" ID="hlExportaImagenSpool" ImageUrl="~/Imagenes/Iconos/excel.png"
                                AlternateText="ExportarExcelSpool" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h"
                            HeaderStyle-Width="30" />
                        <telerik:GridBoundColumn UniqueName="HdNumeroControlSpool" DataField="NumeroControl"
                            HeaderText="Numero Control" HeaderStyle-Width="100" FilterControlWidth="50" meta:resourcekey="HdNumeroControlSpool" />
                        <telerik:GridBoundColumn UniqueName="HdSpool" DataField="Spool" HeaderText="Spool"
                            HeaderStyle-Width="100" FilterControlWidth="50" meta:resourcekey="HdSpool" />
                        <telerik:GridBoundColumn UniqueName="HdPdiSpool" DataField="Pdi" HeaderText="PDI"
                            HeaderStyle-Width="100" FilterControlWidth="50" meta:resourcekey="HdPdiSpool" />
                        <telerik:GridBoundColumn UniqueName="HdMaterial" DataField="Material" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdMaterial" />
                        <telerik:GridBoundColumn UniqueName="HdCedula" DataField="Cedula" HeaderStyle-Width="100"
                            FilterControlWidth="50" meta:resourcekey="HdCedula" />
                        <telerik:GridBoundColumn UniqueName="HdConceptoSpool" DataField="Concepto" HeaderText="Concepto"
                            HeaderStyle-Width="100" FilterControlWidth="50" meta:resourcekey="HdConceptoSpool" />
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
        <p>
        </p>
    </div>
</asp:Content>
