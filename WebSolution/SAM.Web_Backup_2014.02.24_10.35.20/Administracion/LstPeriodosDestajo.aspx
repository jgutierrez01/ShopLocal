<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="LstPeriodosDestajo.aspx.cs" Inherits="SAM.Web.Administracion.LstPeriodosDestajo" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdPersonasDestajo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPersonasDestajo" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina ID="titulo" runat="server" meta:resourcekey="lblTitulo" BotonRegresarVisible="false" />
     <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFechaIni" meta:resourcekey="lblFechaIni" AssociatedControlID="dtpFechaInicio" />
                    <mimo:MappableDatePicker runat="server" ID="dtpFechaInicio" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFechaFin" meta:resourcekey="lblFechaFin" AssociatedControlID="dtpFechaFin" />
                    <mimo:MappableDatePicker runat="server" ID="dtpFechaFin" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button runat="server" ID="btnMostrar" CssClass="boton" OnClick="btnMostrar_Click" meta:resourcekey="btnMostrar" CausesValidation="false" />
                </div>
            </div>
            <p></p>
        </div>
        <div style="margin-top:10px;">
            <asp:PlaceHolder runat="server" ID="phListado" Visible="false">
                <asp:ValidationSummary runat="server" ID="valSummary" meta:resourcekey="valSummary" CssClass="summaryList" />
                <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
                <mimo:MimossRadGrid runat="server" ID="grdPeriodos" OnNeedDataSource="grdPeriodos_OnNeedDataSource" OnItemCommand="grdPeriodos_OnItemCommand" OnItemDataBound="grdPeriodos_OnItemDataBound">
                    <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="PeriodoDestajoID">
                        <CommandItemTemplate>
                            <div class="comandosEncabezado">
                                <samweb:AuthenticatedHyperLink meta:resourcekey="lnkAgregar" runat="server" ID="lnkAgregar" NavigateUrl="~/Administracion/AltaPeriodoDestajo.aspx" CssClass="link" />
                                <samweb:AuthenticatedHyperLink meta:resourcekey="imgAgregar" runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Administracion/AltaPeriodoDestajo.aspx" />
                                <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" CssClass="link" />
                                <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" />
                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="ver_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <samweb:AuthenticatedHyperLink meta:resourcekey="hlVer" runat="server" ID="hlVer" ImageUrl="~/Imagenes/Iconos/info.png" NavigateUrl='<%#Eval("PeriodoDestajoID","~/Administracion/DetPeriodoDestajo.aspx?ID={0}") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" OnClientClick="return Sam.Confirma(1);" CommandArgument='<%#Eval("PeriodoDestajoID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="cerrar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="imgCerrar" meta:resourcekey="imgCerrar" ImageUrl="~/Imagenes/Iconos/ico_cerrarperiodo.png" CommandName="cerrar" OnClientClick="return Sam.Confirma(19);" CommandArgument='<%#Eval("PeriodoDestajoID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="imprimir_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <asp:HyperLink meta:resourcekey="hlImprimir" runat="server" ID="hlImprimir" ImageUrl="~/Imagenes/Iconos/imprimirExcel.png" NavigateUrl='<%#Eval("PeriodoDestajoID","~/Administracion/ImportaPeriodoDestajo.aspx?ID={0}") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="Anio" DataField="Anio" meta:resourcekey="grdAnio" HeaderStyle-Width="60" FilterControlWidth="25" Groupable="false" />
                            <telerik:GridBoundColumn UniqueName="Semana" DataField="Semana" meta:resourcekey="grdSemana" HeaderStyle-Width="60" FilterControlWidth="25" Groupable="false" DataFormatString="{0:N0}" />
                            <telerik:GridDateTimeColumn UniqueName="FechaInicio" DataField="FechaInicio" meta:resourcekey="grdFechaIni" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" DataFormatString="{0:d}"/>
                            <telerik:GridDateTimeColumn UniqueName="FechaFin" DataField="FechaFin" meta:resourcekey="grdFechaFin" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" DataFormatString="{0:d}" />
                            <telerik:GridBoundColumn UniqueName="EstatusTexto" DataField="EstatusTexto" meta:resourcekey="grdEstatus" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" />
                            <telerik:GridBoundColumn UniqueName="TotalAPagar" DataField="TotalAPagar" meta:resourcekey="grdTotal" DataFormatString="{0:C}" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                            <telerik:GridBoundColumn UniqueName="CantidadTuberos" DataField="CantidadTuberos" meta:resourcekey="grdCantTuberos" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" DataFormatString="{0:N0}" />
                            <telerik:GridBoundColumn UniqueName="CantidadSoldadores" DataField="CantidadSoldadores" meta:resourcekey="grdCantSoldadores" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" DataFormatString="{0:N0}" />
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
    </div>
</asp:Content>
