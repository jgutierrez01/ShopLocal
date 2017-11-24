using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Mail;
using Mimo.Framework.Common;
using SAM.Common;
using Mimo.Framework.Cryptography;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Proyectos;
using log4net;

namespace SAM.BusinessLogic.Utilerias
{
    public class EnvioCorreos
    {
        private static readonly object _mutex = new object();
        private static EnvioCorreos _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EnvioCorreos));
        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private EnvioCorreos()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase EnvioCorreos
        /// </summary>
        /// <returns></returns>
        public static EnvioCorreos Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EnvioCorreos();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Envía correos a los usuarios que esten configurados en la pantalla de configuracion
        /// </summary>
        /// <param name="usuarioID">ID del usuario al que se enviará el correo</param>
        /// 
        public void EnviaCorreoDatosNoEncontradosPeqKgtEsp(Guid usuarioID,string fileName, int proyectoid)
        {
            string Correos = string.Empty;
            Usuario usu = UsuarioBO.Instance.Obtener(usuarioID);
            Proyecto proyecto = ProyectoBO.Instance.ObtenerProyectoConfiguracion(proyectoid);

            string cuerpoCorreo = GeneradorCorreos.ObtenerCorreoDatosNoEncontradosPeqKgtEsp(usu, proyecto.Nombre);
            Email correo = inicializaEmail(cuerpoCorreo);
            correo.To = proyecto.ProyectoConfiguracion.CorreoPeqKgEsp;
            _logger.DebugFormat("correo.To {0}", proyecto.ProyectoConfiguracion.CorreoPeqKgEsp);

            if (!string.IsNullOrEmpty(correo.To))
            {
                correo.Subject = usu.Idioma == LanguageHelper.INGLES ? "SAM Control, Data not found PEQs, kg's, Thicknesses" : "SAM Control, Datos no encontrados Peqs, kg's, Espesores";
                correo.fileName = fileName;
                correo.SendWithAttachmentFileNameAsync();
            }
            else {
                _logger.DebugFormat("Favor de proporcionar algun correo, Datos No Encontrados Peq, Kgt, Esp");
            }            
        }

        /// <summary>
        /// Envia el correo de activación al usuario.  El correo contiene una liga a la que el usuario
        /// da click para activar su cuenta.
        /// </summary>
        /// <param name="usu">Usuario al cual se le enviará el correo</param>
        public void EnviaCorreoActivacion(Usuario usu)
        {
            string usernameEncriptado = Crypter.EncryptAndEncode(usu.Username);
            string urlActivacion = string.Format("{0}?UID={1}", Configuracion.UrlActivacion, usernameEncriptado);
            string cuerpoCorreo = GeneradorCorreos.ObtenCorreoDeActivacion(usu, urlActivacion);

            Email correo = inicializaEmail(cuerpoCorreo);
            correo.To = usu.Email;
            correo.Subject = usu.Idioma == LanguageHelper.INGLES ? "SAM Control Activation" : "Activación de cuenta SAM Control";
            correo.SendAsync();
        }

        /// <summary>
        /// Envía al usuario un correo con su nombre de usuario y contraseña de tal forma que
        /// pueda volver a entrar al sistema.
        /// </summary>
        /// <param name="usu">Usuario en cuestión</param>
        /// <param name="passwordActual">Contraseña del usuario</param>
        public void EnviaCorreoOlvidoPassword(Usuario usu, string passwordActual)
        {
            string cuerpoCorreo = GeneradorCorreos.ObtenCorreoOlvidoPassword(usu, passwordActual);

            Email correo = inicializaEmail(cuerpoCorreo);
            correo.To = usu.Email;
            correo.Subject = usu.Idioma == LanguageHelper.INGLES ? "SAM Control forgot password" : "Olivo de contraseña SAM Control";
            correo.SendAsync();
        }

        /// <summary>
        /// Envía al usuario un correo indicándole que un administrador cambió su contraseña.
        /// El correo contiene el userna, y password.
        /// </summary>
        /// <param name="usu">Usuario en cuestión</param>
        /// <param name="nuevoPassword">Nueva contraseña establecida por el administrador</param>
        public void EnviaCorreoReinicioPassword(Usuario usu, string nuevoPassword)
        {
            string cuerpoCorreo = GeneradorCorreos.ObtenCorreoPasswordReiniciado(usu, nuevoPassword);

            Email correo = inicializaEmail(cuerpoCorreo);
            correo.To = usu.Email;
            correo.Subject = usu.Idioma == LanguageHelper.INGLES ? "SAM Control password reset" : "Reinicio de contraseña SAM Control";
            correo.SendAsync();        
        }

        /// <summary>
        /// Envía un correo al usuario indicandole que se ha asignado un pendiente
        /// </summary>
        /// <param name="usuarioID">ID del usuario al que se enviará el correo</param>
        public void EnviaNotificacionDePendientes(Guid usuarioID, string proyecto, string pendiente, string detalle)
        {
            Usuario usu = UsuarioBO.Instance.Obtener(usuarioID);
            string cuerpoCorreo = GeneradorCorreos.ObtenCorreoPendientes(usu, proyecto, pendiente, detalle);

            Email correo = inicializaEmail(cuerpoCorreo);
            correo.To = usu.Email;
            correo.Subject = usu.Idioma == LanguageHelper.INGLES ? "SAM Control task assigned" : "Pendiente Asignado en SAM Control";
            correo.SendAsync();    
        }
        
        /// <summary>
        /// Se encarga de generar los parámetros comunes para el envío de correos.
        /// </summary>
        /// <param name="cuerpoCorreo">Cuerpo del correo</param>
        /// <returns>Objeto correo inicializado con los parámetros default</returns>
        private static Email inicializaEmail(string cuerpoCorreo)
        {
            
            Email email = new Email();
            email.Body = cuerpoCorreo;
            email.From = Configuracion.CuentaCorreo;
            email.Login = Configuracion.UsuarioCorreo;
            email.Password = Configuracion.PasswordCorreo;
            email.SmtpPort = Configuracion.PuertoCorreo;
            email.SmptHost = Configuracion.HostCorreo;
            
            return email;
        }
    }
}
