<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetSoldador.aspx.cs" Inherits="SAM.Web.Catalogos.DetSoldador" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Soldadores/Soldador.ascx" TagName="Soldador" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Soldadores/Wpq.ascx" TagName="Wpq" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstSoldador.aspx"
        meta:resourcekey="lblDetalleSoldador" />
    <div class="cntCentralForma">
        <telerik:RadTabStrip ID="tabMenu" runat="server" MultiPageID="rmpSoldador" SelectedIndex="0"
            Orientation="HorizontalBottom" CausesValidation="false">
            <Tabs>
                <telerik:RadTab Value="Soldador" meta:resourcekey="rdtSoldador" />
                <telerik:RadTab Value="Wpq" meta:resourcekey="rdtWpq" />
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="rmpSoldador" runat="server" SelectedIndex="0">
            <telerik:RadPageView ID="rpvSoldadores" runat="server">
                <ctrl:Soldador ID="ctrlSoldador" runat="server" OnPatioSeleccionado="patioSeleccionado"></ctrl:Soldador>
            </telerik:RadPageView>
            <telerik:RadPageView ID="rpvWpq" runat="server">
                <ctrl:Wpq ID="ctrlWpq" runat="server"></ctrl:Wpq>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        <p>
        </p>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            meta:resourcekey="btnGuardar" CssClass="boton" />
    </div>
</asp:Content>
