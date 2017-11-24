<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="DetColadas.aspx.cs" Inherits="SAM.Web.Proyectos.DetColadas" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetColada" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho30">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox EntityPropertyName="NumeroColada" runat="server" ID="txtNumColada" meta:resourcekey="txtNumColada" MaxLength="20" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblFabricante" meta:resourcekey="lblFabricante" AssociatedControlID="ddlFabricante" />
                    <mimo:MappableDropDown EntityPropertyName="FabricanteID" runat="server" ID="ddlFabricante" meta:resourcekey="ddlFabricante" />
                    <asp:RequiredFieldValidator meta:resourcekey="reqFabricante" runat="server" ID="reqFabricante" ControlToValidate="ddlFabricante" Display="None" />
                    <span class="required">*</span>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="NumeroCertificado" runat="server" ID="txtNumCertificado" meta:resourcekey="txtNumCertificado" MaxLength="20" />
                </div>
                <div class="separador listaCheck">
                    <mimo:MappableCheckBox EntityPropertyName="HoldCalidad" runat="server" ID="chkHoldCalidad" meta:resourcekey="chkHoldCalidad" />
                </div>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="separador">
                    <asp:Label runat="server" ID="lblAcero" meta:resourcekey="lblAcero" AssociatedControlID="ddlAcero" />
                    <mimo:MappableDropDown EntityPropertyName="AceroID" runat="server" ID="ddlAcero" meta:resourcekey="ddlAcero" OnSelectedIndexChanged="ddlAcero_OnSelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator meta:resourcekey="reqAcero" runat="server" ID="reqAcero" ControlToValidate="ddlAcero" Display="None" />
                    <span class="required">*</span>
                </div>
                <div class="cajaAzul" style="width:194px;">
                    <div class="soloLectura textosChicos">
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtFamAcero" meta:resourcekey="txtFamAcero" ReadOnly="true" />
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtFamMaterial" meta:resourcekey="txtFamMaterial" ReadOnly="true" />
                        </div>
                    </div>
                </div>            
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" CssClass="boton" meta:resourcekey="btnGuardar"/>    
    </div>
</asp:Content>
