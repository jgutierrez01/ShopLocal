<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="SAM.Mobile.Controles.Menu" %>
<Mob:Panel ID="Panel1" runat="server" BreakAfter="true">
    <asp:Table runat="server" ID="tblMenu">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <Mob:Image runat="server" ImageUrl="~/Imagenes/Logos/logomenu.jpg" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center" >
                    <Mob:Command runat="server" ID="cmdEspanol" OnClick="Espanol_Click" ImageUrl="~/Imagenes/Iconos/mexico.jpg" BreakAfter="false" />
                    &nbsp;&nbsp;
                    <Mob:Command runat="server" ID="cmdIngles" OnClick="Ingles_Click" ImageUrl="~/Imagenes/Iconos/us.jpg" BreakAfter="false" />
                    &nbsp;&nbsp;
                    <Mob:Command runat="server" ID="btnLogout" OnClick="btnLogout_Click" ImageUrl="~/Imagenes/Iconos/salir.jpg" BreakAfter="false"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left">
                    <Mob:Link runat="server" ID="hypMenu" meta:resourcekey="hypMenu" BreakAfter="false"/>                    
                    &nbsp;&nbsp;
                    <Mob:Label runat="server" ID="lblPatio" Font-Bold="True" BreakAfter="false"/>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</Mob:Panel>