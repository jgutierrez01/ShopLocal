using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Web.Security;
using Mimo.Framework.Common;
using System.Transactions;
using SAM.Common.Membership;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using SAM.Common;
using SAM.Entities.RadCombo;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Administracion
{
    public class UsuarioBO
    {
        private static readonly object _mutex = new object();
        private static UsuarioBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private UsuarioBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase UsuarioBO
        /// </summary>
        /// <returns></returns>
        public static UsuarioBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new UsuarioBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene un usuario de la tabla Usuario y hace el merge con las propiedades
        /// del membership provider
        /// </summary>
        /// <param name="userID">Id del usuario que se desea obtener</param>
        /// <returns>Entidad con el usuario seleccionado, regresa nulo sino existe</returns>
        public Usuario Obtener(Guid userID)
        {
            Usuario usu = null;

            using (SamContext ctx = new SamContext())
            {
                usu = ctx.Usuario.Where(x => x.UserId == userID).SingleOrDefault();
            }

            MembershipUser memUsr = Membership.GetUser(userID);

            usu.IsApproved = memUsr.IsApproved;
            usu.IsLockedOut = memUsr.IsLockedOut;
            usu.Email = memUsr.Email;
            usu.EmailOriginal = memUsr.Email;
            usu.Username = memUsr.UserName;
            usu.UsernameOriginal = memUsr.UserName;

            return usu;
        }


        /// <summary>
        /// Obtiene u usuario con su relación a la tabla UsuarioProyecto
        /// </summary>
        /// <param name="userID">ID del usuario que se desea obtener</param>
        /// <returns>Una entidad de tipo usuario, regresa nulo si no existe</returns>
        public Usuario ObtenerConProyectos(Guid userID)
        {
            Usuario usu = null;

            using (SamContext ctx = new SamContext())
            {
                usu = ctx.Usuario.Include("UsuarioProyecto")
                                 .Where(x => x.UserId.Equals(userID))
                                 .SingleOrDefault();
            }

            MembershipUser memUsr = Membership.GetUser(userID);

            usu.IsApproved = memUsr.IsApproved;
            usu.IsLockedOut = memUsr.IsLockedOut;
            usu.Email = memUsr.Email;
            usu.EmailOriginal = memUsr.Email;
            usu.Username = memUsr.UserName;
            usu.UsernameOriginal = memUsr.UserName;

            return usu;
        }

        /// <summary>
        /// Obtiene un listado de todos los usuarios y su perfil asociado.
        /// </summary>
        /// <returns>Lista de entidades usuario y la propiedad UsurioPerfil llena</returns>
        public List<Usuario> ObtenerTodosConPerfil()
        {
            List<Usuario> lst = null;

            using (SamContext ctx = new SamContext())
            {
                //Para la aplicacion el usuario administrador no existe, asi que lo desaparecemos
                lst = ctx.Usuario.Include("Perfil")
                                 .ToList()
                                 .Where(x => !x.EsAdministradorSistema)
                                 .ToList();
            }

            IEnumerable<MembershipUser> memUsrs = Membership.GetAllUsers().Cast<MembershipUser>();

            lst = (from u in lst
                   join m in memUsrs on u.UserId equals m.ProviderUserKey
                   select new Usuario
                   {
                       Perfil = u.Perfil,
                       ApMaterno = u.ApMaterno,
                       ApPaterno = u.ApPaterno,
                       BloqueadoPorAdministrador = u.BloqueadoPorAdministrador,
                       Email = m.Email,
                       EmailOriginal = m.Email,
                       FechaModificacion = u.FechaModificacion,
                       Idioma = u.Idioma,
                       IsApproved = m.IsApproved,
                       IsLockedOut = m.IsLockedOut,
                       Nombre = u.Nombre,
                       PerfilID = u.PerfilID,
                       UserId = u.UserId,
                       Username = m.UserName,
                       UsernameOriginal = m.UserName,
                       UsuarioModifica = u.UsuarioModifica,
                       VersionRegistro = u.VersionRegistro
                   }).ToList();

            return lst;
        }

        /// <summary>
        /// Obtiene un listado de todos los usuarios con privilegiso dentro de un proyecto
        /// </summary>
        /// <returns>Lista de usuarios pertenecientes a un proyecto</returns>
        public List<RadUsuario> ObtenerTodosPorProyecto(int proyectoID, string nombre, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {

                List<RadUsuario> usuarios = ctx.UsuarioProyecto
                                           .Where(x => x.ProyectoID == proyectoID)
                                           .Select(x => new RadUsuario
                                           {
                                               UsuarioID = x.UserId,
                                               Nombre = x.Usuario.Nombre,
                                               ApPaterno = x.Usuario.ApPaterno,
                                               ApMaterno = x.Usuario.ApMaterno,
                                               NombreCompleto = x.Usuario.Nombre + " " + x.Usuario.ApPaterno + " " + x.Usuario.ApMaterno 
                                           })
                                           .ToList();

                return usuarios.Where(x => x.NombreCompleto.ToLowerInvariant().Contains(nombre.ToLowerInvariant()))
                               .OrderBy(x => x.NombreCompleto)
                               .Skip(skip)
                               .Take(take)
                               .ToList();
            }
        }

        /// <summary>
        /// Se encarga de guardas o editar el usuario que se esté pasando.
        /// En caso que sea nuevo usuario se da de alta en el Membership Provider.
        /// En caso de ser edición si se detecta que hubo un cambio en el correo y/o username
        /// entonces se actualizan dichos campos en la BD.
        /// </summary>
        /// <param name="usu">Entidad de usuario con los datos como deben quedar en la BD</param>
        public void Guarda(Usuario usu)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        ctx.Usuario.ApplyChanges(usu);

                        if (usu.ChangeTracker.State == ObjectState.Added)
                        {
                            CreaCuenta(usu);
                        }
                        else
                        {
                            if (!usu.EmailOriginal.Equals(usu.Email, StringComparison.InvariantCultureIgnoreCase))
                            {
                                MembershipUser mUsr = Membership.GetUser(usu.UserId);
                                mUsr.Email = usu.Email;

                                try
                                {
                                    //Si este update falla 99% probable que sea por correo duplicado
                                    //por lo cual está bien atrapar una excepción genérica, el otro 1%
                                    //es por desconexión de BD o algo más grave
                                    Membership.UpdateUser(mUsr);
                                }
                                catch
                                {
                                    throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.DuplicateEmail));
                                }
                            }

                            if (!usu.UsernameOriginal.Equals(usu.Username, StringComparison.InvariantCultureIgnoreCase))
                            {
                                List<string> errores = new List<string>();

                                if (!ValidacionesUsuario.ValidaUsernameDuplicado(usu.Username, usu.UserId, errores))
                                {
                                    throw new BaseValidationException(errores);
                                }

                                //Mandar llamar nuestro SP para cambiar el username
                                ctx.ActualizaUsername(usu.UserId, usu.Username);
                            }
                        }

                        ctx.SaveChanges();
                    }

                    scope.Complete();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// En base a una entidad de usuario procede a "activar" su cuenta.
        /// El proceso de activar cuenta consiste en establecer su contraseña inicial así como su
        /// pregunta y respuesta secreta, adicionalmente se marca el usuario como IsApproved
        /// en el membership provider.
        /// </summary>
        /// <param name="usu">Entidad que contiene el usuario que se desea activar</param>
        /// <param name="passwordNuevo">Nuevo password</param>
        /// <param name="respuestaSecreta">Respuesta secreta actual</param>
        /// <param name="nuevaPreguntaSecreta">Nueva pregunta secreta</param>
        /// <param name="nuevaRespuestaSecreta">Nueva respuesta secreta</param>
        public void ActivaCuenta(Usuario usu, string passwordNuevo, string respuestaSecreta, string nuevaPreguntaSecreta, string nuevaRespuestaSecreta)
        {
            //Lo primero que hacemos es validar el password
            if (!ValidacionesUsuario.PasswordCumpleRequisitos(passwordNuevo))
            {
                throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.InvalidPassword));
            }

            MembershipUser mUsu = Membership.GetUser(usu.UserId);

            //Usar la pregunta secreta actual del usuario para obtener su password
            string passwordActual = mUsu.GetPassword(respuestaSecreta);

            using (TransactionScope scope = new TransactionScope())
            {
                //Cambiar el password

                bool cambiado = mUsu.ChangePassword(passwordActual, passwordNuevo);

                if (!cambiado)
                {
                    throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.InvalidPassword));
                }

                cambiado = mUsu.ChangePasswordQuestionAndAnswer(passwordNuevo, nuevaPreguntaSecreta, nuevaRespuestaSecreta);

                if (!cambiado)
                {
                    throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.InvalidQuestion));
                }

                mUsu.IsApproved = true;
                Membership.UpdateUser(mUsu);

                scope.Complete();
            }
        }

        /// <summary>
        /// Da de alta una nueva cuenta en el membership provider
        /// </summary>
        /// <param name="usu">Entidad con los datos del usuario que se va a crear</param>
        private static void CreaCuenta(Usuario usu)
        {
            List<string> errores = new List<string>();

            ValidacionesUsuario.ValidaUsernameDuplicado(usu.Username, usu.UserId, errores);
            ValidacionesUsuario.ValidaCorreoDuplicadoUsuarioNuevo(usu.Email, usu.UserId, errores);

            if (errores.Count > 0)
            {
                throw new BaseValidationException(errores);
            }

            MembershipCreateStatus status;

            string passwordGenerado = GeneraPassword();

            MembershipUser mUsu =
            Membership.CreateUser(usu.Username,
                                    passwordGenerado,
                                    usu.Email,
                                    AuxiliarMembershipProvider.PREGUNTA_SECRETA_DEFAULT,
                                    AuxiliarMembershipProvider.RESPUESTA_SECRETA_DEFAULT,
                                    false,
                                    out status);

            if (status == MembershipCreateStatus.Success)
            {
                usu.UserId = (Guid)mUsu.ProviderUserKey;
                usu.BloqueadoPorAdministrador = false;
            }
            else
            {
                throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(status));
            }
        }

        /// <summary>
        /// Genera un nuevo password que cumple con los requerimientos del membership provider
        /// </summary>
        /// <returns>Password que cumple con los requsitos del membership provider</returns>
        public static string GeneraPassword()
        {
            bool passwordValido = false;
            string passwordGenerado = string.Empty;

            while (!passwordValido)
            {
                passwordGenerado = RandomPassword.Generate(6, 20);
                passwordValido = ValidacionesUsuario.PasswordCumpleRequisitos(passwordGenerado);
            }
            return passwordGenerado;
        }

        /// <summary>
        /// Crea una cuenta de "Administrador de Sistema".  Esto se puede hacer únicamente
        /// por código y existe uno solo de manera natural en el SAM. Se hace para contar con una cuenta
        /// que pueda acceder inicialmente al sistema y configurarlo.
        /// </summary>
        public void CreaAdministrador()
        {
            Usuario usu = new Usuario();
            usu.StartTracking();

            usu.ApMaterno = string.Empty;
            usu.ApPaterno = Configuracion.ApellidoDefaultAdmin;
            usu.BloqueadoPorAdministrador = false;
            usu.Email = Configuracion.CorreoDefaultAdmin;
            usu.EsAdministradorSistema = true;
            usu.Idioma = LanguageHelper.ESPANOL;
            usu.Nombre = Configuracion.NombreDefaultAdmin;
            usu.Username = Configuracion.UsernameDefaultAdmin;

            usu.StopTracking();

            using (TransactionScope scope = new TransactionScope())
            {
                Guarda(usu);

                //Como esto es a través de código el administrador se queda con la misma
                //pregunta y respuesta secreta default
                ActivaCuenta(usu,
                                Configuracion.PasswordDefaultAdmin,
                                AuxiliarMembershipProvider.RESPUESTA_SECRETA_DEFAULT,
                                AuxiliarMembershipProvider.PREGUNTA_SECRETA_DEFAULT,
                                AuxiliarMembershipProvider.RESPUESTA_SECRETA_DEFAULT);

                scope.Complete();
            }
        }


        /// <summary>
        /// Marca la cuenta como IsApproved = false y como bloqueada por el administrador
        /// </summary>
        /// <param name="usuarioADesactivar">ID del usuario a desactivar</param>
        /// <param name="usuarioModifica">ID del usuario que manda la instrucción</param>
        public void DesactivaCuenta(Guid usuarioADesactivar, Guid usuarioModifica)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    Usuario usu = ctx.Usuario.Where(u => u.UserId == usuarioADesactivar).Single();
                    usu.BloqueadoPorAdministrador = true;
                    usu.UsuarioModifica = usuarioModifica;
                    usu.FechaModificacion = DateTime.Now;

                    MembershipUser usr = Membership.GetUser(usuarioADesactivar);
                    usr.IsApproved = false;
                    Membership.UpdateUser(usr);

                    ctx.SaveChanges();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// Va al membership provider y manda el unlock para permitir que el usuario
        /// se vuelva a loggear.
        /// </summary>
        /// <param name="usuarioADesbloquear">ID del usuario que se va a desbloquear</param>
        /// <param name="usuarioModifica">ID del usuario que manda la instrucción</param>
        public void Desbloquea(Guid usuarioADesbloquear, Guid usuarioModifica)
        {
            MembershipUser user = Membership.GetUser(usuarioADesbloquear);
            user.UnlockUser();
        }

        /// <summary>
        /// Se utiliza únicamente cuando la cuenta estaba bloqueada por un administrador y la queremos
        /// volver a activar.
        /// IsApproved = 1
        /// BloqueadaPorAdministrador = 0
        /// </summary>
        /// <param name="usuarioAReactivar"></param>
        /// <param name="usuarioModifica"></param>
        public void Reactiva(Guid usuarioAReactivar, Guid usuarioModifica)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    Usuario usu = ctx.Usuario.Where(u => u.UserId == usuarioAReactivar).Single();
                    usu.BloqueadoPorAdministrador = false;
                    usu.UsuarioModifica = usuarioModifica;
                    usu.FechaModificacion = DateTime.Now;

                    MembershipUser usr = Membership.GetUser(usuarioAReactivar);
                    usr.IsApproved = true;
                    Membership.UpdateUser(usr);

                    ctx.SaveChanges();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// Cambia la contraseña del usuario.
        /// La nueva contraseña debe ser válida, de lo contrario se arroja una excepción.
        /// La contraseña anterior debe coincidir, si falla tres veces la cuenta del usuario se bloquea.
        /// </summary>
        /// <param name="usuarioACambiar">ID del usuario al que se le cambiara el password</param>
        /// <param name="passwordActual">Password actual del usuarion</param>
        /// <param name="passwordNuevo">Nuevo password deseado</param>
        /// <param name="usuarioModifica">ID del usuario que manda la instrucción</param>
        public void CambiarContraseña(Guid usuarioACambiar, string passwordActual, string passwordNuevo, Guid usuarioModifica)
        {
            if (!ValidacionesUsuario.PasswordCumpleRequisitos(passwordNuevo))
            {
                throw new BaseValidationException(AuxiliarMembershipProvider.ErrorAmigable(MembershipCreateStatus.InvalidPassword));
            }

            MembershipUser user = Membership.GetUser(usuarioACambiar);

            bool resultado = user.ChangePassword(passwordActual, passwordNuevo);

            if (!resultado)
            {
                user = Membership.GetUser(usuarioACambiar);

                if (user.IsLockedOut)
                {
                    throw new UsuarioBloqueadoException();
                }

                throw new BaseValidationException(MensajesError.Usuario_NoPuedeCambiarPassword);
            }
        }

        /// <summary>
        /// Cambia la pregunta y respuesta secreta de un usuario.
        /// Si el password provisto no concuerda por tercera ocasión la cuenta se bloquea y el método
        /// arroja la excepción correspondiente.
        /// </summary>
        /// <param name="usuarioACambiar">ID del usuario al que se le cambiará su pregunta/respuesta secreta</param>
        /// <param name="preguntaSecreta">Nueva pregunta secreta</param>
        /// <param name="respuestaSecreta">Nueva respuesta secreta</param>
        /// <param name="password">Password actual del usuario</param>
        /// <param name="usuarioModifica">ID del usuario que manda la instrucción</param>
        public void CambiaPreguntaRespuestaSecreta(Guid usuarioACambiar, string preguntaSecreta, string respuestaSecreta, string password, Guid usuarioModifica)
        {
            MembershipUser user = Membership.GetUser(usuarioACambiar);

            bool resultado = user.ChangePasswordQuestionAndAnswer(password, preguntaSecreta, respuestaSecreta);

            if (!resultado)
            {
                user = Membership.GetUser(usuarioACambiar);

                if (user.IsLockedOut)
                {
                    throw new UsuarioBloqueadoException();
                }

                throw new BaseValidationException(MensajesError.Usuario_NoPuedeCambiarPregunta);
            }
        }

        public bool ObtenerPermisosEdicionesEspeciales(int? perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (perfilID.HasValue)
                {
                    bool tienePermiso = (from perfilPermiso in ctx.PerfilPermiso
                                         join permisos in ctx.Permiso on perfilPermiso.PermisoID equals permisos.PermisoID
                                         join perfiles in ctx.Perfil on perfilPermiso.PerfilID equals perfiles.PerfilID
                                         where permisos.Nombre == "Ediciones Especiales"
                                         && perfiles.PerfilID == perfilID
                                         select perfilPermiso.PerfilPermisoID).Any();
                    return tienePermiso;
                }
                else
                {
                    return false;
                }
            }
        }



        public bool ObtenerPermisoDetalleCortadores(int? perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (perfilID.HasValue)
                {

                    bool tienePermiso = (from perfilpermiso in ctx.PerfilPermiso
                                         join permisos in ctx.Permiso on perfilpermiso.PermisoID equals permisos.PermisoID
                                         join perfiles in ctx.Perfil on perfilpermiso.PerfilID equals perfiles.PerfilID
                                         where permisos.Nombre == "Detalle de Cortadores"
                                         && perfiles.PerfilID == perfilID
                                         select perfilpermiso.PerfilPermisoID).Any();

                    return tienePermiso;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool ObtenerPermisosEdicionesLimitadaTubero(int? perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (perfilID.HasValue)
                {
                    bool tienePermiso = (from perfilPermiso in ctx.PerfilPermiso
                                         join permisos in ctx.Permiso on perfilPermiso.PermisoID equals permisos.PermisoID
                                         join perfiles in ctx.Perfil on perfilPermiso.PerfilID equals perfiles.PerfilID
                                         where permisos.Nombre == "Edición limitada Área de Trabajo - Tuberos"
                                         && perfiles.PerfilID == perfilID
                                         select perfilPermiso.PerfilPermisoID).Any();
                    return tienePermiso;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool ObtenerPermisosEdicionesLimitadaSoldador(int? perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (perfilID.HasValue)
                {

                    bool tienePermiso = (from perfilPermiso in ctx.PerfilPermiso
                                         join permisos in ctx.Permiso on perfilPermiso.PermisoID equals permisos.PermisoID
                                         join perfiles in ctx.Perfil on perfilPermiso.PerfilID equals perfiles.PerfilID
                                         where permisos.Nombre == "Edición limitada Área de Trabajo - Soldadores"
                                         && perfiles.PerfilID == perfilID
                                         select perfilPermiso.PerfilPermisoID).Any();

                    return tienePermiso;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ObtenerPermisoDetalleDespachadores(int? perfilID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (perfilID.HasValue)
                {

                    bool tienePermiso = (from perfilpermiso in ctx.PerfilPermiso
                                         join permisos in ctx.Permiso on perfilpermiso.PermisoID equals permisos.PermisoID
                                         join perfiles in ctx.Perfil on perfilpermiso.PerfilID equals perfiles.PerfilID
                                         where permisos.Nombre == "Detalle de Despachadores"
                                         && perfiles.PerfilID == perfilID
                                         select perfilpermiso.PerfilPermisoID).Any();

                    return tienePermiso;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}