<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpRequisicionPruebas.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpRequisicionPruebas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblRequisicion" meta:resourcekey="lblRequisicion" />
    </h4>
    <asp:PlaceHolder runat="server" ID="phControles">
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:Label ID="lblTipoPrueba" runat="server" meta:resourcekey="lblTipoPrueba" CssClass="bold"></asp:Label><br />
                    <asp:TextBox ID="txtTipoPrueba" runat="server" CssClass="soloLectura" ReadOnly="true" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtNumeroRequisicion" runat="server" meta:resourcekey="txtNumeroRequisicion"
                        CssClass="required" MaxLength="50">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaRequisicion" runat="server" meta:resourcekey="lblFechaRequisicion"
                        CssClass="bold" />
                    <br />
                    <telerik:RadDatePicker ID="rdpFechaRequisicion" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaRequisicion" runat="server" ControlToValidate="rdpFechaRequisicion"
                        Display="None" meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <br />
                <div class="separador">
                    <mimo:AutoDisableButton ID="btnARequisitar" runat="server" CssClass="boton" meta:resourcekey="btnRequisitar"
                        OnClick="btnRequisitar_Click" />
                </div>
            </div>
            <div class="divDerecho ancho45">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtCodigo" runat="server" meta:resourcekey="txtCodigo"
                        CssClass="required" Enabled="True" MaxLength="50">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
            </div>
            <p>
            </p>
        </div>
        <div class="divDerecho ancho30">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <div class="separador">
                        <asp:ValidationSummary runat="server" ID="valSummary" meta:resourcekey="valSummary"
                            CssClass="summary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensajeExito" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 20px auto 0 auto;">
            <tr>
                <td rowspan="3" class="icono">
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
                <td>
                    <br />
                    <samweb:LinkVisorReportes ID="lnkReporte" runat="server" meta:resourcekey="lnkReporte"></samweb:LinkVisorReportes>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>