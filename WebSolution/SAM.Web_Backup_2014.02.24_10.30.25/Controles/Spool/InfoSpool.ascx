<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoSpool.ascx.cs" Inherits="SAM.Web.Controles.Spool.InfoSpool" %>
<div class="contenedorCentral">
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:RequiredLabeledTextBox ID="txtSpool" runat="server" EntityPropertyName="Nombre"
                    MaxLength="50" Enabled="false" CssClass="" meta:resourcekey="lblSpool" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox ID="txtDibujo" runat="server" EntityPropertyName="Dibujo"
                    MaxLength="50" meta:resourcekey="lblDibujo" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtExpecificacion" runat="server" EntityPropertyName="Especificacion"
                    MaxLength="50" meta:resourcekey="lblEspecificacion" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtCedula" runat="server" EntityPropertyName="Cedula" MaxLength="50"
                    meta:resourcekey="lblCedula" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtPnd" runat="server" EntityPropertyName="PND" MaxLength="50" Enabled="true"
                    meta:resourcekey="lblPnd" />
                    <asp:RangeValidator  ID="rvPnd" runat="server"  ControlToValidate="txtPnd" MinimumValue="0" MaximumValue="100" meta:resourcekey="rvPnd"  Display="None" Type="Integer" >
                    </asp:RangeValidator>
            </div>
            <div class="separador">
                <asp:Label ID="lblTipoAcero1" runat="server" meta:resourcekey="lblTipoAcero1" CssClass="bold" /><br />
                <mimo:MappableDropDown runat="server" ID="ddlTipoAcero1" EntityPropertyName="FamiliaAcerolID" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rfvTipoAcero1" ControlToValidate="ddlTipoAcero1"
                    InitialValue="" Display="None" meta:resourcekey="rfvTipoAcero1" />
            </div>
            <div class="separador">
                <asp:Label ID="lblTipoAcero2" runat="server" meta:resourcekey="lblTipoAcero2" CssClass="bold" /><br />
                <mimo:MappableDropDown runat="server" ID="ddlTipoAcero2" EntityPropertyName="FamiliaAcero2ID" />
                <%--<asp:DropDownList runat="server" ID ="ddlTipoAcero2" />--%>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:LabeledTextBox ID="txtPeso" runat="server" EntityPropertyName="Peso" MaxLength="50"
                    meta:resourcekey="lblPeso" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtArea" runat="server" EntityPropertyName="Area" MaxLength="50"
                    meta:resourcekey="lblArea" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtRevision" runat="server" EntityPropertyName="Revision"
                    MaxLength="50" meta:resourcekey="lblRevision" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtRevisionCliente" runat="server" EntityPropertyName="RevisionCliente"
                    MaxLength="50" meta:resourcekey="lblRevisionCliente" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtPrioridad" runat="server" EntityPropertyName="Prioridad"
                    MaxLength="50" meta:resourcekey="lblPrioridad" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtPdi" runat="server" EntityPropertyName="Pdis" MaxLength="50"
                    meta:resourcekey="lblPdi" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox  ID="txtDiametroMayor" runat="server" EntityPropertyName="DiametroMayor" MaxLength="50"
                     meta:resourcekey="textDiametroMayor" Enabled="false" />
            </div>
        </div>
        <p>
        </p>
        <div class="divIzquierdo ancho100">
            <div class="divIzquierdo ancho50">
                <p>
                    <asp:CheckBox ID="chkRequierePwht" runat="server" Enabled="true" />
                    <asp:Label ID="lblRequierePwht" runat="server" CssClass="bold" meta:resourcekey="lblRequierePwht" />
                </p>
                <p>
                    <asp:CheckBox ID="chkPendienteDocumental" runat="server" />
                    <asp:Label ID="lblPendienteDocumental" runat="server" CssClass="bold" meta:resourcekey="lblPendienteDocumental" />
                </p>
                <p>
                    <asp:CheckBox ID="chkLiberadoParaCruce" runat="server" />
                    <asp:Label ID="lblLiberadoParaCruce" runat="server" CssClass="bold" meta:resourcekey="lblLiberadoParaCruce" />
                </p>
            </div>
            <div class="divIzquierdo ancho50">
                <p>
                    <asp:CheckBox ID="chkHoldCalidad" runat="server" Enabled="false" />
                    <asp:Label ID="lblHoldCalidad" runat="server" CssClass="bold" meta:resourcekey="lblHoldCalidad" />
                </p>
                <p>
                    <asp:CheckBox ID="chkHoldIng" runat="server" Enabled="false" />
                    <asp:Label ID="lblHoldIng" runat="server" CssClass="bold" meta:resourcekey="lblHoldIng" />
                </p>
                <p>
                    <asp:CheckBox ID="chkConfinado" runat="server" Enabled="false" />
                    <asp:Label ID="lblConfinado" runat="server" CssClass="bold" meta:resourcekey="lblConfinado" />
                </p>
            </div>
        </div>
        <p>
        </p>
        <h5>
            <asp:Literal runat="server" ID="litNomenclatura" meta:resourcekey="litNomenclatura"></asp:Literal></h5>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento1" runat="server" EntityPropertyName="Segmento1"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento1" Visible="false" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento2" runat="server" EntityPropertyName="Segmento2"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento2" Visible="false" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento3" runat="server" EntityPropertyName="Segmento3"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento3" Visible="false" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento4" runat="server" EntityPropertyName="Segmento4"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento4" Visible="false" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento5" runat="server" EntityPropertyName="Segmento5"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento5" Visible="false" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento6" runat="server" EntityPropertyName="Segmento6"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento6" Visible="false" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtSegmento7" runat="server" EntityPropertyName="Segmento7"
                    MaxLength="50" Enabled="false" meta:resourcekey="txtSegmento7" Visible="false" />
            </div>
        </div>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro" style="margin-top: 23px;">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary"
                    meta:resourcekey="valSummary" />
            </div>
        </div>
    </div>
    <%--  No borrar
    <div class="divIzquierdo ancho30">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        
       
    </div>--%>
    <%--<div class="divIzquierdo ancho30">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
       
       
    </div>--%>
    <%--<div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label ID="lblNomenclatura" runat="server" CssClass="labelHack" meta:resourcekey="lblNomenclatura" />
        </div>
    </div>--%>
</div>
