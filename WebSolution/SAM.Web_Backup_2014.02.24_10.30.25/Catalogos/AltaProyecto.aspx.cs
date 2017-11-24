using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;
using SAM.Entities;
using SAM.Web.Controles.Proyecto;
using SAM.BusinessLogic.Proyectos;

namespace SAM.Web.Catalogos
{
    public partial class AltaProyecto : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctrlInformacion.CargaCombos();
            }
        }

        /// <summary>
        /// metodo que lee la informacion en los controles de información y de contactos.
        /// crea una nueva entidad proyecto a la cuál configura con los datos ingresados en los controles
        /// y crea las relaciones con las tablas de:
        /// ProyectoConfiguracion
        /// Contacto
        /// ProyectoConsecutivo
        /// y ProyectoNomenclaturaSpool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Proyecto proyecto = new Proyecto();
                proyecto.Contacto = new Entities.Contacto();
                proyecto.ProyectoConfiguracion = new ProyectoConfiguracion();

                ctrlContacto.Unmap(proyecto.Contacto);
                ctrlInformacion.Unmap(proyecto);

                proyecto.ProyectoConfiguracion.ToleranciaCortes = 0;
                proyecto.ProyectoConfiguracion.AnguloBisel = "0";

                try
                {
                    ProyectoBL.Instance.AltaProyecto(proyecto, SessionFacade.UserId);

                    if (!SessionFacade.EsAdministradorSistema)
                    {
                        //Actualizar la lista de proyectos a la cual el usuario tiene permisos
                        List<int> pids = SessionFacade.ProyectosConPermiso.ToList();
                        pids.Add(proyecto.ProyectoID);
                        SessionFacade.ProyectosConPermiso = pids.ToArray();
                    }

                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, proyecto.ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}