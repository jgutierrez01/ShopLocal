<%@ Page  Language="C#" MasterPageFile="~/Masters/Publico.Master" AutoEventWireup="true" CodeBehind="ActivaCuenta.aspx.cs" Inherits="SAM.Web.ActivaCuenta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
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
    <div class="contenedorCentral bordeNegroFull">
        <asp:PlaceHolder runat="server" ID="phControles">
            <div class="clear">
                <div class="titulo">
                    <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
                </div>
                <div class="divIzquierdo ancho60 bandaAzul">
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtUsername" MaxLength="100" meta:resourcekey="txtUsername" />
                    </div>
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtPassword" MaxLength="20" TextMode="Password" meta:resourcekey="txtPassword" />
                    </div>
                    <div class="notaPassword">
                        <asp:Literal runat="server" ID="litMsgContrasena" />
                    </div>
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtConfPassword" MaxLength="20" TextMode="Password" meta:resourcekey="txtConfPassword" />
                        <asp:CompareValidator runat="server" ID="cmpPassword" Display="None" ControlToValidate="txtPassword" ControlToCompare="txtConfPassword" meta:resourcekey="cmpPassword" />
                        <asp:RegularExpressionValidator runat="server" ID="regPassword" Display="None" ControlToValidate="txtPassword" meta:resourcekey="regPassword" />
                    </div>
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
                        <asp:CustomValidator runat="server" ID="cusUsername" OnServerValidate="cusUsername_ServerValidate" Display="None" meta:resourcekey="cusUsername" />
                    </div>
                </div>
                <div class="divIzquierdo ancho40">
                    <div class="validacionesRecuadro" style="margin-top:20px;">
                        <div class="validacionesHeader">&nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                        </div>
                    </div>
                </div>
                <p></p>
            </div>
            <div class="clear separador">
                <asp:Button ID="btnActivar" runat="server" CssClass="boton" OnClick="btnActivar_Click" meta:resourcekey="btnActivar" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
            <table class="mensajeExito small" cellpadding="0" cellspacing="0">
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
                <tr>
                    <td>&nbsp;</td>
                    <td class="ligas">
                        <div class="cuadroLigas">
                            <ul>
                                <li>
                                    <asp:HyperLink runat="server" ID="hlLogin" NavigateUrl="~/Login.aspx">
                                        <asp:Literal runat="server" ID="litLigaLogin" meta:resourcekey="litLigaLogin" />
                                    </asp:HyperLink>
                                </li>
                            </ul>
                        </div>  
                    </td>
                </tr>
            </table>        
        </asp:PlaceHolder>
    </div>
</asp:Content>
