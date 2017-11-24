<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PiePagina.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.PiePagina" %>
<div class="FooterIzquierda">
    <div class="Elemento">
        <asp:Label runat="server" class="Titulo" meta:resourcekey="lblWorkstatus" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlListaDespachos" NavigateUrl="~/WorkStatus/LstDespacho.aspx"
            meta:resourcekey="hlListaDespachos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlDespachos" NavigateUrl="~/WorkStatus/Despacho.aspx"
            meta:resourcekey="hlDespachos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlDespachosCortes" NavigateUrl="~/WorkStatus/TransferenciaCorte.aspx"
            meta:resourcekey="hlDespachosCortes" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlNuevoCorte" NavigateUrl="~/WorkStatus/NuevoCorte.aspx"
            meta:resourcekey="hlNuevoCorte" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlArmado" NavigateUrl="~/WorkStatus/LstArmado.aspx"
            meta:resourcekey="hlArmado" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlSoldadura" NavigateUrl="~/WorkStatus/LstSoldadura.aspx"
            meta:resourcekey="hlSoldadura" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlInspeccionVisual" NavigateUrl="~/WorkStatus/LstInspeccionVisual.aspx"
            meta:resourcekey="hlInspeccionVisual" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlLiberacionDimensionalPatio" NavigateUrl="~/WorkStatus/LiberacionesDimensionalesPatio.aspx"
            meta:resourcekey="hlLiberacionDimensionalPatio" />
    </div>
    <div class="Elemento">
        <asp:Label ID="lblMateriales" runat="server" class="Titulo" meta:resourcekey="lblMateriales" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlRecepcionMateriales" NavigateUrl="~/Materiales/NuevaRecepcion.aspx"
            meta:resourcekey="hlRecepcionMateriales" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlNumUnicos" NavigateUrl="~/Materiales/LstNumeroUnico.aspx"
            meta:resourcekey="hlNumUnicos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlReqPintura" NavigateUrl="~/Materiales/ReqPinturaNumUnico.aspx"
            meta:resourcekey="hlReqPintura" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlConfinarSpool" NavigateUrl="~/Materiales/LstConfinarSpool.aspx"
            meta:resourcekey="hlConfinarSpool" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlMateriales" NavigateUrl="~/Materiales/MatDefault.aspx"
            meta:resourcekey="hlMateriales" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlPintura" NavigateUrl="~/Materiales/MatDefault.aspx"
            meta:resourcekey="hlPintura" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlMovimientosInventario" NavigateUrl="~/Materiales/MovimientosInventario.aspx"
            meta:resourcekey="hlMovimientosInventario" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlSegmentarTubo" NavigateUrl="~/Materiales/SegmentarTubo.aspx"
            meta:resourcekey="hlSegmentarTubo" />

    </div>
    <div class="Elemento">
        <asp:Label ID="lblIngenieria" runat="server" class="Titulo" meta:resourcekey="lblIngenieria" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlImportacionDatos" NavigateUrl="~/Ingenieria/ImportacionDatos.aspx"
            meta:resourcekey="hlImportacionDatos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlIngenieriaProyecto" NavigateUrl="~/Ingenieria/IngenieriaDeProyecto.aspx"
            meta:resourcekey="hlIngenieriaProyecto" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlCortesAjuste" NavigateUrl="~/Ingenieria/CortesDeAjuste.aspx"
            meta:resourcekey="hlCortesAjuste" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlNombradoSpool" NavigateUrl="~/Ingenieria/NombradoSpool.aspx"
            meta:resourcekey="hlNombradoSpool" />
    </div>
    <div class="Elemento">
        <asp:Label ID="lblProduccion" runat="server" class="Titulo" meta:resourcekey="lblProduccion" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlCruce" NavigateUrl="~/Produccion/CrucePorProyecto.aspx"
            meta:resourcekey="hlCruce" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlPrioridades" NavigateUrl="~/Produccion/FijarPrioridad.aspx"
            meta:resourcekey="hlPrioridades" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlOrdenesTrabajo" NavigateUrl="~/Produccion/NuevaOdt.aspx"
            meta:resourcekey="hlOrdenesTrabajo" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlCortesJunta" NavigateUrl="~/Produccion/CorteJunta.aspx"
            meta:resourcekey="hlCortesJunta" />
    </div>
    <div class="Elemento">
        <asp:Label ID="lblCalidad" runat="server" class="Titulo" meta:resourcekey="lblCalidad" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlSeguimientoJuntas" NavigateUrl="~/Calidad/FiltrosSeguimientoJunta.aspx"
            meta:resourcekey="hlSeguimientoJuntas" />  
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlSeguimientoSpools" NavigateUrl="~/Calidad/FiltrosSeguimientoSpool.aspx"
            meta:resourcekey="hlSeguimientoSpools" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlRevisionSpools" NavigateUrl="~/Calidad/RevisionSpools.aspx"
            meta:resourcekey="hlRevisionSpools" />
    </div>
    <div class="Elemento">
        <asp:Label ID="lblCatalogos" runat="server" class="Titulo" meta:resourcekey="lblCatalogos" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlClientes" NavigateUrl="~/Catalogos/LstCliente.aspx"
            meta:resourcekey="hlClientes" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlFabricantes" NavigateUrl="~/Catalogos/LstFabricantes.aspx"
            meta:resourcekey="hlFabricantes" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlProveedores" NavigateUrl="~/Catalogos/LstProveedor.aspx"
            meta:resourcekey="hlProveedores" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlProyectos" NavigateUrl="~/Catalogos/LstProyecto.aspx"
            meta:resourcekey="hlProyectos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlSoldadores" NavigateUrl="~/Catalogos/LstSoldador.aspx"
            meta:resourcekey="hlSoldadores" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlTransportistas" NavigateUrl="~/Catalogos/LstTransportistas.aspx"
            meta:resourcekey="hlTransportistas" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlTuberos" NavigateUrl="~/Catalogos/LstTubero.aspx"
            meta:resourcekey="hlTuberos" />
    </div>
    <div class="Elemento">
        <asp:Label ID="lblAdministracion" runat="server" class="Titulo" meta:resourcekey="lblAdministracion" /><br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlUsuarios" NavigateUrl="~/Administracion/LstUsuario.aspx"
            meta:resourcekey="hlUsuarios" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlPerfiles" NavigateUrl="~/Administracion/LstPerfiles.aspx"
            meta:resourcekey="hlPerfiles" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlDestajos" NavigateUrl="~/Administracion/LstPeriodosDestajo.aspx"
            meta:resourcekey="hlDestajos" />
        <br />
        <samweb:AuthenticatedHyperLink runat="server" ID="hlEstimaciones" NavigateUrl="~/Administracion/LstEstimado.aspx"
            meta:resourcekey="hlEstimaciones" />
    </div>
</div>
<div class="FooterDerecha">
    <asp:HyperLink runat="server" ID="hlSteelgo" ImageUrl="/Imagenes/Logos/logoSteelgo.png" CssClass="LogoSteelgo" meta:resourcekey="hlSteelgo" Target="_blank" />
    <p style="clear: both; height: 0px; margin: 0px" />
    <div class="Elemento">
        <asp:HyperLink runat="server" ID="hlAcerca" Target="_blank" meta:resourcekey="hlAcerca" />
        <p style="clear: both; height: 0px; margin: 0px"></p>
        <asp:HyperLink runat="server" ID="hlPrivacidad" Target="_blank" meta:resourcekey="hlPrivacidad" />
        <p style="clear: both; height: 0px; margin: 0px"></p>
        <asp:HyperLink runat="server" ID="hlPoliticasUso" Target="_blank" meta:resourcekey="hlPoliticasUso" />
        <p style="clear: both; height: 0px; margin: 0px"></p>
        <asp:HyperLink runat="server" ID="hlContacto" Target="_blank" meta:resourcekey="hlContacto" />
    </div>
    <p style="clear: both; height: 5px; margin: 0px" />
</div>
<p></p>
