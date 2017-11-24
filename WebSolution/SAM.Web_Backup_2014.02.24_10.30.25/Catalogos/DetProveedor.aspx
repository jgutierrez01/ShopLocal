<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetProveedor.aspx.cs" Inherits="SAM.Web.Catalogos.DetProveedor" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstProveedor.aspx"
        meta:resourcekey="lblDetProveedor" />
    <div class="cntCentralForma">
    <div class="dashboardCentral">
        <div class="divIzquierdo ancho70">
            <div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                        MaxLength="50" meta:resourcekey="lblNombre" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtDescripcion" EntityPropertyName="Descripcion"
                        MaxLength="50" meta:Resourcekey="lblDescripcion" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtDireccion" EntityPropertyName="Direccion"
                        MaxLength="50" meta:Resourcekey="lblDireccion" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtTelefono" EntityPropertyName="Telefono"
                        MaxLength="50" meta:Resourcekey="lblTelefono" />
                </div>
            </div>
            <div>
                <br />
                <div class="separador">
                <h5>
                    <asp:Literal runat="server" ID="lblInfoContacto" meta:Resourcekey="lblInfoContacto" />
                    </h5>
                </div>
                <div class="divIzquierdo ancho50">
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtNombreContacto" EntityPropertyName="Contacto.Nombre"
                            MaxLength="50" meta:Resourcekey="lblNombreContacto" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtApPaterno" EntityPropertyName="Contacto.ApPaterno"
                            MaxLength="50" meta:Resourcekey="lblApPaterno" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtApMaterno" EntityPropertyName="Contacto.ApMaterno"
                            MaxLength="50" meta:Resourcekey="lblApMaterno" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtCorreo" EntityPropertyName="Contacto.CorreoElectronico"
                            MaxLength="50" meta:Resourcekey="lblCorreo" />
                        <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo"
                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None"
                            meta:Resourcekey="revCorreo" />
                    </div>
                </div>
                <div class="divIzquierdo ancho50">
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtTelOficina" EntityPropertyName="Contacto.TelefonoOficina"
                            MaxLength="50" meta:Resourcekey="lblTelOficina" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtTelParticular" EntityPropertyName="Contacto.TelefonoParticular"
                            MaxLength="50" meta:Resourcekey="lblTelParticular" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtCelular" EntityPropertyName="Contacto.TelefonoCelular"
                            MaxLength="50" meta:Resourcekey="lblCelular" />
                    </div>
                </div>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="validacionesRecuadro" style="margin-top: 23px;">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true"
                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p>
        </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            meta:resourcekey="btnGuardar" CssClass="boton" />
    </div>
</asp:Content>
