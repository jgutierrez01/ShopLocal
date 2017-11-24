<%@ Page Language="C#" MasterPageFile="~/Masters/WorkStatus.Master" AutoEventWireup="true"
    CodeBehind="WkStDefault.aspx.cs" Inherits="SAM.Web.WorkStatus.WkStDefault" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <div class="headerDashboard">
        <asp:Label runat="server" ID="Label1" meta:resourcekey="lblDashboard" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                    <asp:RectangleHotSpot Top="11" Left="154" Bottom="66" Right="296" NavigateUrl="/Catalogos/LstProyecto.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Top="27" Left="363" Bottom="58" Right="444" NavigateUrl="/Administracion/AdminDefault.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="11" Top="115" Right="95" Bottom="150" NavigateUrl="/Ingenieria/IngDefault.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="114" Top="108" Right="219" Bottom="146" NavigateUrl="/Ingenieria/ImportacionDatos.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="238" Top="108" Right="341" Bottom="147" NavigateUrl="/Materiales/NuevaRecepcion.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="363" Top="114" Right="445" Bottom="145" NavigateUrl="/Materiales/MatDefault.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="41" Top="186" Right="123" Bottom="219" NavigateUrl="/Produccion/ProdDefault.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="183" Right="299" Bottom="208" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="153" Top="224" Right="300" Bottom="250" NavigateUrl="/Produccion/LstOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="155" Top="264" Right="299" Bottom="289" NavigateUrl="/WorkStatus/TransferenciaCorte.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="304" Right="299" Bottom="327" NavigateUrl="/WorkStatus/NuevoCorte.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="343" Right="299" Bottom="372" NavigateUrl="/WorkStatus/Despacho.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="385" Right="300" Bottom="411" NavigateUrl="/WorkStatus/LstArmado.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="155" Top="423" Right="301" Bottom="447" NavigateUrl="/WorkStatus/LstSoldadura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="464" Right="245" Bottom="509" NavigateUrl="/WorkStatus/LstInspeccionVisual.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="244" Top="465" Right="299" Bottom="511" NavigateUrl="/WorkStatus/LiberacionesVisualesPatio.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="153" Top="525" Right="243" Bottom="575" NavigateUrl="/WorkStatus/LstInspeccionDimensional.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="243" Top="526" Right="298" Bottom="578" NavigateUrl="/WorkStatus/LiberacionesDimensionalesPatio.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="590" Right="300" Bottom="616" NavigateUrl="/WorkStatus/LstEmbarque.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="155" Top="628" Right="300" Bottom="653" NavigateUrl="/WorkStatus/LstEmbarque.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="154" Top="666" Right="300" Bottom="691" NavigateUrl="/WorkStatus/LstEmbarque.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="91" Top="260" Right="111" Bottom="289" NavigateUrl="/WorkStatus/NumerosUnicosEnCorte.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="90" Top="302" Right="112" Bottom="332" NavigateUrl="/WorkStatus/LstCortes.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="92" Top="342" Right="113" Bottom="373" NavigateUrl="/WorkStatus/LstDespacho.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="87" Top="398" Right="113" Bottom="432" NavigateUrl="/WorkStatus/DetReporteArmadoSoldadura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="88" Top="469" Right="113" Bottom="504" NavigateUrl="/WorkStatus/RepInspeccionVisual.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="88" Top="535" Right="112" Bottom="569" NavigateUrl="/WorkStatus/RepInspeccionDimensional.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="624" Top="270" Right="728" Bottom="308" NavigateUrl="/Workstatus/LstRequisicionesPruebas.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="745" Top="270" Right="848" Bottom="308" NavigateUrl="/WorkStatus/LstPnd.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="622" Top="404" Right="724" Bottom="440" NavigateUrl="/WorkStatus/LstRequisicionesPruebas.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="740" Top="403" Right="845" Bottom="441" NavigateUrl="/WorkStatus/LstTT.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="623" Top="534" Right="726" Bottom="572" NavigateUrl="/WorkStatus/LstReqPintura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="743" Top="534" Right="845" Bottom="571" NavigateUrl="/WorkStatus/LstPintura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="625" Top="641" Right="708" Bottom="670" NavigateUrl="/Calidad/CalidadDefault.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="661" Top="219" Right="687" Bottom="250" NavigateUrl="/WorkStatus/RepRequisiciones.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="781" Top="218" Right="805" Bottom="253" NavigateUrl="/WorkStatus/ReportePND.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="660" Top="347" Right="685" Bottom="382" NavigateUrl="/WorkStatus/RepRequisiciones.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="779" Top="349" Right="804" Bottom="381" NavigateUrl="/WorkStatus/ReporteTt.aspx" HotSpotMode="Navigate" />

                    <asp:RectangleHotSpot Left="661" Top="110" Right="687" Bottom="141" NavigateUrl="/WorkStatus/RepRequisicionesSpool.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="781" Top="110" Right="805" Bottom="141" NavigateUrl="/WorkStatus/ReporteSpoolPND.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="622" Top="150" Right="724" Bottom="185" NavigateUrl="/WorkStatus/LstRequisicionesSpoolPruebas.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="745" Top="150" Right="848" Bottom="185" NavigateUrl="/WorkStatus/LstSpoolPnd.aspx" HotSpotMode="Navigate" />

                    <asp:RectangleHotSpot Left="662" Top="480" Right="685" Bottom="512" NavigateUrl="/WorkStatus/ReporteRequisicionesPintura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="780" Top="479" Right="803" Bottom="513" NavigateUrl="/WorkStatus/LstPintura.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="791" Top="11" Right="889" Bottom="47" NavigateUrl="/Calidad/FiltrosSeguimientoJunta.aspx" HotSpotMode="Navigate" />
                    <asp:RectangleHotSpot Left="792" Top="56" Right="889" Bottom="95" NavigateUrl="/Calidad/FiltrosSeguimientoSpool.aspx" HotSpotMode="Navigate" />
                     <asp:CircleHotSpot X="303" Y="317" Radius="14" NavigateUrl="#catalogos" />
                         <asp:CircleHotSpot X="303" Y="395" Radius="14" NavigateUrl="#catalogos" />
                         <asp:CircleHotSpot X="303" Y="438" Radius="14" NavigateUrl="#catalogos" />
                         <asp:CircleHotSpot X="303" Y="469" Radius="14" NavigateUrl="#catalogos" />
                         <asp:CircleHotSpot X="842" Y="273" Radius="14" NavigateUrl="#catalogos" />
                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblWorkstatus" Text="Workstatus" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">
                    <div class="divIzquierdo bordeDerecho">
                        <table>
                            <tr>
                                <td rowspan="5" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_1.png" alt="" name="catalogos" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catLocalizacion" NavigateUrl="~/Catalogos/LstPatio.aspx"
                                        meta:resourcekey="catLocalizacion" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catMaquinaCorte" NavigateUrl="~/Catalogos/LstPatio.aspx"
                                        meta:resourcekey="catMaquinaCorte" />
                                </td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>&nbsp;</td></tr>
                        </table>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <table>
                            <tr>
                                <td rowspan="2" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_2.png" alt="" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catTubero" NavigateUrl="~/Catalogos/LstTubero.aspx"
                                        meta:resourcekey="catTubero" />
                                </td>
                            </tr>   
                             <tr><td>&nbsp;</td></tr>
                              <tr><td>&nbsp;</td></tr>
                               <tr><td>&nbsp;</td></tr>
                                <tr><td>&nbsp;</td></tr>                         
                        </table>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <table>
                            <tr>
                                <td rowspan="5" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_3.png" alt="" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catSoldador" NavigateUrl="~/Catalogos/LstSoldador.aspx"
                                        meta:resourcekey="catSoldador" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catMaterialSoldadura" NavigateUrl="~/Catalogos/LstConsumibles.aspx"
                                        meta:resourcekey="catMaterialSoldadura" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catProcesoRaiz" NavigateUrl="~/Catalogos/LstProcesoRaiz.aspx"
                                        meta:resourcekey="catProcesoRaiz" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catProcesoRelleno" NavigateUrl="~/Catalogos/LstProcesoRelleno.aspx"
                                        meta:resourcekey="catProcesoRelleno" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catWPS" NavigateUrl="~/Catalogos/LstWps.aspx"
                                        meta:resourcekey="catWPS" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divIzquierdo special">
                     <table>
                            <tr>
                                <td rowspan="2" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_4.png" alt="" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catDefectos" NavigateUrl="~/Catalogos/LstDefectos.aspx"
                                        meta:resourcekey="catDefectos" />
                                </td>
                            </tr>   
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr><td>&nbsp;</td></tr>                         
                        </table>
                    </div>
                    <p>
                    </p>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
