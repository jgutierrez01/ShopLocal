<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleSeguimientoSpool.aspx.cs" Inherits="SAM.Mobile.Paginas.DetalleSeguimientoSpool" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:StyleSheet ID="StyleSheet1" runat="server">
        <Style Name="TableCells" Font-Bold="True" Font-Size ="Small"/>
        <Style Name="TableLabels"  Font-Size ="Small"/>
    </Mob:StyleSheet>
    <Mob:Form ID="form1" runat="server">
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
                        <Mob:Label runat="server" ID="lblRqPWHT" meta:resourcekey="lblRqPWHT" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblRqPWHT2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <Mob:ObjectList ID="oblstJuntas" Runat="server" StyleReference="TableCells" AutoGenerateFields="False" TableFields="Junta;Tipo;IV;RT;PT;PWHT" meta:resourcekey="oblstJuntas">
            
                <Field DataField="Etiqueta" Name="Junta" Title="Junta" />
                <Field DataField="Tipo" Name="Tipo" Title="Tipo" />
                <Field DataField="InspeccionVisualTexto" Name="IV" Title="IV" />
                <Field DataField="RTTexto" Name="RT" Title="RT" />
                <Field DataField="PTTexto" Name="PT" Title="PT" />
                <Field DataField="PWHTTexto" Name="PWHT" Title="PWHT" />
            </Mob:ObjectList>           

            <br />
            <Mob:Label runat="server" ID="lblTituloLiberacionDimensional" meta:resourcekey="lblTituloLiberacionDimensional" BreakAfter="false" StyleReference="TableCells" Font-Size="Normal"/>
            <asp:Table runat="server" ID="tblLiberacionDimensional">
             <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                         
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaLiberacion" meta:resourcekey="lblFechaLiberacion" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaLiberacion2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaReporte2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroReporte" meta:resourcekey="lblNumeroReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroReporte2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <br />
            <Mob:Label runat="server" ID="lblTituloPintura" meta:resourcekey="lblTituloPintura" BreakAfter="false" StyleReference="TableCells" Font-Size="Normal"/>
            <asp:Table runat="server" ID="tblPintura">
             <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                         
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>

                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaRequisicion" meta:resourcekey="lblFechaRequisicion" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaRequisicion2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroRequisicion" meta:resourcekey="lblNumeroRequisicion" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroRequisicion2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblCodigoPintura" meta:resourcekey="lblCodigoPintura" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblCodigoPintura2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblColorPintura" meta:resourcekey="lblColorPintura" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblColorPintura2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSistemaPintura" meta:resourcekey="lblSistemaPintura" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSistemaPintura2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Table runat="server" ID="tblDetallePintura">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblSandBlast" meta:resourcekey="lblSandBlast" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>                
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaSandBlast" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporteSandBlast" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblPrimarios" meta:resourcekey="lblPrimarios" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha2" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaPrimarios" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte2" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReportePrimarios" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblIntermedios" meta:resourcekey="lblIntermedios" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha3" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaIntermedios" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte3" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporteIntermedios" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblAcabadoVisual" meta:resourcekey="lblAcabadoVisual" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha4" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaAcabadoVisual" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte4" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporteAcabadoVisual" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblAdherencia" meta:resourcekey="lblAdherencia" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha5" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaAdherencia" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte5" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporteAdherencia" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <br />
                        <Mob:Label runat="server" ID="lblPullOff" meta:resourcekey="lblPullOff" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFecha6" meta:resourcekey="lblFecha" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaPullOff" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReporte6" meta:resourcekey="lblReporte" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblReportePullOff" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <br />
            <Mob:Label runat="server" ID="lblTituloEmbarque" meta:resourcekey="lblTituloEmbarque" BreakAfter="false" StyleReference="TableCells" Font-Size="Normal"/>
            <asp:Table runat="server" ID="tblEmbarque">
             <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                         
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaPreparacionEmbarque" meta:resourcekey="lblFechaPreparacionEmbarque" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaPreparacionEmbarque2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblEtiquetaEmbarque" meta:resourcekey="lblEtiquetaEmbarque" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblEtiquetaEmbarque2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaEtiquetaEmbarque" meta:resourcekey="lblFechaEtiquetaEmbarque" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaEtiquetaEmbarque2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroEmbarque" meta:resourcekey="lblNumeroEmbarque" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNumeroEmbarque2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaEmbarque" meta:resourcekey="lblFechaEmbarque" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaEmbarque2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            
            <br />
            <asp:Table runat="server" ID="tblHolds">
             <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">                         
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblHoldCalidad" meta:resourcekey="lblHoldCalidad" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                         <Mob:Label runat="server" ID="lblHldCalidad" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblHoldIngenieria" meta:resourcekey="lblHoldIngenieria" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left"> 
                    <Mob:Label runat="server" ID="lblHldIngenieria" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblConfinado" meta:resourcekey="lblConfinado" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblConfinado2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <Mob:Label runat="server" ID="lblPruebasNoDestructivas" meta:resourcekey="lblPruebasNoDestructivas" BreakAfter="false" StyleReference="TableCells" Font-Size="Normal"/>
            <asp:Table runat="server" ID="tblPruebas">
            <asp:TableRow>                    
                    <asp:TableCell HorizontalAlign="Left">
                        
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>                    
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblPctPND" meta:resourcekey="lblPctPND" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblPctPND2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblRadiadas" meta:resourcekey="lblRadiadas" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblRadiadas2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblLiquidos" meta:resourcekey="lblLiquidos" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblLiquidos2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblRadiadasFaltantes" meta:resourcekey="lblRadiadasFaltantes" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblRadiadasFaltantes2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblLiquidosFaltantes" meta:resourcekey="lblLiquidosFaltantes" BreakAfter="false" StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblLiquidosFaltantes2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClick" ImageUrl="~/Imagenes/Logos/OK.jpg"/>
        </Mob:Panel>
    </Mob:Form>
</body>
</html>