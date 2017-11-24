<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpPinturaNumeroUnico.aspx.cs" Inherits="SAM.Web.Materiales.PopUpPinturaNumeroUnico" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50">
            <h3>
                <asp:Literal runat="server" ID="lblPrimarios"  meta:resourcekey="lblPrimarios" />
            </h3>
            <div class="separador">
                <asp:Label ID="lblFechaPrimarios" runat="server" meta:resourcekey="lblFecha" CssClass="bold" />
                <br />
                <mimo:MappableDatePicker ID="dtpFechaPrimarios" runat="server" MinDate="01/01/1960"
                    MaxDate="01/01/2050" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumReportePrimarios" MaxLength="50" meta:resourcekey="lblNumReporte" />
            </div>
            <div class="separador">
                <asp:CheckBox runat="server" ID="chkLiberado" />
                <asp:Label runat="server" ID="lblLiberado" CssClass="bold" meta:resourcekey="lblLiberado" />
            </div>
            <div class="separador">
                <asp:Button runat="server" CssClass="boton" ID="btnGuardar" OnClick="btnGuardar_Click"
                    ValidationGroup="vgPinta" meta:resourcekey="btnGuardar" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <h3>
                <asp:Literal runat="server" ID="lblIntermedio"  meta:resourcekey="lblIntermedio" />
            </h3>
            <div class="separador">
                <asp:Label ID="lblFechaIntermedios" runat="server" meta:resourcekey="lblFecha" CssClass="bold" />
                <br />
                <mimo:MappableDatePicker ID="dtpFechaIntermedios" runat="server" MinDate="01/01/1960"
                    MaxDate="01/01/2050" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumReporteIntermedio" MaxLength="50" meta:resourcekey="lblNumReporte" />
            </div>
        </div>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="vgPinta"
                    meta:resourcekey="valSummary" CssClass="summary" />
            </div>
            <p style="clear: both; height: 2px;">
            </p>
        </div>
    </div>
</asp:Content>
