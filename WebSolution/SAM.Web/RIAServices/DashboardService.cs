
using SAM.Web.Common;

namespace SAM.Web.RIAServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using SAM.BusinessObjects.Modelo;
    using SAM.Entities.Cache;
    using SAM.Web.Classes;
    using SAM.Entities;
    using SAM.BusinessObjects.Catalogos;
    using SAM.BusinessObjects.Proyectos;
    using SAM.Entities.Grid;
    using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Administracion;
using System.Globalization;


    // Implements application logic using the SamContext context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class DashboardService : LinqToEntitiesDomainService<SamContext>
    {

        private Guid _usuarioID;

        

        public DashboardService(Guid usuarioID)
        {
            _usuarioID = usuarioID;
        }

        public List<Patio> ObtenPatio()
        {   
            return  PatioBO.Instance.ObtenerTodos().Where(x => UserScope.MisPatios.Select(y => y.ID).Contains(x.PatioID)).ToList();
        }

        public List<Proyecto> ObtenProyectosPorPatio(int patioID)
        {
            return ProyectoBO.Instance.ObtenerTodos().Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID) && x.PatioID == patioID).ToList();
        }

        public Usuario ObtenUsuario()
        {
            return this.ObjectContext.Usuario.Where(x => x.UserId == SessionFacade.UserId).Single();
        }

        public List<Proyecto> ObtenProyectos()
        {
            return ProyectoBO.Instance.ObtenerTodos().Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID)).ToList();
        }

        [Update]
        public void GuardaUsuario(Usuario usu)
        {
            this.ObjectContext.Usuario.AttachAsModified(usu, this.ChangeSet.GetOriginal(usu));
        }
 
    }
}


