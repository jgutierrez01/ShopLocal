<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="DetItemCodes.aspx.cs" Inherits="SAM.Web.Proyectos.DetItemCodes" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetItemCodes" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho40">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox EntityPropertyName="Codigo" runat="server" ID="txtItemCode" meta:resourcekey="txtItemCode" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="ItemCodeCliente" runat="server" ID="txtItemCodeCliente" meta:resourcekey="txtItemCodeCliente" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox EntityPropertyName="DescripcionEspanol" runat="server" ID="txtDescripcionEsp" meta:resourcekey="txtDescripcionEsp" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="DescripcionIngles" runat="server" ID="txtDescripcionIng" meta:resourcekey="txtDescripcionIng" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblClasificacion" meta:resourcekey="lblClasificacion" AssociatedControlID="ddlClasificacion" />
                    <mimo:MappableDropDown EntityPropertyName="TipoMaterialID" runat="server" ID="ddlClasificacion" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="reqClasificacion" meta:resourcekey="reqClasificacion" ControlToValidate="ddlClasificacion" Display="None" />
               </div>
               <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="Peso" runat="server" ID="lblPeso" meta:resourcekey="lblPeso" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="DescripcionInterna" runat="server" ID="lblDescripcionInterna" meta:resourcekey="lblDescripcionInterna" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="Diametro1" runat="server" ID="lblDiametro1" meta:resourcekey="lblDiametro1" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox EntityPropertyName="Diametro2" runat="server" ID="lblDiametro2" meta:resourcekey="lblDiametro2" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblFamAcero" meta:resourcekey="lblFamAcero" AssociatedControlID="ddlFamiliaAcero" />
                    <mimo:MappableDropDown EmptyIsNull="true" EntityPropertyName="FamiliaAceroID" runat="server" ID="ddlFamiliaAcero" meta:resourcekey="ddlFamiliaAcero" />
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
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_OnClick" />
    </div>
</asp:Content>
