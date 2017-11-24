<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="ImportaDestajos.aspx.cs" Inherits="SAM.Web.Administracion.ImportaDestajos" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="encabezadoProyecto" TagPrefix="ctrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
  <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<sam:BarraTituloPagina runat="server" ID="titulo" Text="SUBIR TABLA DE DESTAJO" meta:resourcekey="lblImportaPeq" NavigateUrl="~/Administracion/TblDestajos.aspx" />

    <asp:CustomValidator
        runat="server"
        ID="cusExtensionArchivos"
        ClienteValidationFunction="Sam.Administracion.ValidaArchivoCSV"
        OnServerValidate="cusExtensionArchivos_OnServerValidate"
        meta:resourcekey="cusExtensionArchivos"
        Display="None">
    </asp:CustomValidator>

    <div class="cntCentralForma">
        <ctrl:encabezadoProyecto ID="proyEncabezado" runat="server" Visible="true" />
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFamAcero" Text="Familia de Acero:" meta:resourcekey="lblFamAcero" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtFamAcero" Enabled="false" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblTipoJunta" Text="Tipo de Junta:" meta:resourcekey="lblTipoJunta" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtTipoJunta" Enabled="false" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblProceso" Text="Proceso:" meta:resourcekey="lblProceso" CssClass="bold labelHack" />
                    <asp:TextBox runat="server" ID="txtProceso" Enabled="false" />
                </div>
                <p class="">
                    <asp:Label runat="server" Text="Archivo:" ID="lblArchivo" CssClass="bold" meta:resourcekey="lblArchivo" />
                </p>
                <div style="width:33%">
                    <telerik:RadUpload runat="server" ID="rdArchivo" InitialFileInputsCount="1" AllowedFileExtensions=".csv" CssClass="radUpload" ClientIDMode="Static" />
                </div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top:11px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnSubmit" Text="Subir" OnClick="btnSubmit_OnClick" CssClasS="boton" meta:resourcekey="btnSubmit" />
    </div>
</asp:Content>
