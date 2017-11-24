<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PerfilLogin.ascx.cs" Inherits="SAM.Web.Controles.Login.PerfilLogin" %>
<div class="recuadroUsuarioLogin texto">
    <p style="height:3px; min-height:3px; max-height:3px; margin:0px;"></p>
    <asp:ImageButton CausesValidation="false" ImageUrl="~/Imagenes/Iconos/mexico.png" ID="btnEspanol" OnClick="Espanol_Click" runat="server" ToolTip="MEX"/>
     <asp:ImageButton CausesValidation="false" ImageUrl="~/Imagenes/Iconos/us.png" ID="btnIngles" OnClick="Ingles_Click" runat="server" ToolTip="US" />
    
</div>