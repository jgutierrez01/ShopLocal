<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="DetUsuario.aspx.cs" Inherits="SAM.Web.Administracion.DetUsuario" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" NavigateUrl="~/Administracion/LstUsuario.aspx" />
    <div class="cntCentralForma">
        <div class="dashboardCentral detUsuario">
            <div class="divIzquierdo ancho30">
                <p>
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtUsername" runat="server" ID="txtUsername" EntityPropertyName="Username" MaxLength="50" />
                </p>
                <p>
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtCorreo" runat="server" ID="txtCorreo" EntityPropertyName="Email" MaxLength="150" />
                    <asp:RegularExpressionValidator meta:resourcekey="regCorreo" runat="server" ID="regCorreo" ControlToValidate="txtCorreo" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" Display="None" />
                </p>
                <p>
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtNombre" runat="server" ID="txtNombre" EntityPropertyName="Nombre" MaxLength="50" />
                </p>
                <p>
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtApPaterno" runat="server" ID="txtApPaterno" EntityPropertyName="ApPaterno" MaxLength="50" />
                </p>
                <p>
                    <mimo:LabeledTextBox meta:resourcekey="txtApMaterno" runat="server" ID="txtApMaterno" EntityPropertyName="ApMaterno" MaxLength="50"/>
                </p>
            </div>
            <div class="divIzquierdo ancho30">
                <p class="soloLectura">
                    <mimo:LabeledTextBox meta:resourcekey="txtEstatus" runat="server" ID="txtEstatus" EntityPropertyName="Estatus" ReadOnly="true" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblPerfil" AssociatedControlID="ddlPerfil" Text="Perfil" meta:resourcekey="lblPerfil" />
                    <mimo:MappableDropDown runat="server" ID="ddlPerfil" EntityPropertyName="PerfilID" meta:resourcekey="ddlPerfil" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="reqPerfil" ControlToValidate="ddlPerfil" Display="None" meta:resourcekey="reqPerfil" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblIdioma" AssociatedControlID="ddlIdioma" Text="Idioma" meta:resourcekey="lblIdioma" />
                    <mimo:MappableDropDown runat="server" ID="ddlIdioma" EntityPropertyName="Idioma">
                        <asp:ListItem Text="" Value=""/>
                        <asp:ListItem Value="en-US" meta:resourcekey="ingles" />
                        <asp:ListItem Value="es-MX" meta:resourcekey="espanol" />
                    </mimo:MappableDropDown>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="reqIdioma" ControlToValidate="ddlIdioma" Display="None" meta:resourcekey="reqIdioma" />
                </p>
                <asp:PlaceHolder runat="server" ID="phAcciones" Visible="False">
                    <div class="ligas">
                        <asp:LinkButton runat="server" ID="lnkReiniciarPassword" OnClick="lnkReiniciarPassword_OnClick" CausesValidation="false" meta:resourcekey="lnkReiniciarPassword" />
                        <asp:LinkButton runat="server" ID="lnkDesactivar" OnClick="lnkDesactivar_OnClick" CausesValidation="false" meta:resourcekey="lnkDesactivar" />
                        <asp:LinkButton runat="server" ID="lnkDesbloquear" OnClick="lnkDesbloquear_OnClick" CausesValidation="false" meta:resourcekey="lnkDesbloquear" />
                        <asp:LinkButton runat="server" ID="lnkReenviarCorreo" OnClick="lnkReenviarCorreo_OnClick" CausesValidation="false" meta:resourcekey="lnkReenviarCorreo" />
                        <asp:LinkButton runat="server" ID="lnkReactivar" OnClick="lnkReactivar_OnClick" CausesValidation="false"  meta:resourcekey="lnkReactivar" />
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top:24px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <div class="ancho90">
                <h5><asp:Label runat="server" ID="lblProyectos" meta:resourcekey="lblProyectos" /></h5>
            </div>
            <div class="listaCheck clear ancho90">
                <asp:CheckBoxList runat="server" ID="chkProyectos" CssClass="cajaAzul" RepeatColumns="3" width="100%" />
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button meta:resourcekey="btnGuardar" CssClass="boton" runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" />    
    </div>
</asp:Content>
