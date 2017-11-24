<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpTransferenciaCorte.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpTransferenciaCorte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="contenedorCentral">
        <div class="popupsH4">
            <asp:Literal runat="server" ID="lblTransferencia" meta:resourcekey="lblTransferencia" />
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label runat="server" ID="lblLocalizacion" meta:resourcekey="lblLocalizacion"
                    CssClass="bold" />
                <br />
                <asp:PlaceHolder ID="phLocalizacion" runat="server">
                    <asp:DropDownList ID="ddlLocalizacion" runat="server" CssClass="required" meta:resourcekey="ddlLocalizacionResource1">
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valLocalizacion" runat="server" Display="None" ControlToValidate="ddlLocalizacion"
                        meta:resourcekey="valLocalizacion" ValidationGroup="vgTranferir" />
                </asp:PlaceHolder>
            </div>
            <div class="separador">
                <samweb:BotonProcesando runat="server" ID="btnTransferir" CssClass="boton" OnClick="btnTransferir_Click" meta:resourcekey="btnTransferir" ValidationGroup="vgTranferir" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <div class="separador">
                        <asp:ValidationSummary runat="server" ID="valTransferir" ValidationGroup="vgTranferir"
                            meta:resourcekey="valTransferir" CssClass="summary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <p></p>
</asp:Content>
