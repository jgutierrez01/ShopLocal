<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubMenu.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.SubMenu" %>
<div class="menuInferior" style="margin-top:0px; display:block;">
    <!----Administracion ya quedo---->
    <asp:Panel ID="pnAdministracion" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlPerfiles" NavigateUrl="~/Administracion/LstPerfiles.aspx"
                    meta:resourcekey="hlPerfiles" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlUsuarios" NavigateUrl="~/Administracion/LstUsuario.aspx"
                    meta:resourcekey="hlUsuarios" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlDestajos" NavigateUrl="~/Administracion/LstPeriodosDestajo.aspx"
                    meta:resourcekey="hlDestajos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlEstimaciones" NavigateUrl="~/Administracion/LstEstimado.aspx"
                    meta:resourcekey="hlEstimaciones" />
            </li>
        </ul>
    </asp:Panel>
    <!----Calidad ya quedo---->
    <asp:Panel ID="pnCalidad" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlSeguimientoJuntas" NavigateUrl="~/Calidad/FiltrosSeguimientoJunta.aspx"
                    meta:resourcekey="hlSeguimientoJuntas" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlSeguimientoSpools" NavigateUrl="~/Calidad/FiltrosSeguimientoSpool.aspx"
                    meta:resourcekey="hlSeguimientoSpools" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlRevisionSpools" NavigateUrl="~/Calidad/RevisionSpools.aspx"
                    meta:resourcekey="hlRevisionSpools" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCertificacion" NavigateUrl="~/Calidad/CertificacionLigero.aspx"
                    meta:resourcekey="hlCertificacion" />
            </li>
        </ul>
    </asp:Panel>
    <!----Catalogos ya quedo---->
    <asp:Panel ID="pnCatalogo" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlClientes" NavigateUrl="~/Catalogos/LstCliente.aspx"
                    meta:resourcekey="hlClientes" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlFabricantes" NavigateUrl="~/Catalogos/LstFabricantes.aspx"
                    meta:resourcekey="hlFabricantes" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlProveedores" NavigateUrl="~/Catalogos/LstProveedor.aspx"
                    meta:resourcekey="hlProveedores" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlProyectos" NavigateUrl="~/Catalogos/LstProyecto.aspx"
                    meta:resourcekey="hlProyectos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlSoldadores" NavigateUrl="~/Catalogos/LstSoldador.aspx"
                    meta:resourcekey="hlSoldadores" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlTransportistas" NavigateUrl="~/Catalogos/LstTransportistas.aspx"
                    meta:resourcekey="hlTransportistas" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlTuberos" NavigateUrl="~/Catalogos/LstTubero.aspx"
                    meta:resourcekey="hlTuberos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlDiametros" NavigateUrl="~/Catalogos/LstDiametro.aspx"
                    meta:resourcekey="hlDiametros" />
            </li>
           <%-- <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCedulas" NavigateUrl="~/Catalogos/LstCedula.aspx"
                    meta:resourcekey="hlCedulas" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlEspesores" NavigateUrl="~/Catalogos/TblEspesores.aspx"
                    meta:resourcekey="hlEspesores" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlKgTeoricos" NavigateUrl="~/Catalogos/KgTeoricos.aspx"
                    meta:resourcekey="hlKgTeoricos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlPulgadasEquivalentes" NavigateUrl="~/Catalogos/LstPeq.aspx"
                    meta:resourcekey="hlPulgadasEquivalentes" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlTiposCorte" NavigateUrl="~/Catalogos/LstTipoCorte.aspx"
                    meta:resourcekey="hlTiposCorte" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlTiposJunta" NavigateUrl="~/Catalogos/LstTipoJunta.aspx"
                    meta:resourcekey="hlTiposJunta" />
            </li>--%>
        </ul>
    </asp:Panel>
    <!----Ingeniería ya quedo---->
    <asp:Panel ID="pnIngenieria" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlImportacionDatos" NavigateUrl="~/Ingenieria/ImportacionDatos.aspx"
                    meta:resourcekey="hlImportacionDatos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlIngenieriaProyecto" NavigateUrl="~/Ingenieria/IngenieriaDeProyecto.aspx"
                    meta:resourcekey="hlIngenieriaProyecto" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCortesAjuste" NavigateUrl="~/Ingenieria/CortesDeAjuste.aspx"
                    meta:resourcekey="hlCortesAjuste" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlNombradoSpool" NavigateUrl="~/Ingenieria/NombradoSpool.aspx"
                    meta:resourcekey="hlNombradoSpool" />
            </li>
        </ul>
    </asp:Panel>
    <!----Materiales ya quedo---->
    <asp:Panel ID="pnMateriales" runat="server" CssClass="Elemento" Visible="false">
        <ul>           
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlRecepciones" NavigateUrl="~/Materiales/LstRecepcion.aspx"
                    meta:resourcekey="hlRecepciones" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlIC" NavigateUrl="~/Materiales/LstMatPorItemCode.aspx"
                    meta:resourcekey="hlIC" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlNumUnicos" NavigateUrl="~/Materiales/LstNumeroUnico.aspx"
                    meta:resourcekey="hlNumUnicos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlReqPintura" NavigateUrl="~/Materiales/ReqPinturaNumUnico.aspx"
                    meta:resourcekey="hlReqPintura" />
            </li>
           <%-- <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlReporteReqPintura" NavigateUrl="~/Materiales/RepReqPinturaNumUnico.aspx"
                    meta:resourcekey="hlReporteReqPintura" />
            </li>--%>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlPinturaNumUnico" NavigateUrl="~/Materiales/LstPinturaNumUnico.aspx"
                    meta:resourcekey="hlPinturaNumUnico" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlConfinarSpool" NavigateUrl="~/Materiales/LstConfinarSpool.aspx"
                    meta:resourcekey="hlConfinarSpool" />
            </li>
            <%--<li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlMovimientosInventario" NavigateUrl="~/Materiales/MovimientosInventario.aspx"
                    meta:resourcekey="hlMovimientosInventario" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlSegmentarTubo" NavigateUrl="~/Materiales/SegmentarTubo.aspx"
                    meta:resourcekey="hlSegmentarTubo" />
            </li>--%>
        </ul>
    </asp:Panel>
    <!----Producción ya quedo---->
    <asp:Panel ID="pnProduccion" runat="server" CssClass="Elemento" Visible="false">
        <ul>            
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlPrioridades" NavigateUrl="~/Produccion/FijarPrioridad.aspx"
                    meta:resourcekey="hlPrioridades" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCruce" NavigateUrl="~/Produccion/CrucePorProyecto.aspx"
                    meta:resourcekey="hlCruce" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCruceForzado" NavigateUrl="~/Produccion/NuevaOdt.aspx"
                    meta:resourcekey="hlCruceForzado" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlOrdenesTrabajo" NavigateUrl="~/Produccion/LstOrdenTrabajo.aspx"
                    meta:resourcekey="hlOrdenesTrabajo" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCortesJunta" NavigateUrl="~/Produccion/CorteJunta.aspx"
                    meta:resourcekey="hlCortesJunta" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlCongeladosNumeroUnico" NavigateUrl="~/Produccion/CongeladosNumeroUnico.aspx"
                    meta:resourcekey="hlCongeladosNumeroUnico" />
            </li>
        </ul>
    </asp:Panel>
    <!----Falta agregar ID del proyecto en el que están---->
    <asp:Panel ID="pnProyecto" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlProveedoresP" NavigateUrl="~/Proyectos/Proveedores.aspx?ID="
                    meta:resourcekey="hlProveedoresP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlFabricantesP" NavigateUrl="~/Proyectos/Fabricantes.aspx?ID="
                    meta:resourcekey="hlFabricantesP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlTransportistasP" NavigateUrl="~/Proyectos/Transportistas.aspx?ID="
                    meta:resourcekey="hlTransportistasP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlItemCodesP" NavigateUrl="~/Proyectos/ItemCodes.aspx?ID="
                    meta:resourcekey="hlItemCodesP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlICEquivalentesP" NavigateUrl="~/Proyectos/IcEquivalentes.aspx?ID="
                    meta:resourcekey="hlICEquivalentesP" />
            </li>
            <li >
                <samweb:AuthenticatedHyperLink runat="server" ID="hlColadasP" NavigateUrl="~/Proyectos/Coladas.aspx?ID="
                    meta:resourcekey="hlColadasP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlWpsP" NavigateUrl="~/Proyectos/Wps.aspx?ID="
                    meta:resourcekey="hlWpsP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlDossierCalidadP" NavigateUrl="~/Proyectos/DossierCalidad.aspx?ID="
                    meta:resourcekey="hlDossierCalidadP" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlProgramacion" NavigateUrl="~/Proyectos/Programa.aspx?ID="
                    meta:resourcekey="hlProgramacion" />
            </li>
        </ul>
    </asp:Panel>
    <!----WorkStatus ya quedo---->
    <asp:Panel ID="pnWorkstatus" runat="server" CssClass="Elemento" Visible="false">
        <ul>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlListaCuadrante" NavigateUrl="~/WorkStatus/LstCuadrantes.aspx" meta:resourcekey="hlListaCuadrantes">
                </samweb:AuthenticatedHyperLink>
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlListaDespachos" NavigateUrl="~/WorkStatus/TransferenciaCorte.aspx"
                    meta:resourcekey="hlListaDespachos" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlNuevoCorte" NavigateUrl="~/WorkStatus/NuevoCorte.aspx"
                    meta:resourcekey="hlNuevoCorte" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlDespachos" NavigateUrl="~/WorkStatus/Despacho.aspx"
                    meta:resourcekey="hlDespachos" />
            </li>
            
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlArmado" NavigateUrl="~/WorkStatus/LstArmado.aspx"
                    meta:resourcekey="hlArmado" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlSoldadura" NavigateUrl="~/WorkStatus/LstSoldadura.aspx"
                    meta:resourcekey="hlSoldadura" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlInspeccionVisual" NavigateUrl="~/WorkStatus/LstInspeccionVisual.aspx"
                    meta:resourcekey="hlInspeccionVisual" />
            </li>
            <li>
                <samweb:AuthenticatedHyperLink runat="server" ID="hlLiberacionDimensionalPatio" NavigateUrl="~/WorkStatus/LstInspeccionDimensional.aspx"
                    meta:resourcekey="hlLiberacionDimensionalPatio" />
            </li>
        </ul>
    </asp:Panel>
</div>
