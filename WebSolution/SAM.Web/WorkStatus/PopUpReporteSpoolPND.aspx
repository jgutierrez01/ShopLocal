<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
         CodeBehind="PopUpReporteSpoolPND.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpReporteSpoolPND" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblReporte" meta:resourcekey="lblReporte" />
    </h4>
    <asp:PlaceHolder ID="phControles" runat="server">
        <div>
            <div class="divIzquierdo ancho35">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtNumeroReporte" runat="server" meta:resourcekey="txtNumeroReporte"
                        ValidationGroup="vgGenerar" CssClass="required">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte"
                        CssClass="bold" />
                    <br />
                    <telerik:RadDatePicker ID="rdpFechaReporte" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                        EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaReporte" runat="server" ControlToValidate="rdpFechaReporte"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaPrueba" runat="server" meta:resourcekey="lblFechaPrueba" CssClass="bold" />
                    <br />
                    <telerik:RadDatePicker ID="rdpFechaPrueba" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                        EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaPrueba" runat="server" ControlToValidate="rdpFechaPrueba"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaPrueba"></asp:RequiredFieldValidator>
                </div>
                <div class="separador" >
                    <mimo:LabeledTextBox ID="txtPresion" meta:resourcekey="txtPresion" runat="server"></mimo:LabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblResultado" runat="server" meta:resourcekey="lblResultado" CssClass="bold"></asp:Label><br />
                    <asp:DropDownList ID="ddlResultado" runat="server">
                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valResultado" InitialValue="-1" runat="server" ControlToValidate="ddlResultado"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valResultado"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
                <br />
                <br />
                <div class="separador">
                    <mimo:AutoDisableButton ID="btnAGenerar" runat="server" CssClass="boton" meta:resourcekey="btnGenerar"
                         OnClick="btnGenerar_Click" ValidationGroup="vgGenerar" />
                </div>
            </div>
            <div class="divDerecho ancho65">
                <div class="divIzquierdo ancho50">
                    <div class="validacionesRecuadro">
                        <div class="validacionesHeader">
                        </div>
                        <div class="validacionesMain">
                            <div class="separador">
                                <asp:ValidationSummary runat="server" ID="summaryReporte" ValidationGroup="vgGenerar"
                                    meta:resourcekey="valReporte" CssClass="summary" />
                            </div>
                        </div>
                    </div>
                </div>
                <p>
                </p>
            </div>
            <p>
            </p>
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
                    <asp:Label runat="server" ID="lblMensajeExitoTercerRechazo" meta:resourcekey="lblMensajeExitoTercerRechazo"  Visible="false"/>
                    <asp:Label runat="server" ID="lblMensajeExitoRechazo" meta:resourcekey="lblMensajeExitoRechazo"  Visible="false"/>
                    <asp:Label runat="server" ID="lblMensajeExitoReporte" meta:resourcekey="lblMensajeExitoReporte" Visible="false"/> 
                    <br />
                    <br />
                    <samweb:LinkVisorReportes ID="lnkReporte" runat="server" meta:resourcekey="lnkReporte" Visible="false" ></samweb:LinkVisorReportes>
                </td>
            </tr>            
        </table>
    </asp:PlaceHolder>
</asp:Content>