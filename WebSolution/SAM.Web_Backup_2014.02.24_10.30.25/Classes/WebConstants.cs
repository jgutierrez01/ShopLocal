using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Classes
{
    public static class WebConstants
    {
        #region Constantes para las llaves del HttpContext

        public struct Contexto
        {
            public const string CTX_MENSAJE = "Context.MensajeExito";
        }

        #endregion

        #region Urls de la carpeta de Administracion

        public struct AdminUrl
        {
            public const string LST_USUARIOS = "~/Administracion/LstUsuario.aspx";
            public const string LST_PERFIL = "~/Administracion/LstPerfiles.aspx";
            public const string ADMIN_MENSAJE_EXITO = "~/Administracion/MensajeExito.aspx";
            public const string TBL_DESTAJOS = "~/Administracion/TblDestajos.aspx?PTOID={0}&FID={1}&TID={2}&PSO={3}&PSOID={4}";
            public const string ImportaDestajos = "~/Administracion/ImportaDestajos.aspx?PTOID={0}&FID={1}&TID={2}&PSO={3}&PSOID={4}";
            public const string DetallePeriodoDestajo = "~/Administracion/DetPeriodoDestajo.aspx?ID={0}";
            public const string DetalleDestajo = "~/Administracion/DetDestajo.aspx?Tipo={0}&ID={1}";
            public const string DetalleEstimacion = "~/Administracion/DetEstimacion.aspx?ID={0}";
        }

        #endregion

        #region Urls de Calidad

        public struct CalidadUrl
        {
            public const string SEGUIMIENTO_SPOOL = "~/Calidad/SeguimientoSpool.aspx?PID={0}&NC={1}&EM={2}&OT={3}&IDS={4}";
            public const string SEGUIMIENTO_JUNTA = "~/Calidad/SeguimientoJuntasLigero.aspx?PID={0}&NC={1}&EM={2}&HR={3}&OT={4}&IDS={5}";
            public const string DESCARGA_REPORTES = @"~\Calidad\DescargarReportes.aspx?ID={0}&PID={1}&RID={2}";
        }

        #endregion

        #region Urls de Catalogos

        public struct CatalogoUrl
        {
            public const string LST_FAMILIA_MATERIAL = "~/Catalogos/LstFamiliaMaterial.aspx";
            public const string LST_FAMILIA_ACERO = "~/Catalogos/LstFamAcero.aspx";
            public const string LST_ACERO = "~/Catalogos/LstAcero.aspx";
            public const string LST_PROCESO_RAIZ = "~/Catalogos/LstProcesoRaiz.aspx";
            public const string LST_PROCESO_RELLENO = "~/Catalogos/LstProcesoRelleno.aspx";
            public const string LST_WPS = "~/Catalogos/LstWps.aspx";
            public const string LST_PROVEEDOR = "~/Catalogos/LstProveedor.aspx";
            public const string LST_TUBERO = "~/Catalogos/LstTubero.aspx";
            public const string LST_DEFECTO = "~/Catalogos/LstDefectos.aspx";
            public const string LST_FABRICANTE = "~/Catalogos/LstFabricantes.aspx";
            public const string LST_TRANSPORTISTA = "~/Catalogos/LstTransportistas.aspx";
            public const string LST_CLIENTE = "~/Catalogos/LstCliente.aspx";
            public const string LST_PATIO = "~/Catalogos/LstPatio.aspx";
            public const string LST_SOLDADOR = "~/Catalogos/LstSoldador.aspx";
            public const string LST_CONSUMIBLES = "~/Catalogos/LstConsumibles.aspx?PID={0}";
            public const string DET_CONSUMIBLES = "~/Catalogos/DetConsumibles.aspx?PID={0}";
            public const string LST_DIAMETRO = "~/Catalogos/LstDiametro.aspx";
            public const string LST_CEDULA = "~/Catalogos/LstCedula.aspx";
            public const string LST_TIPOCORTE = "~/Catalogos/LstTipoCorte.aspx";
            public const string LST_TIPOJUNTA = "~/Catalogos/LstTipoJunta.aspx";
            public const string TBL_ESPESOR = "~/Catalogos/TblEspesores.aspx";
            public const string TBL_PEQ = "~/Catalogos/LstPeq.aspx?TID={0}&FID={1}&PID={2}";
            public const string TBL_KG = "~/Catalogos/KgTeoricos.aspx";
            public const string ImportaPeq = "~/Catalogos/ImportaPeq.aspx?TID={0}&FID={1}&PID={2}";
        }

        #endregion

        #region Urls de Proyectos

        public struct ProyectoUrl
        {
            public const string DET_PROYECTO = "~/Proyectos/DetProyecto.aspx?ID={0}";
            public const string LST_ITEM_CODES = "~/Proyectos/ItemCodes.aspx?ID={0}";
            public const string LST_COLADAS = "~/Proyectos/Coladas.aspx?ID={0}";
            public const string LST_ITEM_CODES_EQUIVALENTES = "~/Proyectos/IcEquivalentes.aspx?ID={0}";
            public const string LST_PROVEEDORES = "~/Proyectos/Proveedores.aspx?ID={0}";
            public const string LST_FABRICANTES = "~/Proyectos/Fabricantes.aspx?ID={0}";
            public const string LST_TRANSPORTISTAS = "~/Proyectos/Transportistas.aspx?ID={0}";
            public const string LST_WPS = "~/Proyectos/Wps.aspx?ID={0}";
            public const string LST_DOSSIER = "~/Proyectos/DossierCalidad.aspx?ID={0}";
            public const string CONFIGURACION = "~/Proyectos/Configuracion.aspx?ID={0}";
            public const string DETALLE_ITEM_CODE_AGREGAR = "~/Proyectos/DetItemCodes.aspx?PID={0}";
            public const string DET_COLADAS_AGREGAR = "~/Proyectos/DetColadas.aspx?PID={0}";
            public const string DET_IC_EQUIVALENTE_AGREGAR = "~/Proyectos/DetIcEquivalentes.aspx?PID={0}";
            public const string DET_TIPOS_REPORTES = "~/Proyectos/TipoReporteProyecto.aspx?ID={0}";
            public const string DET_PENDIENTES_AUTOMATICOS = "~/Proyectos/PendientesAutomaticos.aspx?ID={0}";
            public const string DET_PROGRAMACION = "~/Proyectos/Programa.aspx?ID={0}";
            public const string PESO_ITEMCODE = "~/Proyectos/PesoItemCode.aspx?ID={0}";
            public const string ExportaExcelItemCodePeso = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}";
        }

        #endregion

        #region Urls de Dashboard

        public struct DashboardUrl
        {
            public const string DEFAULT = "~/Dashboard/DashDefault.aspx";
        }

        #endregion

        #region Urls de Workstatus

        public struct WorkstatusUrl
        {
            public const string DEFAULT = "~/WorkStatus/WkStDefault.aspx";
            public const string LST_CORTES = "~/WorkStatus/LstCortes.aspx";
            public const string NUEVO_CORTE = "~/WorkStatus/NuevoCorte.aspx";
        }

        #endregion

        #region Urls Ingenieria
        public struct IngenieriaUrl
        {
            public const string LST_INGENIERIA = "~/Ingenieria/IngenieriaDeProyecto.aspx";
            public const string LST_INGENIERIAPID = "~/Ingenieria/IngenieriaDeProyecto.aspx?PID={0}";
            public const string MENSAJE_EXITO = "~/Ingenieria/MensajeExitoIng.aspx";
            public const string NOMBRADO_SPOOL = "~/Ingenieria/NombradoSpool.aspx";
            public const string REDIRECT_NOMBRADO_SPOOL = "~/Ingenieria/NombradoSpool.aspx?PID={0}";
            public const string IMPORTACION_DATOS = "~/Ingenieria/ImportacionDatos.aspx";
            public const string PENDIENTES_HOMOLOGAR  = "~/Ingenieria/PendientesHomologar.aspx?ID={0}";
            public const string CAMPO_SPOOL = "~/Ingenieria/CampoSpool.aspx?ID={0}";
            public const string PRIORIDADES_SPOOL = "~/Ingenieria/PrioridadesSpool.aspx?ID={0}";

        }
        #endregion

        #region Urls de Produccion

        public struct ProduccionUrl
        {
            public const string ReporteODT = "~/Produccion/ReporteOdt.aspx";
            public const string ReporteFaltantes = "~/Produccion/ReporteFaltantes.aspx?ID={0}&Type={1}";
            public const string DetalleOdt = "~/Produccion/DetOdt.aspx?ID={0}";
            public const string ListaOrdenesDeTrabajo = "~/Produccion/LstOrdenTrabajo.aspx";
            public const string AgregarSpoolOdt = "~/Produccion/AgregaSpoolOdt.aspx?ID={0}";
            public const string AgregarSpoolOdtAsignado = "~/Produccion/AgregaSpoolOdtAsignado.aspx?ID={0}";
            public const string ExportaExcel = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}";
            public const string ExportaExcelCalidad = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}&Nc={2}&Emb={3}&Ot={4}&Filtros={5}";
            public const string ExportaExcelJuntaCalidad = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}&Nc={2}&Emb={3}&Ot={4}&Rep={5}&Filtros={6}&Columnas={7}";
            public const string ExportaExcelDestajos = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}&Tj={2}&Fa={3}&PrID={4}&PrValor={5}&pytoValor={6}";
            public const string MensajeExito = "~/Produccion/MensajeExitoProd.aspx";
            public const string CongeladosNumeroUnico = "~/Produccion/CongeladosNumeroUnico.aspx";
            public const string CrucePorImportacionCsv = "~/Produccion/CrucePorImportacion.aspx";
        }

        #endregion

        #region Urls de Materiales

        public struct MaterialesUrl
        {
            public const string LST_NUMEROSUNICOS = "~/Materiales/LstNumeroUnico.aspx";
            public const string AltaNumeroUnico = "~/Materiales/AltaNumeroUnico.aspx?ID={0}&NT={1}&NA={2}&PID={3}&MD={4}&NID={5}&MSID={6}";
            public const string DetalleRecepcion = "~/Materiales/DetRecepcion.aspx?ID={0}";
            public const string ExportaExcelLstNumeroUnico = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}&Co={2}&Ic={3}&NuI={4}&NuF={5}";
            public const string MovimientoInventario = "~/Materiales/MovimientosInventario.aspx";
            public const string SegmentarTubo = "~/Materiales/SegmentarTubo.aspx";
            public const string ExportaExcelItemCode = "~/Produccion/ExportaExcel.aspx?ID={0}&Type={1}";
        }

        #endregion

        #region Urls publicos

        public struct PublicUrl
        {
            public const string LOGIN = "~/Login.aspx";
            public const string LOGOUT = "/Logout.aspx";
        }

        #endregion

        #region Enumeracion menus

        public enum MenuItemEnum
        {
            Dashboard = 0,
            Workstatus = 1,
            Materiales = 2,
            Ingenieria = 3,
            Produccion = 4,
            Calidad = 5,
            Proyecto = 6,
            Catalogos = 7,
            Administracion = 8,
        }

        public enum SubMenuItemEnum
        {
            //todas las paginas habidas y por haber por los siglos de los siglos, amen.

            // hasta ahorita estan todas las que estan en los dashboards

            /// <summary>
            /// IMPORTANTE! EL ULTIMO NUMERO HASTA AHORITA ES EL 83
            /// Y ESTA EN CALIDAD... 
            /// SI SE QUIERE AGREGAR SE DEBE INSERTAR UN VALOR NUEVO, SE DEBE INSERTAR
            /// CON EL 84 Y ACTUALIZAR ESTE COMENTARIO.
            /// 
            /// 84 en adm_Destajos
            /// 
            /// 86 en cat_ImportConfigProyecto
            /// 
            /// 87 en proy_PendientesAutomaticos
            /// 
            /// 88 en prod_CongeladosNumeroUnico
            /// 
            /// 89 en prod_CongeladosOrdenTrabajo
            /// 
            /// 90 en prod_CongeladoParcial
            /// </summary>


            #region Administracion
            adm_Perfiles,
            adm_Usuarios,
            adm_Destajos,
            adm_Estimaciones,
            adm_Pendientes,
            #endregion

            #region Calidad
            cal_SeguimientoJuntas,
            cal_RevisionSpools,
            cal_SeguimientoSpools,
            cal_Certificacion,
            #endregion

            #region Catálogo
            cat_Aceros,
            cat_Clientes,
            cat_Consumibles,
            cat_Defectos,
            cat_Fabricantes,
            cat_FamAceros,
            cat_FamMateriales,
            cat_Patios,
            cat_ProcesoRaiz,
            cat_ProcesoRelleno,
            cat_Proveedores,
            cat_Proyectos,
            cat_Soldadores,
            cat_Transportistas,
            cat_Tuberos,
            cat_Wps,
            cat_ImportConfigProyecto,
            cat_Cedulas,
            cat_Diametro,
            cat_Espesores,
            cat_KgTeoricos,
            cat_PulgadasEquivalentes,
            cat_TiposCorte,
            cat_TiposJunta,
            #endregion

            #region Ingeniería
            ing_IngenieriaProyecto,
            ing_CortesAjuste,
            ing_NombradoSpool,
            ing_ImportacionDatos,
            #endregion

            #region Materiales
            mat_RecepcionMateriales,
            mat_Recepciones,
            mat_NumUnicos,
            mat_RequisicionesPintura,
            mat_ReporteRequisicionesPintura,
            mat_PinturaNumUnico,
            mat_ConfinarSpool,
            mat_MovimientosInventario,
            mat_SegmentarTubo,
            mat_ItemCode,
            #endregion

            #region Producción
            prod_CrucePrioridad,
            prod_CruceForzado,
            prod_Prioridades,
            prod_OrdenesTrabajo,
            prod_CortesJunta,
            prod_CongeladosNumeroUnico,
            prod_CongeladosOrdenTrabajo,
            prod_CongeladoParcial,
            #endregion

            #region Proyecto
            proy_Proveedores,
            proy_Fabricantes,
            proy_Transportistas,
            proy_ItemCodes,
            proy_ICEquivalentes,
            proy_Coladas,
            proy_Wps,
            proy_DossierCalidad,
            proy_PendientesAutomaticos,
            proy_TipoReportes,
            proy_Programa,
            #endregion

            #region Workstatus
            wks_DespachoACorte,
            wks_Despachos,
            wks_DespachosCortes,
            wks_NumUnicoTransferencia,
            wks_NuevoCorte,
            wks_ListadoCortes,
            wks_Armado,
            wks_Soldadura,
            wks_InspeccionVisual,
            wks_InspeccionDimensional,
            wks_RequisicionesPruebas,
            wks_PruebasNoDestructivas,
            wks_TratamientosTermicos,
            wks_RequisicionesPinturaSpool,
            wks_PinturaSpool,
            wks_Certificacion,
            wks_Embarque,
            wks_ReporteInspeccionVisual,
            wks_ReporteInspeccionDimensional,
            wks_ReporteRequisiciones,
            wks_Liberaciones,
            wks_ReportePND,
            wks_ReporteTt,
            wks_ReporteRequisicionPintura,
            wks_LiberacionVisualPatio,
            wks_LiberacionDimensionalPatio,
            wks_ListadoCuadrantes
            #endregion
        }
        #endregion


        #region Nombre reportes default

        public struct ReportesAplicacion
        {
            public const string Admon_Destajos = "DestajosAdmon";
            public const string Odt_Caratula = "CaratulaOdt";
            public const string Odt_CaratulaDetallada = "CaratulaDetalladaOdt";
            public const string Odt_CaratulaEstacion = "CaratulaPorEstacionTrabajoOdt";
            public const string Odt_Juntas = "JuntasOdt";
            public const string Odt_JuntasDetalle = "JuntasDetalleOdt";
            public const string Odt_JuntasDetalleEstacion = "JuntasDetalleEstacionOdt";
            public const string Odt_Cortes = "CortesOdt";
            public const string Odt_Materiales = "MaterialesOdt";
            public const string Odt_MaterialesDetalle = "MaterialesDetalleOdt";
            public const string Odt_MaterialesDetalleEstacion = "MaterialesDetalleEstacionOdt";
            public const string Odt_ResumenMateriales = "ResumenMaterialesOdt";
            public const string Odt_ResumenMaterialesEstacion = "ResumenMaterialesEstacionOdt";
            public const string Odt_ResumenMaterialesTaller = "ResumenMaterialesTallerOdt";
            public const string Odt_CorteEstacion = "CortesPorEstacionOdt";
            public const string Odt_CorteTaller = "CortesPorTallerOdt";
            public const string Wkst_Armado = "ArmadoWkst";
            public const string Wkst_Soldadura = "SoldaduraWkst";
            public const string Wkst_Requisicion = "RequisicionWkst";
            public const string Wkst_LiberacionDimensional = "LiberacionDimensionalWkst";
            public const string Wkst_InspeccionVisual = "InspeccionVisualWkst";
            public const string Wkst_RT = "RTWkst";
            public const string Wkst_PMI = "PMIWkst";
            public const string Wkst_PT = "PTWkst";
            public const string Wkst_Pwht = "PWHTWkst";
            public const string Wkst_Pintura = "PinturaWkst";
            public const string Wkst_Durezas = "DurezasWkst";
            public const string Wkst_Embarque = "EmbarqueWkst";
            public const string Wkst_Espesores = "EspesoresWkst";
            public const string Etiqueta_Material = "EtiquetaMaterial";
            public const string Etiqueta_Embarque = "EtiquetaEmbarque";
            public const string RequisicionPintura = "RequisicionPintura";
            public const string Wkst_ReqSpool = "RequisicionSpool";
            public const string Wkst_PNDSpool = "ReportePruebaSpool";
        }

        #endregion

    }
}
