<%@ Page  Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true"
    CodeBehind="CalidadDefault.aspx.cs" Inherits="SAM.Web.Calidad.CalidadDefault" %>
<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">



    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblDashboard" Text="Dashboard" />
    </div>
     <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">       
                  <%--  <area shape="rect" coords="155,16 ,299,70" href="#" />
                        <area shape="rect" coords="361,29 ,443,61" href="#" />
                        <area shape="rect" coords="11 ,117,94 ,147" href="#" />
                        <area shape="rect" coords="112,113,217,151" href="#" />
                        <area shape="rect" coords="237,113,341,147" href="#" />
                        <area shape="rect" coords="362,118,447,147" href="#" />
                        <area shape="rect" coords="149,187,307,212" href="#" />
                        <area shape="rect" coords="44 ,239,131,268" href="#" />
                        <area shape="rect" coords="154,240,300,266" href="#" />
                        <area shape="rect" coords="153,281,301,309" href="#" />
                        <area shape="rect" coords="154,318,299,354" href="#" />
                        <area shape="rect" coords="185,369,265,399" href="#" />
                        <area shape="rect" coords="583,191,686,229" href="#" />
                        <area shape="rect" coords="621,261,644,296" href="#" />
                        <area shape="rect" coords="583,316,684,352" href="#" />
                        <area shape="rect" coords="792,7  ,888,47" href="#" />
                        <area shape="rect" coords="793,56 ,889,96" href="#" />--%>   
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                        <%--1 Creación y Configuración de Proyecto--%>
                        <asp:RectangleHotSpot Left="155" Top="16"   Right="299" Bottom="70" NavigateUrl="/Catalogos/LstProyecto.aspx" HotSpotMode="Navigate" />
                        <%--2 Administracion--%>
                        <asp:RectangleHotSpot Left="361" Top="29"   Right="443" Bottom="61" NavigateUrl="/Administracion/AdminDefault.aspx" HotSpotMode="Navigate" />
                        <%--3 Ingeniería--%>
                        <asp:RectangleHotSpot Left="11"  Top="117"  Right="94"  Bottom="147" NavigateUrl="/Ingenieria/IngDefault.aspx" HotSpotMode="Navigate" />
                        <%--4 Importación de Ingenierías--%>
                        <asp:RectangleHotSpot Left="112" Top="113"  Right="217" Bottom="151" NavigateUrl="/Ingenieria/ImportacionDatos.aspx" HotSpotMode="Navigate" />
                        <%--5 Recepción de Materiales--%>
                        <asp:RectangleHotSpot Left="237" Top="113"  Right="341" Bottom="147" NavigateUrl="/Materiales/LstRecepcion.aspx" HotSpotMode="Navigate" />
                        <%--6 Materiales--%>
                        <asp:RectangleHotSpot Left="362" Top="118"  Right="447" Bottom="147" NavigateUrl="/Materiales/MatDefault.aspx" HotSpotMode="Navigate" />
                        <%--7 Verificación de Catálogos--%>
                        <asp:RectangleHotSpot Left="149" Top="187"  Right="307" Bottom="212" NavigateUrl="/Catalogos/CatDefault.aspx" HotSpotMode="Navigate" />
                        <%--8 Producción--%>
                        <asp:RectangleHotSpot Left="44"  Top="239"  Right="131" Bottom="268" NavigateUrl="/Produccion/ProdDefault.aspx" HotSpotMode="Navigate" />
                        <%--9 Cruce de Materiales--%>
                        <asp:RectangleHotSpot Left="154" Top="240"  Right="300" Bottom="266" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                        <%--10 Orden de Trabajo--%>
                        <asp:RectangleHotSpot Left="153" Top="281"  Right="301" Bottom="309" NavigateUrl="/Produccion/LstOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                        <%--11 IV / LD / Requisiciones / Reportes X --%>
                        <asp:RectangleHotSpot Left="154" Top="318"  Right="299" Bottom="354" NavigateUrl="/WorkStatus/WkStDefault.aspx" HotSpotMode="Navigate" />
                        <%--12 Workstatus--%>
                        <asp:RectangleHotSpot Left="185" Top="369"  Right="265" Bottom="399" NavigateUrl="/WorkStatus/WkStDefault.aspx" HotSpotMode="Navigate" />
                        <%--13 Hold Calidad--%>
                        <asp:RectangleHotSpot Left="583" Top="191"  Right="686" Bottom="229" NavigateUrl="/Calidad/RevisionSpools.aspx" HotSpotMode="Navigate" />
                        <%--14 Reporte de Traceabilidad--%>
                        <asp:RectangleHotSpot Left="621" Top="261"  Right="644" Bottom="296" NavigateUrl="/Calidad/CertificacionLigero.aspx" HotSpotMode="Navigate" />
                        <%--15 Certificación--%>
                        <asp:RectangleHotSpot Left="583" Top="316"  Right="684" Bottom="352" NavigateUrl="/Calidad/CertificacionLigero.aspx" HotSpotMode="Navigate" />
                        <%--16 Seguimiento de Juntas--%>
                        <asp:RectangleHotSpot Left="792" Top="7"    Right="888" Bottom="47" NavigateUrl="/Calidad/FiltrosSeguimientoJunta.aspx" HotSpotMode="Navigate" />
                        <%--17 Seguimiento de Spools--%>
                        <asp:RectangleHotSpot Left="793" Top="56"   Right="889" Bottom="96" NavigateUrl="/Calidad/FiltrosSeguimientoSpool.aspx" HotSpotMode="Navigate" />
                        
                        <asp:CircleHotSpot X="147" Y="187" Radius="14" NavigateUrl="#catalogos" />
                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblCalidad" meta:resourcekey="lblCalidad" Text="Calidad" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">
                    <div class="divIzquierdo">
                        <table>
                            <tr>
                                <td rowspan="3" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_1.png" alt="" name="catalogos" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catAcero" NavigateUrl="~/Catalogos/LstAcero.aspx"
                                        meta:resourcekey="catAcero" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catCedula" NavigateUrl="~/Catalogos/LstCedula.aspx"
                                        meta:resourcekey="catCedula" />
                                </td>
                            </tr>
                            <tr><td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catDiametro" NavigateUrl="~/Catalogos/LstDiametro.aspx"
                                        meta:resourcekey="catDiametro" />
                                </td></tr>                           
                        </table>
                    </div>                  
                    <p>
                    </p>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
