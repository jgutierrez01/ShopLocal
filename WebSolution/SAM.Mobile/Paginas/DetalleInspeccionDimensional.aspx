<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleInspeccionDimensional.aspx.cs" Inherits="SAM.Mobile.Paginas.DetalleInspeccionDimensional" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:StyleSheet ID="StyleSheet1" runat="server">
        <Style Name="TableCells" Font-Bold="True" Font-Size ="Small"/>
        <Style Name="TableLabels" Font-Size ="Small"/>
    </Mob:StyleSheet>
    <Mob:Form ID="form1" runat="server">
    <Mob:DeviceSpecific ID="DeviceSpecific3" Runat="server">
            <Choice Filter="supportsJavaScript" Xmlns="http://schemas.microsoft.com/mobile/html32template">
                <ScriptTemplate>
                <script type="text/javascript" src="/Script/jquery-1.7-vsdoc.js"></script>
                <script type="text/javascript" src="/Script/jquery-1.7.js"></script>

    <script type="text/javascript">
        window.onload = function () {
            var button = $("[name=cmdOK]");
            var mensajeConfirmacion = $("[id*=hfConfirmacion]").val();
            var fechaProcSold = $("[id*=hfFechaSold]").val();

            $("[name=cmdOK]").click(function () {
                if (fechaProcSold != "") {
                    var fechaProceso = $("[name*=lstFecha] option:selected").text().split("\n")[0];

                    var fechaProcLD = new Date(fechaProceso);
                    var fechaProcesoSold = new Date(fechaProcSold);

                    if (fechaProcLD < fechaProcesoSold) {
                        return confirm(mensajeConfirmacion);
                    }
                }
            });
        };
    </script> 
    </ScriptTemplate>
    </Choice>
    </Mob:DeviceSpecific>


        <Mob:Panel runat="server" ID="pnlContent">
            <sam:samMenu ID="samMenu" runat="server" />
            <asp:Table runat="server" ID="tblMenu">
            <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                    </asp:TableCell>
                </asp:TableRow>           
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoOdt" meta:resourcekey="lblNoOdt" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoOdt2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoControl" meta:resourcekey="lblNoControl" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoControl2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
               <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaInspeccionDimensional" meta:resourcekey="lblFechaInspeccionDimensional" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstFecha" BreakAfter="false" SelectType="DropDown" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblResultado" meta:resourcekey="lblResultado" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstResultado" BreakAfter="false" SelectType="DropDown" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblObservaciones" meta:resourcekey="lblObservaciones" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:TextBox runat="server" ID="txtObservaciones" BreakAfter="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            
            <asp:HiddenField runat="server" ID="hfFechaSold" Value="" />
            <asp:HiddenField runat="server" ID="hfConfirmacion" Value="" meta:resourcekey="hfConfirmacion" />
            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" ForeColor="Red" Font-Size="Small" BreakAfter="true" />

            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClick" ImageUrl="~/Imagenes/Logos/OK.jpg"/>
        </Mob:Panel>
    </Mob:Form>
</body>
</html>