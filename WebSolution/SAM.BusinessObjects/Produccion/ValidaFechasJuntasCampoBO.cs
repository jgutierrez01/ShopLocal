using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Extensions;
using SAM.Entities;

namespace SAM.BusinessObjects.Produccion
{
    public class ValidaFechasJuntasCampoBO
    {
        private static readonly object _mutex = new object();
        private static ValidaFechasJuntasCampoBO _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ValidaFechasJuntasCampoBO()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ValidaFechasJuntasCampoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidaFechasJuntasCampoBO();
                    }
                }
                return _instance;
            }
        }

        public string ObtenerFechasArmadoJuntasCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampo juntaCampo = ctx.JuntaCampo.Single(x => x.JuntaCampoID == juntaCampoID);

                if (juntaCampo.ArmadoAprobado == true && juntaCampo.JuntaCampoArmadoID != null)
                {
                    JuntaCampoArmado jca = ctx.JuntaCampoArmado.Single(x => x.JuntaCampoArmadoID == juntaCampo.JuntaCampoArmadoID);

                    return jca.FechaArmado.ToString("MM/dd/yyyy") + "," + jca.FechaReporte.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DateTime ObtenerFechaArmado(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoArmado juntaCampoArmado = ctx.JuntaCampoArmado.Single(x => x.JuntaCampoID == juntaCampoID);

                return juntaCampoArmado.FechaArmado;
            }
        }

        public DateTime ObtenerFechaReporteArmado(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoArmado juntaCampoArmado = ctx.JuntaCampoArmado.Single(x => x.JuntaCampoID == juntaCampoID);

                return juntaCampoArmado.FechaReporte;
            }
        }

        public string ObtenerFechasSoldaduraJuntasCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampo juntaCampo = ctx.JuntaCampo.Single(x => x.JuntaCampoID == juntaCampoID);

                if (juntaCampo.SoldaduraAprobada == true && juntaCampo.JuntaCampoSoldaduraID != null)
                {
                    JuntaCampoSoldadura jcs = ctx.JuntaCampoSoldadura.Single(x => x.JuntaCampoSoldaduraID == juntaCampo.JuntaCampoSoldaduraID);

                    return jcs.FechaSoldadura.ToString("MM/dd/yyyy") + "," + jcs.FechaReporte.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DateTime ObtenerFechaJuntaCampoSoldadura(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoSoldadura juntaCampoSoldadura = ctx.JuntaCampoSoldadura.Single(x => x.JuntaCampoID == juntaCampoID);

                return juntaCampoSoldadura.FechaSoldadura;
            }
        }

        public DateTime ObtenerFechaJuntaCampoSoldaduraReporte(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoSoldadura juntaCampoSoldadura = ctx.JuntaCampoSoldadura.Single(x => x.JuntaCampoID == juntaCampoID);

                return juntaCampoSoldadura.FechaReporte;
            }
        }

        public string ObtenerFechasIVJuntasCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampo juntaCampo = ctx.JuntaCampo.Single(x => x.JuntaCampoID == juntaCampoID);

                if (juntaCampo.InspeccionVisualAprobada.Value && juntaCampo.JuntaCampoInspeccionVisualID != null)
                {
                    JuntaCampoInspeccionVisual jciv = ctx.JuntaCampoInspeccionVisual.Single(x => x.JuntaCampoInspeccionVisualID == juntaCampo.JuntaCampoInspeccionVisualID);

                    InspeccionVisualCampo ivc = ctx.InspeccionVisualCampo.Single(x => x.InspeccionVisualCampoID == jciv.InspeccionVisualCampoID);

                    return jciv.FechaInspeccion.Value.ToString("MM/dd/yyyy") + "," + ivc.FechaReporte.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DateTime ObtenerFechaIVJuntaCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoInspeccionVisual juntaCampoIV = ctx.JuntaCampoInspeccionVisual.Single(x => x.JuntaCampoID == juntaCampoID);

                return juntaCampoIV.FechaInspeccion.Value;
            }
        }

        public DateTime ObtenerFechaReporteIVJuntaCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampoInspeccionVisual juntaCampoIV = ctx.JuntaCampoInspeccionVisual.Single(x => x.JuntaCampoID == juntaCampoID);

                return ctx.InspeccionVisualCampo.Single(x => x.InspeccionVisualCampoID == juntaCampoIV.InspeccionVisualCampoID).FechaReporte;
            }
        }

        public DateTime ObtenerFechaRequisicionJuntaCampo(int juntaCampoID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<JuntaCampoRequisicion> juntaCampoReq = ctx.JuntaCampoRequisicion.Where(x => x.JuntaCampoID == juntaCampoID);

                DateTime fechaRequisicion = ctx.RequisicionCampo.Where(x => juntaCampoReq.Select(y => y.RequisicionCampoID).Contains(x.RequisicionCampoID)).Select(x => x.FechaRequisicion).First();

                return fechaRequisicion;
            }
        }
    }
}
