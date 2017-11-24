<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpImprimirEtiqueta.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpImprimirEtiqueta"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
 <h4>
        <asp:Literal runat="server" ID="lblImprimirEtiqueta" meta:resourcekey="lblImprimirEtiqueta" />
    </h4>
    <div class="divIzquierdo ancho50">
     <div class="separador">
           <asp:CheckBox ID="chkSeleccionadas" runat="server" meta:resourcekey="chkSeleccionadas" CssClass="checkBold" Checked="true" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtNumeroEtiqueta" 
                meta:resourcekey="txtNumeroEtiqueta" >
            </mimo:LabeledTextBox>
        </div>
         <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtNumeroControl" 
                 meta:resourcekey="txtNumeroControl" >
             </mimo:LabeledTextBox>
        </div>
         <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtOrdenTrabajo" 
                 meta:resourcekey="txtOrdenTrabajo" >
             </mimo:LabeledTextBox>
        </div>
        <div class="separador">
            <asp:Button meta:resourcekey="btnImprimir" runat="server" ID="btnImprimir" CssClass="boton"
                OnClick="btnImprimir_OnClick" />
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
