<%@ Page  Language="C#" MasterPageFile="~/Masters/Publico.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SAM.Web.Login" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHead" runat="server">    
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="contenedorCentral bordeNegroFull">
        <div class="divIzquierdo ancho50 bandaAzul">
        <asp:Label runat="server" CssClass="titulo" meta:resourcekey="lblLogin"></asp:Label>
        <asp:Login runat="server" ID="login" OnAuthenticate="login_Authenticate" OnLoggedIn="login_LoggedIn">
            <LayoutTemplate>
                <p>
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" meta:resourcekey="UserNameLabel" />
                    <asp:TextBox ID="UserName" runat="server" CssClass="blue" /><span class="required" >*</span>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ValidationGroup="login" Display="None" meta:resourcekey="UsernameRequired" />
                </p>
                <p>
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" meta:resourcekey="PasswordLabel" />
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" /><span class="required" >*</span>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" Display="None" ValidationGroup="login" meta:resourcekey="PasswordRequired" />
                </p>
                <p>
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" ValidationGroup="login" meta:resourcekey="LoginButton" CssClass="boton"  />
                </p>
            </LayoutTemplate>
        </asp:Login>
        </div>
        <div class="divDerecho ancho50 ">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">&nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="login" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p></p>
    </div>
    <div class="divDerecho" >
    <asp:Label ID="lblIE" runat="server" meta:resourcekey="lblIE" CssClass="ieLeyenda"></asp:Label>
    </div>
    <script language="javascript" type="text/javascript">

        $("input:text,:password").on("keypress", function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {                
                $(":submit").click();
                return false;
            }
        });

    </script>
</asp:Content>
