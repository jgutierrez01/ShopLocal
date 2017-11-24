<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupTaller.aspx.cs" Inherits="SAM.Web.Produccion.PopupTaller" %>

<asp:Content ID="Content2" ContentPlaceHolderID="userAgentDependant" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <asp:PlaceHolder runat="server" ID="phControles">
        <div style="width: 500px;">
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>                                
            <div class="divIzquierdo ancho45">
                <asp:HiddenField ID="hdnProyectoID" runat="server" />

                <div class="separador">
                    <asp:Label meta:resourcekey="lblTaller" runat="server" ID="lblTaller" Text="Taller:"
                        AssociatedControlID="ddlTaller" />
                    <mimo:MappableDropDown runat="server" ID="ddlTaller" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqTaller" runat="server" ID="reqTaller"
                        ErrorMessage="El taller es requerido" ControlToValidate="ddlTaller" Display="None" />
                </div>
                <div class="separador">
                    <samweb:BotonProcesando meta:resourcekey="btnGenerar" runat="server" ID="btnGenerar" Text="Generar"
                        OnClick="btnGenerar_Click" CssClass="boton" />
                </div>
            </div>
            <div class="divIzquierdo ancho50">
                <div class="validacionesRecuadro" style="margin-top: 20px;">
                    <div class="validacionesHeader">
                        &nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary"
                            meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 5px auto 0 auto;">
            <tr>
                <td rowspan="2" class="icono">
                    <img src="/Imagenes/Iconos/mensajeExito.png" />
                </td>
                <td class="titulo">
                    <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
                </td>
            </tr>
            <tr>
                <td class="cuerpo">
                    <asp:Literal runat="server" ID="litMensajeExitoPt1" meta:resourcekey="litMensajeExitoPt1" />
                    <asp:Literal runat="server" ID="litNumOdt" />&nbsp;
                    <asp:Literal runat="server" ID="litMensajeExitoPt2" meta:resourcekey="litMensajeExitoPt2" />
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
                                <asp:HyperLink runat="server" ID="hlReporteOdt" meta:resourcekey="hlReporteOdt" />
                            </li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>

</asp:Content>
