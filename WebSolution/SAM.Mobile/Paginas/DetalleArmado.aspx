<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleArmado.aspx.cs" Inherits="SAM.Mobile.Paginas.DetalleArmado" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:StyleSheet runat="server">
        <Style Name="TableCells" Font-Bold="True" Font-Size ="Small"/>
         <Style Name="TableLabels" Font-Size ="Small"/>
    </Mob:StyleSheet>
    <Mob:Form ID="form1" runat="server">
        <Mob:Panel runat="server" ID="pnlContent">
            <sam:samMenu ID="samMenu" runat="server" />
            <asp:Table runat="server" ID="Table1">
             <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoOdt" meta:resourcekey="lblNoOdt" BreakAfter="false"  StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoOdt2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoControl" meta:resourcekey="lblNoControl" BreakAfter="false"  StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoControl2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" BreakAfter="false"  StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoJunta" meta:resourcekey="lblNoJunta" BreakAfter="false"  StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstNoJunta" BreakAfter="false" SelectType="DropDown" OnSelectedIndexChanged="lstNoJunta_OnSelectedIndexChange"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left" ColumnSpan="2">
                         <Mob:Command runat="server" ID="cmdUpdate" BreakAfter="true" Text="Actualizar" meta:resourcekey="cmdUpdate"/>
                    </asp:TableCell>
                </asp:TableRow>               
                
            </asp:Table>
            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" ForeColor="Red" Font-Size="Small" BreakAfter="true" />
            <Mob:Label runat="server" ID="lblMensaje" Visible="false" StyleReference="TableCells" BreakAfter="true" />

            <asp:Table runat="server" ID="tblMenu" Visible="false">
            <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                </asp:TableRow>
            <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaArmado" meta:resourcekey="lblFechaArmado" BreakAfter="false"  StyleReference="TableCells"/>            
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstFecha" BreakAfter="false" SelectType="DropDown" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoUnico1" meta:resourcekey="lblNoUnico1" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstNumeroUnico1" SelectType="DropDown" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoUnico2" meta:resourcekey="lblNoUnico2" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstNumeroUnico2" SelectType="DropDown" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblTaller" meta:resourcekey="lblTaller" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstTaller" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblTubero" meta:resourcekey="lblTubero" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstTuberos" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblObservaciones" meta:resourcekey="lblObservaciones" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            
            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClik" ImageUrl="~/Imagenes/Logos/OK.jpg" Visible="false" />
        </Mob:Panel>
    </Mob:Form>
</body>
</html>
