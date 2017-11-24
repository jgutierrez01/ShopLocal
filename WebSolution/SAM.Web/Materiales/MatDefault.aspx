<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="MatDefault.aspx.cs" Inherits="SAM.Web.Materiales.MatDefault" %>
    <%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">



    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblDashboard" Text="Dashboard" />
    </div>
     <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">                     
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                        <%--1 Listado de Recepciones--%>
                        <asp:RectangleHotSpot Left="73"  Top="18"    Right="97"  Bottom="50"  NavigateUrl="/Materiales/LstRecepcion.aspx" HotSpotMode="Navigate" />
                        <%--2 Listado de Materiales por Item Code--%>
                        <asp:RectangleHotSpot Left="75"  Top="61"    Right="97"  Bottom="92"  NavigateUrl="/Materiales/LstMatPorItemCode.aspx" HotSpotMode="Navigate" />
                        <%--3 Listado de Materiales por Número Unico--%>
                        <asp:RectangleHotSpot Left="73"  Top="101"   Right="96"  Bottom="132" NavigateUrl="/Materiales/LstNumeroUnico.aspx" HotSpotMode="Navigate" />
                        <%--4 Recepción de Materiales--%>
                        <asp:RectangleHotSpot Left="149" Top="65"    Right="293" Bottom="93"  NavigateUrl="/Materiales/NuevaRecepcion.aspx" HotSpotMode="Navigate" />
                        <%--5 Cruce de Materiales--%>
                        <asp:RectangleHotSpot Left="149" Top="105"   Right="295" Bottom="130" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                        <%--6 Orden de Trabajo--%>
                        <asp:RectangleHotSpot Left="147" Top="146"   Right="294" Bottom="171" NavigateUrl="/Produccion/LstOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                        <%--7 Despacho a Corte--%>
                        <asp:RectangleHotSpot Left="150" Top="186"   Right="292" Bottom="211" NavigateUrl="/WorkStatus/TransferenciaCorte.aspx" HotSpotMode="Navigate" />
                        <%--8 Workstatus--%>
                        <asp:RectangleHotSpot Left="177" Top="234"   Right="256" Bottom="264" NavigateUrl="/WorkStatus/WkStDefault.aspx" HotSpotMode="Navigate" />
                        <%--9 Producción--%>
                        <asp:RectangleHotSpot Left="328" Top="106"   Right="411" Bottom="139" NavigateUrl="/Produccion/ProdDefault.aspx" HotSpotMode="Navigate" />
                        <%--10 Reporte--%>
                        <asp:RectangleHotSpot Left="617" Top="84"    Right="644" Bottom="117" NavigateUrl="/Materiales/RepReqPinturaNumUnico.aspx" HotSpotMode="Navigate" />
                        <%--11 Requisiciones de Pintura--%>
                        <asp:RectangleHotSpot Left="578" Top="135"   Right="680" Bottom="175" NavigateUrl="/Materiales/ReqPinturaNumUnico.aspx" HotSpotMode="Navigate" />
                        <%--12 Pintura de Materiales--%>
                        <asp:RectangleHotSpot Left="698" Top="138"   Right="801" Bottom="173" NavigateUrl="/Materiales/LstPinturaNumUnico.aspx" HotSpotMode="Navigate" />
                        <%--13 Segmentar Tubo--%>
                        <asp:RectangleHotSpot Left="715" Top="248"   Right="816" Bottom="283" NavigateUrl="/Materiales/SegmentarTubo.aspx" HotSpotMode="Navigate" />
                        <%--14 Otros Movimientos a Inventario--%>
                        <asp:RectangleHotSpot Left="591" Top="306"   Right="694" Bottom="344" NavigateUrl="/Materiales/MovimientosInventario.aspx" HotSpotMode="Navigate" />
                        <%--15 Confinar Spool--%>
                        <asp:RectangleHotSpot Left="582" Top="368"   Right="682" Bottom="404" NavigateUrl="/Materiales/LstConfinarSpool.aspx" HotSpotMode="Navigate" />
                        <%--16 Seguimiento de Juntas--%>
                        <asp:RectangleHotSpot Left="792" Top="7"     Right="889" Bottom="44"  NavigateUrl="/Calidad/FiltrosSeguimientoJunta.aspx" HotSpotMode="Navigate" />
                        <%--17 Seguimiento de Spools--%>
                        <asp:RectangleHotSpot Left="792" Top="56"    Right="890" Bottom="93"  NavigateUrl="/Calidad/FiltrosSeguimientoSpool.aspx" HotSpotMode="Navigate" />                        
                         <asp:CircleHotSpot X="144" Y="61" Radius="14" NavigateUrl="#catalogos" />
                         
                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblMateriales" meta:resourcekey="lblMateriales" Text="Materiales" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
      <tr>
            <td>
                <div id="dashBoard">
                    <div class="divIzquierdo">
                        <table>
                            <tr>
                                <td rowspan="6" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_1.png" alt="" name="catalogos" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catAcero" NavigateUrl="~/Catalogos/LstAcero.aspx"
                                        meta:resourcekey="catAcero" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catFabricante" NavigateUrl="~/Catalogos/LstFabricantes.aspx"
                                        meta:resourcekey="catFabricante" />
                                </td>
                            </tr>
                            <tr><td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catFamAcero" NavigateUrl="~/Catalogos/LstFamAcero.aspx"
                                        meta:resourcekey="catFamAcero" />
                                </td></tr>
                            <tr><td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catFamMateriales" NavigateUrl="~/Catalogos/LstFamiliaMaterial.aspx"
                                        meta:resourcekey="catFamMateriales" />
                                </td></tr>
                            <tr><td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catProveedores" NavigateUrl="~/Catalogos/LstProveedor.aspx"
                                        meta:resourcekey="catProveedores" />
                                </td></tr>
                                 <tr><td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catTransportista" NavigateUrl="~/Catalogos/LstTransportistas.aspx"
                                        meta:resourcekey="catTransportista" />
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
