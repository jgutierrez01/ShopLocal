<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpEdicionNumeroUnico.aspx.cs" Inherits="SAM.Web.Materiales.PopUpEdicionNumeroUnico" %>
<%@ Register Src="~/Controles/Materiales/AltaNumUnicoGeneral.ascx" TagPrefix="ctrl" TagName="General" %>
<%@ Register Src="~/Controles/Materiales/AltaNumeroUnicoProveedor.ascx" TagPrefix="ctrl" TagName="Proveedor" %>
<%@ Register Src="~/Controles/Materiales/AltaNumeroUnicoAdicional.ascx" TagPrefix="ctrl" TagName="Adicional" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
<link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadWindowManager runat="server" ID="wndMgr" VisibleOnPageLoad="false" ReloadOnShow="false" ShowContentDuringLoad="false" VisibleStatusbar="false">
        <Windows>
            <telerik:RadWindow runat="server" ID="genericWindow" />
        </Windows>
    </telerik:RadWindowManager>       
    <asp:PlaceHolder ID="phEdicion" runat="server">    
        <div class="paginaHeader">
            <asp:Label runat="server" ID="lblEdicionNumUnico" CssClass="Titulo" meta:resourcekey="lblEdicionNumUnico"></asp:Label>
        </div>                 
        <asp:PlaceHolder ID="pnlAlta" runat="server">        
            <div class="cntCentralForma" style="height: 460px">
                <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
                    SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" meta:resourcekey="menuConfiguracionResource1">
                    <Tabs>
                        <telerik:RadTab Value="General" meta:resourcekey="tabGeneral" />
                        <telerik:RadTab Value="Proveedor" meta:resourcekey="tabProveedor" />
                        <telerik:RadTab Value="Adicional" meta:resourcekey="tabAdicional" />
                    </Tabs>
                </telerik:RadTabStrip>
                <div class="dashboardCentral">
                    <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0" meta:resourcekey="rmConfiguracionResource1">
                        <telerik:RadPageView ID="rpvGeneral" runat="server">
                            <ctrl:General ID="ctrlGeneral" runat="server" OnDdlCedulaOrIC_SelectedIndexChanged="DdlCedulaOrIC_SelectedIndexChanged" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvProveedor" runat="server">
                            <ctrl:Proveedor ID="ctrlProveedor" runat="server" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvAdicional" runat="server">
                            <ctrl:Adicional ID="ctrlAdicional" runat="server" />
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                    <p>
                    </p>
                </div>
            </div>
            <asp:HiddenField ID="hdnPuedeTransferir" runat="server" />
            <asp:HiddenField ID="hdnEstatusOriginal" runat="server"/>
            
            <div class="pestanaBoton mensaje">
                <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton"
                    OnClick="btnGuardar_OnClick" />                    
            </div>
            <div style="Display:inline-block">
                <asp:Label runat="server" ID="lblAdvertencia" meta:resourcekey="lblAdvertencia" CssClass="boldUnderline" Visible="False"/>
            </div>
        </asp:PlaceHolder>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="pnlMensaje" runat="server" Visible="False">
     <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin:5px auto 0 auto;">
        <tr>
            <td rowspan="2" class="icono">
                <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
            </td>
            <td class="titulo">
                <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
            </td>
        </tr>
        <tr>
            <td class="cuerpo">
                 <asp:Label ID="lblMensaje" runat="server" meta:resourcekey="lblMensaje"></asp:Label>
            </td>
        </tr>
    </table>      
    </asp:PlaceHolder>
</asp:Content>
