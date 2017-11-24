<%@ Page Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true"
    CodeBehind="LstCertificacion.aspx.cs" Inherits="SAM.Web.Calidad.LstCertificacion" %>

<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="pnlEnDossier" />
                    <telerik:AjaxUpdatedControl ControlID="pnlFiltrosSegmentos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdCertificacion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCertificacion" LoadingPanelID="grdPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label ID="lblTitulo" runat="server" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" FiltroNumeroUnico="false" FiltroOrdenTrabajo="false"
                FiltroNumeroControl="false" ProyectoHeaderID="proyHeader" ProyectoAutoPostBack="true"
                runat="server" OnDdlProyecto_SelectedIndexChanged="proyecto_Cambio" ID="filtroGenerico">
            </uc2:Filtro>
            <asp:Panel ID="pnlFiltrosSegmentos" runat="server" Visible="false">
                <asp:Repeater ID="repFiltros" runat="server">
                    <ItemTemplate>
                        <div class="divIzquierdo">
                            <div class="separador">
                                <asp:HiddenField ID="hdnSegmento" runat="server" Value='<%#Eval("Segmento") %>' />
                                <mimo:LabeledTextBox ID="txtFiltro" runat="server" Label='<%#Eval("NombreSegmento") %>'>
                                </mimo:LabeledTextBox>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
            <div class="divIzquierdo">
                <div class="separador">
                    <samweb:BotonProcesando CssClass="boton" ID="btnMostrar" OnClick="btnMostrarClick"
                        meta:resourcekey="btnMostrar" runat="server" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        <asp:Panel ID="pnlEnDossier" CssClass="cajaAzul" runat="server" Visible="false">
            <div class="ancho100">
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkTrazabilidad" meta:resourcekey="chkTrazabilidad" runat="server"
                        Enabled="false" CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkWPS" meta:resourcekey="chkWPS" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkInspeccionVisual" meta:resourcekey="chkInspeccionVisual" runat="server"
                        Enabled="false" CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkLibDimensional" meta:resourcekey="chkLibDimensional" runat="server"
                        Enabled="false" CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkEspesores" meta:resourcekey="chkEspesores" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkRT" meta:resourcekey="chkRT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPT" meta:resourcekey="chkPT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPWHT" meta:resourcekey="chkPWHT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkDurezas" meta:resourcekey="chkDurezas" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkRTPostTT" meta:resourcekey="chkRTPostTT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPTPostTT" meta:resourcekey="chkPTPostTT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPreheat" meta:resourcekey="chkPreheat" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkUT" meta:resourcekey="chkUT" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPintura" meta:resourcekey="chkPintura" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkEmbarque" meta:resourcekey="chkEmbarque" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <p>
                </p>
            </div>
        </asp:Panel>
        <div class="ancho100">
            <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList"
                class="summaryList" meta:resourcekey="valSummary" />
        </div>
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="grdPanel" />
        <mimo:MimossRadGrid ID="grdCertificacion" runat="server" OnNeedDataSource="grdCertificacion_OnNeedDataSource"
            OnItemCreated="grdCertificacion_ItemCreated" OnItemCommand="grdCertificacion_ItemCommand"
            OnItemDataBound="grdCertificacion_ItemDataBound" CssClass="RadGrid RadGrid_SAMOrange certificacion" FreezeColumnStartIndex="1"
            PageSize="20" AllowMultiRowSelection="true">
            <ClientSettings Selecting-AllowRowSelect="true" />
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" DataKeyNames="SpoolID"
                ClientDataKeyNames="SpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                     <%--   <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick"
                            CausesValidation="false" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/Iconos/actualizar.png"
                            CausesValidation="false" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" />--%>
                        <%--<a id="lnkDescargar" onclick="Sam.Calidad.Certificacion_DescargarReportes" >
                            <asp:Literal runat="server" ID="litDescargar" CssClass="imgEncabezado" meta:resourcekey="lnkDescargar"/>
                        </a>--%>
                        <a id="lnkImgDescargar" href="javascript:Sam.Calidad.Certificacion_DescargarReportes('<%#grdCertificacion.ClientID%>')"
                            class="link">
                            <asp:Literal runat="server" ID="litDescargar" meta:resourcekey="lnkDescargar" />
                            <asp:Image runat="server" ID="imgDescargarSeleccionados" src="/Imagenes/Iconos/icono_descargar.png"
                                CssClass="imgEncabezado" meta:resourcekey="lnkDescargar" />
                        </a>
                        <%--<asp:HyperLink runat="server" ID="lnkDescargarTodos" NavigateUrl="" CausesValidation="false"
                            meta:resourcekey="lnkDescargarTodos" CssClass="link" />
                        <asp:HyperLink runat="server" ID="lnkImgDescargarTodos" NavigateUrl="" CausesValidation="false"
                            meta:resourcekey="lnkDescargarTodos" CssClass="imgEncabezado" ImageUrl="~/Imagenes/Iconos/icono_descargartodos.png" />--%>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h"
                        HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" AllowFiltering="true"
                        HeaderStyle-Width="110" FilterControlWidth="80" meta:resourcekey="grdNumeroControl" />
                    <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" HeaderText="Spool"
                        AllowFiltering="true" HeaderStyle-Width="115" FilterControlWidth="80" Groupable="false"
                        meta:resourcekey="grdSpool" />
                    <telerik:GridTemplateColumn UniqueName="Caratula" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th align="center">
                                        <asp:Literal ID="litCaratula" runat="server" meta:resourcekey="litCaratula" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td align="center">
                                        <asp:HyperLink ID="caratulaLnkImprimir" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="WPS" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litWps" runat="server" meta:resourcekey="litWps" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="WpsImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="WpsLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="MTR" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litMTR" runat="server" meta:resourcekey="litMTR" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="MTRImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="MTRLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Armado" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th align="center">
                                        <asp:Literal ID="litArmado" runat="server" meta:resourcekey="litArmado" />
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td align="center">
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="armadoImgPalomita" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Soldadura" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th align="center">
                                        <asp:Literal ID="litSoldadura" runat="server" meta:resourcekey="litSoldadura" />
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td align="center">
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="SoldaduraImgPalomita" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="InspVis" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litInspVis" runat="server" meta:resourcekey="litInspVis" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="InspVisImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="InspVisLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="InspDim" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litInspDim" runat="server" meta:resourcekey="litInspDim" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="InspDimImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="InspDimLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Espesores" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litEspesores" runat="server" meta:resourcekey="litEspesores" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="EspesoresImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="EspesoresLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Rt" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litRt" runat="server" meta:resourcekey="litRt" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="RtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="RtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Pt" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPt" runat="server" meta:resourcekey="litPt" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="PtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="PtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Pwht" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPwht" runat="server" meta:resourcekey="litPwht" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="PwhtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="PwhtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Durezas" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litDurezas" runat="server" meta:resourcekey="litDurezas" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="DurezasImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="DurezasLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="RtPostTt" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litRtPostTt" runat="server" meta:resourcekey="litRtPostTt" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="RtPostTtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="RtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="PtPostTt" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPtPostTt" runat="server" meta:resourcekey="litPtPostTt" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="PtPostTtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="PtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Preheat" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPreheat" runat="server" meta:resourcekey="litPreheat" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="PreheatImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="PreheatLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Ut" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litUt" runat="server" meta:resourcekey="litUt" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="UtImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="UtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Pintura" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litPintura" runat="server" meta:resourcekey="litPintura" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="PinturaImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="PinturaLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Embarque" AllowFiltering="false" Groupable="false"
                        ItemStyle-Width="60" HeaderStyle-Width="60" HeaderStyle-CssClass="rgHeader certHeader">
                        <HeaderTemplate>
                            <table width="60">
                                <tr class="">
                                    <th colspan="2" align="center">
                                        <asp:Literal ID="litEmbarque" runat="server" meta:resourcekey="litEmbarque" />
                                    </th>
                                </tr>
                                <tr class="">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="60">
                                <tr>
                                    <td>
                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                            runat="server" ID="EmbarqueImgPalomita" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="EmbarqueLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimir.png" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
