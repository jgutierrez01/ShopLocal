<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
         CodeBehind="ReporteSpoolPND.aspx.cs" Inherits="SAM.Web.WorkStatus.ReporteSpoolPND" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdReporteSpoolPND">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReporteSpoolPND" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblReportePND" CssClass="Titulo" meta:resourcekey="lblReportePND" Text="REPORTE SPOOL - PND" />
    </div>

    <div class="contenedorCentral">
        <div class="cajaFiltros">

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" />
                </div>
            </div>

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" Text="Desde:" meta:resourcekey="lblDesde"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" Text="Hasta:" meta:resourcekey="lblHasta"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroDeReporte" runat="server" Text="Numero de Reporte:" meta:resourcekey="lblNumeroDeReporte"
                        CssClass="bold" />
                    <br />
                    <asp:TextBox ID="txtNumeroDeReporte" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTipoDePrueba" runat="server" Text="Tipo de Prueba:" meta:resourcekey="lblTipoDePrueba"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoDePrueba" meta:resourcekey="ddlTipoDePrueba"
                        AutoPostBack="false" />
                     <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvTipoPrueba" runat="server" ControlToValidate="ddlTipoDePrueba"
                        InitialValue="" meta:resourcekey="valTipoPrueba" Display="None" CssClass="bold" />
                </div>
            </div>

            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        CssClass="boton" OnClick="btnMostrar_OnClick" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        </div>
        <asp:ValidationSummary ID="valSummary" HeaderText="Errores" runat="server" DisplayMode="BulletList"
            CssClass="summaryList" meta:resourcekey="valSummary" />
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid ID="grdReporteSpoolPND" runat="server" OnNeedDataSource="grdReporteSpoolPND_OnNeedDataSource"
                OnItemCommand="grdReporteSpoolPND_ItemCommand" OnItemDataBound="grdReporteSpoolPND_OnItemDataBound">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton runat="server" ID="lnkActualizar" CausesValidation="false" meta:resourcekey="lnkActualizar"
                                CssClass="link" OnClick="lnkActualizar_onClick" />
                            <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                                CausesValidation="false" AlternateText="Actualizar" CssClass="imgEncabezado"
                                OnClick="lnkActualizar_onClick" meta:resourcekey="imgActualizar" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="hlVer_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" meta:resourcekey="hlVer" ID="hlVer" ImageUrl="~/Imagenes/Iconos/info.png"
                                    NavigateUrl='<%#Eval("ReporteSpoolPndID","~/WorkStatus/DetReporteSpoolPnd.aspx?ID={0}") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="editarRegistro" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlEditarSpoolPND"
                                ImageUrl="~/Imagenes/Iconos/editar.png" meta:resourcekey="hlEditarSpoolPND" runat="server" Visible="false"></asp:HyperLink>
                        </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="inventario_h" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server"
                                    ID="hdReporte" Visible="false" meta:resourcekey="hdReporte" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>  
                        <telerik:GridTemplateColumn UniqueName="hlDescargar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" meta:resourcekey="hlDescargar" ID="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("ReporteSpoolPndID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="HdNumeroDeReporte" DataField="NumeroDeReporte"
                            HeaderText="Numero de Reporte" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdNumeroDeReporte" />
                        <telerik:GridBoundColumn UniqueName="HdFecha" DataField="Fecha" HeaderText="Fecha"
                            DataFormatString="{0:d}" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"
                            meta:resourcekey="HdFecha" />
                        <telerik:GridBoundColumn UniqueName="HdTipoDePrueba" DataField="TipoDePruebaSpool" HeaderText="Tipo de Prueba"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdTipoDePrueba" />
                        <telerik:GridBoundColumn UniqueName="HdSpoolsTotales" DataField="SpoolsTotales" HeaderText="Spools Totales" DataFormatString="{0:N0}"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdSpoolsTotales" />
                        <telerik:GridBoundColumn UniqueName="HdSpoolsAprobados" DataField="SpoolsAprobados" DataFormatString="{0:N0}"
                            HeaderText="Spools Aprobados" meta:resourcekey="HdSpoolsAprobados" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="HdSpoolsRechazados" DataField="SpoolsRechazados" DataFormatString="{0:N0}"
                            HeaderText="Spools Rechazados" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdSpoolsRechazados" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
                    OnClick="btnRefresh_Click" />
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>