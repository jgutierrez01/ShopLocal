using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Workstatus
{
    public class OkPNDBO
    {
        private static readonly object _mutex = new object();
        private static OkPNDBO _instance;

        public static OkPNDBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OkPNDBO();
                    }
                }
                return _instance;
            }
        }

        public void GuardarOkPnd(DateTime fechaOk, int ordenTrabajoId, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoId).SingleOrDefault();
                wks.StartTracking();
                wks.FechaOkPnd = fechaOk;
                wks.UsuarioOkPnd = userId;
                wks.FechaModificacion = DateTime.Now;
                wks.UsuarioModifica = userId;
                wks.StopTracking();
                ctx.WorkstatusSpool.ApplyChanges(wks);
                ctx.SaveChanges();
            }
        }


        public void BorrarOkPnd(DateTime fechaOk, int ordenTrabajoId, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoId).SingleOrDefault();
                wks.StartTracking();
                wks.FechaOkPnd = null;
                wks.UsuarioOkPnd = null;
                wks.FechaModificacion = DateTime.Now;
                wks.UsuarioModifica = userId;
                wks.StopTracking();
                ctx.WorkstatusSpool.ApplyChanges(wks);
                ctx.SaveChanges();
            }
        }

        public void GuardarOkPnds(DateTime fechaOk, int [] ordenTrabajoIds, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                foreach(int id in ordenTrabajoIds)
                {
                    GuardarOkPnd(fechaOk, id, userId);
           
                }
            }
        }

        public bool TienePermisoBorrarOkPND(int PerfilID, string cultura, string nombrepermiso, bool esAdministradorSistema)
        {
            using (SamContext ctx = new SamContext())
            {
                Permiso permiso;
                if (!esAdministradorSistema)
                {
                    if (cultura != LanguageHelper.INGLES)
                        permiso = ctx.Permiso.Where(x => x.Nombre == nombrepermiso).SingleOrDefault();
                    else
                        permiso = ctx.Permiso.Where(x => x.NombreIngles == nombrepermiso).SingleOrDefault();

                    if (permiso != null)
                        return ctx.PerfilPermiso.Where(x => x.PerfilID == PerfilID && x.PermisoID == permiso.PermisoID).Any();
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }


        }
        
    }

    
}
