using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Web.Classes
{
    public static class SeguridadQs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAProyecto(int proyectoID)
        {
            return UserScope.MisProyectos.Any(x => x.ID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCodeID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAItemCode(int itemCodeID)
        {
            if ( SessionFacade.EsAdministradorSistema )
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ItemCodeBO.Instance.ObtenerProyectoID(itemCodeID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemCodeEquivalenteID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAItemCodeEquivalente(int itemCodeEquivalenteID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ItemCodeEquivalentesBO.Instance.ObtenerProyectoID(itemCodeEquivalenteID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coladaID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAColada(int coladaID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ColadasBO.Instance.ObtenerProyectoID(coladaID));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAOrdenDeTrabajo(int ordenTrabajoID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(OrdenTrabajoBO.Instance.ObtenerProyectoID(ordenTrabajoID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimacionID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAEstimacion(int estimacionID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(EstimacionBO.Instance.ObtenerProyectoID(estimacionID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAOrdenDeTrabajoSpool(int ordenTrabajoSpoolID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(OrdenTrabajoSpoolBO.Instance.ObtenerProyectoID(ordenTrabajoSpoolID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool TieneAccesoASpool(int spoolID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(SpoolBO.Instance.ObtenerProyectoID(spoolID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <returns></returns>
        public static bool TieneAccesoANumeroUnico(int numeroUnicoID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(NumeroUnicoBO.Instance.ObtenerProyectoID(numeroUnicoID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public static bool TieneAccesoARecepcion(int recepcionID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(RecepcionBO.Instance.ObtenerProyectoID(recepcionID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionNumeroUnicoID"></param>
        /// <returns></returns>
        public static bool TieneAccesoARequisicionNumeroUnico(int requisicionNumeroUnicoID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(RequisicionNumeroUnicoBO.Instance.ObtenerProyectoID(requisicionNumeroUnicoID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaworkstatusID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAJuntaWorkstatus(int juntaworkstatusID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(JuntaWorkstatusBO.Instance.ObtenerProyectoID(juntaworkstatusID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporteDimensionalID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAReporteDimensional(int reporteDimensionalID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ReporteDimensionalBO.Instance.ObtenerProyectoID(reporteDimensionalID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspeccionVisualID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAInspeccionVisual(int inspeccionVisualID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(InspeccionVisualBO.Instance.ObtenerProyectoID(inspeccionVisualID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportePndID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAReportePnd(int reportePndID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ReportePndBO.Instance.ObtenerProyectoID(reportePndID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportePndID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAReporteSpoolPnd(int reporteSpoolPndID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ReportePndBO.Instance.ObtenerProyectoIDPorReporteSpool(reporteSpoolPndID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporteTtID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAReporteTT(int reporteTtID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(ReporteTtBO.Instance.ObtenerProyectoID(reporteTtID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionID"></param>
        /// <returns></returns>
        public static bool TieneAccesoARequisicion(int requisicionID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(RequisicionBO.Instance.ObtenerProyectoID(requisicionID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionID"></param>
        /// <returns></returns>
        public static bool TieneAccesoARequisicionSpool(int requisicionSpoolID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(RequisicionBO.Instance.ObtenerProyectoIDPorRequisicionSpool(requisicionSpoolID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionPinturaID"></param>
        /// <returns></returns>
        public static bool TieneAccesoARequisicionPintura(int requisicionPinturaID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(RequisicionPinturaBO.Instance.ObtenerProyectoID(requisicionPinturaID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAJuntaSpool(int juntaSpoolID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(JuntaSpoolBO.Instance.ObtenerProyectoID(juntaSpoolID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoMaterialID"></param>
        /// <returns></returns>
        public static bool TieneAccesoAOrdenTrabajoMaterial(int ordenTrabajoMaterialID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(OrdenTrabajoMaterialBO.Instance.ObtenerProyectoID(ordenTrabajoMaterialID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="corteID"></param>
        /// <returns></returns>
        public static bool TieneAccesoACorte(int corteID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(CorteBO.Instance.ObtenerProyectoID(corteID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="despachoID"></param>
        /// <returns></returns>
        public static bool TieneAccesoADespacho(int despachoID)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                return TieneAccesoAProyecto(DespachoBO.Instance.ObtenerProyectoID(despachoID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workstatusSpoolIds"></param>
        /// <returns></returns>
        public static bool TieneAccesoATodosWorkstatusSpools(int[] workstatusSpoolIds)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                int[] pids = WorkstatusSpoolBO.Instance.ObtenerProyectos(workstatusSpoolIds);
                return pids.All(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolIds"></param>
        /// <returns></returns>
        public static bool TieneAccesoATodosLosSpools(int[] spoolIds)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                int[] pids = SpoolBO.Instance.ObtenerProyectos(spoolIds);
                return pids.All(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaWorkstatusIds"></param>
        /// <returns></returns>
        public static bool TieneAccesoATodosLasJuntasWorkstatus(int[] juntaWorkstatusIds)
        {
            if (SessionFacade.EsAdministradorSistema)
            {
                return true;
            }
            else
            {
                int[] pids = JuntaWorkstatusBO.Instance.ObtenerProyectos(juntaWorkstatusIds);
                return pids.All(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x));
            }
        }
    }
}