<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="CrucePorImportacion.aspx.cs" Inherits="SAM.Web.Produccion.CrucePorImportacion" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>


<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina ID="titulo" runat="server" NavigateUrl="~/Produccion/LstOrdenTrabajo.aspx" meta:resourcekey="lblOdt" />

    <asp:CustomValidator
        runat="server"
        ID="cusExtensionArchivos"
        ClientValidationFunction="Sam.Administracion.ValidaArchivoCSV" 
        OnServerValidate="cusExtensionArchivos_ServerValidate"
        meta:resourcekey="cusExtensionArchivos"
        Display="None">
    </asp:CustomValidator>

    <div class="cntCentralForma">
        <div class="dashboardCentral">
        <div class="divIzquierdo ancho40">
            <p class="">
                <asp:Label runat="server" ID="lblArchivo" CssClass="bold" meta:resourcekey="lblArchivo" />
            </p>
            <div>
                <telerik:RadUpload ID="rdArchivo" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv"
                    CssClass="radUpload" ClientIDMode="Static" ControlObjectsVisibility="None" />
            </div>
            <p>
                <asp:Label ID="lblFormato" CssClass="ieLeyenda" meta:resourcekey="lblFormato" runat="server"></asp:Label>
                <br />
                <asp:Label ID="ieLeyenda" CssClass="ieLeyenda" meta:resourcekey="lblEspecificacion"
                    runat="server"></asp:Label>
            </p>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="validacionesRecuadro" style="margin-top:20px;">
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
        <asp:Button CssClass="boton" runat="server" ID="btnCruzar" OnClick="btnCruzar_Click" meta:resourcekey="btnCruzar" />
    </div>
</asp:Content>
