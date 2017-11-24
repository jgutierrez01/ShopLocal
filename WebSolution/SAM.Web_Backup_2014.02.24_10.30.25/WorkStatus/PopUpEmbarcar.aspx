<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.master" AutoEventWireup="true"
    CodeBehind="PopUpEmbarcar.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpEmbarcar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblEmbarcar" meta:resourcekey="lblEmbarcar" />
    </h4>
    <asp:PlaceHolder ID="phControles" runat="server">
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroEmbarque" meta:resourcekey="txtNumeroEmbarque">
                </mimo:RequiredLabeledTextBox>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaEmbarque" meta:resourcekey="lblFechaEmbarque"
                    CssClass="bold" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaEmbarque" runat="server" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="mdpFechaEmbarque"
                    Display="None" meta:resourcekey="valFecha"></asp:RequiredFieldValidator>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                    TextMode="MultiLine" Rows="3" MaxLength="500">
                </mimo:LabeledTextBox>
            </div>

            <br /><br />

            <div class="separador">
                <mimo:LabeledTextBox ID="txtNota1" runat="server" meta:resourcekey="txtNota1"
                    MaxLength="50">
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtNota2" runat="server" meta:resourcekey="txtNota2"
                    MaxLength="50">
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtNota3" runat="server" meta:resourcekey="txtNota3"
                    MaxLength="50">
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtNota4" runat="server" meta:resourcekey="txtNota4"
                    MaxLength="50">
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtNota5" runat="server" meta:resourcekey="txtNota5"
                    MaxLength="50">
                </mimo:LabeledTextBox>
            </div>

            <br />
            <br />
            <div class="separador">
                <asp:Button meta:resourcekey="btnEmbarcar" runat="server" ID="btnEmbarcar" CssClass="boton"
                    OnClick="btnEmbarcar_Click" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="validacionesRecuadro" style="margin-top: 20px;">
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                        />
                </div>
            </div>
        </div>
        <p>
        </p>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
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
