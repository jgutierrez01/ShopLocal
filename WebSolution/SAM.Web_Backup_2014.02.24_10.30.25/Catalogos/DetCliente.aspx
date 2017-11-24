<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetCliente.aspx.cs" Inherits="SAM.Web.Catalogos.DetCliente" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Cliente/Cliente.ascx" TagName="Cliente" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Cliente/ContactoCliente.ascx" TagName="Contacto" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstCliente.aspx"
        meta:resourcekey="lblDetalleClientes" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <telerik:RadTabStrip ID="tabMenu" runat="server" MultiPageID="rmpCliente" SelectedIndex="0"
                Orientation="HorizontalBottom" CausesValidation="false">
                <Tabs>
                    <telerik:RadTab Value="Cliente" meta:resourcekey="rdtCliente" />
                    <telerik:RadTab Value="Contacto" meta:resourcekey="rdtContacto" />
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="rmpCliente" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="rpvCliente" runat="server">
                    <sam:Cliente ID="ctrlCliente" runat="server"></sam:Cliente>
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvContacto" runat="server">
                    <sam:Contacto ID="ctrlContacto" runat="server"></sam:Contacto>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <p>
            </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            CssClass="boton" meta:resourcekey="btnGuardar" />
    </div>
</asp:Content>
