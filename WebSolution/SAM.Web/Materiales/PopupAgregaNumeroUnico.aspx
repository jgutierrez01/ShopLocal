<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopupAgregaNumeroUnico.aspx.cs" Inherits="SAM.Web.Materiales.PopupAgregaNumeroUnico"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadNumUnicos" MaxLength="10"
                    meta:resourcekey="txtCantidadNumUnicos" />
                <asp:RangeValidator ID="valCantidad" runat="server" ControlToValidate="txtCantidadNumUnicos"
                    Type="Integer" MaximumValue="2147483647" MinimumValue="1" meta:resourcekey="valCantidad"
                    Display="None"></asp:RangeValidator>
            </div>
            <div class="separador">
                <asp:Label ID="lblApartirDe" runat="server" meta:resourcekey="lblApartirDe" CssClass="bold"></asp:Label><br />
                <asp:Label ID="lblCodigo" runat="server" CssClass="bold"></asp:Label><asp:TextBox
                    runat="server" ID="txtNumeroInicial" MaxLength="10" CssClass="required smaller"></asp:TextBox><span
                        class="required">*</span>
                <asp:RequiredFieldValidator ID="valReqNumInicial" runat="server" ControlToValidate="txtNumeroInicial"
                    Display="None" meta:resourcekey="valReqNumInicial"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="valNumeroInicial" runat="server" ControlToValidate="txtNumeroInicial"
                    Type="Integer" MaximumValue="2147483647" MinimumValue="1" meta:resourcekey="valNumeroInicial"
                    Display="None"></asp:RangeValidator>
            </div>
            <div class="separador">
                <asp:Button runat="server" ID="btnGenerar" Text="Generar" OnClick="btnGenerar_Click"
                    meta:resourcekey="btnGenerar" CssClass="boton" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
                        CssClass="summary" />
                </div>
            </div>
        </div>
        <p style="clear: both; height: 2px;">
        </p>
</asp:Content>
