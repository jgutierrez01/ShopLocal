<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoGeneral.ascx.cs" Inherits="SAM.Web.Controles.Workstatus.Soldadura.InfoGeneral" %>
<div style="width: 735;">
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50 boldElements">
            <p>
                <asp:Label runat="server" ID="ltlNumControl" meta:resourcekey="ltlNumControl" />
                <asp:Literal runat="server" ID="NumControl" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlLocalizacion" meta:resourcekey="ltlLocalizacion" />
                <asp:Literal runat="server" ID="Localizacion" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlTipo" meta:resourcekey="ltlTipo" />
                <asp:Literal runat="server" ID="Tipo" />
            </p>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaSoldadura" meta:resourcekey="lblFechaSoldadura" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaSoldadura" runat="server" Style="width: 209px"        />                
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="valFechaSoldadura" ValidationGroup="valGuardar"
                    Display="None" meta:resourcekey="valFechaSoldadura" ControlToValidate="mdpFechaSoldadura" />
               
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtMaterial1" meta:resourcekey="txtMaterial1"
                    ReadOnly="true" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblTaller" meta:resourcekey="lblTaller" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlTaller" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="valTaller" ValidationGroup="valGuardar"
                    InitialValue="" Display="None" meta:resourcekey="valTaller" ControlToValidate="ddlTaller" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                    TextMode="MultiLine" Rows="3" MaxLength="500">
                </mimo:LabeledTextBox>
            </div>
            <p>
            </p>
        </div>
        <div class="divDerecho ancho50 boldElements">
            <p>
                <asp:Label runat="server" ID="lblNombreSpool" meta:resourcekey="ltlNombreSpool" />
                <asp:Literal runat="server" ID="NombreSpool" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlJunta" meta:resourcekey="ltlJunta" />
                <asp:Literal runat="server" ID="Junta" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlCedula" meta:resourcekey="ltlCedula" />
                <asp:Literal runat="server" ID="Cedula" />
            </p>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" />
                <br />
                <asp:HiddenField runat="server" ID="hdnFechaSoldadura" />
                <mimo:MappableDatePicker runat="server" AutoPostBack="true"  ID="mdpFechaReporte" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="valFechaReporte" ValidationGroup="valGuardar"
                    Display="None" meta:resourcekey="valFechaReporte" ControlToValidate="mdpFechaReporte" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtMaterial2" meta:resourcekey="txtMaterial2"
                    ReadOnly="true" />
            </div>
            <div class="" style="margin-top: 7px;">
                <asp:CheckBox runat="server" ID="chkWpsDiferentes" Checked="false" OnCheckedChanged="chkWpsDiferentes_OnCheckedChanged" AutoPostBack="true" CssClass="divIzquierdo"/>
                <asp:Label ID="lblChkDiferentes" meta:resourcekey="chkWpsDiferentes" runat="server"  CssClass="divIzquierdo"/>
                <br />                
            </div>
            <div class="" style="margin-top: 7px;" >
                <asp:CheckBox ID="chkTerminadoConRaiz" Checked="false" OnCheckedChanged="chkTerminadoConRaiz_CheckedChanged" runat="server" AutoPostBack="true" CssClass="divIzquierdo" />
                <asp:Label ID="lblTermiadoConRaiz" meta:resourcekey="lblTerminadoConRaiz" runat="server" CssClass="divIzquierdo" />
                <br />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblWps" meta:resourcekey="lblWps" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlWps" OnSelectedIndexChanged="ddlWps_SelectedIndexChanged"
                    AutoPostBack="true" Enabled="false"/>
                <%--<span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rvWps" ValidationGroup="valGuardar"
                    Display="None" InitialValue="" meta:resourcekey="valWps" ControlToValidate="ddlWps" />--%>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblWpsRelleno" meta:resourcekey="lblWpsRelleno" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlWpsRelleno" OnSelectedIndexChanged="ddlWpsRelleno_SelectedIndexChanged"
                    AutoPostBack="true" Enabled="false"/>
                <%--<span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rvWpsRelleno" ValidationGroup="valGuardar"
                    Display="None" InitialValue="" meta:resourcekey="valWpsRelleno" ControlToValidate="ddlWps" />--%>
            </div>
            <asp:Panel ID="pnlLeyendas" runat="server" CssClass="separador" Visible="false">
                <%--<asp:CheckBox ID="chkPWHT" runat="server" CssClass="checkBold" Enabled="false" />--%>
                <asp:Label runat="server" ID="lblSiPwht" CssClass="checkBold" Visible="false" meta:resourcekey="lblSiPWHT" />
                <asp:Label runat="server" ID="lblNoPwht" CssClass="checkBold" Visible="false" meta:resourcekey="lblNoPwht" />
            <br />
                <%--<asp:CheckBox ID="chkPreheat" runat="server" CssClass="checkBold" Enabled="false" />--%>
                <asp:Label runat="server" ID="lblSiPreheat" CssClass="checkBold" Visible="false" meta:resourcekey="lblSiPreheat" />
                <asp:Label runat="server" ID="lblNoPreheat" CssClass="checkBold" Visible="false" meta:resourcekey="lblNoPreheat" />
            </asp:Panel>
        </div>
        <input type="hidden" runat="server" id="hdnProyectoID" />
        <p>
        </p>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ValidationGroup="valGuardar" ID="valSummary"
                    CssClass="summary" meta:resourcekey="valSummary" Width="120" />
            </div>
        </div>
    </div>
    <p>
    </p>
</div>
