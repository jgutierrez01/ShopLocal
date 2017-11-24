<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="SAM.Web.Proyectos.Configuracion" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/InformacionGeneral.ascx" TagName="Informacion" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Proyecto/Contacto.ascx" TagName="Contacto" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Proyecto/Configuracion.ascx" TagName="Configuracion" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Proyecto/ConfiguracionLibre.ascx" TagName="ConfiguracionLibre" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Proyecto/ConfiguracionCorreo.ascx" TagName="ConfiguracionCorreo" TagPrefix="ctrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" />
    <div class="cntCentralForma">
        <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion" SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" style="margin-left:-8px;">
            <Tabs>
                <telerik:RadTab Value="General" meta:resourcekey="rtGeneral" />
                <telerik:RadTab Value="Contacto" meta:resourcekey="rtContacto" />
                <telerik:RadTab Value="Configuracion" meta:resourcekey="rtConfiguracion"/>
                <telerik:RadTab Value="ConfiguracionLibre" meta:resourcekey="rtConfiguracionLibre" />
                <telerik:RadTab Value="ConfiguracionCorreo" meta:resourcekey="rtConfiguracionCorreo" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="dashboardCentral edicionProyecto">
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0">
                <telerik:RadPageView ID="rpvGeneral" runat="server" Height="400">
                    <ctrl:Informacion ID="ctrlInformacion" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvContacto" runat="server" Height="400">
                    <ctrl:Contacto ID="ctrlContacto" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvConfiguracion" runat="server" Height="400">
                    <ctrl:Configuracion ID="ctrlConfiguracion" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvConfiguracionLibre" runat="server" Height="400">
                    <ctrl:ConfiguracionLibre ID="ctrlConfiguracionLibre" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvConfiguracionCorreo" runat="server" Height="400">
                    <ctrl:ConfiguracionCorreo ID="ctrlConfiguracionCorreo" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_GuardaConfiguracion" />
    </div>
</asp:Content>
