<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.master" AutoEventWireup="true"
    CodeBehind="PopupEspecificarSistema.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupEspecificarSistema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
   
     <h4>
        <asp:Literal runat="server" ID="lblEspecificarSistema" meta:resourcekey="lblEspecificarSistema" />
    </h4>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtSistema" meta:resourcekey="txtSistema" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtColor" meta:resourcekey="txtColor" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtCodigo" meta:resourcekey="txtCodigo" />
                </div>
                <div class="separador">
                        <%--<asp:Button meta:resourcekey="btnIncluir" runat="server" ID="btnIncluir" Text="Incluir"
                    CssClass="boton" OnClick="btnIncluir_OnClick" />--%>
                    <mimo:AutoDisableButton ID="btnAIncluir" meta:resourcekey="btnIncluir" runat="server" Text="Incluir"
                         CssClass="boton" OnClick="btnIncluir_OnClick" />
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

</asp:Content>
