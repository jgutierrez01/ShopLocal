<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleInspeccionVisual.aspx.cs" Inherits="SAM.Mobile.Paginas.DetalleInspeccionVisual" %>
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
                        <Mob:Label runat="server" ID="lblNoOdt" meta:resourcekey="lblNoOdt" BreakAfter="false" StyleReference="TableCells" />
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
                        <Mob:Label runat="server" ID="lblNoJunta" meta:resourcekey="lblNoJunta" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstNoJunta" BreakAfter="false" SelectType="DropDown" OnSelectedIndexChanged="lstNoJunta_OnSelectedIndexChange"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaInspeccionVisual" meta:resourcekey="lblFechaInspeccionVisual" BreakAfter="false" StyleReference="TableCells"/>
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
            </asp:Table>
            <asp:Table runat="server" ID="tblDefectos">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                        <Mob:Label runat="server" ID="lblDefectosDisponibles" meta:resourcekey="lblDefectosDisponibles" BreakAfter="true" StyleReference="TableCells"/>
                        <Mob:SelectionList runat="server" ID="lstDefectosDisponibles" BreakAfter="false" SelectType="ListBox" />
                    </asp:TableCell>                    
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <Mob:Command runat="server" ID="cmdAgregarDefecto" BreakAfter="false" meta:resourcekey="cmdAgregarDefecto" OnClick="cmdAgregarDefecto_OnClick" ImageUrl="~/Imagenes/Logos/Add.jpg" />
                        &nbsp;&nbsp;&nbsp;
                        <Mob:Command runat="server" ID="cmdRemoverDefecto" BreakAfter="true" meta:resourcekey="cmdRemoverDefecto" OnClick="cmdRemoverDefecto_OnClick" ImageUrl="~/Imagenes/Logos/Remove.jpg"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                        <Mob:Label runat="server" ID="lblDefectosEncontrados" meta:resourcekey="lblDefectosEncontrados" BreakAfter="true" StyleReference="TableCells"/>
                        <Mob:SelectionList runat="server" ID="lstDefectosEncontrados" BreakAfter="false" SelectType="ListBox" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <Mob:Label runat="server" ID="lblObservaciones" meta:resourcekey="lblObservaciones" BreakAfter="false" StyleReference="TableCells"/>
            <Mob:TextBox runat="server" ID="txtObservaciones" BreakAfter="true" />

            <asp:HiddenField runat="server" ID="hfFechaSold" Value="" />
            <asp:HiddenField runat="server" ID="hfConfirmacion" Value="" meta:resourcekey="hfConfirmacion" />
            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" ForeColor="Red" Font-Size="Small" BreakAfter="true" />
            <asp:CheckBox runat="server" Visible="false" Checked="false" ID="cbConfirmacion" />
            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClick" ImageUrl="~/Imagenes/Logos/OK.jpg"/>
        </Mob:Panel>
    </Mob:Form>
</body>
</html>