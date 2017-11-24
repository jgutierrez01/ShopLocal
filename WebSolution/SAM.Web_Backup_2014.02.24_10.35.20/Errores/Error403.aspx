<%@ Page  Language="C#" MasterPageFile="~/Masters/Errores.Master" AutoEventWireup="true" CodeBehind="Error403.aspx.cs" Inherits="SAM.Web.Errores.Error403" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
<div style="margin-top: 25px;">
        <div class="divIzquierdo ancho5" style="text-align: center; padding-top: 4px;">
            <asp:Image runat="server" ID="imgHeader" ImageUrl="~/Imagenes/Iconos/bulletHeader.png" />
        </div>
        <div class="divDerecho ancho95">
            <asp:Label ID="lblError" runat="server" CssClass="tituloError" meta:resourcekey="lblError" />
            <div class="separador mensajeError">
                <asp:Literal ID="literalError" runat="server" meta:resourcekey="literalError" />
            </div>
            <asp:PlaceHolder runat="server" ID="phDetalle" Visible="false">
                <asp:Literal runat="server" ID="litMsg" />
            </asp:PlaceHolder>
            <p>
                <asp:HyperLink meta:resourcekey="hlRegresar" runat="server" ID="hlRegresar" NavigateUrl="~/WorkStatus/WkStDefault.aspx" />
            </p>
        </div>
    </div>
</asp:Content>
