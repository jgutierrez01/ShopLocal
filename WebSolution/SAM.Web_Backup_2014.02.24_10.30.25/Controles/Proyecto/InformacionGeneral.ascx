<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformacionGeneral.ascx.cs" Inherits="SAM.Web.Controles.Proyecto.InformacionGeneral" %>
<%@ Register Src="~/Controles/Proyecto/SeleccionadorColor.ascx" TagName="SeleccionadorColor" TagPrefix="ctrl" %>
<div class="divIzquierdo ancho70">
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre" MaxLength="50" meta:resourcekey="txtNombre" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtDescripcion" MaxLength="150" meta:resourcekey="txtDescripcion" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblCliente" CssClass="bold" meta:resourcekey="lblCliente" AssociatedControlID="ddlCliente" />
            <mimo:MappableDropDown runat="server" ID="ddlCliente" EntityPropertyName="ClienteID" CssClass="required" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator meta:resourcekey="reqCliente" runat="server" ID="reqCliente" ControlToValidate="ddlCliente" ErrorMessage="Se requiere seleccionar un cliente" Display="None" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblFechaInicial" CssClass="bold" meta:resourcekey="lblFechaInicial" AssociatedControlID="dtpFechaInicial" />
            <mimo:MappableDatePicker runat="server" ID="dtpFechaInicial" MinDate="01/01/1960" MaxDate="01/01/2050" EnableEmbeddedSkins="false" Width="230" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblPatio" CssClass="bold" meta:resourcekey="lblPatio" AssociatedControlID="ddlPatio" />
            <mimo:MappableDropDown runat="server" ID="ddlPatio" EntityPropertyName="Nombre" CssClass="required" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator meta:resourcekey="reqPatio" runat="server" ID="reqPatio" ControlToValidate="ddlPatio" Display="None" />
        </div>
    </div>
    <div class="divIzquierdo ancho45">
        <div class="separador">
            <asp:Label runat="server" ID="lblColor" meta:resourcekey="lblColor" CssClass="labelHack bold" />
            <ctrl:SeleccionadorColor ID="picker" runat="server" EsRequerido="true" />
        </div>
        <div class="separador listaCheck checkBold">
            <mimo:MappableCheckBox runat="server" ID="chkStatus" Checked="true" meta:resourcekey="chkStatus" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtCodigoNumUnico" meta:resourcekey="txtCodigoNumUnico" CssClass="required" MaxLength="10" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtCodigoOrdTrabajo" meta:resourcekey="txtCodigoOrdTrabajo" CssClass="required" MaxLength="10" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtDigitosNumUnico" meta:resourcekey="txtDigitosNumUnico" CssClass="required" MaxLength="1" />
            <asp:RangeValidator meta:resourcekey="rngDigitosNu" runat="server" ID="rngDigitosNu" MinimumValue="1" MaximumValue="5" ControlToValidate="txtDigitosNumUnico" Type="Integer" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtDigitosOrdTrabajo" meta:resourcekey="txtDigitosOrdTrabajo" CssClass="required" MaxLength="1" />
            <asp:RangeValidator meta:resourcekey="rngDigitosOdt" runat="server" ID="rngDigitosOdt" MinimumValue="1" MaximumValue="5" ControlToValidate="txtDigitosOrdTrabajo" Type="Integer" />
        </div>
    </div>
</div>
<div class="divIzquierdo ancho30">
    <div class="validacionesRecuadro">
        <div class="validacionesHeader"></div>
        <div class="validacionesMain">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
        </div>
    </div>
</div>
