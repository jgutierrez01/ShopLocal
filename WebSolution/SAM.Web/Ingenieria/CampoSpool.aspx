<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true" CodeBehind="CampoSpool.aspx.cs" Inherits="SAM.Web.Ingenieria.CampoSpool" %>
<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblCampoSpool" />

    <asp:CustomValidator
        runat="server"
        ID="cusExtensionArchivos"
        OnServerValidate="cusExtensionArchivos_ServerValidate"
        meta:resourcekey="cusExtensionArchivos"
        Display="None">
    </asp:CustomValidator>

    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho40">
                <div>
                    <p class="">
                        <asp:Label runat="server" ID="lblArchivoSpool" CssClass="bold" meta:resourcekey="lblArchivoSpool" />
                    </p>
                    <div>
                        <telerik:RadUpload ID="rdArchivoSpool" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv"
                            CssClass="radUpload" ClientIDMode="Static" ControlObjectsVisibility="None" />
                    </div>
                    <p>
                        <asp:Label ID="lblFormatoSpool" CssClass="ieLeyenda" meta:resourcekey="lblFormatoSpool" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="ieLeyenda" CssClass="ieLeyenda" meta:resourcekey="lblEspecificacionSpool"
                            runat="server"></asp:Label>
                    </p>
                </div>
                <div>
                    <p class="">
                        <asp:Label runat="server" ID="lblArchivoJuntaSpool" CssClass="bold" meta:resourcekey="lblArchivoJuntaSpool" />
                    </p>
                    <div>
                        <telerik:RadUpload ID="rdArchivoJuntaSpool" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv"
                            CssClass="radUpload" ClientIDMode="Static" ControlObjectsVisibility="None" />
                    </div>
                    <p>
                        <asp:Label ID="ieFormatoJuntaSpool" CssClass="ieLeyenda" meta:resourcekey="lblFormatoJuntaSpool" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="ieLeyendaJuntaSpool" CssClass="ieLeyenda" meta:resourcekey="lblEspecificacionJuntaSpool"
                            runat="server"></asp:Label>
                    </p>
                </div>
                <div>
                    <p class="">
                        <asp:Label runat="server" ID="lblArchivoMaterialSpool" CssClass="bold" meta:resourcekey="lblArchivoMaterialSpool" />
                    </p>
                    <div>
                        <telerik:RadUpload ID="rdArchivoMaterialSpool" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv"
                            CssClass="radUpload" ClientIDMode="Static" ControlObjectsVisibility="None" />
                    </div>
                    <p>
                        <asp:Label ID="LblFormatoMaterialSpool" CssClass="ieLeyenda" meta:resourcekey="LblFormatoMaterialSpool" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblLeyendaMaterialSpool" CssClass="ieLeyenda" meta:resourcekey="lblEspecificacionMaterialSpool"
                            runat="server"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top: 20px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton"
            OnClick="btnGuardar_OnClick" />
    </div>
</asp:Content>
