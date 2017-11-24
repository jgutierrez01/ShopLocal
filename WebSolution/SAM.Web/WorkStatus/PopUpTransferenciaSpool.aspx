<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpTransferenciaSpool.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpTransferenciaSpool" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
     <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlDestino">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnDefectos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <h4>
        <asp:Literal runat="server" ID="lblTransferencia" meta:resourcekey="lblTransferencia" />
    </h4>
    <asp:PlaceHolder runat="server" ID="phControles">
       
        <div class="divIzquierdo ancho50">                
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroTransferencia" meta:resourcekey="txtNumeroTransferencia">
                </mimo:RequiredLabeledTextBox>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaTransferencia" meta:resourcekey="lblFechaTransferencia"
                    CssClass="bold" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaTransferencia" runat="server" Style="width: 209px" AutoPostBack="true" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="mdpFechaTransferencia"
                    Display="None" meta:resourcekey="valFecha"></asp:RequiredFieldValidator>
            </div>
            <br />
            <div class="separador">
                <asp:Label ID="lblDestino" runat="server" meta:resourcekey="lblDestino" CssClass="bold"></asp:Label><br />
                <mimo:MappableDropDown runat="server" ID="ddlDestino" EntityPropertyName="DestinoID" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valDestino" runat="server" ControlToValidate="ddlDestino"
                Display="None" meta:resourcekey="valDestino"></asp:RequiredFieldValidator>
            </div>
            <br />
            <div class="separador">
                <asp:Button meta:resourcekey="btnTransferencia" runat="server" ID="btnTransferencia" CssClass="boton"
                OnClick="btnTransferencia_Click" />
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
        </table>
    </asp:PlaceHolder>

     <asp:Panel ID="pnDefectos" runat="server" Visible="false">
     </asp:Panel>
</asp:Content>
