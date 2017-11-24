using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;

namespace SAM.Web.Classes
{
    public class SamPaginaInspector : SamPaginaPrincipal
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="urlExito"></param>
        /// <param name="obtener"></param>
        /// <param name="guardar"></param>
        public void GuardaEntidadPorId<TEntity>(string urlExito, Func<int, TEntity> obtener, Action<TEntity> guardar) where TEntity : IObjectWithChangeTracker
        {
            TEntity entidad;

            if (EntityID != null)
            {
                entidad = obtener(EntityID.Value);
                entidad.VersionRegistro = VersionRegistro;
            }
            else
            {
                entidad = Activator.CreateInstance<TEntity>();
            }

            entidad.StartTracking();
            Unmap(entidad);
            entidad.UsuarioModifica = SessionFacade.UserId;
            entidad.FechaModificacion = DateTime.Now;
            entidad.StopTracking();

            try
            {
                guardar(entidad);
                Response.Redirect(urlExito);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="urlExito"></param>
        /// <param name="obtener"></param>
        /// <param name="guardar"></param>
        public void GuardaEntidadPorUId<TEntity>(string urlExito, Func<Guid, TEntity> obtener, Action<TEntity> guardar) where TEntity : IObjectWithChangeTracker
        {
            TEntity entidad;

            if (EntityUID != null)
            {
                entidad = obtener(EntityUID.Value);
                entidad.VersionRegistro = VersionRegistro;
            }
            else
            {
                entidad = Activator.CreateInstance<TEntity>();
            }

            entidad.StartTracking();
            Unmap(entidad);
            entidad.UsuarioModifica = SessionFacade.UserId;
            entidad.FechaModificacion = DateTime.Now;
            entidad.StopTracking();

            try
            {
                guardar(entidad);
                Response.Redirect(urlExito);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

    }
}