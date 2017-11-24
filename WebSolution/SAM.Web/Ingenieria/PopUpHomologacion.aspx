<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUpHomologacion.aspx.cs"
    MasterPageFile="~/Masters/Popup.Master" Inherits="SAM.Web.Ingenieria.PopUpHomologacion" %>

<%@ Register Src="~/Controles/Ingenieria/CorteRO.ascx" TagName="CorteRO" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Ingenieria/MaterialRO.ascx" TagName="MaterialRO" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Ingenieria/JuntaRO.ascx" TagName="JuntaRO" TagPrefix="uc3" %>
<%@ Register Src="~/Controles/Ingenieria/InfoSpoolRO.ascx" TagName="SpoolRO" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="headerAzul ancho100">
        <div class="divIzquierdo ancho70">
            <asp:Label runat="server" meta:resourcekey="lblTitulo" CssClass="tituloBlanco"></asp:Label>
        </div>
        <p>
        </p>
    </div>
    <div class="homologacionPopUp soloLectura ancho100">
        <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
            SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" BackColor=""
            Style="margin-left: -1px; margin-right: -1px;" CssClass="ancho40 divIzquierdo">
            <Tabs>
                <telerik:RadTab Value="InfoGeneral" meta:resourcekey="InfoGeneral" />
                <telerik:RadTab Value="Corte" meta:resourcekey="Corte" />
                <telerik:RadTab Value="Junta" meta:resourcekey="Junta" />
                <telerik:RadTab Value="Material" meta:resourcekey="Material" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="ancho40 divIzquierdo " style="margin-top:5px">
            
            <asp:HyperLink ID="imgAceptar" meta:resourcekey="hlAceptar" runat="server" ImageUrl="~/Imagenes/Iconos/ico_aprobar.png"
                CssClass="imgMiddle" />
            <asp:HyperLink ID="hlAceptar" meta:resourcekey="hlAceptar" runat="server" />&nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="imgRechazar" meta:resourcekey="hlRechazar" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png"
                CssClass="imgMiddle" />
            <asp:HyperLink ID="hlRechazar" meta:resourcekey="hlRechazar" runat="server" />
        </div>
        <p>
        </p>
        <div class="dashboardCentral" style="min-height:370px">
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0">
                <telerik:RadPageView ID="rpvInfo" runat="server">
                    <uc4:SpoolRO runat="server" ID="SpoolRO1" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvCorte" runat="server">
                    <uc1:CorteRO ID="CorteRO1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvJunta" runat="server">
                    <uc3:JuntaRO ID="JuntaRO1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvMaterial" runat="server">
                    <uc2:MaterialRO ID="MaterialRO1" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <p>
            </p>
        </div>
        <div>
            <div class="divIzquierdo ancho50">
                <div class="divIzquierdo homologacionLeyendaCambio">
                    <asp:Literal ID="Literal1" meta:resourcekey="litDiferente" runat="server"></asp:Literal>
                </div>
                <div class="divDerecho homologacionLeyendaNuevo">
                    <asp:Literal ID="Literal2" meta:resourcekey="litNuevo" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="divDerecho homologacionLeyendaEliminado">
                <asp:Literal ID="Literal3" meta:resourcekey="litEliminado" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
