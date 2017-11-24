using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Common;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesOdt
    {

        /// <summary>
        /// En base a un arreglo de ids de spools valida que ninguno de esos spools
        /// se encuentre en ODT.
        /// 
        /// Regresa true cuando los spools están "libres", es decir, cuando no tienen ODT
        /// </summary>
        /// <param name="ctx">Contexto de la BD</param>
        /// <param name="spoolIds">Ids e los spools que se desea revisar contra la BD para ver si tienen ODT o no</param>
        /// <returns>true cuando los spools están libres</returns>
        public static bool ValidaSpoolsSinOdt(SamContext ctx, int[] spoolIds)
        {
            if (spoolIds == null || spoolIds.Length == 0)
            {
                return true;
            }

            return ctx.OrdenTrabajoSpool
                      .Where(Expressions.BuildOrExpression<OrdenTrabajoSpool, int>(x => x.SpoolID, spoolIds))
                      .Count() == 0;
        }

        /// <summary>
        /// Valida que el nuevo número de ODT que se desea generar no exista en la BD.
        /// </summary>
        /// <param name="ctx">Contexto de la BD</param>
        /// <param name="proyectoID">ID del proyecto para el cual se desea validar la ODT</param>
        /// <param name="numeroOdt">Número de la ODT que se desea crear</param>
        /// <param name="numOrden">Regresa el número válido para guardar en la BD</param>
        /// <returns>true cuando se puede generar esa ODT</returns>
        public static bool ValidaNumeroDeOdtDisponible(SamContext ctx, int proyectoID, int numeroOdt, out string numOrden, out ProyectoConfiguracion pc)
        {
            pc = ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == proyectoID).Single();
            
            string numAGenerar =
            string.Format("{0}{1}", pc.PrefijoOrdenTrabajo,
                                    numeroOdt.ToString().PadLeft(pc.DigitosOrdenTrabajo, '0'));

            numOrden = numAGenerar;

            return !ctx.OrdenTrabajo.Any(x => x.ProyectoID == proyectoID && x.NumeroOrden == numAGenerar);
        }

        /// <summary>
        /// Regresa true en caso que la ODT no tenga despachos.
        /// Si el despacho está cancelado se cuenta como si no tuviera despacho.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo a validar</param>
        /// <returns>true en caso que la orden de trabajo NO tenga despachos</returns>
        public static bool ValidaOdtNoTengaDespachos(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.Despacho.Any(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID && !x.Cancelado);
        }

        /// <summary>
        /// Regresa true en caso que la ODT no tenga cortes.
        /// Si el corte está cancelado se cuenta como si no tuviera corte.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo a valida</param>
        /// <returns>true en caso que la orden de trabajo NO tenga cortes</returns>
        public static bool ValidaOdtNoTengaCortes(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.CorteDetalle.Any(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID && !x.Cancelado);
        }

        /// <summary>
        /// Regresa true en caso que la ODT no tenga reportes de Armado o Soldado.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool ValidaOdtNoTengaArmadoSoldado(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.JuntaWorkstatus.Any(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID && (x.ArmadoAprobado || x.SoldaduraAprobada));
        }

        /// <summary>
        /// Regresa true en caso que no exista ninguna requisicion de pruebas en la orden de trabajo.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool ValidaOdtNoTengaRequisiciones(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.JuntaRequisicion.Any(x => x.JuntaWorkstatus.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID);
        }

        /// <summary>
        /// Regresa true en caso que no exista requisicion de pintura para el spool
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool ValidaOdtNoTengaRequisicionesPintura(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.RequisicionPinturaDetalle.Any(x => x.WorkstatusSpool.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID);
        }

        /// <summary>
        /// Regresa true en caso de que no exista embarque para el spool
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool ValidaOdtNoTengaEmbarque(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.EmbarqueSpool.Any(x => x.WorkstatusSpool.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID);
        }

        /// <summary>
        /// Regresa true si no hay un registro que ya tenga liberacion dimensional asociada.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool ValidaOdtNoTengaLiberacionDimensional(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.WorkstatusSpool.Any(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID && x.TieneLiberacionDimensional);
        }

        /// <summary>
        /// Regresa true en caso que un registro de la tabla OrdenTrabajoSpool no tenga corte.
        /// Si el corte está cancelado cuenta como si no tuviera corte.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoSpoolID">ID del registro de la tabla OrdenTrabajoSpool que se va a verificar</param>
        /// <returns>true an caso que la OrdenTrabajoSpool NO tenga cortes</returns>
        public static bool ValidaOdtSpoolNoTengaCortes(SamContext ctx, int ordenTrabajoSpoolID)
        {
            return !ctx.CorteDetalle.Any(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID && !x.Cancelado);
        }

        /// <summary>
        /// Regresa true en caso que la ODTSpool no tenga despachos.
        /// Si el despacho está cancelado se cuenta como si no tuviera despacho.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoID">ID del registro de la tabla OrdenTrabajoSpool a validar</param>
        /// <returns>true en caso que el registro de OrdenTrabajoSpool NO tenga despachos</returns>        
        public static bool ValidaOdtSpoolNoTengaDespachos(SamContext ctx, int ordenTrabajoSpoolID)
        {
            return !ctx.Despacho.Any(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID && !x.Cancelado);
        }

        /// <summary>
        /// Regresa true en caso de que la ODTSpool no tenga requisiciones de pintura
        /// Si la ODTSpool no tiene workstatus se cuenta como si no tuviera requisicion de pintura
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoSpoolID">ID del registro de la tabla OrdenTrabajoSpool a validar</param>
        /// <returns>true en caso que el registro de OrdenTrabajoSpool NO tenga requisiciones de pintura</returns>
        public static bool ValidaOdtSpoolNoTengaRequisicionesPintura(SamContext ctx, int ordenTrabajoSpoolID)
        {
            int? workstatusSpoolID = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).Select(y => y.WorkstatusSpoolID).SingleOrDefault();

            if (workstatusSpoolID.HasValue)
            {
                return !ctx.RequisicionPinturaDetalle.Any(x => x.WorkstatusSpoolID == workstatusSpoolID);
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Regresa true en caso de que la ODTSpool no tenga embarque
        /// Si la ODTSpool no tiene workstatus se cuenta como si no tuviera embarque 
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoSpoolID">ID del registro de la tabla OrdenTrabajoSpool a validar</param>
        /// <returns>true en caso que el registro de OrdenTrabajoSpool NO tenga embarque</returns>
        public static bool ValidaOdtSpoolNoTengaEmbarque(SamContext ctx, int ordenTrabajoSpoolID)
        {
            int? workstatusSpoolID = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).Select(y => y.WorkstatusSpoolID).SingleOrDefault();

            if (workstatusSpoolID.HasValue)
            {
                return !ctx.EmbarqueSpool.Any(x => x.WorkstatusSpoolID == workstatusSpoolID);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Valida que la partida (número de control) se encuentre disponible par auna orden
        /// de trabajo en particular.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo para la cual se va a validar</param>
        /// <param name="partida">Partida a validar que no exista</param>
        /// <returns>Regresa true si la partida está "libre/disponible", false de lo contrario</returns>
        public static bool ValidaPartidaDisponible(SamContext ctx, int ordenTrabajoID, int partida)
        {
            return !ctx.OrdenTrabajoSpool.Any(x => x.Partida == partida && x.OrdenTrabajoID == ordenTrabajoID);
        }

        /// <summary>
        /// Regresa true si la orden de trabajo no ha sido utilizada para transferir materiales a corte.
        /// </summary>
        /// <param name="ctx">Contexto actual</param>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo a validar</param>
        /// <returns>Regresa true si NO se han transferido números únicos a corte para la ODT seleccionada</returns>
        public static bool ValidaOdtNoTengaNumerosUnicosEnCorte(SamContext ctx, int ordenTrabajoID)
        {
            return !ctx.NumeroUnicoCorte.Any(x => x.OrdenTrabajoID == ordenTrabajoID && x.NumeroUnicoMovimiento.Estatus == EstatusNumeroUnicoMovimiento.ACTIVO);
        }
    }
}
