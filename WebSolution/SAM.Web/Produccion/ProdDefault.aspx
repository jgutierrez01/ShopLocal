<%@ Page  Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true"
    CodeBehind="ProdDefault.aspx.cs" Inherits="SAM.Web.Produccion.ProdDefault" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
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
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                        <%--1 Creación y Configuración de Proyecto--%>
                        <asp:RectangleHotSpot Left="154" Top="13"  Right="299" Bottom="66" NavigateUrl="/Catalogos/LstProyecto.aspx" HotSpotMode="Navigate" />
                        <%--2 Administracion--%>
                        <asp:RectangleHotSpot Left="362" Top="29"  Right="445" Bottom="61" NavigateUrl="/Administracion/AdminDefault.aspx" HotSpotMode="Navigate" />
                        <%--3 Ingeniería--%>
                        <asp:RectangleHotSpot Left="8"   Top="119" Right="94"  Bottom="145" NavigateUrl="/Ingenieria/IngDefault.aspx" HotSpotMode="Navigate" />
                        <%--4 Importación de Ingenierías--%>
                        <asp:RectangleHotSpot Left="115" Top="112" Right="217" Bottom="151" NavigateUrl="/Ingenieria/ImportacionDatos.aspx" HotSpotMode="Navigate" />
                        <%--5 Recepción de Materiales--%>
                        <asp:RectangleHotSpot Left="238" Top="111" Right="340" Bottom="150" NavigateUrl="/Materiales/LstRecepcion.aspx" HotSpotMode="Navigate" />
                        <%--6 Materiales--%>
                        <asp:RectangleHotSpot Left="362" Top="117" Right="443" Bottom="147" NavigateUrl="/Materiales/MatDefault.aspx" HotSpotMode="Navigate" />
                        <%--7 Congelado Parcial--%>
                        <asp:RectangleHotSpot Left="151" Top="190" Right="295" Bottom="216" NavigateUrl="/Produccion/CongeladoParcial.aspx" HotSpotMode="Navigate" />
                        <%--8 Orden de Trabajo--%>
                        <asp:RectangleHotSpot Left="155" Top="275" Right="299" Bottom="300" NavigateUrl="/Produccion/LstOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                        <%--8 Congelados Por Numero Unico--%>
                        <asp:RectangleHotSpot Left="90" Top="252" Right="113" Bottom="284" NavigateUrl="/Produccion/CongeladosNumeroUnico.aspx" HotSpotMode="Navigate" />
                        <%--8 Congelados por ODT--%>
                        <asp:RectangleHotSpot Left="92" Top="303" Right="114" Bottom="332" NavigateUrl="/Produccion/CongeladosOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                        <%--9 Workstatus--%>
                        <asp:RectangleHotSpot Left="177" Top="320" Right="258" Bottom="351" NavigateUrl="/WorkStatus/WkStDefault.aspx" HotSpotMode="Navigate" />
                        <%--10 Fijar Prioridad--%>
                        <asp:RectangleHotSpot Left="587" Top="148" Right="689" Bottom="187" NavigateUrl="/Produccion/FijarPrioridad.aspx" HotSpotMode="Navigate" />
                        <%--11 Cruce por Prioridad X --%>
                        <asp:RectangleHotSpot Left="707" Top="149" Right="810" Bottom="186" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                        <%--12 Cruce Forzado por Spool--%>
                        <asp:RectangleHotSpot Left="581" Top="235" Right="684" Bottom="275" NavigateUrl="/Produccion/NuevaOdt.aspx" HotSpotMode="Navigate" />
                        <%--13 Cruce Forzado con Asignación de Materiales--%>
                        <asp:RectangleHotSpot Left="584" Top="317" Right="737" Bottom="352" NavigateUrl="/Produccion/NuevaOdt.aspx" HotSpotMode="Navigate" />
                        <%--14 Corte de Junta--%>
                        <asp:RectangleHotSpot Left="586" Top="430" Right="690" Bottom="470" NavigateUrl="/Produccion/CorteJunta.aspx" HotSpotMode="Navigate" />
                        <%--15 Reporte de Faltantes--%>
                        <asp:RectangleHotSpot Left="742" Top="99"  Right="768" Bottom="130" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                        <%--16 Seguimiento de Juntas--%>
                        <asp:RectangleHotSpot Left="791" Top="8"   Right="887" Bottom="45" NavigateUrl="/Calidad/FiltrosSeguimientoJunta.aspx" HotSpotMode="Navigate" />
                        <%--17 Seguimiento de Spools--%>
                        <asp:RectangleHotSpot Left="792" Top="56"  Right="889" Bottom="94" NavigateUrl="/Calidad/FiltrosSeguimientoSpool.aspx" HotSpotMode="Navigate" />
                        <%--18 Juntas de Campo--%>
                        <asp:RectangleHotSpot Left="450" Top="530"  Right="555" Bottom="560" NavigateUrl="/Produccion/JuntasCampo.aspx" HotSpotMode="Navigate" />

                        <asp:CircleHotSpot X="238" Y="110" Radius="14" NavigateUrl="#catalogos" />
                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblProduccion" meta:resourcekey="lblProduccion" Text="Producción" />
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
