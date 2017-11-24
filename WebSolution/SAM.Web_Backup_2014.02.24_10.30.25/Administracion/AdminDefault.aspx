<%@ Page Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true"
    CodeBehind="AdminDefault.aspx.cs" Inherits="SAM.Web.Administracion.AdminDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblDashboard" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">
                <%--<area shape="rect" coords="59 ,50 ,205,76" href="#" />
                    <area shape="rect" coords="59 ,92 ,205,116" href="#" />
                    <area shape="rect" coords="58 ,129,204,155" href="#" />
                    <area shape="rect" coords="59 ,168,203,193" href="#" />
                    <area shape="rect" coords="59 ,207,205,236" href="#" />
                    <area shape="rect" coords="59 ,249,205,274" href="#" />
                    <area shape="rect" coords="245,205,350,241" href="#" />
                    <area shape="rect" coords="523,202,625,240" href="#" />
                    <area shape="rect" coords="644,203,744,239" href="#" />
                    <area shape="rect" coords="491,282,596,320" href="#" />
                    <area shape="rect" coords="493,362,591,398" href="#" />
                    <area shape="rect" coords="650,325,672,360" href="#" />
                    <area shape="rect" coords="683,136,707,167" href="#" />
                    <area shape="rect" coords="745,135,769,170" href="#" />--%>
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                       <%--1 Perfiles--%>
                       <asp:RectangleHotSpot Left="59"  Top="50"  Right="205" Bottom="76"   NavigateUrl="/Administracion/LstPerfiles.aspx" HotSpotMode="Navigate" />
                       <%--2 Usuarios--%>
                       <asp:RectangleHotSpot Left="59"  Top="92"  Right="205" Bottom="116"  NavigateUrl="/Administracion/LstUsuario.aspx" HotSpotMode="Navigate" />
                       <%--3 Patio--%>
                       <asp:RectangleHotSpot Left="58"  Top="129" Right="204" Bottom="155"  NavigateUrl="/Catalogos/LstPatio.aspx" HotSpotMode="Navigate" />
                       <%--4 Proyecto--%>
                       <asp:RectangleHotSpot Left="59"  Top="168" Right="203" Bottom="193"  NavigateUrl="/Catalogos/LstProyecto.aspx" HotSpotMode="Navigate" />
                       <%--5 Destajo--%>
                       <asp:RectangleHotSpot Left="59"  Top="207" Right="205" Bottom="236"  NavigateUrl="/Administracion/LstPeriodosDestajo.aspx" HotSpotMode="Navigate" />
                       <%--6 Estimación--%>
                       <asp:RectangleHotSpot Left="59"  Top="249" Right="205" Bottom="274"  NavigateUrl="/Administracion/LstEstimado.aspx" HotSpotMode="Navigate" />
                       <%--7 Tablas de Destajo--%>
                       <asp:RectangleHotSpot Left="245" Top="205" Right="350" Bottom="241"  NavigateUrl="/Administracion/TblDestajos.aspx" HotSpotMode="Navigate" />
                       <%--8 Alta de Período--%>
                       <asp:RectangleHotSpot Left="523" Top="202" Right="625" Bottom="240"  NavigateUrl="/Administracion/AltaPeriodoDestajo.aspx" HotSpotMode="Navigate" />
                       <%--9 Cálculo de Destajos--%>
                       <asp:RectangleHotSpot Left="644" Top="203" Right="744" Bottom="239"  NavigateUrl="/Administracion/LstPeriodosDestajo.aspx" HotSpotMode="Navigate" />
                       <%--10 Estimación de Juntas--%>
                       <asp:RectangleHotSpot Left="491" Top="282" Right="596" Bottom="320"  NavigateUrl="/Administracion/EstimacionDeJunta.aspx" HotSpotMode="Navigate" />
                       <%--11 Estimación de Spool--%>
                       <asp:RectangleHotSpot Left="493" Top="362" Right="591" Bottom="398"  NavigateUrl="/Administracion/EstimacionDeSpool.aspx" HotSpotMode="Navigate" />
                       <%--12 Reporte de Estimaciones--%>
                       <asp:RectangleHotSpot Left="650" Top="325" Right="672" Bottom="360"  NavigateUrl="/Administracion/LstEstimado.aspx" HotSpotMode="Navigate" />
                       <%--13 Análisis de Destajo--%>
                       <asp:RectangleHotSpot Left="683" Top="136" Right="707" Bottom="167"  NavigateUrl="/Administracion/DetReporteDestajos.aspx" HotSpotMode="Navigate" />
                       <%--14 Reporte Individual de Destajo--%>
                       <asp:RectangleHotSpot Left="683" Top="76" Right="707" Bottom="107"  NavigateUrl="/Administracion/LstPeriodosDestajo.aspx" HotSpotMode="Navigate" />                       
                         <asp:CircleHotSpot X="60" Y="134" Radius="14" NavigateUrl="#catalogos" />

                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblCatalogos" meta:resourcekey="lblCatalogos" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">                  
                    <div class="divIzquierdo" >
                        <table>
                            <tr>
                                <td rowspan="3" valign="top">                                
                                    <img src="../Imagenes/Dashboards/ico_1.png" alt="" name="catalogos"/>
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catTalleres" NavigateUrl="~/Catalogos/LstPatio.aspx"
                                        meta:resourcekey="catTalleres" />
                                </td>
                            </tr>
                             <tr>
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
                        </table>
                    </div>
                    <p>
                    </p>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
