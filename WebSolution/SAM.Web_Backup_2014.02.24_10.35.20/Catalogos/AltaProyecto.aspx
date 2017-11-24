<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="AltaProyecto.aspx.cs" Inherits="SAM.Web.Catalogos.AltaProyecto" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<%@ Register Src="~/Controles/Proyecto/InformacionGeneral.ascx" TagName="Informacion" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Proyecto/Contacto.ascx" TagName="Contacto" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetalleProyecto" NavigateUrl="~/Catalogos/LstProyecto.aspx" />
    <div class="cntCentralForma">
        <telerik:RadTabStrip ID="tabMenu" runat="server" MultiPageID="rmpProyecto" SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" style="margin-left:-8px;">
            <Tabs>
                <telerik:RadTab Value="General" meta:resourcekey="rdGeneral" />
                <telerik:RadTab Value="Contacto" meta:resourcekey="rdContacto" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="dashboardCentral">
            <telerik:RadMultiPage ID="rmpProyecto" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="rpvGeneral" runat="server">
                    <div style="height:300px;">
                        <ctrl:Informacion ID="ctrlInformacion" runat="server" />
                    </div>
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvContacto" runat="server">
                    <div style="height:300px;">
                        <ctrl:Contacto ID="ctrlContacto" runat="server" />
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button meta:resourcekey="btnGuardar" CssClass="boton" runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" />    
    </div>
</asp:Content>
