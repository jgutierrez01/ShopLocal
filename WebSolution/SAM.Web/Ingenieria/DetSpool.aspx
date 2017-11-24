<%@ Page Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="DetSpool.aspx.cs" Inherits="SAM.Web.Ingenieria.DetSpool" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/InfoSpool.ascx" TagName="InfoSpool" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/Junta.ascx" TagName="Junta" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/Material.ascx" TagName="Material" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/Corte.ascx" TagName="Corte" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/Hold.ascx" TagName="Hold" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <asp:PlaceHolder runat="server" ID="phDetSpool">
        <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Ingenieria/NombradoSpool.aspx"
            meta:resourcekey="lblDetalleSpool" />
        <div class="cntCentralForma">
            <div class="dashboardCentral">
                <telerik:RadTabStrip ID="tabMenu" runat="server" MultiPageID="rmpSpool" SelectedIndex="0"
                    Orientation="HorizontalBottom" CausesValidation="false">
                    <Tabs>
                        <telerik:RadTab Value="Info" meta:resourcekey="rtInfo" />
                        <telerik:RadTab Value="Junta" meta:resourcekey="rtJunta" />
                        <telerik:RadTab Value="Material" meta:resourcekey="rtMaterial" />
                        <telerik:RadTab Value="Corte" meta:resourcekey="rtCorte" />
                        <telerik:RadTab Value="Hold" meta:resourcekey="rtHold" />
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rmpSpool" runat="server" SelectedIndex="0">
                    <telerik:RadPageView ID="rpvInfo" runat="server">
                        <sam:InfoSpool ID="ctrlInfo" runat="server"></sam:InfoSpool>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="rpvJunta" runat="server">
                        <sam:Junta ID="ctrlJunta" runat="server"></sam:Junta>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="rpvMaterial" runat="server">
                        <sam:Material ID="ctrlMaterial" runat="server"></sam:Material>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="rpvCorte" runat="server">
                        <sam:Corte ID="ctrlCorte" runat="server"></sam:Corte>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="rpvHold" runat="server">
                        <sam:Hold ID="ctrlHold" runat="server"></sam:Hold>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
            <p>
            </p>
        </div>
        <div class="pestanaBoton">
            <asp:Button runat="server" ID="btnGuardar" Text="Guardar" meta:resourcekey="btnGuardar"
                CssClass="divIzquierdo boton" OnClick="btnGuardar_Click" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phMensajeExito" runat="server" Visible="False">
        <table class="mensajeExito" cellpadding="0" cellspacing="0" style="margin: 5px auto 0 auto;">
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
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="ligas">
                    <div class="cuadroLigas">
                        <ul>
                            <li>
                                <asp:HyperLink ID="hlIngenieria" runat="server" meta:resourcekey="hlIngenieria"
                                    NavigateUrl="~/Ingenieria/IngenieriaDeProyecto.aspx?PID={0}" /></li>
                            <li>
                                <asp:HyperLink ID="hlOrdenesDeTrabajo" runat="server" meta:resourcekey="hlOrdenesDeTrabajo" 
                                    NavigateUrl="~/Produccion/LstOrdenTrabajo.aspx" /></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
            OnClick="lnkActualizar_Click" />
    </div>
</asp:Content>
