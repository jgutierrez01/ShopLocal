<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstPintura.aspx.cs" Inherits="SAM.Web.WorkStatus.LstPintura" %>

<%@ Import Namespace="Mimo.Framework.Extensions" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                    <telerik:AjaxUpdatedControl ControlID="pnRequisicion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblPintura" CssClass="Titulo" meta:resourcekey="lblPintura"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="false" FiltroNumeroUnico="false"
                ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true" FiltroNumeroControl="false"
                OrdenTrabajoAutoPostBack="true" runat="server" ID="filtroGenerico" OnRadCmbOrdenTrabajo_OnSelectedIndexChanged="filtro_SelectedIndexChanged"
                OnDdlProyecto_SelectedIndexChanged="filtro_SelectedIndexChanged"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Panel ID="pnRequisicion" runat="server">
                        <asp:Label ID="lblRequisicion" runat="server" meta:resourcekey="lblRequisicion" CssClass="bold"></asp:Label><br />
                        <asp:DropDownList ID="ddlRequisicion" runat="server" meta:resourcekey="ddlRequisicion" />
                        <span class="required" id="reqStyle" runat="server">*</span>
                        <asp:RequiredFieldValidator ID="valRequisicion" runat="server" ControlToValidate="ddlRequisicion"
                            Display="None" meta:resourcekey="valRequisicion"></asp:RequiredFieldValidator>
                    </asp:Panel>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" meta:resourcekey="btnMostrar" runat="server" CssClass="boton"
                        OnClick="btnMostrar_Click" />
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
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
            CssClass="summaryList" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemCreated="grdSpools_ItemCreated"
            AllowMultiRowSelection="true" Visible="false" OnItemDataBound="grdSpools_ItemDataBound" AllowFilteringByColumn="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="WorkstatusSpoolID,RequisicionPinturaDetalleID,OrdenTrabajoSpoolID"
                ClientDataKeyNames="WorkstatusSpoolID,RequisicionPinturaDetalleID,OrdenTrabajoSpoolID">
                <CommandItemTemplate> 
                  <div class="comandosEncabezado">
                        <asp:HyperLink ID="hypReporte" runat="server" CssClass="link" meta:resourcekey="hypReporte"></asp:HyperLink>
                        <asp:HyperLink ID="imgReporte" runat="server" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_generareporte.png"></asp:HyperLink>
                    </div>                  
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                        HeaderStyle-Width="30" />
                         <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" Resizable="false" Reorderable="false"
                            UniqueName="Ver_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlVer" meta:resourcekey="hlVer" 
                                    ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="inventario_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                        <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server" ID="hdReporte"
                                meta:resourcekey="hdReporte" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="hlDescargar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" meta:resourcekey="hlDescargar" ID="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    <telerik:GridCheckBoxColumn UniqueName="hdLiberado" DataField="Liberado" meta:resourcekey="hdLiberado"
                        FilterControlWidth="50" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdSpool" DataField="NombreSpool" meta:resourcekey="hdSpool"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroControl" DataField="NumeroControl" meta:resourcekey="hdNumeroControl"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdSistema" DataField="Sistema" meta:resourcekey="hdSistema"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdColor" DataField="Color" meta:resourcekey="hdColor"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCodigo" DataField="Codigo" meta:resourcekey="hdCodigo"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <%-- el siguiente bloque son columnas invisibles para que el radfilter pueda filtar el grid  dado que estas columnas estan en  template columns y autogenerate columns esta en false -->   --%>
                    <telerik:GridBoundColumn UniqueName="SandBlastFecha_h" DataField="SandBlastFecha"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="SandBlastReporte_h" DataField="SandBlastReporte"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="PrimarioFecha_h" DataField="PrimarioFecha" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="PrimarioReporte_h" DataField="PrimarioReporte"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="IntermedioFecha_h" DataField="IntermedioFecha"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="IntermedioReporte_h" DataField="IntermedioReporte"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="AcabadoVisualFecha_h" DataField="AcabadoVisualFecha"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="AcabadoVisualReporte_h" DataField="AcabadoVisualReporte"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="AdherenciaFecha_h" DataField="AdherenciaFecha"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="AdherenciaReporte_h" DataField="AdherenciaReporte"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="PullOffFecha_h" DataField="PullOffFecha" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="PullOffReporte_h" DataField="PullOffReporte"
                        Visible="false" />
                    <telerik:GridTemplateColumn UniqueName="SandBlast" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader" FilterControlWidth="150" DataField="ReporteSandBlast" >
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litSandBlast" runat="server" meta:resourcekey="litSandBlast" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td  width="75">
                                        <asp:Literal ID="litSandBlastFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litSandBlastReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                    <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaSandBlast").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypSandBlast" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteSandBlast")%>'></asp:HyperLink>
                                        <asp:Literal ID="litSandBlast" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteSandBlast")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Primario" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader"  FilterControlWidth="150" DataField="ReportePrimario">
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPrimario" runat="server" meta:resourcekey="litPrimario" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td width="75">
                                        <asp:Literal ID="litPrimarioFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litPrimarioReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                   <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaPrimario").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypPrimario" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReportePrimario")%>'></asp:HyperLink>
                                        <asp:Literal ID="litPrimario" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReportePrimario")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Intermedio" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader" FilterControlWidth="150" DataField="ReporteIntermedio">
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litIntermedio" runat="server" meta:resourcekey="litIntermedio" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td width="75">
                                        <asp:Literal ID="litIntermedioFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litIntermedioReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                    <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaIntermedio").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypIntermedio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteIntermedio")%>'></asp:HyperLink>
                                        <asp:Literal ID="litIntermedio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteIntermedio")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="AcabadoVisual" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader" FilterControlWidth="150"  DataField="ReporteAcabadoVisual">
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litAcabadoVisual" runat="server" meta:resourcekey="litAcabadoVisual" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td width="75">
                                        <asp:Literal ID="litAcabadoVisualFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litAcabadoVisualReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                   <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaAcabadoVisual").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypAcabadoVisual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteAcabadoVisual")%>'></asp:HyperLink>
                                        <asp:Literal ID="litAcabadoVisual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteAcabadoVisual")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Adherencia" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader" FilterControlWidth="150" DataField="ReporteAdherencia">
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litAdherencia" runat="server" meta:resourcekey="litAdherencia" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td width="75">
                                        <asp:Literal ID="litAdherenciaFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litAdherenciaReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                    <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaAdherencia").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypAdherencia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteAdherencia")%>'></asp:HyperLink>
                                        <asp:Literal ID="litAdherencia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReporteAdherencia")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="PullOff" AllowFiltering="true" Groupable="false"
                        ItemStyle-Width="200" HeaderStyle-Width="200" HeaderStyle-CssClass="rgHeader segJuntaHeader" FilterControlWidth="150" DataField="ReportePullOff">
                        <HeaderTemplate>
                            <table id="Table1" width="200">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPullOff" runat="server" meta:resourcekey="litPullOff" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td width="75">
                                        <asp:Literal ID="litPullOffFecha" runat="server" meta:resourcekey="litFecha" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="litPullOffReporte" runat="server" meta:resourcekey="litReporte" />
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2" width="200">
                                <tr>
                                   <td width="50" style="border-left:0px solid">
                                        <%# DataBinder.Eval(Container.DataItem, "FechaPullOff").SafeDateAsStringParse()%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hypPullOff" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReportePullOff")%>'></asp:HyperLink>
                                        <asp:Literal ID="litPullOff" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReportePullOff")%>'></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridCheckBoxColumn UniqueName="hdHold" DataField="Hold" meta:resourcekey="hdHold"
                        FilterControlWidth="40" HeaderStyle-Width="80" />                        
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </mimo:MimossRadGrid>
    </div>
    <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="False"
            OnClick="lnkActualizar_Click" meta:resourcekey="btnRefreshResource1" />
    </div>
</asp:Content>
