using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using System.Data.Objects;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Destajos;
using System.Threading;
using System.Transactions;
using SAM.BusinessLogic.Excepciones;

namespace SAM.BusinessLogic.Administracion
{
    public class DestajoBL
    {
        decimal valorCosto;
        private static readonly object _mutex = new object();
        private static DestajoBL _instance;
        private const decimal MAXIMO_COSTO = (decimal)999999.9999;
        List<string> errores;
        List<DiametroCache> diam = CacheCatalogos.Instance.ObtenerDiametros();
        List<CedulaCache> ced = CacheCatalogos.Instance.ObtenerCedulas();

        private DestajoBL()
        {
        }

        public static DestajoBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DestajoBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Este método se encarga de crear un nuevo periodo de destajo en la BD.  Al crear un periodo de destajo se inserta
        /// información en las siguientes tablas:
        /// 1. PeriodoDestajo
        /// 2. DestajoSoldador
        /// 3. DestajoSoldadorDetalle
        /// 4. DestajoTubero
        /// 5. DestajoTuberoDetalle
        /// 
        /// Debido a que la cantidad de información que potencialmente se envía a la BD en lugar de enviar estatuto por estuto
        /// este método hace una llamada a un solo SP el cual posteriormente hace los inserts correspondientes.
        /// El SP espera recibir la información PROCESADA y lista para inserción por lo cual en este lugar se llevan a cabo
        /// todos los cálculos, el SP recibe un par de Xmls con la información para los tuberos y los soldadores.
        /// </summary>
        /// <param name="anio">Año del periodo de destajo</param>
        /// <param name="semana">Semana del perioso de destajo</param>
        /// <param name="fechaInicio">Fecha de inicio del periodo</param>
        /// <param name="fechaFin">Fecha final del periodo</param>
        /// <param name="diasFestivos">Número de días festivos que tiene el periodo</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public int GeneraNuevoPeriodoDestajo(int anio, string semana, DateTime fechaInicio, DateTime fechaFin, int diasFestivos, Guid userID)
        {
            int periodoDestajoID = -1;

            try
            {
                List<Soldador> lstSoldadores;
                List<Tubero> lstTuberos;
                List<CostoArmado> lstCostoArmado;
                List<CostoProcesoRaiz> lstCostoRaiz;
                List<CostoProcesoRelleno> lstCostoRelleno;
                List<ProyectoConfiguracion> lstConfiguracion;
                List<DestajoArmadoDetalle> lstArmado;
                List<DestajoSoldaduraDetalle> lstSoldadura;

                using (SamContext ctx = new SamContext())
                {
                    if (ctx.PeriodoDestajo.Any(x => x.Anio == anio && x.Semana == semana))
                    {
                        throw new ExcepcionDestajo(MensajesError.PeriodoDestajo_YaExiste);
                    }

                    ctx.Soldador.MergeOption = MergeOption.NoTracking;
                    ctx.Tubero.MergeOption = MergeOption.NoTracking;
                    ctx.CostoArmado.MergeOption = MergeOption.NoTracking;
                    ctx.CostoProcesoRaiz.MergeOption = MergeOption.NoTracking;
                    ctx.CostoProcesoRelleno.MergeOption = MergeOption.NoTracking;
                    ctx.ProyectoConfiguracion.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaArmado.MergeOption = MergeOption.NoTracking;
                    ctx.JuntaSoldadura.MergeOption = MergeOption.NoTracking;

                    #region Query directo a BD para ir por las juntas armadas que aun no tengan destajo de tuberos

                    //Trae la juntas armadas con fecha menor o igual a la fecha de fin del periodo (nótese como
                    //no son solo las juntas que caen en el rango).  Excluye las juntas que ya estén en algún
                    //otro destajo para no volverlas a pagar por armado
                    lstArmado =
                        (from ja in ctx.JuntaArmado
                         join jw in ctx.JuntaWorkstatus on ja.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                         join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                         join s in ctx.Spool on js.SpoolID equals s.SpoolID
                         where ja.FechaArmado <= fechaFin //Incluso juntas fuera de rango
                                 && !ctx.DestajoTuberoDetalle.Select(x => x.JuntaWorkstatusID).Contains(ja.JuntaWorkstatusID)
                         select new DestajoArmadoDetalle
                         {
                             JuntaWorkstatusID = ja.JuntaWorkstatusID,
                             TuberoID = ja.TuberoID,
                             TipoJuntaID = js.TipoJuntaID,
                             FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                             ProyectoID = s.ProyectoID,
                             Diametro = js.Diametro,
                             Cedula = js.Cedula,
                             EsDePeriodoAnterior = ja.FechaArmado < fechaInicio //Si están fuera de rango indicarlo en el bit
                         }).ToList();

                    #endregion

                    #region Query directo a BD para ir por las juntas soldadas que aun no tengas destajo

                    //Trae la juntas soldadas con fecha menor o igual a la fecha de fin del periodo (nótese como
                    //no son solo las juntas que caen en el rango).  Excluye las juntas que ya estén en algún
                    //otro destajo para no volverlas a pagar por soldadura.
                    lstSoldadura =
                        (from sold in ctx.JuntaSoldadura
                         join jw in ctx.JuntaWorkstatus on sold.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                         join det in ctx.
                         JuntaSoldaduraDetalle on sold.JuntaSoldaduraID equals det.JuntaSoldaduraID
                         join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                         join s in ctx.Spool on js.SpoolID equals s.SpoolID
                         where sold.FechaSoldadura <= fechaFin //Incluso juntas fuera de rango
                                 && !ctx.DestajoSoldadorDetalle.Select(x => x.JuntaWorkstatusID).Contains(sold.JuntaWorkstatusID)
                         select new DestajoSoldaduraDetalle
                         {
                             JuntaWorkstatusID = sold.JuntaWorkstatusID,
                             SoldadorID = det.SoldadorID,
                             TecnicaSoldadorID = det.TecnicaSoldadorID,
                             ProcesoRaizID = sold.ProcesoRaizID ?? -1,
                             ProcesoRellenoID = sold.ProcesoRellenoID ?? -1,
                             TipoJuntaID = js.TipoJuntaID,
                             FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                             ProyectoID = s.ProyectoID,
                             Diametro = js.Diametro,
                             Cedula = js.Cedula,
                             EsDePeriodoAnterior = sold.FechaSoldadura < fechaInicio //Si están fuera de rango indicarlo en el bit
                         }).Distinct().ToList(); // excluimos duplicados a causa de soldadores con varias coladas en una misma junta

                    #endregion

                    //Traer todos los soldadores y tuberos activos (no deben ser tantos)
                    lstSoldadores =
                        ctx.Soldador.Where(x => x.Activo).ToList();

                    lstTuberos =
                        ctx.Tubero.Where(x => x.Activo).ToList();

                    #region Traer las tablas de costo para los proyectos que necesitemos

                    IQueryable<int> iQPidsSoldadura =
                        lstSoldadura.Select(x => x.ProyectoID).Distinct().AsQueryable();

                    IQueryable<int> iQPidsArmado =
                        lstArmado.Select(x => x.ProyectoID).Distinct().AsQueryable();

                    lstCostoArmado =
                        ctx.CostoArmado
                           .Where(ca => iQPidsArmado.Contains(ca.ProyectoID)).ToList();

                    lstCostoRaiz =
                        ctx.CostoProcesoRaiz.Where(cra => iQPidsSoldadura.Contains(cra.ProyectoID)).ToList();

                    lstCostoRelleno =
                        ctx.CostoProcesoRelleno.Where(cre => iQPidsSoldadura.Contains(cre.ProyectoID)).ToList();

                    #endregion

                    //Traer la configuración del proyecto para saber en base a que se pagan los cuadros
                    //nos podemos traer todos los proyectos no son tantos
                    lstConfiguracion =
                        ctx.ProyectoConfiguracion.ToList();
                }

                Dictionary<decimal, int> diametros = CacheCatalogos.Instance
                                                                   .ObtenerDiametros()
                                                                   .ToDictionary(x => x.Valor, x => x.ID);

                Dictionary<string, int> cedulas = CacheCatalogos.Instance
                                                                .ObtenerCedulas()
                                                                .ToDictionary(x => x.Nombre, y => y.ID);

                #region Hacer los cálculos asíncronos para los destajos de soldadores y tuberos

                CalculoDestajoTuberos calcTuberos = new CalculoDestajoTuberos();
                CalculoDestajoSoldadores calcSoldadores = new CalculoDestajoSoldadores();

                Thread tTuberos = new Thread(() => calcTuberos.CalculaDestajoTuberos(diasFestivos,
                                                                                        lstTuberos,
                                                                                        lstCostoArmado,
                                                                                        lstConfiguracion,
                                                                                        lstArmado,
                                                                                        diametros,
                                                                                        cedulas));

                Thread tSoldadores = new Thread(() => calcSoldadores.CalculaDestajoSoldadores(diasFestivos,
                                                                                                lstSoldadores,
                                                                                                lstCostoRaiz,
                                                                                                lstCostoRelleno,
                                                                                                lstConfiguracion,
                                                                                                lstSoldadura,
                                                                                                diametros,
                                                                                                cedulas));
                //Iniciar los threads
                tTuberos.Start();
                tSoldadores.Start();

                //Esperar a que ambos threads terminen
                tTuberos.Join();
                tSoldadores.Join();

                #endregion

                string xmlTuberos = calcTuberos.Xml;
                string xmlSoldadores = calcSoldadores.Xml;

                //Persistir información en la BD, hacerlo a través de un SP para evitar múltiples round-trips
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        periodoDestajoID =
                            ctx.GeneraPeriodoDestajo(semana,
                                                        anio,
                                                        fechaInicio,
                                                        fechaFin,
                                                        diasFestivos,
                                                        xmlTuberos,
                                                        xmlSoldadores,
                                                        userID).Single().Value;
                    }

                    scope.Complete();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }

            return periodoDestajoID;
        }


        /// <summary>
        /// Vuelve a calcular el destajo para un tubero o soldador en particular.
        /// En el recálculo del destajo los valores introducidos manualmente por el usuario se pierden y el sistema
        /// vuelve a hacer el cálculo completo.
        /// 
        /// De hecho el método de recálculo borra los registros y los vuelve a crear.
        /// </summary>
        /// <param name="esTubero">Indica si se trata de un tubero o soldador</param>
        /// <param name="idRegistro">DestajoTuberoID si esTubero=true, DestajoSoldadorID si esTubero=false</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public void RecalculaDestajoPersona(bool esTubero, int idRegistro, Guid userID)
        {
            try
            {
                if (esTubero)
                {
                    RecalculaDestajoTubero(idRegistro, userID);
                }
                else
                {
                    RecalculaDestajoSoldador(idRegistro, userID);
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_Concurrencia);
            }
        }


        /// <summary>
        /// Vuelve a calcular las juntas a pagar para un soldador en particular en un periodo en específico.
        /// Al recalcular un destajo de soldador se tiene que tener en cuenta que como las juntas pueden haber sido
        /// soldadas por más de una persona el recálculo de un soldador en particular puede afectar el destajo de otros
        /// soldadores.
        /// 
        /// Este proceso se encarga de detectar cuales son los soldadores afectados y en base a eso vuelve a recalcular
        /// el destajo para todos los afectados.  Al llevar a cabo este cálculo los registros en DestajoSoldador y DestajoSoldadorDetalle
        /// son eleiminados y regenerados nuevamente.  Cada uno de los nuevos registros
        /// generados dentro de la tabla DestajoSoldador se relacionan a un registro existente en la tabla PeriodoDestajo.
        /// </summary>
        /// <param name="destajoTuberoID">ID del destajo que se desea recalcular</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public void RecalculaDestajoSoldador(int destajoSoldadorID, Guid userID)
        {
            List<Soldador> lstSoldadores;
            List<CostoProcesoRaiz> lstCostoRaiz;
            List<CostoProcesoRelleno> lstCostoRelleno;
            List<ProyectoConfiguracion> lstConfiguracion;
            List<DestajoSoldaduraDetalle> lstSoldadura;
            DestajoSoldador destajo;
            PeriodoDestajo periodo;

            using (SamContext ctx = new SamContext())
            {
                ctx.Soldador.MergeOption = MergeOption.NoTracking;
                ctx.CostoArmado.MergeOption = MergeOption.NoTracking;
                ctx.CostoProcesoRaiz.MergeOption = MergeOption.NoTracking;
                ctx.CostoProcesoRelleno.MergeOption = MergeOption.NoTracking;
                ctx.ProyectoConfiguracion.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSoldadura.MergeOption = MergeOption.NoTracking;
                ctx.DestajoSoldador.MergeOption = MergeOption.NoTracking;
                ctx.PeriodoDestajo.MergeOption = MergeOption.NoTracking;

                //Traer el destajo del soldador seleccionado
                destajo = ctx.DestajoSoldador
                             .Where(x => x.DestajoSoldadorID == destajoSoldadorID)
                             .Single();

                //Traer la información del periodo al que corresponde
                periodo = ctx.PeriodoDestajo
                             .Where(p => p.PeriodoDestajoID == destajo.PeriodoDestajoID)
                             .Single();

                #region Queries para traer las juntas

                //Ids de las juntas workstatus que ya tienen destajo en cualquier otro periodo, salvo sobre el que queremos regenerar
                //soldadura.  Estas juntas se debe EXCLUIR a la hora de regenerar
                IQueryable<int> iqJwsPeriodo = ctx.DestajoSoldadorDetalle
                                                  .Where(x => !ctx.DestajoSoldador
                                                                  .Where(y => y.PeriodoDestajoID == periodo.PeriodoDestajoID)
                                                                  .Select(z => z.DestajoSoldadorID)
                                                                  .Contains(x.DestajoSoldadorID))
                                                  .Select(x => x.JuntaWorkstatusID);

                //Primero traer las juntas soldadas por el soldador en particular que caigan dentro del periodo
                List<int> lstJuntasSoldadas =
                    (from sold in ctx.JuntaSoldadura
                     join det in ctx.JuntaSoldaduraDetalle on sold.JuntaSoldaduraID equals det.JuntaSoldaduraID
                     where sold.FechaSoldadura <= periodo.FechaFin
                           && !iqJwsPeriodo.Contains(sold.JuntaWorkstatusID) //excluimos juntas pagadas en otros periodos
                           && det.SoldadorID == destajo.SoldadorID //solo las juntas del fulanito en cuestión
                     select sold.JuntaSoldaduraID).ToList();

                //Ids de JuntaSoldadura que se tienen que volver a calcular
                IQueryable<int> iqJtasSoldadas = lstJuntasSoldadas.Distinct().AsQueryable();

                //Detalles de JuntaSoldaduraDetalle que se tienen que incluir en el recálculo
                IQueryable<JuntaSoldaduraDetalle> iqDetSoldadura = ctx.JuntaSoldaduraDetalle
                                                                      .Where(x => iqJtasSoldadas.Contains(x.JuntaSoldaduraID));

                //Traer las juntas del soldador o soldadores que potencialmente fueron afectadas
                lstSoldadura =
                    (from sold in ctx.JuntaSoldadura
                     join jw in ctx.JuntaWorkstatus on sold.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                     join det in iqDetSoldadura on sold.JuntaSoldaduraID equals det.JuntaSoldaduraID
                     join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                     join s in ctx.Spool on js.SpoolID equals s.SpoolID
                     where sold.FechaSoldadura <= periodo.FechaFin
                            && !iqJwsPeriodo.Contains(sold.JuntaWorkstatusID)
                     select new DestajoSoldaduraDetalle
                     {
                         JuntaWorkstatusID = sold.JuntaWorkstatusID,
                         SoldadorID = det.SoldadorID,
                         TecnicaSoldadorID = det.TecnicaSoldadorID,
                         ProcesoRaizID = sold.ProcesoRaizID ?? -1,
                         ProcesoRellenoID = sold.ProcesoRellenoID ?? -1,
                         TipoJuntaID = js.TipoJuntaID,
                         FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                         ProyectoID = s.ProyectoID,
                         Diametro = js.Diametro,
                         Cedula = js.Cedula,
                         EsDePeriodoAnterior = sold.FechaSoldadura < periodo.FechaInicio
                     }).ToList();

                #endregion

                //Solos los soldadores activos que nos interesan
                IQueryable<int> iqSoldId = lstSoldadura.Select(x => x.SoldadorID).Distinct().AsQueryable();
                lstSoldadores = ctx.Soldador.Where(s => iqSoldId.Contains(s.SoldadorID) && s.Activo).ToList();

                #region Traer las tablas de costo para los proyectos que necesitemos

                IQueryable<int> iQPidsSoldadura =
                    lstSoldadura.Select(x => x.ProyectoID).Distinct().AsQueryable();

                lstCostoRaiz =
                    ctx.CostoProcesoRaiz.Where(cra => iQPidsSoldadura.Contains(cra.ProyectoID)).ToList();

                lstCostoRelleno =
                    ctx.CostoProcesoRelleno.Where(cre => iQPidsSoldadura.Contains(cre.ProyectoID)).ToList();

                #endregion

                //Traer la configuración del proyecto para saber en base a que se pagan los cuadros
                lstConfiguracion =
                    ctx.ProyectoConfiguracion.ToList();
            }

            Dictionary<decimal, int> diametros = CacheCatalogos.Instance
                                                               .ObtenerDiametros()
                                                               .ToDictionary(x => x.Valor, x => x.ID);

            Dictionary<string, int> cedulas = CacheCatalogos.Instance
                                                            .ObtenerCedulas()
                                                            .ToDictionary(x => x.Nombre, y => y.ID);

            CalculoDestajoSoldadores calcSoldadores = new CalculoDestajoSoldadores();


            calcSoldadores.CalculaDestajoSoldadores(periodo.CantidadDiasFestivos,
                                                        lstSoldadores,
                                                        lstCostoRaiz,
                                                        lstCostoRelleno,
                                                        lstConfiguracion,
                                                        lstSoldadura,
                                                        diametros,
                                                        cedulas);

            string xmlSoldadores = calcSoldadores.Xml;

            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.RegeneraDestajoSoldador(periodo.PeriodoDestajoID, periodo.CantidadDiasFestivos, xmlSoldadores, userID);

                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// Elimina el registro DestajoTubero y los registros DestajoTuberoDetalla para volver
        /// a recalcular las juntas a pagar y pasar por el proceso como si se tratase de un destajo nuevo.
        /// La diferencia es que en lugar de crear un nuevo registro en PeriodoDestajo simplemente
        /// se relaciona el registro a insertar de DestajoTubero con un periodo ya existente.
        /// </summary>
        /// <param name="destajoTuberoID">ID del destajo que se desea recalcular</param>
        /// <param name="userID">Usuario que lleva a cabo la operación</param>
        public static void RecalculaDestajoTubero(int destajoTuberoID, Guid userID)
        {
            List<CostoArmado> lstCostoArmado;
            List<ProyectoConfiguracion> lstConfiguracion;
            List<DestajoArmadoDetalle> lstArmado;
            DestajoTubero destajo;
            PeriodoDestajo periodo;

            using (SamContext ctx = new SamContext())
            {
                ctx.DestajoTubero.MergeOption = MergeOption.NoTracking;
                ctx.PeriodoDestajo.MergeOption = MergeOption.NoTracking;
                ctx.JuntaArmado.MergeOption = MergeOption.NoTracking;
                ctx.JuntaWorkstatus.MergeOption = MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                ctx.Spool.MergeOption = MergeOption.NoTracking;
                ctx.DestajoTuberoDetalle.MergeOption = MergeOption.NoTracking;
                ctx.CostoArmado.MergeOption = MergeOption.NoTracking;
                ctx.ProyectoConfiguracion.MergeOption = MergeOption.NoTracking;

                //Traer el destajo del tubero seleccionado
                destajo = ctx.DestajoTubero
                             .Where(x => x.DestajoTuberoID == destajoTuberoID)
                             .Single();

                //Traer la información del periodo al que corresponde
                periodo = ctx.PeriodoDestajo
                             .Where(p => p.PeriodoDestajoID == destajo.PeriodoDestajoID)
                             .Single();

                //Traer nuevamente las juntas armadas pero únicamente para el tubero seleccionado
                //hay que volver a consultar contra todas las juntas armadas excluyendo las ya pagadas (en otro periodo
                //de detajo)
                lstArmado =
                    (from ja in ctx.JuntaArmado
                     join jw in ctx.JuntaWorkstatus on ja.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                     join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                     join s in ctx.Spool on js.SpoolID equals s.SpoolID
                     where ja.FechaArmado <= periodo.FechaFin
                         && !ctx.DestajoTuberoDetalle
                                 .Where(x => x.DestajoTuberoID != destajoTuberoID)
                                 .Select(x => x.JuntaWorkstatusID)
                                 .Contains(ja.JuntaWorkstatusID)
                         && ja.TuberoID == destajo.TuberoID
                     select new DestajoArmadoDetalle
                     {
                         JuntaWorkstatusID = ja.JuntaWorkstatusID,
                         TuberoID = ja.TuberoID,
                         TipoJuntaID = js.TipoJuntaID,
                         FamiliaAceroID = js.FamiliaAceroMaterial1ID,
                         ProyectoID = s.ProyectoID,
                         Diametro = js.Diametro,
                         Cedula = js.Cedula,
                         EsDePeriodoAnterior = ja.FechaArmado < periodo.FechaInicio
                     }).ToList();

                IQueryable<int> iQPidsArmado = lstArmado.Select(x => x.ProyectoID).Distinct().AsQueryable();
                lstCostoArmado = ctx.CostoArmado.Where(ca => iQPidsArmado.Contains(ca.ProyectoID)).ToList();
                lstConfiguracion = ctx.ProyectoConfiguracion.ToList();
            }

            Dictionary<decimal, int> diametros = CacheCatalogos.Instance
                                                               .ObtenerDiametros()
                                                               .ToDictionary(x => x.Valor, x => x.ID);

            Dictionary<string, int> cedulas = CacheCatalogos.Instance
                                                            .ObtenerCedulas()
                                                            .ToDictionary(x => x.Nombre, y => y.ID);


            CalculoDestajoTuberos calcTubero = new CalculoDestajoTuberos();
            calcTubero.CalculaDestajoParaUnTubero(periodo.CantidadDiasFestivos,
                                                    destajo.TuberoID,
                                                    lstCostoArmado,
                                                    lstConfiguracion,
                                                    lstArmado,
                                                    diametros,
                                                    cedulas);

            string xmlTubero = calcTubero.Xml;

            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.RegeneraDestajoTubero(destajoTuberoID, periodo.PeriodoDestajoID, periodo.CantidadDiasFestivos, xmlTubero, userID);

                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="usuarioModifica"></param>
        /// <param name="familiaAceroID"></param>
        /// <param name="tipoJuntaID"></param>
        /// <param name="procesoValor"></param>
        /// <param name="procesoID"></param>
        /// <param name="proyectoID"></param>
        public void ProcesaDestajo(Stream stream, Guid usuarioModifica, int familiaAceroID, int tipoJuntaID, string procesoValor, int procesoID, int proyectoID)
        {
            string[] lineas = IOUtils.GetLinesFromTextStream(stream);

            if (lineas.Length == 0)
            {
                throw new ExcepcionDestajo(MensajesError.LineasX_Invalido);
            }

            errores = new List<string>();

            using (SamContext ctx = new SamContext())
            {

                // Seleccion de tipo de lista dependiendo del proceso (Relleno, Raiz, Armado)
                if (procesoValor.StartsWith("RE"))
                {
                    List<CostoProcesoRelleno> procesosRelleno = ctx.CostoProcesoRelleno.Where(x => x.FamiliaAceroID == familiaAceroID &&
                                                                                                  x.TipoJuntaID == tipoJuntaID &&
                                                                                                  x.ProcesoRellenoID == procesoID &&
                                                                                                  x.ProyectoID == proyectoID).ToList();

                    IterarArchivoRelleno(usuarioModifica, familiaAceroID, tipoJuntaID, procesoID, proyectoID, lineas, procesosRelleno, ctx);
                    if (errores.Count == 0)
                    {
                        ctx.SaveChanges();
                    }
                    else
                    {
                        throw new ExcepcionProcesos(errores);
                    }
                }
                else if (procesoValor.StartsWith("R"))
                {
                    List<CostoProcesoRaiz> procesosRaiz = ctx.CostoProcesoRaiz.Where(x => x.FamiliaAceroID == familiaAceroID &&
                                                                                          x.TipoJuntaID == tipoJuntaID &&
                                                                                          x.ProcesoRaizID == procesoID &&
                                                                                          x.ProyectoID == proyectoID).ToList();

                    IterarArchivoRaiz(usuarioModifica, familiaAceroID, tipoJuntaID, procesoID, proyectoID, lineas, procesosRaiz, ctx);
                    if (errores.Count == 0)
                    {
                        ctx.SaveChanges();
                    }
                    else
                    {
                        throw new ExcepcionProcesos(errores);
                    }
                }
                else
                {
                    List<CostoArmado> procesosArmado = ctx.CostoArmado.Where(x => x.FamiliaAceroID == familiaAceroID &&
                                                                                  x.TipoJuntaID == tipoJuntaID &&
                                                                                  x.ProyectoID == proyectoID).ToList();

                    IterarArchivoArmado(usuarioModifica, familiaAceroID, tipoJuntaID, proyectoID, lineas, procesosArmado, ctx);
                    if (errores.Count == 0)
                    {
                        ctx.SaveChanges();
                    }
                    else
                    {
                        throw new ExcepcionProcesos(errores);
                    }
                }
            }
        }

        public void IterarArchivoRelleno(Guid usuarioModifica, int familiaAceroID, int tipoJuntaID, int procesoID, int proyectoID, string[] lineas, List<CostoProcesoRelleno> procesosRelleno, SamContext ctx)
        {
                #region Iterar para cada renglon del archivo
                List<CostoProcesoRelleno> listaCostos = new List<CostoProcesoRelleno>();

                for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
                {
                    string linea = lineas[numLinea];
                    string[] palabras = linea.Split(',');
                    palabras = palabras.ToList().Select(p => p.Trim()).ToArray();
                    int diametroID = 0;
                    int cedulaID = 0;

                    decimal valorDiametro = palabras[0].SafeDecimalParse();
                    DiametroCache diametro = diam.SingleOrDefault(x => x.Valor == valorDiametro);
                    string cedulaStr = palabras[1];
                    CedulaCache cedula = ced.SingleOrDefault(x => x.Nombre.EqualsIgnoreCase(cedulaStr));

                    if (diametro == null && cedula == null && numLinea == 0)
                    {
                    }
                    else
                    {
                        if (diametro == null)
                        {
                            //Internacionalizar y especificar qué diámetro
                            errores.Add(string.Format(MensajesError.DiametroX_NoExiste, numLinea + 1, palabras[0]));
                        }
                        else
                        {
                            diametroID = diametro.ID;
                        }

                        if (cedula == null)
                        {
                            //Internacionalizar y especificar qué cédula
                            errores.Add(string.Format(MensajesError.CedulaX_NoExiste, numLinea + 1, palabras[1]));
                        }
                        else
                        {
                            cedulaID = cedula.ID;
                        }

                        if (!decimal.TryParse(palabras[2], out valorCosto))
                        {
                            errores.Add(string.Format(MensajesError.CostoX_Invalido, numLinea + 1, palabras[2]));
                        }
                        else if (valorCosto > MAXIMO_COSTO || valorCosto < 0)
                        {
                            //Especificar para que línea del archivo
                            errores.Add(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                            throw new ExcepcionProcesos(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                        }


                        CostoProcesoRelleno proRelleno = procesosRelleno.Where(x => x.DiametroID == diametroID &&
                                                                                    x.CedulaID == cedulaID).SingleOrDefault();

                        if (proRelleno != null)
                        {
                            //actualizar
                            proRelleno.StartTracking();
                            proRelleno.UsuarioModifica = usuarioModifica;
                            proRelleno.FechaModificacion = DateTime.Now;
                            proRelleno.Costo = valorCosto;
                            proRelleno.StopTracking();
                            ctx.CostoProcesoRelleno.ApplyChanges(proRelleno);
                        }
                        else
                        {
                            //Se trata de una nueva
                            proRelleno = new CostoProcesoRelleno
                            {
                                CedulaID = cedulaID,
                                TipoJuntaID = tipoJuntaID,
                                FamiliaAceroID = familiaAceroID,
                                DiametroID = diametroID,
                                ProcesoRellenoID = procesoID,
                                Costo = valorCosto,
                                ProyectoID = proyectoID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModifica = usuarioModifica
                            };
                            //Agregar la nueva
                            ctx.CostoProcesoRelleno.ApplyChanges(proRelleno);
                        }

                        
                        if (listaCostos.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID && x.TipoJuntaID == tipoJuntaID && x.FamiliaAceroID == familiaAceroID && x.ProcesoRellenoID == procesoID).Any())
                        {
                            errores.Add(string.Format(MensajesError.DestajoDuplicado, diametro.Nombre, cedula.Nombre));
                        }
                        listaCostos.Add(proRelleno);

                    }
                }
                #endregion
            
        }

        public void IterarArchivoRaiz(Guid usuarioModifica, int familiaAceroID, int tipoJuntaID, int procesoID, int proyectoID, string[] lineas, List<CostoProcesoRaiz> procesosRaiz, SamContext ctx)
        {

            #region Iterar para cada renglon del archivo
            List<CostoProcesoRaiz> listaCostos = new List<CostoProcesoRaiz>();

            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();
                int diametroID = 0;
                int cedulaID = 0;

                decimal valorDiametro = palabras[0].SafeDecimalParse();
                DiametroCache diametro = diam.SingleOrDefault(x => x.Valor == valorDiametro);
                string cedulaStr = palabras[1];
                CedulaCache cedula = ced.SingleOrDefault(x => x.Nombre.EqualsIgnoreCase(cedulaStr));

                if (diametro == null && cedula == null && numLinea == 0)
                {
                }
                else
                {
                    if (diametro == null)
                    {
                        //Internacionalizar y especificar qué diámetro
                        errores.Add(string.Format(MensajesError.DiametroX_NoExiste, numLinea + 1, palabras[0]));
                    }
                    else
                    {
                        diametroID = diametro.ID;
                    }

                    if (cedula == null)
                    {
                        //Internacionalizar y especificar qué cédula
                        errores.Add(string.Format(MensajesError.CedulaX_NoExiste, numLinea + 1, palabras[1]));
                    }
                    else
                    {
                        cedulaID = cedula.ID;
                    }

                    if (!decimal.TryParse(palabras[2], out valorCosto))
                    {
                        errores.Add(string.Format(MensajesError.CostoX_Invalido, numLinea + 1, palabras[2]));
                    }
                    else if (valorCosto > MAXIMO_COSTO || valorCosto < 0)
                    {
                        //Especificar para que línea del archivo
                        errores.Add(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                        throw new ExcepcionProcesos(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                    }

                    CostoProcesoRaiz proRaiz = procesosRaiz.Where(x => x.DiametroID == diametroID &&
                                                                        x.CedulaID == cedulaID).SingleOrDefault();

                    if (proRaiz != null)
                    {
                        //actualizar
                        proRaiz.StartTracking();
                        proRaiz.UsuarioModifica = usuarioModifica;
                        proRaiz.FechaModificacion = DateTime.Now;
                        proRaiz.Costo = valorCosto;
                        proRaiz.StopTracking();
                        ctx.CostoProcesoRaiz.ApplyChanges(proRaiz);
                    }
                    else
                    {
                        //Se trata de una nueva
                        proRaiz = new CostoProcesoRaiz
                        {
                            CedulaID = cedulaID,
                            TipoJuntaID = tipoJuntaID,
                            FamiliaAceroID = familiaAceroID,
                            DiametroID = diametroID,
                            ProcesoRaizID = procesoID,
                            Costo = valorCosto,
                            ProyectoID = proyectoID,
                            FechaModificacion = DateTime.Now,
                            UsuarioModifica = usuarioModifica
                        };
                        //Agregar la nueva
                        ctx.CostoProcesoRaiz.ApplyChanges(proRaiz);
                    }

                    if (listaCostos.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID && x.TipoJuntaID == tipoJuntaID && x.FamiliaAceroID == familiaAceroID && x.ProcesoRaizID == procesoID).Any())
                    {
                        errores.Add(string.Format(MensajesError.DestajoDuplicado, diametro.Nombre, cedula.Nombre));
                    }
                    listaCostos.Add(proRaiz);
                }
            }
            #endregion

        }

        public void IterarArchivoArmado(Guid usuarioModifica, int familiaAceroID, int tipoJuntaID, int proyectoID, string[] lineas, List<CostoArmado> procesosArmado, SamContext ctx)
        {

            #region Iterar para cada renglon del archivo

            List<CostoArmado> listaCostos = new List<CostoArmado>();

            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                string[] palabras = linea.Split(',');
                palabras = palabras.ToList().Select(p => p.Trim()).ToArray();
                int diametroID = 0;
                int cedulaID = 0;

                decimal valorDiametro = palabras[0].SafeDecimalParse();
                DiametroCache diametro = diam.SingleOrDefault(x => x.Valor == valorDiametro);
                string cedulaStr = palabras[1];
                CedulaCache cedula = ced.SingleOrDefault(x => x.Nombre.EqualsIgnoreCase(cedulaStr));

                if (diametro == null && cedula == null && numLinea == 0)
                {
                }
                else
                {
                    if (diametro == null)
                    {
                        //Internacionalizar y especificar qué diámetro
                        errores.Add(string.Format(MensajesError.DiametroX_NoExiste, numLinea + 1, palabras[0]));
                    }
                    else
                    {
                        diametroID = diametro.ID;
                    }

                    if (cedula == null)
                    {
                        //Internacionalizar y especificar qué cédula
                        errores.Add(string.Format(MensajesError.CedulaX_NoExiste, numLinea + 1, palabras[1]));
                    }
                    else
                    {
                        cedulaID = cedula.ID;
                    }

                    if (!decimal.TryParse(palabras[2], out valorCosto))
                    {
                        errores.Add(string.Format(MensajesError.CostoX_Invalido, numLinea + 1, palabras[2]));
                    }
                    else if (valorCosto > MAXIMO_COSTO || valorCosto < 0)
                    {
                        //Especificar para que línea del archivo
                        errores.Add(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                        throw new ExcepcionProcesos(string.Format(MensajesError.CostoX_Rango, valorCosto, numLinea + 1));
                    }


                    CostoArmado proArmado = procesosArmado.Where(x => x.DiametroID == diametroID &&
                                                                        x.CedulaID == cedulaID).SingleOrDefault();

                    if (proArmado != null)
                    {
                        //actualizar
                        proArmado.StartTracking();
                        proArmado.UsuarioModifica = usuarioModifica;
                        proArmado.FechaModificacion = DateTime.Now;
                        proArmado.Costo = valorCosto;
                        proArmado.StopTracking();
                        ctx.CostoArmado.ApplyChanges(proArmado);
                    }
                    else
                    {
                        //Se trata de una nueva
                        proArmado = new CostoArmado
                        {
                            CedulaID = cedulaID,
                            TipoJuntaID = tipoJuntaID,
                            FamiliaAceroID = familiaAceroID,
                            DiametroID = diametroID,
                            Costo = valorCosto,
                            ProyectoID = proyectoID,
                            FechaModificacion = DateTime.Now,
                            UsuarioModifica = usuarioModifica
                        };
                        //Agregar la nueva
                        ctx.CostoArmado.ApplyChanges(proArmado);
                    }

                    if (listaCostos.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID && x.TipoJuntaID == tipoJuntaID && x.FamiliaAceroID == familiaAceroID).Any())
                    {
                        errores.Add(string.Format(MensajesError.DestajoDuplicado, diametro.Nombre, cedula.Nombre));
                    }
                    listaCostos.Add(proArmado);
                }

            #endregion
            }
        }
    }
}