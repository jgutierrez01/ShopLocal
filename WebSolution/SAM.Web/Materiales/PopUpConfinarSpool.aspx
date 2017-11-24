<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpConfinarSpool.aspx.cs" Inherits="SAM.Web.Materiales.PopUpConfinarSpool" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div>
        <div>
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
            <div class="divIzquierdo ancho40">
                <p>
                    <asp:Label ID="lblSpool" runat="server" CssClass="bold" meta:resourcekey="lblSpool" />
                    <asp:Label runat="server" ID="lblSpoolData" />
                </p>
                <p>
                    <asp:Label ID="lblEspecificacion" runat="server" CssClass="bold" meta:resourcekey="lblEspecificacion" />
                    <asp:Label runat="server" ID="lblEspecificacionData" />
                </p>
                <p>
                    <asp:Label ID="lblCedula" runat="server" CssClass="bold" meta:resourcekey="lblCedula" />
                    <asp:Label runat="server" ID="lblCedulaData" />
                </p>
                <p>
                    <asp:Label ID="lblMaterial" runat="server" CssClass="bold" meta:resourcekey="lblMaterial" />
                    <asp:Label runat="server" ID="lblMaterialData" />
                </p>
            </div>
            <div class="divIzquierdo ancho30">
                <p>
                    <asp:Label ID="lblPeso" runat="server" CssClass="bold" meta:resourcekey="lblPeso" />
                    <asp:Label runat="server" ID="lblPesoData" />
                </p>
                <p>
                    <asp:Label ID="lblArea" runat="server" CssClass="bold" meta:resourcekey="lblArea" />
                    <asp:Label runat="server" ID="lblAreaData" />
                </p>
                <p>
                    <asp:Label ID="lblPnd" runat="server" CssClass="bold" meta:resourcekey="lblPnd" />
                    <asp:Label runat="server" ID="lblPndData" />
                </p>
                <p>
                    <asp:Label ID="lblPdi" runat="server" CssClass="bold" meta:resourcekey="lblPdi" />
                    <asp:Label runat="server" ID="lblPdiData" />
                </p>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top: 24px;">
                    <div class="validacionesHeader">
                        &nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary"
                            ValidationGroup="vgConfinado" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
        <p>
        </p>
        <div class="divIzquierdo ancho50">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtMotivo" TextMode="MultiLine" ValidationGroup="vgConfinado"
                meta:resourcekey="lblMotivo" />
        </div>
    </div>
    <p>
    </p>
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <asp:HiddenField runat="server" ID="hdnTipoConfinado" />
            <asp:Button runat="server" ID="btnConfinar" CssClass="divIzquierdo boton" ValidationGroup="vgConfinado"
                meta:resourcekey="btnConfinar" OnClick="btnConfinar_Click" />
            <asp:Button runat="server" ID="btnDesconfinar" CssClass="divIzquierdo boton" ValidationGroup="vgConfinado"
                Visible="false" meta:resourcekey="btnDesconfinar" OnClick="btnDesconfinar_Click" />
        </div>
    </div>
</asp:Content>
