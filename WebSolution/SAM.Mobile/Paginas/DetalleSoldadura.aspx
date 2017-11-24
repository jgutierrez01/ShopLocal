<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleSoldadura.aspx.cs" Inherits="SAM.Mobile.Paginas.DetalleSoldadura" %>
<%@ Register Src="~/Controles/Menu.ascx" TagName="samMenu" TagPrefix="sam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sam Control</title>
</head>
<body>
    <Mob:StyleSheet ID="StyleSheet1" runat="server">
        <Style Name="TableCells" Font-Bold="True" Font-Size ="Small"/>
         <Style Name="TableLabels" Font-Size ="Small" />
    </Mob:StyleSheet>
    <Mob:Form ID="form1" runat="server">
        <Mob:DeviceSpecific ID="DeviceSpecific3" Runat="server">
            <Choice Filter="supportsJavaScript" Xmlns="http://schemas.microsoft.com/mobile/html32template">
                <ScriptTemplate>
                    <script type="text/javascript" src="/Script/jquery-1.7-vsdoc.js"></script>
                    <script type="text/javascript" src="/Script/jquery-1.7.js"></script>

                    <script type="text/javascript">

                        $(document).ready(function () {
                            var seen = {};

                            $("[name=lstWpsRaiz]").children().each(function () {
                                var txt = $(this).text();
                                if (seen[txt])
                                    $(this).remove();
                                else
                                    seen[txt] = true;
                            });

                            seen = {};
                            $("[name=lstWpsRelleno]").children().each(function () {
                                var txt = $(this).text();
                                if (seen[txt])
                                    $(this).remove();
                                else
                                    seen[txt] = true;
                            });

                            $("[name=slWpsDiferentes]").click(function () {
                                $("[name=lstWpsRaiz]").val($("[name=lstWpsRaiz] option:first").val());
                                $("[name=lstWpsRelleno]").val($("[name=lstWpsRelleno] option:first").val());

                                var checkbox = $("[name=slWpsDiferentes]");
                                var button = $("[name*=cmdOK]");

                                button.click();
                            });

                            $("[name=slTermidadoConRaiz]").click(function () {
                                $("[name=lstWpsRaiz]").val($("[name=lstWpsRaiz] option:first").val());
                                $("[name=lstWpsRelleno]").val($("[name=lstWpsRelleno] option:first").val());
                                var checkbox = $("[name=slTermidadoConRaiz]");
                                var button = $("[name*=cmdOK]");
                                var botonTerminado = $("[name*=borrarTRaiz]");
                                button.click();
                            });

                            $("[name=lstProcRaiz]").change(function () {
                                debugger;
                                var chekTerminadoRaiz = $("[name=slTermidadoConRaiz]");
                                var boton = $("[name*=cmdUpdateWPS]");
                                if (chekTerminadoRaiz.is(":checked")) {
                                    boton.click();
                                }
                            });

                            $("[name=lstWpsRaiz]").change(function () {
                                var checkbox = $("[name=slWpsDiferentes]");
                                if (!checkbox.is(":checked")) {

                                    var WpsRaiz = $("[name=lstWpsRaiz] option:selected").text();
                                    $("[name=lstWpsRelleno]").children().each(function () {
                                        if ($(this).text() == WpsRaiz) {
                                            $(this).attr("selected", "selected");
                                            return;
                                        }
                                    });
                                }
                            });

                            $("[name=lstWpsRelleno]").change(function () {
                                var checkbox = $("[name=slWpsDiferentes]");
                                if (!checkbox.is(":checked")) {

                                    var WpsRelleno = $("[name=lstWpsRelleno] option:selected").text();
                                    $("[name=lstWpsRaiz]").children().each(function () {
                                        if ($(this).text() == WpsRelleno) {
                                            $(this).attr("selected", "selected");
                                            return;
                                        }
                                    });
                                }
                            });
                        });

                    </script> 
                </ScriptTemplate>
            </Choice>
        </Mob:DeviceSpecific>
        <Mob:Panel runat="server" ID="pnlContent">
            <sam:samMenu ID="samMenu" runat="server" />
            <br />
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
                        <Mob:Label runat="server" ID="lblNoControl" meta:resourcekey="lblNoControl" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoControl2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSpool2" BreakAfter="false" StyleReference="TableLabels"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblPwht" meta:resourcekey="lblPwht" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblPwhtTexto" BreakAfter="false" StyleReference="TableLabels" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblNoJunta" meta:resourcekey="lblNoJunta" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstNoJunta" BreakAfter="false" SelectType="DropDown" OnSelectedIndexChanged="lstNoJunta_OnSelectedIndexChange"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                            <Mob:Command runat="server" ID="cmdUpdate" BreakAfter="true" meta:resourcekey="cmdUpdate" />
                            <br />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table> 

            <Mob:Label runat="server" ID="lblError" Visible="false" meta:resourcekey="lblError" ForeColor="Red" Font-Size="Small" BreakAfter="true" />
            <Mob:Label runat="server" ID="lblMensaje" Visible="false" meta:resourcekey="lblMensaje" Font-Size="Small" BreakAfter="true" />

            <Mob:Panel runat="server" ID="pnlGeneral" Visible="false">
                <asp:Table runat="server" ID="tblSoldaduraGeneral" >
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblFechaSoldadura" meta:resourcekey="lblFechaSoldadura" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstFecha" BreakAfter="false" SelectType="DropDown" />
                    </asp:TableCell>
                </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblTaller" meta:resourcekey="lblTaller" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left" >
                        <Mob:SelectionList runat="server" ID="lstTaller" BreakAfter="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblProcRaiz" meta:resourcekey="lblProcRaiz" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstProcRaiz" SelectType="DropDown" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblProcRelleno" meta:resourcekey="lblProcRelleno" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstProcRelleno" SelectType="DropDown" BreakAfter="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblObservaciones" meta:resourcekey="lblObservaciones" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox runat="server" ID="txtObservaciones" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </asp:TableCell>
                </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblMatBase1" meta:resourcekey="lblMatBase1" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblMaterialBase1" BreakAfter="false"/>
                    </asp:TableCell>
                </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblMatBase2" meta:resourcekey="lblMatBase2" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblMaterialBase2" BreakAfter="false"/>
                    </asp:TableCell>
                </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:SelectionList runat="server" ID="slWpsDiferentes" SelectType="CheckBox" StyleReference="TableCells" OnSelectedIndexChanged="slWpsDiferentes_OnSelectedIndexChanged" >
                                <Item meta:resourcekey="lblWpsDiferentes" Selected="false" />
                            </Mob:SelectionList>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:SelectionList runat="server" ID="slTermidadoConRaiz" SelectType="CheckBox" StyleReference="TableCells" OnSelectedIndexChanged="slTermidadoConRaiz_SelectedIndexChanged">
                                <Item meta:resourcekey="lblTerminadoConRaiz" Selected="false" />
                            </Mob:SelectionList>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                            <Mob:Command runat="server" ID="cmdUpdateWPS" BreakAfter="true" meta:resourcekey="cmdUpdateWPS" OnClick="cmUpdateWPS_OnClick" />      
                            <br />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:Label runat="server" ID="lblWpsRaiz" meta:resourcekey="lblWpsRaiz" BreakAfter="false" StyleReference="TableCells" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:SelectionList runat="server" ID="lstWpsRaiz" SelectType="DropDown" BreakAfter="false" OnSelectedIndexChanged="lstWpsRaiz_OnSelectedIndexChanged" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:Label runat="server" ID="lblWpsRelleno" meta:resourcekey="lblWpsRelleno" BreakAfter="false" StyleReference="TableCells" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left">
                            <Mob:SelectionList runat="server" ID="lstWpsRelleno" SelectType="DropDown" BreakAfter="false" OnSelectedIndexChanged="lstWpsRelleno_OnSelectedIndexChanged" />
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                             <Mob:Command runat="server" ID="cmdActualizarSoldadores" BreakAfter="true" meta:resourcekey="cmdUpdateSoldadores" />
                             <br />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table runat="server" ID="tblSolRelleno" Visible="false">
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSoldadorRelleno" meta:resourcekey="lblSoldadorRelleno" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstSoldadorRelleno" SelectType="DropDown" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblColadaSoldadorRelleno" meta:resourcekey="lblColadaSoldadorRelleno" BreakAfter="false"  StyleReference="TableCells"/>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:TextBox runat="server" ID="txtColadaSoldadorRelleno" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <Mob:Command runat="server" ID="cmdAgregarSolRelleno" BreakAfter="true" meta:resourcekey="cmdAgregarSolRelleno" OnClick="cmdAgregarSolRelleno_OnClick" />      
                    </asp:TableCell>
                </asp:TableRow>
                </asp:Table>

                <Mob:ObjectList ID="oblstSoldadoresRelleno" Runat="server" AutoGenerateFields="False" TableFields="Codigo;Nombre;Colada" LabelStyle-Font-Size="Small"
             OnItemCommand="oblstSoldadoresRelleno_CommandClick" OnShowItemCommands="oblstSoldadoresRelleno_ItemCommands_Show" StyleReference="TableLabels"  >
                <Field DataField="Codigo" Name="Codigo"   />
                <Field DataField="Nombre" Name="Nombre" Title="Nombre" />
                <Field DataField="Colada" Name="Colada" Title="Colada" />
                <Command Name="Borrar" Text="Borrar"/>
                <Field DataField="ID" Name="ID" Visible="False" />
            </Mob:ObjectList>
                <br />
                <asp:Table runat="server" ID="tlbSoldadorRaiz" Visible="false">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblSoldadorRaiz" meta:resourcekey="lblSoldadorRaiz" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:SelectionList runat="server" ID="lstSoldadorRaiz" SelectType="DropDown" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:Label runat="server" ID="lblColadaSoldadorRaiz" meta:resourcekey="lblColadaSoldadorRaiz" BreakAfter="false" StyleReference="TableCells" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <Mob:TextBox runat="server" ID="txtColadaSoldadorRaiz" BreakAfter="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" ColumnSpan="2">
                        <Mob:Command runat="server" ID="cmdAgregarSolRaiz" BreakAfter="true" meta:resourcekey="cmdAgregarSolRaiz" OnClick="cmdAgregarSolRaiz_OnClick" />      
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

                <Mob:ObjectList ID="oblstSoldadoresRaiz" Runat="server" AutoGenerateFields="False" TableFields="Codigo;Nombre;Colada" LabelStyle-Font-Size="Small" StyleReference="TableLabels" 
                OnItemCommand="oblstSoldadoresRaiz_CommandClick" OnShowItemCommands="oblstSoldadoresRaiz_ItemCommands_Show">
                <Field DataField="Codigo" Name="Codigo" Title="Codigo" />
                <Field DataField="Nombre" Name="Nombre" Title="Nombre" />
                <Field DataField="Colada" Name="Colada" Title="Colada" />
                <Command Name="Borrar" Text="Borrar"/>
                <Field DataField="ID" Name="ID" Visible="False" />
            </Mob:ObjectList>
                <br />

                <Mob:Command runat="server" ID="cmdOK" meta:resourcekey="cmdOK" OnClick="cmdOK_OnClick" ImageUrl="~/Imagenes/Logos/OK.jpg"/>
            </Mob:Panel>
        </Mob:Panel>
    </Mob:Form>
</body>
</html>
