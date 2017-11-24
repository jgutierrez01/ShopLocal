<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" 
CodeBehind="ImportaPeq.aspx.cs" Inherits="SAM.Web.Catalogos.ImportaPeq" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="CntHeader" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="CntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<sam:BarraTituloPagina runat="server" ID="titulo" Text="SUBIR PULGADAS EQUIVALENTES" meta:resourcekey="lblImportaPeq"  />

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
                <div class="separador">
                    <asp:Label runat="server" ID="lblProyecto" meta:resourcekey="lblProyecto" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtProyecto" Enabled="false" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblTipoJunta" Text="Tipo de Junta:" meta:resourcekey="lblTipoJunta" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtTipoJunta" Enabled="false" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblFamAcero" Text="Familia de Acero:" meta:resourcekey="lblFamAcero" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtFamAcero" Enabled="false" />
                </div>
                <p class="">
                    <asp:Label runat="server" Text="Archivo:" ID="lblArchivo" CssClass="bold" meta:resourcekey="lblArchivo" />
                </p>
                <div style="width:33%">
                    <telerik:RadUpload ID="rdArchivo" runat="server" InitialFileInputsCount="1" AllowedFileExtensions=".csv" CssClass="radUpload" ClientIDMode="Static" /> 
                </div>
                 <p>
                    <div>
                    <asp:Label ID="Label1" CssClass="ieLeyenda" meta:resourcekey="lblFormato" runat="server"></asp:Label>
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
                        <asp:ValidationSummary runat="server" ID="valsummary" EnableClientScript="true" DisplayMode="BulletList" HeaderText="ERRORES" CssClass="summary" meta:resourcekey="valsummary" />
                    </div>
                </div>
            </div>  
            <p></p> 
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button ID="btnSubmit" Text="Subir" runat="server" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmit" CssClass="boton" />
    </div>
</asp:Content>
