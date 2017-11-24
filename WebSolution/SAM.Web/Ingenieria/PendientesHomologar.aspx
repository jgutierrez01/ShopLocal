<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendientesHomologar.aspx.cs"
    MasterPageFile="~/Masters/Ingenieria.master" Inherits="SAM.Web.Ingenieria.PendientesHomologar" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="FiltroGenerico"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="pnlHomologacion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnMostrar">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="btnMostrar" />
                    <telerik:AjaxUpdatedControl ControlID="pnlHomologacion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="contenedorCentral">
        <div id="paginaHeader" class="paginaHeader">
            <asp:Label ID="lblTitulo" runat="server" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
        </div>
        <div class="cajaFiltros">
            <div class="divIzquierdo ancho100">
                <uc1:FiltroGenerico ID="filtroGenerico" runat="server" FiltroNumeroControl="false"
                    FiltroNumeroUnico="false" FiltroOrdenTrabajo="false" ProyectoHeaderID="proyHeader"
                    ProyectoRequerido="true" OnDdlProyecto_SelectedIndexChanged="proyecto_Cambio"  />
                    <div class="separador">
                        <asp:Button CssClass="boton" ID="btnMostrar" OnClick="btnMostrarClick"
                            meta:resourcekey="btnMostrar" runat="server" />
                    </div>
            </div>            
            <p>
            </p>
        </div>
        <p>
        </p>
        <uc2:Header ID="proyHeader" runat="server" Visible="false" />
        <div class="dashboardCentral">
            <asp:Panel ID="pnlHomologacion" runat="server" Visible="false">
                <div class="cajaGris soloLectura">
                    <asp:Repeater runat="server" ID="repHomologacion" OnItemDataBound="repHomologacion_OnItemDataBound">
                        <HeaderTemplate>
                            <table>
                            <colgroup>
                                <col width="300" />                                            
                            </colgroup>
                            <thead>
                                <th><asp:Literal runat="server" meta:resourcekey="litSpool"></asp:Literal></th>                                            
                            </thead>                                        
                        </HeaderTemplate>
                        <ItemTemplate>                                        
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hlNombreSpool" runat="server" />
                                </td>                                             
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>                            
            </asp:Panel>
        </div>
    </div>
</asp:Content>