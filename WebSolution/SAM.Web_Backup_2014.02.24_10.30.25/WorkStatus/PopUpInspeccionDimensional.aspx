<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.master" AutoEventWireup="true"
    CodeBehind="PopUpInspeccionDimensional.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpInspeccionDimensional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlResultado">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnDefectos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>
        <asp:Literal runat="server" ID="lblReporte" meta:resourcekey="lblReporte" />
    </h4>
    <asp:PlaceHolder runat="server" ID="phControles">
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:Label ID="lblTipoReporte" runat="server" meta:resourcekey="lblTipoReporte" CssClass="bold"></asp:Label><br />
                    <asp:TextBox ID="txtTipoReporte" runat="server" meta:resourcekey="txtTipoReporte"
                        CssClass="soloLectura"></asp:TextBox>
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtNumeroReporte" runat="server" meta:resourcekey="txtNumeroReporte"
                        CssClass="required">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblResultado" runat="server" meta:resourcekey="lblResultado" CssClass="bold"></asp:Label><br />
                    <asp:DropDownList ID="ddlResultado" runat="server" meta:resourcekey="ddlResultadoResource1">
                        <asp:ListItem Value="-1" Text="" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valResultado" InitialValue="-1" runat="server" ControlToValidate="ddlResultado"
                        Display="None" meta:resourcekey="valResultado"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Button ID="btnGenerar" runat="server" CssClass="boton" meta:resourcekey="btnGenerar"
                        OnClick="btnGenerar_Click" />
                </div>
            </div>
            <div class="divDerecho ancho45">
                <div class="separador">
                    <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte"
                        CssClass="bold" />
                    <br />
                    <telerik:RadDatePicker ID="rdpFechaReporte" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                        EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaReporte" runat="server" ControlToValidate="rdpFechaReporte"
                        Display="None" meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaLiberacion" runat="server" meta:resourcekey="lblFechaLiberacion"
                        CssClass="bold" />
                    <br />
                    <telerik:RadDatePicker ID="rdpFechaLiberacion" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaLiberacion" runat="server" ControlToValidate="rdpFechaLiberacion"
                        Display="None" meta:resourcekey="valFechaLiberacion"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
                <p>
                </p>
            </div>
        </div>
        <div class="divDerecho ancho25">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <div class="separador">
                        <asp:ValidationSummary runat="server" ID="summaryReporte" meta:resourcekey="valReporte"
                            CssClass="summary" />
                    </div>
                </div>
            </div>
        </div>
        <p></p>
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
        <p></p>
    </asp:PlaceHolder>
</asp:Content>
