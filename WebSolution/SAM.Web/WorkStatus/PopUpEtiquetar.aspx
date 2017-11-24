<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpEtiquetar.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpEtiquetar"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblEtiquetar" meta:resourcekey="lblEtiquetar" />
    </h4>
   <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroEtiqueta" 
                meta:resourcekey="txtNumeroEtiqueta">
            </mimo:RequiredLabeledTextBox>
        </div>        
        <div class="separador">
            <asp:Button meta:resourcekey="btnEtiquetar" runat="server" ID="btnEtiquetar" CssClass="boton"
                OnClick="btnEtiquetar_Click" />
        </div>
    </div>
    <div class="divDerecho ancho50">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valSummary"
                    CssClass="summary" meta:resourcekey="valSummary" Width="120px" />
            </div>
        </div>
    </div>
    <p>
    </p>
</asp:Content>
