using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Common;
using SAM.Web.Common;

namespace SAM.Web.Administracion
{
    public partial class DetPerfiles : SamPaginaPrincipal
    {
        private List<Permiso> _lstPermisos;
        private Perfil _perfil;

        /// <summary>
        /// Inicializar los controles con el contenido de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Perfiles);
                if (EntityID != null)
                {
                    _perfil = PerfilBO.Instance.ObtenerConPermisos(EntityID.Value);
                    Map(_perfil);
                    VersionRegistro = _perfil.VersionRegistro;
                }

                cargaPermisosPorModulo();
            }
        }

        /// <summary>
        /// Se dispara cada que se hace un binding al repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void repModulos_OnItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            RepeaterItem item = args.Item;

            if (item.IsItem())
            {
                CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkLst");
                Label lblNombre = (Label)item.FindControl("lblNombre");
                
                //El DataItem es una entidad de tipo modulo
                Modulo mod = (Modulo)item.DataItem;

                lblNombre.Text = (Cultura == LanguageHelper.INGLES ? mod.NombreIngles : mod.Nombre);

                if (Cultura == LanguageHelper.INGLES)
                {
                    chkLst.DataSource = _lstPermisos.Where(x => x.ModuloID == mod.ModuloID).OrderBy(x => x.NombreIngles).ToList();
                    chkLst.DataTextField = "NombreIngles";
                }
                else
                {
                    chkLst.DataSource = _lstPermisos.Where(x => x.ModuloID == mod.ModuloID).OrderBy(x => x.Nombre).ToList();
                    chkLst.DataTextField = "Nombre";
                }

                chkLst.DataValueField = "PermisoID";
                chkLst.DataBind();

                //Seleccionar los permisos que ya están relacionados con el perfil
                if (_perfil != null)
                {
                    foreach(ListItem lstItem in chkLst.Items)
                    {
                        int permisoID = lstItem.Value.SafeIntParse();
                        lstItem.Selected = _perfil.PerfilPermiso.Any( x => x.PermisoID == permisoID);
                    }
                }
            }
        }

        /// <summary>
        /// Hace unmapping de los controles básicos y del repeater hacia la entidad perfil.
        /// Luego toma esta entidad y es la que persiste hacia los business objects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            Perfil perfil;

            if (EntityID != null)
            {
                perfil = PerfilBO.Instance.ObtenerConPermisos(EntityID.Value);
                perfil.VersionRegistro = VersionRegistro;
            }
            else
            {
                perfil = new Perfil();
            }

            perfil.StartTracking();
            Unmap(perfil);
            unbindRepeater(perfil);
            perfil.UsuarioModifica = SessionFacade.UserId;
            perfil.FechaModificacion = DateTime.Now;
            perfil.StopTracking();

            try
            {
                PerfilBO.Instance.Guarda(perfil);
                Response.Redirect(WebConstants.AdminUrl.LST_PERFIL);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Itera sobre los controles del repeater para mappear al modelo de objectos correspondientemente.
        /// </summary>
        /// <param name="perfil">objeto perfil sobre el cual se harán los cambios a la colección PerfilPermiso</param>
        private void unbindRepeater(Perfil perfil)
        {
            foreach (RepeaterItem item in repModulos.Items)
            {
                if (item.IsItem())
                {
                    CheckBoxList chkLst = (CheckBoxList)item.FindControl("chkLst");

                    foreach (ListItem chk in chkLst.Items)
                    {
                        int permisoID = chk.Value.SafeIntParse();
                        
                        //buscar si ya está en la BD
                        PerfilPermiso pp = perfil.PerfilPermiso.Where(x => x.PermisoID == permisoID).SingleOrDefault();

                        if (pp != null)
                        {
                            pp.StartTracking();
                            //Si ya está en la BD y no está seleccionado hay que borrarlo
                            if (!chk.Selected)
                            {
                                pp.MarkAsDeleted();
                            }
                            else //Actualizar el usuario
                            {
                                pp.UsuarioModifica = SessionFacade.UserId;
                                pp.FechaModificacion = DateTime.Now;
                            }
                            pp.StopTracking();
                        }
                        else if (chk.Selected) //Si no está en la BD y está seleccionado hay que agregarlo
                        {
                            pp = new PerfilPermiso();
                            pp.FechaModificacion = DateTime.Now;
                            pp.UsuarioModifica = SessionFacade.UserId;
                            pp.PermisoID = permisoID;
                            perfil.PerfilPermiso.Add(pp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deja en una variabla privada todos los permisos del sistema y hace el bind al repeater con únicamente los módulos
        /// </summary>
        private void cargaPermisosPorModulo()
        {
            _lstPermisos = PermisoBO.Instance.ObtenerTodos();
            
            List<Modulo> lstModulo = ModuloBO.Instance.ObtenerTodos();

            //Hacer el primer bind nada más con los módulos
            if ( Cultura == LanguageHelper.INGLES )
            {
                repModulos.DataSource = lstModulo;
            }
            else
            {
                repModulos.DataSource = lstModulo;
            }

            repModulos.DataBind();
        }
    }
}