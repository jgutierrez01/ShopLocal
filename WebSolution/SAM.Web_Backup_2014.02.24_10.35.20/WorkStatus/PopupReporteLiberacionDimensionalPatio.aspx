<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.master" AutoEventWireup="true"
    CodeBehind="PopupReporteLiberacionDimensionalPatio.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupReporteLiberacionDimensionalPatio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:PlaceHolder runat="server" ID="phControles">
        <h4>
            <asp:Literal ID="litTitulo" runat="server" meta:resourcekey="litTitulo"></asp:Literal>
        </h4>
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:RadioButton ID="radEstimacionExistente" AutoPostBack="true" meta:resourcekey="radReporteExistente"
                        OnCheckedChanged="radReporteExistente_OnCheckedChanged" Text="Incluir En Reporte Existente"
                        runat="server" GroupName="Reporte" CssClass="displayInline" />
                </div>
                <asp:Panel ID="pnExistente" runat="server" Visible="false">
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblNumeroReporteExistente" runat="server" ID="lblNumeroReporteExistente"
                            Text="Numero de Reporte:" AssociatedControlID="ddlNumeroReporteExistente" />
                        <mimo:MappableDropDown runat="server" ID="ddlNumeroReporteExistente" OnDataBound="ddlNumeroReporteExistente_OnDataBound" /><span class="required">*</span>
                        <asp:RequiredFieldValidator ID="NumeroReporteExistenteRequerido" ErrorMessage="El numero de reporte es requerido"
                            runat="server" ControlToValidate="ddlNumeroReporteExistente" Display="None" meta:resourcekey="NumeroReporteExistenteRequerido" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnNuevo" runat="server">
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblNumeroReporteNueva" runat="server" ID="lblNumeroReporteNueva"
                            Text="Numero Reporte:" AssociatedControlID="txtNumerioReporteNuevo" />
                        <asp:TextBox ID="txtNumerioReporteNuevo" runat="server" CssClass="required"></asp:TextBox><span class="required">*</span>
                        <asp:RequiredFieldValidator ID="NumeroReporteRequerido" ErrorMessage="El numero de reporte es requerido"
                            runat="server" ControlToValidate="txtNumerioReporteNuevo" Display="None" meta:resourcekey="NumeroReporteRequerido" />
                    </div>
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblFechaReporte" runat="server" ID="lblFechaReporte"
                            Text="Fecha de Reporte:" AssociatedControlID="dtpFechaReporte" />
                        <mimo:MappableDatePicker ID="dtpFechaReporte" runat="server" MinDate="01/01/1960"
                            MaxDate="01/01/2050" /><span class="required">*</span>
                        <asp:RequiredFieldValidator ID="FechaDeReporteRequerida" ErrorMessage="La fecha de reporte es requerida"
                            runat="server" ControlToValidate="dtpFechaReporte" Display="None" meta:resourcekey="FechaDeReporteRequerida" />
                    </div>
                </asp:Panel>
            </div>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:RadioButton ID="radNuevoReporte" meta:resourcekey="radNuevoReporte" AutoPostBack="true"
                        Text="Nuevo Reporte" OnCheckedChanged="radNuevoReporte_OnCheckedChanged" runat="server"
                        GroupName="Reporte" CssClass="displayInline" />
                </div>
            </div>
        </div>
        <div class="divDerecho ancho30">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary HeaderText="Errores" runat="server" ID="valSummary" DisplayMode="BulletList"
                        CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <div class="divIzquierdo">
            <div class="divDerecho">
                <br />
                <br />
                <asp:Button ID="btnGuardar" OnClick="btnGuardar_Click" meta:resourcekey="btnGuardar"
                    runat="server" Text="Guardar" class="boton" />
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 20px auto 0 auto;">
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
                    <br />
                    <br />                  
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
