<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Configuracion.ascx.cs"
    Inherits="SAM.Web.Controles.Proyecto.Configuracion" %>
<div class="divIzquierdo ancho70">
    <div class="divIzquierdo ancho50">
        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblNomenclaturaSpool" meta:resourcekey="lblNomenclaturaSpool" /></h4>
        <div class="separador">
            <asp:Label meta:resourcekey="lblNumSegmentos" runat="server" ID="lblNumSegmentos"
                AssociatedControlID="ddlNumSegmentos" />
            <mimo:MappableDropDown runat="server" ID="ddlNumSegmentos" Width="60" AutoPostBack="true"
                OnSelectedIndexChanged="ddlNumSegmentos_SelectedIndexChanged">
                <asp:ListItem Value="0" Text="" Selected="True" />
                <asp:ListItem Value="1" Text="1" />
                <asp:ListItem Value="2" Text="2" />
                <asp:ListItem Value="3" Text="3" />
                <asp:ListItem Value="4" Text="4" />
                <asp:ListItem Value="5" Text="5" />
                <asp:ListItem Value="6" Text="6" />
                <asp:ListItem Value="7" Text="7" />
            </mimo:MappableDropDown>
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento1" runat="server" ID="txtSegmento1"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento2" runat="server" ID="txtSegmento2"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento3" runat="server" ID="txtSegmento3"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento4" runat="server" ID="txtSegmento4"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento5" runat="server" ID="txtSegmento5"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento6" runat="server" ID="txtSegmento6"
                MaxLength="20" Enabled="false" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox meta:resourcekey="txtSegmento7" runat="server" ID="txtSegmento7"
                Label="7" MaxLength="20" Enabled="false" />
        </div>
    </div>
    <div class="divIzquierdo ancho45">
        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblRestricciones" meta:resourcekey="lblRestricciones" /></h4>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtTolerancia" MaxLength="5" meta:resourcekey="txtTolerancia" />
            <asp:RangeValidator meta:resourcekey="rngTolerancia" runat="server" ID="rngTolerancia"
                Type="Integer" MinimumValue="0" MaximumValue="999" ControlToValidate="txtTolerancia"
                Display="None" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtAngulo" MaxLength="20" meta:resourcekey="txtAngulo" />
        </div>
        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblDestajos" meta:resourcekey="lblDestajos" /></h4>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtCostoCuadroArmado" MaxLength="12"
                meta:resourcekey="txtCostoCuadroArmado" />
            <asp:RangeValidator runat="server" ID="rvCostoCuadroArmado" ControlToValidate="txtCostoCuadroArmado"
                Display="None" MinimumValue="0" MaximumValue="9999999.99" Type="Double" meta:resourcekey="rvCostoCuadroArmado"/>
        </div>
         <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtCostoCuadroRaiz" MaxLength="12"
                meta:resourcekey="txtCostoCuadroRaiz" />
            <asp:RangeValidator runat="server" ID="rvCostoCuadroRaiz" ControlToValidate="txtCostoCuadroRaiz"
                Display="None" MinimumValue="0" MaximumValue="9999999.99" Type="Double" meta:resourcekey="rvCostoCuadroRaiz"/>
        </div>
         <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtCostoCuadroRelleno" MaxLength="12"
                meta:resourcekey="txtCostoCuadroRelleno" />
            <asp:RangeValidator runat="server" ID="rvCostoCuadroRelleno" ControlToValidate="txtCostoCuadroRelleno"
                Display="None" MinimumValue="0" MaximumValue="9999999.99" Type="Double" meta:resourcekey="rvCostoCuadroRelleno"/>
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
