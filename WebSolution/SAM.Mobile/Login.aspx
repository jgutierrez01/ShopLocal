<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SAM.Mobile.Login" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sam Control</title>    

</head>
<body>         
    <Mob:Form ID="form1" runat="server"> 
    <Mob:DeviceSpecific ID="DeviceSpecific3" Runat="server">
            <Choice Filter="supportsJavaScript" Xmlns="http://schemas.microsoft.com/mobile/html32template">
                <ScriptTemplate>
                <script type="text/javascript" src="/Script/jquery-1.7-vsdoc.js"></script>
                <script type="text/javascript" src="/Script/jquery-1.7.js"></script>

    <script type="text/javascript">
    <!--
        function checkEnter(e) {
            var button = $("[name*=btnLogin]");
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    if (document.createEvent) {
                        button.click();
                    }
                    else if (document.createEventObject) {
                        button.focus();
                    }          
                }
            };
        //-->
    </script> 
    </ScriptTemplate>
    </Choice>
    </Mob:DeviceSpecific>
        <Mob:Panel runat="server" ID="pnlLogin">            
            <asp:Table runat="server">
                <asp:TableRow HorizontalAlign="Center">
                    <asp:TableCell><Mob:Image ID="Image1" runat="server" ImageUrl="Imagenes/Logos/logojpg.jpg" AlternateText="Logo Steelgo"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell HorizontalAlign="Center">
                        <Mob:Command runat="server" ID="cmdEspanol" OnClick="CambioEspanol" ImageUrl="~/Imagenes/Iconos/mexico.jpg" BreakAfter="false" CausesValidation="false" />
                            &nbsp;&nbsp;
                        <Mob:Command runat="server" ID="cmdIngles" OnClick="CambioIngles" ImageUrl="~/Imagenes/Iconos/us.jpg" BreakAfter="false" CausesValidation="false"/>
                    </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell><Mob:Label runat="server" ID="lblUsuario" meta:resourcekey="lblUsuario"/></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><Mob:TextBox runat="server" ID="txtUsuario" Size="20" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><Mob:Label runat="server" ID="lblPassword" meta:resourcekey="lblPassword" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:TextBox runat="server" ID="txtPassword" Size="21" TextMode="Password" onkeypress="checkEnter(event)" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <Mob:Command runat="server" ID="btnLogin" meta:resourcekey="btnLogin" OnClick="btnLogin_OnClick"/>
                   </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Label ID="lblError" runat="server" ForeColor="Red" /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>            
        </Mob:Panel>               
    </Mob:Form>               
</body>
</html>
