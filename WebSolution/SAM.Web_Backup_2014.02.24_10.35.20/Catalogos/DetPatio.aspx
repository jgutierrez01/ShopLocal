<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetPatio.aspx.cs" Inherits="SAM.Web.Catalogos.DetPatio" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Patios/Patio.ascx" TagName="Patio" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Patios/Taller.ascx" TagName="Talleres" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Patios/Estacion.ascx" TagName="Estacion" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Patios/Maquina.ascx" TagName="Maquinas" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Patios/Localizaciones.ascx" TagName="Localizaciones" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstPatio.aspx" meta:resourcekey="lblDetallePatio" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <telerik:RadTabStrip ID="tabMenu" runat="server" MultiPageID="rmpPatio" SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" >
                <Tabs>
                    <telerik:RadTab Value="Patio" meta:resourcekey="rdtPatio"/>
                    <telerik:RadTab Value="Talleres" meta:resourcekey="rdtTalleres"/>
                    <telerik:RadTab Value="Estacion" meta:resourcekey="rdtEstacion" />
                    <telerik:RadTab Value="Maquinas" meta:resourcekey="rdtMaquinas"/>
                    <telerik:RadTab Value="Localizaciones" meta:resourcekey="rdtLocalizaciones"/>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="rmpPatio" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="rpvPatio" runat="server" Height="300px" >
                    <sam:Patio ID="ctrlPatio" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvTalleres" runat="server" Height="300px" >
                    <sam:Talleres ID="ctrlTalleres" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvEstacion" runat="server" Height="300px">
                    <sam:Estacion ID="ctrlEstacion" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvMaquinas" runat="server" Height="300px" >
                    <sam:Maquinas ID="ctrlMaquinas" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvLocalizaciones" runat="server" Height="300px" >
                    <sam:Localizaciones ID="ctrlLocalizaciones" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <p>
            </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true" CssClass="boton" meta:resourcekey="btnGuardar" />
    </div>
</asp:Content>
