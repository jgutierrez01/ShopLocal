<%@ Page  Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="IngDefault.aspx.cs" Inherits="SAM.Web.Ingenieria.IngDefault" %>
<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">

    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblDashboard" Text="Dashboard" />
    </div>
     <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">  
                      
                    <asp:ImageMap ID="imgMap" runat="server" CssClass="noBorders" meta:resourcekey="imgMap">
                        <%--1 Creación y Configuración de Proyecto--%>
                        <asp:RectangleHotSpot Left="413" Top="44"  Right="556" Bottom="93" NavigateUrl="/Catalogos/LstProyecto.aspx" HotSpotMode="Navigate" />
                        <%--2 Administracion--%>
                        <asp:RectangleHotSpot Left="620" Top="56"  Right="702" Bottom="85" NavigateUrl="/Administracion/AdminDefault.aspx" HotSpotMode="Navigate" />
                        <%--3 Importación de Ingenierías--%>
                        <asp:RectangleHotSpot Left="371" Top="139" Right="475" Bottom="176" NavigateUrl="/Ingenieria/ImportacionDatos.aspx" HotSpotMode="Navigate" />
                        <%--4 Recepción de Materiales--%>
                        <asp:RectangleHotSpot Left="497" Top="139" Right="599" Bottom="176" NavigateUrl="/Materiales/LstRecepcion.aspx" HotSpotMode="Navigate" />
                        <%--5 Materiales--%>
                        <asp:RectangleHotSpot Left="619" Top="145" Right="702" Bottom="178" NavigateUrl="/Materiales/MatDefault.aspx" HotSpotMode="Navigate" />
                        <%--6 Ingeniería Pendiente por Aprobar --%>
                        <asp:RectangleHotSpot Left="204" Top="208" Right="309" Bottom="242" NavigateUrl="/Ingenieria/PendientesHomologar.aspx" HotSpotMode="Navigate" />
                        <%--7 Ingeniería de Proyecto--%>
                        <asp:RectangleHotSpot Left="204" Top="270" Right="309" Bottom="307" NavigateUrl="/Ingenieria/IngenieriaDeProyecto.aspx" HotSpotMode="Navigate" />
                        <%--7 Historial Workstatus--%>
                        <asp:RectangleHotSpot Left="153" Top="245" Right="176" Bottom="277" NavigateUrl="/Ingenieria/WorkstatusIngenieria.aspx" HotSpotMode="Navigate" />
                        <%--7 Historial Revisiones--%>
                        <asp:RectangleHotSpot Left="153" Top="290" Right="176" Bottom="322" NavigateUrl="/Ingenieria/RevisionesIngenieria.aspx" HotSpotMode="Navigate" />
                        <%--7 Edición Nombre de Spool--%>
                        <asp:RectangleHotSpot Left="136" Top="356" Right="245" Bottom="392" NavigateUrl="/Ingenieria/NombradoSpool.aspx" HotSpotMode="Navigate" />
                        <%--8 Cortes de Ajuste--%>
                        <asp:RectangleHotSpot Left="263" Top="356" Right="367" Bottom="392" NavigateUrl="/Ingenieria/CortesDeAjuste.aspx" HotSpotMode="Navigate" />
                        <%--9 Cruce de Materiales--%>
                        <asp:RectangleHotSpot Left="413" Top="212" Right="557" Bottom="239" NavigateUrl="/Produccion/CrucePorProyecto.aspx" HotSpotMode="Navigate" />
                        <%--10 Orden de Trabajo--%>
                        <asp:RectangleHotSpot Left="414" Top="254" Right="559" Bottom="280" NavigateUrl="/Produccion/LstOrdenTrabajo.aspx" HotSpotMode="Navigate" />
                        <%--11 Workstatus--%>
                        <asp:RectangleHotSpot Left="438" Top="300" Right="519" Bottom="332" NavigateUrl="/WorkStatus/WkStDefault.aspx" HotSpotMode="Navigate" />
                        <%--12 Producción--%>
                        <asp:RectangleHotSpot Left="591" Top="213" Right="675" Bottom="247" NavigateUrl="/Produccion/ProdDefault.aspx" HotSpotMode="Navigate" />
                        <%--13 Seguimiento Juntas--%>
                        <asp:RectangleHotSpot Left="791" Top="7"   Right="889" Bottom="46" NavigateUrl="/Calidad/FiltrosSeguimientoJunta.aspx" HotSpotMode="Navigate" />
                        <%--14 Seguimiento Spools--%>
                        <asp:RectangleHotSpot Left="792" Top="55"  Right="890" Bottom="94" NavigateUrl="/Calidad/FiltrosSeguimientoSpool.aspx" HotSpotMode="Navigate" />
                        <asp:CircleHotSpot X="372" Y="141" Radius="14" NavigateUrl="#catalogos" />
                    </asp:ImageMap>
                </div>
            </td>
        </tr>
    </table>
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblIngenieria" meta:resourcekey="lblIngenieria" Text="Ingeniería" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                 <div id="dashBoard">                  
                    <div class="divIzquierdo" >
                        <table>
                            <tr>
                                <td rowspan="3" valign="top">
                                    <img src="../Imagenes/Dashboards/ico_1.png" alt="" name="catalogos" />
                                </td>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catCliente" NavigateUrl="~/Catalogos/LstCliente.aspx"
                                        meta:resourcekey="catCliente" />
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catPatio" NavigateUrl="~/Catalogos/LstPatio.aspx"
                                        meta:resourcekey="catPatio" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <samweb:AuthenticatedHyperLink runat="server" ID="catProyecto" NavigateUrl="~/Catalogos/LstProyecto.aspx"
                                        meta:resourcekey="catProyecto" />
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
