<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="DetPerfiles.aspx.cs" Inherits="SAM.Web.Administracion.DetPerfiles" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" NavigateUrl="~/Administracion/LstPerfiles.aspx" />
    <div class="cntCentralForma">
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <mimo:RequiredLabeledTextBox    meta:resourcekey="txtNombre"
                                                runat="server"
                                                ID="txtNombre"
                                                EntityPropertyName="Nombre"
                                                MaxLength="50"
                                                TextMode="SingleLine" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox    meta:resourcekey="txtNombreIng"
                                                runat="server"
                                                ID="txtNombreIng"
                                                EntityPropertyName="NombreIngles"
                                                MaxLength="50"
                                                TextMode="SingleLine" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox    meta:resourcekey="txtDescripcion"
                                        runat="server" 
                                        ID="txtDescripcion" 
                                        EntityPropertyName="Descripcion" 
                                        MaxLength="500" />
            </div>
            <p></p>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="validacionesRecuadro" style="margin-top:23px;">
                <div class="validacionesHeader">&nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
            <p></p>
        </div>
        <div class="clear ancho90">
            <h5>
                <asp:Label meta:resourcekey="lblPermisos" runat="server" ID="lblPermisos" />
            </h5>
            <asp:Repeater runat="server" ID="repModulos" OnItemDataBound="repModulos_OnItemDataBound">
                <ItemTemplate>
                    <div class="modulo">
                        <div class="titulo">
                            <asp:Label runat="server" ID="lblNombre" />
                            <asp:CheckBox runat="server" ID="chkTitulo" onclick="Sam.Administracion.ToggleLista(this);" />
                        </div>
                        <div class="lista">
                            <asp:CheckBoxList runat="server" ID="chkLst" RepeatLayout="Table" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <p></p>
    </div>
    <div class="pestanaBoton">
        <asp:Button meta:resourcekey="btnGuardar" CssClass="boton" runat="server" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
    </div>
</asp:Content>
