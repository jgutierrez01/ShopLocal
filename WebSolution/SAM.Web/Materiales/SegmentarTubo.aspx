<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.Master" AutoEventWireup="true" CodeBehind="SegmentarTubo.aspx.cs" Inherits="SAM.Web.Materiales.SegmentarTubo" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlSegmento">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblInventarioFisicoData" UpdatePanelRenderMode="Inline"/>
                    <telerik:AjaxUpdatedControl ControlID="lblInventarioBuenEdoData" UpdatePanelRenderMode="Inline"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" CssClasS="Titulo" meta:resourcekey="lblTitulo"
        NavigateUrl="~/Produccion/ProdDefault.aspx" />
    <div class="cntCentralForma">
    <br />
        <div class="cajaFiltros">
            <uc2:Filtro runat="server" ID="filtroGenerico" FiltroProyecto="true" FiltroNumeroUnico="true"
                FiltroNumeroControl="false" FiltroOrdenTrabajo="false" ProyectoHeaderID="proyEncabezado"
                ProyectoAutoPostBack="true" ProyectoRequerido="true" NumeroUnicoRequerido="true">
            </uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button CssClass="boton" runat="server" ID="btnMostrar" OnClick="btnMostrar_Click"
                        meta:resourcekey="btnMostrar" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" DisplayMode="BulletList"
            meta:resourcekey="valSummary" />
        <div>
            <sam:Header ID="proyEncabezado" runat="server" Visible="False" />
        </div>
        <asp:Panel runat="server" ID="pnlDatos" Visible="false">
            <div class="cajaFiltros">
                <div class="divIzquierdo ancho45">
                    <asp:Label runat="server" ID="lblItemCode" CssClass="bold" meta:resourcekey="lblItemCode"></asp:Label>
                    <asp:Label runat="server" ID="lblItemCodeData" CssClass="ToClear"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblDescripcion" CssClass="bold" meta:resourcekey="lblDescripcion"></asp:Label>
                    <asp:Label runat="server" ID="lblDescripcionData" CssClass="ToClear"></asp:Label>
                </div>
                <div class="divIzquierdo ancho45">
                    <asp:Label runat="server" ID="lblDiametro1" CssClass="bold" meta:resourcekey="lblDiametro1"></asp:Label>
                    <asp:Label runat="server" ID="lblDiametro1Data" CssClass="ToClear"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblDiametro2" CssClass="bold" meta:resourcekey="lblDiametro2"></asp:Label>
                    <asp:Label runat="server" ID="lblDiametro2Data" CssClass="ToClear"></asp:Label>
                </div>
                <p>
                </p>
            </div>
            <div class="divIzquierdo ancho35">
                <div class="separador">
                    <asp:Label runat="server" ID="lblSegmento" CssClass="bold" meta:resourcekey="lblSegmento" />
                    <br />
                    <asp:DropDownList ID="ddlSegmento" runat="server" CausesValidation="false" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSegmento_SelectedIndexChanged">
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvSegmento" runat="server" Display="None" ControlToValidate="ddlSegmento"
                        ValidationGroup="vgSegmentar" meta:resourcekey="rfvSegmento" InitialValue="" />
                </div>
                <div class="cajaFiltros">
                    <asp:Label runat="server" ID="lblInventarioFisico" CssClass="bold" meta:resourcekey="lblInventarioFisico"></asp:Label>
                    <asp:Label runat="server" ID="lblInventarioFisicoData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblInventarioBuenEdo" CssClass="bold" meta:resourcekey="lblInventarioBuenEdo"></asp:Label>
                    <asp:Label runat="server" ID="lblInventarioBuenEdoData" CssClass=""></asp:Label>
                    <br />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtLongitudSegmento" MaxLength="50"
                        ValidationGroup="vgSegmentar" meta:resourcekey="lblLongitudSegmento" />
                    <asp:CustomValidator runat="server" ID="cvLongitudes" Display="None" OnServerValidate="cvLongitudes_ServerValidate"
                        ValidationGroup="vgSegmentar" meta:resourcekey="cvLongitudes" />
                    <asp:RegularExpressionValidator runat="server" ID="revLongitud" ControlToValidate="txtLongitudSegmento"
                        Display="None" ValidationExpression="\d*" ValidationGroup="vgSegmentar" meta:resourcekey="revLongitud" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombreSegmento" MaxLength="1"
                        ValidationGroup="vgSegmentar" meta:resourcekey="lblNombreSegmento" />
                    <asp:RegularExpressionValidator runat="server" ID="revNomSegmento" ControlToValidate="txtNombreSegmento"
                        Display="None" ValidationExpression="[A-Z]" ValidationGroup="vgSegmentar" meta:resourcekey="revNomSegmento" />
                    <asp:CustomValidator runat="server" ID="cvNombreSegmento" Display="None" OnServerValidate="cvNombreSegmento_ServerValidate"
                        ValidationGroup="vgSegmentar" meta:resourcekey="cvNombreSegmento" />
                </div>
            </div>
            <div class="divDerecho ancho35">
                <div class="validacionesRecuadro" style="margin-top: 23px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummaryDatos" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgSegmentar" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p>
            </p>
        </asp:Panel>
    </div>
    <asp:PlaceHolder runat="server" ID="phBoton" Visible="false">
        <div class="pestanaBoton">
            <asp:Button runat="server" ID="btnSegmentar" meta:resourcekey="btnSegmentar" CssClass="boton"
                CausesValidation="true" OnClick="btnSegmentar_Click" ValidationGroup="vgSegmentar" />
        </div>
    </asp:PlaceHolder>
</asp:Content>
