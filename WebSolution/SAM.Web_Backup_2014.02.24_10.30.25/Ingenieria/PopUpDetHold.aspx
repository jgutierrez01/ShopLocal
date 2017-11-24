<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpDetHold.aspx.cs" Inherits="SAM.Web.Ingenieria.PopUpDetHold" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div>
        <div>
            <div>
                <h4>
                    <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
                <div class="divIzquierdo ancho40">
                    <div class="separador">
                        <asp:Label ID="lblSpool" runat="server" CssClass="bold" meta:resourcekey="lblSpool" />
                        <asp:Label runat="server" ID="lblSpoolData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblEspecificacion" runat="server" CssClass="bold" meta:resourcekey="lblEspecificacion" />
                        <asp:Label runat="server" ID="lblEspecificacionData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblCedula" runat="server" CssClass="bold" meta:resourcekey="lblCedula" />
                        <asp:Label runat="server" ID="lblCedulaData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblMaterial" runat="server" CssClass="bold" meta:resourcekey="lblMaterial" />
                        <asp:Label runat="server" ID="lblMaterialData" />
                    </div>
                </div>
                <div class="divIzquierdo ancho30">
                    <div class="separador">
                        <asp:Label ID="lblPeso" runat="server" CssClass="bold" meta:resourcekey="lblPeso" />
                        <asp:Label runat="server" ID="lblPesoData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblArea" runat="server" CssClass="bold" meta:resourcekey="lblArea" />
                        <asp:Label runat="server" ID="lblAreaData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblPnd" runat="server" CssClass="bold" meta:resourcekey="lblPnd" />
                        <asp:Label runat="server" ID="lblPndData" />
                    </div>
                    <div class="separador">
                        <asp:Label ID="lblPdi" runat="server" CssClass="bold" meta:resourcekey="lblPdi" />
                        <asp:Label runat="server" ID="lblPdiData" />
                    </div>
                </div>
                <div class="divIzquierdo ancho30">
                    <div class="validacionesRecuadro">
                        <div class="validacionesHeader">
                            &nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valHold" DisplayMode="BulletList" CssClass="summary"
                                ValidationGroup="vgHold" meta:resourcekey="valHold" Width="160" />
                        </div>
                    </div>
                    <%--<asp:ValidationSummary runat="server" ID="valHold" ValidationGroup="vgHold" DisplayMode="BulletList" />--%>
                </div>
            </div>
            <p>
            </p>
            <div class="divIzquierdo ancho50">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtMotivo" TextMode="MultiLine" ValidationGroup="vgHold"
                    meta:resourcekey="lblMotivo" />
            </div>
        </div>
        <p>
        </p>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:HiddenField runat="server" ID="hdnTipoHold" />
                <asp:Button runat="server" ID="btnHold" CssClass="divIzquierdo boton" ValidationGroup="vgHold"
                    meta:resourcekey="btnHold" OnClick="btnHold_Click" />
                <asp:Button runat="server" ID="btnNoHold" CssClass="divIzquierdo boton" ValidationGroup="vgHold"
                    Visible="false" meta:resourcekey="btnNoHold" OnClick="btnNoHold_Click" />
            </div>
        </div>
    </div>
</asp:Content>
