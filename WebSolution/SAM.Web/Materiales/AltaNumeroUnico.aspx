<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="AltaNumeroUnico.aspx.cs" Inherits="SAM.Web.Materiales.AltaNumeroUnico"
    meta:resourcekey="PageResource1" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ Register Src="~/Controles/Materiales/AltaNumUnicoGeneral.ascx" TagPrefix="ctrl" TagName="General" %>
<%@ Register Src="~/Controles/Materiales/AltaNumeroUnicoProveedor.ascx" TagPrefix="ctrl" TagName="Proveedor" %>
<%@ Register Src="~/Controles/Materiales/AltaNumeroUnicoAdicional.ascx" TagPrefix="ctrl" TagName="Adicional" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <proy:Encabezado ID="proyEncabezado" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblRecepcionNumUnico"
        NavigateUrl="~/Materiales/LstRecepcion.aspx" />
    <asp:PlaceHolder ID="pnlAlta" runat="server">
        <div class="cntCentralForma">
            <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
                SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false">
                <Tabs>
                    <telerik:RadTab Value="General" meta:resourcekey="tabGeneral" />
                    <telerik:RadTab Value="Proveedor" meta:resourcekey="tabProveedor" />
                    <telerik:RadTab Value="Adicional" meta:resourcekey="tabAdicional" />
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0">
                <telerik:RadPageView ID="rpvGeneral" runat="server" Height="515px">
                    <ctrl:General ID="ctrlGeneral" runat="server" OnDdlCedulaOrIC_SelectedIndexChanged="DdlCedulaOrIC_SelectedIndexChanged" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvProveedor" runat="server" Height="515px">
                    <ctrl:Proveedor ID="ctrlProveedor" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvAdicional" runat="server" Height="515px">
                    <ctrl:Adicional ID="ctrlAdicional" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <p>
            </p>
        </div>
        <div class="pestanaBotonLarga">
            <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton"
                OnClick="btnGuardar_OnClick" />&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnGuardarMantener" meta:resourcekey="btnGuardarMantener"
                CssClass="boton" OnClick="btnGuardarMantener_OnClick" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="pnlAcciones" runat="server" Visible="false">
        <div class="cntCentralForma">        
            <table class="mensajeExito" cellpadding="0" cellspacing="0">
            <tr><td><p></p></td></tr>
                <tr>
                    <td rowspan="2" class="icono">
                        <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
                    </td>
                    <td class="titulo">
                        <asp:Label runat="server" ID="lblTitulo" meta:resourcekey="lblTitulo" />
                    </td>
                </tr>
                <tr>
                    <td class="cuerpo">
                        <asp:Label ID="lblMensaje" runat="server" meta:resourcekey="lblMensaje"></asp:Label>&nbsp;
                        <asp:Label ID="lblCantidadNumUnicosGenerados" runat="server" CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblMensaje2" runat="server" meta:resourcekey="lblMensaje2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ligas">
                        <div class="cuadroLigas">
                            <ul>
                                <li>
                                    <asp:HyperLink ID="btnEtiquetas" runat="server" meta:resourcekey="btnEtiquetas" Target="_blank"  /></li>
                                <li>
                                    <asp:HyperLink ID="btnNuevaRecepcion" runat="server" meta:resourcekey="btnNuevaRecepcion"
                                        NavigateUrl="~/Materiales/NuevaRecepcion.aspx" /></li>
                                <li>
                                    <asp:HyperLink ID="btnRecepcion" runat="server" meta:resourcekey="btnRecepcion" /></li>
                                <li>
                                    <asp:HyperLink ID="btnGenerarNumUnico" runat="server" meta:resourcekey="btnGenerarNumUnico" /></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
     <div id="btnWrapper">
        <asp:Button runat="server" CausesValidation="false" ID="btnRedirectAltaNumUnicos"
            CssClass="oculto" OnClick="btnRedirectAltaNumUnicos_Click" />
    </div>
    <asp:HiddenField ID="hdnNumeroUnicoID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCantidadNumUnicos" runat="server" ClientIDMode="Static" />
</asp:Content>
