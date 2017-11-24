<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Armado.aspx.cs" Inherits="SAM.Mobile.Paginas.Armado" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            var button = $("[name*=cmdOK]");
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
        <Mob:Panel runat="server" ID="pnlContent">
            <sam:samMenu ID="samMenu" runat="server" />
            
            <Mob:Image runat="server" ID="imgTitle" meta:resourcekey="imgTitle" BreakAfter="true" />
            <br />
            <Mob:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" />
            <asp:TextBox runat="server" ID="txtSpool" BreakAfter="true" onkeypress="checkEnter(event)" />
            <br />
            <Mob:Label runat="server" ID="lblNoControl" meta:resourcekey="lblNoControl" />
            <asp:TextBox runat="server" ID="txtNoControl" BreakAfter="true" onkeypress="checkEnter(event)" />
            <br />
            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" ForeColor="Red" Font-Size="Small" BreakAfter="true" />
            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClik" ImageUrl="~/Imagenes/Logos/OK.jpg" />
        </Mob:Panel>
    </Mob:Form>
</body>
</html>
