<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PerfilUsuario.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.PerfilUsuario" %>

<div class="recuadroUsuario texto">
    <p style="height:3px; min-height:3px; max-height:3px; margin:0px;"></p>
    <asp:Label ID="lblBienvenido" runat="server" meta:resourcekey="lblBienvenido"></asp:Label>,&nbsp;<u><asp:Label ID="lblUsuario" runat="server"/></u>
    <asp:ImageButton CausesValidation="false" ImageUrl="~/Imagenes/Iconos/mexico.png" ID="btnEspanol" OnClick="Espanol_Click" runat="server" ToolTip="MEX"/>
    <asp:ImageButton CausesValidation="false" ImageUrl="~/Imagenes/Iconos/us.png" ID="btnIngles" OnClick="Ingles_Click" runat="server" ToolTip="US"/>
    <asp:HyperLink runat="server" ID="hlAyuda" ImageUrl="~/Imagenes/Iconos/help.png" meta:resourcekey="hlAyuda" ClientIDMode="Static" />
    <asp:HyperLink runat="server" ID="perfil_hlSalir" ImageUrl="~/Imagenes/Iconos/salir.png" meta:resourcekey="hlSalir" ClientIDMode="Static" />
    <telerik:RadContextMenu runat="server" ID="menuUsuario">
        <Items>
            <telerik:RadMenuItem Text="Cambiar contraseña" meta:resourcekey="itemContraseña"/>
            <telerik:RadMenuItem Text="Cambiar pregunta y respuesta secreta" meta:resourcekey="itemSecreta"/>
        </Items>
    </telerik:RadContextMenu>
    <telerik:RadMenu runat="server" ID="radMenu">
    </telerik:RadMenu>
    <telerik:RadContextMenu runat="server" ID="menuAyuda">
        <Items>
            <telerik:RadMenuItem Target="_blank" meta:resourcekey="itemOnline"/>
            <telerik:RadMenuItem meta:resourcekey="itemDesk"/>
        </Items>
    </telerik:RadContextMenu>
</div>