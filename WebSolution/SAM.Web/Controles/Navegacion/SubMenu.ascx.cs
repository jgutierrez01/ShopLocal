using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MenuItemEnum = SAM.Web.Classes.WebConstants.MenuItemEnum;
using SubMenuItemEnum = SAM.Web.Classes.WebConstants.SubMenuItemEnum;

namespace SAM.Web.Controles.Navegacion
{
    public partial class SubMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// recibe el item del submenu que estará activo.
        /// </summary>
        /// <param name="tab"></param>
        public void EstableceSubItemActivo(SubMenuItemEnum tab)
        {
            //EstablecerSubMenuInactivos();
            switch (tab)
            {
                #region Administración
                case SubMenuItemEnum.adm_Perfiles:
                    hlPerfiles.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Usuarios:
                    hlUsuarios.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Estimaciones:
                    hlEstimaciones.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Destajos:
                    hlDestajos.CssClass = "Activo";
                    break;
                #endregion

                #region Certificacion
                case SubMenuItemEnum.cal_SeguimientoJuntas:
                    hlSeguimientoJuntas.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_RevisionSpools:
                    hlRevisionSpools.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_SeguimientoSpools:
                    hlSeguimientoSpools.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_Certificacion:
                    hlCertificacion.CssClass = "Activo";
                    break;
                #endregion

                #region Catalogos
                //case SubMenuItemEnum.cat_Cedulas:
                //    hlCedulas.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Diametro:
                    hlDiametros.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.cat_Espesores:
                //    hlEspesores.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_KgTeoricos:
                //    hlKgTeoricos.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_PulgadasEquivalentes:
                //    hlPulgadasEquivalentes.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_TiposCorte:
                //    hlTiposCorte.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_TiposJunta:
                //    hlTiposJunta.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_Aceros:
                //    hlAceros.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Clientes:
                    hlClientes.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.cat_Defectos:
                //    hlDefectos.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Fabricantes:
                    hlFabricantes.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.cat_FamAceros:
                //    hlFamAceros.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_Patios:
                //    hlPatios.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_ProcesoRaiz:
                //    hlProcesoRaiz.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_ProcesoRelleno:
                //    hlProcesoRelleno.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_Proveedores:
                //    hlProcesoRelleno.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Proyectos:
                    hlProyectos.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Soldadores:
                    hlSoldadores.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Transportistas:
                    hlTransportistas.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Tuberos:
                    hlTuberos.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.cat_Wps:
                //    hlWps.CssClass = "Activo";
                //    break;
                #endregion

                #region Ingeniería
                case SubMenuItemEnum.ing_IngenieriaProyecto:
                    hlIngenieriaProyecto.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_CortesAjuste:
                    hlCortesAjuste.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_NombradoSpool:
                    hlNombradoSpool.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_ImportacionDatos:
                    hlImportacionDatos.CssClass = "Activo";
                    break;
                #endregion

                #region Materiales
                case SubMenuItemEnum.mat_ItemCode:
                    hlIC.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_Recepciones:
                    hlRecepciones.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_NumUnicos:
                    hlNumUnicos.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_RequisicionesPintura:
                    hlReqPintura.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.mat_ReporteRequisicionesPintura:
                //    hlReporteReqPintura.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.mat_PinturaNumUnico:
                    hlPinturaNumUnico.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_ConfinarSpool:
                    hlConfinarSpool.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.mat_MovimientosInventario:
                //    hlMovimientosInventario.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.mat_SegmentarTubo:
                //    hlSegmentarTubo.CssClass = "Activo";
                //    break;
                #endregion

                #region Produccion
                case SubMenuItemEnum.prod_CrucePrioridad:
                    hlCruce.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_Prioridades:
                    hlPrioridades.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_OrdenesTrabajo:
                    hlOrdenesTrabajo.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CortesJunta:
                    hlCortesJunta.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CruceForzado:
                    hlCruceForzado.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CongeladosNumeroUnico:
                    hlCongeladosNumeroUnico.CssClass = "Activo";
                    break;
                #endregion

                #region Proyectos
                case SubMenuItemEnum.proy_Proveedores:
                    hlProveedoresP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Fabricantes:
                    hlFabricantesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Transportistas:
                    hlTransportistasP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_ItemCodes:
                    hlItemCodesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_ICEquivalentes:
                    hlICEquivalentesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Coladas:
                    hlColadasP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Wps:
                    hlWpsP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_DossierCalidad:
                    hlDossierCalidadP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Programa:
                    hlProgramacion.CssClass = "Activo";
                    break;
                #endregion

                #region Workstatus
                case SubMenuItemEnum.wks_ListadoCuadrantes:
                    hlListaCuadrante.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.wks_Certificacion:
                    hlCertificacion.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.wks_Despachos:
                    hlDespachos.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.wks_Armado:
                    hlArmado.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_DespachosCortes:
                //    hlDespachosCortes.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_Embarque:
                //    hlEmbarque.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_InspeccionDimensional:
                //    hlInspeccionDimensional.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_InspeccionVisual:
                    hlInspeccionVisual.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.wks_LiberacionDimensionalPatio:
                    hlLiberacionDimensionalPatio.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_Liberaciones:
                //    hlLiberaciones.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_LiberacionVisualPatio:
                //    hlLiberacionVisualPatio.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_DespachoACorte:
                    hlListaDespachos.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_ListadoCortes:
                //    hlListadoCortes.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_NuevoCorte:
                    hlNuevoCorte.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_NumUnicoTransferencia:
                //    hlNumUnicoTransferencia.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_PinturaSpool:
                //    hlPinturaSpool.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_PruebasNoDestructivas:
                //    hlPruebasNoDestructivas.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteInspeccionDimensional:
                //    hlReporteInspeccionDimensional.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteInspeccionVisual:
                //    hlReporteInspeccionVisual.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReportePND:
                //    hlReportePND.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteRequisiciones:
                //    hlReporteRequisiciones.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteRequisicionPintura:
                //    hlReporteRequisicionPintura.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteTt:
                //    hlReporteTt.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_RequisicionesPinturaSpool:
                //    hlRequisicionesPinturaSpool.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_RequisicionesPruebas:
                //    hlRequisicionesPruebas.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_Soldadura:
                    hlSoldadura.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_TratamientosTermicos:
                //    hlTratamientosTermicos.CssClass = "Activo";
                //    break;

                case SubMenuItemEnum.wks_TransferenciaSpool:
                    hlTransferenciaSpool.CssClass = "Activo";
                    break;
                default: 
                    break;


                #endregion
            }
        }
        
        /// <summary>
        /// recibe el item del submenu que estará activo
        /// asi como el id del proyecto que se está ligando al NavigateUrl.
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="proyectoID"></param>
        public void EstableceSubItemActivo(SubMenuItemEnum tab, int proyectoID)
        {
            EstableceProyectoId(proyectoID);
            //EstablecerSubMenuInactivos();
            switch (tab)
            {
                #region Administración
                case SubMenuItemEnum.adm_Perfiles:
                    hlPerfiles.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Usuarios:
                    hlUsuarios.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Estimaciones:
                    hlEstimaciones.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.adm_Destajos:
                    hlDestajos.CssClass = "Activo";
                    break;
                #endregion

                #region Certificacion
                case SubMenuItemEnum.cal_SeguimientoJuntas:
                    hlSeguimientoJuntas.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_RevisionSpools:
                    hlRevisionSpools.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_SeguimientoSpools:
                    hlSeguimientoSpools.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cal_Certificacion:
                    hlCertificacion.CssClass = "Activo";
                    break;
                #endregion

                #region Catalogos
                //case SubMenuItemEnum.cat_Cedulas:
                //    hlCedulas.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Diametro:
                    hlDiametros.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.cat_Espesores:
                //    hlEspesores.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_KgTeoricos:
                //    hlKgTeoricos.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_PulgadasEquivalentes:
                //    hlPulgadasEquivalentes.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_TiposCorte:
                //    hlTiposCorte.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.cat_TiposJunta:
                //    hlTiposJunta.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.cat_Clientes:
                    hlClientes.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Fabricantes:
                    hlFabricantes.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Proyectos:
                    hlProyectos.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Soldadores:
                    hlSoldadores.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Transportistas:
                    hlTransportistas.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.cat_Tuberos:
                    hlTuberos.CssClass = "Activo";
                    break;
                #endregion

                #region Ingeniería
                case SubMenuItemEnum.ing_IngenieriaProyecto:
                    hlIngenieriaProyecto.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_CortesAjuste:
                    hlCortesAjuste.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_NombradoSpool:
                    hlNombradoSpool.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.ing_ImportacionDatos:
                    hlImportacionDatos.CssClass = "Activo";
                    break;
                #endregion

                #region Materiales
                case SubMenuItemEnum.mat_ItemCode:
                    hlIC.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_Recepciones:
                    hlRecepciones.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_NumUnicos:
                    hlNumUnicos.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_RequisicionesPintura:
                    hlReqPintura.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.mat_ReporteRequisicionesPintura:
                //    hlReporteReqPintura.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.mat_PinturaNumUnico:
                    hlPinturaNumUnico.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.mat_ConfinarSpool:
                    hlConfinarSpool.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.mat_MovimientosInventario:
                //    hlMovimientosInventario.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.mat_SegmentarTubo:
                //    hlSegmentarTubo.CssClass = "Activo";
                //    break;
                #endregion

                #region Produccion
                case SubMenuItemEnum.prod_CrucePrioridad:
                    hlCruce.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_Prioridades:
                    hlPrioridades.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_OrdenesTrabajo:
                    hlOrdenesTrabajo.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CortesJunta:
                    hlCortesJunta.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CruceForzado:
                    hlCruceForzado.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.prod_CongeladosNumeroUnico:
                    hlCongeladosNumeroUnico.CssClass = "Activo";
                    break;
                #endregion

                #region Proyectos
                case SubMenuItemEnum.proy_Proveedores:
                    hlProveedoresP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Fabricantes:
                    hlFabricantesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Transportistas:
                    hlTransportistasP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_ItemCodes:
                    hlItemCodesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_ICEquivalentes:
                    hlICEquivalentesP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Coladas:
                    hlColadasP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Wps:
                    hlWpsP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_DossierCalidad:
                    hlDossierCalidadP.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.proy_Programa:
                    hlProgramacion.CssClass = "Activo";
                    break;

                #endregion

                #region Workstatus
                //case SubMenuItemEnum.wks_Certificacion:
                //    hlCertificacion.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_Despachos:
                    hlDespachos.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_DespachosCortes:
                //    hlDespachosCortes.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_Embarque:
                //    hlEmbarque.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_InspeccionDimensional:
                //    hlInspeccionDimensional.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_InspeccionVisual:
                    hlInspeccionVisual.CssClass = "Activo";
                    break;
                case SubMenuItemEnum.wks_LiberacionDimensionalPatio:
                    hlLiberacionDimensionalPatio.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_Liberaciones:
                //    hlLiberaciones.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_LiberacionVisualPatio:
                //    hlLiberacionVisualPatio.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_DespachoACorte:
                    hlListaDespachos.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_ListadoCortes:
                //    hlListadoCortes.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_NuevoCorte:
                    hlNuevoCorte.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_NumUnicoTransferencia:
                //    hlNumUnicoTransferencia.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_PinturaSpool:
                //    hlPinturaSpool.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_PruebasNoDestructivas:
                //    hlPruebasNoDestructivas.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteInspeccionDimensional:
                //    hlReporteInspeccionDimensional.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteInspeccionVisual:
                //    hlReporteInspeccionVisual.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReportePND:
                //    hlReportePND.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteRequisiciones:
                //    hlReporteRequisiciones.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteRequisicionPintura:
                //    hlReporteRequisicionPintura.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_ReporteTt:
                //    hlReporteTt.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_RequisicionesPinturaSpool:
                //    hlRequisicionesPinturaSpool.CssClass = "Activo";
                //    break;
                //case SubMenuItemEnum.wks_RequisicionesPruebas:
                //    hlRequisicionesPruebas.CssClass = "Activo";
                //    break;
                case SubMenuItemEnum.wks_Soldadura:
                    hlSoldadura.CssClass = "Activo";
                    break;

                case SubMenuItemEnum.wks_TransferenciaSpool:
                    hlTransferenciaSpool.CssClass = "Activo";
                    break;
                //case SubMenuItemEnum.wks_TratamientosTermicos:
                //    hlTratamientosTermicos.CssClass = "Activo";
                //    break;
                default:
                    break;


                #endregion
            }
        }

        /// <summary>
        /// establece el proyectoID a todos los links del submenu de proyectos
        /// </summary>
        /// <param name="proyectoID"></param>
        public void EstableceProyectoId(int proyectoID)
        {
            hlProveedoresP.NavigateUrl += proyectoID;
            hlFabricantesP.NavigateUrl += proyectoID;
            hlTransportistasP.NavigateUrl += proyectoID;
            hlItemCodesP.NavigateUrl += proyectoID;
            hlICEquivalentesP.NavigateUrl += proyectoID;
            hlColadasP.NavigateUrl += proyectoID;
            hlWpsP.NavigateUrl += proyectoID;
            hlDossierCalidadP.NavigateUrl += proyectoID;
            hlProgramacion.NavigateUrl += proyectoID;
        }

        public void QuitaLigasSubmenuProyecto()
        {
            hlProveedoresP.NavigateUrl = "javascript:void(0);";
            hlFabricantesP.NavigateUrl = "javascript:void(0);";
            hlTransportistasP.NavigateUrl = "javascript:void(0);";
            hlItemCodesP.NavigateUrl = "javascript:void(0);";
            hlICEquivalentesP.NavigateUrl = "javascript:void(0);";
            hlColadasP.NavigateUrl = "javascript:void(0);";
            hlWpsP.NavigateUrl = "javascript:void(0);";
            hlDossierCalidadP.NavigateUrl = "javascript:void(0);";
            hlProgramacion.NavigateUrl = "javascript:void(0);";
        }

        /// <summary>
        /// enciende y apaga los paneles con los links de los submenus
        /// </summary>
        /// <param name="menu"></param>
        public void EstablecerPanelActivo(MenuItemEnum menu)
        {
            EstablecerInactivos();

            switch (menu)
            {
                case MenuItemEnum.Workstatus:
                    pnWorkstatus.Visible = true;
                    break;
                case MenuItemEnum.Materiales:
                    pnMateriales.Visible = true;
                    break;
                case MenuItemEnum.Ingenieria:
                    pnIngenieria.Visible = true;
                    break;
                case MenuItemEnum.Produccion:
                    pnProduccion.Visible = true;
                    break;
                case MenuItemEnum.Calidad:
                    pnCalidad.Visible = true;
                    break;
                case MenuItemEnum.Proyecto:
                    pnProyecto.Visible = true;
                    break;
                case MenuItemEnum.Catalogos:
                    pnCatalogo.Visible = true;
                    break;
                case MenuItemEnum.Administracion:
                    pnAdministracion.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// oculta todos los paneles.
        /// </summary>
        public void EstablecerInactivos()
        {
            pnWorkstatus.CssClass = "Elemento";
            pnWorkstatus.Visible = false;

            pnMateriales.CssClass = "Elemento";
            pnMateriales.Visible = false;

            pnIngenieria.CssClass = "Elemento";
            pnIngenieria.Visible = false;

            pnProduccion.CssClass = "Elemento";
            pnProduccion.Visible = false;

            pnCalidad.CssClass = "Elemento";
            pnCalidad.Visible = false;

            pnProyecto.CssClass = "Elemento";
            pnProyecto.Visible = false;

            pnCatalogo.CssClass = "Elemento";
            pnCatalogo.Visible = false;

            pnAdministracion.CssClass = "Elemento";
            pnAdministracion.Visible = false;
        }
    }
}