using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Materiales
{
    public class PinturaNumeroUnicoBO
    {
        private static readonly object _mutex = new object();
        private static PinturaNumeroUnicoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PinturaNumeroUnicoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PinturaNumeroUnicoBO
        /// </summary>
        /// <returns></returns>
        public static PinturaNumeroUnicoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PinturaNumeroUnicoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene la información de un PinturaNumeroUnico
        /// </summary>
        /// <param name="pinturaNumUnicoID">PinturaNumeroUnicoID</param>
        /// <returns></returns>
        public PinturaNumeroUnico Obtener(int pinturaNumUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.PinturaNumeroUnico.Where(x => x.PinturaNumeroUnicoID == pinturaNumUnicoID).SingleOrDefault();
            }
        }

        public void PintarNumerosUnicos()
        {
            throw new NotImplementedException();
        }

        public PinturaNumeroUnico ObtenerPorNumeroUnico(int idNumUnico)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.PinturaNumeroUnico.Where(x => x.NumeroUnicoID == idNumUnico).SingleOrDefault();
            }
        }

        /// <summary>
        /// Guarda las todos los numeros unicos que se enviarán a pintura
        /// </summary>
        /// <param name="pinturaNumUnico">Entidad PinturaNumeroUnico</param>
        public void GuardaRequisicionPintura(int[] nuIds, int proyectoID ,string numReportePrimarios, DateTime? fechaPrimarios,string numReporteIntermedio, DateTime? fechaIntermedio, bool liberado, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                PinturaNumeroUnico pnu;

                IQueryable<int> iqNus = nuIds.AsQueryable();

                Dictionary<int,int> dicReqs =
                    ctx.RequisicionNumeroUnicoDetalle
                       .Where(x => iqNus.Contains(x.NumeroUnicoID))
                       .Select(x => new { NumeroUnicoID = x.NumeroUnicoID, RequisicionNumeroUnicoDetalleID = x.RequisicionNumeroUnicoDetalleID})
                       .ToDictionary(x => x.NumeroUnicoID, y => y.RequisicionNumeroUnicoDetalleID);


                foreach (int numUnicoID in nuIds)
                {
                    pnu = ctx.PinturaNumeroUnico.Where(x => x.ProyectoID == proyectoID && x.NumeroUnicoID == numUnicoID).SingleOrDefault();

                    if (pnu != null)
                    {
                        pnu.StartTracking();
                        
                        if (numReportePrimarios != string.Empty)
                        {
                            pnu.ReportePrimarios = numReportePrimarios;
                        }

                        if (fechaPrimarios.HasValue)
                        {
                            pnu.FechaPrimarios = fechaPrimarios;
                        }

                        if (numReporteIntermedio != string.Empty)
                        {
                            pnu.ReporteIntermedio = numReporteIntermedio;
                        }

                        if (fechaIntermedio.HasValue)
                        {
                            pnu.FechaIntermedio = fechaIntermedio;
                        }

                        pnu.Liberado = liberado;
                        pnu.FechaModificacion = DateTime.Now;
                        pnu.UsuarioModifica = userID;
                        pnu.StopTracking();
                    }
                    else
                    {
                        pnu = new PinturaNumeroUnico();
                        pnu.ProyectoID = proyectoID;
                        pnu.NumeroUnicoID = numUnicoID;
                        pnu.RequisicionNumeroUnicoDetalleID = dicReqs[numUnicoID];
                        pnu.ReportePrimarios = numReportePrimarios;
                        pnu.FechaPrimarios = fechaPrimarios;
                        pnu.ReporteIntermedio = numReporteIntermedio;
                        pnu.FechaIntermedio = fechaIntermedio;
                        pnu.Liberado = liberado;
                        pnu.FechaModificacion = DateTime.Now;
                        pnu.UsuarioModifica = userID;

                        ctx.PinturaNumeroUnico.AddObject(pnu);
                    }
                }

                ctx.SaveChanges();
            }
        }
    }
}
