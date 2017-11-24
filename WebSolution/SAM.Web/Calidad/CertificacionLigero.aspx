<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true"
    CodeBehind="CertificacionLigero.aspx.cs" Inherits="SAM.Web.Calidad.CertificacionLigero" %>

<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Navegacion/Paginador.ascx" TagName="Paginador" TagPrefix="pag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">

    <telerik:RadAjaxManager ID="radManager" runat="server" ClientEvents-OnResponseEnd="Sam.Calidad.AjaxRequestEnd">
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

        <telerik:RadContextMenu runat="server" ID="menuColumnas" OnItemClick="menuColumnas_ItemClick" OnClientItemClicked="Sam.Calidad.ItemMenuClicked" CausesValidation="false">        
            <Items>
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmOrdenarAscendente" PostBack="true" Value="ordAsc" />
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmOrdenarDescendente" PostBack="true" Value="ordDesc" />
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmQuitarOrdenamiento" PostBack="true" Value="remove" />                
            </Items>
        </telerik:RadContextMenu>

         <asp:HiddenField runat="server" ID="hdnMenuColumnasID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnColumnaSeleccionada" ClientIDMode="Static" />

        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" FiltroNumeroUnico="false" OrdenTrabajoAutoPostback="true"
                NumeroControlAutoPostBack="true" ProyectoHeaderID="proyHeader" ProyectoAutoPostBack="true"
                runat="server" OnDdlProyecto_SelectedIndexChanged="proyecto_Cambio" ID="filtroGenerico">
            </uc2:Filtro>  
            <asp:Panel ID="PanelEmbarque" runat="server" CssClass="filtro">
            <asp:Panel ID="phProyecto" runat="server" CssClass="divIzquierdo" >
                <div class="separador">
                    <asp:Label ID="lblEmbarque" runat="server" meta:resourcekey="lblEmbarque" CssClass="labelHack bold"/>
                    <asp:TextBox ID="txtEmbarque" runat="server" />                    
                </div>
                </asp:Panel>
            </asp:Panel>
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
                    <asp:CheckBox ID="chkDrawing" meta:resourcekey="chkDrawing" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkWPS" meta:resourcekey="chkWPS" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkMTR" meta:resourcekey="chkMTR" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkMTRSold" meta:resourcekey="chkMTRSold" runat="server" Enabled="false"
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
                    <asp:CheckBox ID="chkPMI" meta:resourcekey="chkPMI" runat="server" Enabled="false"
                        CssClass="checkYTexto" />
                </div>
                <div class="divIzquierdo ancho25">
                    <asp:CheckBox ID="chkPruebaHidro" meta:resourcekey="chkPruebaHidro" runat="server" Enabled="false"
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
        <asp:Panel runat="server" ID="panel">
            <div id="ocultoInicio">
                <table  cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td colspan="2">
                                <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                                    <thead>
                                        <tr class="repEncabezado">
                                            <th>
                                                <div class="comandosEncabezado">
                                                <span class="iconoDerecha">
                                                    <asp:LinkButton runat="server" ID="lnkDescargar" meta:resourcekey="lnkDescargar"
                                                        OnClick="lnkDescargar_Click" />
                                                    <asp:ImageButton runat="server" ID="imgDescargarSeleccionados" src="/Imagenes/Iconos/icono_descargar.png"
                                                        CssClass="imgEncabezado" meta:resourcekey="lnkDescargar" OnClick="lnkDescargar_Click" />
                                                        </span>
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="scrollHeaders congelados" id="topLeft">
                                    <table class="repSam" cellpadding="0" cellspacing="0">
                                        <thead>
                                            <tr class="repTitulos">
                                                <th class="icoVer">
                                                    &nbsp;
                                                </th>
                                                <th class="mediana" align="center">     
                                                    <span class="spHeader" data="NumeroControl">
                                                        <asp:Literal ID="Literal1"  runat="server" meta:resourcekey="grdNumeroControl"></asp:Literal>
                                                    </span>                                      
                                                </th>
                                                <th class="larga" align="center" width="100%">
                                                    <span class="spHeader" data="Spool">
                                                        <asp:Literal ID="litSpool" runat="server" meta:resourcekey="grdSpool"></asp:Literal>
                                                    </span>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div class="scrollHeaders" id="tblHeaders">
                                    <table class="repSam" cellpadding="0" cellspacing="0">
                                        <thead>
                                            <tr class="repTitulos">
                                                <th class="cortaSola" align="center">
                                                    <asp:Literal ID="litTrazabilidad" runat="server" meta:resourcekey="litCaratula"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center">
                                                    <asp:Literal ID="litDrawing" runat="server" meta:resourcekey="litDrawing"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litWPS" runat="server" meta:resourcekey="litWps"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litMTR" runat="server" meta:resourcekey="litMTR"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litMTRSold" runat="server" meta:resourcekey="litMTRSold"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center">
                                                    <asp:Literal ID="litArmado" runat="server" meta:resourcekey="litArmado"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center">
                                                    <asp:Literal ID="litSoldadura" runat="server" meta:resourcekey="litSoldadura"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litInspVis" runat="server" meta:resourcekey="litInspVis"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litInspDim" runat="server" meta:resourcekey="litInspDim"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litEspesores" runat="server" meta:resourcekey="litEspesores"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litRt" runat="server" meta:resourcekey="litRt"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPt" runat="server" meta:resourcekey="litPt"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPwht" runat="server" meta:resourcekey="litPwht"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litDurezas" runat="server" meta:resourcekey="litDurezas"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litRtPostTt" runat="server" meta:resourcekey="litRtPostTt"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPtPostTt" runat="server" meta:resourcekey="litPtPostTt"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPreheat" runat="server" meta:resourcekey="litPreheat"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litUt" runat="server" meta:resourcekey="litUt"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPMI" runat="server" meta:resourcekey="litPMI"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPruebaHidro" runat="server" meta:resourcekey="litPruebaHidro"></asp:Literal>
                                                </th>
                                                <th class="cortaSola" align="center" >
                                                    <asp:Literal ID="litPintura" runat="server" meta:resourcekey="litPintura"></asp:Literal>
                                                </th>
                                                <th class="cortaSolaEsc" align="center" >
                                                    <asp:Literal ID="litEmbarque" runat="server" meta:resourcekey="litEmbarque"></asp:Literal>
                                                </th>
                                                <th style="width:15px;">
                                                &nbsp;
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <div class="scrollTblVerticalCert congelados" id="tblCongelados">
                            <table  class="repSam" cellpadding="0" cellspacing="0">
                                <asp:Repeater ID="repCongelados" runat="server">
                                    <ItemTemplate>
                                        <tr class="repFila">
                                            <td class="icoVer">
                                                <asp:CheckBox ID="chkSpool" runat="server" />
                                                <asp:HiddenField ID="hdnSpoolID" Value='<%#Eval("SpoolID") %>' runat="server" />
                                            </td>
                                            <td class="mediana">
                                                <%#Eval("NumeroControl") %>
                                            </td>
                                            <td class="larga">
                                                <%#Eval("Spool") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="repFilaPar">
                                            <td class="icoVer">
                                                <asp:CheckBox ID="chkSpool" runat="server" />
                                                <asp:HiddenField ID="hdnSpoolID" Value='<%#Eval("SpoolID") %>' runat="server" />
                                            </td>
                                            <td class="mediana">
                                                <%#Eval("NumeroControl") %>
                                            </td>
                                            <td class="larga">
                                                <%#Eval("Spool") %>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                                <tr class="repFila">
                                                    <td class="icoVer">&nbsp;</td>
                                                    <asp:Literal runat="server" ID="thFooter" />
                                                </tr>
                                            </FooterTemplate>
                                </asp:Repeater>
                                </table>
                                </div>
                            </td>
                            <td>
                                <div class="scrollTblVerticalCert" id="tblBody">
                                    <table class="repSam" cellpadding="0" cellspacing="0">
                                        <asp:Repeater ID="repCertificacion" runat="server" OnItemDataBound="repCertificacion_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="repFila">
                                                    <td class="corta" align="center" runat="server" id="tdTra">
                                                        <asp:HyperLink ID="caratulaLnkImprimir" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" />
                                                    </td>
                                                    <td class="corta" align="center" runat="server" id="tdTra2">
                                                        <asp:HyperLink ID="caratulaLnkEscaneo" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="cortaSola" align="center" runat="server" id="tdDra">
                                                        <asp:HyperLink ID="dibujoLnkEscaneo" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdWps">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="WpsImgPalomita" />
                                                    </td>                                                    
                                                    <td class="corta" runat="server" id="tdWps2">
                                                        <asp:HyperLink ID="WpsLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTR">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="MTRImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTR2">
                                                        <asp:HyperLink ID="MTRLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTRSold">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="MTRSoldImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTRSold2">
                                                        <asp:HyperLink ID="MTRSoldLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="cortaSola" align="center">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="armadoImgPalomita" />
                                                    </td>
                                                    <td class="cortaSola" align="center">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="SoldaduraImgPalomita" />
                                                    </td>
                                                    <td class="corta"  runat="server" id="tdIV">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="InspVisImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdIV2">
                                                        <asp:HyperLink ID="InspVisLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdID">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="InspDimImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdID2">
                                                        <asp:HyperLink ID="InspDimLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEsp">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="EspesoresImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEsp2">
                                                        <asp:HyperLink ID="EspesoresLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="RtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRT2">
                                                        <asp:HyperLink ID="RtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPT2">
                                                        <asp:HyperLink ID="PtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPWHT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PwhtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPWHT2">
                                                        <asp:HyperLink ID="PwhtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdDur">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="DurezasImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdDur2">
                                                        <asp:HyperLink ID="DurezasLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRTTT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="RtPostTtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRTTT2">
                                                        <asp:HyperLink ID="RtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPTTT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PtPostTtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPTTT2">
                                                        <asp:HyperLink ID="PtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPre">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PreheatImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPre2">
                                                        <asp:HyperLink ID="PreheatLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdUT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="UtImgPalomita" />
                                                    </td>                                                    
                                                    <td class="corta" runat="server" id="tdUT2">
                                                        <asp:HyperLink ID="UtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPMI">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PMIImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPMI2">
                                                        <asp:HyperLink ID="PMILnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPruebaHidro">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PruebaHidroImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPruebaHidro2">
                                                        <asp:HyperLink ID="PruebaHidroLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPin">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PinturaImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPin2">
                                                        <asp:HyperLink ID="PinturaLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="EmbarqueImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb2">
                                                        <asp:HyperLink ID="EmbarqueLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb3">
                                                        <asp:HyperLink ID="EmbarqueLnkEsc" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                 <tr class="repFilaPar">
                                                    <td class="corta" align="center" runat="server" id="tdTra">
                                                        <asp:HyperLink ID="caratulaLnkImprimir" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" />
                                                    </td>
                                                    <td class="corta" align="center" runat="server" id="tdTra2">
                                                        <asp:HyperLink ID="caratulaLnkEscaneo" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="cortaSola" align="center" runat="server" id="tdDra">
                                                        <asp:HyperLink ID="dibujoLnkEscaneo" runat="server" Visible="true" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdWps">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="WpsImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdWps2">
                                                        <asp:HyperLink ID="WpsLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTR">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="MTRImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTR2">
                                                        <asp:HyperLink ID="MTRLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTRSold">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="MTRSoldImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdMTRSold2">
                                                        <asp:HyperLink ID="MTRSoldLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="cortaSola" align="center">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="armadoImgPalomita" />
                                                    </td>
                                                    <td class="cortaSola" align="center">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="SoldaduraImgPalomita" />
                                                    </td>
                                                    <td class="corta"  runat="server" id="tdIV">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="InspVisImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdIV2">
                                                        <asp:HyperLink ID="InspVisLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdID">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="InspDimImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdID2">
                                                        <asp:HyperLink ID="InspDimLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEsp">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="EspesoresImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEsp2">
                                                        <asp:HyperLink ID="EspesoresLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="RtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRT2">
                                                        <asp:HyperLink ID="RtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPT2">
                                                        <asp:HyperLink ID="PtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPWHT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PwhtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPWHT2">
                                                        <asp:HyperLink ID="PwhtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdDur">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="DurezasImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdDur2">
                                                        <asp:HyperLink ID="DurezasLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRTTT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="RtPostTtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdRTTT2">
                                                        <asp:HyperLink ID="RtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPTTT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PtPostTtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPTTT2">
                                                        <asp:HyperLink ID="PtPostTtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPre">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PreheatImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPre2">
                                                        <asp:HyperLink ID="PreheatLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdUT">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="UtImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdUT2">
                                                        <asp:HyperLink ID="UtLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPMI">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PMIImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPMI2">
                                                        <asp:HyperLink ID="PMILnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPruebaHidro">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PruebaHidroImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPruebaHidro2">
                                                        <asp:HyperLink ID="PruebaHidroLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPin">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="PinturaImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdPin2">
                                                        <asp:HyperLink ID="PinturaLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb">
                                                        <asp:Image Visible="false" ImageUrl="~/Imagenes/Iconos/ico_proceso_completo.png"
                                                            runat="server" ID="EmbarqueImgPalomita" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb2">
                                                        <asp:HyperLink ID="EmbarqueLnkImprimir" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" />
                                                    </td>
                                                    <td class="corta" runat="server" id="tdEmb3">
                                                        <asp:HyperLink ID="EmbarqueLnkEsc" runat="server" Visible="false" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <pag:Paginador runat="server" ID="pager" TamanioPagina="20" OnPaginaCambio="pager_PaginaCambio"
                    MuestraPanelCargando="true" />
            </div>
        </asp:Panel>
    </div>
     <script type="text/javascript" language="javascript">
         $(function () {
             Sam.Calidad.AttachHandlers();
         });
    </script>
</asp:Content>
