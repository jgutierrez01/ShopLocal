<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetDefecto.aspx.cs" Inherits="SAM.Web.Catalogos.DetDefecto" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetDefecto" NavigateUrl="~/Catalogos/LstDefectos.aspx" />

    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <asp:Label runat="server" ID="lblTipoPrueba" CssClass="bold labelHack" meta:resourcekey="lblTipoPrueba" />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoPrueba" EntityPropertyName="TipoPruebaID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvTipoPrueba" runat="server" ControlToValidate="ddlTipoPrueba"
                        InitialValue="" Display="None" meta:resourcekey="valTipoPrueba" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                        MaxLength="50" meta:resourcekey="txtNombre" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombreIngles" EntityPropertyName="NombreIngles"
                        MaxLength="50" meta:resourcekey="txtNombreIngles" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtDescripcion" EntityPropertyName="Descripcion"
                        meta:resourcekey="txtDescripcion" />
                </div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top: 23px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true"
                            DisplayMode="BulletList" HeaderText="Errores" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p>
            </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" meta:resourcekey="btnGuardar"
            OnClick="btnGuardar_OnClick" CssClass="boton" />
    </div>
</asp:Content>
