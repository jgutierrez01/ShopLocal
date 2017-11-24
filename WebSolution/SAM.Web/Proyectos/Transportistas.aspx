<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="Transportistas.aspx.cs" Inherits="SAM.Web.Proyectos.Transportistas" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTransportistasProyecto" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="listaCheck clear ancho90" style="padding-top:15px;">
                <asp:CheckBoxList runat="server" ID="chkTransportistas" CssClass="cajaAzul" RepeatColumns="3" Width="100%" />
            </div>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_OnClick" />    
    </div>
</asp:Content>
