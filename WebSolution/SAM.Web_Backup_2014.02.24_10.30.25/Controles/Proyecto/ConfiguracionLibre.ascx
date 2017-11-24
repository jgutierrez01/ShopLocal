<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionLibre.ascx.cs"
            Inherits="SAM.Web.Controles.Proyecto.ConfiguracionLibre" %>

<div class="divIzquierdo ancho70">
    <div class="divIzquierdo ancho50">
        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblRecepcion" meta:resourcekey="lblRecepcion" />
        </h4>
        <div class="separador">
            <asp:Label meta:resourcekey="lblNumRecepciones" runat="server" ID="lblNumRecepciones"
                       AssociatedControlID="ddlNumCamposRecepcion" />
            <mimo:MappableDropDown runat="server" ID="ddlNumCamposRecepcion" Width="60" AutoPostBack="true"
                OnSelectedIndexChanged="ddlNumCamposRecepcion_SelectedIndexChanged">
                <asp:ListItem Value="0" Text="" Selected="True" />
                <asp:ListItem Value="1" Text="1" />
                <asp:ListItem Value="2" Text="2" />
                <asp:ListItem Value="3" Text="3" />
                <asp:ListItem Value="4" Text="4" />
                <asp:ListItem Value="5" Text="5" />
            </mimo:MappableDropDown>
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoRecepcion1" runat="server" ID="txtCampoRecepcion1"
                MaxLength="100" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoRecepcion2" runat="server" ID="txtCampoRecepcion2"
                MaxLength="100" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoRecepcion3" runat="server" ID="txtCampoRecepcion3"
                MaxLength="100" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoRecepcion4" runat="server" ID="txtCampoRecepcion4"
                MaxLength="100" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoRecepcion5" runat="server" ID="txtCampoRecepcion5"
                MaxLength="100" Enabled="false" CssClass="" />
        </div>
    </div>
    <div class="divIzquierdo ancho45">
        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblNumeroUnico" meta:resourcekey="lblNumeroUnico" />
        </h4>
        <div class="separador">
            <asp:Label meta:resourcekey="lblNumNumerosUnicos" runat="server" ID="lblNumNumerosUnicos"
                       AssociatedControlID="ddlNumCamposNumeroUnico" />
            <mimo:MappableDropDown runat="server" ID="ddlNumCamposNumeroUnico" Width="60" AutoPostBack="true"
                OnSelectedIndexChanged="ddlNumCamposNumeroUnico_SelectedIndexChanged">
                <asp:ListItem Value="0" Text="" Selected="True" />
                <asp:ListItem Value="1" Text="1" />
                <asp:ListItem Value="2" Text="2" />
                <asp:ListItem Value="3" Text="3" />
                <asp:ListItem Value="4" Text="4" />
                <asp:ListItem Value="5" Text="5" />
            </mimo:MappableDropDown>
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoNumeroUnico1" runat="server" ID="txtCampoNumeroUnico1"
                MaxLength="20" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoNumeroUnico2" runat="server" ID="txtCampoNumeroUnico2"
                 MaxLength="20" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoNumeroUnico3" runat="server" ID="txtCampoNumeroUnico3"
                MaxLength="20" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoNumeroUnico4" runat="server" ID="txtCampoNumeroUnico4"
                MaxLength="20" Enabled="false" CssClass="" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtCampoNumeroUnico5" runat="server" ID="txtCampoNumeroUnico5"
                MaxLength="20" Enabled="false" CssClass="" />
        </div>
    </div>
</div>
<div class="divIzquierdo ancho30">
    <div class="validacionesRecuadro">
        <div class="validacionesHeader">
        </div>
        <div class="validacionesMain">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
        </div>
    </div>
</div>
<p>
</p>
