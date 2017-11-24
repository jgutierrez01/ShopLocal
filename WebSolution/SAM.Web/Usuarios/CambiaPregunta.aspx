<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="CambiaPregunta.aspx.cs" Inherits="SAM.Web.Usuarios.CambiaPregunta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="chkPropia">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phSistema" />
                    <telerik:AjaxUpdatedControl ControlID="phPropia" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:PlaceHolder runat="server" ID="phControles">
        <div style="width:580px;">
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </h4>
            <div class="divIzquierdo" style="width:65%;">
                <div class="separador">
                    <asp:CheckBox runat="server" ID="chkPropia" OnCheckedChanged="chkPropia_CheckChanged" AutoPostBack="true" meta:resourcekey="chkPropia" CssClass="inline" />
                </div>
                <asp:PlaceHolder runat="server" ID="phSistema">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblPregunta" AssociatedControlID="ddlPregunta" meta:resourcekey="lblPregunta" />
                        <asp:DropDownList runat="server" ID="ddlPregunta" Width="350"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqPregunta" ControlToValidate="ddlPregunta" Display="None" meta:resourcekey="reqPregunta" />
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phPropia" Visible="false">
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox ID="txtPregunta" runat="server" MaxLength="200" meta:resourcekey="txtPregunta" />
                    </div>
                </asp:PlaceHolder>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtRespuesta" MaxLength="110" meta:resourcekey="txtRespuesta" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtPassword" TextMode="Password" MaxLength="20" meta:resourcekey="txtPassword" />
                </div>
            </div>
            <div class="divIzquierdo" style="width:35%;">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" Width="120" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
        <div class="clear">
            <p></p>
            <asp:Button ID="btnCambiar" runat="server" CssClass="boton" Text="Cambiar" OnClick="btnCambiar_Click" meta:resourcekey="btnCambiar" />
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" Visible="false" ID="phMensaje">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin:20px auto 0 auto;">
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
                    <asp:Label runat="server" ID="lblMensajeExito" meta:resourcekey="lblMensajeExito" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
