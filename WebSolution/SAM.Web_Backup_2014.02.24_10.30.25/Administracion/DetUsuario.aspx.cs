using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Proyectos;
using System.Web.Security;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessObjects.Utilerias;
using Resources;

namespace SAM.Web.Administracion
{
    public partial class DetUsuario : SamPaginaPrincipal
    {
        /// <summary>
        /// Variable de instancia con el usuario
        /// </summary>
        private Usuario _usuario;

        /// <summary>
        /// Handler para cuando se carga la página.  Se encarga de presentar la pantalla en modo de alta
        /// para un usuario nuevo y en modo edición para un usuario existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Usuarios);
                cargaCombos();
                if (EntityUID != null)
                {
                    carga();
                    configuraUI();
                }
            }
        }

        /// <summary>
        /// Handler para guardar los datos capturados.  En caso de errores esperados los registra
        /// y muestra en el validation summary.  Si todo sale bien redirecciona al listado de usuarios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario usuario;

            if (EntityUID != null)
            {
                usuario = UsuarioBO.Instance.ObtenerConProyectos(EntityUID.Value);
                usuario.VersionRegistro = VersionRegistro;
            }
            else
            {
                usuario = new Usuario();
            }

            usuario.StartTracking();
            Unmap(usuario);
            unbindCheckboxList(usuario);
            usuario.UsuarioModifica = SessionFacade.UserId;
            usuario.FechaModificacion = DateTime.Now;
            usuario.StopTracking();

            try
            {
                UsuarioBL.Instance.Guarda(usuario);
                Response.Redirect(WebConstants.AdminUrl.LST_USUARIOS);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Toma los elementos seleccionados y deseleccionados y hace lo correspondiente en la colección
        /// de proyectos relacionados.
        /// </summary>
        /// <param name="usuario"></param>
        private void unbindCheckboxList(Usuario usuario)
        {
            foreach (ListItem item in chkProyectos.Items)
            {
                int proyectoID = item.Value.SafeIntParse();

                //buscar si ya está en la BD
                UsuarioProyecto up = usuario.UsuarioProyecto.Where(x => x.ProyectoID == proyectoID).SingleOrDefault();

                if (up != null)
                {
                    up.StartTracking();
                    //Si ya está en la BD y no está seleccionado hay que borrarlo
                    if (!item.Selected)
                    {
                        up.MarkAsDeleted();
                    }
                    else //Actualizar el usuario
                    {
                        up.UsuarioModifica = SessionFacade.UserId;
                        up.FechaModificacion = DateTime.Now;
                    }
                    up.StopTracking();
                }
                else if (item.Selected) //Si no está en la BD y está seleccionado hay que agregarlo
                {
                    up = new UsuarioProyecto();
                    up.FechaModificacion = DateTime.Now;
                    up.UsuarioModifica = SessionFacade.UserId;
                    up.ProyectoID = proyectoID;
                    usuario.UsuarioProyecto.Add(up);
                }
            }
        }

        /// <summary>
        /// Handler para reiniciar la contraseña de un usuario, al finalizar se envía
        /// a una página de éxito con el mensaje de la acción realizada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkReiniciarPassword_OnClick(object sender, EventArgs e)
        {
            UsuarioBL.Instance.ReiniciaPassword(EntityUID.Value);

            UtileriaRedireccion.RedireccionaExitoAdmin(MensajesAplicacion.Usuario_ReseteoPasswordTitulo,
                                                        MensajesAplicacion.Usuario_ReseteoPasswordMensaje,
                                                        new List<LigaMensaje>()
                                                        {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_ListadoDeUsuarios, 
                                                                Url = WebConstants.AdminUrl.LST_USUARIOS
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_RegresarUsuarioAnterior,
                                                                Url = Request.Url.PathAndQuery
                                                            }
                                                        });
        }

        /// <summary>
        /// Handler para desactivar la cuenta de un usuario, al finalizar se envía
        /// a una página de éxito con el mensaje de la acción realizada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDesactivar_OnClick(object sender, EventArgs e)
        {
            UsuarioBO.Instance.DesactivaCuenta(EntityUID.Value, SessionFacade.UserId);

            UtileriaRedireccion.RedireccionaExitoAdmin(MensajesAplicacion.Usuario_CuentaDesactivadaTitulo,
                                                       MensajesAplicacion.Usuario_CuentaDesactivadaMensaje,
                                                       new List<LigaMensaje>()
                                                        {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_ListadoDeUsuarios, 
                                                                Url = WebConstants.AdminUrl.LST_USUARIOS
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_RegresarUsuarioAnterior,
                                                                Url = Request.Url.PathAndQuery
                                                            }
                                                        });
        }

        /// <summary>
        /// Handler para desbloquear la cuenta de un usuario, al finalizar se envía
        /// a una página de éxito con el mensaje de la acción realizada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDesbloquear_OnClick(object sender, EventArgs e)
        {
            UsuarioBO.Instance.Desbloquea(EntityUID.Value, SessionFacade.UserId);

            UtileriaRedireccion.RedireccionaExitoAdmin(MensajesAplicacion.Usuario_CuentaDesbloqueadaTitulo,
                                                       MensajesAplicacion.Usuario_CuentaDesbloqueadaMensaje,
                                                        new List<LigaMensaje>()
                                                        {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_ListadoDeUsuarios, 
                                                                Url = WebConstants.AdminUrl.LST_USUARIOS
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_RegresarUsuarioAnterior,
                                                                Url = Request.Url.PathAndQuery
                                                            }
                                                        });
        }

        /// <summary>
        /// Handler para reactivar una cuenta que se desctivo a través del panel
        /// de administración, al finalizar se envía
        /// a una página de éxito con el mensaje de la acción realizada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkReactivar_OnClick(object sender, EventArgs e)
        {
            UsuarioBO.Instance.Reactiva(EntityUID.Value, SessionFacade.UserId);

            UtileriaRedireccion.RedireccionaExitoAdmin(MensajesAplicacion.Usuario_CuentaReactivadaTitulo,
                                                       MensajesAplicacion.Usuario_CuentaReactivadaMensaje,
                                                       new List<LigaMensaje>()
                                                       {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_ListadoDeUsuarios, 
                                                                Url = WebConstants.AdminUrl.LST_USUARIOS
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_RegresarUsuarioAnterior,
                                                                Url = Request.Url.PathAndQuery
                                                            }
                                                        });
        }

        /// <summary>
        /// Handler para reenviar el correo de activación, al finalizar se envía
        /// a una página de éxito con el mensaje de la acción realizada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkReenviarCorreo_OnClick(object sender, EventArgs e)
        {
            UsuarioBL.Instance.EnviaCorreoActivacion(EntityUID.Value);

            UtileriaRedireccion.RedireccionaExitoAdmin(MensajesAplicacion.Usuario_CorreoActivacionEnviadoTitulo,
                                                        MensajesAplicacion.Usuario_CorreoActivacionEnviadoMensaje,
                                                        new List<LigaMensaje>()
                                                        {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_ListadoDeUsuarios, 
                                                                Url = WebConstants.AdminUrl.LST_USUARIOS
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesAplicacion.Usuario_RegresarUsuarioAnterior,
                                                                Url = Request.Url.PathAndQuery
                                                            }
                                                        });
        }

        /// <summary>
        /// Muestra/oculta las opciones disponibles para administrar la cuenta que se está viendo.
        /// </summary>
        private void configuraUI()
        {
            phAcciones.Visible = true;

            //ocultar todos por default
            lnkReiniciarPassword.Visible = false;
            lnkDesactivar.Visible = false;
            lnkDesbloquear.Visible = false;
            lnkReactivar.Visible = false;
            lnkReenviarCorreo.Visible = false;

            if (_usuario.BloqueadoPorAdministrador)
            {
                lnkReactivar.Visible = true;
            }
            else if (_usuario.IsLockedOut)
            {
                lnkReiniciarPassword.Visible = false;
                lnkDesactivar.Visible = true;
                lnkDesbloquear.Visible = true;
            }
            else if (!_usuario.IsApproved)
            {
                lnkReenviarCorreo.Visible = true;
            }
            else
            {
                lnkDesactivar.Visible = true;
                lnkReiniciarPassword.Visible = true;
            }
        }

        /// <summary>
        /// Carga el combo de perfiles y la lista de proyectos disponibles
        /// </summary>
        private void cargaCombos()
        {
            ddlPerfil.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerPerfiles().OrderBy(x => x.Nombre));

            chkProyectos.DataSource = CacheCatalogos.Instance.ObtenerProyectos().OrderBy(x => x.Nombre);
            chkProyectos.DataTextField = "Nombre";
            chkProyectos.DataValueField = "ID";
            chkProyectos.DataBind();
        }

        /// <summary>
        /// Carga los datos de un usuario y su listado de proyectos
        /// </summary>
        private void carga()
        {
            _usuario = UsuarioBO.Instance.Obtener(EntityUID.Value);
            Map(_usuario);
            VersionRegistro = _usuario.VersionRegistro;

            List<Proyecto> proyectos = ProyectoBO.Instance.ObtenerPorUsuario(_usuario.UserId);

            //Seleccionar los checks de los proyectos para los que el usuario ya tiene permisos
            for (int i = chkProyectos.Items.Count - 1; i >= 0; i--)
            {
                int proyectoID = chkProyectos.Items[i].Value.SafeIntParse();

                if (proyectos.Any(x => x.ProyectoID == proyectoID))
                {
                    chkProyectos.Items[i].Selected = true;
                }
            }
        }
    }
}