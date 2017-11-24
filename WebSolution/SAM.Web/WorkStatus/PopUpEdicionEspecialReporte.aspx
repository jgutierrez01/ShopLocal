<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpEdicionEspecialReporte.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpEdicionEspecialReporte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="userAgentDependant" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <asp:PlaceHolder runat="server" ID="phControles">
        <div style="width: 500px;">
            <h4>
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>                                
            <div class="divIzquierdo ancho45">
                <asp:HiddenField ID="hdnProyectoID" runat="server" />

                <div class="separador">
                    <asp:Label ID="lblNumeroReporte" runat="server" meta:resourcekey="lblNumeroReporte" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtNumeroReporte" runat="server"></asp:TextBox>
                    <asp:Label ID="reqNumeroReporte" runat="server" Text="*" CssClass="required"></asp:Label>
                    <asp:RequiredFieldValidator ID="valNumeroReporte" runat="server" ControlToValidate="txtNumeroReporte" Display="None"
                         meta:resourcekey="valNumeroReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte" CssClass="bold"></asp:Label>
                    <br />&nbsp;
                    <telerik:RadDatePicker ID="rdpFechaReporte" runat="server" EnableEmbeddedSkins="false"></telerik:RadDatePicker>
                    <asp:Label ID="reqFechaReporte" runat="server" Text="*" CssClass="required"></asp:Label>
                    <asp:RequiredFieldValidator ID="valFechaReporte" runat="server" Display="None" ControlToValidate="rdpFechaReporte"
                         meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Button ID="btnGuardar" runat="server" CssClass="boton" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" />
                </div>
            </div>
            <div class="divIzquierdo ancho50">
                <div class="validacionesRecuadro" style="margin-top: 20px;">
                    <div class="validacionesHeader">
                        &nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary"
                            meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
