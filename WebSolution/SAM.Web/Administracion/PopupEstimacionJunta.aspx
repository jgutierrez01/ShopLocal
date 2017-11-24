<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopupEstimacionJunta.aspx.cs" Inherits="SAM.Web.Administracion.PopupEstimacionJunta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:PlaceHolder runat="server" ID="phControles">        
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" Text="Estimacion Junta" />
            </h4>
            <div class="divIzquierdo ancho70">
                <div class="divIzquierdo ancho50">
                    <div class="separador">
                        <asp:RadioButton ID="radEstimacionExistente" AutoPostBack="true" meta:resourcekey="radEstimacionExistente"
                            OnCheckedChanged="radEstimacionExistente_OnCheckedChanged" Text="Estimacion Existente"
                            runat="server" GroupName="Estimacion" CssClass="displayInline" />
                    </div>
                    <asp:Panel ID="pnlExistente" runat="server" Visible="false">
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblNumeroEstimacionExistente" runat="server" ID="lblNumeroEstimacionExistente"
                            Text="NumeroEstimacion:" AssociatedControlID="ddlNumeroEstimacionExistente" />
                        <mimo:MappableDropDown runat="server" ID="ddlNumeroEstimacionExistente" />
                         <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="NumeroEstimacionExistenteRequerido" ErrorMessage="El numero de estimacion es requerido"
                            runat="server" ControlToValidate="ddlNumeroEstimacionExistente" Display="None"
                            meta:resourcekey="NumeroEstimacionExistenteRequerido" />
                    </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNueva" runat="server">
                      <div class="separador">
                        <asp:Label meta:resourcekey="lblNumeroEstimacionNueva" runat="server" ID="lblNumeroEstimacionNueva"
                            Text="NumeroEstimacion:" AssociatedControlID="txtNumerioEstimacionNueva" />
                        <asp:TextBox ID="txtNumerioEstimacionNueva" runat="server" CssClass="required"></asp:TextBox>
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="NumeroEstimacionRequerido" ErrorMessage="El numero de estimacion es requerido"
                            runat="server" ControlToValidate="txtNumerioEstimacionNueva" Display="None" meta:resourcekey="NumeroEstimacionRequerido" />
                    </div>
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblFechaEstimacion" runat="server" ID="lblFechaEstimacion"
                            Text="Fecha de Estimacion:" AssociatedControlID="dtpFechaEstimacion" />
                        <mimo:MappableDatePicker ID="dtpFechaEstimacion" runat="server" MinDate="01/01/1960"
                            MaxDate="01/01/2050" />
                        <asp:RequiredFieldValidator ID="FechaDeEstimacionRequerida" ErrorMessage="La fecha de estimacion es requerida"
                            runat="server" ControlToValidate="dtpFechaEstimacion" Display="None" meta:resourcekey="FechaDeEstimacionRequerida" />
                    </div>
                    </asp:Panel>
                </div>
                <div class="divDerecho ancho50">
                    <div class="separador">
                        <asp:RadioButton ID="radNuevaEstimacion" meta:resourcekey="radNuevaEstimacion" AutoPostBack="true"
                            Text="Nueva Estimacion" OnCheckedChanged="radNuevaEstimacion_OnCheckedChanged"
                            runat="server" GroupName="Estimacion" CssClass="displayInline" />
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
            <div >
                <h3>
                    <asp:Literal meta:resourcekey="lblConceptos" runat="server" ID="lblConceptos" Text="Conceptos" />
                </h3>
            </div>
                    <asp:CheckBoxList ID="chkEstimaciones" RepeatColumns="2" Width="100%" runat="server"
                        CssClass="displayInline">
                    </asp:CheckBoxList>
            <p> </p>
            <div>
                <asp:Button ID="btnEstimar" OnClick="btnEstimar_Click" runat="server" meta:resourcekey="btnEstimar"
                    class="boton" />
            </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 20px auto 0 auto;">
            <tr>
                <td rowspan="2" class="icono">
                    <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
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
