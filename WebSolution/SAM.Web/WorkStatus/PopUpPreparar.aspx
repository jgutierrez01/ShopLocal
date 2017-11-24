<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpPreparar.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpPreparar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblPreparar" meta:resourcekey="lblPreparar" />
    </h4>
   <div class="divIzquierdo ancho50">
       <div class="separador">
            <asp:Label runat="server" ID="lblFecha" meta:resourcekey="lblFecha" CssClass="bold" />
            <br />
           <asp:TextBox ID="txtFolioPreparacion" runat="server" meta:resorcekey="txtFolioPreparacion"></asp:TextBox>
            <span class="required">*</span>
            <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="txtFolioPreparacion"
                Display="None" meta:resourcekey="valFecha"></asp:RequiredFieldValidator>
           <asp:RegularExpressionValidator ID="ValidaFormatoFolio" runat="server" meta:resourcekey="validaFormatoFolio"
                ValidationExpression="^\d{4}-\d{2}-\d{2}-\d{3}$" Display="None" ControlToValidate="txtFolioPreparacion" ></asp:RegularExpressionValidator>
        </div>        
        <div class="separador">
            <asp:Button meta:resourcekey="btnPreparar" runat="server" ID="btnPreparar" CssClass="boton"
                OnClick="btnPreparar_Click" />
        </div>
    </div>
    <div class="divDerecho ancho50">
        <div class="validacionesRecuadro" style="margin-top: 10px;">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valSummary"
                    CssClass="summary" meta:resourcekey="valSummary" Width="120px"  />
            </div>
        </div>
    </div>
    <p>
    </p>
</asp:Content>
