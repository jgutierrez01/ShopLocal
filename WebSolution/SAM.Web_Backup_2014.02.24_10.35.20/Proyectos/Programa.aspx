<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="Programa.aspx.cs" Inherits="SAM.Web.Proyectos.Programa" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetPrograma" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div>
                <div class="divIzquierdo ancho70">
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            <asp:Label runat="server" ID="lblFechaInicial" CssClass="bold" AssociatedControlID="dtpFechaInicial" meta:resourcekey="lblFechaInicial" />
                            <mimo:MappableDatePicker runat="server" ID="dtpFechaInicial" MinDate="01/01/1960" MaxDate="01/01/2050" EnableEmbeddedSkins="false" Width="230" />
                            <asp:RequiredFieldValidator runat="server" ID="reqFecha" ControlToValidate="dtpFechaInicial" Display="None" ValidationGroup="vgPrincipal" meta:resourcekey="reqFecha" />
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblRango" meta:resourcekey="lblRango" AssociatedControlID="ddlRango" />
                            <asp:DropDownList runat="server" ID="ddlRango">
                                <asp:ListItem Text="" Value="" Selected="True" />
                                <asp:ListItem Value="D" meta:resourcekey="liDia" />
                                <asp:ListItem Value="S" meta:resourcekey="liSemana"/>
                                <asp:ListItem Value="M" meta:resourcekey="liMes" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="reqRango" meta:resourcekey="reqRango" Display="None" ControlToValidate="ddlRango" ValidationGroup="vgPrincipal" />
                        </div>
                        <div class="subtitulo">
                            <asp:Label runat="server" ID="lblContrato" meta:resourcekey="lblContrato"/>
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtIsosPlaneados" meta:resourcekey="txtIsosPlaneados" MaxLength="8" />
                            <asp:RangeValidator runat="server" ID="rngIsosPlaneados" meta:resourcekey="rngIsosPlaneados" ControlToValidate="txtIsosPlaneados" Display="None" MinimumValue="0" MaximumValue="99999999" Type="Integer" ValidationGroup="vgPrincipal" />
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtSpoolsPlaneados" meta:resourcekey="txtSpoolsPlaneados"  MaxLength="8" />
                            <asp:RangeValidator runat="server" ID="rngSpoolsPlaneados" meta:resourcekey="rngSpoolsPlaneados" ControlToValidate="txtSpoolsPlaneados" Display="None" MinimumValue="0" MaximumValue="99999999" Type="Integer" ValidationGroup="vgPrincipal" />
                        </div>
                    </div>
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            &nbsp;
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblUnidades" meta:resourcekey="lblUnidades" AssociatedControlID="ddlUnidades" />
                            <asp:DropDownList runat="server" ID="ddlUnidades">
                                <asp:ListItem Text="" Value="" Selected="True" />
                                <asp:ListItem Value="P" meta:resourcekey="liPdis" />
                                <asp:ListItem Value="M" meta:resourcekey="liM2" />
                                <asp:ListItem Value="K" meta:resourcekey="liKg" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="reqUnidades" meta:resourcekey="reqUnidades" Display="None" ControlToValidate="ddlUnidades" ValidationGroup="vgPrincipal" />
                        </div>
                        <div class="subtitulo">
                            <asp:Label runat="server" ID="lblReprogramaciones" meta:resourcekey="lblReprogramaciones" />
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtIsosReprogramaciones" meta:resourcekey="txtIsosReprogramaciones" MaxLength="8" />
                            <asp:RangeValidator runat="server" ID="rngIsosRepro" meta:resourcekey="rngIsosRepro" ControlToValidate="txtIsosReprogramaciones" Display="None" MinimumValue="0" MaximumValue="99999999" Type="Integer" ValidationGroup="vgPrincipal" />
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtSpoolsReprogramaciones" meta:resourcekey="txtSpoolsReprogramaciones" MaxLength="8" />
                            <asp:RangeValidator runat="server" ID="rngSpoolsRepro" meta:resourcekey="rngSpoolsRepro" ControlToValidate="txtSpoolsReprogramaciones" Display="None" MinimumValue="0" MaximumValue="99999999" Type="Integer" ValidationGroup="vgPrincipal" />
                        </div>
                    </div>
                </div>
                <div class="divIzquierdo ancho30">
                    <div class="validacionesRecuadro" style="margin-top:20px;">
                        <div class="validacionesHeader"></div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" ValidationGroup="vgPrincipal" />
                            <asp:ValidationSummary runat="server" ID="valGlobal" CssClass="summary" meta:resourcekey="valGlobal" ValidationGroup="vgDosGrupos" />
                        </div>
                    </div>
                </div>
                <p></p>
            </div>
            <div style="margin-top:20px;">
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="40" />
                        <col width="60" />
                        <col width="100" />
                        <col width="100" />
                        <col width="200" />
                        <col width="200" />
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado" style="font-weight:normal;">
                            <th colspan="4">
                                <span class="tituloIzquierda">
                                    <asp:Literal runat="server" ID="litPeriodos" meta:resourcekey="litPeriodos" />
                                </span>
                            </th>
                            <th colspan="2">
                                <span class="iconoDerecha">
                                    <asp:ImageButton runat="server" meta:resourcekey="imgAgregar" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" OnClick="imgAgregar_Click" CausesValidation="true" ValidationGroup="vgPrincipal" />
                                    <asp:LinkButton runat="server" ID="lnkAgregar" meta:resourcekey="lnkAgregar" CssClass="link" OnClick="lnkAgregar_Click" CausesValidation="true" ValidationGroup="vgPrincipal" />
                                </span>
                            </th>
                        </tr>
                        <tr class="repTitulos">
                            <th>&nbsp;</th>
                            <th><asp:Literal runat="server" ID="litNum" meta:resourcekey="litNum" /></th>
                            <th><asp:Literal runat="server" ID="litFechaInicial" meta:resourcekey="litFechaInicial" /></th>
                            <th><asp:Literal runat="server" ID="litFechaFinal" meta:resourcekey="litFechaFinal" /></th>
                            <th><asp:Literal runat="server" ID="litPorContrato" meta:resourcekey="litPorContrato" /></th>
                            <th><asp:Literal runat="server" ID="litRepro" meta:resourcekey="litRepro" /></th>
                        </tr>
                    </thead>
                    <asp:Repeater runat="server" ID="repPeriodos" OnItemDataBound="repPeriodos_ItemDataBound">
                        <ItemTemplate>
                            <tr class="repFila">
                                <td style="text-align:center;">
                                    <asp:ImageButton runat="server" ID="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" meta:resourcekey="imgBorrar" OnClick="imgBorrar_Click" Visible="false" OnClientClick="return Sam.Confirma(1);" CausesValidation="false" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNumero" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblFechaInicio" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblFechaFin" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtContrato" MaxLength="13" CssClass="dentroGrid" />
                                    <asp:Label runat="server" ID="lblUnidades" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqContratoPeriodo" meta:resourcekey="reqContratoPeriodo" ControlToValidate="txtContrato" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" />
                                    <asp:CustomValidator runat="server" ID="rngContratoPeriodo" meta:resourcekey="rngContratoPeriodo" ControlToValidate="txtContrato" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" ClientValidationFunction="Sam.Programa.Validaciones.MayorIgualCeroMenorOchoNueves" OnServerValidate="cusRango_ServerValidate" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtReprogramaciones" MaxLength="13" CssClass="dentroGrid" />
                                    <asp:Label runat="server" ID="lblUnidades2" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqReproPeriodo" meta:resourcekey="reqReproPeriodo" ControlToValidate="txtReprogramaciones" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" />
                                    <asp:CustomValidator runat="server" ID="rngReproPeriodo" meta:resourcekey="rngReproPeriodo" ControlToValidate="txtReprogramaciones" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" ClientValidationFunction="Sam.Programa.Validaciones.MayorIgualCeroMenorOchoNueves" OnServerValidate="cusRango_ServerValidate" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td style="text-align:center;">
                                    <asp:ImageButton runat="server" ID="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" meta:resourcekey="imgBorrar" OnClick="imgBorrar_Click" Visible="false" OnClientClick="return Sam.Confirma(1);" CausesValidation="false" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNumero" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblFechaInicio" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblFechaFin" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtContrato" MaxLength="13" CssClass="dentroGrid" />
                                    <asp:Label runat="server" ID="lblUnidades" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqContratoPeriodo" meta:resourcekey="reqContratoPeriodo" ControlToValidate="txtContrato" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" />
                                    <asp:CustomValidator runat="server" ID="rngContratoPeriodo" meta:resourcekey="rngContratoPeriodo" ControlToValidate="txtContrato" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" ClientValidationFunction="Sam.Programa.Validaciones.MayorIgualCeroMenorOchoNueves" OnServerValidate="cusRango_ServerValidate" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtReprogramaciones" MaxLength="13" CssClass="dentroGrid" />
                                    <asp:Label runat="server" ID="lblUnidades2" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqReproPeriodo" meta:resourcekey="reqReproPeriodo" ControlToValidate="txtReprogramaciones" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" />
                                    <asp:CustomValidator runat="server" ID="rngReproPeriodo" meta:resourcekey="rngReproPeriodo" ControlToValidate="txtReprogramaciones" Display="Dynamic" CssClass="required" ValidationGroup="vgPeriodos" ClientValidationFunction="Sam.Programa.Validaciones.MayorIgualCeroMenorOchoNueves" OnServerValidate="cusRango_ServerValidate" />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="6">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_OnClick" ValidationGroup="vgDosGrupos" />
        <asp:CustomValidator runat="server" ID="cusPeriodos" meta:resourcekey="cusPeriodos" ClientValidationFunction="Sam.Programa.Validaciones.ValidaPeriodos" ValidationGroup="vgDosGrupos" Display="None" />
    </div>
</asp:Content>
<asp:Content runat="server" ID="cntFooter" ContentPlaceHolderID="cphInnerFoot">
    <script language="javascript" type="text/javascript" src="/Scripts/autoNumeric-1.6.2.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            Sam.Programa.Inicializa();
        });
    </script>
</asp:Content>
