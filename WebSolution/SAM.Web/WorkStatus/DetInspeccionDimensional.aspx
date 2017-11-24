<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="DetInspeccionDimensional.aspx.cs" Inherits="SAM.Web.WorkStatus.DetInspeccionDimensional" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
      <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdDimensional">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDimensional" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader"
        NavigateUrl="~/WorkStatus/RepInspeccionDimensional.aspx" /> 
    <div class="contenedorCentral">
        
     <uc1:Header ID="proyHeader" runat="server" Visible="true" />
      <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />

        <asp:PlaceHolder runat="server" ID="phDatos">
            <div class="cajaAzul">
                <div class="divIzquierdo ancho30">
                        <asp:Label runat="server" ID="lblNumeroReporte" CssClass="bold" meta:resourcekey="lblNumeroReporte" />
                        <asp:Label runat="server" ID="lblNumeroReporteData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblFechaReporte" CssClass="bold" meta:resourcekey="lblFechaReporte" />
                        <asp:Label runat="server" ID="lblFechaReporteData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblTipoReporte" CssClass="bold" meta:resourcekey="lblTipoReporte" />
                        <asp:Label runat="server" ID="lblTipoReporteData" />
                    <p></p>
                </div>
                <div class="divIzquierdo">
                        <asp:Label runat="server" ID="lblTotalSpools" CssClass="bold" meta:resourcekey="lblTotalSpools" />
                        <asp:Label runat="server" ID="lblTotalSpoolsData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblSpoolsAprobados" CssClass="bold" meta:resourcekey="lblSpoolsAprobados" />
                        <asp:Label runat="server" ID="lblSpoolsAprobadosData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblSpoolsRechazados" CssClass="bold" meta:resourcekey="lblSpoolsRechazados" />
                        <asp:Label runat="server" ID="lblSpoolsRechazadosData" />
                    <p></p>
                </div>
                <p>
                </p>
            </div>
        </asp:PlaceHolder>
        <p>
        </p>      
        
        <asp:PlaceHolder runat="server" ID="phGrd">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdDimensional" OnNeedDataSource="grdDimensional_OnNeedDataSource"
                OnItemCommand="grdDimensional_OnItemCommand" AllowMultiRowSelection="true">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("ReporteDimensionalDetalleID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="OrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="gbcOrdenTrabajo"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" meta:resourcekey="gbcNumeroControl"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" meta:resourcekey="gbcSpool"
                            FilterControlWidth="100" HeaderStyle-Width="230" />
                        <telerik:GridBoundColumn UniqueName="Hoja" DataField="Hoja" meta:resourcekey="gbcHoja"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Resultado" DataField="Resultado" meta:resourcekey="gbcResultado"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="FechaInspeccion" DataField="FechaLiberacion"
                            meta:resourcekey="gbcFechaInspeccion" DataFormatString="{0:d}" FilterControlWidth="80"
                            HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Observaciones" DataField="Observaciones" meta:resourcekey="gbcObservaciones"
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
        </asp:PlaceHolder>
    </div>
</asp:Content>
