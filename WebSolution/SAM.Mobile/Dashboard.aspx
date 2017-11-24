<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SAM.Mobile.Dashboard" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:Form ID="form1" runat="server">
        <Mob:Panel runat="server" ID="pnlContent">
        <sam:samMenu ID="samMenu" runat="server" />
        <br />
        <Mob:Command runat="server" ID="hypCambiarPatio" meta:resourcekey="hypCambiarPatio" OnClick="hypCambiarPatio_OnClick"/>
        <br />
        <Mob:Command runat="server" ID="hypArmado" meta:resourcekey="hypArmado" OnClick="hypArmado_OnClick"/>
        <Mob:Command runat="server" ID="hypSoldadura" meta:resourcekey="hypSoldadura" OnClick="hypSoldadura_OnClick"/>
        <Mob:Command runat="server" ID="hypInspeccionVisual" meta:resourcekey="hypInspeccionVisual" OnClick="hypInspeccionVisual_OnClick"/>
        <Mob:Command runat="server" ID="hypInspeccionDimensional" meta:resourcekey="hypInspeccionDimensional" OnClick="hypInspeccionDimensional_OnClick" />
        <Mob:Command runat="server" ID="hypSeguimientoSpool" meta:resourcekey="hypSeguimientoSpool" OnClick="hypSeguimientoSpool_OnClick"/>
    
        </Mob:Panel>
    </Mob:Form>
</body>
</html>
