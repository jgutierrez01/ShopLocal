<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpReqPinturaNumUnico.aspx.cs" Inherits="SAM.Web.Materiales.PopUpReqPinturaNumUnico" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtNumRequisicion" MaxLength="50"
                ValidationGroup="vgRequisita" meta:resourcekey="lblRequisicion" />
        </div>
        <br />
        <div class="separador">
            <asp:Label ID="lblFechaRequisicion" runat="server" meta:resourcekey="lblFechaRequisicion"
                CssClass="bold" />
            <br />
            <mimo:MappableDatePicker ID="dtpFechaRequisicion" runat="server" MinDate="01/01/1960"
                MaxDate="01/01/2050" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvFecha" Display="None" ControlToValidate="dtpFechaRequisicion"
                ValidationGroup="vgRequisita" meta:resourcekey="rfvFecha" />
        </div>
        <p>
        </p>
        <div class="separador">
            <asp:Button runat="server" CssClass="boton" ID="btnRequisitar" OnClick="btnRequisitar_Click"
                ValidationGroup="vgRequisita" meta:resourcekey="btnRequisitar" />
        </div>
    </div>
    <div class="divDerecho ancho50">
        <div class="validacionesRecuadro">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary ID="valRequisita" runat="server" ValidationGroup="vgRequisita"
                    meta:resourcekey="valRequisita" CssClass="summary" />
            </div>
        </div>
    </div>
</asp:Content>
