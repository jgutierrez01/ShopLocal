<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CambioPatio.aspx.cs" Inherits="SAM.Mobile.Paginas.CambioPatio"%>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:Form ID="form1" runat="server">
        <Mob:Panel runat="server" ID="pnlContent">
            <sam:samMenu ID="samMenu" runat="server" />
            <br />
            <Mob:Image runat="server" ID="imgTitle" meta:resourcekey="imgTitle" BreakAfter="true" />
            <%--<Mob:Label runat="server" ID="lblPatio" meta:resourcekey="lblPatio" />--%>
            <br />
            <Mob:SelectionList runat="server" ID="lstPatios" BreakAfter="true" />
            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" Font-Italic="True" ForeColor="Red" Font-Size="Small" BreakAfter="true" />
            <br />
            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClik" ImageUrl="~/Imagenes/Logos/OK.jpg" />
        </Mob:Panel>
    </Mob:Form>
</body>
</html>
