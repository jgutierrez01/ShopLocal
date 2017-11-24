<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Soldador.ascx.cs" Inherits="SAM.Web.Controles.Soldadores.Soldador" %>
<div class="dashboardCentral">
    <div class="divIzquierdo ancho70">
        <div class="separador">
            <mimo:RequiredLabeledTextBox ID="txtCodigo" runat="server" EntityPropertyName="Codigo"
                MaxLength="15" meta:resourcekey="lblCodigo" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox ID="txtNombre" runat="server" EntityPropertyName="Nombre"
                MaxLength="50" meta:resourcekey="lblNombre" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox ID="txtApPaterno" runat="server" EntityPropertyName="Paterno"
                MaxLength="50" meta:resourcekey="lblPaterno" />
        </div>
        <div class="separador">
        <mimo:LabeledTextBox runat="server" ID="txtApMaterno" EntityPropertyName="Materno"
                MaxLength="50" meta:resourcekey="lblMaterno" />
            <%--<mimo:RequiredLabeledTextBox ID="txtApMaterno" runat="server" EntityPropertyName="Materno"
                MaxLength="50" meta:resourcekey="lblMaterno" />--%>
        </div>
        <div class="separador">
            <asp:Label ID="lblPatio" runat="server" CssClass="bold" meta:resourcekey="lblPatio" />
            <br />
            <mimo:MappableDropDown EntityPropertyName="PatioID" ID="ddlPatios" OnSelectedIndexChanged="ddlPatios_SelectedIndexChanged" AutoPostBack="true" runat="server" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvPatio" ControlToValidate="ddlPatios"
                InitialValue="" Display="None" meta:resourcekey="rfvPatio" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtNumEmpleado" EntityPropertyName="NumeroEmpleado"
                MaxLength="15" meta:resourcekey="lblEmpleado" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblActivo" CssClass="bold inline" meta:resourcekey="lblActivo" />
            <mimo:MappableCheckBox runat="server" ID="chkActivo" Checked="false" CausesValidation="false"
                EntityPropertyName="Activo" />
        </div>
        <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtAreaTrabajo" EntityPropertyName="AreaTrabajo"
                    MaxLength="50" meta:resourcekey="lblAreaTrabajo" />
            </div>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro" style="margin-top: 23px;">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true"
                    DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
            </div>
        </div>
    </div>
</div>
