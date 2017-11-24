<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpAjustarLongitud.aspx.cs" Inherits="SAM.Web.Ingenieria.PopUpAjustarLongitud" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div>
        <h4>
            <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
        <div class="divIzquierdo ancho40">
            <div class="separador">
                <asp:Label runat="server" ID="lblSpool" CssClass="bold" meta:resourcekey="lblSpool" />
                <asp:Label runat="server" ID="lblSpoolData" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblNumeroControl" CssClass="bold" meta:resourcekey="lblNumeroControl" />
                <asp:Label runat="server" ID="lblNumeroControlData" />
            </div>
        </div>
        <div class="divDerecho ancho40">
            <div class="separador">
                <asp:Label runat="server" ID="lblEtiquetaMaterial" CssClass="bold" meta:resourcekey="lblEtiquetaMaterial" />
                <asp:Label runat="server" ID="lblEtiquetaMaterialData" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblDescripcion" CssClass="bold" meta:resourcekey="lblDescripcion" />
                <asp:Label runat="server" ID="lblDescripcionData" />
            </div>
        </div>
    </div>
    <p>
    </p>
    <div>
        <div class="divIzquierdo ancho40">
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtLongitudCorte" Enabled="false" meta:resourcekey="txtLongitudCorte" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtLongitudIngenieria" Enabled="false" meta:resourcekey="txtLongitudIngenieria" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNuevaLongitud" meta:resourcekey="txtNuevaLongitud" />
                <asp:RegularExpressionValidator runat="server" ID="revNuevaLongitud" ControlToValidate="txtNuevaLongitud"
                    Display="None" ValidationExpression="\d*" meta:resourcekey="revNuevaLongitud" />
            </div>
            <div class="separador">
                <asp:Button runat="server" ID="btnAjustar" meta:resourcekey="btnAjustar" OnClick="btnAjustar_OnClick"
                    CssClass="divDerecho boton" />
            </div>
            <asp:HiddenField runat="server" ID="hdnMaterialSpoolID" />
        </div>
        <div class="divDerecho ancho40">
            <div class="validacionesRecuadro" style="margin-top: 20px;">
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p>
        </p>
    </div>
</asp:Content>
