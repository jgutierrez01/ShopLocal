<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="ImportaEspesores.aspx.cs" Inherits="SAM.Web.Catalogos.ImportaEspesores" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<sam:BarraTituloPagina runat="server" ID="titulo" Text="SUBIR ESPESORES" meta:resourcekey="lblSubirEspesores" NavigateUrl="~/Catalogos/TblEspesores.aspx" />

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
            <div class="divIzquierdo ancho70">
                <p class="">
                    <asp:Label Text="Archivo:" ID="lblArchivo" runat="server" meta:resourcekey="lblArchivo" CssClass="bold"></asp:Label>
                </p>
                <div style="width:33%">
                        <telerik:RadUpload ID="rdArchivo" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv" CssClass="radUpload"/> 
                </div>     
                <p>
                    <div>
                    <asp:Label CssClass="ieLeyenda" meta:resourcekey="lblFormato" runat="server"></asp:Label>
                    </div>
                    <div>
                    <asp:Label ID="ieLeyenda" CssClass="ieLeyenda" meta:resourcekey="lblEspecificacion" runat="server"></asp:Label>
                    </div>
                </p>            
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top:23px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true" DisplayMode="BulletList" HeaderText="ERRORES" CssClass="summary" meta:resourcekey="valsummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button ID="Button1" Text="Subir" runat="server" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmit" CssClass="boton" />
    </div>
</asp:Content>