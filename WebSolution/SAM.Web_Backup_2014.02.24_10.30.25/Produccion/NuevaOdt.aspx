<%@ Page  Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="NuevaOdt.aspx.cs" Inherits="SAM.Web.Produccion.NuevaOdt" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phTaller" />
                    <telerik:AjaxUpdatedControl ControlID="txtOdt" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
    <sam:BarraTituloPagina ID="titulo" runat="server" NavigateUrl="~/Produccion/LstOrdenTrabajo.aspx" meta:resourcekey="lblOdt" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
        <div class="divIzquierdo ancho60" style="margin-right:20px;">
            <div class="separador">
                <asp:Label meta:resourcekey="lblProyecto" runat="server" ID="lblProyecto" AssociatedControlID="ddlProyecto" />
                <mimo:MappableDropDown meta:resourcekey="ddlProyecto" runat="server" ID="ddlProyecto" AutoPostBack="True" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator meta:resourcekey="reqProyecto" runat="server" ID="reqProyecto" ControlToValidate="ddlProyecto" Display="None" />
            </div>
            <div class="separador">
                <asp:PlaceHolder runat="server" ID="phTaller">
                    <asp:Label meta:resourcekey="lblTaller" runat="server" ID="lblTaller" AssociatedControlID="ddlTaller" />
                    <mimo:MappableDropDown meta:resourcekey="ddlTaller" runat="server" ID="ddlTaller" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqTaller" runat="server" ID="reqTaller" ControlToValidate="ddlTaller" Display="None" />
                </asp:PlaceHolder>
            </div>
            <div class="separador">
                <asp:Label meta:resourcekey="lblFecha" runat="server" ID="lblFecha" AssociatedControlID="dtpFecha" />
                <mimo:MappableDatePicker runat="server" ID="dtpFecha" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator meta:resourcekey="reqFecha" runat="server" ID="reqFecha" ControlToValidate="dtpFecha" Display="None" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" meta:resourcekey="txtOdt" ID="txtOdt" MaxLength="5" />
                <asp:RangeValidator Display="None" runat="server" ID="rngOdt" ControlToValidate="txtOdt" MinimumValue="1" MaximumValue="99999" Type="Integer" meta:resourcekey="rngOdt" />
            </div>
        </div>
        <div class="divDerecho ancho35">
            <div class="validacionesRecuadro" style="width:250px; margin-top:20px;">
                <div class="validacionesHeader">&nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p></p>
        </div>
    </div>
    <div class="pestanaBotonMasLarga">
        <asp:Button CssClass="boton" runat="server" ID="btnCruceForzado" OnClick="btnCruceForzado_Click" meta:resourcekey="btnCruceForzado" />
        <asp:Button CssClass="boton" runat="server" ID="btnCruceForzadoAsignado" OnClick="btnCruceForzadoAsignado_Click" meta:resourcekey="btnCruceForzadoAsignado" />
        <asp:Button CssClass="boton" runat="server" ID="btnCruceForzadoCsv" OnClick="brnCruceForzadoCsv_Click" meta:resourcekey="btnCruceForzadoCsv" CausesValidation="false" />
    </div>
</asp:Content>
