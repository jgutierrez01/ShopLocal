using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.Entities.Cache;
using Mimo.Framework.Common;

namespace SAM.BusinessLogic.Calidad
{
    public class SeguimientoJuntaBL
    {
        private static readonly object _mutex = new object();
        private static SeguimientoJuntaBL _instance;

        private SeguimientoJuntaBL()
        {
        }

        /// <summary>
        /// Patron De Singleton
        /// </summary>
        public static SeguimientoJuntaBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SeguimientoJuntaBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Propiedad de conveniencia para acceder a Cache
        /// </summary>
        private static Cache cache
        {
            get
            {
                return HttpRuntime.Cache;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        private GrdSeguimientoJunta ObtenerDetalleJunta(int proyectoID, int juntaSpoolID)
        {
            //Teóricamente este SP solo regresa una fila, pero hay que asegurarnos.
            DataTable tabla = ObtenerDataSetParaSeguimientoJunta(proyectoID, null, null, juntaSpoolID, true).Tables[0];
            DataRow detalle;

            #region Valida resultado

            DataRow[] filas = tabla.Select(string.Format("GeneralJuntaSpoolID = {0}", juntaSpoolID));

            if (filas.Length != 1)
            {
                throw new Exception(string.Format("Se esperaba una fila para JuntaSpoolID={0} y se obtuvieron {1}", juntaSpoolID, filas.Length));
            }

            detalle = filas[0];

            #endregion

            return new GrdSeguimientoJunta
            {
                JuntaWorkstatusID = detalle.Field<int?>("GeneralJuntaWorkstatusID"),
                FabArea = detalle.Field<string>("GeneralFabArea"),
                FamAcero1 = detalle.Field<string>("GeneralFamiliaAcero1"),
                ItemCode1 = detalle.Field<string>("CodigoItemCodeMaterial1"),
                ItemCode2 = detalle.Field<string>("CodigoItemCodeMaterial2"),
                DescItemCode1 = detalle.Field<string>("DescripcionItemCodeMaterial1"),
                DescItemCode2 = detalle.Field<string>("DescripcionItemCodeMaterial2"),
                ArmadoFecha = detalle.Field<DateTime?>("ArmadoFecha"),
                ArmadoFechaReporte = detalle.Field<DateTime?>("ArmadoFechaReporte"),
                ArmadoNumeroUnico1 = detalle.Field<string>("ArmadoNumeroUnico1"),
                ArmadoNumeroUnico2 = detalle.Field<string>("ArmadoNumeroUnico2"),
                ArmadoTaller = detalle.Field<string>("ArmadoTaller"),
                ArmadoTubero = detalle.Field<string>("ArmadoTubero"),

                AreaTrabajoTubero = detalle.Field<string>("AreaTrabajoTubero"),

                ArmadoEtiquetaMaterial1 = detalle.Field<string>("ArmadoEtiquetaMaterial1"),
                ArmadoEtiquetaMaterial2 = detalle.Field<string>("ArmadoEtiquetaMaterial2"),
                ArmadoSpool1 = detalle.Field<string>("ArmadoSpool1"),
                ArmadoSpool2 = detalle.Field<string>("ArmadoSpool2"),
                EmbarqueEtiqueta = detalle.Field<string>("EmbarqueEtiqueta"),
                EmbarqueFechaEmbarque = detalle.Field<DateTime?>("EmbarqueFechaEmbarque"),
                EmbarqueFechaEtiqueta = detalle.Field<DateTime?>("EmbarqueFechaEtiqueta"),
                EmbarqueFolioPreparacion = detalle.Field<string>("EmbarqueFolioPreparaqcion"),
                EmbarqueNumeroEmbarque = detalle.Field<string>("EmbarqueNumeroEmbarque"),
                Cedula = detalle.Field<string>("GeneralCedula"),
                Diametro = detalle.Field<decimal?>("GeneralDiametro"),
                Espesor = detalle.Field<decimal?>("GeneralEspesor"),
                Peqs = detalle.Field<decimal?>("GeneralPeqs"),
                KgTeoricos = detalle.Field<decimal?>("GeneralKgTeoricos"),
                Junta = detalle.Field<string>("GeneralJunta"),
                Localizacion = detalle.Field<string>("GeneralLocalizacion"),
                NumeroDeControl = detalle.Field<string>("GeneralNumeroDeControl"),
                OrdenDeTrabajo = detalle.Field<string>("GeneralOrdenDeTrabajo"),
                Proyecto = detalle.Field<string>("GeneralProyecto"),
                Spool = detalle.Field<string>("GeneralSpool"),
                TieneHold = TraductorEnumeraciones.TextoSiNo(detalle.Field<bool?>("GeneralTieneHold").GetValueOrDefault(false)),
                TipoJunta = detalle.Field<string>("GeneralTipoJunta"),
                UltimoProceso = detalle.Field<string>("GeneralUltimoProceso"),
                RequierePWHT = TraductorEnumeraciones.TextoSiNo(detalle.Field<bool?>("GeneralPWHT").GetValueOrDefault(false)),
                InspeccionDimensionalFecha = detalle.Field<DateTime?>("InspeccionDimensionalFecha"),
                InspeccionDimensionalFechaReporte = detalle.Field<DateTime?>("InspeccionDimensionalFechaReporte"),
                InspeccionDimensionalHoja = detalle.Field<int?>("InspeccionDimensionalHoja"),
                InspeccionDimensionalNumeroReporte = detalle.Field<string>("InspeccionDimensionalNumeroReporte"),
                InspeccionDimensionalResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("InspeccionDimensionalResultado")),
                InspeccionEspesoresFecha = detalle.Field<DateTime?>("InspeccionEspesoresFecha"),
                InspeccionEspesoresFechaReporte = detalle.Field<DateTime?>("InspeccionEspesoresFechaReporte"),
                InspeccionEspesoresHoja = detalle.Field<int?>("InspeccionEspesoresHoja"),
                InspeccionEspesoresNumeroReporte = detalle.Field<string>("InspeccionEspesoresNumeroReporte"),
                InspeccionEspesoresResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("InspeccionEspesoresResultado")),

                InspeccionVisualInspector = detalle.Field<string>("InspeccionVisualInspector"),
                InspeccionDimensionalInspector = detalle.Field<string>("InspeccionDimensionalInspector"),

                InspeccionVisualDefecto = detalle.Field<string>("InspeccionVisualDefecto"),
                InspeccionVisualFecha = detalle.Field<DateTime?>("InspeccionVisualFecha"),
                InspeccionVisualFechaReporte = detalle.Field<DateTime?>("InspeccionVisualFechaReporte"),
                InspeccionVisualHoja = detalle.Field<int?>("InspeccionVisualHoja"),
                InspeccionVisualNumeroReporte = detalle.Field<string>("InspeccionVisualNumeroReporte"),
                InspeccionVisualResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("InspeccionVisualResultado")),
                PinturaCodigo = detalle.Field<string>("PinturaCodigo"),
                PinturaColor = detalle.Field<string>("PinturaColor"),
                PinturaFechaAcabadoVisual = detalle.Field<DateTime?>("PinturaFechaAcabadoVisual"),
                PinturaFechaAdherencia = detalle.Field<DateTime?>("PinturaFechaAdherencia"),
                PinturaFechaIntermedios = detalle.Field<DateTime?>("PinturaFechaIntermedios"),
                PinturaFechaPrimarios = detalle.Field<DateTime?>("PinturaFechaPrimarios"),
                PinturaFechaPullOff = detalle.Field<DateTime?>("PinturaFechaPullOff"),
                PinturaFechaRequisicion = detalle.Field<DateTime?>("PinturaFechaRequisicion"),
                PinturaFechaSandBlast = detalle.Field<DateTime?>("PinturaFechaSendBlast"),
                PinturaNumeroRequisicion = detalle.Field<string>("PinturaNumeroRequisicion"),
                PinturaReporteAcabadoVisual = detalle.Field<string>("PinturaReporteAcabadoVisual"),
                PinturaReporteAdherencia = detalle.Field<string>("PinturaReporteAdherencia"),
                PinturaReporteIntermedios = detalle.Field<string>("PinturaReporteIntermedios"),
                PinturaReportePrimarios = detalle.Field<string>("PinturaReportePrimarios"),
                PinturaReportePullOff = detalle.Field<string>("PinturaReportePullOff"),
                PinturaReporteSandBlast = detalle.Field<string>("PinturaReporteSendBlast"),
                PinturaSistema = detalle.Field<string>("PinturaSistema"),
                PruebaPTCodigoRequisicion = detalle.Field<string>("PruebaPTCodigoRequisicion"),
                PruebaPTDefecto = detalle.Field<string>("PruebaPTDefecto"),
                PruebaPTFechaPrueba = detalle.Field<DateTime?>("PruebaPTFechaPrueba"),
                PruebaPTFechaReporte = detalle.Field<DateTime?>("PruebaPTFechaReporte"),
                PruebaPTFechaRequisicion = detalle.Field<DateTime?>("PruebaPTFechaRequisicion"),
                PruebaPTHoja = detalle.Field<int?>("PruebaPTHoja"),
                PruebaPTNumeroReporte = detalle.Field<string>("PruebaPTNumeroReporte"),
                PruebaPTNumeroRequisicion = detalle.Field<string>("PruebaPTNumeroRequisicion"),
                PruebaPTPostTTCodigoRequisicion = detalle.Field<string>("PruebaPTPostTTCodigoRequisicion"),
                PruebaPTPostTTDefecto = detalle.Field<string>("PruebaPTPostTTDefecto"),
                PruebaPTPostTTFechaPrueba = detalle.Field<DateTime?>("PruebaPTPostTTFechaPrueba"),
                PruebaPTPostTTFechaReporte = detalle.Field<DateTime?>("PruebaPTPostTTFechaReporte"),
                PruebaPTPostTTFechaRequisicion = detalle.Field<DateTime?>("PruebaPTPostTTFechaRequisicion"),
                PruebaPTPostTTHoja = detalle.Field<int?>("PruebaPTPostTTHoja"),
                PruebaPTPostTTNumeroReporte = detalle.Field<string>("PruebaPTPostTTNumeroReporte"),
                PruebaPTPostTTNumeroRequisicion = detalle.Field<string>("PruebaPTPostTTNumeroRequisicion"),
                PruebaPTPostTTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaPTPostTTResultado")),
                PruebaPTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaPTResultado")),
                PruebaRTCodigoRequisicion = detalle.Field<string>("PruebaRTCodigoRequisicion"),
                PruebaRTDefecto = detalle.Field<string>("PruebaRTDefecto"),
                PruebaRTFechaPrueba = detalle.Field<DateTime?>("PruebaRTFechaPrueba"),
                PruebaRTFechaReporte = detalle.Field<DateTime?>("PruebaRTFechaReporte"),
                PruebaRTFechaRequisicion = detalle.Field<DateTime?>("PruebaRTFechaRequisicion"),
                PruebaRTHoja = detalle.Field<int?>("PruebaRTHoja"),
                PruebaRTNumeroReporte = detalle.Field<string>("PruebaRTNumeroReporte"),
                PruebaRTNumeroRequisicion = detalle.Field<string>("PruebaRTNumeroRequisicion"),
                PruebaRTPostTTCodigoRequisicion = detalle.Field<string>("PruebaRTPostTTCodigoRequisicion"),
                PruebaRTPostTTDefecto = detalle.Field<string>("PruebaRTPostTTDefecto"),
                PruebaRTPostTTFechaPrueba = detalle.Field<DateTime?>("PruebaRTPostTTFechaPrueba"),
                PruebaRTPostTTFechaReporte = detalle.Field<DateTime?>("PruebaRTPostTTFechaReporte"),
                PruebaRTPostTTFechaRequisicion = detalle.Field<DateTime?>("PruebaRTPostTTFechaRequisicion"),
                PruebaRTPostTTHoja = detalle.Field<int?>("PruebaRTPostTTHoja"),
                PruebaRTPostTTNumeroReporte = detalle.Field<string>("PruebaRTPostTTNumeroReporte"),
                PruebaRTPostTTNumeroRequisicion = detalle.Field<string>("PruebaRTPostTTNumeroRequisicion"),
                PruebaRTPostTTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaRTPostTTResultado")),
                PruebaPMICodigoRequisicion = detalle.Field<string>("PruebaPMICodigoRequisicion"),
                PruebaPMIDefecto = detalle.Field<string>("PruebaPMIDefecto"),
                PruebaPMIFechaPrueba = detalle.Field<DateTime?>("PruebaPMIFechaPrueba"),
                PruebaPMIFechaReporte = detalle.Field<DateTime?>("PruebaPMIFechaReporte"),
                PruebaPMIFechaRequisicion = detalle.Field<DateTime?>("PruebaPMIFechaRequisicion"),
                PruebaPMIHoja = detalle.Field<int?>("PruebaPMIHoja"),
                PruebaPMINumeroReporte = detalle.Field<string>("PruebaPMINumeroReporte"),
                PruebaPMINumeroRequisicion = detalle.Field<string>("PruebaPMINumeroRequisicion"),
                PruebaPMIResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaPMIResultado")),
                PruebaRTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaRTResultado")),
                PruebaUTCodigoRequisicion = detalle.Field<string>("PruebaUTCodigoRequisicion"),
                PruebaUTDefecto = detalle.Field<string>("PruebaUTDefecto"),
                PruebaUTFechaPrueba = detalle.Field<DateTime?>("PruebaUTFechaPrueba"),
                PruebaUTFechaReporte = detalle.Field<DateTime?>("PruebaUTFechaReporte"),
                PruebaUTFechaRequisicion = detalle.Field<DateTime?>("PruebaUTFechaRequisicion"),
                PruebaUTHoja = detalle.Field<int?>("PruebaUTHoja"),
                PruebaUTNumeroReporte = detalle.Field<string>("PruebaUTNumeroReporte"),
                PruebaUTNumeroRequisicion = detalle.Field<string>("PruebaUTNumeroRequisicion"),
                PruebaUTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("PruebaUTResultado")),
                SoldaduraConsumiblesRelleno = detalle.Field<string>("SoldaduraConsumiblesRelleno"),
                SoldaduraFecha = detalle.Field<DateTime?>("SoldaduraFecha"),
                SoldaduraFechaReporte = detalle.Field<DateTime?>("SoldaduraFechaReporte"),
                SoldaduraProcesoRaiz = detalle.Field<string>("SoldaduraProcesoRaiz"),
                SoldaduraProcesoRelleno = detalle.Field<string>("SoldaduraProcesoRelleno"),
                SoldaduraSoldadorRaiz = detalle.Field<string>("SoldaduraSoldadorRaiz"),
                SoldaduraSoldadorRelleno = detalle.Field<string>("SoldaduraSoldadorRelleno"),
                SoldaduraTaller = detalle.Field<string>("SoldaduraTaller"),
                SoldaduraWPS = detalle.Field<string>("SoldaduraWps"),
                SoldaduraWPSRelleno = detalle.Field<string>("SoldaduraWpsRelleno"),
                
                AreaTrabajoRaiz = detalle.Field<string>("AreaTrabajoRaiz"),
                AreaTrabajoRelleno = detalle.Field<string>("AreaTrabajoRelleno"),

                TratamientoDurezasCodigoRequisicion = detalle.Field<string>("TratamientoDurezasCodigoRequisicion"),
                TratamientoDurezasFechaReporte = detalle.Field<DateTime?>("TratamientoDurezasFechaReporte"),
                TratamientoDurezasFechaRequisicion = detalle.Field<DateTime?>("TratamientoDurezasFechaRequisicion"),
                TratamientoDurezasFechaTratamiento = detalle.Field<DateTime?>("TratamientoDurezasFechaTratamiento"),
                TratamientoDurezasGrafica = detalle.Field<string>("TratamientoDurezasGrafica"),
                TratamientoDurezasHoja = detalle.Field<int?>("TratamientoDurezasHoja"),
                TratamientoDurezasNumeroReporte = detalle.Field<string>("TratamientoDurezasNumeroReporte"),
                TratamientoDurezasNumeroRequisicion = detalle.Field<string>("TratamientoDurezasNumeroRequisicion"),
                TratamientoDurezasResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("TratamientoDurezasResultado")),
                TratamientoPreheatCodigoRequisicion = detalle.Field<string>("TratamientoPreHeatCodigoRequisicion"),
                TratamientoPreheatFechaReporte = detalle.Field<DateTime?>("TratamientoPreHeatFechaReporte"),
                TratamientoPreheatFechaRequisicion = detalle.Field<DateTime?>("TratamientoPreHeatFechaRequisicion"),
                TratamientoPreheatFechaTratamiento = detalle.Field<DateTime?>("TratamientoPreHeatFechaTratamiento"),
                TratamientoPreheatGrafica = detalle.Field<string>("TratamientoPreHeatGrafica"),
                TratamientoPreheatHoja = detalle.Field<int?>("TratamientoPreHeatHoja"),
                TratamientoPreheatNumeroReporte = detalle.Field<string>("TratamientoPreHeatNumeroReporte"),
                TratamientoPreheatNumeroRequisicion = detalle.Field<string>("TratamientoPreHeatNumeroRequisicion"),
                TratamientoPreheatResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("TratamientoPreHeatResultado")),
                TratamientoPWHTCodigoRequisicion = detalle.Field<string>("TratamientoPwhtCodigoRequisicion"),
                TratamientoPWHTFechaReporte = detalle.Field<DateTime?>("TratamientoPwhtFechaReporte"),
                TratamientoPWHTFechaRequisicion = detalle.Field<DateTime?>("TratamientoPwhtFechaRequisicion"),
                TratamientoPWHTFechaTratamiento = detalle.Field<DateTime?>("TratamientoPwhtFechaTratamiento"),
                TratamientoPWHTGrafica = detalle.Field<string>("TratamientoPwhtGrafica"),
                TratamientoPWHTHoja = detalle.Field<int?>("TratamientoPwhtHoja"),
                TratamientoPWHTNumeroReporte = detalle.Field<string>("TratamientoPwhtNumeroReporte"),
                TratamientoPWHTNumeroRequisicion = detalle.Field<string>("TratamientoPwhtNumeroRequisicion"),
                TratamientoPWHTResultado = TraductorEnumeraciones.TextoAprobadoONoAprobado(detalle.Field<bool?>("TratamientoPwhtResultado"))
            };
        }


        /// <summary>
        /// Regresa la lista para llenar el grid de Seguimiento de juntas, si el parametro juntaSpoolID contiene un valor, solo una elemento sera regresado
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="juntaSpoolID"></param>
        /// <param name="esJuntaDeCampo"></param>
        /// <returns></returns>
        public GrdSeguimientoJunta ObtenerDetalleSeguimientoJunta(int proyectoID, int juntaSpoolID, bool esJuntaDeCampo)
        {
            GrdSeguimientoJunta grdSgJta = new GrdSeguimientoJunta();

            //Si el proyectoID no existe
            if (proyectoID <= 0)
            {
                return grdSgJta;
            }

            using (SamContext ctx = new SamContext())
            {
                grdSgJta = ObtenerDetalleJunta(proyectoID, juntaSpoolID);

                DetalleJunta junta = DetalleJunta.ObtenerInstaciaConcreta(proyectoID, juntaSpoolID, grdSgJta.JuntaWorkstatusID, esJuntaDeCampo);

                junta.ComplementaInformacion(ctx, grdSgJta);
            }

            return grdSgJta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="juntaWorkstatusID"></param>
        /// <param name="historialRep"></param>
        /// <param name="expresionOrdenamiendo"></param>
        /// <returns></returns>
        public DataSet ObtenerDataSetParaSeguimientoJunta(int proyectoID, int? ordenTrabajoID, int? ordenTrabajoSpoolID, int? juntaSpoolID, bool historialRep)
        {
            const string nombreTabla = "SegJunta";
            DataSet ds;

            bool insertaEnCache = false;
            string llaveCache = "ObtenerDataSetParaSeguimientoJunta_" + proyectoID + "_" + historialRep;

            if (ordenTrabajoID.GetValueOrDefault(0) <= 0 && ordenTrabajoSpoolID.GetValueOrDefault(0) <= 0 && juntaSpoolID.GetValueOrDefault(0) <= 0)
            {
                //Significa que quieren filtrar por proyecto, vamos a ver si lo podemos tomar de Cache para ahorrarnos chamba, traer toda la info de un proyecto es pesado
                ds = (DataSet)cache.Get(llaveCache);

                if (ds != null)
                {
                    return ds;
                }
            }


            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                //Ejecutamos el SP más específico que se pueda
                if (juntaSpoolID.HasValue && juntaSpoolID.Value > 0)
                {
                    ds = ejecutaSpSeguimientoPorJs(connection, nombreTabla, juntaSpoolID.Value);
                }
                else if (ordenTrabajoSpoolID.HasValue && ordenTrabajoSpoolID.Value > 0)
                {
                    ds = ejecutaSpSeguimientoPorOdts(connection, nombreTabla, ordenTrabajoSpoolID.Value, historialRep);
                }
                else if (ordenTrabajoID.HasValue && ordenTrabajoID.Value > 0)
                {
                    ds = ejecutaSpSeguimientoPorOdt(connection, nombreTabla, ordenTrabajoID.Value, historialRep);
                }
                else
                {
                    ds = ejecutaSpSeguimientoPorProyecto(connection, nombreTabla, proyectoID, historialRep);
                    insertaEnCache = true;
                }

                if (insertaEnCache)
                {
                    //En caso que el filtro haya sido por proyecto vamos a meterlo a Cache
                    cache.Insert(llaveCache,
                                ds,
                                null,
                                DateTime.Now.AddMinutes(Configuracion.CacheMuyPocosMinutos),
                                Cache.NoSlidingExpiration);
                }

                return ds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="proyectoID"></param>
        /// <param name="historialRep"></param>
        /// <returns></returns>
        private DataSet ejecutaSpSeguimientoPorProyecto(IDbConnection connection, string nombreTabla, int proyectoID, bool historialRep)
        {
            const string nombreProc = "ObtenerSeguimientoDeJuntasPorProyecto";

            DataSet ds = new DataSet();
            IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
            parameters[0].Value = proyectoID;
            parameters[1].Value = historialRep;


            return DataAccess.ExecuteDataset(connection,
                                                CommandType.StoredProcedure,
                                                nombreProc,
                                                ds,
                                                nombreTabla,
                                                parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="historialRep"></param>
        /// <returns></returns>
        private DataSet ejecutaSpSeguimientoPorOdt(IDbConnection connection, string nombreTabla, int ordenTrabajoID, bool historialRep)
        {
            const string nombreProc = "ObtenerSeguimientoDeJuntasPorOrdenTrabajo";

            DataSet ds = new DataSet();
            IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
            parameters[0].Value = ordenTrabajoID;
            parameters[1].Value = historialRep;


            return DataAccess.ExecuteDataset(connection,
                                                CommandType.StoredProcedure,
                                                nombreProc,
                                                ds,
                                                nombreTabla,
                                                parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="historialRep"></param>
        /// <returns></returns>
        private DataSet ejecutaSpSeguimientoPorOdts(IDbConnection connection, string nombreTabla, int ordenTrabajoSpoolID, bool historialRep)
        {
            const string nombreProc = "ObtenerSeguimientoDeJuntasPorOrdenTrabajoSpool";

            DataSet ds = new DataSet();
            IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
            parameters[0].Value = ordenTrabajoSpoolID;
            parameters[1].Value = historialRep;


            return DataAccess.ExecuteDataset(connection,
                                                CommandType.StoredProcedure,
                                                nombreProc,
                                                ds,
                                                nombreTabla,
                                                parameters);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="nombreTabla"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="historialRep"></param>
        /// <returns></returns>
        private DataSet ejecutaSpSeguimientoPorJs(IDbConnection connection, string nombreTabla, int juntaSpoolID)
        {
            const string nombreProc = "ObtenerSeguimientoDeJuntasPorJuntaSpool";

            DataSet ds = new DataSet();
            IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
            parameters[0].Value = juntaSpoolID;

            return DataAccess.ExecuteDataset(connection,
                                                CommandType.StoredProcedure,
                                                nombreProc,
                                                ds,
                                                nombreTabla,
                                                parameters);
        }

        /// <summary>
        /// Genera una nueva personalizacion
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public void GuardaPersonalizacionSegmentoJunta(PersonalizacionSeguimientoJunta pers)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.PersonalizacionSeguimientoJunta.Where(x => x.Nombre == pers.Nombre).Any())
                    {
                        ctx.PersonalizacionSeguimientoJunta.ApplyChanges(pers);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { "Error de concurrencia" });
            }
        }

        /// <summary>
        /// trae el id de la personalizacion guardada
        /// </summary>
        /// <param name="nombre">Nombre de la personalizacion</param>
        /// <returns>int = Id de la personalizacion</returns>
        public int ObtenerPersonalizacionSeguimentoJuntaID(string nombre)
        {
            using (SamContext ctx = new SamContext())
            {
                return
                    ctx.PersonalizacionSeguimientoJunta.Where(x => x.Nombre == nombre).Select(
                        x => x.PersonalizacionSeguimientoJuntaID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Guara todos los campos a mostrar en seguimiento Junta en la base de datos relacionado a una personalizacion
        /// </summary>
        /// <param name="campos">lista de los campos a generar</param>
        public void GuardaDetallePersonalizacionSeguimientoJunta(List<DetallePersonalizacionSeguimientoJunta> campos)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    foreach (var detallePersonalizacionSeguimiento in campos)
                    {
                        ctx.DetallePersonalizacionSeguimientoJunta.ApplyChanges(detallePersonalizacionSeguimiento);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { "Error de concurrencia" });
            }
        }

        public List<PersonalizacionSeguimientoJunta> ObtenerPersonalizacion(System.Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<PersonalizacionSeguimientoJunta> persSeguimJunta =
                    ctx.PersonalizacionSeguimientoJunta.Where(x => x.UserId == userID);
                return persSeguimJunta.ToList();
            }
        }


        public List<DetallePersonalizacionSeguimientoJunta> ObtenerDetallePersonalizacion(int personalizacionSeguimientoJuntaID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<DetallePersonalizacionSeguimientoJunta> detallepersSeguimJunta =
                    ctx.DetallePersonalizacionSeguimientoJunta.Where(x => x.PersonalizacionSeguimientoJuntaID == personalizacionSeguimientoJuntaID);
                return detallepersSeguimJunta.ToList();
            }
        }

        public void BorrarPersonalizacionSeguimientoJunta(int PersSegJuntaID)
        {
            List<DetallePersonalizacionSeguimientoJunta> detsegJunta = new List<DetallePersonalizacionSeguimientoJunta>();

            using (SamContext ctx = new SamContext())
            {
                detsegJunta =
                    ctx.DetallePersonalizacionSeguimientoJunta.Where(
                        x => x.PersonalizacionSeguimientoJuntaID == PersSegJuntaID).ToList();

                foreach (var detallePersSeguimientoJunta in detsegJunta)
                {
                    ctx.DeleteObject(detallePersSeguimientoJunta);
                }

                PersonalizacionSeguimientoJunta PersSeguimientoJunta =
                    ctx.PersonalizacionSeguimientoJunta.FirstOrDefault(
                        x => x.PersonalizacionSeguimientoJuntaID == PersSegJuntaID);

                ctx.DeleteObject(PersSeguimientoJunta);

                ctx.SaveChanges();
            }
        }

        public void MensajeErrorChk()
        {
            throw new ExcepcionSeguimiento(new List<string> { MensajesError.Excepcion_SegimientoSpoolChk });
        }

    
        public List<string> ObtenerNombreCampoSeguimientoJunta(int[] CSJId)
        {
            List<string> NombresCampoSeguimientoJunta = null;

            using (SamContext ctx = new SamContext())
            {
                NombresCampoSeguimientoJunta = ctx.CampoSeguimientoJunta.Where(x => CSJId.Contains(x.CampoSeguimientoJuntaID)).Select(y => y.NombreColumnaSp).ToList();
            }
            return NombresCampoSeguimientoJunta;
        }

        public List<string> ObtenerTodosLosCamposSeguimientoJunta(string idioma,string columna)
        {
            List<string> NombresCampoSeguimientoJunta = null;

            using (SamContext ctx = new SamContext())
            {
                if(LanguageHelper.INGLES==idioma)
                    NombresCampoSeguimientoJunta = ctx.CampoSeguimientoJunta.Where(x => !x.NombreIngles.Contains(columna)).Select(y => y.NombreColumnaSp).ToList();
                else
                    NombresCampoSeguimientoJunta = ctx.CampoSeguimientoJunta.Where(x => !x.Nombre.Contains(columna)).Select(y => y.NombreColumnaSp).ToList();
               
            }
            return NombresCampoSeguimientoJunta;
        }


    }
}
