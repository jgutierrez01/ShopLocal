<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TablaDestajos.ascx.cs" Inherits="SAM.Web.Controles.ImportarProyecto.TablaDestajos" %>
<div class="contenedorCentral">
    <asp:PlaceHolder runat="server" ID="phTablaDestajos">
    <div class="cajaFiltros">
        <div class="divIzquierdo">
            <div class="separador">
                <span class="required">*</span>
                <asp:Label runat="server" ID="lblProyecto" Text="Proyecto:" CssClass="bold" meta:resourcekey="lblProyecto" />
                <mimo:MappableDropDown runat="server" ID="ddlProyecto2" EntityPropertyName="ProyectoID" AutoPostBack="True" CssClass="labelHack" OnSelectedIndexChanged="ddlProyecto2_OnSelectedIndexChanged" />
                <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto2" InitialValue="" Display="None"
                        ErrorMessage="El Proyecto es requerido" ValidationGroup="vgTabla" meta:resourcekey="rfvProyecto" />
            </div>
        </div>
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Button runat="server" ID="btnImportar" Text="Importar" CssClass="boton" OnClick="btnImportar_Click" ValidationGroup="vgTabla" meta:resourcekey="btnImportar" />
            </div>
        </div>
        <p></p>
    </div>
    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <br />
    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" ValidationGroup="vgTabla" meta:resourcekey="valSummary" />
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
                        <asp:Label runat="server" ID="lblMensajeExito" meta:resourcekey="lblMensajeExito" />
                    </td>
                </tr>       
        </table>
    </asp:PlaceHolder>
</div>