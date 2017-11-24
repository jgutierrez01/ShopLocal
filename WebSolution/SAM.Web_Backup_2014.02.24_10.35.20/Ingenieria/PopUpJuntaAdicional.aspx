<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpJuntaAdicional.aspx.cs" Inherits="SAM.Web.Ingenieria.PopUpJuntaAdicional" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <style type="text/css">
        .lista-juntas label { bottom: 1px; display: inline-block; position: relative; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userAgentDependant" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho50">
            <p>
                <asp:Label ID="litItemCode" CssClass="bold" runat="server" meta:resourcekey="litItemCode" />
                <asp:Label ID="lblItemCode" runat="server" />
            </p>
            <p>
                <asp:Label ID="litDescripcion" CssClass="bold" runat="server" meta:resourcekey="litDescripcion" />
                <asp:Label ID="lblDescripcion" runat="server" />
            </p>
            <p>
                <asp:Label ID="litCantidad" CssClass="bold" runat="server" meta:resourcekey="litCantidad" />
                <asp:Label ID="lblCantidad" runat="server" />
            </p>
            <p>
                <asp:Label ID="litJuntas" CssClass="bold" runat="server" meta:resourcekey="litJuntas" /><br />
                <asp:Panel runat="server" ID="pnlJuntas">
                </asp:Panel>
            </p>
        </div>
        <div class="divIzquierdo ancho50">
            <p>
                <asp:Label ID="litDiametro1" CssClass="bold" runat="server" meta:resourcekey="litDiametro1" />
                <asp:Label ID="lblDiametro1" runat="server" />
            </p>
            <p>
                <asp:Label ID="litDiametro2" CssClass="bold" runat="server" meta:resourcekey="litDiametro2" />
                <asp:Label ID="lblDiametro2" runat="server" />
            </p>
            <p>
                <asp:Label ID="litEtiqueta" CssClass="bold" runat="server" meta:resourcekey="litEtiqueta" />
                <asp:Label ID="lblEtiqueta" runat="server" />
            </p>
            <p>
                <asp:Label ID="litEtiquetaSeccion" CssClass="bold" runat="server" meta:resourcekey="litEtiquetaSeccion" />
                <asp:Label ID="lblEtiquetaSeccion" runat="server" />
            </p>
        </div>
        <p></p>
    </div>

    <div class="divIzquierdo ancho35" style="margin-top: 20px">
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtDistancia" meta:resourcekey="txtDistancia" ValidationGroup="valJuntaAdicional" />
            <asp:RegularExpressionValidator runat="server" ID="revDistancia" ValidationGroup="valJuntaAdicional" ControlToValidate="txtDistancia" ValidationExpression="^[0-9]*$" Display="None" EnableClientScript="true" meta:resourcekey="revDistancia" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtEtiqueta" ValidationGroup="valJuntaAdicional" meta:resourcekey="txtEtiqueta" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtEtiquetaMaterial" ValidationGroup="valJuntaAdicional" meta:resourcekey="txtEtiquetaMaterial" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtEtiquetaTubo" ValidationGroup="valJuntaAdicional" meta:resourcekey="txtEtiquetaTubo" />
        </div>
        <div runat="server" id="juntaCamposExtras">
            <div class="separador">
                <asp:Label CssClass="bold" runat="server" ID="lblCedula" meta:resourcekey="lblCedula" /><br />
                <asp:DropDownList runat="server" ID="ddlCedula" />
            </div>
            <div class="separador">
                <asp:Label CssClass="bold" runat="server" ID="lblFamiliaAcero" meta:resourcekey="lblFamiliaAcero" /><br />
                <asp:DropDownList runat="server" ID="ddlFamiliaAcero" />
            </div>
        </div>
        <div class="separador">
            <asp:Button runat="server" ID="btnGuardar" CssClass="boton" ValidationGroup="valJuntaAdicional" CausesValidation="true" OnClick="btnGuardar_OnClick" meta:resourcekey="btnGuardar" />
            <asp:Button runat="server" ID="btnCancelar" CssClass="boton" CausesValidation="false" OnClick="btnCancelar_OnClick" meta:resourcekey="btnCancelar" />
        </div>
    </div>
    <div class="divIzquierdo ancho25" style="margin-top: 20px">
        <div class="separador">
            <asp:Label CssClass="bold" runat="server" ID="lblJuntaNoBW" meta:resourcekey="lblJuntaNoBW" /><br />
            <asp:CheckBoxList runat="server" ID="chklJuntas" TextAlign="Right" CssClass="lista-juntas">
            </asp:CheckBoxList>
        </div>
    </div>
    <div class="divIzquierdo ancho40">
        <div class="validacionesRecuadro" style="margin-top: 23px;">
            <div class="validacionesHeader"></div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valsummary" ValidationGroup="valJuntaAdicional" EnableClienteScript="true"
                    DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
            </div>
        </div>
    </div>
    <p></p>
</asp:Content>
