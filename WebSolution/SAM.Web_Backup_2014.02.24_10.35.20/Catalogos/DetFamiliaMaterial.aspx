﻿<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="DetFamiliaMaterial.aspx.cs" Inherits="SAM.Web.Catalogos.DetFamiliaMaterial" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstFamiliaMaterial.aspx"
        meta:resourcekey="lblDetFamiliaMaterial" />
    <div class="cntCentralForma">
    <div class="dashboardCentral">
        <div class="divIzquierdo ancho70">
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                    MaxLength="50" meta:resourcekey="lblNombre" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtDescripcion" EntityPropertyName="Descripcion"
                    MaxLength="500" meta:resourcekey="lblDescripcion" />
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
        <p>
        </p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            CssClass="boton" meta:resourcekey="btnGuardar" />
    </div>
</asp:Content>
