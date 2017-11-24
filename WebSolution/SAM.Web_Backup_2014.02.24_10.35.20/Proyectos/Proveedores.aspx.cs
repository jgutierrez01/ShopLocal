using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Proyectos
{
    public partial class Proveedores : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar proveedores de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Proveedores, EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }

        private void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);

            chkProveedores.DataSource = CacheCatalogos.Instance.ObtenerProveedores().OrderBy(x => x.Nombre);
            chkProveedores.DataTextField = "Nombre";
            chkProveedores.DataValueField = "ID";
            chkProveedores.DataBind();

            List<Proveedor> proveedor = ProveedorBO.Instance.ObtenerPorProyecto(proyectoID);
            int proveedorID;
            int i;

            for (i = chkProveedores.Items.Count - 1; i >= 0; i--)
            {
                proveedorID = chkProveedores.Items[i].Value.SafeIntParse();

                if (proveedor.Any(x => x.ProveedorID == proveedorID))
                {
                    chkProveedores.Items[i].Selected = true;
                }
            }
        }

        /// <summary>
        /// crea una entidad proyecto y manda llamar el método unbindcheckboxlist
        /// el cual guardará los cambios en la entidad proyecto.
        /// Esta entidad se guarda en la base de datos en la llamada a Guarda();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConProveedores(EntityID.Value);
                proyecto.StartTracking();
                unbindCheckBoxList(proyecto);
                proyecto.StopTracking();

                try
                {
                    ProyectoBO.Instance.Guarda(proyecto);
                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, proyecto.ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }


        /// <summary>
        /// recibe un indice de proyecto con el cuál valida si existe un proveedor de proyecto para el item en 
        /// el que se encuentra.
        /// si ya existe y sigue seleccionado, lo guarda
        /// si ya existe y no esta seleccionado, lo borra
        /// si no existe y esta seleccionado, lo agruega
        /// si no existe y no está seleccionado, no hace nada.
        /// </summary>
        /// <param name="proyectoID"></param>
        private void unbindCheckBoxList(Proyecto proyecto)
        {
            int proveedorID;
            ProveedorProyecto proveedor;
            foreach (ListItem item in chkProveedores.Items)
            {
                proveedorID = item.Value.SafeIntParse();

                proveedor = proyecto.ProveedorProyecto.Where(x => x.ProveedorID == proveedorID).SingleOrDefault();

                //si ya existe en la base de datos;
                if (proveedor != null)
                {
                    proveedor.StartTracking();

                    // si ya no está seleccionado, se borra.
                    if (!item.Selected)
                    {
                        proveedor.MarkAsDeleted();
                    }
                    else //si sigue seleccionado, solo se actualiza.
                    {
                        proveedor.UsuarioModifica = SessionFacade.UserId;
                        proveedor.FechaModificacion = DateTime.Now;
                    }

                    proveedor.StopTracking();
                }
                    // si no está en BD pero si está seleccionado, se debe agregar.
                else if (item.Selected)
                {
                    proveedor = new ProveedorProyecto();
                    proveedor.ProveedorID = proveedorID;
                    proveedor.ProyectoID = proyecto.ProyectoID;
                    proveedor.FechaModificacion = DateTime.Now;
                    proveedor.UsuarioModifica = SessionFacade.UserId;

                    proyecto.ProveedorProyecto.Add(proveedor);
                }
            }
        }


    }
}