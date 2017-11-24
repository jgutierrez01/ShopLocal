using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Administracion
{
    public class PendienteDetalleBO
    {
        private static readonly object _mutex = new object();
        private static PendienteDetalleBO _instance;

         /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PendienteDetalleBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PendienteDetalleBO
        /// </summary>
        /// <returns></returns>
        public static PendienteDetalleBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PendienteDetalleBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdPendienteDetalle> ObtenerPorPendienteID(int pendienteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from pendiente in ctx.Pendiente
                        join pendienteDetalle in ctx.PendienteDetalle on pendiente.PendienteID equals pendienteDetalle.PendienteID
                        join area in ctx.CategoriaPendiente on pendienteDetalle.CategoriaPendienteID equals area.CategoriaPendienteID
                        join responsable in ctx.Usuario on pendienteDetalle.Responsable equals responsable.UserId
                        join autor in ctx.Usuario on pendienteDetalle.UsuarioModifica equals autor.UserId
                        where pendienteDetalle.PendienteID == pendienteID
                        select new GrdPendienteDetalle
                        {
                            PendienteDetalleID = pendienteDetalle.PendienteDetalleID,
                            EsAlta = pendienteDetalle.EsAlta,
                            Area = (LanguageHelper.CustomCulture == LanguageHelper.INGLES) ? area.NombreIngles : area.Nombre,
                            Responsable = responsable.Nombre + " " + responsable.ApPaterno + (responsable.ApMaterno != null ? " " + responsable.ApMaterno : string.Empty),
                            Estatus = pendienteDetalle.Estatus,
                            Fecha = pendienteDetalle.FechaModificacion,
                            Autor = autor.Nombre + " " + autor.ApPaterno + (autor.ApMaterno != null ? " " + autor.ApMaterno : string.Empty),
                            Observaciones = pendienteDetalle.Observaciones
                        }).OrderByDescending(x => x.Fecha).ToList();
            }
        }

    }
}
