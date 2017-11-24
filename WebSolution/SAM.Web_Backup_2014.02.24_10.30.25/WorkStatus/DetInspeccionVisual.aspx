<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="DetInspeccionVisual.aspx.cs" Inherits="SAM.Web.WorkStatus.DetInspeccionVisual" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdVisual">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVisual" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

      <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader"
        NavigateUrl="/WorkStatus/RepInspeccionVisual.aspx" /> 

    <div class="contenedorCentral">
    <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <asp:PlaceHolder runat="server" ID="phDatos">
            <div class="cajaAzul">
                <div class="divIzquierdo ancho30">
                        <asp:Label runat="server" ID="lblNumeroReporte" CssClass="bold" meta:resourcekey="lblNumeroReporte" />
                        <asp:Label runat="server" ID="lblNumeroReporteData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblFechaReporte" CssClass="bold" meta:resourcekey="lblFechaReporte" />
                        <asp:Label runat="server" ID="lblFechaReporteData" />
                    <p></p>
                    <p></p>
                </div>
                
                <div class="divIzquierdo">
                        <asp:Label runat="server" ID="lblTotalJuntas" CssClass="bold" meta:resourcekey="lblTotalJuntas" />
                        <asp:Label runat="server" ID="lblTotalJuntasData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblJuntasAprobadas" CssClass="bold" meta:resourcekey="lblJuntasAprobadas" />
                        <asp:Label runat="server" ID="lblJuntasAprobadasData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblJuntasRechazadas" CssClass="bold" meta:resourcekey="lblJuntasRechazadas" />
                        <asp:Label runat="server" ID="lblJuntasRechazadasData" />
                    <p></p>
                </div>
                <p>
                </p>
            </div>
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrd">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdVisual" OnNeedDataSource="grdVisual_OnNeedDataSource"
                OnItemCommand="grdVisual_OnItemCommand" AllowMultiRowSelection="true">
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
                                <asp:ImageButton CommandName="Borrar" ID="btnBorrar" runat="server" meta:resourcekey="imgBorrar"
                                    ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("JuntaInspeccionVisualID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="OrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="gbcOrdenTrabajo"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" meta:resourcekey="gbcNumeroControl"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" meta:resourcekey="gbcSpool"
                            FilterControlWidth="100" HeaderStyle-Width="230" />
                        <telerik:GridBoundColumn UniqueName="Junta" DataField="Junta" meta:resourcekey="gbcJunta"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Localizacion" DataField="Localizacion" meta:resourcekey="gbcLocalizacion"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Tipo" DataField="Tipo" meta:resourcekey="gbcTipo"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" meta:resourcekey="gbcCedula"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Material1" DataField="Material1" meta:resourcekey="gbcMaterial1"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Material2" DataField="Material2" meta:resourcekey="gbcMaterial2"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" DataFormatString="{0:#0.000}" meta:resourcekey="gbcDiametro"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Hoja" DataField="Hoja" meta:resourcekey="gbcHoja"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Resultado" DataField="Resultado" meta:resourcekey="gbcResultado"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="FechaInspeccion" DataField="FechaInspeccion"
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
