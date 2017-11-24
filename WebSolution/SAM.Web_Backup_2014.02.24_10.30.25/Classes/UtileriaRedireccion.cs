using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using SAM.Common;

namespace SAM.Web.Classes
{
    public static class UtileriaRedireccion
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UtileriaRedireccion));

        /// <summary>
        /// Hace un Server.Transfer a la página donde se muestran mensajes
        /// de éxito de la carpeta de administración.
        /// </summary>
        /// <param name="tituloMensaje">Título para el mensaje</param>
        /// <param name="cuerpoMensaje">Cuerpo para el mensaje, es válido que tenga tags de HTML</param>
        /// <param name="ligas">Listado de ligas a las cuales tiene opción el usuario de ir después del proceso</param>
        public static void RedireccionaExitoAdmin(string tituloMensaje, string cuerpoMensaje, List<LigaMensaje> ligas)
        {
            MensajeExitoUI mensaje = new MensajeExitoUI
                                     {
                                         Titulo = tituloMensaje, 
                                         CuerpoMensaje = cuerpoMensaje, 
                                         Ligas = ligas
                                     };

            HttpContext.Current.Items[WebConstants.Contexto.CTX_MENSAJE] = mensaje;
            HttpContext.Current.Server.Transfer(WebConstants.AdminUrl.ADMIN_MENSAJE_EXITO);
        }


        /// <summary>
        /// Hace un Server.Transfer a la página donde se muestran mensajes
        /// de éxito de la carpeta de producción.
        /// </summary>
        /// <param name="tituloMensaje">Título para el mensaje</param>
        /// <param name="cuerpoMensaje">Cuerpo para el mensaje, es válido que tenga tags de HTML</param>
        /// <param name="ligas">Listado de ligas a las cuales tiene opción el usuario de ir después del proceso</param>
        public static void RedireccionaExitoProduccion(string tituloMensaje, string cuerpoMensaje, List<LigaMensaje> ligas)
        {
            MensajeExitoUI mensaje = new MensajeExitoUI
            {
                Titulo = tituloMensaje,
                CuerpoMensaje = cuerpoMensaje,
                Ligas = ligas
            };

            HttpContext.Current.Items[WebConstants.Contexto.CTX_MENSAJE] = mensaje;
            HttpContext.Current.Server.Transfer(WebConstants.ProduccionUrl.MensajeExito);
        }

        /// <summary>
        /// Hace un Server.Transfer a la página donde se muestran mensajes
        /// de éxito de la carpeta de ingeniería.
        /// </summary>
        /// <param name="tituloMensaje">Título para el mensaje</param>
        /// <param name="cuerpoMensaje">Cuerpo para el mensaje, es válido que tenga tags de HTML</param>
        /// <param name="ligas">Listado de ligas a las cuales tiene opción el usuario de ir después del proceso</param>
        public static void RedireccionaExitoIngenieria(string tituloMensaje, string cuerpoMensaje, List<LigaMensaje> ligas)
        {
            MensajeExitoUI mensaje = new MensajeExitoUI
            {
                Titulo = tituloMensaje,
                CuerpoMensaje = cuerpoMensaje,
                Ligas = ligas
            };

            HttpContext.Current.Items[WebConstants.Contexto.CTX_MENSAJE] = mensaje;
            HttpContext.Current.Server.Transfer(WebConstants.IngenieriaUrl.MENSAJE_EXITO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mensaje"></param>
        public static void EnviaPaginaDeAccesoNoAutorizado(string mensaje)
        {
            _logger.Error(mensaje);
            HttpContext.Current.Response.Redirect(string.Format("/Errores/Error403.aspx?aspxerrorpath={0}", HttpContext.Current.Request.Url.PathAndQuery), true);
        }


        public static void PushSSL(bool requiresSsl, HttpContext context)
        {
            const string SECURE = "https://";
            const string UNSECURE = "http://";
            string httpUrl = "";

            if (Configuracion.SSLEnabled)
            {
                //Force required into secure channel
                if (requiresSsl && !context.Request.IsSecureConnection)
                {
                    httpUrl = SECURE + context.Request.Url.Host + context.Request.RawUrl;
                    context.Response.Redirect(httpUrl);
                }

                //Force non-required out of secure channel
                if (!requiresSsl && context.Request.IsSecureConnection)
                {
                    httpUrl = UNSECURE + context.Request.Url.Host + context.Request.RawUrl;
                    context.Response.Redirect(httpUrl);
                }
            }
        }
    }
}