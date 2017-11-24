using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesProyecto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoNombre"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool ProyectoDuplicado(SamContext ctx, string proyectoNombre, int proyectoID)
        {
            return ctx.Proyecto.Any(x => x.Nombre == proyectoNombre && x.ProyectoID != proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneSpools(SamContext ctx, int proyectoID)
        {
            return ctx.Spool.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneNumerosUnicos(SamContext ctx, int proyectoID)
        {
            return ctx.NumeroUnico.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneItemCodes(SamContext ctx, int proyectoID)
        {
            return ctx.ItemCode.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneRecepciones(SamContext ctx, int proyectoID)
        {
            return ctx.Recepcion.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneProveedores(SamContext ctx, int proyectoID)
        {
            return ctx.ProveedorProyecto.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneTransportistas(SamContext ctx, int proyectoID)
        {
            return ctx.TransportistaProyecto.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneWps(SamContext ctx, int proyectoID)
        {
            return ctx.WpsProyecto.Any(x => x.ProyectoID == proyectoID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public static bool TieneFabricantes(SamContext ctx, int proyectoID)
        {
            return ctx.FabricanteProyecto.Any(x => x.ProyectoID == proyectoID);
        }
    }
}
