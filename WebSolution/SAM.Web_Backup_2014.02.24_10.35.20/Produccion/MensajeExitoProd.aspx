<%@ Page  Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="MensajeExitoProd.aspx.cs" Inherits="SAM.Web.Produccion.MensajeExitoProd" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
<table class="mensajeExito" cellpadding="0" cellspacing="0">
        <tr>
            <td rowspan="2" class="icono">
                <img src="/Imagenes/Iconos/mensajeExito.png" />
            </td>
            <td class="titulo">
                <asp:Label runat="server" ID="lblTitulo" />
            </td>
        </tr>
        <tr>
            <td class="cuerpo">
                <asp:Label runat="server" ID="lblCuerpo" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="ligas">
                <div class="cuadroLigas">
                    <asp:Repeater runat="server" ID="repLigas">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li><samweb:AuthenticatedHyperLink ID="hlLiga" runat="server" NavigateUrl='<%# Eval("Url") %>' Text='<%# Eval("Texto") %>' /></li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>  
            </td>
        </tr>
    </table>
</asp:Content>
