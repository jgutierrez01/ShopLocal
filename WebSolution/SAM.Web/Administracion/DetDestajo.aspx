<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="DetDestajo.aspx.cs" Inherits="SAM.Web.Administracion.DetDestajo" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadWindow runat="server" ID="rdwComentariosProceso" OnClientClose="Sam.Destajo.UnbindCmts">
        <ContentTemplate>
            <div id="cmtsProceso"></div>
            <div id="dvCmtsDestajo">
                <asp:TextBox runat="server" ID="txtCmtsDestajo" Columns="100" Rows="4" ClientIDMode="Static" TextMode="MultiLine" />
                <div class="separador" style="padding-left: 50px;">
                    <asp:Button runat="server" ID="btnCmts" CssClass="boton" meta:resourcekey="btnCmts" ClientIDMode="Static" UseSubmitBehavior="false" CausesValidation="false"/>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" />
    <div class="cntCentralForma">
        <div>&nbsp;</div>
        <asp:HiddenField runat="server" ID="hdnTipoDestajo" ClientIDMode="Static" />
        <div class="cajaAzul detalleDestajo">
            <div class="divIzquierdo col1">
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblCategoriaTexto" meta:resourcekey="lblCategoriaTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblCategoria" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblCodigoTexto" meta:resourcekey="lblCodigoTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblCodigo" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblNombreTexto" meta:resourcekey="lblNombreTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblNombre" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblNumEmpleadoTexto" meta:resourcekey="lblNumEmpleadoTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblNumEmpleado" />
                </div>
            </div>
            <div class="divIzquierdo col2">
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblSemanaTexto" meta:resourcekey="lblSemanaTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblSemana" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblFechasTexto" meta:resourcekey="lblFechasTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblFechas" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblCostoCuadroTexto" meta:resourcekey="lblCostoCuadroTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblCostoCuadro" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblDiasFestPeriodoTexto" meta:resourcekey="lblDiasFestPeriodoTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblDiasFest" />
                </div>
            </div>
            <div class="divIzquierdo col3">
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblEstatusPeriodoTexto" meta:resourcekey="lblEstatusPeriodoTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblEstatusPeriodo" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label runat="server" ID="lblAprobadoTexto" meta:resourcekey="lblAprobadoTexto" CssClass="bold" />
                    <asp:Label runat="server" ID="lblAprobado" />
                </div>
                <div class="separadorDashboard">
                    <div class="paginadorDestajo">
                        <asp:ImageButton CssClass="ant" runat="server" ID="imgAnterior" meta:resourcekey="imgAnterior" OnClick="imgAnterior_Click" ImageUrl="~/Imagenes/Iconos/ico_anterior.png" />
                        <asp:Literal runat="server" ID="litTextoPager" />
                        <asp:ImageButton CssClass="sig" runat="server" ID="imgSiguiente" meta:resourcekey="imgSiguiente" OnClick="imgSiguiente_Click" ImageUrl="~/Imagenes/Iconos/ico_siguiente.png" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
        <asp:PlaceHolder runat="server" ID="phTubero" Visible="false">
            <table class="repSam sinHover" cellpadding="0" cellspacing="0" width="100%">
                <colgroup>
                    <col width="30" /><!--icono borrar-->
                    <col width="30" /><!--icono cmts armado-->
                    <col width="90" /><!--#ctrl-->
                    <col width="35" /><!--etiqueta-->
                    <col width="50" /><!--Pdis-->
                    <col width="40" /><!--T.J.-->
                    <col width="45" /><!--Fam. Acero-->
                    <col width="70" /><!--destajo-->
                    <col width="70" /><!--cuadro-->
                    <col width="70" /><!--diasf-->
                    <col width="70" /><!--otros-->
                    <col width="70" /><!--ajuste-->
                    <col width="70" /><!--total-->
                    <col /><!--icono cmts destajo-->
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="14"><asp:Literal runat="server" ID="litDetJuntasArmadas" meta:resourcekey="litDetJuntasArmadas" /></th>
                    </tr>
                    <tr class="repTitulos">
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th><asp:Label runat="server" ID="lblNumCtrl" meta:resourcekey="lblNumCtrl" /></th>
                        <th><asp:Label runat="server" ID="lblEt" meta:resourcekey="lblEt" /></th>
                        <th><asp:Label runat="server" ID="lblPdis" meta:resourcekey="lblPdis" /></th>
                        <th><asp:Label runat="server" ID="lblTj" meta:resourcekey="lblTj" /></th>
                        <th><asp:Label runat="server" ID="lblFa" meta:resourcekey="lblFa" /></th>
                        <th><asp:Label runat="server" ID="lblDestajo" meta:resourcekey="lblDestajo" /></th>
                        <th><asp:Label runat="server" ID="lblCuadro" meta:resourcekey="lblCuadro" /></th>
                        <th><asp:Label runat="server" ID="lblDiasF" meta:resourcekey="lblDiasF" /></th>
                        <th><asp:Label runat="server" ID="lblOtros" meta:resourcekey="lblOtros" /></th>
                        <th><asp:Label runat="server" ID="lblAjuste" meta:resourcekey="lblAjuste" /></th>
                        <th><asp:Label runat="server" ID="lblTotal" meta:resourcekey="lblTotal" /></th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="rpTubero" OnItemDataBound="rpTubero_OnItemDataBound" OnItemCommand="rpTubero_OnItemCommand">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <asp:ImageButton meta:resourcekey="lnkEliminar" runat="server" ID="lnkEliminarArmado" CommandName="eliminar" CommandArgument='<%#Eval("DestajoTuberoDetalleID")%>' OnClientClick="return Sam.Confirma(15);" ImageUrl="~/Imagenes/Iconos/borrar.png" />
                            </td>
                            <td><asp:HyperLink meta:resourcekey="hlCmtsVerArmado" runat="server" ID="hlCmtProceso" ImageUrl="~/Imagenes/Iconos/info.png" Visible="false" /></td>
                            <td><%#Eval("NumeroControl")%></td>
                            <td class="ctr"><%#Eval("EtiquetaJunta")%></td>
                            <td class="rgt"><%#Eval("Diametro", "{0:#0.000}")%></td>
                            <td class="ctr"><%#Eval("TipoJunta")%></td>
                            <td class="ctr"><%#Eval("FamiliaAcero")%></td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionArmado" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionArmado" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Label runat="server" ID="colDestajo" CssClass="colDestajo" Text='<%#Eval("Destajo","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colCuadro" CssClass="colCuadro" Text='<%#Eval("ProrrateoCuadro","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colDiasF" CssClass="colDiasF" Text='<%#Eval("ProrrateoDiasFestivos","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colOtros" CssClass="colOtros" Text='<%#Eval("ProrrateoOtros","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAjuste" Text='<%#Eval("Ajuste","{0:C}")%>' MaxLength="10" CssClass="grdMoneda ajuste" />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colTotal" CssClass="colTotal" Text='<%#Eval("Total","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:HyperLink meta:resourcekey="hlCmtsPartida" runat="server" ID="hlCmtsDestajo" ImageUrl="~/Imagenes/Iconos/editar.png" />
                                <asp:HiddenField runat="server" ID="hdnDetalleID" Value='<%#Eval("DestajoTuberoDetalleID")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiametro" Value='<%#Eval("Diametro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajo" Value='<%#Eval("Destajo")%>' />
                                <asp:HiddenField runat="server" ID="hdnCuadro" Value='<%#Eval("ProrrateoCuadro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiasF" Value='<%#Eval("ProrrateoDiasFestivos")%>' />
                                <asp:HiddenField runat="server" ID="hdnOtros" Value='<%#Eval("ProrrateoOtros")%>' />
                                <asp:HiddenField runat="server" ID="hdnTotal" Value='<%#Eval("Total")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsArmado" Value='<%#Eval("ComentariosArmado")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsDestajo" Value='<%#Eval("ComentariosDestajo")%>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:ImageButton meta:resourcekey="lnkEliminar"  runat="server" ID="lnkEliminarArmado" CommandName="eliminar" CommandArgument='<%#Eval("DestajoTuberoDetalleID")%>' OnClientClick="return Sam.Confirma(15);" ImageUrl="~/Imagenes/Iconos/borrar.png" />
                            </td>
                            <td><asp:HyperLink meta:resourcekey="hlCmtsVerArmado" runat="server" ID="hlCmtProceso" ImageUrl="~/Imagenes/Iconos/info.png" Visible="false" /></td>
                            <td><%#Eval("NumeroControl")%></td>
                            <td class="ctr"><%#Eval("EtiquetaJunta")%></td>
                            <td class="rgt"><%#Eval("Diametro", "{0:#0.000}")%></td>
                            <td class="ctr"><%#Eval("TipoJunta")%></td>
                            <td class="ctr"><%#Eval("FamiliaAcero")%></td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionArmado" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionArmado" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Label runat="server" ID="colDestajo" CssClass="colDestajo" Text='<%#Eval("Destajo","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colCuadro" CssClass="colCuadro" Text='<%#Eval("ProrrateoCuadro","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colDiasF" CssClass="colDiasF" Text='<%#Eval("ProrrateoDiasFestivos","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colOtros" CssClass="colOtros" Text='<%#Eval("ProrrateoOtros","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAjuste" Text='<%#Eval("Ajuste","{0:C}")%>' MaxLength="10" CssClass="grdMoneda ajuste" />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colTotal" CssClass="colTotal" Text='<%#Eval("Total","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:HyperLink meta:resourcekey="hlCmtsPartida" runat="server" ID="hlCmtsDestajo" ImageUrl="~/Imagenes/Iconos/editar.png" />
                                <asp:HiddenField runat="server" ID="hdnDetalleID" Value='<%#Eval("DestajoTuberoDetalleID")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiametro" Value='<%#Eval("Diametro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajo" Value='<%#Eval("Destajo")%>' />
                                <asp:HiddenField runat="server" ID="hdnCuadro" Value='<%#Eval("ProrrateoCuadro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiasF" Value='<%#Eval("ProrrateoDiasFestivos")%>' />
                                <asp:HiddenField runat="server" ID="hdnOtros" Value='<%#Eval("ProrrateoOtros")%>' />
                                <asp:HiddenField runat="server" ID="hdnTotal" Value='<%#Eval("Total")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsArmado" Value='<%#Eval("ComentariosArmado")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsDestajo" Value='<%#Eval("ComentariosDestajo")%>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>            
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="7">&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblTotalDestajoT" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalCuadroT" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalDiasFT" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalOtrosT" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalAjusteT" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblGranTotalT" ClientIDMode="Static" /></td>
                        <td>&nbsp;</td>
                    </tr>
                </tfoot>
            </table>
            <div class="pieDestajo">
                <div class="totales">
                    <mimo:LabeledTextBox meta:resourcekey="txtDestajo" runat="server" ID="txtDestajo" ReadOnly="true" CssTextBox="moneda" />
                    <mimo:LabeledTextBox meta:resourcekey="txtCuadro" runat="server" ID="txtCuadro" CssTextBox="moneda" MaxLength="10" />
                    <mimo:LabeledTextBox meta:resourcekey="txtOtros" runat="server" ID="txtOtros" CssTextBox="ajuste" MaxLength="10" />
                    <mimo:LabeledTextBox meta:resourcekey="txtDiasFestivos" runat="server" ID="txtDiasFestivos" ReadOnly="true" CssTextBox="moneda" />
                    <mimo:LabeledTextBox meta:resourcekey="txtAjuste" runat="server" ID="txtAjuste" ReadOnly="true" CssTextBox="ajuste" />
                    <mimo:LabeledTextBox meta:resourcekey="txtTotal" runat="server" ID="txtTotal" ReadOnly="true" CssTextBox="moneda" />
                </div>
                <div class="cmts">
                    <div class="diasF">
                        <mimo:LabeledTextBox meta:resourcekey="txtCantidadDiasF" runat="server" ID="txtCantidadDiasF" CssTextBox="entero" MaxLength="1" />
                        <mimo:LabeledTextBox meta:resourcekey="txtCostoDiaF" runat="server" ID="txtCostoDiaF" CssTextBox="moneda"  MaxLength="10"/>
                    </div>
                    <div class="comentarios">
                        <asp:Label meta:resourcekey="lblComentarios" runat="server" ID="lblComentarios" AssociatedControlID="txtComentariosTubero" />
                        <asp:TextBox runat="server" ID="txtComentariosTubero" Rows="4" Columns="50" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="errores">
                    <div class="validacionesRecuadro">
                        <div class="validacionesHeader">&nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phSoldador" Visible="false">
            <table class="repSam sinHover" cellpadding="0" cellspacing="0" width="100%">
                <colgroup>
                    <col width="30" /><!--icono borrar-->
                    <col width="30" /><!--icono cmts soldadura-->
                    <col width="90" /><!--#ctrl-->
                    <col width="35" /><!--etiqueta-->
                    <col width="50" /><!--Pdis-->
                    <col width="40" /><!--T.J.-->
                    <col width="45" /><!--Fam. Acero-->
                    <col width="90" /><!--destajo raiz-->
                    <col width="90" /><!--destajo relleno-->
                    <col width="70" /><!--cuadro-->
                    <col width="70" /><!--diasf-->
                    <col width="70" /><!--otros-->
                    <col width="70" /><!--ajuste-->
                    <col width="70" /><!--total-->
                    <col /><!--icono cmts destajo-->
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="15"><asp:Literal runat="server" ID="litDetJuntasSoldadas" meta:resourcekey="litDetJuntasSoldadas" /></th>
                    </tr>
                    <tr class="repTitulos">
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th><asp:Label runat="server" meta:resourcekey="lblNumCtrl" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblEt" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblPdis" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblTj" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblFa" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblDestajoRaiz" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblDestajoRelleno" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblCuadro" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblDiasF" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblOtros" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblAjuste" /></th>
                        <th><asp:Label runat="server" meta:resourcekey="lblTotal" /></th>
                        <th>&nbsp;</th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="rpSoldadores" OnItemDataBound="rpSoldadores_OnItemDataBound" OnItemCommand="rpSoldadores_OnItemCommand">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <asp:ImageButton meta:resourcekey="lnkEliminar"  runat="server" ID="lnkElminarSoldadura" CommandName="eliminar" CommandArgument='<%#Eval("DestajoSoldadorDetalleID")%>' OnClientClick="return Sam.Confirma(15);" ImageUrl="~/Imagenes/Iconos/borrar.png" />
                            </td>
                            <td><asp:HyperLink meta:resourcekey="hlVerCmtsSoldadura" runat="server" ID="hlCmtProceso" ImageUrl="~/Imagenes/Iconos/info.png" Visible="false" /></td>
                            <td><%#Eval("NumeroControl")%></td>
                            <td class="ctr"><%#Eval("EtiquetaJunta")%></td>
                            <td class="rgt"><%#Eval("Diametro", "{0:#0.000}")%></td>
                            <td class="ctr"><%#Eval("TipoJunta")%></td>
                            <td class="ctr"><%#Eval("FamiliaAcero")%></td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionRaiz" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionRaiz" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Image runat="server" ID="imgRaizEquipo" ImageUrl="~/Imagenes/Iconos/ico_equipo.png" Visible="false" />
                                <asp:Label runat="server" ID="colDestajoRaiz" CssClass="colDestajoRaiz" Text='<%#Eval("DestajoRaiz","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionRelleno" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionRelleno" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Image runat="server" ID="imgRellenoEquipo" ImageUrl="~/Imagenes/Iconos/ico_soldadoenconjunto.png" Visible="false" />
                                <asp:Label runat="server" ID="colDestajoRelleno" CssClass="colDestajoRelleno" Text='<%#Eval("DestajoRelleno","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colCuadro" CssClass="colCuadro" Text='<%#Eval("ProrrateoCuadro","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colDiasF" CssClass="colDiasF" Text='<%#Eval("ProrrateoDiasFestivos","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colOtros" CssClass="colOtros" Text='<%#Eval("ProrrateoOtros","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAjuste" Text='<%#Eval("Ajuste","{0:C}")%>' MaxLength="10" CssClass="grdMoneda ajuste" />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colTotal" CssClass="colTotal" Text='<%#Eval("Total","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:HyperLink meta:resourcekey="hlCmtsPartida" runat="server" ID="hlCmtsDestajo" ImageUrl="~/Imagenes/Iconos/editar.png" />
                                <asp:HiddenField runat="server" ID="hdnDetalleID" Value='<%#Eval("DestajoSoldadorDetalleID")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiametro" Value='<%#Eval("Diametro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajoRaiz" Value='<%#Eval("DestajoRaiz")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajoRelleno" Value='<%#Eval("DestajoRelleno")%>' />
                                <asp:HiddenField runat="server" ID="hdnCuadro" Value='<%#Eval("ProrrateoCuadro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiasF" Value='<%#Eval("ProrrateoDiasFestivos")%>' />
                                <asp:HiddenField runat="server" ID="hdnOtros" Value='<%#Eval("ProrrateoOtros")%>' />
                                <asp:HiddenField runat="server" ID="hdnTotal" Value='<%#Eval("Total")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsSoldadura" Value='<%#Eval("ComentariosSoldadura")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsDestajo" Value='<%#Eval("ComentariosDestajo")%>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:ImageButton meta:resourcekey="lnkEliminar" runat="server" ID="lnkElminarSoldadura" CommandName="eliminar" CommandArgument='<%#Eval("DestajoSoldadorDetalleID")%>' OnClientClick="return Sam.Confirma(15);" ImageUrl="~/Imagenes/Iconos/borrar.png" />
                            </td>
                            <td><asp:HyperLink meta:resourcekey="hlVerCmtsSoldadura" runat="server" ID="hlCmtProceso" ImageUrl="~/Imagenes/Iconos/info.png" Visible="false" /></td>
                            <td><%#Eval("NumeroControl")%></td>
                            <td class="ctr"><%#Eval("EtiquetaJunta")%></td>
                            <td class="rgt"><%#Eval("Diametro", "{0:#0.000}")%></td>
                            <td class="ctr"><%#Eval("TipoJunta")%></td>
                            <td class="ctr"><%#Eval("FamiliaAcero")%></td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionRaiz" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionRaiz" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Image runat="server" ID="imgRaizEquipo" ImageUrl="~/Imagenes/Iconos/ico_equipo.png" Visible="false" />
                                <asp:Label runat="server" ID="colDestajoRaiz" CssClass="colDestajoRaiz" Text='<%#Eval("DestajoRaiz","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Image runat="server" ID="imgExclamacionRelleno" CssClass="exclamacion" Visible="false" meta:resourcekey="imgExclamacionRelleno" ImageUrl="~/Imagenes/Iconos/ico_admiracion.png" />
                                <asp:Image runat="server" ID="imgRellenoEquipo" ImageUrl="~/Imagenes/Iconos/ico_equipo.png" Visible="false" />
                                <asp:Label runat="server" ID="colDestajoRelleno" CssClass="colDestajoRelleno" Text='<%#Eval("DestajoRelleno","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colCuadro" CssClass="colCuadro" Text='<%#Eval("ProrrateoCuadro","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colDiasF" CssClass="colDiasF" Text='<%#Eval("ProrrateoDiasFestivos","{0:C}")%>' />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colOtros" CssClass="colOtros" Text='<%#Eval("ProrrateoOtros","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAjuste" Text='<%#Eval("Ajuste","{0:C}")%>' MaxLength="10" CssClass="grdMoneda ajuste" />
                            </td>
                            <td class="rgt">
                                <asp:Label runat="server" ID="colTotal" CssClass="colTotal" Text='<%#Eval("Total","{0:C}")%>' />
                            </td>
                            <td>
                                <asp:HyperLink meta:resourcekey="hlCmtsPartida" runat="server" ID="hlCmtsDestajo" ImageUrl="~/Imagenes/Iconos/editar.png" />
                                <asp:HiddenField runat="server" ID="hdnDetalleID" Value='<%#Eval("DestajoSoldadorDetalleID")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiametro" Value='<%#Eval("Diametro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajoRaiz" Value='<%#Eval("DestajoRaiz")%>' />
                                <asp:HiddenField runat="server" ID="hdnDestajoRelleno" Value='<%#Eval("DestajoRelleno")%>' />
                                <asp:HiddenField runat="server" ID="hdnCuadro" Value='<%#Eval("ProrrateoCuadro")%>' />
                                <asp:HiddenField runat="server" ID="hdnDiasF" Value='<%#Eval("ProrrateoDiasFestivos")%>' />
                                <asp:HiddenField runat="server" ID="hdnOtros" Value='<%#Eval("ProrrateoOtros")%>' />
                                <asp:HiddenField runat="server" ID="hdnTotal" Value='<%#Eval("Total")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsSoldadura" Value='<%#Eval("ComentariosSoldadura")%>' />
                                <asp:HiddenField runat="server" ID="hdnCmtsDestajo" Value='<%#Eval("ComentariosDestajo")%>' />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>            
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="7">&nbsp;</td>
                        <td><asp:Label runat="server" ID="lblTotalDestajoRaizS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalDestajoRellenoS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalCuadroS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalDiasFS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalOtrosS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblTotalAjusteS" ClientIDMode="Static" /></td>
                        <td><asp:Label runat="server" ID="lblGranTotalS" ClientIDMode="Static" /></td>
                        <td>&nbsp;</td>
                    </tr>
                </tfoot>
            </table>
            <div class="pieDestajo">
                <div class="totales">
                    <mimo:LabeledTextBox meta:resourcekey="txtDestajoRaiz" runat="server" ID="txtDestajoRaiz" ReadOnly="true" CssTextBox="moneda" />
                    <mimo:LabeledTextBox meta:resourcekey="txtDestajoRelleno" runat="server" ID="txtDestajoRelleno" ReadOnly="true" CssTextBox="moneda" />
                    <mimo:LabeledTextBox meta:resourcekey="txtCuadro" runat="server" ID="txtCuadroS" CssTextBox="moneda" MaxLength="10" />
                    <mimo:LabeledTextBox meta:resourcekey="txtOtros" runat="server" ID="txtOtrosS" CssTextBox="ajuste" MaxLength="10" />
                    <mimo:LabeledTextBox meta:resourcekey="txtDiasFestivos" runat="server" ID="txtDiasFestivosS" ReadOnly="true" CssTextBox="moneda" />
                    <mimo:LabeledTextBox meta:resourcekey="txtAjuste" runat="server" ID="txtAjusteS" ReadOnly="true" CssTextBox="ajuste" />
                    <mimo:LabeledTextBox meta:resourcekey="txtTotal" runat="server" ID="txtTotalS" ReadOnly="true" CssTextBox="moneda" />
                </div>
                <div class="cmts">
                    <div class="diasF">
                        <mimo:LabeledTextBox meta:resourcekey="txtCantidadDiasF" runat="server" ID="txtCantidadDiasFS" CssTextBox="entero" MaxLength="1" />
                        <mimo:LabeledTextBox meta:resourcekey="txtCostoDiaF" runat="server" ID="txtCostoDiaFS" CssTextBox="moneda"  MaxLength="10"/>
                    </div>
                    <div class="comentarios">
                        <asp:Label meta:resourcekey="lblComentarios" runat="server" ID="Label13" AssociatedControlID="txtComentariosSoldador" />
                        <asp:TextBox runat="server" ID="txtComentariosSoldador" Rows="4" Columns="50" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="errores">
                    <div class="validacionesRecuadro">
                        <div class="validacionesHeader">&nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummaryS" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                        </div>
                    </div>
                </div>
            </div>        
        </asp:PlaceHolder>
    </div>
    <div class="pestanaBotonLarga">
        <asp:Button CssClass="boton" ID="btnAprobar" runat="server" meta:resourcekey="btnAprobar" OnClick="btnAprobar_Click" />
        <asp:Button CssClass="boton" ID="btnCancelarAprobacion" runat="server" meta:resourcekey="btnCancelarAprobacion" OnClick="btnCancelarAprobacion_Click" />
    </div>
</asp:Content>
<asp:Content runat="server" ID="cntFooter" ContentPlaceHolderID="cphInnerFoot">
    <script language="javascript" type="text/javascript" src="/Scripts/autoNumeric-1.6.2.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            Sam.Destajo.Inicializa();
        });
    </script>
</asp:Content>
